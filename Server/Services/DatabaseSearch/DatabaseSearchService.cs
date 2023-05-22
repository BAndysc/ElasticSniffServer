using LinqKit;
using Microsoft.EntityFrameworkCore;
using SearchSniffServer.Models;
using Server.Database;
using Server.Database.Models;
using Server.Database.Models.Index;
using Server.Models.SearchRequests;
using Z.EntityFramework.Plus;

namespace Server.Services.DatabaseSearch;

public class DatabaseSearchService : ISearchService
{
    private readonly DatabaseContext databaseContext;
    private readonly IServiceScope scope;

    public DatabaseSearchService(IServiceProvider serviceProvider)
    {
        scope = serviceProvider.CreateScope();
        databaseContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    }

    public async Task<bool> ContainsMD5(string md5)
    {
        var count = await databaseContext.SniffsIndex.CountAsync(x => x.MD5 == md5);
        return count > 0;
    }
    
    public async Task<SearchResults> Search(ISniffSearchRequest request, int start, int count)
    {
        List<SniffModel> sniffs = new();
        foreach (var alt in request.Alternatives)
        {
            var typedConditions = alt.Conditions.Select(c => ((c as NumericValueCondition), (c as SearchBuildCondition))).ToList();
            var numericConditions = typedConditions.Where(c => c.Item1 != null).Select(c => c.Item1).ToList();
            var searchBuildConditions = typedConditions.Where(c => c.Item2 != null).Select(c => c.Item2).ToList();

            List<string> sniffCandidates = new();
            if (numericConditions.Count > 0)
            {
                var predicate = PredicateBuilder.New<SniffNumberFieldModel>();
                foreach (var cond in numericConditions)
                {
                    if (cond == null)
                        continue;
                    var field = (DatabaseNumberField)(int)cond!.Field;
                    predicate = predicate.Or(x => x.Field == field && x.Value == (long)cond!.Value);
                }

                var preCandidates = await databaseContext.SniffNumbers.Where(predicate).ToListAsync();

                foreach (var groups in preCandidates.GroupBy(x => x.SniffModelId))
                {
                    var set = groups.Select(x => ((uint)x.Field, x.Value)).ToHashSet();
                    bool ok = true;
                    foreach (var cond in numericConditions)
                    {
                        if (cond == null)
                            continue;
                        if (!set.Contains(((uint)cond.Field, cond.Value)))
                        {
                            ok = false;
                            break;
                        }
                    }

                    if (ok)
                        sniffCandidates.Add(groups.Key);
                }   
            }

            if (searchBuildConditions.Count == 0)
            {
                if (sniffCandidates.Count > 0)
                    sniffs.AddRange(await databaseContext.SniffsIndex.Where(i => sniffCandidates.Contains(i.MD5)).ToListAsync());
            }
            else
            {
                var predicate = PredicateBuilder.New<SniffModel>();
                if (sniffCandidates.Count > 0)
                {
                    predicate = predicate.And(s => sniffCandidates.Contains(s.MD5));
                }

                foreach (var cond in searchBuildConditions)
                {
                    if (cond == null)
                        continue;

                    switch (cond.Operator)
                    {
                        case ComparisonOperator.Equals:
                            predicate = predicate.And(p => p.BuildVersion == (uint)cond.Build);
                            break;
                        case ComparisonOperator.GreaterThan:
                            predicate = predicate.And(p => p.BuildVersion > (uint)cond.Build);
                            break;
                        case ComparisonOperator.GreaterThanOrEqual:
                            predicate = predicate.And(p => p.BuildVersion >= (uint)cond.Build);
                            break;
                        case ComparisonOperator.LessThan:
                            predicate = predicate.And(p => p.BuildVersion < (uint)cond.Build);
                            break;
                        case ComparisonOperator.LessThanOrEqual:
                            predicate = predicate.And(p => p.BuildVersion <= (uint)cond.Build);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                
                sniffs.AddRange(await databaseContext.SniffsIndex.Where(predicate).ToListAsync());
            }
        }

        return new SearchResults(sniffs.Select(s => new AbstractSniff()
        {
            Path = s.Path,
            PathInArchive = s.PathInArchive,
            MD5 = s.MD5,
            Source = s.Source,
            StartTime = s.StartTime,
            EndTime = s.EndTime,
            IndexedOn = s.IndexedOn,
            FileSize = s.FileSize,
            BuildVersion = s.BuildVersion
        }).ToList(), sniffs.Count);
    }
}

public class DatabaseUploadService : IUploadService
{
    private readonly DatabaseContext databaseContext;
    private readonly IServiceScope scope;

    public DatabaseUploadService(IServiceProvider serviceProvider)
    {
        scope = serviceProvider.CreateScope();
        databaseContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    }

    public async Task<bool> Upload(UserModel uploader, ISniff sniff)
    {
        await using var transaction = await databaseContext.Database.BeginTransactionAsync();

        await databaseContext.SniffsIndex.Where(x => x.MD5 == sniff.MD5).DeleteAsync();

        var model = new SniffModel()
        {
            MD5 = sniff.MD5,
            Source = sniff.Source,
            BuildVersion = (uint)sniff.BuildVersion,
            IndexedOn = sniff.IndexedOn,
            StartTime = sniff.StartTime,
            EndTime = sniff.EndTime,
            FileSize = sniff.FileSize,
            Path = sniff.Path,
            Uploader = uploader.User
        };

        await databaseContext.SniffsIndex.AddAsync(model);

        List<SniffNumberFieldModel> numbers = new();
        List<SniffTextFieldModel> texts = new();
        
        foreach (var f in sniff.Creatures)
            numbers.Add(new (){SniffModel = model, Field = DatabaseNumberField.Creature, Value = f});
        
        foreach (var f in sniff.Emotes)
            numbers.Add(new (){SniffModel = model, Field = DatabaseNumberField.Emote, Value = f});

        foreach (var f in sniff.Gossips)
            numbers.Add(new (){SniffModel = model, Field = DatabaseNumberField.Gossip, Value = f});

        foreach (var f in sniff.Maps)
            numbers.Add(new (){SniffModel = model, Field = DatabaseNumberField.Map, Value = f});

        foreach (var f in sniff.Phases)
            numbers.Add(new (){SniffModel = model, Field = DatabaseNumberField.Phase, Value = f});

        foreach (var f in sniff.Quests)
            numbers.Add(new (){SniffModel = model, Field = DatabaseNumberField.Quest, Value = f});

        foreach (var f in sniff.Sounds)
            numbers.Add(new (){SniffModel = model, Field = DatabaseNumberField.Sound, Value = f});

        foreach (var f in sniff.Spells)
            numbers.Add(new (){SniffModel = model, Field = DatabaseNumberField.Spell, Value = f});
        
        foreach (var f in sniff.AreaTriggers)
            numbers.Add(new (){SniffModel = model, Field = DatabaseNumberField.AreaTrigger, Value = f});

        foreach (var f in sniff.GameObjects)
            numbers.Add(new (){SniffModel = model, Field = DatabaseNumberField.GameObject, Value = f});

        foreach (var f in sniff.Texts)
            texts.Add(new (){SniffModel = model, Field = DatabaseTextField.Chat, Text = f});

        await databaseContext.SniffNumbers.AddRangeAsync(numbers);

        await databaseContext.SniffTexts.AddRangeAsync(texts);
        
        await databaseContext.SaveChangesAsync();
        await transaction.CommitAsync();
        return true;
    }
}
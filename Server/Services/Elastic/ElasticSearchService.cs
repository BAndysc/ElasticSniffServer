using Nest;
using SearchSniffServer.Models;
using Server.Models.SearchRequests;

namespace Server.Services.Elastic;

public class ElasticQueryBuilder : ISearchConditionVisitor<QueryContainer>
{
    private readonly QueryContainerDescriptor<SniffElasticDocument> query;

    public ElasticQueryBuilder(QueryContainerDescriptor<SniffElasticDocument> query)
    {
        this.query = query;
    }
    
    public QueryContainer Visit(SearchBuildCondition condition)
    {
        switch (condition.Operator)
        {
            case ComparisonOperator.Equals:
                return query.Term(r => r
                    .Field(d => d.BuildVersion)
                    .Value(condition.Build));
            case ComparisonOperator.GreaterThan:
                return query.Range(r => r
                    .Field(d => d.BuildVersion)
                    .GreaterThan(condition.Build));
            case ComparisonOperator.GreaterThanOrEqual:
                return query.Range(r => r
                    .Field(d => d.BuildVersion)
                    .GreaterThanOrEquals(condition.Build));
            case ComparisonOperator.LessThan:
                return query.Range(r => r
                    .Field(d => d.BuildVersion)
                    .LessThan(condition.Build));
            case ComparisonOperator.LessThanOrEqual:
                return query.Range(r => r
                    .Field(d => d.BuildVersion)
                    .LessThanOrEquals(condition.Build));
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public QueryContainer Visit(NumericValueCondition condition)
    {
        switch (condition.Field)
        {
            case NumericField.Spell:
                return query.Term(r => r
                    .Field(d => d.spells)
                    .Value(condition.Value));
            case NumericField.GameObject:
                return query.Term(r => r
                    .Field(d => d.gameObjects)
                    .Value(condition.Value));
            case NumericField.Creature:
                return query.Term(r => r
                    .Field(d => d.creatures)
                    .Value(condition.Value));
            case NumericField.AreaTrigger:
                return query.Term(r => r
                    .Field(d => d.areaTriggers)
                    .Value(condition.Value));
            case NumericField.Phase:
                return query.Term(r => r
                    .Field(d => d.phases)
                    .Value(condition.Value));
            case NumericField.Gossip:
                return query.Term(r => r
                    .Field(d => d.gossips)
                    .Value(condition.Value));
            case NumericField.Sound:
                return query.Term(r => r
                    .Field(d => d.sounds)
                    .Value(condition.Value));
            case NumericField.Emote:
                return query.Term(r => r
                    .Field(d => d.emotes)
                    .Value(condition.Value));
            case NumericField.Quest:
                return query.Term(r => r
                    .Field(d => d.quests)
                    .Value(condition.Value));
            case NumericField.Map:
                return query.Term(r => r
                    .Field(d => d.maps)
                    .Value(condition.Value));
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}

public class ElasticSearchService : ISearchService
{
    private readonly ElasticClient client;

    public ElasticSearchService(IElasticFactory factory)
    {
        client = factory.Factory();
    }

    public async Task<SearchResults> Search(ISniffSearchRequest request, int start, int count)
    {
        var searchResponse = await client.SearchAsync<SniffElasticDocument>(s => s
            .From(start)
            .Size(count)
            .Query(q =>
            {
                QueryContainer? masterOr = null;
                foreach (var or in request.Alternatives)
                {
                    QueryContainer? subAnd = null;
                    foreach (var and in or.Conditions)
                    {
                        var result = and.Accept(new ElasticQueryBuilder(q));
                        if (subAnd == null)
                            subAnd = result;
                        else
                            subAnd = subAnd && result;
                    }

                    if (masterOr == null)
                        masterOr = subAnd;
                    else
                        masterOr = masterOr || subAnd;
                }

                return masterOr;
            }));

        return new SearchResults(searchResponse.Documents.ToList(), searchResponse.Total);
    }
}
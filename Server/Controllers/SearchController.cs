using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SearchSniffServer.Models;
using Server.Database;
using Server.Models;
using Server.Models.SearchRequests;
using Server.Services;
using Server.Services.DatabaseSearch;
using Server.Services.Elastic;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchController : Controller
    {
        private readonly IDatabaseRepository databaseRepository;
        private readonly ElasticSearchService elasticSearchService;
        private readonly DatabaseSearchService databaseSearchService;
        private readonly ISearchService searchService;

        public SearchController(IUserService userService, 
            IDatabaseRepository databaseRepository,
            ElasticSearchService elasticSearchService,
            DatabaseSearchService databaseSearchService,
            ISearchService searchService) : base(userService)
        {
            this.databaseRepository = databaseRepository;
            this.elasticSearchService = elasticSearchService;
            this.databaseSearchService = databaseSearchService;
            this.searchService = searchService;
        }
        
        [HttpPost(Name = "Search")]
        public async Task<IActionResult> Get(RequestSniffSearch request)
        {
            if (!await IsAuthorized())
                return BadRequest("Not authorized");

            var searchRequest = new SniffSearchRequest(request.Alternatives.Select(and =>
            {
                return new SearchRequestTerm(and.Select(term =>
                {
                    switch (term.Type)
                    {
                        case TermType.build:
                            return (ISearchCondition)new SearchBuildCondition(term.NumericValue, (ComparisonOperator)term.Eq);
                        case TermType.numeric:
                            NumericField field;
                            switch (term.Field)
                            {
                                case "spell":
                                    field = NumericField.Spell;
                                    break;
                                case "gameObject":
                                    field = NumericField.GameObject;
                                    break;
                                case "creature":
                                    field = NumericField.Creature;
                                    break;
                                case "areaTrigger":
                                    field = NumericField.AreaTrigger;
                                    break;
                                case "phase":
                                    field = NumericField.Phase;
                                    break;
                                case "gossip":
                                    field = NumericField.Gossip;
                                    break;
                                case "sound":
                                    field = NumericField.Sound;
                                    break;
                                case "emote":
                                    field = NumericField.Emote;
                                    break;
                                case "quest":
                                    field = NumericField.Quest;
                                    break;
                                case "map":
                                    field = NumericField.Map;
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException("Field " + term.Field);
                            }
                            return new NumericValueCondition(term.NumericValue, field);
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }).ToArray());
            }).ToArray<ISearchRequestTerm>());

            Stopwatch timer = new();
            timer.Start();
            var elasticResults = await elasticSearchService.Search(searchRequest, request.Start, 10000);// Math.Min(request.Count, 100));
            var elastic = timer.Elapsed;
            
            timer.Restart();
            var databaseResults = await databaseSearchService.Search(searchRequest, request.Start, Math.Min(request.Count, 100));
            var db = timer.Elapsed;
            timer.Stop();

            GetHeaderUser(out var user);
            var log = JsonConvert.SerializeObject(request);
            await databaseRepository.Log(Request, $"User: {user}. Elastic: {elastic} Database: {db} Request: {log}");
            
            return Ok(new SniffSearchResponse(elasticResults.Items.Select(r => new SniffModelResponse()
            {
                Path   = r.Path,
                PathInArchive = r.PathInArchive,
                MD5 = r.MD5,
                GameBuild = r.BuildVersion,
                SniffTime = r.StartTime,
                Source = r.Source,
                FileSize = r.FileSize
            }).ToList(), (int)elasticResults.Total));
        }
    }
}
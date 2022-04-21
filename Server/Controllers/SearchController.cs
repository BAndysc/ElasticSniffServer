using Microsoft.AspNetCore.Mvc;
using SearchSniffServer.Models;
using Server.Models;
using Server.Models.SearchRequests;
using Server.Services;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchController : Controller
    {
        private readonly ISearchService searchService;

        public SearchController(IUserService userService, ISearchService searchService) : base(userService)
        {
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
            
            var results = await searchService.Search(searchRequest, request.Start, Math.Min(request.Count, 100));
            
            return Ok(new SniffSearchResponse(results.Items.Select(r => new SniffModelResponse()
            {
                Path   = r.Path,
                PathInArchive = r.PathInArchive,
                MD5 = r.MD5,
                GameBuild = r.BuildVersion,
                SniffTime = r.StartTime,
                Source = r.Source,
                FileSize = r.FileSize
            }).ToList()));
        }
    }
}
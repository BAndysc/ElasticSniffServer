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
    public class ContainsController : Controller
    {
        private readonly IDatabaseRepository databaseRepository;
        private readonly ElasticSearchService elasticSearchService;
        private readonly DatabaseSearchService databaseSearchService;
        private readonly ISearchService searchService;

        public ContainsController(IUserService userService, 
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
        
        [HttpPost(Name = "Contains")]
        public async Task<IActionResult> Contains(SniffContainsRequest request)
        {
            if (!await IsAdminAuthorized())
                return BadRequest("Not authorized");

            var containsSniff = await databaseSearchService.ContainsMD5(request.MD5);
            return Ok(new SniffContainsResponse(containsSniff));
        }
    }
}
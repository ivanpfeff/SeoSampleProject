using Microsoft.AspNetCore.Mvc;
using SeoSampleApp.Entities;
using SeoSampleApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeoSampleApp
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;
        
        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        // POST api/<SearchController>
        [HttpPost]
        public async Task<SearchResult> Post([FromBody] SearchRequest searchRequest)
        {
            var searchResult = await _searchService.ProcessSearch(searchRequest);
            return searchResult;
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using SeoSampleApp.Entities;
using SeoSampleApp.Services.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeoSampleApp
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchHistoryController : ControllerBase
    {
        private readonly ISearchHistoryService _searchHistoryService;

        public SearchHistoryController(ISearchHistoryService searchHistoryService)
        {
            _searchHistoryService = searchHistoryService;
        }

        [HttpGet]
        public IEnumerable<SearchResult> Get()
        {
            return _searchHistoryService.LoadAll().OrderByDescending(x => x.Date);
        }
    }
}

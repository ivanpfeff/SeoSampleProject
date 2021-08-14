using SeoSampleApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeoSampleApp.Services
{
    public class SearchService : ISearchService
    {
        private readonly IHttpWrapperService _httpWrapperService;

        public SearchService(IHttpWrapperService httpWrapperService)
        {
            _httpWrapperService = httpWrapperService;
        }

        public SearchResult ProcessSearch(SearchRequest searchRequest)
        {
            throw new NotImplementedException();
        }
    }
}

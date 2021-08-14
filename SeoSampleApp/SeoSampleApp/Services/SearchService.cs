using SeoSampleApp.Configuration;
using SeoSampleApp.Entities;
using SeoSampleApp.Services.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SeoSampleApp.Services
{
    public class SearchService : ISearchService
    {
        private readonly SearchConfiguration _searchConfiguration;
        private readonly IHttpWrapperService _httpWrapperService;
        private readonly ISearchHistoryService _searchHistoryService;

        public SearchService(SearchConfiguration searchConfiguration, IHttpWrapperService httpWrapperService, ISearchHistoryService searchHistoryService)
        {
            _searchConfiguration = searchConfiguration;
            _httpWrapperService = httpWrapperService;
            _searchHistoryService = searchHistoryService;
        }

        public async Task<SearchResult> ProcessSearch(SearchRequest searchRequest)
        {
            if(searchRequest == null)
            {
                throw new Exception("Search request is null");
            }

            if (string.IsNullOrEmpty(searchRequest.SEOTerm))
            {
                throw new Exception("SEO term must be provided");
            }

            if (searchRequest.UseGoogle && string.IsNullOrEmpty(searchRequest.SearchTerm))
            {
                throw new Exception("Search term must be provided");
            }

            if (!searchRequest.UseGoogle && string.IsNullOrEmpty(searchRequest.SearchURL))
            {
                throw new Exception("A search URL must be provided when not using Google");
            }

            var searchUrl = searchRequest.UseGoogle 
                ? Regex.Replace(_searchConfiguration.GoogleSearchFormat, "SEARCH_TERM", searchRequest.SearchTerm) 
                : searchRequest.SearchURL;

            var response = await _httpWrapperService.ExecuteGETRequest(searchUrl);
            
            var responseCaseInvariant = response.ToLowerInvariant();
            var seoTermInvariant = searchRequest.SEOTerm.ToLowerInvariant();

            var count = 0;
            var idx = responseCaseInvariant.IndexOf(seoTermInvariant, 0);
            while(idx != -1)
            {
                count++;
                idx = responseCaseInvariant.IndexOf(seoTermInvariant, idx + 1);
            }

            var result = new SearchResult()
            {
                Hits = count,
                Body = response,
                SEOTerm = seoTermInvariant,
                URL = searchUrl
            };

            try
            {
                _searchHistoryService.Save(result);
            }
            catch(Exception ex)
            {
                //TODO: logging
            }

            return result;
        }
    }
}

using Newtonsoft.Json;
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

            if (string.IsNullOrEmpty(searchRequest.SearchTerm))
            {
                throw new Exception("Search term must be provided");
            }


            var searchUrl = Regex.Replace(_searchConfiguration.GoogleSearchFormat, "SEARCH_TERM", searchRequest.SearchTerm);
            var start = 0;
            var startSuffix = "&start=";
            var foundAt = -1;

            while(foundAt == -1 && start < 100)
            {
                var query = $"{searchUrl}{startSuffix}{start}";
                var responseJson = await _httpWrapperService.ExecuteGETRequest(query);
                var searchResult = JsonConvert.DeserializeObject<GoogleSearchResult>(responseJson);
                for(var i = 0; i < searchResult.Items.Length; i++)
                {
                    var item = searchResult.Items[i];
                    var link = item.Link.ToLowerInvariant();
                    var displayLink = item.DisplayLink.ToLowerInvariant();

                    if(link.Contains(searchRequest.URL) || displayLink.Contains(searchRequest.URL))
                    {
                        foundAt = start + i + 1;
                    }
                }

                start += 10;
            }

            var result = new SearchResult()
            {
                SearchID = Guid.NewGuid(),
                Date = DateTime.UtcNow,
                SearchTerm = searchRequest.SearchTerm,
                URL = searchUrl,
                Score = foundAt,
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

using SeoSampleApp.Entities;

namespace SeoSampleApp.Services
{
    public interface ISearchService
    {
        SearchResult ProcessSearch(SearchRequest searchRequest);
    }
}
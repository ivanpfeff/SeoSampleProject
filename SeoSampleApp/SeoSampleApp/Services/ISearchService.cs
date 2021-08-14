using SeoSampleApp.Entities;
using System.Threading.Tasks;

namespace SeoSampleApp.Services
{
    public interface ISearchService
    {
        Task<SearchResult> ProcessSearch(SearchRequest searchRequest);
    }
}
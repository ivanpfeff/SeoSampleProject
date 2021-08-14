using SeoSampleApp.Entities;
using System.Collections.Generic;

namespace SeoSampleApp.Services.DataLayer
{
    public interface ISearchHistoryService
    {
        void Save(SearchResult searchResult);
        List<SearchResult> LoadAll();
    }
}
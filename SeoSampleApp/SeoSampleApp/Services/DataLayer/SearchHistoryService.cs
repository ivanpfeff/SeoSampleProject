using MongoDB.Driver;
using SeoSampleApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeoSampleApp.Services.DataLayer
{
    public class SearchHistoryService : ISearchHistoryService
    {
        private readonly IMongoCollection<SearchResult> _collection;

        public SearchHistoryService(IMongoDatabase database)
        {
            _collection = database.GetCollection<SearchResult>("searchResults");
        }

        public List<SearchResult> LoadAll()
        {
            return _collection.AsQueryable().ToList();
        }

        public void Save(SearchResult searchResult)
        {
            _collection.InsertOne(searchResult);
        }
    }
}

using Autofac;
using FluentAssertions;
using NUnit.Framework;
using SeoSampleApp.Entities;
using SeoSampleApp.Services;
using SeoSampleApp.Services.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTests.SeoSampleApp
{
    [TestFixture]
    class SearchHistoryServiceIntegrationTests
    {
        [Test]
        public async Task ShouldAddAndLoadSearchHistory()
        {
            var searchService = IntegrationTestHarness.Container.Resolve<ISearchService>();
            var searchHistoryService = IntegrationTestHarness.Container.Resolve<ISearchHistoryService>();

            //Run a search to make sure there's searches in the mongodb
            await searchService.ProcessSearch(new SearchRequest()
            {
                SearchTerm = "Cat Food",
                URL = "chewy.com"
            });

            //Load to show we can load
            var allSearchHistory = searchHistoryService.LoadAll();
            allSearchHistory.Should().HaveCountGreaterOrEqualTo(1);

            //Show we can find the search we just ran
            var savedSearch = allSearchHistory.FirstOrDefault(x => x.SearchTerm == "Cat Food");
            savedSearch.Should().NotBeNull();
        }
    }
}

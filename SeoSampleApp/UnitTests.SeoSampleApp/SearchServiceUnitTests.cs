using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using SeoSampleApp.Configuration;
using SeoSampleApp.Entities;
using SeoSampleApp.Services;
using SeoSampleApp.Services.DataLayer;

namespace UnitTests.SeoSampleApp
{
    [TestFixture]
    class SearchServiceUnitTests
    {
        private ISearchService _sut;

        private SearchConfiguration _searchConfiguration;
        private ILogger _loggerMock;
        private IHttpWrapperService _httpWrapperMock;
        private ISearchHistoryService _searchHistoryServiceMock;

        private SearchRequest _sampleRequest;

        [SetUp]
        public void SetUp()
        {
            _searchConfiguration = new SearchConfiguration()
            {
                GoogleSearchFormat = "https://google.com/q=SEARCH_TERM"
            };

            _loggerMock = Substitute.For<ILogger>();
            _httpWrapperMock = Substitute.For<IHttpWrapperService>();
            _searchHistoryServiceMock = Substitute.For<ISearchHistoryService>();

            _sut = new SearchService(_loggerMock, _searchConfiguration, _httpWrapperMock, _searchHistoryServiceMock);
            _sampleRequest = new SearchRequest()
            {
                SearchTerm = "Cat Foods",
                URL = "chewy.com"
            };

            _httpWrapperMock.ExecuteGETRequest(Arg.Any<string>()).Returns(
            @"{
              ""items"": 
                [
                  {
                    ""link"": ""https://chewy.com/"",
                    ""displayLink"": ""https://chewy.com"",
                  }
                ]
            }");
        }

        [Test]
        public async Task ShouldSearch()
        {
            var result = await _sut.ProcessSearch(_sampleRequest);
            result.Score.Should().Be(1);
        }

        [Test]
        public async Task ShouldHandleNullRequests()
        {
            Assert.ThrowsAsync<Exception>(async () => { await _sut.ProcessSearch(null); });
        }

        [Test]
        public async Task ShouldHandleRequestsWithNoSearchTerm()
        {
            _sampleRequest.SearchTerm = "";
            Assert.ThrowsAsync<Exception>(async () => { await _sut.ProcessSearch(_sampleRequest); });
        }


        [Test]
        public async Task ShouldSaveSearchResults()
        {
            var result = await _sut.ProcessSearch(_sampleRequest);
            result.Score.Should().Be(1);

            _searchHistoryServiceMock.Received().Save(Arg.Any<SearchResult>());
        }

        [Test]
        public async Task ShouldReturnResultIfSaveThrows()
        {
            _searchHistoryServiceMock.When(x => x.Save(Arg.Any<SearchResult>())).Do(x => { throw new Exception(); });
            var result = await _sut.ProcessSearch(_sampleRequest);
            result.Score.Should().Be(1);
        }
    }
}

using System;
using System.Threading.Tasks;
using FluentAssertions;
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

            _httpWrapperMock = Substitute.For<IHttpWrapperService>();
            _searchHistoryServiceMock = Substitute.For<ISearchHistoryService>();

            _sut = new SearchService(_searchConfiguration, _httpWrapperMock, _searchHistoryServiceMock);
            _sampleRequest = new SearchRequest()
            {
                UseGoogle = true,
                SearchTerm = "Cat Foods",
                SEOTerm = "Meow Mix",
            };

            _httpWrapperMock.ExecuteGETRequest(Arg.Any<string>()).Returns("Meow Mix");
        }

        [Test]
        public async Task ShouldSearch()
        {
            var result = await _sut.ProcessSearch(_sampleRequest);
            result.Hits.Should().Be(1);
        }

        [Test]
        public async Task ShouldSearchWithoutCaseSensitivity()
        {
            _sampleRequest.SEOTerm = "MeOw MiX";
            var result = await _sut.ProcessSearch(_sampleRequest);
            result.Hits.Should().Be(1);
        }

        [Test]
        public async Task ShouldHandleNullRequests()
        {
            Assert.ThrowsAsync<Exception>(async () => { await _sut.ProcessSearch(null); });
        }

        [Test]
        public async Task ShouldHandleNonGoogleRequestsWithoutURL()
        {
            _sampleRequest.UseGoogle = false;
            _sampleRequest.SearchURL = "";
            Assert.ThrowsAsync<Exception>(async () => { await _sut.ProcessSearch(_sampleRequest); });
        }

        [Test]
        public async Task ShouldHandleRequestsWithNoSearchTerm()
        {
            _sampleRequest.SearchTerm = "";
            Assert.ThrowsAsync<Exception>(async () => { await _sut.ProcessSearch(_sampleRequest); });
        }

        [Test]
        public async Task ShouldHandleRequestsWithNoSEOTerm()
        {
            _sampleRequest.SEOTerm = "";
            Assert.ThrowsAsync<Exception>(async () => { await _sut.ProcessSearch(_sampleRequest); });
        }

        [Test]
        public async Task ShouldSaveSearchResults()
        {
            var result = await _sut.ProcessSearch(_sampleRequest);
            result.Hits.Should().Be(1);

            _searchHistoryServiceMock.Received().Save(Arg.Any<SearchResult>());
        }

        [Test]
        public async Task ShouldReturnResultIfSaveThrows()
        {
            _searchHistoryServiceMock.When(x => x.Save(Arg.Any<SearchResult>())).Do(x => { throw new Exception(); });
            var result = await _sut.ProcessSearch(_sampleRequest);
            result.Hits.Should().Be(1);
        }
    }
}

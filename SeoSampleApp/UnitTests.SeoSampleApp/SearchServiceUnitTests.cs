using System;
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
        public void ShouldSearch()
        {
            var result = _sut.ProcessSearch(_sampleRequest);
            result.Hits.Should().Be(1);
        }

        [Test]
        public void ShouldSearchWithoutCaseSensitivity()
        {
            _sampleRequest.SEOTerm = "MeOw MiX";
            var result = _sut.ProcessSearch(_sampleRequest);
            result.Hits.Should().Be(1);
        }

        [Test]
        public void ShouldHandleNullRequests()
        {
            Assert.Throws<Exception>(() => { _sut.ProcessSearch(null); });
        }

        [Test]
        public void ShouldHandleNonGoogleRequestsWithoutURL()
        {
            _sampleRequest.UseGoogle = false;
            _sampleRequest.SearchURL = "";
            Assert.Throws<Exception>(() => { _sut.ProcessSearch(_sampleRequest); });
        }

        [Test]
        public void ShouldHandleRequestsWithNoSearchTerm()
        {
            _sampleRequest.SearchTerm = "";
            Assert.Throws<Exception>(() => { _sut.ProcessSearch(_sampleRequest); });
        }

        [Test]
        public void ShouldHandleRequestsWithNoSEOTerm()
        {
            _sampleRequest.SEOTerm = "";
            Assert.Throws<Exception>(() => { _sut.ProcessSearch(_sampleRequest); });
        }

        [Test]
        public void ShouldSaveSearchResults()
        {
            var result = _sut.ProcessSearch(_sampleRequest);
            result.Hits.Should().Be(1);

            _searchHistoryServiceMock.Received().Save(Arg.Any<SearchResult>());
        }

        [Test]
        public void ShouldReturnResultIfSaveThrows()
        {
            _searchHistoryServiceMock.When(x => x.Save(Arg.Any<SearchResult>())).Do(x => { throw new Exception(); });
            var result = _sut.ProcessSearch(_sampleRequest);
            result.Hits.Should().Be(1);
        }
    }
}

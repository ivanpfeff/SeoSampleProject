using System;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SeoSampleApp.Entities;
using SeoSampleApp.Services;

namespace UnitTests.SeoSampleApp
{
    [TestFixture]
    class SearchServiceUnitTests
    {
        private ISearchService _sut;

        private IHttpWrapperService _httpWrapperMock;
        private SearchRequest _sampleRequest;

        [SetUp]
        public void SetUp()
        {
            _httpWrapperMock = Substitute.For<IHttpWrapperService>();

            _sut = new SearchService(_httpWrapperMock);
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

        public void ShouldHandleRequestsWithNoSEOTerm()
        {
            _sampleRequest.SEOTerm = "";
            Assert.Throws<Exception>(() => { _sut.ProcessSearch(_sampleRequest); });
        }
    }
}

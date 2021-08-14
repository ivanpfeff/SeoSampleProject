using Autofac;
using FluentAssertions;
using NUnit.Framework;
using SeoSampleApp.Entities;
using SeoSampleApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTests.SeoSampleApp
{
    [TestFixture]
    class SearchServiceIntegrationTests
    {
        [Test]
        public async Task ShouldFindMusicStreamingServices()
        {
            var searchService = IntegrationTestHarness.Container.Resolve<ISearchService>();

            var pandoraResult = await searchService.ProcessSearch(new SearchRequest()
            {
                SearchTerm = "music streaming services",
                URL = "pandora.com"
            });

            var spotifyResult = await searchService.ProcessSearch(new SearchRequest()
            {
                SearchTerm = "music streaming services",
                URL = "spotify.com"
            });

            var tidalResult = await searchService.ProcessSearch(new SearchRequest()
            {
                SearchTerm = "music streaming services",
                URL = "tidal.com"
            });

            pandoraResult.Score.Should().BeGreaterThan(0);
            spotifyResult.Score.Should().BeGreaterThan(0);
            tidalResult.Score.Should().BeGreaterThan(0);
        }
    }
}

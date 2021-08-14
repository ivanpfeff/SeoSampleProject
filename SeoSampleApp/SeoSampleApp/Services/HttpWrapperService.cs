using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SeoSampleApp.Services
{
    public class HttpWrapperService : IHttpWrapperService
    {
        public async Task<string> ExecuteGETRequest(string searchUrl)
        {
            var httpClient = new HttpClient();
            return await httpClient.GetStringAsync(searchUrl);
        }
    }
}

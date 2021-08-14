using Microsoft.AspNetCore.Mvc;
using SeoSampleApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeoSampleApp
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        // POST api/<SearchController>
        [HttpPost]
        public void Post([FromBody] SearchRequest searchRequest)
        {
        }
    }
}

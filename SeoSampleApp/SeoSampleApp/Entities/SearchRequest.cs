using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeoSampleApp.Entities
{
    public class SearchRequest
    {
        public bool UseGoogle { get; set; }
        public string SearchTerm { get; set; }
        public string SearchURL { get; set; }
        public string SEOTerm { get; set; }
    }
}

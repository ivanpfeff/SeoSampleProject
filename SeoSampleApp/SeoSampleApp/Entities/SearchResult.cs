using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeoSampleApp.Entities
{
    public class SearchResult
    {
        public Guid SearchID { get; set; }
        public DateTime Date { get; set; }
        public string SEOTerm { get; set; }
        public string URL { get; set; }
        public int Hits { get; set; }
        public string Body { get; set; }
    }
}

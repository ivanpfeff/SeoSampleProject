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
        public string SearchTerm { get; set; }
        public string URL { get; set; }
        public int Score { get; set; }
    }
}

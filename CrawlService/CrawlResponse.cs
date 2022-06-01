using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceStack;

namespace CrawlService
{
    public class CrawlResponse
    {
        public List<string> Articles { get; set; }
        public List<Uri> Urls { get; set; }
    }
}

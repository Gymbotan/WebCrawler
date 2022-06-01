using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceStack;

namespace CrawlService
{
    [Route("/crawl")]
    public class CrawlRequest : IReturn<CrawlResponse>
    {
        public CrawlRequest()
        {
        }

        public CrawlRequest(string url, int amount)
        {
            Url = url;
            Amount = amount;
        }

        public string Url { get; set; }
        public int Amount { get; set; }        
    }
}

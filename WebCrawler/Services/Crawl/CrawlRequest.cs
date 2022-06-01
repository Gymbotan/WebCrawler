using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceStack;
using WebCrawler.Domain.Entities;

namespace WebCrawler.Services.Crawl
{
    [Route("/crawl1")]
    public class CrawlRequest1 : IReturn<CrawlResponse1>
    {
        public CrawlRequest1()
        {
        }

        public CrawlRequest1(string url, int amount)
        {
            Url = url;
            Amount = amount;
        }

        public string Url { get; set; }
        public int Amount { get; set; }        
    }
}

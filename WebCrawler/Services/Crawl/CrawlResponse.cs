using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceStack;
using WebCrawler.Domain.Entities;

namespace WebCrawler.Services.Crawl
{
    public class CrawlResponse1
    {
        public List<Article> Articles { get; set; }
    }
}

using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Domain.Crawlers;

namespace WebCrawler.Services.Crawl
{
    public class Crawl1Service : Service
    {
        private readonly MyCrawler crawler;

        public Crawl1Service(MyCrawler crawler)
        {
            this.crawler = crawler;
        }

        [AddHeader(ContentType = MimeTypes.Json)]
        public async Task<object> Get(CrawlRequest1 request)
        {
            var response = new CrawlResponse1();
            if (string.IsNullOrWhiteSpace(request.Url))
            {
                string siteUrl = "http://regions.by";
                response.Articles = await crawler.Crawl(siteUrl, request.Amount);
            }
            else
            {
                response.Articles = await crawler.Crawl(request.Url, request.Amount);
            }

            return response;
        }
    }
}

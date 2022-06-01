using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrawlService.Crawlers;

namespace CrawlService
{
    public class CrawlService : Service
    {
        private readonly MyServiceCrawler crawler;

        public CrawlService(MyServiceCrawler crawler)
        {
            this.crawler = crawler;
        }

        [AddHeader(ContentType = MimeTypes.Json)]
        public async Task<object> Get(CrawlRequest request)
        {
            var response = new CrawlResponse();
            if (string.IsNullOrWhiteSpace(request.Url))
            {
                string siteUrl = "http://regions.by";
                var result = await crawler.Crawl(siteUrl, request.Amount);
                response.Articles = result.Item1;
                response.Urls = result.Item2;
            }
            else
            {
                var result = await crawler.Crawl(request.Url, request.Amount);
                response.Articles = result.Item1;
                response.Urls = result.Item2;
            }

            return response;
        }
    }
}

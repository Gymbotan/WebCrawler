using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Domain.Crawlers;
using WebCrawler.Domain.Entities;

namespace WebCrawler.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CrawlerController : ControllerBase
    {
        private readonly MyCrawler crawler;

        public CrawlerController(MyCrawler crawler)
        {
            this.crawler = crawler;
        }

        [HttpGet]
        public async Task<List<Article>> Get(string url, int amount)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                string siteUrl = "http://regions.by";
                return await crawler.Crawl(siteUrl, amount);
            }
            else
            {
                return await crawler.Crawl(url, amount);
            }
        }
    }
}

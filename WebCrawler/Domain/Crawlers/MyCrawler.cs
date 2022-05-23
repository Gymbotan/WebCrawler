using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abot2.Core;
using Abot2.Crawler;
using Abot2.Poco;
using Abot2.Util;
using WebCrawler.Domain.Entities;

namespace WebCrawler.Domain.Crawlers
{
    public class MyCrawler
    {
        private readonly List<Article> articles;

        public MyCrawler()
        {
            this.articles = new List<Article>();
        }

        public async Task<List<Article>> Crawl(string siteUrl, int amount)
        {
            var config = new CrawlConfiguration
            {
                MaxPagesToCrawl = amount, //Only crawl 'amount' pages
                MinCrawlDelayPerDomainMilliSeconds = 10 //Wait this many millisecs between requests
            };
            var crawler = new PoliteWebCrawler(config);

            crawler.PageCrawlCompleted += PageCrawlCompleted;//Several events available...

            string url = (siteUrl.Trim().StartsWith("https://") || siteUrl.Trim().StartsWith("http://")) ? siteUrl.Trim() : "https://" + siteUrl.Trim();

            //await crawler.CrawlAsync(new Uri("https://" + siteUrl.Trim()));
            await crawler.CrawlAsync(new Uri(url));
            return articles;
        }

        //private static async Task DemoSimpleCrawler(string siteUrl, int amount)
        //{
        //    var config = new CrawlConfiguration
        //    {
        //        MaxPagesToCrawl = amount, //Only crawl 'amount' pages
        //        MinCrawlDelayPerDomainMilliSeconds = 10 //Wait this many millisecs between requests
        //    };
        //    var crawler = new PoliteWebCrawler(config);

        //    crawler.PageCrawlCompleted += PageCrawlCompleted;//Several events available...

        //    await crawler.CrawlAsync(new Uri("https://" + siteUrl.Trim()));
        //}

        private void PageCrawlCompleted(object sender, PageCrawlCompletedArgs e)
        {   
            var rawPageText = e.CrawledPage.Content.Text;

            // Check is the page contains an article or not.
            if (!rawPageText.Contains("time itemprop=\"datePublished\""))
            {
                return;
            }
            else
            {
                Article article = new();
                article.FullText = rawPageText;
                article.Url = e.CrawledPage.HttpRequestMessage.RequestUri;
                articles.Add(article);
            }
        }
    }
}

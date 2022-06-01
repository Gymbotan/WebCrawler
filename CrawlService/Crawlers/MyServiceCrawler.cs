using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abot2.Core;
using Abot2.Crawler;
using Abot2.Poco;
using Abot2.Util;

namespace CrawlService.Crawlers
{
    public class MyServiceCrawler
    {
        private readonly List<string> articles;
        private readonly List<Uri> urls;

        public MyServiceCrawler()
        {
            this.articles = new List<string>();
            this.urls = new List<Uri>();
        }

        public async Task<(List<string>, List<Uri>)> Crawl(string siteUrl, int amount)
        {
            var config = new CrawlConfiguration
            {
                MaxPagesToCrawl = amount, //Only crawl 'amount' pages
                MinCrawlDelayPerDomainMilliSeconds = 10 //Wait this many millisecs between requests
            };
            var crawler = new PoliteWebCrawler(config);

            crawler.PageCrawlCompleted += PageCrawlCompleted;//Several events available...

            string clearUrl = (siteUrl.Trim().StartsWith("https://") || siteUrl.Trim().StartsWith("http://")) ? siteUrl.Trim() : "https://" + siteUrl.Trim();

            await crawler.CrawlAsync(new Uri(clearUrl));
            return (articles, urls);
        }

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
                string article = rawPageText;
                Uri url = e.CrawledPage.HttpRequestMessage.RequestUri;
                articles.Add(article);
                urls.Add(url);
            }
        }
    }
}

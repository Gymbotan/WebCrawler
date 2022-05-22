using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abot2.Core;
using Abot2.Crawler;
using Abot2.Poco;
using Abot2.Util;
using Serilog;
using WebCrawler;
using WebCrawler.Domain.Entities;
using WebCrawler.Domain.Parsers;
using WebCrawler.Domain.Repositories.Interfaces;

namespace WebCrawler.Controllers
{
    public class HomeController : Controller
    {
        private readonly IArticlesRepository repository;

        public HomeController(IArticlesRepository repository)
        {
            this.repository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Contacts()
        {
            return View();
        }

        public IActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Search(string siteUrl, int amount)
        {
            ViewBag.siteUrl = siteUrl;

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                //.WriteTo.Console()
                .CreateLogger();

            //Log.Logger.Information("Demo starting up!");
            int before = Storage.articlesRepository.GetAmountOfArticles();
            await DemoSimpleCrawler(siteUrl, amount);
            int after = Storage.articlesRepository.GetAmountOfArticles();
            ViewBag.AmountOfFindedArticles = after - before;
            return View();
        }

        private static async Task DemoSimpleCrawler(string siteUrl, int amount)
        {
            var config = new CrawlConfiguration
            {
                MaxPagesToCrawl = amount, //Only crawl 'amount' pages
                MinCrawlDelayPerDomainMilliSeconds = 10 //Wait this many millisecs between requests
            };
            var crawler = new PoliteWebCrawler(config);

            crawler.PageCrawlCompleted += PageCrawlCompleted;//Several events available...

            await crawler.CrawlAsync(new Uri("https://"+siteUrl.Trim()));
        }

        private static void PageCrawlCompleted(object sender, PageCrawlCompletedArgs e)
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
                article.Id = Guid.NewGuid();
                article.FullText = rawPageText;
                article.Url = e.CrawledPage.HttpRequestMessage.RequestUri;

                rawPageText = rawPageText.Substring(rawPageText.IndexOf("document.oncopy"));
                Parser.Parse(article, rawPageText);

                if (!Storage.articlesRepository.Contains(article))
                {
                    Storage.articlesRepository.SaveArticle(article);
                }
            }
        }
    }
}

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
using WebCrawler.Domain.Crawlers;
using WebCrawler.Domain.AttributeFinder;

namespace WebCrawler.Controllers
{
    public class HomeController : Controller
    {
        private readonly IArticlesRepository repository;
        private readonly MyCrawler crawler;
        private readonly MyParser parser;
        private readonly MyAttributeFinder finder;

        public HomeController(IArticlesRepository repository, MyCrawler crawler, MyParser parser, MyAttributeFinder finder)
        {
            this.repository = repository;
            this.crawler = crawler;
            this.parser = parser;
            this.finder = finder;
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

            //Log.Logger = new LoggerConfiguration()
            //    .MinimumLevel.Information()
            //    //.WriteTo.Console()
            //    .CreateLogger();

            //Log.Logger.Information("Demo starting up!");
            int before = Storage.articlesRepository.GetAmountOfArticles();
            //await DemoSimpleCrawler(siteUrl, amount);
            List<Article> articles = await crawler.Crawl(siteUrl, amount);
            for (int i = 0; i < articles.Count; i++)
            {
                var result = parser.Parse(articles[i].FullText);
                articles[i].Title = result.Item1;
                articles[i].Text = result.Item2;
                articles[i].Date = result.Item3;
                if (!Storage.articlesRepository.Contains(articles[i]))
                {
                    articles[i].Id = Guid.NewGuid();
                    var attrResult = finder.FindAttributes(articles[i].Text);
                    SaveAttributes(articles[i], attrResult.Item1, attrResult.Item2, attrResult.Item3);
                    Storage.articlesRepository.SaveArticle(articles[i]);
                }
            }

            int after = Storage.articlesRepository.GetAmountOfArticles();
            ViewBag.AmountOfFindedArticles = after - before;
            return View();
        }

        private void SaveAttributes(Article article, List<PersonAttribute> pAttributes, List<GeoAttribute> gAttributes, List<OrganizationAttribute> oAttributes)
        {
            for (int i = 0; i < pAttributes.Count; i++)
            {
                PersonAttribute attribute;
                if (Storage.personAttributesRepository.Contains(pAttributes[i]))
                {
                    attribute = Storage.personAttributesRepository.GetPersonAttributeByFIO(pAttributes[i]);
                }
                else
                {
                    attribute = pAttributes[i];
                }

                attribute.Owners.Add(article);
                article.PersonAttributes.Add(attribute);
                Storage.personAttributesRepository.SavePersonAtribute(attribute);
            }

            for (int i = 0; i < gAttributes.Count; i++)
            {
                GeoAttribute attribute;
                if (Storage.geoAttributesRepository.Contains(gAttributes[i]))
                {
                    attribute = Storage.geoAttributesRepository.GetGeoAttributeByNameAndType(gAttributes[i]);

                }
                else
                {
                    attribute = gAttributes[i];
                }
                attribute.Owners.Add(article);
                article.GeoAttributes.Add(attribute);
                Storage.geoAttributesRepository.SaveGeoAtribute(attribute);
            }

            for (int i = 0; i < oAttributes.Count; i++)
            {
                OrganizationAttribute attribute;
                if (Storage.organizationAttributesRepository.Contains(oAttributes[i]))
                {
                    attribute = Storage.organizationAttributesRepository.GetOrganizationAttributeByName(oAttributes[i]);
                }
                else
                {
                    attribute = oAttributes[i];
                }
                attribute.Owners.Add(article);
                article.OrganizationAttributes.Add(attribute);
                Storage.organizationAttributesRepository.SaveOrganizationAttribute(attribute);
            }
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

        //    await crawler.CrawlAsync(new Uri("https://"+siteUrl.Trim()));
        //}

        //private static void PageCrawlCompleted(object sender, PageCrawlCompletedArgs e)
        //{
        //    var rawPageText = e.CrawledPage.Content.Text;

        //    // Check is the page contains an article or not.
        //    if (!rawPageText.Contains("time itemprop=\"datePublished\""))
        //    {                
        //        return;
        //    }
        //    else
        //    {
        //        Article article = new();
        //        article.Id = Guid.NewGuid();
        //        article.FullText = rawPageText;
        //        article.Url = e.CrawledPage.HttpRequestMessage.RequestUri;

        //        rawPageText = rawPageText.Substring(rawPageText.IndexOf("document.oncopy"));
        //        Parser.Parse(article, rawPageText);

        //        if (!Storage.articlesRepository.Contains(article))
        //        {
        //            Storage.articlesRepository.SaveArticle(article);
        //        }
        //    }
        //}
    }
}

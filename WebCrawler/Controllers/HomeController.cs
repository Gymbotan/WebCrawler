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
//using WebCrawler.Domain.Crawlers;
using WebCrawler.Domain.AttributeFinder;
using System.Net.Http;
using System.Text.Json;
using ServiceStack;
//using WebCrawler.Services.Crawl;
using CrawlService;
using ParseService;
using WebCrawler.Services.Parse;

namespace WebCrawler.Controllers
{
    public class HomeController : Controller
    {
        private readonly Storage storage;
        //private readonly MyCrawler crawler;
        private readonly MyParser parser;
        private readonly MyAttributeFinder finder;
        private readonly HttpClient client;
        private readonly string crawlAddress = "https://localhost:44381/";
        private readonly string parseAddress = "https://localhost:44382/";
        //private readonly JsonServiceClient crawlClient;
        //private readonly JsonServiceClient parseClient;

        public HomeController(Storage storage, /*MyCrawler crawler,*/ MyParser parser, MyAttributeFinder finder, HttpClient client)
        {
            this.storage = storage;
            //this.crawler = crawler;
            this.parser = parser;
            this.finder = finder;
            this.client = client;
            client.BaseAddress = new Uri(crawlAddress);
            //crawlClient = new JsonServiceClient(baseAddress);
            //parseClient = new JsonServiceClient(parseAddress);
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
            int before = storage.articlesRepository.GetAmountOfArticles();

            // Request to API Crawler
            //var requestResult = await client.GetStringAsync($"crawler?amount={amount}");

            //var requestResult = await client.GetStringAsync($"Crawler?url={siteUrl}&amount={amount}");
            //JsonSerializerOptions options = new();
            //options.PropertyNameCaseInsensitive = true;
            //List<Article> articles = JsonSerializer.Deserialize<List<Article>>(requestResult, options);

            //List<Article> articles = new();
            //List<(Uri, string)> articlesData = new();

            List<string> articlesData;
            List<Uri> articlesUrls;


            /*Здесь создаю клиент, он возвращает полные тексты и ссылки на статьи. В этом случае второй сервис не виден
             Сначала был без using, решил протестить, вдруг больше одного клиента нельзя создать*/


            using (var crawlClient = new JsonServiceClient(crawlAddress))
            {
                var crawlResult = crawlClient.Get(new CrawlRequest(siteUrl, amount));
                articlesData = crawlResult.Articles;
                articlesUrls = crawlResult.Urls;
            }


            /*Если сервис не вызывать, а подать какой-то набор строк (как внизу, то второй сервис станет доступен)*/


            //var articlesData = new List<string>() { "asdasdasdas", "sfnjk324j234", "sdkcbcjkxbvjk234234s"};

            //List <Article> articles = await crawler.Crawl(siteUrl, amount);
            for (int i = 0; i < Math.Min(articlesData.Count, 10/*articlesUrls.Count*/); i++)
            {
                Article article = new();
                article.Url = new Uri("http://regions.by");
                //article.Url = articlesUrls[i];
                article.FullText = articlesData[i];

                // Request to API Parser
                //requestResult = await client.GetStringAsync($"Parser?text={articles[i].FullText}");
                //var parseResult = JsonSerializer.Deserialize<RawTextParams>(requestResult, options);

                //var parseResult = parser.Parse(articles[i].FullText);
                //articles[i].Title = parseResult.Title;
                //articles[i].Text = parseResult.Text;
                //articles[i].Date = parseResult.Date;

                string rawText = SimplifyText(article.FullText);


                /* Вот вызов второго сервиса */ 


                using (var parseClient = new JsonServiceClient(parseAddress))
                {
                    var result = parseClient.Get(new ParseRequest(rawText));
                    article.Title = result.Title;
                    article.Text = result.Text;
                    article.Date = result.Date;
                }
                if (!storage.articlesRepository.Contains(article))
                {
                    article.Id = Guid.NewGuid();

                    // Request to API Attributes
                    //requestResult = await client.GetStringAsync($"Attributes?text={articles[i].Text}");
                    //var attrResult = JsonSerializer.Deserialize<TextAttributes>(requestResult, options);

                    var attrResult = finder.FindAttributes(article.Text);
                    SaveAttributes(article, attrResult.PersonalAttributes, attrResult.GeoAttributes, attrResult.OrganizationAttributes);
                    storage.articlesRepository.SaveArticle(article);
                }
            }

            int after = storage.articlesRepository.GetAmountOfArticles();
            ViewBag.AmountOfFindedArticles = after - before;
            return View();
        }

        private static string SimplifyText(string rawText)
        {
            string simpleText;
            if (rawText.Contains("document.oncopy"))
            {
                simpleText = rawText.Substring(rawText.IndexOf("document.oncopy"));
            }
            else
            {
                simpleText = rawText;
            }

            if (simpleText.Contains("<h1 class=\"entry-title\" itemprop=\"headline\">"))
            {
                simpleText = simpleText.Substring(simpleText.IndexOf("<h1 class=\"entry-title\" itemprop=\"headline\">"));
            }

            if (simpleText.Contains("div class=\"social-buttons\""))
            {
                simpleText = simpleText.Remove(simpleText.IndexOf("div class=\"social-buttons\""));
            }

            return simpleText;
        }

        private void SaveAttributes(Article article, List<PersonAttribute> pAttributes, List<GeoAttribute> gAttributes, List<OrganizationAttribute> oAttributes)
        {
            for (int i = 0; i < pAttributes.Count; i++)
            {
                PersonAttribute attribute;
                if (storage.personAttributesRepository.Contains(pAttributes[i]))
                {
                    attribute = storage.personAttributesRepository.GetPersonAttributeByFIO(pAttributes[i]);
                }
                else
                {
                    attribute = pAttributes[i];
                }

                attribute.Owners.Add(article);
                article.PersonAttributes.Add(attribute);
                storage.personAttributesRepository.SavePersonAtribute(attribute);
            }

            for (int i = 0; i < gAttributes.Count; i++)
            {
                GeoAttribute attribute;
                if (storage.geoAttributesRepository.Contains(gAttributes[i]))
                {
                    attribute = storage.geoAttributesRepository.GetGeoAttributeByNameAndType(gAttributes[i]);

                }
                else
                {
                    attribute = gAttributes[i];
                }
                attribute.Owners.Add(article);
                article.GeoAttributes.Add(attribute);
                storage.geoAttributesRepository.SaveGeoAtribute(attribute);
            }

            for (int i = 0; i < oAttributes.Count; i++)
            {
                OrganizationAttribute attribute;
                if (storage.organizationAttributesRepository.Contains(oAttributes[i]))
                {
                    attribute = storage.organizationAttributesRepository.GetOrganizationAttributeByName(oAttributes[i]);
                }
                else
                {
                    attribute = oAttributes[i];
                }
                attribute.Owners.Add(article);
                article.OrganizationAttributes.Add(attribute);
                storage.organizationAttributesRepository.SaveOrganizationAttribute(attribute);
            }
        }
    }
}

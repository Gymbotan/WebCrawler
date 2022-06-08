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
using AttributesService;

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
        private readonly string attributesAddress = "https://localhost:44362/";
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
                var crawlResult = crawlClient.Get(new CrawlRequest { Url = siteUrl, Amount = amount});
                articlesData = crawlResult.Articles;
                articlesUrls = crawlResult.Urls;
            }


            /*Если сервис не вызывать, а подать какой-то набор строк (как внизу, то второй сервис станет доступен)*/


            //var articlesData = new List<string>() { "asdasdasdas", "sfnjk324j234", "sdkcbcjkxbvjk234234s"};

            //List <Article> articles = await crawler.Crawl(siteUrl, amount);
            for (int i = 0; i < Math.Min(articlesData.Count, articlesUrls.Count); i++)
            {
                Article article = new();
                //article.Url = new Uri("http://regions.by");
                article.Url = articlesUrls[i];
                article.FullText = articlesData[i];



                // Request to API Parser
                //requestResult = await client.GetStringAsync($"Parser?text={articles[i].FullText}");
                //var parseResult = JsonSerializer.Deserialize<RawTextParams>(requestResult, options);

                var parseResult = parser.Parse(article.FullText);
                article.Title = parseResult.Item1;
                article.Text = parseResult.Item2;
                article.Date = parseResult.Item3;

                //string rawText = SimplifyText(article.FullText);


                /* Вот вызов второго сервиса */


                //using (var parseClient = new JsonServiceClient(parseAddress))
                //{
                //    var result = parseClient.Get(new ParseRequest("<h1 class=\"entry-title\" itemprop=\"headline\">Студентка из Витебска стала «Королевой студенчества – 2022»</h1> <time itemprop=\"datePublished\" datetime=\"2022-06-03\">03.06.2022</time> <div class=\"entry-content\" itemprop=\"articleBody\"> data-smi-blockid=\"19612\"></div> <script>(window.smiq=window.smiq||[]).push({})</script></div><p>Финал республиканского конкурса красоты и таланта «Королева студенчества – 2022» прошел в Минске. Звания королевы была удостоена студентка Витебского государственного технологического университета Анна Иванова. Звания Анна.</p></div> "));
                //    article.Title = result.Title;
                //    article.Text = result.Text;
                //    article.Date = result.Date;
                //}
                if (!storage.articlesRepository.Contains(article))
                {
                    article.Id = Guid.NewGuid();

                    // Request to API Attributes
                    //requestResult = await client.GetStringAsync($"Attributes?text={articles[i].Text}");
                    //var attrResult = JsonSerializer.Deserialize<TextAttributes>(requestResult, options);

                    //var attrResult = finder.FindAttributes(article.Text);
                    //SaveAttributes(article, attrResult.PersonalAttributes, attrResult.GeoAttributes, attrResult.OrganizationAttributes);

                    var attributeClient = new JsonServiceClient(attributesAddress);
                    var attributesResult = attributeClient.Get(new AttributesRequest { Text = article.Text.Substring(0, Math.Min(article.Text.Length, 350)) });
                    SaveAttributes(article, attributesResult);

                    storage.articlesRepository.SaveArticle(article);
                }
            }

            int after = storage.articlesRepository.GetAmountOfArticles();
            ViewBag.AmountOfFindedArticles = after - before;
            return View();
        }

        private void SaveAttributes(Article article, AttributesResponse response)
        {
            foreach (var attr in response.PersonalAttributes)
            {
                PersonAttribute attribute = new();
                attribute.FirstName = attr.FirstName;
                attribute.MiddleName = attr.MiddleName;
                attribute.LastName = attr.LastName;
                if (storage.personAttributesRepository.Contains(attribute))
                {
                    attribute = storage.personAttributesRepository.GetPersonAttributeByFIO(attribute);
                }
                else
                {
                    attribute.Id = Guid.NewGuid();
                    attribute.Age = attr.Age;
                    attribute.Gender = attr.Gender;
                }
                attribute.Owners.Add(article);
                article.PersonAttributes.Add(attribute);
                storage.personAttributesRepository.SavePersonAtribute(attribute);
            }

            foreach (var attr in response.GeoAttributes)
            {
                GeoAttribute attribute = new();
                attribute.Name = attr.Name;
                attribute.Type = attr.Type;
                if (storage.geoAttributesRepository.Contains(attribute))
                {
                    attribute = storage.geoAttributesRepository.GetGeoAttributeByNameAndType(attribute);
                }
                else
                {
                    attribute.Id = Guid.NewGuid();
                    attribute.Alpha2 = attr.Alpha2;
                }
                attribute.Owners.Add(article);
                article.GeoAttributes.Add(attribute);
                storage.geoAttributesRepository.SaveGeoAtribute(attribute);
            }

            foreach (var attr in response.OrganizationAttributes)
            {
                OrganizationAttribute attribute = new();
                attribute.Name = attr.Name;
                attribute.Type = attr.Type;
                if (storage.organizationAttributesRepository.Contains(attribute))
                {
                    attribute = storage.organizationAttributesRepository.GetOrganizationAttributeByName(attribute);
                }
                else
                {
                    attribute.Id = Guid.NewGuid();
                    attribute.INN = attr.INN;
                    attribute.Geo = attr.Geo;
                }
                attribute.Owners.Add(article);
                article.OrganizationAttributes.Add(attribute);
                storage.organizationAttributesRepository.SaveOrganizationAttribute(attribute);
            }
        }

        //private void SaveAttributes(Article article, List<PersonAttribute> pAttributes, List<GeoAttribute> gAttributes, List<OrganizationAttribute> oAttributes)
        //{
        //    for (int i = 0; i < pAttributes.Count; i++)
        //    {
        //        PersonAttribute attribute;
        //        if (storage.personAttributesRepository.Contains(pAttributes[i]))
        //        {
        //            attribute = storage.personAttributesRepository.GetPersonAttributeByFIO(pAttributes[i]);
        //        }
        //        else
        //        {
        //            attribute = pAttributes[i];
        //        }

        //        attribute.Owners.Add(article);
        //        article.PersonAttributes.Add(attribute);
        //        storage.personAttributesRepository.SavePersonAtribute(attribute);
        //    }

        //    for (int i = 0; i < gAttributes.Count; i++)
        //    {
        //        GeoAttribute attribute;
        //        if (storage.geoAttributesRepository.Contains(gAttributes[i]))
        //        {
        //            attribute = storage.geoAttributesRepository.GetGeoAttributeByNameAndType(gAttributes[i]);

        //        }
        //        else
        //        {
        //            attribute = gAttributes[i];
        //        }
        //        attribute.Owners.Add(article);
        //        article.GeoAttributes.Add(attribute);
        //        storage.geoAttributesRepository.SaveGeoAtribute(attribute);
        //    }

        //    for (int i = 0; i < oAttributes.Count; i++)
        //    {
        //        OrganizationAttribute attribute;
        //        if (storage.organizationAttributesRepository.Contains(oAttributes[i]))
        //        {
        //            attribute = storage.organizationAttributesRepository.GetOrganizationAttributeByName(oAttributes[i]);
        //        }
        //        else
        //        {
        //            attribute = oAttributes[i];
        //        }
        //        attribute.Owners.Add(article);
        //        article.OrganizationAttributes.Add(attribute);
        //        storage.organizationAttributesRepository.SaveOrganizationAttribute(attribute);
        //    }
        //}
    }
}

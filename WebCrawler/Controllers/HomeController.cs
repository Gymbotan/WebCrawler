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
using System.Net.Http;
using System.Text.Json;

namespace WebCrawler.Controllers
{
    public class HomeController : Controller
    {
        private readonly Storage storage;
        private readonly MyCrawler crawler;
        private readonly MyParser parser;
        private readonly MyAttributeFinder finder;
        private readonly HttpClient client;
        private readonly string baseAddress = "https://localhost:44386/";

        public HomeController(Storage storage, MyCrawler crawler, MyParser parser, MyAttributeFinder finder, HttpClient client)
        {
            this.storage = storage;
            this.crawler = crawler;
            this.parser = parser;
            this.finder = finder;
            this.client = client;
            client.BaseAddress = new Uri(baseAddress);
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
            var requestResult = await client.GetStringAsync($"Crawler?url={siteUrl}&amount={amount}");
            JsonSerializerOptions options = new();
            options.PropertyNameCaseInsensitive = true;
            List<Article> articles = JsonSerializer.Deserialize<List<Article>>(requestResult, options);

            //List <Article> articles = await crawler.Crawl(siteUrl, amount);
            for (int i = 0; i < articles.Count; i++)
            {
                // Request to API Parser
                //requestResult = await client.GetStringAsync($"Parser?text={articles[i].FullText}");
                //var parseResult = JsonSerializer.Deserialize<RawTextParams>(requestResult, options);

                var parseResult = parser.Parse(articles[i].FullText);
                articles[i].Title = parseResult.Title;
                articles[i].Text = parseResult.Text;
                articles[i].Date = parseResult.Date;
                if (!storage.articlesRepository.Contains(articles[i]))
                {
                    articles[i].Id = Guid.NewGuid();

                    // Request to API Attributes
                    //requestResult = await client.GetStringAsync($"Attributes?text={articles[i].Text}");
                    //var attrResult = JsonSerializer.Deserialize<TextAttributes>(requestResult, options);

                    var attrResult = finder.FindAttributes(articles[i].Text);
                    SaveAttributes(articles[i], attrResult.PersonalAttributes, attrResult.GeoAttributes, attrResult.OrganizationAttributes);
                    storage.articlesRepository.SaveArticle(articles[i]);
                }
            }

            int after = storage.articlesRepository.GetAmountOfArticles();
            ViewBag.AmountOfFindedArticles = after - before;
            return View();
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

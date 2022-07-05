using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using WebCrawler;
using WebCrawler.Domain.Entities;
using WebCrawler.Domain.Parsers;
using WebCrawler.Domain.Repositories.Interfaces;
using System.Net.Http;
using System.Text.Json;
using ServiceStack;
using CrawlService;
using ParseService;
using AttributesService;
using Microsoft.Extensions.Configuration;

namespace WebCrawler.Controllers
{
    public class HomeController : Controller
    {
        private readonly Storage storage;
        private readonly MyParser parser;
        private readonly JsonServiceClient crawlClient;
        private readonly JsonServiceClient attributeClient;

        //private readonly string crawlAddress = Confiration["CrawlAddress"]; "https://localhost:44381/";
        //private readonly string attributesAddress = "https://localhost:44362/";

        public IConfiguration Configuration { get; }

        public HomeController(Storage storage, MyParser parser, HttpClient client, IConfiguration configuration)
        {
            this.storage = storage;
            this.parser = parser;
            //client.BaseAddress = new Uri(crawlAddress);
            this.Configuration = configuration;
            crawlClient = new JsonServiceClient(Configuration["CrawlAddress"]);
            attributeClient = new JsonServiceClient(Configuration["AttributesAddress"]);
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

            List<string> articlesData;
            List<Uri> articlesUrls;

            var crawlResult = crawlClient.Get(new CrawlRequest { Url = siteUrl, Amount = amount });
            articlesData = crawlResult.Articles;
            articlesUrls = crawlResult.Urls;

            //using (var crawlClient = new JsonServiceClient(crawlAddress))
            //{
            //    var crawlResult = crawlClient.Get(new CrawlRequest { Url = siteUrl, Amount = amount});
            //    articlesData = crawlResult.Articles;
            //    articlesUrls = crawlResult.Urls;
            //}

            for (int i = 0; i < Math.Min(articlesData.Count, articlesUrls.Count); i++)
            {
                Article article = new();
                article.Url = articlesUrls[i];
                article.FullText = articlesData[i];

                var parseResult = parser.Parse(article.FullText);
                article.Title = parseResult.Item1;
                article.Text = parseResult.Item2;
                article.Date = parseResult.Item3;

                if (!storage.articlesRepository.Contains(article))
                {
                    //Guid id;
                    //do
                    //{
                    //    id = Guid.NewGuid();
                    //}
                    //while (storage.articlesRepository.GetArticleById(id) != null);
                    //article.Id = id;
                    //var attributeClient = new JsonServiceClient(attributesAddress);
                    storage.articlesRepository.SaveArticle(article);
                    article = storage.articlesRepository.GetArticleByTitle(article.Title);
                    var attributesResult = attributeClient.Get(new AttributesRequest { Text = article.Text.Substring(0, Math.Min(article.Text.Length, 355)) });
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
                    //attribute.Id = Guid.NewGuid();
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
                    //attribute.Id = Guid.NewGuid();
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
                    //attribute.Id = Guid.NewGuid();
                    attribute.INN = attr.INN;
                    attribute.Geo = attr.Geo;
                }
                attribute.Owners.Add(article);
                article.OrganizationAttributes.Add(attribute);
                storage.organizationAttributesRepository.SaveOrganizationAttribute(attribute);
            }
        }
    }
}

﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Domain.Entities;
using WebCrawler.Domain.Repositories.Interfaces;

namespace WebCrawler.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticlesRepository repository;

        public ArticleController(IArticlesRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Show main (Index) page with articles.
        /// </summary>
        /// <param name="id">Id of a specific article.</param>
        /// <returns></returns>
        public ActionResult Index(Guid id)
        {
            if (id != default)
            {
                return View("Show", Storage.articlesRepository.GetArticleById(id));
            }
            //ViewBag.TextField = dataManager.TextFields.GetTextFieldByCodeWord("PageServices");
            //ViewBag.CurrentPageNumber = num ?? 0;
            //ViewBag.AmountOfPages = (dataManager.Articles.GetAmountOfArticles() - 1) / pageSize;

            //int page = num ?? 0;
            //if (Request.Headers["x-requested-with"] == "XMLHttpRequest")
            //{
            //    return PartialView("_Items", GetItemsPage(page));
            //}
            return View(Storage.articlesRepository.GetArticles());
        }

        public ActionResult Search(string template)
        {
            if (!string.IsNullOrWhiteSpace(template))
            {
                ViewBag.searchTemplate = template;
                return View(Storage.articlesRepository.GetArticlesByTemplate(template));
            }
            //ViewBag.TextField = dataManager.TextFields.GetTextFieldByCodeWord("PageServices");
            //ViewBag.CurrentPageNumber = num ?? 0;
            //ViewBag.AmountOfPages = (dataManager.Articles.GetAmountOfArticles() - 1) / pageSize;

            //int page = num ?? 0;
            //if (Request.Headers["x-requested-with"] == "XMLHttpRequest")
            //{
            //    return PartialView("_Items", GetItemsPage(page));
            //}
            return View(Enumerable.Empty<Article>().AsQueryable());
        }

        public ActionResult AllAttributes()
        {
            return View();
        }

        public ActionResult Attribute(ArticleAttribute attribute)
        {
            if (attribute != default)
            {
                return View(attribute);
            }
            //ViewBag.TextField = dataManager.TextFields.GetTextFieldByCodeWord("PageServices");
            //ViewBag.CurrentPageNumber = num ?? 0;
            //ViewBag.AmountOfPages = (dataManager.Articles.GetAmountOfArticles() - 1) / pageSize;

            //int page = num ?? 0;
            //if (Request.Headers["x-requested-with"] == "XMLHttpRequest")
            //{
            //    return PartialView("_Items", GetItemsPage(page));
            //}
            return View();
        }

        public ActionResult PAttribute(Guid id)
        {
            return View(Storage.personAttributesRepository.GetPersonAttributeById(id));
        }


        public ActionResult GAttribute(Guid id)
        {
            return View(Storage.geoAttributesRepository.GetGeoAttributeById(id));
        }


        public ActionResult OAttribute(Guid id)
        {
            return View(Storage.organizationAttributesRepository.GetOrganizationAttributeById(id));
        }
    }
}

using Microsoft.AspNetCore.Mvc;
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
        private readonly Storage storage;

        public ArticleController(Storage storage)
        {
            this.storage = storage;
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
                return View("Show", storage.articlesRepository.GetArticleById(id));
            }
            
            return View(storage.articlesRepository.GetArticles());
        }

        public ActionResult Search(string template)
        {
            if (!string.IsNullOrWhiteSpace(template))
            {
                ViewBag.searchTemplate = template;
                return View(storage.articlesRepository.GetArticlesByTemplate(template));
            }

            return View(Enumerable.Empty<Article>().AsQueryable());
        }

        public ActionResult AllAttributes()
        {
            return View(storage);
        }

        public ActionResult Attribute(ArticleAttribute attribute)
        {
            if (attribute != default)
            {
                return View(attribute);
            }
            
            return View();
        }

        public ActionResult PAttribute(Guid id)
        {
            return View(storage.personAttributesRepository.GetPersonAttributeById(id));
        }


        public ActionResult GAttribute(Guid id)
        {
            return View(storage.geoAttributesRepository.GetGeoAttributeById(id));
        }


        public ActionResult OAttribute(Guid id)
        {
            return View(storage.organizationAttributesRepository.GetOrganizationAttributeById(id));
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Domain.Entities;

namespace WebCrawler.Controllers
{
    public class ArticleController : Controller
    {
        /// <summary>
        /// Show main (Index) page with articles.
        /// </summary>
        /// <param name="id">Id of a specific article.</param>
        /// <returns></returns>
        public ActionResult Index(Guid id)
        {
            if (id != default)
            {
                return View("Show", Storage.Articles.FirstOrDefault(article => article.Id == id));
            }
            //ViewBag.TextField = dataManager.TextFields.GetTextFieldByCodeWord("PageServices");
            //ViewBag.CurrentPageNumber = num ?? 0;
            //ViewBag.AmountOfPages = (dataManager.Articles.GetAmountOfArticles() - 1) / pageSize;

            //int page = num ?? 0;
            //if (Request.Headers["x-requested-with"] == "XMLHttpRequest")
            //{
            //    return PartialView("_Items", GetItemsPage(page));
            //}
            return View(Storage.Articles);
        }
    }
}

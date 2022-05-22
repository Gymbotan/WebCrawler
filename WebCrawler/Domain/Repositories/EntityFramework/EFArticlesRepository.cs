using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebCrawler.Domain.Repositories.Interfaces;
using WebCrawler.Domain.Entities;

namespace WebCrawler.Domain.Repositories.EntityFramework
{
    /// <summary>
    /// Entity Framework realization of IArticlesRepository interface.
    /// </summary>
    public class EFArticlesRepository : IArticlesRepository
    {
        private readonly AppDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="EFArticlesRepository"/> class.
        /// </summary>
        /// <param name="context">DB context.</param>
        public EFArticlesRepository(AppDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Get all the articles.
        /// </summary>
        /// <returns>All the articles.</returns>
        public IQueryable<Article> GetArticles()
        {
            return context.Articles;
        }

        /// <summary>
        /// Save article.
        /// </summary>
        /// <param name="entity">Article that should be saved.</param>
        public void SaveArticle(Article entity)
        {
            if (entity.Id == default)
            {
                context.Entry(entity).State = EntityState.Added;
            }
            else
            {
                context.Entry(entity).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        ///// <summary>
        ///// Delete article with choosen id.
        ///// </summary>
        ///// <param name="id">Id of the article.</param>
        //public void DeleteArticle(Guid id)
        //{
        //    context.Articles.Remove(new Article { Id = id });
        //    context.SaveChanges();
        //}

        /// <summary>
        /// Get specific article with choosen id.
        /// </summary>
        /// <param name="id">Id of the article.</param>
        /// <returns>Article with choosen id.</returns>
        public Article GetArticleById(Guid id)
        {
            return context.Articles.FirstOrDefault(x => x.Id == id);
        }

        ///// <summary>
        ///// Get last N articles.
        ///// </summary>
        ///// <param name="amount"></param>
        ///// <returns></returns>
        //public IQueryable<Article> GetLastArticles(int amount)
        //{
        //    return context.Articles.OrderByDescending(x => x.DateAdded).Take(amount);
        //}

        /// <summary>
        /// Get amount of articles in the database.
        /// </summary>
        /// <returns>Amount of articles.</returns>
        public int GetAmountOfArticles()
        {
            return context.Articles.Count();
        }

        public IQueryable<Article> GetArticlesByTemplate(string template)
        {
            return context.Articles.Where(article => article.Text.Contains(template, StringComparison.OrdinalIgnoreCase)).Select(x => x);
        }

        public bool Contains(Article entity)
        {
            return context.Articles.Any(article => article.Title.ToLower() == entity.Title.ToLower());
        }
    }
}
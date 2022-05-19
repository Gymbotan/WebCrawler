using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Domain.Entities;

namespace WebCrawler.Domain.Repositories.Interfaces
{
    /// <summary>
    /// Interface of a repository for articles.
    /// </summary>
    public interface IArticlesRepository
    {
        /// <summary>
        /// Get all the articles.
        /// </summary>
        /// <returns>All the articles.</returns>
        IQueryable<Article> GetArticles();

        /// <summary>
        /// Get specific article with choosen id.
        /// </summary>
        /// <param name="id">Id of the article.</param>
        /// <returns>Article with choosen id.</returns>
        Article GetArticleById(Guid id);

        /// <summary>
        /// Save article.
        /// </summary>
        /// <param name="entity">Article that should be saved.</param>
        void SaveArticle(Article entity);

        ///// <summary>
        ///// Delete article with choosen id.
        ///// </summary>
        ///// <param name="id">Id of the article.</param>
        //void DeleteArticle(Guid id);

        /// <summary>
        /// Get amount of articles in the database.
        /// </summary>
        /// <returns>Amount of articles.</returns>
        public int GetAmountOfArticles();
    }
}
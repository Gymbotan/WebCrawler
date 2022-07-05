using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Domain.Entities;
using WebCrawler.Domain.Repositories.Interfaces;

namespace WebCrawler.Domain.Repositories.MemoryRepository
{
    public class MemoryArticlesRepository : IArticlesRepository
    {
        private readonly List<Article> Articles = new();

        public bool Contains(Article entity)
        {
            return Articles.Find(article => article.Title.ToLower() == entity.Title.ToLower()) != null;
        }

        public int GetAmountOfArticles()
        {
            return Articles.Count;
        }

        public Article GetArticleById(Guid id)
        {
            return Articles.FirstOrDefault(x => x.Id == id);
        }

        public Article GetArticleByTitle(string title)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Article> GetArticles()
        {
            return Articles.AsQueryable();
        }

        public IQueryable<Article> GetArticlesByTemplate(string template)
        {
            return Articles.Where(article => article.Text.Contains(template, StringComparison.OrdinalIgnoreCase)).Select(x => x).AsQueryable();
        }

        public void SaveArticle(Article entity)
        {
            Articles.Add(entity);
        }
    }
}

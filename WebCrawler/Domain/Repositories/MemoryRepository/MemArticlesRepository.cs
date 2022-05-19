using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Domain.Entities;
using WebCrawler.Domain.Repositories.Interfaces;

namespace WebCrawler.Domain.Repositories.MemoryRepository
{
    public class MemArticlesRepository : IArticlesRepository
    {
        public int GetAmountOfArticles()
        {
            throw new NotImplementedException();
        }

        public Article GetArticleById(Guid id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Article> GetArticles()
        {
            throw new NotImplementedException();
        }

        public void SaveArticle(Article entity)
        {
            throw new NotImplementedException();
        }
    }
}

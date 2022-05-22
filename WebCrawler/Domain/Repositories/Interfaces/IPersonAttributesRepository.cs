using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Domain.Entities;

namespace WebCrawler.Domain.Repositories.Interfaces
{
    public interface IPersonAttributesRepository
    {
        public void SavePersonAtribute(PersonAttribute attribute);

        public IQueryable<Article> GetConnectedArticles(PersonAttribute attribute);

        public bool Contains(PersonAttribute attribute);

        public PersonAttribute GetPersonAttributeByFIO(PersonAttribute attribute);

        public PersonAttribute GetPersonAttributeById(Guid id);

        public IQueryable<PersonAttribute> GetPersonAttributes();
    }
}

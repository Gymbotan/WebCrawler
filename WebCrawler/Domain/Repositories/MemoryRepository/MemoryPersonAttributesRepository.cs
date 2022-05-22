using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Domain.Entities;
using WebCrawler.Domain.Repositories.Interfaces;

namespace WebCrawler.Domain.Repositories.MemoryRepository
{
    public class MemoryPersonAttributesRepository : IPersonAttributesRepository
    {
        private readonly List<PersonAttribute> personAttributes = new();

        public bool Contains(PersonAttribute attribute)
        {
            return personAttributes.Find(a => (a.FirstName == attribute.FirstName) &&
                (a.LastName == attribute.LastName) && (a.MiddleName == attribute.MiddleName)) != null;
        }

        public IQueryable<Article> GetConnectedArticles(PersonAttribute attribute)
        {
            return attribute.Owners.AsQueryable();
        }

        public PersonAttribute GetPersonAttributeByFIO(PersonAttribute attribute)
        {
            return personAttributes.Find(a => (a.FirstName == attribute.FirstName) &&
                (a.LastName == attribute.LastName) && (a.MiddleName == attribute.MiddleName));
        }

        public PersonAttribute GetPersonAttributeById(Guid id)
        {
            return personAttributes.Find(a => a.Id == id);
        }

        public IQueryable<PersonAttribute> GetPersonAttributes()
        {
            return personAttributes.AsQueryable();
        }

        public void SavePersonAtribute(PersonAttribute attribute)
        {
            if (personAttributes.Find(a => a.Id == attribute.Id) is null)
            {
                personAttributes.Add(attribute);
            }
            else
            {
                personAttributes.Remove(attribute);
                personAttributes.Add(attribute);
            }
        }
    }
}

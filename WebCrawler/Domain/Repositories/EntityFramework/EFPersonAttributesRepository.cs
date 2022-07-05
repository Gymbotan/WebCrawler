using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Domain.Entities;
using WebCrawler.Domain.Repositories.Interfaces;

namespace WebCrawler.Domain.Repositories.EntityFramework
{
    public class EFPersonAttributesRepository : IPersonAttributesRepository
    {
        private readonly AppDbContext context;

        public EFPersonAttributesRepository(AppDbContext context)
        {
            this.context = context;
        }

        public bool Contains(PersonAttribute attribute)
        {
            return context.PersonAttributes.FirstOrDefault(a => (a.FirstName == attribute.FirstName) &&
                (a.LastName == attribute.LastName) && (a.MiddleName == attribute.MiddleName)) != null;
        }

        public IQueryable<Article> GetConnectedArticles(PersonAttribute attribute)
        {
            return context.PersonAttributes.Include(a => a.Owners)
                .FirstOrDefault(a => (a.FirstName == attribute.FirstName) &&
                (a.LastName == attribute.LastName) && (a.MiddleName == attribute.MiddleName)).Owners.AsQueryable();
        }

        public PersonAttribute GetPersonAttributeByFIO(PersonAttribute attribute)
        {
            return context.PersonAttributes.Include(a => a.Owners)
                .FirstOrDefault(a => (a.FirstName == attribute.FirstName) &&
                (a.LastName == attribute.LastName) && (a.MiddleName == attribute.MiddleName));
        }

        public PersonAttribute GetPersonAttributeById(Guid id)
        {
            return context.PersonAttributes.Include(a => a.Owners)
                .FirstOrDefault(x => x.Id == id);
        }

        public IQueryable<PersonAttribute> GetPersonAttributes()
        {
            return context.PersonAttributes.Include(a => a.Owners);
        }

        public void SavePersonAtribute(PersonAttribute attribute)
        {
            if (attribute.Id == default)
            {
                context.Entry(attribute).State = EntityState.Added;
            }
            else
            {
                context.Entry(attribute).State = EntityState.Modified;
            }
            context.SaveChanges();
        }
    }
}

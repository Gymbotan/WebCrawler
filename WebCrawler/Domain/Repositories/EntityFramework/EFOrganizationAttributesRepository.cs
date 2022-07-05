using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Domain.Entities;
using WebCrawler.Domain.Repositories.Interfaces;

namespace WebCrawler.Domain.Repositories.EntityFramework
{
    public class EFOrganizationAttributesRepository : IOrganizationAttributesRepository
    {
        private readonly AppDbContext context;

        public EFOrganizationAttributesRepository(AppDbContext context)
        {
            this.context = context;
        }

        public bool Contains(OrganizationAttribute attribute)
        {
            return context.OrganizationAttributes.FirstOrDefault(a => (a.Name == attribute.Name) && (a.Type == attribute.Type)) != null;
        }

        public IQueryable<Article> GetConnectedArticles(OrganizationAttribute attribute)
        {
            return context.OrganizationAttributes.Include(a => a.Owners)
                .FirstOrDefault(a => (a.Name == attribute.Name) && (a.Type == attribute.Type)).Owners.AsQueryable();
        }

        public OrganizationAttribute GetOrganizationAttributeById(Guid id)
        {
            return context.OrganizationAttributes.Include(a => a.Owners)
                .FirstOrDefault(x => x.Id == id);
        }

        public OrganizationAttribute GetOrganizationAttributeByName(OrganizationAttribute attribute)
        {
            return context.OrganizationAttributes.Include(a => a.Owners)
                .FirstOrDefault(a => (a.Name == attribute.Name) && (a.Type == attribute.Type));
        }

        public IQueryable<OrganizationAttribute> GetOrganizationAttributes()
        {
            return context.OrganizationAttributes.Include(a => a.Owners);
        }

        public void SaveOrganizationAttribute(OrganizationAttribute attribute)
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

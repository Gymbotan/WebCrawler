using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Domain.Entities;
using WebCrawler.Domain.Repositories.Interfaces;

namespace WebCrawler.Domain.Repositories.MemoryRepository
{
    public class MemoryOrganizationAttributesRepository : IOrganizationAttributesRepository
    {
        private readonly List<OrganizationAttribute> organizationAttributes = new();

        public bool Contains(OrganizationAttribute attribute)
        {
            return organizationAttributes.Find(a => (a.Name == attribute.Name) && (a.Type == attribute.Type)) != null;
        }

        public IQueryable<Article> GetConnectedArticles(OrganizationAttribute attribute)
        {
            return attribute.Owners.AsQueryable();
        }

        public OrganizationAttribute GetOrganizationAttributeById(Guid id)
        {
            return organizationAttributes.Find(a => a.Id == id);
        }

        public OrganizationAttribute GetOrganizationAttributeByName(OrganizationAttribute attribute)
        {
            return organizationAttributes.Find(a => (a.Name == attribute.Name) && (a.Type == attribute.Type));
        }

        public IQueryable<OrganizationAttribute> GetOrganizationAttributes()
        {
            return organizationAttributes.AsQueryable();
        }

        public void SaveOrganizationAttribute(OrganizationAttribute attribute)
        {
            if (organizationAttributes.Find(a => a.Id == attribute.Id) is null)
            {
                organizationAttributes.Add(attribute);
            }
            else
            {
                organizationAttributes.Remove(attribute);
                organizationAttributes.Add(attribute);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Domain.Entities;

namespace WebCrawler.Domain.Repositories.Interfaces
{
    public interface IOrganizationAttributesRepository
    {
        public void SaveOrganizationAttribute(OrganizationAttribute attribute);

        public IQueryable<Article> GetConnectedArticles(OrganizationAttribute attribute);

        public bool Contains(OrganizationAttribute attribute);

        public OrganizationAttribute GetOrganizationAttributeByName(OrganizationAttribute attribute);

        public OrganizationAttribute GetOrganizationAttributeById(Guid id);

        public IQueryable<OrganizationAttribute> GetOrganizationAttributes();
    }
}

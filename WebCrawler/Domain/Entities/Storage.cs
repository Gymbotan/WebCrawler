using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Domain.Repositories.Interfaces;
using WebCrawler.Domain.Repositories.MemoryRepository;

namespace WebCrawler.Domain.Entities
{
    public class Storage
    {
        public readonly IArticlesRepository articlesRepository; // = new MemoryArticlesRepository();

        public readonly IPersonAttributesRepository personAttributesRepository; // = new MemoryPersonAttributesRepository();

        public readonly IGeoAttributesRepository geoAttributesRepository; // = new MemoryGeoAttributesRepository();

        public readonly IOrganizationAttributesRepository organizationAttributesRepository; // = new MemoryOrganizationAttributesRepository();

        public Storage(IArticlesRepository articlesRepository, IPersonAttributesRepository personAttributesRepository, IGeoAttributesRepository geoAttributesRepository, IOrganizationAttributesRepository organizationAttributesRepository)
        {
            this.articlesRepository = articlesRepository;
            this.personAttributesRepository = personAttributesRepository;
            this.geoAttributesRepository = geoAttributesRepository;
            this.organizationAttributesRepository = organizationAttributesRepository;
        }
    }
}

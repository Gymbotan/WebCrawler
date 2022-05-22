using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Domain.Repositories.Interfaces;
using WebCrawler.Domain.Repositories.MemoryRepository;

namespace WebCrawler.Domain.Entities
{
    public static class Storage
    {
        //public readonly static List<Article> Articles = new();
        public readonly static IArticlesRepository articlesRepository = new MemoryArticlesRepository();

        public readonly static IPersonAttributesRepository personAttributesRepository = new MemoryPersonAttributesRepository();

        public readonly static IGeoAttributesRepository geoAttributesRepository = new MemoryGeoAttributesRepository();

        public readonly static IOrganizationAttributesRepository organizationAttributesRepository = new MemoryOrganizationAttributesRepository();
    }
}

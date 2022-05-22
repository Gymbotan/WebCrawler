using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Domain.Entities;

namespace WebCrawler.Domain.Repositories.Interfaces
{
    public interface IGeoAttributesRepository
    {
        public void SaveGeoAtribute(GeoAttribute attribute);

        public IQueryable<Article> GetConnectedArticles(GeoAttribute attribute);

        public bool Contains(GeoAttribute attribute);

        public GeoAttribute GetGeoAttributeByNameAndType(GeoAttribute attribute);

        public GeoAttribute GetGeoAttributeById(Guid id);

        public IQueryable<GeoAttribute> GetGeoAttributes();
    }
}

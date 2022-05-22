using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Domain.Entities;
using WebCrawler.Domain.Repositories.Interfaces;

namespace WebCrawler.Domain.Repositories.MemoryRepository
{
    public class MemoryGeoAttributesRepository : IGeoAttributesRepository
    {
        private readonly List<GeoAttribute> geoAttributes = new();

        public bool Contains(GeoAttribute attribute)
        {
            return geoAttributes.Find(a => (a.Name == attribute.Name) && (a.Type == attribute.Type)) != null;
        }

        public IQueryable<Article> GetConnectedArticles(GeoAttribute attribute)
        {
            return attribute.Owners.AsQueryable();
        }

        public GeoAttribute GetGeoAttributeById(Guid id)
        {
            return geoAttributes.Find(a => a.Id == id);
        }

        public GeoAttribute GetGeoAttributeByNameAndType(GeoAttribute attribute)
        {
            return geoAttributes.Find(a => (a.Name == attribute.Name) && (a.Type == attribute.Type));
        }

        public IQueryable<GeoAttribute> GetGeoAttributes()
        {
            return geoAttributes.AsQueryable();
        }

        public void SaveGeoAtribute(GeoAttribute attribute)
        {
            if (geoAttributes.Find(a => a.Id == attribute.Id) is null)
            {
                geoAttributes.Add(attribute);
            }
            else
            {
                geoAttributes.Remove(attribute);
                geoAttributes.Add(attribute);
            }
        }
    }
}

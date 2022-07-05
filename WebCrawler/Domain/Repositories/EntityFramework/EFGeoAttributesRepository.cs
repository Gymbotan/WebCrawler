using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Domain.Entities;
using WebCrawler.Domain.Repositories.Interfaces;

namespace WebCrawler.Domain.Repositories.EntityFramework
{
    public class EFGeoAttributesRepository : IGeoAttributesRepository
    {
        private readonly AppDbContext context;

        public EFGeoAttributesRepository(AppDbContext context)
        {
            this.context = context;
        }

        public bool Contains(GeoAttribute attribute)
        {
            return context.GeoAttributes.Include(a => a.Owners).FirstOrDefault(a => (a.Name == attribute.Name) && (a.Type == attribute.Type)) != null;
        }

        public IQueryable<Article> GetConnectedArticles(GeoAttribute attribute)
        {
            return context.GeoAttributes.Include(a => a.Owners)
                .FirstOrDefault(a => (a.Name == attribute.Name) && (a.Type == attribute.Type)).Owners.AsQueryable();
        }

        public GeoAttribute GetGeoAttributeById(Guid id)
        {
            return context.GeoAttributes.Include(a => a.Owners)
                .FirstOrDefault(x => x.Id == id);
        }

        public GeoAttribute GetGeoAttributeByNameAndType(GeoAttribute attribute)
        {
            return context.GeoAttributes.FirstOrDefault(a => (a.Name == attribute.Name) && (a.Type == attribute.Type));
        }

        public IQueryable<GeoAttribute> GetGeoAttributes()
        {
            return context.GeoAttributes.Include(a => a.Owners);
        }

        public void SaveGeoAtribute(GeoAttribute attribute)
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

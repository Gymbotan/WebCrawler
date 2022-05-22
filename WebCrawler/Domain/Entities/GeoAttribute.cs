using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebCrawler.Domain.Entities
{
    public class GeoAttribute : ArticleAttribute
    {
        public GeoAttribute()
        {
            this.Owners = new();
        }

        public string Type { get; set; }

        public string Name { get; set; }

        public string Alpha2 { get; set; }
    }
}

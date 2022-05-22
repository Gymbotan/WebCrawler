using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebCrawler.Domain.Entities
{
    public class OrganizationAttribute : ArticleAttribute
    {
        public OrganizationAttribute()
        {
            this.Owners = new();
        }

        public string Type { get; set; }

        public string Name { get; set; }

        public string INN { get; set; }

        public string Geo { get; set; }
    }
}

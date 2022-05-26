using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCrawler.Domain.Entities
{
    public class TextAttributes
    {
        public TextAttributes()
        {
            PersonalAttributes = new();
            GeoAttributes = new();
            OrganizationAttributes = new();
        }
        public List<PersonAttribute> PersonalAttributes { get; set; }
        public List<GeoAttribute> GeoAttributes { get; set; }
        public List<OrganizationAttribute> OrganizationAttributes { get; set; }
    }
}

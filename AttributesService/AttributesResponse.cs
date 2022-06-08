using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AttributesService.Classes;

namespace AttributesService
{
    public class AttributesResponse
    {
        public AttributesResponse()
        {
            PersonalAttributes = new();
            GeoAttributes = new();
            OrganizationAttributes = new();
        }
        public List<PersAttribute> PersonalAttributes { get; set; }
        public List<GeogrAttribute> GeoAttributes { get; set; }
        public List<OrgAttribute> OrganizationAttributes { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AttributesService.Classes
{
    public class GeogrAttribute
    {       
        public string Type { get; set; }

        public string Name { get; set; }

        public string Alpha2 { get; set; }
    }
}

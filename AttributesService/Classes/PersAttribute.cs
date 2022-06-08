using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AttributesService.Classes
{
    public class PersAttribute
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public int? Age { get; set; }

        public bool? Gender { get; set; }
    }
}

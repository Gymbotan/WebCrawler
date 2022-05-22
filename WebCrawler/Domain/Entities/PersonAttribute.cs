using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebCrawler.Domain.Entities
{
    public class PersonAttribute : ArticleAttribute
    {
        public PersonAttribute()
        {
            this.Owners = new();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public int? Age { get; set; }

        public bool? Gender { get; set; }
    }
}

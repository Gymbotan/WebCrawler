using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebCrawler.Domain.Entities
{
    public class ArticleAttribute
    {
        [Required]
        public Guid Id { get; set; }

        public List<Article> Owners { get; set; }
    }
}

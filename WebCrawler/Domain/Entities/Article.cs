using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebCrawler.Domain.Entities
{
    public class Article
    {
        public Article()
        {
            //Attributes = new();
            PersonAttributes = new();
            GeoAttributes = new();
            OrganizationAttributes = new();
        }

        /// <summary>
        /// Entity's id.
        /// </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// Title of the article.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Full HTML text of page.
        /// </summary>
        public string FullText { get; set; }

        /// <summary>
        /// Text of the article.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Url of the article.
        /// </summary>
        public Uri Url { get; set; }

        /// <summary>
        /// Date of the article.
        /// </summary>
        public DateTime Date { get; set; }

        ///// <summary>
        ///// Article's attributes.
        ///// </summary>
        //public List<string> Attributes { get; set; }

        /// <summary>
        /// Article's person attributes.
        /// </summary>
        public List<PersonAttribute> PersonAttributes { get; set; }

        /// <summary>
        /// Article's geo attributes.
        /// </summary>
        public List<GeoAttribute> GeoAttributes { get; set; }

        /// <summary>
        /// Article's organization attributes.
        /// </summary>
        public List<OrganizationAttribute> OrganizationAttributes { get; set; }

        public override string ToString()
        {
            string result = ($"Title: {Title}\nUrl: {Url}\nDate: {Date}\nText: {Text}\n");
            return result;
        }
    }
}

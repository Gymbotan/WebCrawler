using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebCrawler.Domain.Entities;

namespace WebCrawler.Domain
{
    public class AppDbContext:DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppDbContext"/> class.
        /// </summary>
        /// <param name="options">DbContext Options.</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        /// <summary>
        /// Table for Articles.
        /// </summary>
        public DbSet<Article> Articles { get; set; }

        /// <summary>
        /// DataBase creating.
        /// </summary>
        /// <param name="builder">Model Builder.</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}

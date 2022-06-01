using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funq;
using ServiceStack;

namespace WebCrawler.Services.Crawl
{
    public class Crawl1AppHost : AppHostBase
    {
        public Crawl1AppHost():base("Crawl Service", typeof(Crawl1Service).Assembly)
        {
        }

        public override void Configure(Container container)
        {
        }
    }
}

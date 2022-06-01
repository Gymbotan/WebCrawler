using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funq;
using ServiceStack;

namespace CrawlService
{
    public class CrawlAppHost : AppHostBase
    {
        public CrawlAppHost() : base("Crawl Service", typeof(CrawlService).Assembly)
        {
        }

        public override void Configure(Container container)
        {
        }
    }
}

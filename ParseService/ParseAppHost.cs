using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funq;
using ServiceStack;

namespace ParseService
{
    public class ParseAppHost : AppHostBase
    {
        public ParseAppHost() : base("Parse Service", typeof(ParseService).Assembly)
        {
        }

        public override void Configure(Container container)
        {
        }
    }
}

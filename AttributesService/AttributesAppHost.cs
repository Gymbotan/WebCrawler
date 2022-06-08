using Funq;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttributesService
{
    public class AttributesAppHost : AppHostBase
    {
        public AttributesAppHost() : base("Attributes Service", typeof(AttributesService).Assembly)
        {
        }

        public override void Configure(Container container)
        {
        }
    }
}

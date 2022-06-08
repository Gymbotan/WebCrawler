using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceStack;

namespace AttributesService
{
    [Route("/attributes")]
    public class AttributesRequest : IReturn<AttributesResponse>
    {
        public string Text { get; set; }
    }
}

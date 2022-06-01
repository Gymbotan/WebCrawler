using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceStack;

namespace ParseService
{
    [Route("/parse")]
    public class ParseRequest : IReturn<ParseResponse>
    {
        public ParseRequest()
        {
        }

        public ParseRequest(string text)
        {
            Text = text;
        }

        public string Text { get; set; }
    }
}

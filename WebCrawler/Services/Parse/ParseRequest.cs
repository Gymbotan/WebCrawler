using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceStack;

namespace WebCrawler.Services.Parse
{
    [Route("/parse1")]
    public class Parse1Request :IReturn<Parse1Response>
    {
        public Parse1Request()
        {
        }

        public Parse1Request(string text)
        {
            Text = text;
        }

        public string Text { get; set; }
    }
}

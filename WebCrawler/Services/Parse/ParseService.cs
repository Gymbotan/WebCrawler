using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Domain.Parsers;

namespace WebCrawler.Services.Parse
{
    public class ParseService : Service
    {
        private readonly MyParser parser;

        public ParseService(MyParser parser)
        {
            this.parser = parser;
        }

        [AddHeader(ContentType = MimeTypes.Json)]
        public object Get(Parse1Request request)
        {
            var response = new Parse1Response();
            if (!string.IsNullOrWhiteSpace(request.Text))
            {
                var result = parser.Parse(request.Text);
                response.Title = result.Item1;
                response.Text = result.Item2;
                response.Date = result.Item3;
            }

            return response;
        }
    }
}

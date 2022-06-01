using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParseService.Parsers;

namespace ParseService
{
    public class ParseService : Service
    {
        private readonly MyServiceParser parser;

        public ParseService(MyServiceParser parser)
        {
            this.parser = parser;
        }

        [AddHeader(ContentType = MimeTypes.Json)]
        public object Get(ParseRequest request)
        {
            var response = new ParseResponse();
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

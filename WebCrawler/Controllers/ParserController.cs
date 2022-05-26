using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Domain.Entities;
using WebCrawler.Domain.Parsers;

namespace WebCrawler.Controllers
{
    [ApiController]
    [Route("Parser")]
    public class ParserController : ControllerBase
    {
        private readonly MyParser parser;

        public ParserController(MyParser parser)
        {
            this.parser = parser;
        }

        [HttpGet]
        public RawTextParams Get(string text)
        {
            return parser.Parse(text);
        }
    }
}

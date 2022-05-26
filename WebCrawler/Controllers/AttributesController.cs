using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Domain.AttributeFinder;
using WebCrawler.Domain.Entities;

namespace WebCrawler.Controllers
{
    [ApiController]
    [Route("Attributes")]
    public class AttributesController : ControllerBase
    {
        private readonly MyAttributeFinder finder;

        public AttributesController(MyAttributeFinder finder)
        {
            this.finder = finder;
        }

        [HttpGet]
        public TextAttributes Get(string text)
        {
            return finder.FindAttributes(text);
        }
    }
}

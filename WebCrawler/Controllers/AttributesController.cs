﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Domain.AttributeFinder;
using WebCrawler.Domain.Entities;

namespace WebCrawler.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AttributesController : ControllerBase
    {
        private readonly MyAttributeFinder finder;

        public AttributesController(MyAttributeFinder finder)
        {
            this.finder = finder;
        }

        [HttpGet]
        public (List<PersonAttribute>, List<GeoAttribute>, List<OrganizationAttribute>) Get(string text)
        {
            return finder.FindAttributes(text);
        }
    }
}

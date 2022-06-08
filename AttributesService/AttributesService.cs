using AttributesService.AttributeFinder;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttributesService
{
    public class AttributesService : Service
    {
        private readonly MyServiceAttributeFinder finder;

        public AttributesService(MyServiceAttributeFinder finder)
        {
            this.finder = finder;
        }

        [AddHeader(ContentType = MimeTypes.Json)]
        public async Task<object> Get(AttributesRequest request)
        {
            var response = new AttributesResponse();

            var result = finder.FindAttributes(request.Text);

            response.PersonalAttributes = result.Item1;
            response.GeoAttributes = result.Item2;
            response.OrganizationAttributes = result.Item3;

            return response;
        }
    }
}

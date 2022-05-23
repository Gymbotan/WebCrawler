using Pullenti.Ner;
using Pullenti.Ner.Geo;
using Pullenti.Ner.Org;
using Pullenti.Ner.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Domain.Entities;

namespace WebCrawler.Domain.AttributeFinder
{
    public class MyAttributeFinder
    {
        public (List<PersonAttribute>, List<GeoAttribute>, List<OrganizationAttribute>) FindAttributes(string text)
        {
            List<PersonAttribute> personalAttributes = new();
            List<GeoAttribute> geoAttributes = new();
            List<OrganizationAttribute> organizationAttributes = new();

            using (Processor processor = ProcessorService.CreateEmptyProcessor())
            {
                processor.AddAnalyzer(new PersonAnalyzer());
                processor.AddAnalyzer(new GeoAnalyzer());
                processor.AddAnalyzer(new OrganizationAnalyzer());

                AnalysisResult result = processor.Process(new SourceOfAnalysis(text));
                // получили выделенные сущности
                foreach (Referent entity in result.Entities)
                {
                    if (entity is PersonReferent)
                    {
                        SaveAsPersonalAttribute(personalAttributes, entity);
                    }

                    if (entity is GeoReferent)
                    {
                        SaveAsGeoAttribute(geoAttributes, entity);
                    }

                    if (entity is OrganizationReferent)
                    {
                        SaveAsOrganizationAttribute(organizationAttributes, entity);
                    }
                }
            }

            return (personalAttributes, geoAttributes, organizationAttributes);
        }

        private void SaveAsOrganizationAttribute(List<OrganizationAttribute> organizationAttributes, Referent entity)
        {
            OrganizationAttribute attribute = new();
            attribute.Name = (string)entity.GetSlotValue("NAME");
            attribute.Type = (string)entity.GetSlotValue("TYPE");
            attribute.Id = Guid.NewGuid();
            attribute.INN = (entity as OrganizationReferent).INN;
            attribute.Geo = entity.GetSlotValue("GEO")?.ToString();
            organizationAttributes.Add(attribute);
        }

        private void SaveAsGeoAttribute(List<GeoAttribute> geoAttributes, Referent entity)
        {
            GeoAttribute attribute = new();
            attribute.Name = (string)entity.GetSlotValue("NAME");
            attribute.Type = (string)entity.GetSlotValue("TYPE");
            attribute.Id = Guid.NewGuid();
            attribute.Alpha2 = (entity as GeoReferent).Alpha2;
            geoAttributes.Add(attribute);
        }

        private void SaveAsPersonalAttribute(List<PersonAttribute> personalAttributes, Referent entity)
        {
            PersonAttribute attribute = new();
            attribute.FirstName = (string)entity.GetSlotValue("FIRSTNAME");
            attribute.LastName = (string)entity.GetSlotValue("LASTNAME");
            attribute.MiddleName = (string)entity.GetSlotValue("MIDDLENAME");
            attribute.Id = Guid.NewGuid();
            attribute.Gender = (entity as PersonReferent).IsMale;
            if ((entity as PersonReferent).Age > 0)
            {
                attribute.Age = (entity as PersonReferent).Age;
            }
            personalAttributes.Add(attribute);
        }
    }
}

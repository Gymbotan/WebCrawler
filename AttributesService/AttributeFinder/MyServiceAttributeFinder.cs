using AttributesService.Classes;
using Pullenti.Ner;
using Pullenti.Ner.Geo;
using Pullenti.Ner.Org;
using Pullenti.Ner.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttributesService.AttributeFinder
{
    public class MyServiceAttributeFinder
    {
        public (List<PersAttribute>, List<GeogrAttribute>, List<OrgAttribute>) FindAttributes(string text)
        {
            List<PersAttribute> personalAttributes = new();
            List<GeogrAttribute> geoAttributes = new();
            List<OrgAttribute> organizationAttributes = new();
            //TextAttributes textAttributes = new();

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

        private void SaveAsOrganizationAttribute(List<OrgAttribute> organizationAttributes, Referent entity)
        {
            OrgAttribute attribute = new();
            attribute.Name = (string)entity.GetSlotValue("NAME");
            attribute.Type = (string)entity.GetSlotValue("TYPE");
            attribute.INN = (entity as OrganizationReferent).INN;
            attribute.Geo = entity.GetSlotValue("GEO")?.ToString();
            organizationAttributes.Add(attribute);
        }

        private void SaveAsGeoAttribute(List<GeogrAttribute> geoAttributes, Referent entity)
        {
            GeogrAttribute attribute = new();
            attribute.Name = (string)entity.GetSlotValue("NAME");
            attribute.Type = (string)entity.GetSlotValue("TYPE");
            attribute.Alpha2 = (entity as GeoReferent).Alpha2;
            geoAttributes.Add(attribute);
        }

        private void SaveAsPersonalAttribute(List<PersAttribute> personalAttributes, Referent entity)
        {
            PersAttribute attribute = new();
            attribute.FirstName = (string)entity.GetSlotValue("FIRSTNAME");
            attribute.LastName = (string)entity.GetSlotValue("LASTNAME");
            attribute.MiddleName = (string)entity.GetSlotValue("MIDDLENAME");
            attribute.Gender = (entity as PersonReferent).IsMale;
            if ((entity as PersonReferent).Age > 0)
            {
                attribute.Age = (entity as PersonReferent).Age;
            }
            personalAttributes.Add(attribute);
        }
    }
}

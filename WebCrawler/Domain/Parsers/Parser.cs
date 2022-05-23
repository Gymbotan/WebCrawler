using Pullenti.Ner;
using Pullenti.Ner.Geo;
using Pullenti.Ner.Org;
using Pullenti.Ner.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Domain.Entities;

namespace WebCrawler.Domain.Parsers
{
    public static class Parser
    {
        public static void Parse(Article article, string rawText)
        {
            if (article is null)
            {
                throw new ArgumentNullException(nameof(article));
            }

            if (string.IsNullOrWhiteSpace(rawText))
            {
                throw new ArgumentNullException(nameof(rawText));
            }

            article.Title = ParseTitle(rawText);

            article.Text = ParseText(rawText);

            article.Date = ParseDate(rawText);

            FindAttributes(article);
        }

        private static string ParseTitle(string rawText)
        {
            if (string.IsNullOrWhiteSpace(rawText))
            {
                throw new ArgumentNullException(nameof(rawText));
            }

            string flag = "<h1 class=\"entry-title\" itemprop=\"headline\">"; // title tag
            int position = rawText.IndexOf(flag);
            string title = rawText.Substring(position + 44);
            title = title.Remove(title.IndexOf("</h1>"));
            return ClearText(title);
        }

        private static string ParseText(string rawText)
        {
            if (string.IsNullOrWhiteSpace(rawText))
            {
                throw new ArgumentNullException(nameof(rawText));
            }

            string flag = "div class=\"entry-content\" itemprop=\"articleBody\""; // tags before text
            int position = rawText.IndexOf(flag);
            string text = rawText.Substring(position + 48);
            flag = "</script></div>"; // tags a bit closer
            position = text.IndexOf(flag);
            text = text.Remove(0, position + 17);
            position = text.IndexOf(">");

            text = text.Substring(position + 1); // cut off data before text
            text = text.Remove(text.IndexOf("</div>"));// cut off data after text
            return ClearText(text);
        }

        private static string ClearText(string initialText)
        {
            string text = initialText;
            while (text.Contains("figure")) // delete tags <figure> and it's content
            {
                int openingBracket = text.IndexOf("<figure");
                int closingBracket = text.IndexOf("</figure>");
                text = text.Remove(openingBracket, closingBracket - openingBracket + 9);
            }
            while (text.Contains("blockquote")) // delete tags <blockquote> and it's content
            {
                int openingBracket = text.IndexOf("<blockquote");
                int closingBracket = text.IndexOf("</blockquote>");
                text = text.Remove(openingBracket, closingBracket - openingBracket + 13);
            }
            while (text.Contains("<a")) // delete tags <a> and it's content
            {
                int openingBracket = text.IndexOf("<a");
                int closingBracket = text.IndexOf("</a>");
                text = text.Remove(openingBracket, closingBracket - openingBracket + 4);
            }
            while (text.Contains("<script>")) // delete tags <script> and it's content
            {
                int openingBracket = text.IndexOf("<script");
                int closingBracket = text.IndexOf("</script>");
                text = text.Remove(openingBracket, closingBracket - openingBracket + 9);
            }
            while (text.Contains("<")) // delete all other tags
            {
                int openingBracket = text.IndexOf("<");
                int closingBracket = text.IndexOf(">");
                text = text.Remove(openingBracket, closingBracket - openingBracket + 1);
            }
            while (text.Contains("&#171") || text.Contains("&#187") || text.Contains("&#8230")) // change some special symbols
            {
                text = text.Replace("&#171;", "\"");
                text = text.Replace("&#187;", "\"");
                text = text.Replace("&#8230;", "...");
            }
            return text;
        }

        private static DateTime ParseDate(string rawText)
        {
            if (string.IsNullOrWhiteSpace(rawText))
            {
                throw new ArgumentNullException(nameof(rawText));
            }

            string flag = "datePublished";
            int position = rawText.IndexOf(flag);
            string dateAsString = rawText.Substring(position + 25, 10);
            string[] dividedDate = dateAsString.Split('-');
            return new DateTime(int.Parse(dividedDate[0]), int.Parse(dividedDate[1]), int.Parse(dividedDate[2]));
        }

        private static void FindAttributes(Article article)
        {
            using (Processor processor = ProcessorService.CreateEmptyProcessor())
            {
                PersonAnalyzer anPer = new();
                GeoAnalyzer anGeo = new();
                OrganizationAnalyzer anOrg = new();

                processor.AddAnalyzer(anPer);
                processor.AddAnalyzer(anGeo);
                processor.AddAnalyzer(anOrg);

                AnalysisResult result = processor.Process(new SourceOfAnalysis(article.Text));
                // получили выделенные сущности
                foreach (Referent entity in result.Entities)
                {
                    if (entity is PersonReferent)
                    {
                        SaveAsPersonalAttribute(article, entity);
                    }

                    if (entity is GeoReferent)
                    {
                        SaveAsGeoAttribute(article, entity);
                    }

                    if (entity is OrganizationReferent)
                    {
                        SaveAsOrganizationAttribute(article, entity);
                    }
                }
            }
        }

        private static void SaveAsOrganizationAttribute(Article article, Referent entity)
        {
            OrganizationAttribute attribute = new();
            attribute.Name = (string)entity.GetSlotValue("NAME");
            attribute.Type = (string)entity.GetSlotValue("TYPE");
            if (Storage.organizationAttributesRepository.Contains(attribute))
            {
                attribute = Storage.organizationAttributesRepository.GetOrganizationAttributeByName(attribute);
            }
            else
            {
                attribute.Id = Guid.NewGuid();
                attribute.INN = (entity as OrganizationReferent).INN;
                attribute.Geo = (string)entity.GetSlotValue("GEO");
            }
            attribute.Owners.Add(article);
            article.OrganizationAttributes.Add(attribute);
            Storage.organizationAttributesRepository.SaveOrganizationAttribute(attribute);
        }

        private static void SaveAsGeoAttribute(Article article, Referent entity)
        {
            GeoAttribute attribute = new();
            attribute.Name = (string)entity.GetSlotValue("NAME");
            attribute.Type = (string)entity.GetSlotValue("TYPE");
            if (Storage.geoAttributesRepository.Contains(attribute))
            {
                attribute = Storage.geoAttributesRepository.GetGeoAttributeByNameAndType(attribute);
                attribute.Owners.Add(article);
            }
            else
            {
                attribute.Id = Guid.NewGuid();
                attribute.Alpha2 = (entity as GeoReferent).Alpha2;
                attribute.Owners.Add(article);
            }
            article.GeoAttributes.Add(attribute);
            Storage.geoAttributesRepository.SaveGeoAtribute(attribute);
        }

        private static void SaveAsPersonalAttribute(Article article, Referent entity)
        {
            PersonAttribute attribute = new();
            attribute.FirstName = (string)entity.GetSlotValue("FIRSTNAME");
            attribute.LastName = (string)entity.GetSlotValue("LASTNAME");
            attribute.MiddleName = (string)entity.GetSlotValue("MIDDLENAME");
            if (Storage.personAttributesRepository.Contains(attribute))
            {
                attribute = Storage.personAttributesRepository.GetPersonAttributeByFIO(attribute);
                attribute.Owners.Add(article);
            }
            else
            {
                attribute.Id = Guid.NewGuid();
                attribute.Gender = (entity as PersonReferent).IsMale;
                if ((entity as PersonReferent).Age > 0)
                {
                    attribute.Age = (entity as PersonReferent).Age;
                }
                attribute.Owners.Add(article);
            }
            article.PersonAttributes.Add(attribute);
            Storage.personAttributesRepository.SavePersonAtribute(attribute);
        }
    }
}

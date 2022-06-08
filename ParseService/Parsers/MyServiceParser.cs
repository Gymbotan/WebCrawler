using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParseService.Parsers
{
    public class MyServiceParser
    {
        public (string, string, DateTime) Parse(string rawText)
        {
            if (string.IsNullOrWhiteSpace(rawText))
            {
                throw new ArgumentNullException(nameof(rawText));
            }

            if (rawText.Contains("document.oncopy"))
            {
                rawText = rawText.Substring(rawText.IndexOf("document.oncopy"));
            }

            return (ParseTitle(rawText), ParseText(rawText), ParseDate(rawText));
        }

        private string ParseTitle(string rawText)
        {
            if (string.IsNullOrWhiteSpace(rawText))
            {
                throw new ArgumentNullException(nameof(rawText));
            }

            string flag = "<h1 class=\"entry-title\" itemprop=\"headline\">"; // title tag
            if (rawText.Contains(flag))
            {
                int position = rawText.IndexOf(flag);
                string title = rawText.Substring(position + 44);
                title = title.Remove(title.IndexOf("</h1>"));
                return ClearText(title);
            }
            else
            {
                return string.Empty;
            }
        }

        private string ParseText(string rawText)
        {
            if (string.IsNullOrWhiteSpace(rawText))
            {
                throw new ArgumentNullException(nameof(rawText));
            }

            string flag = "div class=\"entry-content\" itemprop=\"articleBody\""; // tags before text

            if (rawText.Contains(flag))
            {
                int position = rawText.IndexOf(flag);
                string text = rawText.Substring(position + 48);
                flag = "</script></div>"; // tags a bit closer to text
                position = text.IndexOf(flag);
                text = text.Remove(0, position + 17);
                position = text.IndexOf(">");
                text = text.Substring(position + 1); // cut off data before text
                text = text.Remove(text.IndexOf("</div>"));// cut off data after text
                return ClearText(text);
            }
            else
            {
                return string.Empty;
            }
        }

        private string ClearText(string initialText)
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

        private DateTime ParseDate(string rawText)
        {
            if (string.IsNullOrWhiteSpace(rawText))
            {
                throw new ArgumentNullException(nameof(rawText));
            }

            string flag = "datePublished";
            if (rawText.Contains(flag))
            {
                int position = rawText.IndexOf(flag);
                string dateAsString = rawText.Substring(position + 25, 10);
                string[] dividedDate = dateAsString.Split('-');
                return new DateTime(int.Parse(dividedDate[0]), int.Parse(dividedDate[1]), int.Parse(dividedDate[2]));
            }
            else
            {
                return DateTime.Now;
            }
        }
    }
}

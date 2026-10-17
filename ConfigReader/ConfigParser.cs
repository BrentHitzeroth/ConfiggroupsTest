using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ConfigReader
{
    public static class ConfigParser
    {
        public static List<ConfigEntry> Parse(string path)
        {
            var doc = XDocument.Load(path, LoadOptions.None);
            var result = new List<ConfigEntry>();
            if (doc.Root == null) return result;

            foreach (var add in doc.Root.Descendants("add"))
            {
                var key = (string)add.Attribute("key") ?? (string)add.Attribute("name");
                if (key == null) continue;
                var value = (string)add.Attribute("value") ?? (string)add.Attribute("connectionString")
                    ?? (string)add.Attribute("type");
                result.Add(new ConfigEntry
                {
                    Section = add.Parent?.Name.LocalName,
                    Key = key,
                    Value = value,
                    Comment = GetComment(add)
                });
            }
            return result;
        }

        // Joins the consecutive comment nodes directly above the element (whitespace ignored).
        private static string GetComment(XElement element)
        {
            var comments = new List<string>();
            var node = element.PreviousNode;
            while (node != null)
            {
                if (node is XComment c) comments.Insert(0, c.Value.Trim());
                else if (!(node is XText t && string.IsNullOrWhiteSpace(t.Value))) break;
                node = node.PreviousNode;
            }
            return string.Join(" ", comments.Where(s => s.Length > 0));
        }
    }
}

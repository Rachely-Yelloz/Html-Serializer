using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Html_Serializer
{
    public class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; }
        public Selector Parent { get; set; }
        public Selector Child { get; set; }

        public static Selector Convert(string query)
        {
            Selector root = null;
            Selector current = null;

            string[] degrees = query.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var degree in degrees)
            {
                Selector newSelector = new Selector();

                string[] parts = degree.Split(new[] { '#', '.' }, StringSplitOptions.RemoveEmptyEntries);

                int i = 0;
                if (HtmlHelper.Instance.HtmlTags.Any(t => t == parts[i]))
                {
                    newSelector.TagName = parts[i++];
                }

                if (degree.Contains("#"))
                {
                    newSelector.Id = parts[i++];
                }

                while (i < parts.Length)
                {
                    newSelector.Classes.Add(parts[i++]);
                }

                if (current == null)
                {
                    root = newSelector;
                }
                else
                {
                    current.Child = newSelector; 
                    newSelector.Parent = current; 
                }

                current = newSelector; 
            }

            return root;
        }
    }
}

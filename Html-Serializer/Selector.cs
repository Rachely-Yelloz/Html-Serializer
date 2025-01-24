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
            Selector current = new Selector(), root = current;
            string[] degree = query.Split(" ");
            if (degree.Length != 1)
            {

                for (int i = degree.Length - 1; i >= 0; i--)
                {
                    if (degree[i][0] == '#')
                        current.Id = degree[i].Substring(1);
                    else if (degree[i][0] == '.')

                        current.Classes = new List<string>() { degree[i].Substring(1) };

                    else
                    {
                        if (HtmlHelper.Instance.HtmlTags.Any(t => t == degree[i]))
                            current.TagName = degree[i];
                    }
                    current.Parent = new Selector() { Child = current };
                    current = current.Parent;
                }
            }
            else
            {
              

                string[] parts = query.Split(new char[] { '#', '.' }, StringSplitOptions.RemoveEmptyEntries);
                if(HtmlHelper.Instance.HtmlTags.Any(t=>t==parts[0]))

                root.TagName = parts[0]; // התג תמיד יהיה הראשון

                // חיפוש מזהה
                if (query.Contains("#"))
                {
                    root.Id = parts[1]; // המזהה יהיה השני אם קיים
                }

                // חיפוש מחלקות
                for (int i = 2; i < parts.Length; i++)
                {
                    root.Classes.Add(parts[i]); // שאר החלקים הם מחלקות
                }
            }

            return root;
        }
    }
}

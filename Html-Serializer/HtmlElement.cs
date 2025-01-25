using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Html_Serializer
{
    public class HtmlElement
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Attributes { get; set; }
        public List<string> Classes { get; set; }
        public string InnerHtml { get; set; }
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; }

        public HtmlElement(string name, string atr, HtmlElement parent)
        {
            Name = name;
            Classes = new List<string>();
            Attributes = new List<string>();
            var atrebuteList = new Regex("([^\\s]*?)=\"(.*?)\"")
             .Matches(atr.Substring(name.Length))
             .Cast<Match>().ToList()
             .Select(match => match.Value).ToList();
            foreach (var at in atrebuteList)
            {
                string aname = at.Substring(0, at.IndexOf('='));
                if (aname.ToLower() == "class")
                {

                    Classes.Add(aname.Substring(aname.IndexOf("=") + 1));
                }
                else if (aname.ToLower() == "id")
                {
                    Id = int.Parse(aname.Substring(aname.IndexOf("=") + 1));

                }
                else
                {
                    Attributes.Add(at);
                }
            }
            Parent = parent;
        }
        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> htmlElementsQueue = new Queue<HtmlElement>();
            if (this.Children != null)
                foreach (var child in this.Children)
                    htmlElementsQueue.Enqueue(child);
            while (htmlElementsQueue.Count > 0)
            {
                HtmlElement htmlElement = htmlElementsQueue.Dequeue();
                yield return htmlElement;
                if (htmlElement.Children != null)
                    foreach (var child in htmlElement.Children)
                        htmlElementsQueue.Enqueue(child);
            }
        }
        public IEnumerable<HtmlElement> Ancestors()
        {
            HtmlElement htmlElement = this.Parent;
            while (htmlElement != null)
            {
                yield return htmlElement;
                htmlElement = htmlElement.Parent;
            }
        }
    }
}


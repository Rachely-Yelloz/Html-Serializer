using Html_Serializer;
using System.Text.RegularExpressions;
using System.Threading.Channels;

static async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}
var html = await Load("https://www.malkabruk.co.il/");
var cleanHtml = new Regex("//s").Replace(html, "");
var htmlLines = new Regex("<(.*?)>").Split(cleanHtml).Where(s => s.Length > 0).ToArray();

var htmlElement = "<div id=\"my-id\" class=\"my-class-1 my-class-2\" width=\"100%\">text</div>";
var attributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(htmlElement);
HtmlElement root = new HtmlElement() { Name = "html" };
HtmlElement current = root;

for (int i = 0; i < htmlLines.Length; i++)
{
    if (htmlLines[i].StartsWith("/"))
    {

        if (htmlLines[i].Substring(1) == current.Name)
        {
            current = current.Parent;
        }
        else
        {
            if (current.InnerHtml == null)
            {
                current.InnerHtml = "";
            }
            current.InnerHtml += htmlLines[i];
        }

    }
    int spaceIndex = htmlLines[i].IndexOf(' ');
    string n;
    if (spaceIndex != -1)
    {
        n = htmlLines[i].Substring(0, spaceIndex);
    }
    else n = htmlLines[i];

    if (HtmlHelper.Instance.HtmlTags.Any(tags => tags == n))
    {
        HtmlElement newChild = new HtmlElement();
        //ביטוי רגולרי להכנסה של אטרביוט ועוד
        newChild.Attributes = new Regex("([^\\s]*?)=\"(.*?)\"")
            .Matches(htmlLines[i].Substring(n.Length))
            .Cast<Match>().ToList()
            .Select(match => match.Value).ToList();
        newChild.Name = n;
        newChild.Parent = current;
        if(current.Children==null)
        {
            current.Children = new List<HtmlElement>();
        }
        current.Children.Add(newChild);
        if (!HtmlHelper.Instance.HtmlVoidTags.Any(h => h == n))
            current = newChild;
    }
    else
    {
        if (current.InnerHtml == null)
            current.InnerHtml = "";
        current.InnerHtml += htmlLines[i];
    }

}

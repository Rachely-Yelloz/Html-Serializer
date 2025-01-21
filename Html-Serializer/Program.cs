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
var html=await Load("https://www.malkabruk.co.il/");
var cleanHtml = new Regex("//s").Replace(html, "");
var htmlLines = new Regex("<(.*?)>").Split(cleanHtml).Where(s => s.Length > 0).ToArray();

var htmlElement = "<div id=\"my-id\" class=\"my-class-1 my-class-2\" width=\"100%\">text</div>";
var attributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(htmlElement);
HtmlElement root = new HtmlElement();
HtmlElement current = root;

for (int i = 0; i < htmlLines.Length; i++)
{
    int spaceIndex = htmlLines[i].IndexOf(' ');
    if(spaceIndex != -1)
    {
        string n= htmlLines[i].Substring(0, spaceIndex);
        if(n== "html/")
        {
            //הגעתי לסוף HTML
        }
        else if (n.StartsWith('/'))
        {
            //סגירת תווית
            current = current.Parent;
        }
        else if (HtmlHelper.Instance.HtmlTags.Any(tags => tags == n)
            || HtmlHelper.Instance.HtmlVoidTags.Any(tags => tags == n))
        {
            HtmlElement newChild = new HtmlElement();
            //ביטוי רגולרי להכנסה של אטרביוט ועוד
            current.Children.Add(newChild);
        }
    }

}

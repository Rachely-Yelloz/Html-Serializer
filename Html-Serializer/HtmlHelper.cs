using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Html_Serializer
{
    public class HtmlHelper
    {
        private static HtmlHelper _instance= new HtmlHelper();
        public static HtmlHelper Instance=> _instance;
        public string[] HtmlTags { get; /*set;*/ }
        public string[] HtmlVoidTags { get;/* set;*/ }

        private HtmlHelper()
        {
            // קריאת תוכן הקובץ כטקסט
            var htmlTagsJson = File.ReadAllText("JSON-Files/HtmlTags.json");
            var htmlVoidTagsJson = File.ReadAllText("JSON-Files/HtmlVoidTags.json");

            // המרת תוכן JSON למערך באמצעות JsonSerializer
            HtmlTags = JsonSerializer.Deserialize<string[]>(htmlTagsJson);
            HtmlVoidTags = JsonSerializer.Deserialize<string[]>(htmlVoidTagsJson);
        }

    }
}

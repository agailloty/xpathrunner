using Cocona;
using HtmlAgilityPack;

var app = CoconaLiteApp.Create();
app.AddCommand("file", (string filepath, string xpath) => ApplyOnHTMLFile(filepath, xpath));
app.AddCommand("web", (string url) => Console.Write(RetrievePageFromWeb(url)));
app.Run();

void ApplyOnHTMLFile(string filepath, string xpath)
{
    var doc = new HtmlDocument();
    doc.Load(filepath);

    var results = doc.DocumentNode.SelectNodes(xpath);
    foreach (var result in results)
        Console.WriteLine(result.InnerText);
}

string RetrievePageFromWeb(string url)
{
    using var client = new HttpClient();
    return client.GetStringAsync(url).Result;
}

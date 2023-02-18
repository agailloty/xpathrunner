using Cocona;
using HtmlAgilityPack;
using System.Net;

var app = CoconaLiteApp.Create();
app.AddCommand("file", (string filepath, string xpath) => ApplyOnHTMLFile(filepath, xpath));
app.AddCommand("web", (string url) => Console.Write(RetrievePageFromWeb(url)));
app.Run();

// "//span[starts-with(@id, 'jobTitle')]/text()"
void ApplyOnHTMLFile(string filepath, string xpath)
{
    var doc = new HtmlDocument();
    doc.Load(filepath);

    var jobTitles = doc.DocumentNode.SelectNodes(xpath);
    foreach (var jobTitle in jobTitles)
        Console.WriteLine(jobTitle.InnerText);
}

string RetrievePageFromWeb(string url)
{
    using var client = new HttpClient();
    return client.GetStringAsync(url).Result;
}

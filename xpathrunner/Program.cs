using Cocona;
using HtmlAgilityPack;

var app = CoconaLiteApp.Create();
app.AddCommand((string filepath, string xpath) => ApplyOnHTMLFile(filepath, xpath));
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

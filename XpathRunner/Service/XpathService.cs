using System.Collections.Generic;
using HtmlAgilityPack;

namespace XpathRunner.Service;

public class XpathService
{
    public IList<string> ExtractHtmlContent(string filepath, string xpath)
    {
        if (string.IsNullOrEmpty(filepath) || string.IsNullOrEmpty(xpath))
            return new List<string>();
        
        var content = new List<string>();
        var doc = new HtmlDocument();
        doc.Load(filepath);

        var results = doc.DocumentNode.SelectNodes(xpath);
        if (results == null)
            return content;

        foreach (var result in results)
            content.Add(result.InnerText.Trim());
        return content;
    }
    
    public IList<string> ExtractHtmlContent(string[] filepaths, string xpath)
    {
        if (filepaths == null || filepaths.Length == 0 || string.IsNullOrEmpty(xpath))
            return new List<string>();
        
        var content = new List<string>();
        foreach (var filepath in filepaths)
        {
            var doc = new HtmlDocument();
            doc.Load(filepath);

            var results = doc.DocumentNode.SelectNodes(xpath);
            if (results == null)
                continue;

            foreach (var result in results)
                content.Add(result.InnerText.Trim());
        }
        return content;
    }
}
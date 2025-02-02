using System.Collections.Generic;
using System.Linq;
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

    public IList<List<string>> ExtractMultipleHtmlContent(string[] filepaths, string[] xpaths)
    {
        // keep only the paths and xpaths that are not empty
        var paths = filepaths.Where(path => !string.IsNullOrEmpty(path)).ToArray();
        var xpathsList = xpaths.Where(xpath => !string.IsNullOrEmpty(xpath)).ToArray();
        
        // if there are no paths or xpaths, return an empty list
        if (paths.Length == 0 || xpathsList.Length == 0)
            return new List<List<string>>();
        
        var results = new List<List<string>>();
        
        foreach (var filepath in paths)
        {
            var doc = new HtmlDocument();
            doc.Load(filepath);
            var content = EvaluateMultipleXpaths(doc, xpathsList);
            results.AddRange(content);
        }
        return results;
    }

    public IList<List<string>> EvaluateMultipleXpaths(HtmlDocument doc, IEnumerable<string> xpaths)
    {
        var results = new List<List<string>>();
        foreach (var xpath in xpaths)
        {
            var content = new List<string>();
            var resultsNode = doc.DocumentNode.SelectNodes(xpath);
            if (resultsNode == null)
            {
                results.Add(content);
                continue;
            }

            foreach (var result in resultsNode)
                content.Add(result.InnerText.Trim());
            results.Add(content);
        }
        return results;
    }

}
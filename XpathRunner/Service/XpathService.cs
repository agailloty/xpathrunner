using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace XpathRunner.Service;

public class XpathService
{
    public IList<ResultModel> ExtractMultipleHtmlContent(string[] filepaths, string[] xpaths)
    {
        // keep only the paths and xpaths that are not empty
        var paths = filepaths.Where(path => !string.IsNullOrEmpty(path)).ToArray();
        var xpathsList = xpaths.Where(xpath => !string.IsNullOrEmpty(xpath)).ToArray();
        
        // if there are no paths or xpaths, return an empty list
        if (paths.Length == 0 || xpathsList.Length == 0)
            return new List<ResultModel>();
        
        var results = new List<ResultModel>();
        
        foreach (var filepath in paths)
        {
            var doc = new HtmlDocument();
            doc.Load(filepath);
            var content = EvaluateMultipleXpaths(doc, xpathsList);
            results.AddRange(content);
        }
        
        // Get unique xpath expressions
        var uniqueXpaths = results.Select(x => x.ColumnName).Distinct().ToList();
        var uniqueResults = new List<ResultModel>();
        foreach (var xpath in uniqueXpaths)
        {
            var rows = results.Where(r => r.ColumnName == xpath).SelectMany(r => r.Rows).ToList();
            uniqueResults.Add(new ResultModel { ColumnName = xpath, Rows = rows });
                
        }
        
        return uniqueResults;
    }

    public IList<ResultModel> EvaluateMultipleXpaths(HtmlDocument doc, IEnumerable<string> xpaths)
    {
        var results = new List<ResultModel>();
        foreach (var xpath in xpaths)
        {
            var content = new ResultModel( ) { ColumnName = xpath };
            var resultsNode = doc.DocumentNode.SelectNodes(xpath);
            if (resultsNode == null)
            {
                results.Add(content);
                continue;
            }

            foreach (var result in resultsNode)
                content.Rows.Add(result.InnerText.Trim());
            results.Add(content);
        }
        return results;
    }

}
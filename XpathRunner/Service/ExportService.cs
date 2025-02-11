using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XpathRunner.Service;

public class ExportService
{
    private readonly DialogService _dialogService;

    public ExportService()
    {
        _dialogService = new DialogService();
    }
    public async Task SaveResultsToCsvAsync(IEnumerable<ResultModel> results)
    {
        var columns = ConcatenateResults(results.ToList());
        if (columns != null)
        {
            await _dialogService.ExportCSVFile(columns);
        }
    }
    
    private List<string> ConcatenateResults(IList<ResultModel> results)
    {
        int longestList = results.Max(x => x.Rows.Count);
        var resultList = new List<string>();
        int nColumns = results.Count;
        for (int i = 0; i < longestList; i++)
        {
            StringBuilder line = new();
            for (int j = 0; j < nColumns; j++)
            {
                if (results[j].Rows.Count <= i)
                {
                    line.Append(",");
                    continue;
                }
                line.Append($"\"{results[j].Rows[i]}\"");
                if (j < nColumns - 1)
                {
                    line.Append(",");
                }
            }
            resultList.Add(line.ToString());
        }
        results.ToList().ForEach(r => {r.ColumnName = r.ColumnName.Replace("/", "");});
        string colNames = results.Select(r => r.ColumnName).Aggregate((x, y) => $"{x},{y}");
        resultList.Insert(0, colNames);
        return resultList;
    }
}
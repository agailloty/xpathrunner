using System.Collections.Generic;
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
    public async Task SaveResultsToCsvAsync(IEnumerable<IList<string>> content)
    {
        StringBuilder lines = new();
        foreach (var line in content)
        {
            var result = string.Join(",", line);
            lines.AppendLine(result);
        }
        
        await _dialogService.ExportCSVFile(lines.ToString());
    }
    
    public async Task SaveResultsToCsvAsync(IEnumerable<ResultModel> results)
    {
        StringBuilder lines = new();
        StringBuilder columns = new();
        foreach (var result in results)
        {
            lines.Append(result.ColumnName);
            lines.AppendLine();
            foreach (var row in result.Rows)
            {
                lines.Append(row);
                lines.AppendLine();
            }
            
            columns.AppendJoin(";", $"{lines.ToString()}");
        }
        
        await _dialogService.ExportCSVFile(columns.ToString());
    }
}
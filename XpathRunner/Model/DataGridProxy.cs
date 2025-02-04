using System.Collections.Generic;
using System.Linq;

namespace XpathRunner.Model;

public class DataGridProxy
{
    public IReadOnlyList<string> Columns { get; } 
    public IReadOnlyList<IReadOnlyList<string>> Rows { get; }
    
    public DataGridProxy(IList<ResultModel> results)
    {
        Columns = results.Select(x => x.ColumnName).ToList();
        Rows = results.Select(x => x.Rows).ToList();
    }
}
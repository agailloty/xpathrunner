using System.Collections.Generic;

namespace XpathRunner;

public class ResultModel
{
    public string ColumnName { get; set; } = string.Empty;
    public List<string> Rows { get; set; } = new();
}
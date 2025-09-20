using CommunityToolkit.Mvvm.ComponentModel;

namespace XpathRunner.ViewModels;

public class XpathExpressionItem : ObservableObject
{
    private string? _xpathExpression;

    public string XpathExpression
    {
        get => _xpathExpression ?? string.Empty;
        set => SetProperty(ref _xpathExpression, value);
    }
}
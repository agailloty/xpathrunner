using CommunityToolkit.Mvvm.ComponentModel;

namespace XpathRunner.ViewModels;

public class XpathExpressionItem : ObservableObject
{
    private string _xpathExpression = string.Empty;

    public string XpathExpression
    {
        get => _xpathExpression;
        set => SetProperty(ref _xpathExpression, value);
    }
}
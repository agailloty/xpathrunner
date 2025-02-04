using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using XpathRunner.ViewModels;

namespace XpathRunner.Views;

public partial class XpathResults : UserControl
{
    public XpathResults()
    {
        InitializeComponent();
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        if (DataContext is MainWindowViewModel viewModel)
        {
            viewModel.ResultsChanged += UpdateDataGrid;
        }
    }

    private void UpdateDataGrid(object? sender, PropertyChangedEventArgs e)
    {
        BuildDataGrid();
    }

    private void BuildDataGrid()
    {
        ResultsDataGrid.Columns.Clear();
        if (DataContext is MainWindowViewModel viewModel)
        {
            foreach (var column in viewModel.DataGridProxy.Columns)
            {
                ResultsDataGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = column
                });
            }
        }
    }
}
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using XpathRunner.Service;

namespace XpathRunner.ViewModels;

public class MainWindowViewModel : ObservableObject
{
    private bool _isBusy;
    private readonly DialogService _dialogService = new();
    private string _filePath;
    private bool _isFileSelected;
    private string _xpathExpression;

    public MainWindowViewModel()
    {
        FilePickerCommand = new RelayCommand(async () =>
        {
            IsBusy = true;
            var files = await _dialogService.ShowFolderBrowserDialogAsync();
            FilePath = files?[0];
            IsBusy = false;
        });
        
        GetXpathResultsCommand = new RelayCommand(() =>
        {
            IsBusy = true;
            var xpathService = new XpathService();
            var results = xpathService.ExtractHtmlContent(FilePath, XpathExpression);
            XpathResults.Clear();
            foreach (var result in results)
            {
                XpathResults.Add(result);
            }
            IsBusy = false;
        });
    }

    #region  Public

    public string FilePath
    {
        get => _filePath;
        set
        {
            if (SetProperty(ref _filePath, value))
            {
                IsFileSelected = !string.IsNullOrEmpty(value);
            }
        }
    }

    public bool IsFileSelected
    {
        get => _isFileSelected;
        set => SetProperty(ref _isFileSelected, value);
    }

    public string XpathExpression
    {
        get => _xpathExpression;
        set => SetProperty(ref _xpathExpression, value);
    }
    
    public ObservableCollection<string> XpathResults { get; } = new();

    #endregion

    #region  

    public ICommand FilePickerCommand { get; }
    public ICommand GetXpathResultsCommand { get; }

    #endregion

    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            if (_isBusy != value)
            {
                SetProperty(ref _isBusy, value);
            }
        }
    }
}
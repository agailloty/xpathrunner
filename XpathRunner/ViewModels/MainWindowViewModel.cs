using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using XpathRunner.Service;

namespace XpathRunner.ViewModels;

public class MainWindowViewModel : ObservableObject
{
    #region Private members
    private bool _isBusy;
    private readonly DialogService _dialogService = new();
    private string _filePath = string.Empty;
    private bool _isFileSelected;
    private string _xpathExpression = string.Empty;
    private bool _isXpathResultsEmpty;
    private int _xpathResultCount;
    private ObservableCollection<FileInfo> _selectedFiles;
    private string _selectedFileLabel = string.Empty;
    private ObservableCollection<XpathExpressionItem> _xpathExpressions = new();
    private readonly XpathService _xpathService;
    private readonly ExportService _exportService;

    #endregion

    public MainWindowViewModel()
    {
        _exportService = new ExportService();
        _xpathService = new XpathService();
        _selectedFiles = new ObservableCollection<FileInfo>();
        IsXpathResultsEmpty = true;
        XpathExpressions =
        [
            new XpathExpressionItem()
        ];
        FilePickerCommand = new AsyncRelayCommand(PickFiles);
        
        GetXpathResultsCommand = new RelayCommand(GetXpathResults);
        
        AddFilesFileCommand = new AsyncRelayCommand(AddFiles);
        RemoveFileCommand = new RelayCommand<FileInfo>(RemoveFile);
        
        SelectedFiles.CollectionChanged += (sender, args) =>
        {
            UpdateSelectedFilesLabel();
        };
        
        ExportResultsCommand = new AsyncRelayCommand(ExportCsv);
        
        AddXpathCommand = new RelayCommand(() =>
        {
                XpathExpressions.Add(new XpathExpressionItem());
        });
    }

    #region  Public properties
    
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
    
    public ObservableCollection<ResultModel> XpathResults { get; } = new();

    public bool IsXpathResultsEmpty
    {
        get => _isXpathResultsEmpty;
        set => SetProperty(ref _isXpathResultsEmpty, value);
    }
    
    public int XpathResultsCount
    {
        get => _xpathResultCount;
        set => SetProperty(ref _xpathResultCount, value);
    }
    
    public ObservableCollection<FileInfo> FilesToProcess { get; } = new();
    
    public string SelectedFilesLabel
    {
        get => _selectedFileLabel;
        set => SetProperty(ref _selectedFileLabel, value);
    }

    #endregion

    #region  Commands

    public ICommand FilePickerCommand { get; }
    public ICommand GetXpathResultsCommand { get; }
    public ICommand AddFilesFileCommand { get; }
    public ICommand RemoveFileCommand { get; }
    public ICommand ExportResultsCommand { get; }
    
    public ICommand AddXpathCommand { get; }

    public ObservableCollection<FileInfo> SelectedFiles
    {
        get => _selectedFiles;
        set => SetProperty(ref _selectedFiles, value);
    }

    public ObservableCollection<XpathExpressionItem> XpathExpressions
    {
        get => _xpathExpressions;
        set => SetProperty(ref _xpathExpressions, value);
    }
    

    #endregion
    
    #region Private methods
    private async Task AddFiles()
    {
        var paths = await _dialogService.ShowFolderBrowserDialogAsync();
        if (paths == null) return;
        foreach (string path in paths)
        {
            var fileInfo = new FileInfo(path);
            bool isValidFile = fileInfo.Extension == ".html" || fileInfo.Extension == ".htm" || fileInfo.Extension == ".xml";
            if (isValidFile && FilesToProcess.All(file => file.FullName != fileInfo.FullName))
            {
                FilesToProcess.Add(new FileInfo(path));
            }
            
            if (FilesToProcess.Count > 0)
            {
                FilePath = FilesToProcess[0].FullName;
            }

            foreach (var file in FilesToProcess)
            {
                if (SelectedFiles.Contains(file)) continue;
                SelectedFiles.Add(file);
            }
            UpdateSelectedFilesLabel();
        }
    }
    
    private void RemoveFile(FileInfo? file)
    {
        if (file == null || !FilesToProcess.Contains(file)) return;
        FilesToProcess.Remove(file);
    }

    private void UpdateSelectedFilesLabel()
    {
        if (SelectedFiles.Count == 0)
        {
            SelectedFilesLabel = "No files selected";
        }
        else if (SelectedFiles.Count == 1)
        {
            SelectedFilesLabel = $"Selected file : {FilePath}";
        }
        else
        {
            SelectedFilesLabel = $"Number of selected files : {SelectedFiles.Count}";
        }
    }
    
    private void GetXpathResults()
    {
        IsBusy = true;
        var filePaths = SelectedFiles?.Select(file => file.FullName).ToArray();
        if (filePaths != null)
        {
            var nonEmptyXpaths = XpathExpressions.Where(expression => !string.IsNullOrEmpty(expression.XpathExpression))
                .Select(x => x.XpathExpression).ToArray();
            var results = _xpathService.ExtractMultipleHtmlContent(filePaths, nonEmptyXpaths);
            XpathResults.Clear();
            foreach (var result in results)
            {
                XpathResults.Add(result);
            }

            XpathResultsCount = XpathResults
                .Select(x => x.Rows.Count).Sum();
            IsXpathResultsEmpty = XpathResultsCount == 0;
        }
        IsBusy = false;
    }

    private async Task PickFiles()
    {
            IsBusy = true;
            var files = await _dialogService.ShowFolderBrowserDialogAsync();
            if (files != null)
            {
                FilePath = files[0];
            }
            IsBusy = false;
    }
    
    private async Task ExportCsv()
    {
        IsBusy = true;
        var content = XpathResults.ToList();
        await _exportService.SaveResultsToCsvAsync(content);
        IsBusy = false;
    }
    #endregion
}
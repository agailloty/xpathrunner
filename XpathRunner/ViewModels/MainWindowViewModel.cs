using System.Collections;
using System.Collections.Generic;
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
    private bool _isBusy;
    private readonly DialogService _dialogService = new();
    private string _filePath;
    private bool _isFileSelected;
    private string _xpathExpression ;
    private bool _isXpathResultsEmpty;
    private int _xpathResultCount;
    private ObservableCollection<FileInfo>? _selectedFiles;
    private string _selectedFileLabel;

    public MainWindowViewModel()
    {
        IsXpathResultsEmpty = true;
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
            var filePaths = SelectedFiles?.Select(file => file.FullName).ToArray();
            if (filePaths != null)
            {
                var results = xpathService.ExtractHtmlContent(filePaths, XpathExpression);
                XpathResults.Clear();
                foreach (var result in results)
                {
                    XpathResults.Add(result);
                }
                XpathResultsCount = XpathResults.Count;
                IsXpathResultsEmpty = XpathResultsCount == 0;
            }
            IsBusy = false;
        });
        
        AddFilesFileCommand = new RelayCommand(async () => await AddFiles());
        RemoveFileCommand = new RelayCommand<FileInfo>(RemoveFile);
        
        SelectedFiles = new ObservableCollection<FileInfo>();
        SelectedFiles.CollectionChanged += (sender, args) =>
        {
            if (args.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (FileInfo file in args.NewItems)
                {
                    FilePath = file.FullName;
                }
                UpdateSelectedFilesLabel();
            }
        };
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
    
    public ObservableCollection<string> XpathResults { get; } = new();

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

    public ObservableCollection<FileInfo>? SelectedFiles
    {
        get => _selectedFiles;
        set => SetProperty(ref _selectedFiles, value);
    }

    #endregion
    
    #region Private methods
    private async Task AddFiles()
    {
        var paths = await _dialogService.ShowFolderBrowserDialogAsync();
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
        if (SelectedFiles.Count == 1)
        {
            SelectedFilesLabel = $"Selected file : {FilePath}";
        }
        else
        {
            SelectedFilesLabel = $"Number of selected files : {SelectedFiles.Count}";
        }
    }
    #endregion
}
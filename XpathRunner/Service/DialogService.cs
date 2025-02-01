using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;

namespace XpathRunner.Service;

public class DialogService
{
    public async Task<string[]?> ShowFolderBrowserDialogAsync()
    {
        string[]? fileList = Array.Empty<string>();
        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime
            {
                MainWindow.StorageProvider: { } provider
            }) return fileList;
        var files = await provider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            Title = "Open Text File",
            AllowMultiple = true,
            FileTypeFilter = AllowedFileTypes()
            
        });
        fileList = files?.Select(file => file.TryGetLocalPath()).ToArray();


        return fileList;
    }
    
    private static FilePickerFileType[] AllowedFileTypes()
    {
        return new[]
        {
            new FilePickerFileType("HTML or XML Files")
            {
                Patterns = new[] {"*.html", "*.htm", "*.xml"}
            }
        };
    }
}
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
            AllowMultiple = false
        });
        fileList = files?.Select(file => file.TryGetLocalPath()).ToArray();


        return fileList;
    }
}
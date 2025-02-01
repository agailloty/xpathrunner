using System;
using System.IO;
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
    
    public async Task SaveResultsToCsvAsync(string[] content)
    {
        var result = string.Join(Environment.NewLine, content);
        await ExportFile(result);
    }
    
    private async Task ExportFile(string content)
    {
        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime
            {
                MainWindow.StorageProvider: { } provider
            }) return;
        
        var options = new FilePickerSaveOptions
        {
            Title = "Save File",
            SuggestedFileName = "NewFile.txt",
            DefaultExtension = "txt",
            ShowOverwritePrompt = true
        };

        var file = await provider.SaveFilePickerAsync(options);

        if (file != null)
        {
            await using var stream = await file.OpenWriteAsync();
            await using var writer = new StreamWriter(stream);
            await writer.WriteAsync(content);
        }
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
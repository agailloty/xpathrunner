using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
        fileList = files?
            .Select(file => file.TryGetLocalPath())
            .Where(path => path != null)
            .Cast<string>()
            .ToArray();


        return fileList;
    }
    
    
    public async Task ExportCSVFile(IList<string> content)
    {
        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime
            {
                MainWindow.StorageProvider: { } provider
            }) return;
        
        var options = new FilePickerSaveOptions
        {
            Title = "Save File",
            SuggestedFileName = "export.csv",
            DefaultExtension = "csv",
            ShowOverwritePrompt = true
        };

        var file = await provider.SaveFilePickerAsync(options);

        if (file != null)
        {
            await using var stream = await file.OpenWriteAsync();
            await using var writer = new StreamWriter(stream, Encoding.UTF8);
            foreach (var line in content)
            {
                await writer.WriteLineAsync(line);
            }
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
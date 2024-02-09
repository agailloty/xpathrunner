using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace xpathrunnerui
{
    public partial class MainWindow : Window
    {
        private string? _filepath;
        public MainWindow()
        {
            InitializeComponent();
        }

        public async void PickFileHandler(object sender, RoutedEventArgs args)
        {
            var file = await OpenFilePackerAsync();
            if (file is null) return;

            _filepath = file.TryGetLocalPath();
            if (_filepath != null)
            {
                txtFilePath.IsVisible = true;
                txtFilePath.Text = _filepath;
            }
                
        }

        private async Task<IStorageFile?> OpenFilePackerAsync()
        {
            if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop ||
                desktop.MainWindow?.StorageProvider is not { } provider)
                throw new NullReferenceException("Missing StorageProvider instance.");

            var files = await provider.OpenFilePickerAsync(new FilePickerOpenOptions()
            {
                Title = "Open Text File",
                AllowMultiple = false
            });

            return files?.Count >= 1 ? files[0] : null;
        }

        public void ValidateHandler(object sender, RoutedEventArgs args)
        {
            lstResults.ItemsSource = null;
            if (_filepath == null || txtXpath.Text == null)
                return;
            lstResults.ItemsSource = ExtractHtmlContent(_filepath, txtXpath.Text);
        }

        private static List<string> ExtractHtmlContent(string filepath, string xpath)
        {
            var content = new List<string>();
            var doc = new HtmlDocument();
            doc.Load(filepath);

            var results = doc.DocumentNode.SelectNodes(xpath);
            if (results == null)
                return content;

            foreach (var result in results)
                content.Add(result.InnerText);
            return content;
        }

    }
}
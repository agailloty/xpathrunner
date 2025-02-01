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
            var screen = Screens.Primary;
            // Set width and height as percentages of the screen
            if (screen != null)
            {
                Width = screen.WorkingArea.Width * 0.5;
                Height = screen.WorkingArea.Height * 0.6;
            }
        }
        

    }
}
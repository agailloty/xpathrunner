using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using HtmlAgilityPack;
using XpathRunner.ViewModels;

namespace XpathRunner.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }
    
    public MainWindowViewModel ViewModel => (MainWindowViewModel)DataContext;
}
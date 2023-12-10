using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DocumentCollector.FileSystemIndexer;

public partial class FileSystemIndexerConfigView : UserControl
{
    public FileSystemIndexerConfigView()
    {
        InitializeComponent();
        Console.WriteLine("FileSystemIndexerConfigView created");
        DataContextChanged += (sender, args) =>
        {
            var dt = this.DataContext;
            Console.WriteLine("got ctx");
        };
    }
}
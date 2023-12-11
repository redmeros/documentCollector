using System;
#if DEBUG
using Avalonia;
#endif
using Avalonia.Controls;

namespace DocumentCollector.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContextChanged += OnDataContextChanged;
        #if DEBUG
        this.AttachDevTools();
        #endif
    }

    private void OnDataContextChanged(object? sender, EventArgs e)
    {
        Console.WriteLine("ctx changed");
    }
}
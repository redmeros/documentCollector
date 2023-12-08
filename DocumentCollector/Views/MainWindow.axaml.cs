using System;
using Avalonia;
using Avalonia.Controls;

namespace DocumentCollector.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContextChanged += OnDataContextChanged;
        this.AttachDevTools();
    }

    private void OnDataContextChanged(object? sender, EventArgs e)
    {
        Console.WriteLine("ctx changed");
    }
}
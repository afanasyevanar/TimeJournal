using Avalonia.Controls;
using System;
using System.Timers;
using Avalonia.Threading;

namespace TimeJournal.Views;

public partial class MainWindow : Window
{
    private readonly DispatcherTimer _windowTimer;

    public MainWindow()
    {
        InitializeComponent();
        
        _windowTimer = new DispatcherTimer(TimeSpan.FromMinutes(20), DispatcherPriority.Background, OnTimerElapsed);
    }

    private void OnTimerElapsed(object? sender, EventArgs e)
    { 
        WindowState = WindowState.Normal; 
    }
}
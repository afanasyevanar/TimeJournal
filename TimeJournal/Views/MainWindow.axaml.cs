using Avalonia.Controls;
using System;
using TimeJournal.ViewModels;

namespace TimeJournal.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        // скорее всего DataContext тут будет null и код не выполнится
        if (DataContext is MainWindowViewModel mainWindowViewModel)
        {
            mainWindowViewModel.TimerElapsedEvent += OnTimerElapsed;
        }
        
        DataContextChanged += (sender, e) =>
        {
            if (DataContext is MainWindowViewModel viewModel)
            {
                viewModel.TimerElapsedEvent += OnTimerElapsed;
            }
        };
    }

    private void OnTimerElapsed(object? sender, EventArgs e)
    { 
        WindowState = WindowState.Normal; 
    }
}
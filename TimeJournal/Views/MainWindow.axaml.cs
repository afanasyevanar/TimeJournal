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
        AttachHandlersToModelView();
        
        DataContextChanged += (_, _) =>
        {
            AttachHandlersToModelView();
        };
    }

    private void AttachHandlersToModelView()
    {
        if (DataContext is MainWindowViewModel mainWindowViewModel)
        {
            mainWindowViewModel.TimerElapsedEvent += OnTimerElapsed;
            mainWindowViewModel.NewLineEvent += (sender, args) =>
            {
                MainTextBox.CaretIndex = MainTextBox.Text?.Length ?? 0;
            };
        }
    }

    private void OnTimerElapsed(object? sender, EventArgs e)
    { 
        WindowState = WindowState.Normal; 
    }
}
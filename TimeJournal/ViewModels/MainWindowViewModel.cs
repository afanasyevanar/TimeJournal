using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.IO;
using Avalonia.Threading;
using ReactiveUI;

namespace TimeJournal.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public event EventHandler? TimerElapsedEvent;

    private string _currentText = string.Empty;
    private ObservableCollection<JournalEntry> _entries;
    private readonly DispatcherTimer _windowTimer = new();
    private bool _isTimerEnabled;
    private int _timerInterval = 10;

    public MainWindowViewModel()
    {
        Entries = [];
        AddEntryCommand = ReactiveCommand.Create(AddEntry);
        AddNewLineCommand = ReactiveCommand.Create(AddNewLine);
        SaveCommand = ReactiveCommand.Create(Save);
        StartTimerCommand = ReactiveCommand.Create(StartTimer);
        StopTimerCommand = ReactiveCommand.Create(StopTimer);
        SetTimerIntervalCommand = ReactiveCommand.Create<int>(SetTimerInterval);
        _windowTimer.Tick += TimerElapsed;
        _windowTimer.Interval = TimeSpan.FromMinutes(_timerInterval);
    }

    public string CurrentText
    {
        get => _currentText;
        set => this.RaiseAndSetIfChanged(ref _currentText, value);
    }

    public ObservableCollection<JournalEntry> Entries
    {
        get => _entries;
        set => this.RaiseAndSetIfChanged(ref _entries, value);
    }

    public bool IsTimerEnabled
    {
        get => _isTimerEnabled;
        set
        {
            this.RaiseAndSetIfChanged(ref _isTimerEnabled, value);
            if (_isTimerEnabled)
            {
                _windowTimer.Start();
            } 
            else
            {
                _windowTimer.Stop();
            }
        }
    }

    public int TimerInterval
    {
        get => _timerInterval;
        set => this.RaiseAndSetIfChanged(ref _timerInterval, value);
    }

    public ReactiveCommand<Unit, Unit> AddEntryCommand { get; }
    public ReactiveCommand<Unit, Unit> AddNewLineCommand { get; }
    public ReactiveCommand<Unit, Unit> SaveCommand { get; }
    public ReactiveCommand<Unit, Unit> StartTimerCommand { get; }
    public ReactiveCommand<Unit, Unit> StopTimerCommand { get; }
    public ReactiveCommand<int, Unit> SetTimerIntervalCommand { get; }

    private void AddEntry()
    {
        if (!string.IsNullOrWhiteSpace(CurrentText))
        {
            Entries.Add(new JournalEntry
            {
                Text = CurrentText,
                Timestamp = DateTime.Now
            });
            CurrentText = string.Empty;
        }
    }

    private void AddNewLine()
    {
        CurrentText += Environment.NewLine;
    }
    
    private void Save()
    {
        var savesDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "saves");
        Directory.CreateDirectory(savesDirectory);
        
        var fileName = Path.Combine(savesDirectory, $"{DateTime.Now:yyyy-MM-dd}.txt");
        
        using var writer = new StreamWriter(fileName);
        foreach (var entry in Entries)
        {
            writer.WriteLine($"[{entry.Timestamp:yyyy-MM-dd HH:mm:ss}]");
            writer.WriteLine(entry.Text);
            writer.WriteLine();
        }
    }
    
    private void StartTimer()
    {
        if (!IsTimerEnabled)
        {
            IsTimerEnabled = true;
        }
    }

    private void StopTimer()
    {
        if (IsTimerEnabled)
        {
            IsTimerEnabled = false;
        }
    }

    private void SetTimerInterval(int minutes)
    {
        TimerInterval = minutes;
        _windowTimer.Interval = TimeSpan.FromMinutes(minutes);
        if (IsTimerEnabled)
        {
            _windowTimer.Stop();
            _windowTimer.Start();
        }
    }
    
    private void TimerElapsed(object? sender, EventArgs e)
    {
        TimerElapsedEvent?.Invoke(sender, e);
    }
}

public class JournalEntry
{
    public string Text { get; set; }
    public DateTime Timestamp { get; set; }
}
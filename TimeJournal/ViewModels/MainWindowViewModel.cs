using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.IO;
using ReactiveUI;

namespace TimeJournal.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private string _currentText = string.Empty;
    private ObservableCollection<JournalEntry> _entries;

    public MainWindowViewModel()
    {
        Entries = [];
        AddEntryCommand = ReactiveCommand.Create(AddEntry);
        AddNewLineCommand = ReactiveCommand.Create(AddNewLine);
        SaveCommand = ReactiveCommand.Create(Save);
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

    public ReactiveCommand<Unit, Unit> AddEntryCommand { get; }
    public ReactiveCommand<Unit, Unit> AddNewLineCommand { get; }
    public ReactiveCommand<Unit, Unit> SaveCommand { get; }

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
}

public class JournalEntry
{
    public string Text { get; set; }
    public DateTime Timestamp { get; set; }
}
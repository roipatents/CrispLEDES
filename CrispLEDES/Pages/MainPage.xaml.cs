using CrispLEDES.Models;
using Microsoft.Extensions.Logging;

namespace CrispLEDES.Pages;

public partial class MainPage : ContentPage
{
    public event EventHandler? ChooseConfigurationRequested;
    public event EventHandler? ChooseCsvFilesRequested;
    public event EventHandler<CopySelectedMessagesRequestedEventArgs>? CopySelectedMessagesRequested;

    private List<MessageInfo> _selectedMessages = [];

    public MainPage(MainPageModel model)
    {
        BindingContext = model;
        InitializeComponent();
        model.MessageInfoArray.Add(new MessageInfo(LogLevel.Information, "test.csv", "Hello world!"));
        model.MessageInfoArray.Add(new MessageInfo(LogLevel.Information, "test.csv", "Hello world!"));
        model.MessageInfoArray.Add(new MessageInfo(LogLevel.Information, "test.csv", "Hello world!"));
        model.MessageInfoArray.Add(new MessageInfo(LogLevel.Information, "test.csv", "Hello world!"));
        model.MessageInfoArray.Add(new MessageInfo(LogLevel.Information, "test.csv", "Hello world!"));
        model.MessageInfoArray.Add(new MessageInfo(LogLevel.Information, "test.csv", "Hello world!"));
        model.MessageInfoArray.Add(new MessageInfo(LogLevel.Information, "test.csv", "Hello world!"));
    }

    private void OnChooseConfigurationClicked(object sender, EventArgs e)
    {
        ChooseConfigurationRequested?.Invoke(this, EventArgs.Empty);
    }

    private void OnChooseCsvClicked(object sender, EventArgs e)
    {
        ChooseCsvFilesRequested?.Invoke(this, EventArgs.Empty);
    }

    private void OnMessagesSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        _selectedMessages = e.CurrentSelection.OfType<MessageInfo>().ToList();
    }

    private void OnCopySelectedClicked(object sender, EventArgs e)
    {
        if (_selectedMessages.Count == 0)
        {
            return;
        }

        CopySelectedMessagesRequested?.Invoke(
            this,
            new CopySelectedMessagesRequestedEventArgs(_selectedMessages));
    }
}

public sealed class CopySelectedMessagesRequestedEventArgs : EventArgs
{
    public CopySelectedMessagesRequestedEventArgs(IReadOnlyList<MessageInfo> messages)
    {
        Messages = messages;
    }

    public IReadOnlyList<MessageInfo> Messages { get; }
}

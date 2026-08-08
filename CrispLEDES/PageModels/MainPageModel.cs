using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CrispLEDES.Models;
using Microsoft.Extensions.Logging;

namespace CrispLEDES.PageModels;

public class MainPageModel : ObservableObject
{
    public ObservableCollection<MessageInfo> MessageInfoArray { get; } = [];
    
    public string? LastSuccessfulInvoice { get; set; }

    public ILogger CreateLogger(string filename) => new Logger(MessageInfoArray, filename);
    
    private class Logger(ObservableCollection<MessageInfo> messages, string filename) : ILogger
    {
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            var message = formatter(state, exception);
            if (exception is not null)
            {
                message += $" [{exception.Message}]";
            }
            messages.Add(new MessageInfo(logLevel, filename, message));
        }

        public bool IsEnabled(LogLevel logLevel) =>
            logLevel is LogLevel.Information or LogLevel.Warning or LogLevel.Error or LogLevel.Critical;

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;
    }
}

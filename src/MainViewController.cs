/*
 * The MIT License (MIT)
 *
 * Copyright © 2022-2025, Richardson Oliver Insights, LLC, All Rights Reserved
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
using System.Text;
using CrispLEDES.Converter;
using Microsoft.Extensions.Logging;
using ObjCRuntime;

namespace CrispLEDES;

public partial class MainViewController : NSViewController
{
    private const string MessagesArrayName = "messageInfoArray";

    private NSMutableArray<MessageInfo> _messages = new();

    private string? _lastSuccessfulInvoice;

    public static MainViewController? Current { get; private set; }

    #region Constructors

    public MainViewController(NativeHandle handle) : base(handle)
    {
        Current = this;
    }

    #endregion

    partial void ChooseConfigurationFile(NSObject sender)
    {
        Common.ChooseConfigFile();
    }

    partial void ChooseCsvFiles(NSObject sender)
    {
        var chooser = NSOpenPanel.OpenPanel;
        chooser.AllowedContentTypes =
        [
            UniformTypeIdentifiers.UTTypes.CommaSeparatedText
        ];
        chooser.AllowsMultipleSelection = true;
        chooser.AllowsOtherFileTypes = false;
        chooser.CanChooseDirectories = false;
        chooser.CanChooseFiles = true;
        chooser.CanCreateDirectories = false;
        chooser.ShowsHiddenFiles = false;
        chooser.Title = "Choose the Freshbooks Invoices to Process";
        var currentPath = _lastSuccessfulInvoice is null
            ? Common.DefaultCsvDirectory
            : Path.GetDirectoryName(_lastSuccessfulInvoice);
        if (!string.IsNullOrWhiteSpace(currentPath) && Directory.Exists(currentPath))
        {
            chooser.DirectoryUrl = new NSUrl(currentPath, true);
        }

        if (chooser.RunModal() != (int)NSModalResponse.OK)
        {
            return;
        }
#pragma warning disable CA1806
        chooser.Urls.Select(url => url.Path).Where(path => path is not null).Aggregate((ConfigurationInfo?) null, (current, filename) => ProcessSingleFile(current, filename!));
#pragma warning restore CA1806
    }

    public void OpenFile(string filename)
    {
        ProcessSingleFile(null, filename);
    }

    private ConfigurationInfo? ProcessSingleFile(ConfigurationInfo? configurationInfo, string filename)
    {
        NSDocumentController.SharedDocumentController.NoteNewRecentDocumentURL(new Uri(filename)!);
        var logger = new Logger(this, filename);
        if (configurationInfo is null)
        {
            try
            {
                configurationInfo = new ConfigurationInfo(Common.ConfigFile!);
            }
            catch (Exception exception)
            {
                logger.LogCritical(exception, "Could not parse configuration file: {FileName}", Common.ConfigFile);
                return null;
            }
        }
        try
        {
            // ReSharper disable once UnusedVariable
            var converter = new FreshbooksInvoiceToLEDESConverter(configurationInfo, filename, logger);
            _lastSuccessfulInvoice = filename;
        }
        catch (Exception exception)
        {
            logger.LogCritical(exception, "Could not process invoice: {FileName}", filename);
        }

        return configurationInfo;
    }

    partial void CopySelectedRows(NSObject sender)
    {
        if (MessageInfoView.SelectedRows.Count == 0)
        {
            return;
        }
        var result = new StringBuilder();
        foreach (var index in MessageInfoView.SelectedRows)
        {
            var messageInfo = Messages[index];
            result.Append(((DateTime)messageInfo.Time).ToString("O"));
            result.Append('\t');
            result.Append(messageInfo.EntryType);
            result.Append('\t');
            result.Append(messageInfo.Filename);
            result.Append('\t');
            result.Append(messageInfo.Message.Replace("\n", "\n    "));
            result.AppendLine();
        }
        var pasteboard = NSPasteboard.GeneralPasteboard;
        pasteboard.ClearContents();
        pasteboard.WriteObjects([(NSString)result.ToString()]);
    }

    [Export(MessagesArrayName)]
    // ReSharper disable once MemberCanBePrivate.Global
    public NSMutableArray<MessageInfo> Messages => _messages;

    [Export("addObject:")]
    // ReSharper disable once MemberCanBePrivate.Global
    public void AddMessage(MessageInfo message)
    {
        WillChangeValue(MessagesArrayName);
        _messages.Add(message);
        DidChangeValue(MessagesArrayName);
    }

    [Export("insertObject:inMessageInfoArrayAtIndex:")]
    public void InsertMessage(MessageInfo message, nint index)
    {
        WillChangeValue(MessagesArrayName);
        _messages.Insert(message, index);
        DidChangeValue(MessagesArrayName);
    }

    [Export("removeObjectFromMessageInfoArrayAtIndex:")]
    public void RemoveMessage(nint index)
    {
        WillChangeValue(MessagesArrayName);
        _messages.RemoveObject(index);
        DidChangeValue(MessagesArrayName);
    }

    [Export("setMessageInfoArray:")]
    public void SetMessages(NSMutableArray<MessageInfo> messages)
    {
        WillChangeValue(MessagesArrayName);
        _messages = messages;
        DidChangeValue(MessagesArrayName);
    }

    // TODO: Create an appropriate state variable for context instead?
    private class Logger(MainViewController controller, string filename) : ILogger
    {
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) =>
            logLevel is LogLevel.Information or LogLevel.Warning or LogLevel.Error or LogLevel.Critical;
        
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            var message = formatter(state, exception);
            if (exception is not null)
            {
                message += $" [{exception.Message}]";
            }
            controller.AddMessage(new MessageInfo(logLevel, filename, message));
        }
    }
}

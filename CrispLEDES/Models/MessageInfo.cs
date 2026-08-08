using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Graphics;

namespace CrispLEDES.Models;

public sealed class MessageInfo(LogLevel entryType, string filename, string message, DateTime? time = null)
{
    public string BaseFilename => Path.GetFileName(Filename);

    public string EntryType => entryType.ToString().ToUpperInvariant();

    public Color EntryTypeColor => entryType switch
    {
        LogLevel.Trace => Colors.Black,
        LogLevel.Debug => Colors.Black,
        LogLevel.Information => Colors.Green,
        LogLevel.Warning => Colors.Orange,
        LogLevel.Error => Colors.Red,
        LogLevel.Critical => Color.FromArgb("#9B0000"),
        LogLevel.None => Colors.Black,
        _ => Colors.Black
    };

    public string Filename { get; } = filename;

    public string Message { get; } = message;

    public DateTime Time { get; } = time ?? DateTime.Now;
}

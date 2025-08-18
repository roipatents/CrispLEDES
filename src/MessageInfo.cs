using Microsoft.Extensions.Logging;

namespace FreshbooksLedesConverter;

// TODO: Use custom transformers to convert the LogLevel values to text / colors
[Register(nameof(MessageInfo))]
public partial class MessageInfo(LogLevel entryType, string filename, string message) : NSObject
{
    [Export(nameof(BaseFilename))]
    public string BaseFilename => Path.GetFileName(Filename);

    [Export(nameof(EntryType))]
    public string EntryType => entryType.ToString().ToUpper();

    [Export(nameof(EntryTypeColor))]
    public NSColor EntryTypeColor => entryType switch
    {
        LogLevel.Trace => NSColor.Text,
        LogLevel.Debug => NSColor.Text,
        LogLevel.Information => NSColor.Green,
        LogLevel.Warning => NSColor.Orange,
        LogLevel.Error => NSColor.Red,
        LogLevel.Critical => NSColor.FromRgb(0x9b, 0x00, 0x00),
        LogLevel.None => NSColor.Text,
        _ => NSColor.Text
    };

    [Export(nameof(Filename))]
    public string Filename { get; } = filename;

    [Export(nameof(Message))]
    public string Message { get; } = message;

    [Export(nameof(Time))]
    public NSDate Time { get; } = (NSDate)DateTime.Now;
}

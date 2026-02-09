/*
 * The MIT License (MIT)
 *
 * Copyright © 2022-2026, Richardson Oliver Insights, LLC, All Rights Reserved
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
using Microsoft.Extensions.Logging;

namespace CrispLEDES;

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

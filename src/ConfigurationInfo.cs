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
namespace CrispLEDES;

public class ConfigurationInfo
{
    public ConfigurationInfo(string configurationFilename)
    {
        using var reader = File.OpenText(configurationFilename);
        TaxId = null;
        var lineIndex = 0;
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            lineIndex++;
            if (line is null || line.StartsWith('#'))
            {
                continue;
            }
            line = line.Trim();
            var values = line.Split(',');
            if (TaxId is null)
            {
                var maxInvoiceAmount = 0.0M;
                if (values.Length < 1 || values.Length > 2 || (values.Length == 2 && !decimal.TryParse(values[1], out maxInvoiceAmount)) || maxInvoiceAmount < 0.0M)
                {
                    throw new InvalidOperationException($"{configurationFilename} contains invalid data on line {lineIndex}: {line}");
                }
                // First non-comment is the tax id
                TaxId = values[0];
                MaxInvoiceAmount = values.Length < 2 ? 20_000M : maxInvoiceAmount;
            }
            else
            {
                if (values.Length < 3 || values.Length > 4 || !int.TryParse(values[1], out var id))
                {
                    throw new InvalidOperationException($"{configurationFilename} contains invalid data on line {lineIndex}: {line}");
                }
                var rate = values.Length == 4 && decimal.TryParse(values[3], out var rateValue) ? rateValue : (decimal?) null;
                var person = new Person(values[0], id, values[2], rate);
                People.Add(person);
            }
        }
    }

    public string? TaxId { get; }

    public People People { get; } = [];
        
    public decimal MaxInvoiceAmount { get; }
}

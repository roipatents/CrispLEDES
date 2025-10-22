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
using System.Globalization;
using System.Text.RegularExpressions;
using CsvHelper;
using Microsoft.Extensions.Logging;

namespace CrispLEDES.Converter;

public partial class FreshbooksInvoiceToLEDESConverter
{
    private static readonly Regex LineTypeMatcher = LineTypeRegex();

    private static readonly Regex ExpenseUtbmsMatcher = ExpenseUtbmsRegex();

    private static readonly Regex TaskAndActivityMatcher = TaskAndActivityRegex();

    private static readonly Regex DescriptionParser = DescriptionRegex();

    private static readonly Regex GeneralUtbmsMatcher = UtbmsRegex();

    private static readonly Regex WhitespaceMatcher = WhitespaceRegex();

    private static readonly Regex BuzzwordMatcher = BuzzwordRegex();

    private static readonly Regex PatentNumberMatcher = PatentNumberRegex();

    private static readonly Regex FlatFeeMatcher = FlatFeeRegex();

    // TODO: Move processing out of the constructor? Is `new FreshbooksInvoiceToLEDESConverter(...).Process()` better than `new FreshbooksInvoiceToLEDESConverter(...)`?
    public FreshbooksInvoiceToLEDESConverter(ConfigurationInfo configuration, string invoiceFilename,
        ILogger logger)
    {
        // Conceptually each CSV file may have multiple invoices and potentially even multiple clients
        // but practically each CSV should be a single client
        // If this changes, we may need to structure the resulting LEDES files differently
        using var reader = File.OpenText(invoiceFilename);
        using var csv = new CsvReader(reader, CultureInfo.CurrentCulture);
        var invoiceDictionary = new Dictionary<string, Invoice>();
        var invoices = new List<Invoice>();
        var index = 0;
        var messages = new MessageCaptureLogger(logger);
        foreach (var invoiceLine in csv.GetRecords<FreshbooksInvoiceLine>())
        {
            index++;

            if (!invoiceDictionary.TryGetValue(invoiceLine.InvoiceNumber, out var invoice))
            {
                invoice = invoiceDictionary[invoiceLine.InvoiceNumber] = new Invoice
                {
                    InvoiceNumber = invoiceLine.InvoiceNumber
                };
                // Maintain a list of invoices in the order encountered in addition to the lookup
                invoices.Add(invoice);
            }

            // Figure out month end if we have an end date - e.g. we've processed at least one standard item
            // This somewhat assumes most invoices only cover a month and the month end is used to date adjustments and expenses
            var monthEnd = invoice.End is { } end
                ? new DateTime(end.Year, end.Month, DateTime.DaysInMonth(end.Year, end.Month))
                : (DateTime?)null;

            // Split processing based on the Item Name in new Freshbooks, Adjustments and Expenses seem to have very thin Item Description fields
            var lineTypeMatch = LineTypeMatcher.Match(invoiceLine.ItemName);

            var lineItem =
                lineTypeMatch.Groups["Expense"].Success ? HandleExpenseItem(index, messages, invoiceLine, invoice, monthEnd) :
                lineTypeMatch.Groups["Adjustment"].Success || lineTypeMatch.Groups["Discount"].Success ? HandleAdjustmentOrDiscountItem(index, invoiceLine, invoice, monthEnd) :
                HandleStandardItem(configuration, index, messages, invoiceLine, invoice);

            if (lineItem is not null)
            {
                invoice.Items.Add(lineItem);
            }
        }

        WriteFileResults(configuration, invoiceFilename, invoices, messages.Messages, logger);
    }

    private static LineItem HandleExpenseItem(int index, ILogger messages, FreshbooksInvoiceLine invoiceLine,
        Invoice invoice, DateTime? monthEnd)
    {
        if (invoice.InvoiceDate is null || invoice.Begin is null || invoice.End is null)
        {
            // Raise an error if we are here since this invoice doesn't have a prior line item parsed yet, and thus
            // we don't know a date to assign the expense
            throw new InvalidOperationException(
                $"{invoiceLine.InvoiceNumber} - Invoices with expenses as the first line item are not supported.");
        }

        if (invoice.End < monthEnd)
        {
            invoice.End = monthEnd;
        }

        var utbms = ExpenseUtbmsMatcher.Match(invoiceLine.ItemDescription).Groups["Utbms"].Value;
        if (invoiceLine.LineTotal > 5_000.00m)
        {
            messages.LogWarning("{InvoiceNumber} - WARNING total for expense is over $5K, actual amount: {LineTotal:0.00}",
                invoiceLine.InvoiceNumber, invoiceLine.LineTotal);
        }

        return new LineItem()
        {
            LineItemNumber = index,
            ExpFeeInvAdjType = "E",
            LineItemNumberOfUnits = invoiceLine.Quantity,
            LineItemAdjustmentAmount = null,
            LineItemTotal = invoiceLine.LineTotal,
            LineItemDate = monthEnd!.Value,
            TaskCode = "",
            ExpenseCode = utbms,
            ActivityCode = "",
            Timekeeper = null,
            LineItemDescription = invoiceLine.ItemDescription,
            LineItemUnitCost = invoiceLine.Rate
        };
    }

    private static LineItem HandleAdjustmentOrDiscountItem(int index, FreshbooksInvoiceLine invoiceLine,
        Invoice invoice, DateTime? monthEnd)
    {
        if (invoice.InvoiceDate is null || invoice.Begin is null || invoice.End is null)
        {
            // Raise an error if we are here since this invoice doesn't have a prior line item parsed yet, and thus
            // we don't know a date to assign the adjustment or discount
            throw new InvalidOperationException(
                $"{invoiceLine.InvoiceNumber} - Invoices with adjustments or discounts as the first line item are not supported.");
        }

        if (invoice.End < monthEnd)
        {
            invoice.End = monthEnd;
        }

        var taskAndActivityMatch = TaskAndActivityMatcher.Match(invoiceLine.ItemDescription);
        var item = new LineItem
        {
            LineItemNumber = index,
            ExpFeeInvAdjType = "IF",
            LineItemNumberOfUnits = invoiceLine.Quantity,
            LineItemAdjustmentAmount = invoiceLine.LineTotal,
            LineItemTotal = invoiceLine.LineTotal,
            LineItemDate = monthEnd!.Value,
            TaskCode = taskAndActivityMatch.Groups["Task"].Value,
            ExpenseCode = "",
            ActivityCode = taskAndActivityMatch.Groups["Activity"].Value,
            Timekeeper = null,
            LineItemDescription = invoiceLine.ItemDescription,
            LineItemUnitCost = invoiceLine.Rate
        };
        return item;
    }

    private static LineItem? HandleStandardItem(ConfigurationInfo? configuration, int index, ILogger messages,
        FreshbooksInvoiceLine invoiceLine, Invoice invoice)
    {
        var match = DescriptionParser.Match(invoiceLine.ItemDescription);

        if (!match.Success)
        {
            messages.LogError("{InvoiceNumber} - ERROR: Unparseable line #{LineNumber}: {ItemDescription}", invoiceLine.InvoiceNumber, index + 1,
                invoiceLine.ItemDescription);
            return null;
        }

        // TODO: Allow for other languages?
        var dateToParse = $"{match.Groups["Month"].Value} {match.Groups["Day"].Value} {match.Groups["Year"].Value}";
        if (!DateTime.TryParseExact(dateToParse, "MMM d yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None,
                out var lineDate))
        {
            messages.LogError("{InvoiceNumber} - ERROR: Unparseable line date #{LineNumber}: {LineDate}", invoiceLine.InvoiceNumber, index + 1,
                dateToParse);
            return null;
        }

        if (invoice.Begin is null || lineDate < invoice.Begin)
        {
            invoice.Begin = lineDate;
        }

        if (invoice.End is null || lineDate > invoice.End)
        {
            invoice.End = lineDate;
        }

        invoice.InvoiceDate ??= invoiceLine.DateIssued;

        if (string.IsNullOrWhiteSpace(invoice.OurMatter))
        {
            invoice.OurMatter = match.Groups["OurMatter"].Value;
        }

        if (string.IsNullOrWhiteSpace(invoice.OurClient))
        {
            invoice.OurClient = match.Groups["OurClient"].Value;
        }

        if (string.IsNullOrWhiteSpace(invoice.ClientMatter))
        {
            invoice.ClientMatter = match.Groups["ClientMatter"].Value;
        }

        Match codes;
        if (match.Groups["Utbms"].Success)
        {
            // TODO: Parse the codes as part of the description parser?
            codes = GeneralUtbmsMatcher.Match(match.Groups["Utbms"].Value);
            if (!codes.Success)
            {
                messages.LogError(
                    "{InvoiceNumber} - {LineDate:yyyy-MM-dd} - ERROR: Skipping row unparseable UTBMS code line #{LineNumber}: {ItemDescription}",
                    invoiceLine.InvoiceNumber, lineDate, index + 1, invoiceLine.ItemDescription);
                return null;
            }
        }
        else
        {
            codes = Match.Empty;
            if (!string.IsNullOrWhiteSpace(invoice.ClientMatter))
            {
                messages.LogError("{InvoiceNumber} - {LineDate:yyyy-MM-dd} - ERROR: Missing required UTBMS code line #{LineNumber}: {ItemDescription}",
                    invoiceLine.InvoiceNumber, lineDate, index + 1, invoiceLine.ItemDescription);
            }
        }

        var expense = "";
        var task = "";
        var activity = "";
        string lineType;

        if (codes.Success)
        {
            if (codes.Groups["Activity"].Success)
            {
                lineType = "F";
                task = codes.Groups["ExpenseOrTask"].Value;
                activity = codes.Groups["Activity"].Value;
            }
            else
            {
                lineType = "E";
                expense = codes.Groups["ExpenseOrTask"].Value;
            }
        }
        else
        {
            // No UTBMS case
            lineType = "F";
        }

        var narrative = match.Groups["Narrative"].Value;
        if (WhitespaceMatcher.Split(narrative).Length > 30)
        {
            messages.LogWarning("{InvoiceNumber} - {LineDate:yyyy-MM-dd} - WARNING long description, >30 words: {Narrative}",
                invoiceLine.InvoiceNumber, lineDate, narrative);
        }

        var buzzword = BuzzwordMatcher.Match(narrative);
        if (buzzword.Success)
        {
            // TODO: Include all such phrases?
            messages.LogWarning("{InvoiceNumber} - {LineDate:yyyy-MM-dd} - WARNING \"{Buzzword}\" is a flagged word or phrase in: {Narrative}",
                invoiceLine.InvoiceNumber, lineDate, buzzword.Value, narrative);
        }

        var patentNumber = PatentNumberMatcher.Match(narrative);
        if (patentNumber.Success)
        {
            // TODO: Include all possible patent numbers?
            messages.LogWarning("{InvoiceNumber} - {LineDate:yyyy-MM-dd} - WARNING \"{PatentNumber}\"  may be a patent number in: {Narrative}",
                invoiceLine.InvoiceNumber, lineDate, patentNumber.Value, narrative);
        }

        if (FlatFeeMatcher.IsMatch(narrative))
        {
            messages.LogWarning("{InvoiceNumber} - {LineDate:yyyy-MM-dd} - WARNING possible flat fee in: {Narrative}",
                invoiceLine.InvoiceNumber, lineDate, narrative);
        }

        if (invoiceLine.Quantity is { } quantity)
        {
            var roundingDifference = Math.Abs(Math.Round(quantity, 1) - quantity);
            if (roundingDifference > 0.02m)
            {
                messages.LogWarning(
                    "{InvoiceNumber} - {LineDate:yyyy-MM-dd} - WARNING \"{Quantity}\" is a time unit not in tenths of an hour, e.g. only 0.1, 0.2, etc, are allowed: {Narrative}",
                    invoiceLine.InvoiceNumber, lineDate, quantity, narrative);
            }
        }

        // TODO: check for > 16 hours one person/day

        Person? timekeeper;
        var name = WhitespaceMatcher.Replace(match.Groups["TimeKeeper"].Value, " ");
        if (string.IsNullOrWhiteSpace(name))
        {
            timekeeper = null;
        }
        else if (configuration is null || !configuration.People.TryGetValue(name, out timekeeper))
        {
            throw new InvalidOperationException(
                $"{invoiceLine.InvoiceNumber} - {lineDate:yyyy-MM-dd} - Cannot find timekeeper \"{name}\" from line {index + 1} in the configuration file.");
        } else if (timekeeper.Rate == 0m)
        {
            messages.LogWarning(
                "{InvoiceNumber} - {LineDate:yyyy-MM-dd} - WARNING \"{Name}\" does not have a non-zero rate in the configuration file.",
                invoiceLine.InvoiceNumber, lineDate, timekeeper.Name);
        } else if (timekeeper.Rate is not null && invoiceLine.Rate is not null && timekeeper.Rate != invoiceLine.Rate)
        {
            messages.LogWarning(
                "{InvoiceNumber} - {LineDate:yyyy-MM-dd} - WARNING \"{Name}\" has a rate of {Rate} in the configuration file, but the invoice line has a rate of {InvoiceRate}.",
                invoiceLine.InvoiceNumber, lineDate, timekeeper.Name, timekeeper.Rate, invoiceLine.Rate);
        }

        return new LineItem
        {
            LineItemNumber = index,
            ExpFeeInvAdjType = lineType,
            LineItemNumberOfUnits = invoiceLine.Quantity,
            LineItemAdjustmentAmount = null,
            LineItemTotal = invoiceLine.LineTotal,
            LineItemDate = lineDate,
            TaskCode = task,
            ExpenseCode = expense,
            ActivityCode = activity,
            Timekeeper = timekeeper,
            LineItemDescription = narrative,
            LineItemUnitCost = invoiceLine.Rate
        };
    }

    private static void WriteFileResults(ConfigurationInfo configuration, string invoiceFilename, IList<Invoice> invoices, List<string> messages,
        ILogger logger)
    {
        var results = new List<LEDESOutputLine>();

        foreach (var invoice in invoices)
        {
            if (invoice.Total > configuration.MaxInvoiceAmount)
            {
                // TODO: Why can't we just use the wrapped logger?
                var message =
                    $"{invoice.InvoiceNumber} - WARNING total for invoice is over ${configuration.MaxInvoiceAmount / 1_000M}K, actual amount: {invoice.Total:0.0000}";
                messages.Add(message);
                logger.LogWarning(message);
            }

            results.AddRange(invoice.Items.Select(item => new LEDESOutputLine
            {
                InvoiceDate = invoice.InvoiceDate.ToLEDES(),
                InvoiceNumber = invoice.InvoiceNumber.ToLEDES(),
                ClientId = invoice.OurClient.ToLEDES(),
                LawFirmMatterId = invoice.OurMatter.ToLEDES(),
                InvoiceTotal = invoice.Total.ToLEDES(4),
                BillingStartDate = invoice.Begin.ToLEDES(),
                BillingEndDate = invoice.End.ToLEDES(),
                InvoiceDescription = "",
                LineItemNumber = item.LineItemNumber.ToLEDES(),
                ExpFeeInvAdjType = item.ExpFeeInvAdjType.ToLEDES(),
                LineItemNumberOfUnits = item.LineItemNumberOfUnits.ToLEDES(),
                LineItemAdjustmentAmount = item.LineItemAdjustmentAmount.ToLEDES(),
                LineItemTotal = item.LineItemTotal.ToLEDES(),
                LineItemDate = item.LineItemDate.ToLEDES(),
                LineItemTaskCode = item.TaskCode.ToLEDES(),
                LineItemExpenseCode = item.ExpenseCode.ToLEDES(),
                LineItemActivityCode = item.ActivityCode.ToLEDES(),
                TimekeeperId = item.Timekeeper?.Id.ToLEDES(),
                LineItemDescription = item.LineItemDescription.ToLEDES(),
                LawFirmId = configuration.TaxId.ToLEDES(),
                LineItemUnitCost = item.LineItemUnitCost.ToLEDES(),
                TimekeeperName = item.Timekeeper?.Name.ToLEDES(),
                TimekeeperClassification = item.Timekeeper?.Classification.ToLEDES(),
                ClientMatterId = invoice.ClientMatter.ToLEDES()
            }));
        }

        var outputPath = Path.GetDirectoryName(invoiceFilename);
        var outputPrefix = DateTime.Today.ToString("yyyyMMdd") + Path.GetFileNameWithoutExtension(invoiceFilename);

        {
            // CSV Output
            var path = GetOutputFilename("-csv.csv");
            using var writer = new CsvWriter(File.CreateText(path), CultureInfo.InvariantCulture);
            writer.WriteRecords(results);
            logger.LogInformation("Wrote CSV to {Path}", path);
        }

        {
            // LEDES/TXT Output
            var path = GetOutputFilename("-ledes.txt");
            using var writer = File.CreateText(path);
            writer.Write("LEDES1998B[]\n");
            writer.Write(LEDESOutputLine.LEDESHeaderLine);
            foreach (var line in results)
            {
                writer.Write(line.ToLEDES());
            }

            logger.LogInformation("Wrote LEDES to {Path}", path);
        }

        {
            // Summary File
            var path = GetOutputFilename("-summary.csv");
            using var writer = new CsvWriter(File.CreateText(path), CultureInfo.InvariantCulture);
            writer.WriteRecords(invoices);
            logger.LogInformation("Wrote Summary to {Path}", path);
        }

        {
            // Save a copy of the configuration file
            var path = GetOutputFilename("-configfileused.txt");
            File.Copy(Common.ConfigFile!, path, true);
            logger.LogInformation("Copied Configuration File to {Path}", path);
        }

        {
            // Save a copy of the input
            var path = GetOutputFilename("-invoiceinput.csv");
            File.Copy(invoiceFilename, path, true);
            logger.LogInformation("Copied Invoice File to {Path}", path);
        }

        {
            // Errors
            var path = GetOutputFilename("-errors.txt");
            using var writer = File.CreateText(path);
            if (messages.Count == 0)
            {
                writer.Write("No errors this run\n");
            }
            else
            {
                // TODO: Case or culture insensitive?
                messages.Sort();
                foreach (var line in messages)
                {
                    writer.Write(line);
                    writer.Write('\n');
                }
            }

            logger.LogInformation("Wrote Errors (if any) to {Path}", path);
        }
        return;

        string GetOutputFilename(string suffix) => Path.Join(outputPath, outputPrefix + suffix);
    }

    private class MessageCaptureLogger(ILogger inner) : ILogger
    {
        public List<string> Messages { get; } = new();

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => inner.BeginScope(state);
        
        public bool IsEnabled(LogLevel logLevel) => inner.IsEnabled(logLevel);

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            inner.Log(logLevel, eventId, state, exception, formatter);
            Messages.Add(formatter(state, exception));
        }
    }

    [GeneratedRegex("""
                    ^(
                      (?<Expense>E(XP(ENSE)?)?)
                     |(?<Adjustment>A(DJ(USTMENT)?)?)
                     |(?<Discount>DIS(COUNT)?)
                    )\b
                    """, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace, "en-US")]
    private static partial Regex LineTypeRegex();
    [GeneratedRegex(@"##(?<Utbms>[A-Z]\d{3})", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled, "en-US")]
    private static partial Regex ExpenseUtbmsRegex();
    [GeneratedRegex(@"##(?<Task>[A-Z]\d{3})-(?<Activity>[A-Z]\d{3})", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled, "en-US")]
    private static partial Regex TaskAndActivityRegex();
    [GeneratedRegex("""
                    \(
                      (?<OurMatter>(?<OurClient>[A-Z]{4})-[A-Z][0-9]{4})
                      .*?
                      (\#\#\s*(?<ClientMatter>.*?))?
                    \)
                    \s*
                    (?<TimeKeeper>[^)]*?)
                    \s*–\s*
                    (?<Month>[A-Z]{3})\s(?<Day>\d+),\s(?<Year>\d{4})
                    \s+
                    (?<Narrative>.*?)
                    (\s*\#\#\s*(?<Utbms>.*?))?
                    \s*$
                    """, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace, "en-US")]
    private static partial Regex DescriptionRegex();
    [GeneratedRegex(@"(?<ExpenseOrTask>[^-]+)(-(?<Activity>\S+)|$)", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled, "en-US")]
    private static partial Regex UtbmsRegex();
    [GeneratedRegex(@"\s+", RegexOptions.Compiled | RegexOptions.Singleline)]
    private static partial Regex WhitespaceRegex();
    [GeneratedRegex("""
                      attention
                     |internal
                     |travel
                     |research
                     |touch(ing)?\s+base
                     |reach(ing)?\s+out
                     |communications\s+with
                     |train
                     |administration
                     |business\s+dev(elopment)?
                     |courtesy
                     |discount
                    """, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace, "en-US")]
    private static partial Regex BuzzwordRegex();
    [GeneratedRegex("""
                      \d{7,}
                     |\d,\d{3}.\d{3}
                     |'\d{3}
                    """, RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace)]
    private static partial Regex PatentNumberRegex();
    [GeneratedRegex("""
                    \b(
                      (fixed|flat)\s+fee
                     |FF
                    )\b
                    """, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace, "en-US")]
    private static partial Regex FlatFeeRegex();
}

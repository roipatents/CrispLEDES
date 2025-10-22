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
using CsvHelper.Configuration.Attributes;

namespace CrispLEDES.Converter;

public class Invoice
{
    [Name("Invoice Number")]
    [Index(0)]
    public string? InvoiceNumber { get; init; }

    [Name("Earliest Date")]
    [Index(2)]
    [Format("yyyy-MM-dd")]
    public DateTime? Begin { get; set; }

    [Name("Latest Date")]
    [Index(3)]
    [Format("yyyy-MM-dd")]
    public DateTime? End { get; set; }

    [Ignore]
    public string? ClientMatter { get; set; }

    [Name("Matter")]
    [Index(1)]
    public string? OurMatter { get; set; }

    [Ignore]
    public string? OurClient { get; set; }

    [Name("Total")]
    [Index(5)]
    [Format("0.0000")]
    public decimal Total => Items.Aggregate(0.00m, (total, item) => total + item.LineItemTotal);

    [Name("Invoice Date")]
    [Index(4)]
    [Format("yyyy-MM-dd")]
    public DateTime? InvoiceDate { get; set; }

    [Ignore]
    public List<LineItem> Items { get; } = [];
}

public class LineItem
{
    public int LineItemNumber { get; init; }

    public required string ExpFeeInvAdjType { get; init; }

    public decimal? LineItemNumberOfUnits { get; init; }

    public decimal? LineItemAdjustmentAmount { get; init; }

    public decimal LineItemTotal { get; init; }

    public DateTime LineItemDate { get; init; }

    public required string TaskCode { get; init; }

    public required string ExpenseCode { get; init; }

    public required string ActivityCode { get; init; }

    public Person? Timekeeper { get; init; }

    public required string LineItemDescription { get; init; }

    public decimal? LineItemUnitCost { get; init; }
}

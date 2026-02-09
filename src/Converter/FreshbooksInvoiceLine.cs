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
using CsvHelper.Configuration.Attributes;

namespace CrispLEDES.Converter;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable once ClassNeverInstantiated.Global
public class FreshbooksInvoiceLine
{
    [Name("Client Name")]
    public string? ClientName { get; set; }

    [Name("Invoice #")]
    public required string InvoiceNumber { get; set; }

    [Name("Date Issued")]
    public DateTime? DateIssued { get; set; }

    [Name("Invoice Status")]
    public string? InvoiceStatus { get; set; }

    [Name("Date Paid")]
    public DateTime? DatePaid { get; set; }

    [Name("Item Name")]
    public required string ItemName { get; set; }

    [Name("Item Description")]
    public required string ItemDescription { get; set; }

    [Name("Rate")]
    public decimal? Rate { get; set; }

    [Name("Quantity")]
    public decimal? Quantity { get; set; }

    [Name("Discount Percentage")]
    public decimal? DiscountPercentage { get; set; }

    [Name("Line Subtotal")]
    public decimal LineSubtotal { get; set; }

    [Name("Tax 1 Type")]
    public string? Tax1Type { get; set; }

    [Name("Tax 1 Amount")]
    public decimal? Tax1Amount { get; set; }

    [Name("Tax 2 Type")]
    public string? Tax2Type { get; set; }

    [Name("Tax 2 Amount")]
    public decimal? Tax2Amount { get; set; }

    [Name("Line Total")]
    public decimal LineTotal { get; set; }

    [Name("Currency")]
    public string? Currency { get; set; }
}

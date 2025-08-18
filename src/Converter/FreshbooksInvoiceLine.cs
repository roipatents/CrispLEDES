using CsvHelper.Configuration.Attributes;

namespace FreshbooksLedesConverter.Converter;

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

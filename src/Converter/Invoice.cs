using CsvHelper.Configuration.Attributes;

namespace FreshbooksLedesConverter.Converter;

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

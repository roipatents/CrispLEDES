namespace FreshbooksLedesConverter;

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

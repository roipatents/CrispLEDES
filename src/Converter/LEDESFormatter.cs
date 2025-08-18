using System.Globalization;

namespace FreshbooksLedesConverter.Converter;

public static class LEDESFormatter
{
    public static string ToLEDES(this string? value) => value ?? "";

    public static string ToLEDES(this DateTime value) => value.ToString("yyyyMMdd");

    public static string ToLEDES(this DateTime? value) => value is null ? "" : value.Value.ToLEDES();

    public static string ToLEDES(this int value) => value.ToString(CultureInfo.InvariantCulture);

    public static string ToLEDES(this int? value) => value is null ? "" : value.Value.ToLEDES();

    public static string ToLEDES(this decimal value, int decimalPlaces = 2) => value.ToString("0." + new string('0', decimalPlaces));

    public static string ToLEDES(this decimal? value, int decimalPlaces = 2) => value is null ? "" : value.Value.ToLEDES(decimalPlaces);
}

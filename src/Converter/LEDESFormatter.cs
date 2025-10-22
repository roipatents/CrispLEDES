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

namespace CrispLEDES.Converter;

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

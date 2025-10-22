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

public class LEDESOutputLine
{
    public const string LEDESHeaderLine = "INVOICE_DATE|INVOICE_NUMBER|CLIENT_ID|LAW_FIRM_MATTER_ID|INVOICE_TOTAL|BILLING_START_DATE|BILLING_END_DATE|INVOICE_DESCRIPTION|LINE_ITEM_NUMBER|EXP/FEE/INV_ADJ_TYPE|LINE_ITEM_NUMBER_OF_UNITS|LINE_ITEM_ADJUSTMENT_AMOUNT|LINE_ITEM_TOTAL|LINE_ITEM_DATE|LINE_ITEM_TASK_CODE|LINE_ITEM_EXPENSE_CODE|LINE_ITEM_ACTIVITY_CODE|TIMEKEEPER_ID|LINE_ITEM_DESCRIPTION|LAW_FIRM_ID|LINE_ITEM_UNIT_COST|TIMEKEEPER_NAME|TIMEKEEPER_CLASSIFICATION|CLIENT_MATTER_ID[]\n";

    public string ToLEDES() => $"{InvoiceDate}|{InvoiceNumber}|{ClientId}|{LawFirmMatterId}|{InvoiceTotal}|{BillingStartDate}|{BillingEndDate}|{InvoiceDescription}|{LineItemNumber}|{ExpFeeInvAdjType}|{LineItemNumberOfUnits}|{LineItemAdjustmentAmount}|{LineItemTotal}|{LineItemDate}|{LineItemTaskCode}|{LineItemExpenseCode}|{LineItemActivityCode}|{TimekeeperId}|{LineItemDescription}|{LawFirmId}|{LineItemUnitCost}|{TimekeeperName}|{TimekeeperClassification}|{ClientMatterId}[]\n";

    [Name("INVOICE_DATE")]
    [Index(0)]
    public string? InvoiceDate { get; init; }

    [Name("INVOICE_NUMBER")]
    [Index(1)]
    public string? InvoiceNumber { get; init; }

    [Name("CLIENT_ID")]
    [Index(2)]
    public string? ClientId { get; init; }

    [Name("LAW_FIRM_MATTER_ID")]
    [Index(3)]
    public string? LawFirmMatterId { get; init; }

    [Name("INVOICE_TOTAL")]
    [Index(4)]
    public string? InvoiceTotal { get; init; }

    [Name("BILLING_START_DATE")]
    [Index(5)]
    public string? BillingStartDate { get; init; }

    [Name("BILLING_END_DATE")]
    [Index(6)]
    public string? BillingEndDate { get; init; }

    [Name("INVOICE_DESCRIPTION")]
    [Index(7)]
    public string? InvoiceDescription { get; init; }

    [Name("LINE_ITEM_NUMBER")]
    [Index(8)]
    public string? LineItemNumber { get; init; }

    [Name("EXP/FEE/INV_ADJ_TYPE")]
    [Index(9)]
    public string? ExpFeeInvAdjType { get; init; }

    [Name("LINE_ITEM_NUMBER_OF_UNITS")]
    [Index(10)]
    public string? LineItemNumberOfUnits { get; init; }

    [Name("LINE_ITEM_ADJUSTMENT_AMOUNT")]
    [Index(11)]
    public string? LineItemAdjustmentAmount { get; init; }

    [Name("LINE_ITEM_TOTAL")]
    [Index(12)]
    public string? LineItemTotal { get; init; }

    [Name("LINE_ITEM_DATE")]
    [Index(13)]
    public string? LineItemDate { get; init; }

    [Name("LINE_ITEM_TASK_CODE")]
    [Index(14)]
    public string? LineItemTaskCode { get; init; }

    [Name("LINE_ITEM_EXPENSE_CODE")]
    [Index(15)]
    public string? LineItemExpenseCode { get; init; }

    [Name("LINE_ITEM_ACTIVITY_CODE")]
    [Index(16)]
    public string? LineItemActivityCode { get; init; }

    [Name("TIMEKEEPER_ID")]
    [Index(17)]
    public string? TimekeeperId { get; init; }

    [Name("LINE_ITEM_DESCRIPTION")]
    [Index(18)]
    public string? LineItemDescription { get; init; }

    [Name("LAW_FIRM_ID")]
    [Index(19)]
    public string? LawFirmId { get; init; }

    [Name("LINE_ITEM_UNIT_COST")]
    [Index(20)]
    public string? LineItemUnitCost { get; init; }

    [Name("TIMEKEEPER_NAME")]
    [Index(21)]
    public string? TimekeeperName { get; init; }

    [Name("TIMEKEEPER_CLASSIFICATION")]
    [Index(22)]
    public string? TimekeeperClassification { get; init; }

    [Name("CLIENT_MATTER_ID")]
    [Index(23)]
    public string? ClientMatterId { get; init; }
}

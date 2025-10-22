# **CrispLEDES – FreshBooks to LEDES Converter**

## **Overview**

CrispLEDES is a lightweight **macOS desktop application** written in C\#. It converts FreshBooks invoice CSV files into the **LEDES 98B REV. 7** format required by most corporate clients’ e-billing systems. The tool also validates invoice data, performs sanity checks, and generates clear feedback messages — helping law firms avoid rejected invoices and delayed payments.

CrispLEDES was created and is maintained by **Richardson Oliver LLP**.

This project is released under the **MIT License** (see LICENSE.md).

## **Features**

* Convert FreshBooks CSV invoices into LEDES 98B REV. 7 text files.

* Perform **validation checks** for:

  * Proper invoice structure (start/end dates required).

  * Narrative length and banned keywords.

  * Timekeeper codes, classifications, and rates.

  * Billing in tenths of an hour.

  * Expense thresholds (e.g., \>$5,000 flagged).

  * Invoice totals exceeding configured limits (default $20,000).

* Identify line types automatically (expense, adjustment, discount, or standard).

* Generate multiple helpful outputs:

  * LEDES invoice file

  * Copy of configuration file

  * CSV equivalent of LEDES data

  * CSV invoice summary

  * Input CSV copy

  * Log of messages, warnings, and errors

## **Configuration File**

CrispLEDES relies on a configuration file to map timekeepers and enforce firm-specific rules.

* First non-comment line \= **law firm tax ID** (optionally followed by a warning threshold for invoice totals).

* Subsequent lines \= **timekeepers**, in the format:

  ```
  Name,Identifier,RoleCode,OptionalRate
  ```

  * Example: Jane Doe,2,AS,165

* Supported roles include:

  * PT – Partner

  * AS – Associate

  * OC – Counsel

  * LA – Legal Assistant

  * OT – Other Timekeeper

See configuration-file.txt for a working example.

## **User Interface**

The UI is intentionally minimal:

* Select your configuration file.

* Select your FreshBooks CSV invoice file.

* Review the list of messages generated during conversion.

If issues are detected (e.g., incorrect timekeeper rate, overlong narratives, or totals above thresholds), CrispLEDES provides warnings or errors so you can fix the data before submission.

## **Example Validation Rules**

* Narratives over 30 words generate warnings.

* Narratives containing certain keywords, possible patent numbers, or flat-fee indications generate warnings.

* Standard line items must have consistent timekeeper rates.

* Expenses \> $5,000 or invoices \> $20,000 generate warnings.

## **Installation**

1. Download the latest CrispLEDES release from the [Releases page](https://github.com/roipatents/CrispLEDES/releases).

2. Open the .pkg file and follow the prompts for installation.

3. (Optional) Update your configuration file to reflect your firm’s tax ID, timekeepers, and rates.

## **Usage**

1. Export invoices from FreshBooks as CSV.

2. Open CrispLEDES.

3. Select your configuration file and invoice file.

4. Review messages, warnings, and errors.

5. Submit the generated LEDES 98B file to your client’s e-billing system.

## **Quick Start (Sample Inputs & Output)**

Below is a simple, copy-pasteable walkthrough that shows the expected shape of inputs and the kind of LEDES output CrispLEDES generates.

### **1\) Sample configuration file**

Save as configuration.txt and select it in the app when prompted.

```
00-0000000,20000
John Doe,1,PT,1000
Jane Doe,2,AS,165
```

### **2\) Sample FreshBooks CSV (minimal)**

Save as invoice\_details.csv. Only essential columns are shown here for clarity.

```
Client,Matter,Date,Description,Hours,Rate,Amount,Type
ACME Corp,Prosecution,2025-01-03,"(ABCD-1A2B Optional description ##CL12345) TK1 - Jan 3, 2025 Draft office action response ##L110",2.5,165,412.50,FEE
ACME Corp,Prosecution,2025-01-04,"(ABCD-1A2B) TK2 - Jan 4, 2025 Prior art search ##L320",1.0,1000,1000.00,FEE
ACME Corp,Prosecution,2025-01-05,"(ABCD-1A2B) Expense: USPTO filing fee ##E101",,,$760.00,EXPENSE
```

*   
  The parenthetical header (...) includes your internal client/matter code and optional client matter number \#\#CL....

* TK1 / TK2 are your timekeeper codes.

* An optional UTBMS code can appear at the end of the description, prefixed with \#\# (e.g., \#\#L110).

### **3\) Example LEDES 98B output (excerpt)**

CrispLEDES produces a pipe-delimited LEDES 98B text file. Below is a **representative excerpt** (fields and values will reflect your configuration and invoice data):

```
INVOICE_DATE|INVOICE_NUMBER|CLIENT_ID|LAW_FIRM_MATTER_ID|INVOICE_TOTAL|...|LINE_ITEM_NUMBER|EXP/FEE/ADR|LINE_ITEM_DATE|TIMEKEEPER_ID|TASK_CODE|LINE_ITEM_AMOUNT|LINE_ITEM_UNITS|LINE_ITEM_RATE|LINE_ITEM_DESCRIPTION
20250106|INV-2025-0001|ACME|ABCD-1A2B|2172.50|...|1|F|20250103|2|L110|412.50|2.5|165.00|Draft office action response
20250106|INV-2025-0001|ACME|ABCD-1A2B|2172.50|...|2|F|20250104|1|L320|1000.00|1.0|1000.00|Prior art search
20250106|INV-2025-0001|ACME|ABCD-1A2B|2172.50|...|3|E|20250105|||E101|760.00|||USPTO filing fee
```

Tip: The app also generates summary.csv, details.csv, a copy of your input CSV, the configuration file used, and a messages.log with warnings and errors.

### **4\) Common issues (and how to fix)**

* **Narrative too long**: tighten to \~30 words.

* **Mismatched/zero timekeeper rate**: update rate in configuration or the line item.

* **Non-tenth hour increments**: adjust hours (e.g., 1.3 → 1.3 is OK; 1.33 → 1.3).

* **High expense or invoice total**: confirm justification or split invoices if appropriate.

## **License**

This project is licensed under the MIT License. See the LICENSE.md file for details.

## **Contributing**

Contributions are welcome\! Please fork the repo, submit pull requests, or open issues to suggest improvements.

## **Acknowledgments**

* LEDES format reference: [LEDES.org](https://ledes.org/ledes-98b-format/)

* UTBMS code reference: [UTBMS.com](https://utbms.com/)

## **Attribution**

CrispLEDES was created and is maintained by **Richardson Oliver LLP**.


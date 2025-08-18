Overview
-----------------------------------

The Freshbooks to LEDES Converter is a macOS desktop application written in C#
that takes a Freshbooks CSV invoice file and converts it to a
[LEDES 98B REV. 7](https://ledes.org/ledes-98b-format/) formatted text file.
Additionally, it performs validation and sanity checking on the LEDES invoice
and provides a list of informational messages, warnings, and errors.
The program determines whether a given line in the invoice file is an expense,
adjustment, discount, or standard invoice line. A separate configuration file
is maintained, with a blank version distributed with the program, but a custom
configuration file may be selected in its place. The format of the
configuration file is as follows:

*   Lines that start with "#" are comments.


*   The first uncommented line must be the tax ID number of the law firm. It
may be followed by an optional comma and the amount above which an invoice
generates a warning. The default is 20000 ($20,000).


*   Subsequent uncommented lines are timekeepers in the form
&lt;name>,&lt;integer identifier>,&lt;LEDES timekeeper classification
code>,&lt;optional timekeeper's hourly rate>

The LEDES file, a copy of the configuration file, a CSV equivalent of the
LEDES data, a CSV summary of the invoices, a copy of the input CSV, and a
log of any generated messages are created using date-stamped filenames
based upon the input file name in the same directory as the input file.

User Interface
-----------------------------------------

The user interface is quite simple. It consists of a couple of buttons to
choose the configuration and input files and a list of the generated
messages:

![](https://lh4.googleusercontent.com/S9jPdS3RMJCnHNhbEpZxPNrGfSV-OVVmr-iCw4lM2T81O9ZgYapUKzFQiI-5S6PXMGpkVZKwe1qOcFlSG0LtjEUIqfxlxpSKpjnVaGrEoOh65f7dMpdzsEPSkeKyAaqvdh3Ftwp-ls2bCLMoPbJHCraqwVWAD6dXcb9_eH0XPWjYpZgwHd8KfQ=w1280)

The image above shows the result of a successful run of the tool with
warnings for the input file "invoice\_details (13).csv".

Validations
--------------------------------------

*   Invoices are required to start with a standard invoice line that
includes beginning and ending date information before any other line type
may appear.

  
* Standard line items are expected to have a description of the following
form (line breaks and indentation are for readability):\
\
    (\
    &nbsp;&nbsp;&nbsp;&nbsp;&lt;4 letter internal client code>-&lt;4 character alphanumeric code that disambiguates the matter number>\
    &nbsp;&nbsp;&nbsp;&nbsp;&lt;optional description>\
    &nbsp;&nbsp;&nbsp;&nbsp;&lt;optional client matter number prefaced with "##">\
    )&lt;optional whitespace>\
    &lt;timekeeper code>&lt;optional whitespace>-&lt;optional whitespace>\
    &lt;the invoice line date in "MMM d, yyyy" format, e.g. Jan 3, 2025>&lt;whitespace>\
    &lt;optional narrative>\
    &lt;optional [UTBMS](https://utbms.com/) code prefaced with "##">&lt;optional whitespace>


*   A warning is generated if the narrative has more than 30 words or if it
contains certain keywords that tend to annoy clients, a possible patent
number, or an indication of a flat fee.


*   Standard line items should be billed in tenths of an hour, otherwise a
warning is generated.


*   The rate for an active timekeeper on a standard line item should not be
zero (indicating that we don't know what their standard rate should be),
otherwise a warning is generated.


*   If the timekeeper has an assigned rate then the standard line item rate
should match, otherwise a warning is generated.


*   Other line types can also have an optional UTBMS code prefaced with "##"
in their descriptions.


*   Expenses greater than $5K generate a warning.


*   A warning is generated if a given invoice has line items totaling over
the invoice warning amount from the configuration file ($20K if not provided).


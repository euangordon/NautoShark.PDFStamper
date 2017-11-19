The NautoShark PDFStamper, is intended to be used to update PDF files with Copyright information. 
It was created as a stand along project to update UKHO Copyright files with the VAR licence for service corrections to vessels using Compass.

Example Usage:
public static Stream AddUKHOCopyrightTracings(Stream InputFile, float topMargin)
{
    PDFStamper ps = new PDFStamper(ConfigurationManager.AppSettings["UkhoVARLicenceNumber"], ConfigurationManager.AppSettings["UkhoVARLicenceYear"]);
    MemoryStream _ms = new MemoryStream();
    InputFile.CopyTo(_ms);
    return ps.AddVARLicenceTracings(_ms, topMargin);
}

NautoShark.PDFStamper is licenced as AGPL software.

It uses iText version 5.5.12 which is licensed as AGPL software.

AGPL is a free / open source software license.

This doesn't mean the software is gratis!

Buying a license is mandatory as soon as you develop commercial activities distributing the iText software inside your product or deploying it on a network without disclosing the source code of your own applications under the AGPL license. These activities include:

offering paid services to customers as an ASP
serving PDFs on the fly in the cloud or in a web application
shipping iText with a closed source product
Contact sales for more info: http://itextpdf.com/sales

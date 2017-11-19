The [NautoShark](http://www.nautoshark.com/) PDFStamper, is intended to be used to update PDF files with Copyright information. 
It was created as a stand along project to update UKHO Copyright files with the VAR licence for service corrections to vessels using Compass.

Example Usage:
```c#
//AddUKHOCopyrightBlocks can be used for Blocks, Diagrams
//The Block image is moved to the bottom left corner, to make room in the top right for the copyright information
//The licence information is stamped under the image layer, so it will never print ontop of the block image
public static Stream AddUKHOCopyrightBlocks(Stream InputFile)
{
    PDFStamper ps = new PDFStamper("0001/000001/01", "2017");
    MemoryStream _ms = new MemoryStream();
    InputFile.CopyTo(_ms);
    return ps.AddVARLicenceBlocks(_ms);
}

//AddUKHOCopyrightTracings can be used for all tracings and depth tables
//Suggested topMargin 0 for tracings and 20 for depth tables
public static Stream AddUKHOCopyrightTracings(Stream InputFile, float topMargin) 
{
    PDFStamper ps = new PDFStamper("0001/000001/01", "2017");
    MemoryStream _ms = new MemoryStream();
    InputFile.CopyTo(_ms);
    return ps.AddVARLicenceTracings(_ms, topMargin);
}

//AddVARLicenceWKNM can be used for the full weekly NTM
//Only updates page 1
//Adds the VAR number to the top right
//Adds the Copyright to bottom left
public static Stream AddVARCopyrightWKNM(Stream InputFile)
{
    PDFStamper ps = new PDFStamper("0001/000001/01", "2017");
    MemoryStream _ms = new MemoryStream();
    InputFile.CopyTo(_ms);
    return ps.AddVARLicenceWKNM(_ms);
}

//Used to update publication corrections which have been split into individual PDF pages
public static Stream AddVARCopyrightPublicationCorrection(Stream InputFile)
{
    PDFStamper ps = new PDFStamper("0001/000001/01", "2017");
    MemoryStream _ms = new MemoryStream();
    InputFile.CopyTo(_ms);
    return ps.AddVARLicencePublicationCorrection(_ms);
}
```

NautoShark.PDFStamper is licenced as AGPL software.

It uses iText version 5.5.12 which is licensed as AGPL software.

AGPL is a free / open source software license.

This doesn't mean the software is gratis!

Buying a license is mandatory as soon as you develop commercial activities distributing the iText software inside your product or deploying it on a network without disclosing the source code of your own applications under the AGPL license. These activities include:

offering paid services to customers as an ASP
serving PDFs on the fly in the cloud or in a web application
shipping iText with a closed source product
Contact sales for more info: http://itextpdf.com/sales

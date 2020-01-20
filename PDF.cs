using System;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.IO;

namespace NautoShark.PDFStamper
{
    public class PDFStamper
    {
        public string UKHOVARLicenceNumber, UKHOCopyRightYear;
        public PDFStamper(string ukhoVARLicenceNumber, string ukhoCopyRightYear)
        {
            //2484/170721/01
            UKHOVARLicenceNumber = ukhoVARLicenceNumber;
            //2017
            UKHOCopyRightYear = ukhoCopyRightYear;
        }

        public MemoryStream AddVARLicenceBlocks(MemoryStream inputStream)
        {
            PdfReader reader = new PdfReader((byte[])inputStream.ToArray());

            MemoryStream outputStream = new MemoryStream();

            using (Document document = new Document(reader.GetPageSizeWithRotation(1), 0, 0, 0, 0))
            {
                using (PdfWriter writer = PdfWriter.GetInstance(document, outputStream))
                {
                    document.Open();

                    PdfImportedPage importedPage = writer.GetImportedPage(reader, 1);

                    var pageRotation = reader.GetPageRotation(1);
                    var pageWidth = reader.GetPageSizeWithRotation(1).Width;
                    var pageHeight = reader.GetPageSizeWithRotation(1).Height;
                    var titlefont = FontFactory.GetFont(BaseFont.COURIER, 7, Font.NORMAL);
                    var title1 = new Paragraph(20, "Reproduced from Admiralty digital Notices to Mariners by permission of the Controller of Her Majesty’s Stationery", titlefont);
                    var title2 = new Paragraph("Office and the UK Hydrographic Office", titlefont);
                    var title1and2 = new Paragraph(20, "Reproduced from Admiralty digital Notices to Mariners by permission of the Controller of Her Majesty’s Stationery Office and the UK Hydrographic Office", titlefont);
                    var title3 = new Paragraph("HO " + UKHOVARLicenceNumber + " © British Crown Copyright " + UKHOCopyRightYear, titlefont);

                    title1.Alignment = Element.ALIGN_RIGHT;
                    title2.Alignment = Element.ALIGN_RIGHT;
                    title1and2.Alignment = Element.ALIGN_RIGHT;
                    title3.Alignment = Element.ALIGN_RIGHT;
                    title1.IndentationRight = 15;
                    title2.IndentationRight = 15;
                    title1and2.IndentationRight = 15;
                    title3.IndentationRight = 15;

                    PdfReaderContentParser parser = new PdfReaderContentParser(reader);
                    MyImageRenderListener listener = new MyImageRenderListener();
                    parser.ProcessContent(1, listener);
                    var imgWidth = listener.ImgWidth;
                    var imgHeight = listener.ImgHeight;
                    var ctmWidth = listener.CtmWidth;
                    var ctmHeight = listener.CtmHeight;
                    var xlocation = listener.Xlocation;
                    var ylocation = listener.Ylocation;

                    switch (pageRotation)
                    {
                        case 0:
                            document.Add(title1);
                            document.Add(title2);
                            document.Add(title3);
                            writer.DirectContent.AddTemplate(importedPage, 1f, 0, 0, 1f, 15-xlocation, -ylocation + 15);
                            break;

                        case 90:
                            document.Add(title1and2);
                            document.Add(title3);
                            //Remember this page is on its side, so values are not always easy to keep track of
                            // 20-ylocation
                            //      This is affecting the left hand side of the page, moving the image on the X Axis
                            //      We moved the image as far to the left as possible, but leave a margin of 20
                            // pageHeight-(pageHeight-xlocation)+5
                            //      In order for the image to be in its original location, this value should be 595 == pageHeight
                            //      We want to move the image to the bottom of the page, on the Y Axis so:
                            //      We calculate the distance between the bottom of the image and the bottom of the page == (pageHeight-xlocation)
                            //      We then move the image down by this amount, leaving an addition 5 for margin
                            writer.DirectContent.AddTemplate(importedPage, 0, -1f, 1f, 0, 15-ylocation, pageHeight-(pageHeight-xlocation)+15);
                            break;

                        default:
                            throw new InvalidOperationException(string.Format("Unexpected page rotation: [{0}].", pageRotation));
                    }

                    writer.CloseStream = false;
                    document.Close();
                    Console.WriteLine(pageRotation + "\t" + imgWidth + "\t" + imgHeight + "\t" + xlocation + "\t" + ylocation);
                }
            }

            outputStream.Position = 0;
            return outputStream;
        }

        public MemoryStream AddVARLicenceTracings(MemoryStream inputStream, float topMargin)
        {
            //Reproduced from Admiralty digital Notices to Mariners by permission of the Controller of Her Majesty’s Stationery 
            //Office and the UK Hydrographic Office
            //HO 2484/170721/01
            PdfReader reader = new PdfReader((byte[])inputStream.ToArray());

            MemoryStream outputStream = new MemoryStream();

            using (Document document = new Document(reader.GetPageSizeWithRotation(1), 0, 0, 0, 0))
            {
                using (PdfWriter writer = PdfWriter.GetInstance(document, outputStream))
                {
                    document.Open();

                    PdfImportedPage importedPage = writer.GetImportedPage(reader, 1);

                    var pageRotation = reader.GetPageRotation(1);
                    var pageWidth = reader.GetPageSizeWithRotation(1).Width;
                    var pageHeight = reader.GetPageSizeWithRotation(1).Height;
                    var titlefont = FontFactory.GetFont(BaseFont.COURIER, 7, Font.NORMAL);
                    var title1 = new Paragraph(20, "Reproduced from Admiralty digital Notices to Mariners by permission of the Controller of Her Majesty’s Stationery", titlefont);
                    var title2 = new Paragraph("Office and the UK Hydrographic Office", titlefont);
                    var title3 = new Paragraph("HO " + UKHOVARLicenceNumber, titlefont);
                    title1.Alignment = Element.ALIGN_RIGHT;
                    title2.Alignment = Element.ALIGN_RIGHT;
                    title3.Alignment = Element.ALIGN_RIGHT;
                    title1.IndentationRight = 15;
                    title2.IndentationRight = 15;
                    title3.IndentationRight = 15;

                    switch (pageRotation)
                    {
                        case 0:
                            document.Add(title1);
                            document.Add(title2);
                            document.Add(title3);
                            writer.DirectContent.AddTemplate(importedPage, 1f, 0, 0, 1f, 0, -topMargin);
                            break;

                        default:
                            throw new InvalidOperationException(string.Format("Unexpected page rotation: [{0}].", pageRotation));
                    }

                    writer.CloseStream = false;
                    document.Close();
                }
            }

            outputStream.Position = 0;
            return outputStream;
        }

        public MemoryStream AddVARLicenceWKNM(MemoryStream inputStream)
        {
            //Reproduced from Admiralty digital Notices to Mariners by permission of the Controller of Her Majesty’s Stationery 
            //Office and the UK Hydrographic Office
            //HO 2484/170721/01
            PdfReader reader = new PdfReader((byte[])inputStream.ToArray());

            MemoryStream outputStream = new MemoryStream();

            using (Document document = new Document(reader.GetPageSizeWithRotation(1), 0, 0, 0, 0))
            {
                using (PdfWriter writer = PdfWriter.GetInstance(document, outputStream))
                {
                    document.Open();
                    for (int pageNumber = 1; pageNumber < reader.NumberOfPages + 1; pageNumber++)
                    {
                        document.NewPage();
                        PdfImportedPage importedPage = writer.GetImportedPage(reader, pageNumber);

                        if (pageNumber == 1)
                        {
                            var pageRotation = reader.GetPageRotation(1);
                            var pageWidth = reader.GetPageSizeWithRotation(1).Width;
                            var pageHeight = reader.GetPageSizeWithRotation(1).Height;
                            var titlefont = FontFactory.GetFont(BaseFont.COURIER, 7, Font.NORMAL);
                            var title1 = new Paragraph(20, "HO " + UKHOVARLicenceNumber, titlefont);
                            var title2 = new Paragraph(785, "Reproduced from Admiralty digital Notices to Mariners by permission of the ", titlefont);
                            var title3 = new Paragraph("Controller of Her Majesty’s Stationery Office and the UK Hydrographic Office", titlefont);

                            title1.Alignment = Element.ALIGN_RIGHT;
                            title2.Alignment = Element.ALIGN_LEFT;
                            title3.Alignment = Element.ALIGN_LEFT;
                            title1.IndentationRight = 15;
                            title2.IndentationLeft = 15;
                            title3.IndentationLeft = 15;

                            switch (pageRotation)
                            {
                                case 0:
                                    document.Add(title1);
                                    document.Add(title2);
                                    document.Add(title3);
                                    writer.DirectContent.AddTemplate(importedPage, 1f, 0, 0, 1f, 0, 0);
                                    break;

                                default:
                                    throw new InvalidOperationException(string.Format("Unexpected page rotation: [{0}].", pageRotation));
                            }
                        }
                        else
                        {
                            writer.DirectContent.AddTemplate(importedPage, 1f, 0, 0, 1f, 0, 0);
                        }
                    }

                    writer.CloseStream = false;
                    document.Close();
                }
            }

            outputStream.Position = 0;
            return outputStream;
        }

        public MemoryStream AddVARLicencePublicationCorrection(Stream inputStream)
        {
            //Reproduced from Admiralty digital Notices to Mariners by permission of the Controller of Her Majesty’s Stationery 
            //Office and the UK Hydrographic Office
            //HO 2484/170721/01
            PdfReader reader = new PdfReader(inputStream);

            MemoryStream outputStream = new MemoryStream();

            using (Document document = new Document(reader.GetPageSizeWithRotation(1), 0, 0, 0, 0))
            {
                using (PdfWriter writer = PdfWriter.GetInstance(document, outputStream))
                {
                    document.Open();
                    for (int pageNumber = 1; pageNumber < reader.NumberOfPages + 1; pageNumber++)
                    {
                        document.NewPage();
                        PdfImportedPage importedPage = writer.GetImportedPage(reader, pageNumber);

                        if (pageNumber == 1)
                        {
                            var pageRotation = reader.GetPageRotation(1);
                            var pageWidth = reader.GetPageSizeWithRotation(1).Width;
                            var pageHeight = reader.GetPageSizeWithRotation(1).Height;
                            var titlefont = FontFactory.GetFont(BaseFont.COURIER, 7, Font.NORMAL);
                            var title1 = new Paragraph(20, "HO " + UKHOVARLicenceNumber, titlefont);
                            var title2 = new Paragraph(785, "Reproduced from Admiralty digital Notices to Mariners by permission of the ", titlefont);
                            var title3 = new Paragraph("Controller of Her Majesty’s Stationery Office and the UK Hydrographic Office", titlefont);

                            title1.Alignment = Element.ALIGN_RIGHT;
                            title2.Alignment = Element.ALIGN_LEFT;
                            title3.Alignment = Element.ALIGN_LEFT;
                            title1.IndentationRight = 15;
                            title2.IndentationLeft = 15;
                            title3.IndentationLeft = 15;

                            switch (pageRotation)
                            {
                                case 0:
                                    document.Add(title1);
                                    document.Add(title2);
                                    document.Add(title3);
                                    writer.DirectContent.AddTemplate(importedPage, 1f, 0, 0, 1f, 0, 0);
                                    break;

                                default:
                                    throw new InvalidOperationException(string.Format("Unexpected page rotation: [{0}].", pageRotation));
                            }
                        }
                        else
                        {
                            writer.DirectContent.AddTemplate(importedPage, 1f, 0, 0, 1f, 0, 0);
                        }
                    }

                    writer.CloseStream = false;
                    document.Close();
                }
            }

            outputStream.Position = 0;
            return outputStream;
        }

        public MemoryStream ImageStripper(MemoryStream inputStream)
        {
            var imageStream = new MemoryStream();
            PdfReader reader = new PdfReader((byte[])inputStream.ToArray());

            //using (Document document = new Document(reader.GetPageSizeWithRotation(1), 0, 0, 0, 0))
            //{
                //document.Open();
                PdfReaderContentParser parser = new PdfReaderContentParser(reader);
                MyImageRenderListener listener = new MyImageRenderListener();
                parser.ProcessContent(1, listener);
                imageStream = new MemoryStream(listener.Image);
                Console.WriteLine($"Image Type - {listener.ImageType}");
                Console.WriteLine($"Width in Pixels - {listener.ImageWidthPixels}");

                //document.Close();
                //}

            return imageStream;
        }
    }
}



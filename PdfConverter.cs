using Compass.Logging;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace NautoShark.PDFStamper
{
    public class PdfConverter
    {
        public static MemoryStream ConvertJpgStreamToPdfStream(MemoryStream inputStream)
        {
            MemoryStream outputStream = new MemoryStream();

            var pageSize = PageSize.A4;
            var pageWidth = pageSize.Width;
            var pageHeight = pageSize.Height;

            using (Document document = new Document())
            {
                using (PdfWriter writer = PdfWriter.GetInstance(document, outputStream))
                {
                    document.Open();

                    var image = Image.GetInstance(inputStream);
                    //Need to scale the image to the correct size, depending on the original DPI
                    //https://www.mikesdotnetting.com/article/87/itextsharp-working-with-images?fbclid=IwAR0JDk6xKoxKPt4nC1UESgCX6Yb04ZccU1qOSJ6eRlLttkfM9u48c5nD5Tk

                    var scalePercentage = (72f / image.DpiX) * 100; 

                    if(scalePercentage != 24)
                    {
                        Log.Info($"Scale Percentage - {scalePercentage}");
                    }

                    image.ScalePercent(scalePercentage);

                    if (image.ScaledWidth > pageWidth)
                    {
                        image.RotationDegrees = 90f;
                    }

                    //Log.Info($"Image Width - {image.Width}");
                    //Log.Info($"Image Height - {image.Height}");
                    //Log.Info($"Image Scaled W - {image.ScaledWidth}");
                    //Log.Info($"Image Scaled H - {image.ScaledHeight}");
                    //Log.Info($"Image DpiX - {image.DpiX}");
                    //Log.Info($"Image DpiY - {image.DpiY}");

                    document.Add(image);

                    writer.CloseStream = false;
                    document.Close();
                }
            }

            outputStream.Position = 0;
            return outputStream;
        }
    }
}

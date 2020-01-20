/*
 * This class is part of the book "iText in Action - 2nd Edition"
 * written by Bruno Lowagie (ISBN: 9781935182610)
 * For more info, go to: http://itextpdf.com/examples/
 * This example only works with the AGPL version of iText.
 */
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace NautoShark.PDFStamper
{
    public class MyImageRenderListener : IRenderListener
    {
        // ===========================================================================
        /** the byte array of the extracted images */
        private float _imgWidth;
        public float ImgWidth
        {
            get { return _imgWidth; }
        }

        private float _imgHeight;
        public float ImgHeight
        {
            get { return _imgHeight; }
        }

        private float _ctmWidth;
        public float CtmWidth
        {
            get { return _ctmWidth; }
        }

        private float _ctmHeight;
        public float CtmHeight
        {
            get { return _ctmHeight; }
        }

        private float _xlocation;
        public float Xlocation
        {
            get { return _xlocation; }
        }

        private float _ylocation;
        public float Ylocation
        {
            get { return _ylocation; }
        }

        private byte[] _image;
        public byte[] Image
        {
            get { return _image; }
        }

        private string _imageType;
        public string ImageType
        {
            get { return _imageType; }
        }

        public float ImageWidthPixels
        {
            get
            {   
                var widthInches = _ctmWidth / 72;
                var widthPixels = widthInches * 96;
                return widthPixels;
            }
        }

        // ---------------------------------------------------------------------------
        /**
         * Creates a RenderListener that will look for images.
         */
        public MyImageRenderListener()
        {
            _imgWidth = new float();
            _imgHeight = new float();
            _ctmWidth = new float();
            _ctmHeight = new float();
            _xlocation = new float();
            _ylocation = new float();
            _imageType = "";
        }
        // ---------------------------------------------------------------------------
        /**
         * @see com.itextpdf.text.pdf.parser.RenderListener#beginTextBlock()
         */
        public void BeginTextBlock() { }
        // ---------------------------------------------------------------------------     
        /**
         * @see com.itextpdf.text.pdf.parser.RenderListener#endTextBlock()
         */
        public void EndTextBlock() { }
        // ---------------------------------------------------------------------------     
        /**
         * @see com.itextpdf.text.pdf.parser.RenderListener#renderImage(
         *     com.itextpdf.text.pdf.parser.ImageRenderInfo)
         */
        public void RenderImage(ImageRenderInfo renderInfo)
        {
            try
            {
                var img = renderInfo.GetImage().GetDrawingImage();
                _imgWidth = img.Width;
                _imgHeight = img.Height;
                img.Dispose();

                //Get the current transformation matrix
                var ctm = renderInfo.GetImageCTM();
                _ctmWidth = ctm[0];
                _ctmHeight = ctm[4];
                _xlocation = ctm[Matrix.I31];
                _ylocation = ctm[Matrix.I32];

                var imageObject = renderInfo.GetImage();
                _image = imageObject.GetImageAsBytes();
                _imageType = imageObject.GetFileType();

            }
            catch
            {
                // pass through any other unsupported image types
            }
        }
        // ---------------------------------------------------------------------------     
        /**
          * @see com.itextpdf.text.pdf.parser.RenderListener#renderText(
          *     com.itextpdf.text.pdf.parser.TextRenderInfo)
          */
        public void RenderText(TextRenderInfo renderInfo) { }
        // ===========================================================================
    }
}

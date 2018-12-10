using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Winnovative;

namespace Advantech.Myadvantech.DataAccess.Common
{
    public class PDFUtil
    {
        public static byte[] GeneratePDFWithHeaderFooter(string HtmlString, string headerHtmlId, string footerHtmlId)
        {
            string pdfConverterLicenseKey = "fvDg8eDx4+jx6P/h8eLg/+Dj/+jo6Og=";

            byte[] pdfBytes = null;

            PdfConverter pdfConverter = new PdfConverter();
            pdfConverter.LicenseKey = pdfConverterLicenseKey;
            pdfConverter.PdfDocumentOptions.EmbedFonts = false;
            pdfConverter.PdfDocumentOptions.TopMargin = 10;
            pdfConverter.PdfDocumentOptions.RightMargin = 10;
            pdfConverter.PdfDocumentOptions.LeftMargin = 10;
            pdfConverter.PdfDocumentOptions.BottomMargin = 30;

            var headerBlock = "";
            var footerBlock = "";

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            TextReader htmlReader = new StringReader(HtmlString.Replace("&nbsp;", " "));
            doc.Load(htmlReader);

            // Install a handler where to change the header and footer in first page
            //pdfConverter.PrepareRenderPdfPageEvent += new PrepareRenderPdfPageDelegate(htmlToPdfConverter_PrepareRenderPdfPageEvent);


            //Header control
            pdfConverter.PdfDocumentOptions.ShowHeader = true;
            headerBlock = doc.GetElementbyId(headerHtmlId).InnerHtml;



            HtmlToPdfElement headerHtml = new HtmlToPdfElement(0, 0, 600, headerBlock, "");
            //headerHtml.FitHeight = true;  //用了會變小
            pdfConverter.PdfHeaderOptions.HeaderHeight = 180;
            headerHtml.FitWidth = true;
            headerHtml.EmbedFonts = true;
            pdfConverter.PdfHeaderOptions.AddElement(headerHtml);
            pdfConverter.PdfHeaderOptions.PageNumberingStartIndex = 1;


            //footer control
            pdfConverter.PdfDocumentOptions.ShowFooter = true;
            footerBlock = doc.GetElementbyId(footerHtmlId).InnerHtml;
            HtmlToPdfElement footerHtml = new HtmlToPdfElement(0, 0, 570, footerBlock, "");
            pdfConverter.PdfFooterOptions.FooterHeight = 250;
            footerHtml.FitWidth = true;
            footerHtml.EmbedFonts = true;
            pdfConverter.PdfFooterOptions.AddElement(footerHtml);
            pdfConverter.PdfFooterOptions.PageNumberingStartIndex = 2;
            pdfConverter.PdfFooterOptions.PageNumberingPageCountIncrement = 1;


            HtmlString = HtmlString.Replace("id=\"" + headerHtmlId + "\"", "id=\"" + headerHtmlId + "\"" + " style ='display:none;'");
            HtmlString = HtmlString.Replace("id=\"" + footerHtmlId + "\"", "id=\"" + footerHtmlId + "\"" + " style ='display:none;'");
            pdfBytes = pdfConverter.GetPdfBytesFromHtmlString(HtmlString);

            return pdfBytes;
         
        }

        //private static void htmlToPdfConverter_PrepareRenderPdfPageEvent(PrepareRenderPdfPageParams eventParams)
        //{
        //    if ((eventParams.PageNumber == 2))
        //    {
        //        // Change the header and footer in first page with an alternative header and footer

        //        // The PDF page being rendered
        //        PdfPage pdfPage = eventParams.Page;


        //        // Add a custom footer of 80 points in height to this page
        //        // The default document footer will be replaced in this page
        //        pdfPage.AddFooterTemplate(80);
        //        // Draw the page header elements
        //        DrawAlternativePageFooter(pdfPage.Footer, true, true);
        //    }
        //}

        ///// <summary>
        ///// Draw the alternative page footer elements
        ///// </summary>
        ///// <param name="htmlToPdfConverter">The HTML to PDF Converter object</param>
        ///// <param name="addPageNumbers">A flag indicating if the page numbering is present in footer</param>
        ///// <param name="drawFooterLine">A flag indicating if a line should be drawn at the top of the footer</param>
        //private static void DrawAlternativePageFooter(Template footerTemplate, string footerBlock, bool addPageNumbers, bool drawFooterLine)
        //{
        //    string footerHtmlUrl = Server.MapPath("~/DemoAppFiles/Input/HTML_Files/Footer_Alt_HTML.html");


        //    //footer control
        //    //pdfConverter.PdfDocumentOptions.ShowFooter = true;
        //    //footerBlock = doc.GetElementbyId(footerHtmlId).InnerHtml;
        //    HtmlToPdfElement footerHtml = new HtmlToPdfElement(0, 0, 570, footerBlock, "");
        //    pdfConverter.PdfFooterOptions.FooterHeight = 250;
        //    footerHtml.FitWidth = true;
        //    footerHtml.EmbedFonts = true;
        //    pdfConverter.PdfFooterOptions.AddElement(footerHtml);


        //    // Create a HTML element to be added in footer
        //    HtmlToPdfElement footerHtml = new HtmlToPdfElement(footerHtmlUrl);

        //    // Set the HTML element to fit the container height
        //    footerHtml.FitHeight = true;

        //    // Add HTML element to footer
        //    footerTemplate.AddElement(footerHtml);

        //    // Add page numbering
        //    //if (addPageNumbers)
        //    //{
        //    //    // Create a text element with page numbering place holders &p; and & P;
        //    //    TextElement footerText = new TextElement(10, 30, "Page &p; of &P;",
        //    //        new System.Drawing.Font(new System.Drawing.FontFamily("Times New Roman"), 10, System.Drawing.GraphicsUnit.Point));

        //    //    // Align the text at the right of the footer
        //    //    footerText.TextAlign = HorizontalTextAlign.Left;

        //    //    // Set page numbering text color
        //    //    footerText.ForeColor = Color.Navy;

        //    //    // Embed the text element font in PDF
        //    //    footerText.EmbedSysFont = true;

        //    //    // Add the text element to footer
        //    //    footerTemplate.AddElement(footerText);
        //    //}

        //}

        //public static byte[] GeneratePDFWithHeaderFooterId(string HtmlString, string headerHtmlId, string footerHtmlId)
        //{
        //    byte[] pdfBytes = null;

        //    var headerBlock = "";
        //    var footerBlock = "";

        //    // Create a HTML to PDF converter object with default settings
        //    HtmlToPdfConverter htmlToPdfConverter = new HtmlToPdfConverter();

        //    // Set license key received after purchase to use the converter in licensed mode
        //    // Leave it not set to use the converter in demo mode
        //    htmlToPdfConverter.LicenseKey = "fvDg8eDx4+jx6P/h8eLg/+Dj/+jo6Og=";

        //    // Install a handler where you can set header and footer visibility or create a custom header and footer in each page
        //    //htmlToPdfConverter.PrepareRenderPdfPageEvent += new PrepareRenderPdfPageDelegate(htmlToPdfConverter_PrepareRenderPdfPageEvent);

        //    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
        //    TextReader htmlReader = new StringReader(HtmlString.Replace("&nbsp;", " "));
        //    doc.Load(htmlReader);



        //    // Enable header in the generated PDF document
        //    htmlToPdfConverter.PdfDocumentOptions.ShowHeader = true;
        //    headerBlock = doc.GetElementbyId(headerHtmlId).InnerHtml;

        //    HtmlToPdfVariableElement headerHtmlWithPageNumbers = new HtmlToPdfVariableElement(headerBlock, "");


        //    // Add variable HTML element with page numbering to footer  
        //    htmlToPdfConverter.PdfHeaderOptions.AddElement(headerHtmlWithPageNumbers);

        //    HtmlString = HtmlString.Replace("id=\"" + headerHtmlId + "\"", "id=\"" + headerHtmlId + "\"" + " style ='display:none;'");
        //    HtmlString = HtmlString.Replace("id=\"" + footerHtmlId + "\"", "id=\"" + footerHtmlId + "\"" + " style ='display:none;'");

        //    // Convert the HTML page to a PDF document in a memory buffer
        //    byte[] outPdfBuffer = htmlToPdfConverter.ConvertHtml(HtmlString,"");

        //    return pdfBytes;

        //}
    }
}

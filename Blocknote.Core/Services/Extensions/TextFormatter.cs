using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Markdig;
using HtmlToOpenXml;
using DinkToPdf;
using System.Net.Http;


namespace Blocknote.Core.Services.Extensions
{
    public static class TextFormatter
    {
        private static readonly HeyRed.MarkdownSharp.Markdown _markdown = new();
        private static readonly MarkdownPipeline _markdownPipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();

        public static byte[] FormatPDF(string title, string? subtitle, string content)
        {
            var htmlFullContent = FormatHtml(title, subtitle, content);
            var converter = new BasicConverter(new PdfTools());

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4
            },
                Objects = {
                new ObjectSettings() {
                    HtmlContent = htmlFullContent,
                    WebSettings = {
                        DefaultEncoding = "utf-8"
                    }
                }
            }
            };

            var pdfBytes = converter.Convert(doc);

            return pdfBytes;
        }

        public static byte[] FormatDocx(string title, string? subtitle, string content)
        {
            var htmlConvertedFullContent = FormatHtml(title, subtitle, content);

            using var memoryStream = new MemoryStream();
            using (var doc = WordprocessingDocument.Create(memoryStream, WordprocessingDocumentType.Document, true))
            {
                var mainPart = doc.AddMainDocumentPart();
                mainPart.Document = new DocumentFormat.OpenXml.Wordprocessing.Document();
                var body = new Body();
                mainPart.Document.Append(body);

                var converter = new HtmlConverter(mainPart);
                var elements = converter.Parse(htmlConvertedFullContent);

                foreach (var element in elements)
                {
                    body.Append(element);
                }
            }
            return memoryStream.ToArray();
        }

        public static byte[] FormatPdf(string title, string? subtitle, string content)
        {
            using var stream = new MemoryStream();
            var document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 50, 50, 50, 50);
            var writer = PdfWriter.GetInstance(document, stream);
            document.Open();

            document.Add(new iTextSharp.text.Paragraph(title, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 24)) { Alignment = Element.ALIGN_CENTER, SpacingAfter = 20f });
            if (!string.IsNullOrEmpty(subtitle))
            {
                document.Add(new iTextSharp.text.Paragraph(subtitle, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18)) { Alignment = Element.ALIGN_CENTER, SpacingAfter = 30f });
            }
            document.Add(new iTextSharp.text.Paragraph(_markdown.Transform(content), FontFactory.GetFont(FontFactory.HELVETICA, 12)) { SpacingAfter = 15f });

            document.Close();
            return stream.ToArray();
        }

        public static string FormatHtml(string title, string? subtitle, string content)
        {
            return $"<!DOCTYPE html>\n<html>\n<head>\n<title>{title}</title>\n<meta charset='utf-8'>\n<style>\nbody {{ font-family: Arial, sans-serif; margin: 40px; }}\nh1 {{ text-align: center; }}\nh2 {{ text-align: center; color: #666; }}\n</style>\n</head>\n<body>\n<h1>{title}</h1>\n{(string.IsNullOrEmpty(subtitle) ? "" : $"<h2>{subtitle}</h2>")}\n{_markdown.Transform(content)}\n</body>\n</html>";
        }

        public static string FormatMarkdown(string title, string? subtitle, string content)
        {
            return $"# {title}\n\n{(string.IsNullOrEmpty(subtitle) ? "" : $"## {subtitle}\n\n")}{content}";
        }



    }
}

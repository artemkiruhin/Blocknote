using DinkToPdf;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using HtmlToOpenXml;
using Markdig;

namespace Blocknote.Core.Services.Extensions.Format
{
    public class TextFormatter : ITextFormatter
    {
        private static readonly HeyRed.MarkdownSharp.Markdown _markdown = new();
        private static readonly MarkdownPipeline _markdownPipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();

        public byte[] FormatPDF(string title, string? subtitle, string content)
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

        public byte[] FormatDocx(string title, string? subtitle, string content)
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

        public string FormatHtml(string title, string? subtitle, string content)
        {
            return $"<!DOCTYPE html>\n<html>\n<head>\n<title>{title}</title>\n<meta charset='utf-8'>\n<style>\nbody {{ font-family: Arial, sans-serif; margin: 40px; }}\nh1 {{ text-align: center; }}\nh2 {{ text-align: center; color: #666; }}\n</style>\n</head>\n<body>\n<h1>{title}</h1>\n{(string.IsNullOrEmpty(subtitle) ? "" : $"<h2>{subtitle}</h2>")}\n{_markdown.Transform(content)}\n</body>\n</html>";
        }

        public string FormatMarkdown(string title, string? subtitle, string content)
        {
            return $"# {title}\n\n{(string.IsNullOrEmpty(subtitle) ? "" : $"## {subtitle}\n\n")}{content}";
        }
    }
}

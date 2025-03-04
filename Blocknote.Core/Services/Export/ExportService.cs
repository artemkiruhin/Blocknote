using Blocknote.Core.Models.Dtos;
using Blocknote.Core.Services.Extensions.Format;
using DinkToPdf;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using HtmlToOpenXml;
using Markdig;

namespace Blocknote.Core.Services.Export;

public class ExportService : IExportService
{
    private readonly ITextFormatter _formatter;
    private static readonly HeyRed.MarkdownSharp.Markdown _markdown = new();
    private static readonly MarkdownPipeline _markdownPipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();


    public ExportService(ITextFormatter formatter)
    {
        _formatter = formatter;
    }
    
    public byte[] ExportPDF(ExportDto exportDto)
    {
        var htmlFullContent = _formatter.FormatHtml(exportDto.Title, exportDto.Subtitle, exportDto.Content);
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

    public byte[] ExportDocx(ExportDto exportDto)
    {
        var htmlConvertedFullContent = _formatter.FormatHtml(exportDto.Title, exportDto.Subtitle, exportDto.Content);

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

    public string ExportHtml(ExportDto exportDto)
    {
        return _formatter.FormatHtml(exportDto.Title, exportDto.Subtitle, exportDto.Content);
    }

    public string ExportMarkdown(ExportDto exportDto)
    {
        return _formatter.FormatMarkdown(exportDto.Title, exportDto.Subtitle, exportDto.Content);
    }
}
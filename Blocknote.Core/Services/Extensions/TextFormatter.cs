using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Markdig;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System.Text;

namespace Blocknote.Core.Services.Extensions
{
    public static class TextFormatter
    {
        private static readonly HeyRed.MarkdownSharp.Markdown _markdown = new();
        private static readonly MarkdownPipeline _markdownPipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();

        public static byte[] FormatWord(string title, string? subtitle, string content)
        {
            using var stream = new MemoryStream();
            using (var wordDocument = WordprocessingDocument.Create(stream, WordprocessingDocumentType.Document))
            {
                var mainPart = wordDocument.AddMainDocumentPart();
                mainPart.Document = new DocumentFormat.OpenXml.Wordprocessing.Document();
                var body = new Body();

                body.Append(CreateParagraphFromText(title, 36, true, JustificationValues.Center));

                if (!string.IsNullOrEmpty(subtitle))
                {
                    body.Append(CreateParagraphFromText(subtitle, 28, true, JustificationValues.Center));
                }

                AppendMarkdownContent(body, content);

                mainPart.Document.AppendChild(body);
                mainPart.Document.Save();
            }
            return stream.ToArray();
        }

        private static void AppendMarkdownContent(Body body, string markdownText)
        {
            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            var document = Markdown.Parse(markdownText, pipeline);

            foreach (var block in document)
            {
                switch (block)
                {
                    case HeadingBlock heading:
                        int fontSize = 28 - heading.Level * 2;
                        var headingRuns = ExtractInlineRuns(heading.Inline);
                        body.Append(CreateParagraphFromRuns(headingRuns, fontSize, JustificationValues.Left, true));
                        break;

                    case ParagraphBlock paragraph:
                        var paragraphRuns = ExtractInlineRuns(paragraph.Inline);
                        body.Append(CreateParagraphFromRuns(paragraphRuns, 24, JustificationValues.Left));
                        break;

                    case ListBlock list:
                        int itemNumber = 1;
                        foreach (var listItem in list.OfType<ListItemBlock>())
                        {
                            foreach (var subBlock in listItem.OfType<ParagraphBlock>())
                            {
                                var listItemRuns = ExtractInlineRuns(subBlock.Inline);
                                string prefix = list.IsOrdered ? $"{itemNumber}. " : "• ";
                                listItemRuns.Insert(0, CreateRun(prefix));
                                body.Append(CreateParagraphFromRuns(listItemRuns, 24, JustificationValues.Left));
                            }
                            itemNumber++;
                        }
                        break;

                    case CodeBlock code:
                        var codeText = string.Join("\n", code.Lines);
                        var codeRun = CreateRun(codeText, isMonospace: true, fontSize: 22);
                        body.Append(CreateParagraphFromRuns(new List<Run> { codeRun }, 22, JustificationValues.Left));
                        break;
                }
            }
        }

        private static List<Run> ExtractInlineRuns(ContainerInline inline)
        {
            var runs = new List<Run>();
            if (inline == null) return runs;

            foreach (var element in inline)
            {
                switch (element)
                {
                    case LiteralInline literal:
                        runs.Add(CreateRun(literal.Content.Text));
                        break;

                    case EmphasisInline emphasis:
                        bool isBold = emphasis.DelimiterCount >= 2;
                        bool isItalic = emphasis.DelimiterCount % 2 != 0;
                        var innerRuns = ExtractInlineRuns(emphasis);
                        ApplyFormatting(innerRuns, isBold, isItalic);
                        runs.AddRange(innerRuns);
                        break;
                }
            }
            return runs;
        }

        private static void ApplyFormatting(List<Run> runs, bool isBold, bool isItalic)
        {
            foreach (var run in runs)
            {
                var props = run.RunProperties ?? new RunProperties();
                if (isBold) props.Bold = new Bold();
                if (isItalic) props.Italic = new Italic();
                run.RunProperties = props;
            }
        }

        private static Run CreateRun(string text, bool isBold = false, bool isItalic = false,
                                   bool isMonospace = false, int fontSize = 24)
        {
            var props = new RunProperties();
            if (isBold) props.Append(new Bold());
            if (isItalic) props.Append(new Italic());
            if (isMonospace) props.Append(new RunFonts { Ascii = "Courier New" });
            props.Append(new FontSize { Val = (fontSize * 2).ToString() });

            return new Run(props, new Text(text) { Space = SpaceProcessingModeValues.Preserve });
        }

        private static DocumentFormat.OpenXml.Wordprocessing.Paragraph CreateParagraphFromRuns(IEnumerable<Run> runs, int fontSize,
                                               JustificationValues justification, bool isBold = false)
        {
            var paragraph = new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new ParagraphProperties(new Justification { Val = justification }));
            foreach (var run in runs)
            {
                var props = run.RunProperties ?? new RunProperties();
                props.FontSize = new FontSize { Val = (fontSize * 2).ToString() };
                if (isBold) props.Bold = new Bold();
                run.RunProperties = props;
                paragraph.Append(run);
            }
            return paragraph;
        }

        private static DocumentFormat.OpenXml.Wordprocessing.Paragraph CreateParagraphFromText(string text, int fontSize,
                                               bool isBold, JustificationValues justification)
            => CreateParagraphFromRuns(new List<Run> { CreateRun(text, isBold, fontSize: fontSize) },
                              fontSize, justification, isBold);

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

        private static DocumentFormat.OpenXml.Wordprocessing.Paragraph CreateParagraph(string text, int fontSize, bool bold, JustificationValues justification)
        {
            return new DocumentFormat.OpenXml.Wordprocessing.Paragraph(
                new ParagraphProperties(
                    new Justification { Val = justification },
                    new SpacingBetweenLines { After = "200" }
                ),
                new Run(
                    new RunProperties(
                        new FontSize { Val = (fontSize * 2).ToString() },
                        bold ? new Bold() : null
                    ),
                    new Text(text)
                )
            );
        }
    }
}

using System.Text.Json;
using Markdig;

namespace Blocknote.Core.Services.Extensions.Format
{
    public class TextFormatter : ITextFormatter
    {
        private static readonly HeyRed.MarkdownSharp.Markdown _markdown = new();
        private static readonly MarkdownPipeline _markdownPipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
        public string FormatHtml(string title, string? subtitle, string content)
        {
            return $"<!DOCTYPE html>\n<html>\n<head>\n<title>{title}</title>\n<meta charset='utf-8'>\n<style>\nbody {{ font-family: Arial, sans-serif; margin: 40px; }}\nh1 {{ text-align: center; }}\nh2 {{ text-align: center; color: #666; }}\n</style>\n</head>\n<body>\n<h1>{title}</h1>\n{(string.IsNullOrEmpty(subtitle) ? "" : $"<h2>{subtitle}</h2>")}\n{_markdown.Transform(content)}\n</body>\n</html>";
        }

        public string FormatMarkdown(string title, string? subtitle, string content)
        {
            return $"# {title}\n\n{(string.IsNullOrEmpty(subtitle) ? "" : $"## {subtitle}\n\n")}{content}";
        }

        public string FormatJSON(string title, string? subtitle, string content)
        {
            var dto = new { Title = title, Subtitle = subtitle, Content = content };
            var data = JsonSerializer.Serialize(dto, new JsonSerializerOptions()
            {
                WriteIndented = true
            });
            return data;
        }
    }
}

namespace Blocknote.Core.Services.Extensions.Format;

public interface ITextFormatter
{
    string FormatHtml(string title, string? subtitle, string content);
    string FormatMarkdown(string title, string? subtitle, string content);
    string FormatJSON(string title, string? subtitle, string content);
}
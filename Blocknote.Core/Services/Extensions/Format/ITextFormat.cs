namespace Blocknote.Core.Services.Extensions.Format;

public interface ITextFormatter
{
    byte[] FormatPDF(string title, string? subtitle, string content);
    byte[] FormatDocx(string title, string? subtitle, string content);
    string FormatHtml(string title, string? subtitle, string content);
    string FormatMarkdown(string title, string? subtitle, string content);
}
using Blocknote.Core.Models.Dtos;

namespace Blocknote.Core.Services.Export;

public interface IExportService
{
    byte[] ExportPDF(ExportDto exportDto);
    byte[] ExportDocx(ExportDto exportDto);
    string ExportHtml(ExportDto exportDto);
    string ExportMarkdown(ExportDto exportDto);
    string ExportJSON(ExportDto exportDto);
}
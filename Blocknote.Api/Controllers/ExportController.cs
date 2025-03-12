using System.Security.Claims;
using System.Text;
using Blocknote.Core.Models.Dtos;
using Blocknote.Core.Models.Enums;
using Blocknote.Core.Services.Base;
using Blocknote.Core.Services.Export;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blocknote.Api.Contracts
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExportController : ControllerBase
    {
        private readonly IExportService _exportService;
        private readonly INoteService _noteService;

        public ExportController(IExportService exportService, INoteService noteService)
        {
            _exportService = exportService;
            _noteService = noteService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetExport(Guid noteId, FormatType type)
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized();
                }

                var note = await _noteService.GetInfoAsync(userId, noteId);
                if (note == null) return NotFound();

                var exportDto = new ExportDto(note.Title, note.Subtitle, note.Content ?? "");

                byte[] fileBytes;
                string contentType;
                string fileDownloadName;

                switch (type)
                {
                    case FormatType.Docx:
                        fileBytes = _exportService.ExportDocx(exportDto);
                        contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                        fileDownloadName = $"{note.Title}.docx";
                        break;
                    case FormatType.Markdown:
                        var markdownContent = _exportService.ExportMarkdown(exportDto);
                        fileBytes = Encoding.UTF8.GetBytes(markdownContent);
                        contentType = "text/markdown";
                        fileDownloadName = $"{note.Title}.md";
                        break;
                    case FormatType.PDF:
                        fileBytes = _exportService.ExportPDF(exportDto);
                        contentType = "application/pdf";
                        fileDownloadName = $"{note.Title}.pdf";
                        break;
                    case FormatType.HTML:
                        var htmlContent = _exportService.ExportHtml(exportDto);
                        fileBytes = Encoding.UTF8.GetBytes(htmlContent);
                        contentType = "text/html";
                        fileDownloadName = $"{note.Title}.html";
                        break;
                    default:
                        return BadRequest("Unsupported format type.");
                }

                return File(fileBytes, contentType, fileDownloadName);
            }
            catch (Exception e)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("full")]
        public async Task<IActionResult> GetFullExport()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }
            
            var notes = await _noteService.GetAllAsync(userId);
            if (notes == null || !notes.Any())
            {
                return NotFound();
            }
            
            var exportNotes = notes.Select(note => 
                _exportService.ExportJSON(new ExportDto(note.Title, note.Subtitle, note.Content))
            ).ToList();
            
            var jsonArray = $"[{string.Join(",", exportNotes)}]";
            
            var contentType = "application/json";
            var fileDownloadName = $"Экспорт.json";
            return File(Encoding.UTF8.GetBytes(jsonArray), contentType, fileDownloadName);
        }
    }
}

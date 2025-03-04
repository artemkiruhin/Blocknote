using System.Security.Claims;
using Blocknote.Api.Contracts;
using Blocknote.Core.Models.Enums;
using Blocknote.Core.Services.Sharing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blocknote.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SharingsController : ControllerBase
    {
        private readonly ISharingService _service;

        public SharingsController(ISharingService service)
        {
            _service = service;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
                var sharings = await _service.GetSharingsAsync(userId);
                return Ok(new {sharings});
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Add([FromBody] SharingCreateRequestModel request)
        {
            try
            {
                var userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
                var finishDate = request.HasFinishDate ? request.FinishDate : DateTime.MaxValue;
                var type = request.AllowedAll ? SharingType.All : SharingType.Registered;
                var result = await _service.CreateSharingCodeAsync(userId, request.NoteId, finishDate, type);
                return Ok(new {id = result});
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }
        }
        
        [HttpPost("delete")]
        public async Task<IActionResult> Delete(Guid sharingId)
        {
            try
            {
                var userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
                var result = await _service.DeleteSharingCodeAsync(sharingId.ToString(), userId);
                return Ok(new {result});
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }
        }
        
        [HttpPost("edit")]
        public async Task<IActionResult> Edit([FromBody] SharingCreateRequestModel request)
        {
            try
            {
                // upd stub
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }
        }
    }
}

using System.Security.Claims;
using Blocknote.Api.Contracts;
using Blocknote.Core.Models.Enums;
using Blocknote.Core.Services.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blocknote.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetAll(Guid id)
        {
            try
            {
                var userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
                var sharing = await _service.GetSharingCodeAsync(id);
                return Ok(new {sharing});
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }
        }

        [HttpGet("code/{code}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll(string code)
        {
            try
            {
                var userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
                
                var sharing = await _service.GetSharingCodeAsync(code);
                return Ok(new {sharing});
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
                var finishDate = request.HasFinishDate ? request.FinishDate.Value : DateTime.MaxValue;
                var type = request.AllowedAll ? SharingType.Public : SharingType.Registered;
                var result = await _service.CreateSharingAsync(userId, request.NoteId, finishDate, type);
                return Ok(new {id = result.Id});
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }
        }
        
        [HttpDelete("delete/{sharingId:guid}")]
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
        
        [HttpPut("update")]
        public async Task<IActionResult> Edit([FromBody] UpdateSharingRequestModel request)
        {
            try
            {
                var result = await _service.UpdateSharingAsync(
                    request.Id,
                    request.IsAllowedAll,
                    request.HasExpires,
                    request.ExpiresAt
                );
                     
                return Ok(new {newSharing = result});
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }
        }
    }
}

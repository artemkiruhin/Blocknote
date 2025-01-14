using Blocknote.Core.Services.Base;
using Blocknote.Core.Services.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blocknote.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotesController : ControllerBase
    {
        private readonly INoteService _service;
        private readonly IJwtService _jwtService;
        private readonly IUserService _userService;

        public NotesController(INoteService service, IJwtService jwtService, IUserService userService)
        {
            _service = service;
            _jwtService = jwtService;
            _userService = userService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var userId = User.FindFirst("UserId")?.Value;
                if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid)) return Unauthorized();
                var notes = await _service.GetAllAsync(userGuid);
                return Ok(notes);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
        
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var userId = User.FindFirst("UserId")?.Value;
                if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid)) return Unauthorized();
                var note = await _service.GetInfoAsync(userGuid, id);
                return Ok(note);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
    }
}

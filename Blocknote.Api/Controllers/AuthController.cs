using Blocknote.Api.Contracts;
using Blocknote.Core.Services.Base;
using Blocknote.Core.Services.Hasher;
using Blocknote.Core.Services.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Blocknote.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly IJwtService _jwtService;
        private readonly IHashService _hasher;

        public AuthController(IUserService service, IJwtService jwtService, IHashService hasher)
        {
            _service = service;
            _jwtService = jwtService;
            _hasher = hasher;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel request)
        {
            try
            {
                var result = await _service.Login(request.Username, _hasher.Compute(request.Password));
                if (result == string.Empty) return Unauthorized();
                Response.Cookies.Append("jwt", result, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false
                });
                return Ok(new {token = result});
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestModel request)
        {
            try
            {
                var result = await _service.Register(request.Username, _hasher.Compute(request.Password));
                if (!result) return Unauthorized();
                return Created();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
        
        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            try
            {
                Response.Cookies.Delete("jwt");
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
        
        [HttpGet("validate")]
        [Authorize]
        public async Task<IActionResult> Validate()
        {
            try
            {
                var userId = User.FindFirst("UserId")?.Value;
                if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid)) return Unauthorized();
                
                var user = await _service.GetInfoAsync(userGuid);
                if (user == null) return Unauthorized();
                
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
        
    }
}

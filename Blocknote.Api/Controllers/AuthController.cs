using Blocknote.Api.Contracts;
using Blocknote.Core.Services.Base;
using Blocknote.Core.Services.Hasher;
using Blocknote.Core.Services.Jwt;
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
                    Secure = false,
                    SameSite = SameSiteMode.Strict
                });
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> Login([FromBody] RegisterRequestModel request)
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
        
    }
}

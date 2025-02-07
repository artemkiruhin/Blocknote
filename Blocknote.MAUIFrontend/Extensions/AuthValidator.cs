using Blocknote.Core.Services.Jwt;

namespace Blocknote.MAUIFrontend.Extensions
{
    public class AuthValidator
    {
        private readonly IJwtService _jwtService;
        public AuthValidator(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }
        public async Task<bool> ValidateUser()
        {
            var token = await SecureStorage.Default.GetAsync("jwt");
            if (string.IsNullOrEmpty(token)) return false;
            
            var userId = _jwtService.GetUserId(token);
            if (userId == Guid.Empty) return false;
            
            return true;
        }
    }
}

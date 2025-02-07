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
        public async Task<(bool, Guid)> ValidateUser()
        {
            var token = await SecureStorage.Default.GetAsync("jwt");
            if (string.IsNullOrEmpty(token)) return (false, Guid.Empty);
            
            var userId = _jwtService.GetUserId(token);
            if (userId == Guid.Empty) return (false, Guid.Empty);

            return (true, userId);
        }
    }
}

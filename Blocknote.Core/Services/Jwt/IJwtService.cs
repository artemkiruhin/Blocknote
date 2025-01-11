namespace Blocknote.Core.Services.Jwt;

public interface IJwtService
{
    string GenerateToken(Guid userId);
    bool ValidateToken(string token);
    Guid GetUserId(string token);
}
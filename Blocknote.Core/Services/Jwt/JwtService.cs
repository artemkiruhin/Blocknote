using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Blocknote.Core.Services.Jwt;

public class JwtService : IJwtService
{
    private readonly byte[] _encodedKey;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _expiresInHours;

    public JwtService(byte[] encodedKey, string issuer, string audience, int expiresInHours)
    {
        _encodedKey = encodedKey;
        _issuer = issuer;
        _audience = audience;
        _expiresInHours = expiresInHours;
    }
    
    public string GenerateToken(Guid userId)
    {
        var claims = new Claim[]
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        
        var key = new SymmetricSecurityKey(_encodedKey);
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            issuer: _issuer,
            audience: _audience,
            expires: DateTime.Now.AddMinutes(_expiresInHours),
            signingCredentials: creds
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public bool ValidateToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(_encodedKey);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _issuer,
                ValidateAudience = true,
                ValidAudience = _audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero 
            };
            
            tokenHandler.ValidateToken(token, validationParameters, out _);
        
            return true; 
        }
        catch (SecurityTokenException)
        {
            return false; 
        }
        catch (Exception)
        {
            return false;
        }
    }

    public Guid GetUserId(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(_encodedKey);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _issuer,
                ValidateAudience = true,
                ValidAudience = _audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero 
            };
            
            var claims = tokenHandler.ValidateToken(token, validationParameters, out _);
            var value = claims.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
            return value != null ? Guid.Parse(value) : Guid.Empty;
        }
        catch (Exception e)
        {
            return Guid.Empty;
        }
    }
}
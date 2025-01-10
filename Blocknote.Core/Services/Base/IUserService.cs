using Blocknote.Core.Models.Dtos;

namespace Blocknote.Core.Services.Base;

public interface IUserService
{
    Task<UserDto?> GetInfoAsync(Guid userId);
    Task<UserDto?> GetByUsernameAsync(string username);
    Task<UserDto?> GetByUsernameAndPasswordAsync(string username, string password);
    Task<bool> Register(string username, string passwordHash);
    Task<bool> ChangePassword(Guid id, string oldPassword, string newPassword);
    Task<string> Login(string username, string password);
    Task<bool> DeleteUser(Guid userId);
}
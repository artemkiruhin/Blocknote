using Blocknote.Core.Database.Repositories.Base;
using Blocknote.Core.Models.Dtos;
using Blocknote.Core.Models.Entities;
using Blocknote.Core.Services.Base;
using Blocknote.Core.Services.Jwt;
using NotImplementedException = System.NotImplementedException;

namespace Blocknote.Core.Services.Entity;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IJwtService _jwtService;

    public UserService(IUserRepository repository, IJwtService jwtService)
    {
        _repository = repository;
        _jwtService = jwtService;
    }

    public async Task<UserDto?> GetInfoAsync(Guid userId)
    {
        try
        {
            var user = await _repository.GetByIdAsync(userId);
            if (user != null)
                return new UserDto()
                {
                    Id = user.Id,
                    Username = user.Username,
                    RegisteredAt = user.RegisteredAt,
                    Notes = user.Notes.Select(n => new NoteDto()
                    {
                        Content = n.Content,
                        CreatedAt = n.CreatedAt,
                        UpdatedAt = n.UpdatedAt,
                        Subtitle = n.Subtitle,
                        Title = n.Title,
                    }).ToList()
                };
            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<UserDto?> GetByUsernameAsync(string username)
    {
        try
        {
            var users = await _repository.FindAsync(u => u.Username == username);
            var user = users.FirstOrDefault();
            if (user != null)
                return new UserDto()
                {
                    Id = user.Id,
                    Username = user.Username,
                    RegisteredAt = user.RegisteredAt,
                    Notes = user.Notes.Select(n => new NoteDto()
                    {
                        Content = n.Content,
                        CreatedAt = n.CreatedAt,
                        UpdatedAt = n.UpdatedAt,
                        Subtitle = n.Subtitle,
                        Title = n.Title,
                    }).ToList()
                };
            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<UserDto?> GetByUsernameAndPasswordAsync(string username, string password)
    {
        try
        {
            var users = await _repository.FindAsync(u => u.Username == username && u.PasswordHash == password);
            var user = users.FirstOrDefault();
            if (user != null)
                return new UserDto()
                {
                    Id = user.Id,
                    Username = user.Username,
                    RegisteredAt = user.RegisteredAt,
                    Notes = user.Notes.Select(n => new NoteDto()
                    {
                        Content = n.Content,
                        CreatedAt = n.CreatedAt,
                        UpdatedAt = n.UpdatedAt,
                        Subtitle = n.Subtitle,
                        Title = n.Title,
                    }).ToList()
                };
            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> Register(string username, string passwordHash)
    {
        try
        {
            var users = await _repository.FindAsync(x => x.Username == username && x.PasswordHash == passwordHash);
            if (users.Any()) return false;
            var user = UserEntity.Create(username, passwordHash);
            return await _repository.AddAsync(user);
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public async Task<bool> ChangePassword(Guid id, string oldPassword, string newPassword)
    {
        try
        {
            if (oldPassword == newPassword) return false;
            var user = await _repository.GetByIdAsync(id);
            if (user == null) return false;
            user.PasswordHash = newPassword;
            return await _repository.EditAsync(user);
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public async Task<string> Login(string username, string password)
    {
        try
        {
            var users = await _repository.FindAsync(u => u.Username == username && u.PasswordHash == password);
            var user = users.FirstOrDefault();
            if (user is null) return string.Empty;
            var token = _jwtService.GenerateToken(user.Id);
            return token;
        }
        catch (Exception e)
        {
            return string.Empty;
        }
    }

    public async Task<bool> DeleteUser(Guid userId)
    {
        try
        {
            var user  = await _repository.GetByIdAsync(userId);
            if (user is null) return false;
            
            var result = await _repository.DeleteAsync(user.Id);
            return result;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}
using System.Text;
using Blocknote.Core.Database;
using Blocknote.Core.Database.Repositories;
using Blocknote.Core.Database.Repositories.Base;
using Blocknote.Core.Services.Base;
using Blocknote.Core.Services.Entity;
using Blocknote.Core.Services.Hasher;
using Blocknote.Core.Services.Jwt;
using Blocknote.Core.Services.Sharing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Blocknote.MAUIFrontend;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => { fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"); });
        
        IConfigurationRoot configuration;
        try
        {
            string filePath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Файл конфигурации appsettings.json не найден.");
            }

            configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
        }
        catch (Exception ex)
        {
            throw new Exception("Ошибка при загрузке конфигурации: " + ex.Message, ex);
        }
        
        builder.Services.AddMauiBlazorWebView();

        builder.Services.AddTransient<AppDbContext>(provider =>
        {
            return new AppDbContext(configuration.GetConnectionString("Database") ?? throw new NullReferenceException("Строка подключения к БД не указана в конфигурации."));
        }
        );
        
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<INoteRepository, NoteRepository>();
        builder.Services.AddScoped<ISharingRepository, SharingNoteRepository>();
        
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<INoteService, NoteService>();
        builder.Services.AddScoped<ISharingService, SharingService>();

        builder.Services.AddScoped<IHashService, Sha256HashService>();
        builder.Services.AddScoped<IJwtService>(provider =>
        {
            try
            {
                var key = Encoding.UTF8.GetBytes(configuration["jwt:key"] 
                            ?? throw new NullReferenceException("JWT ключ не указан в конфигурации."));
                var issuer = configuration["jwt:issuer"] 
                            ?? throw new NullReferenceException("JWT издатель не указан в конфигурации.");
                var audience = configuration["jwt:audience"] 
                            ?? throw new NullReferenceException("JWT аудитория не указана в конфигурации.");
                var expires = int.Parse(configuration["jwt:expires"] 
                            ?? throw new NullReferenceException("JWT время жизни не указано в конфигурации."));

                return new JwtService(key, issuer, audience, expires);
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при настройке JWT-сервиса: " + ex.Message, ex);
            }
        });

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
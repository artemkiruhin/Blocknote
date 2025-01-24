using System.Text;
using Blocknote.Core.Database;
using Blocknote.Core.Database.Repositories;
using Blocknote.Core.Database.Repositories.Base;
using Blocknote.Core.Services.Base;
using Blocknote.Core.Services.Entity;
using Blocknote.Core.Services.Hasher;
using Blocknote.Core.Services.Jwt;
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

        var configuration = builder.Configuration;
        
        builder.Services.AddMauiBlazorWebView();

        
        
        builder.Services.AddDbContext<AppDbContext>(optionsBuilder =>
        {
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("Database"));
            optionsBuilder.UseLazyLoadingProxies();
        });

        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<INoteRepository, NoteRepository>();

        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<INoteService, NoteService>();

        builder.Services.AddScoped<IHashService, Sha256HashService>();
        builder.Services.AddScoped<IJwtService>(provider =>
        {
            var key = Encoding.UTF8.GetBytes(configuration["jwt:key"] ?? throw new NullReferenceException());
            var issuer = configuration["jwt:issuer"] ?? throw new NullReferenceException();
            var audience = configuration["jwt:audience"] ?? throw new NullReferenceException();
            var expires = int.Parse(configuration["jwt:expires"] ?? throw new NullReferenceException());

            return new JwtService(key, issuer, audience, expires);
        });
        builder.Services.AddScoped<IHashService, Sha256HashService>();
        
        
        
#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
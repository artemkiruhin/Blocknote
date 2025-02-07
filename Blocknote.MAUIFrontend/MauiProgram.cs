using Blocknote.Core.Database;
using Blocknote.Core.Database.Repositories;
using Blocknote.Core.Database.Repositories.Base;
using Blocknote.Core.Services.Base;
using Blocknote.Core.Services.Entity;
using Blocknote.Core.Services.Hasher;
using Blocknote.Core.Services.Jwt;
using Blocknote.Core.Services.Sharing;
using Blocknote.MAUIFrontend.Extensions;
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
        
        builder.Services.AddMauiBlazorWebView();

        builder.Services.AddTransient<AppDbContext>();

        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<INoteRepository, NoteRepository>();
        builder.Services.AddScoped<ISharingRepository, SharingNoteRepository>();
        
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<INoteService, NoteService>();
        builder.Services.AddScoped<ISharingService, SharingService>();
        builder.Services.AddScoped<IHashService, Sha256HashService>();
        builder.Services.AddScoped<IJwtService>(provider => JwtServiceFactory.Create());
        builder.Services.AddScoped<AuthValidator>();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
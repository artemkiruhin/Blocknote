using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Blocknote.Core.Database;
using Blocknote.Core.Database.Repositories;
using Blocknote.Core.Database.Repositories.Base;
using Blocknote.Core.Services.Base;
using Blocknote.Core.Services.Entity;
using Blocknote.Core.Services.Export;
using Blocknote.Core.Services.Extensions.Format;
using Blocknote.Core.Services.Hasher;
using Blocknote.Core.Services.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
var configuration = builder.Configuration;

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowed(_ => true);
    });
});

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var token = context.Request.Cookies["jwt"];
                if (!string.IsNullOrEmpty(token)) context.Token = token;
                return Task.CompletedTask;
                
            },
            OnTokenValidated = context =>
            {
                try
                {
                    //Console.WriteLine("Token validated successfully");
                    var claims = context.Principal?.Claims;
                    var userIdClaim = claims?.FirstOrDefault(c => c.Type == "UserId");
                    if (Guid.TryParse(userIdClaim?.Value, out var userId))
                    {
                        //Console.WriteLine($"UserId from token: {userId}");
                        var identity = context.Principal.Identity as ClaimsIdentity;
                        if (identity != null)
                        {
                            if (!identity.HasClaim(c => c.Type == "UserId"))
                            {
                                identity.AddClaim(new Claim("UserId", userId.ToString()));
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Failed to parse UserId from token");
                        context.Fail("Invalid UserId in token");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Token validation error: {ex.Message}");
                    context.Fail("Invalid token");
                }
                return Task.CompletedTask;
            }


        };

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["jwt:issuer"],
            ValidAudience = configuration["jwt:audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwt:key"]))
        };
    });

builder.Services.AddDbContext<AppDbContext>(optionsBuilder =>
{
    optionsBuilder.UseNpgsql(configuration.GetConnectionString("Database"));
    optionsBuilder.UseLazyLoadingProxies();
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddScoped<ISharingRepository, SharingNoteRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<INoteService, NoteService>();
builder.Services.AddScoped<ISharingService, SharingService>();
builder.Services.AddScoped<ITextFormatter, TextFormatter>();
builder.Services.AddScoped<IExportService, ExportService>();

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

builder.Services.AddControllers();

var app = builder.Build();

app.UseCors("AllowAll");
app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
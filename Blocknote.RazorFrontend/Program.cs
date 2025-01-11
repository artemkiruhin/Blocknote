using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using Blocknote.Core.Database;
using Blocknote.Core.Database.Repositories;
using Blocknote.Core.Database.Repositories.Base;
using Blocknote.Core.Services.Base;
using Blocknote.Core.Services.Entity;
using Blocknote.Core.Services.Hasher;
using Blocknote.Core.Services.Jwt;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddRazorPages();

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = BearerTokenDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = BearerTokenDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var cookie = context.Request.Cookies["jwt"];
                if (!string.IsNullOrEmpty(cookie)) context.Token = cookie;
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                var claims = context.Principal?.Claims;
                var userIdClaim = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
                if (Guid.TryParse(userIdClaim?.Value, out var userId)) context.HttpContext.Items["UserId"] = userId;
                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                if (!context.HttpContext.User.Identity?.IsAuthenticated ?? true) context.Response.Redirect("/login");
                return Task.CompletedTask;
            }
        };

        options.TokenValidationParameters = new TokenValidationParameters()
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

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<INoteService, NoteService>();

builder.Services.AddScoped<IHashService, Sha256HashService>();
builder.Services.AddScoped<IJwtService>(provider =>
{
    var key = Encoding.UTF8.GetBytes(configuration["jwt:key"] ?? throw new NullReferenceException());
    var issuer = configuration["jwt:issuer"] ?? throw new NullReferenceException();
    var audience = configuration["jwt:audience"] ?? throw new NullReferenceException();
    var expires = int.Parse(configuration["jwt:expires"] ?? throw new NullReferenceException());

    return new JwtService(key, audience, issuer, expires);
});
builder.Services.AddScoped<IHashService, Sha256HashService>();



var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
    .WithStaticAssets();

app.Run();
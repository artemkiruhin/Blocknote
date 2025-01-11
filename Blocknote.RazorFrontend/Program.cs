using System.Security.Cryptography;
using System.Text;
using Blocknote.Core.Database;
using Blocknote.Core.Database.Repositories;
using Blocknote.Core.Database.Repositories.Base;
using Blocknote.Core.Services.Base;
using Blocknote.Core.Services.Entity;
using Blocknote.Core.Services.Hasher;
using Blocknote.Core.Services.Jwt;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddRazorPages();

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

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
    .WithStaticAssets();

app.Run();
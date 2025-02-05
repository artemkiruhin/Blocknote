using AngleSharp;
using Blocknote.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text;
using Blocknote.Core.Database;
using Blocknote.Core.Database.Repositories;
using Blocknote.Core.Database.Repositories.Base;
using Blocknote.Core.Services.Base;
using Blocknote.Core.Services.Entity;
using Blocknote.Core.Services.Hasher;
using Blocknote.Core.Services.Jwt;
using Blocknote.Core.Services.Sharing;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Blocknote.Core.Database;

public class AppDbContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<NoteEntity> Notes { get; set; }
    public DbSet<SharingNoteEntity> Sharings { get; set; }


    //public AppDbContext(DbContextOptions<AppDbContext> options)
    //    : base(options)
    //{
    //}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        try
        {
            string filePath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Файл конфигурации appsettings.json не найден.");
            }

            var text = File.ReadAllText(filePath);
            var jsonData = JsonSerializer.Deserialize<JsonElement>(text);

            var connectionString = jsonData
            .GetProperty("ConnectionStrings")
            .GetProperty("Database")
            .GetString();

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Строка подключения не найдена в конфигурации.");
            }

            optionsBuilder.UseNpgsql(connectionString);
            optionsBuilder.UseLazyLoadingProxies();
        }
        catch (Exception ex)
        {
            throw new Exception("Ошибка при загрузке конфигурации: " + ex.Message, ex);
        }
    }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>(builder =>
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("uuid");
            builder.HasIndex(e => e.Id).IsUnique();

            builder.Property(e => e.Username).IsRequired();
            builder.HasIndex(e => e.Username).IsUnique();

            builder.Property(e => e.PasswordHash).IsRequired();
            builder.Property(e => e.RegisteredAt).IsRequired();

            builder
                .HasMany(e => e.Notes)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId);
            builder
                .HasMany(e => e.Sharings)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId);
        });
        modelBuilder.Entity<NoteEntity>(builder =>
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("uuid");
            builder.HasIndex(e => e.Id).IsUnique();

            builder.Property(e => e.Title).IsRequired();
            builder.Property(e => e.CreatedAt).IsRequired();

            builder
                .HasOne(e => e.User)
                .WithMany(e => e.Notes)
                .HasForeignKey(e => e.UserId);
            builder
                .HasMany(e => e.Sharings)
                .WithOne(e => e.Note)
                .HasForeignKey(e => e.NoteId);
        });
        modelBuilder.Entity<SharingNoteEntity>(builder =>
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("uuid");
            builder.HasIndex(e => e.Id).IsUnique();

            builder.Property(e => e.CreatedAt).IsRequired();
            builder.Property(e => e.Type).IsRequired();
            builder.Property(e => e.CloseAt).IsRequired();

            builder
                .HasOne(e => e.User)
                .WithMany(e => e.Sharings)
                .HasForeignKey(e => e.UserId);
            builder
                .HasOne(e => e.Note)
                .WithMany(e => e.Sharings)
                .HasForeignKey(e => e.NoteId);
        });
    }
}
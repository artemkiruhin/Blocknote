using Blocknote.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blocknote.Core.Database;

public class AppDbContext : DbContext
{
    private readonly string _connectionString;

    public DbSet<UserEntity> Users { get; set; }
    public DbSet<NoteEntity> Notes { get; set; }
    public DbSet<SharingNoteEntity> Sharings { get; set; }


    public AppDbContext(DbContextOptions<AppDbContext> options, string connectionString)
        : base(options)
    {
        _connectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(_connectionString);
            optionsBuilder.UseLazyLoadingProxies();
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
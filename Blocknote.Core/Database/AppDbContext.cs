﻿using Blocknote.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blocknote.Core.Database;

public class AppDbContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<NoteEntity> Notes { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

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
        });
    }
}
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace MangaReader.Models;

public partial class MangaDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public MangaDbContext()
    {
        // Load configuration from appsettings.json
        _configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true)
            .Build();

    }

    public MangaDbContext(DbContextOptions<MangaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Chapter> Chapters { get; set; }

    public virtual DbSet<ChapterImage> ChapterImages { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Manga> Mangas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Username=postgres;Password=905000Nxk@;Database=MangaDB;");
            //string h = _configuration.GetConnectionString("DefaultConnection");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chapter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("chapters_pkey");

            entity.ToTable("chapters");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ChapterLink)
                .HasMaxLength(255)
                .HasColumnName("chapter_link");
            entity.Property(e => e.ChapterNumber)
                .HasMaxLength(255)
                .HasColumnName("chapter_number");
            entity.Property(e => e.MangaId).HasColumnName("mangaId");

            entity.HasOne(d => d.Manga).WithMany(p => p.Chapters)
                .HasForeignKey(d => d.MangaId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("chapters_mangaId_fkey");
        });

        modelBuilder.Entity<ChapterImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("chapter_images_pkey");

            entity.ToTable("chapter_images");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ChapterId).HasColumnName("chapterId");
            entity.Property(e => e.ChapterImageUrl)
                .HasMaxLength(255)
                .HasColumnName("chapter_image_url");

            entity.HasOne(d => d.Chapter).WithMany(p => p.ChapterImages)
                .HasForeignKey(d => d.ChapterId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("chapter_images_chapterId_fkey");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("genres_pkey");

            entity.ToTable("genres");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.GenreName)
                .HasMaxLength(255)
                .HasColumnName("genre_name");

            entity.HasMany(d => d.Mangas).WithMany(p => p.Genres)
                .UsingEntity<Dictionary<string, object>>(
                    "MangaGenre",
                    r => r.HasOne<Manga>().WithMany()
                        .HasForeignKey("MangaId")
                        .HasConstraintName("manga_genre_mangaId_fkey"),
                    l => l.HasOne<Genre>().WithMany()
                        .HasForeignKey("GenreId")
                        .HasConstraintName("manga_genre_genreId_fkey"),
                    j =>
                    {
                        j.HasKey("GenreId", "MangaId").HasName("manga_genre_pkey");
                        j.ToTable("manga_genre");
                        j.IndexerProperty<int>("GenreId").HasColumnName("genreId");
                        j.IndexerProperty<int>("MangaId").HasColumnName("mangaId");
                    });
        });

        modelBuilder.Entity<Manga>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("mangas_pkey");

            entity.ToTable("mangas");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Author)
                .HasMaxLength(255)
                .HasColumnName("author");
            entity.Property(e => e.CoverImageUrl)
                .HasMaxLength(255)
                .HasColumnName("cover_image_url");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

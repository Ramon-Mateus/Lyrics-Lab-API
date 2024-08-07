﻿using Lyrics_Lab.Models;
using Microsoft.EntityFrameworkCore;

namespace Lyrics_Lab.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        public DbSet<Album> Albums => Set<Album>();
        public DbSet<Song> Songs => Set<Song>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity => { entity.HasIndex(e => e.Email).IsUnique(); });

            modelBuilder.Entity<Album>()
                .HasOne(p => p.User)
                .WithMany(u => u.Albums)
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<Song>()
                .HasOne(s => s.Album)
                .WithMany(p => p.Songs)
                .HasForeignKey(s => s.AlbumId);
        }
    }
}

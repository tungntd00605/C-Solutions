using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMusic.Models
{
    public class MyDbContext: DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        { }

        public DbSet<Song> Songs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SongCategory> SongCategories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=MyMusic;Trusted_Connection=True;ConnectRetryCount=0");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SongCategory>(songCategory =>
            {
                songCategory.HasKey(sc => new { sc.SongId, sc.CategoryId });

                songCategory.HasOne(sc => sc.Song)
                        .WithMany(s => s.SongCategory)
                        .HasForeignKey(sc => sc.SongId)
                        .IsRequired();

                songCategory.HasOne(sc => sc.Category)
                        .WithMany(c => c.SongCategory)
                        .HasForeignKey(sc => sc.CategoryId)
                        .IsRequired();

            });                
        }
    }
}

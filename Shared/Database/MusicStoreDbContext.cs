using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MusicStoreDemo.Database.Entities;
using MusicStoreDemo.Database.Entities.Relationships;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MusicStoreDemo.Database
{
    public class MusicStoreDbContext : IdentityDbContext<DbUser, DbRole, string>
    {
        public MusicStoreDbContext(DbContextOptions<MusicStoreDbContext> options) : base(options) {
        }

        public DbSet<DbArtist> Artists { get; set; }
        public DbSet<DbAlbum> Albums { get; set; }
        public DbSet<DbGenre> Genres { get; set; }
        public DbSet<DbTrack> Tracks { get; set; }
        public DbSet<DbAlbumGroup> AlbumGroups { get; set; }
        public DbSet<DbImageResource> ImageResources { get; set; }


        // define relationships
        public DbSet<DbArtistGenre> ArtistGenres { get; set; }
        public DbSet<DbAlbumGenre> AlbumGenres { get; set; }
        public DbSet<DbAlbumGroupAlbumPosition> AlbumGroupListPositions { get; set; }
        public DbSet<DbUserPurchasedAlbum> UserAlbums { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // rename identity tables
            builder.Entity<DbUser>().ToTable("User");
            builder.Entity<DbRole>().ToTable("Role");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogin");
            
//dentityUserClaim<TKey>, IdentityUserRole<TKey>, IdentityUserLogin<TKey>, IdentityRoleClaim<TKey>, IdentityUserToken<TKey>>
            // define composite keys for N to N relationships
            builder.Entity<DbAlbumGenre>().HasKey(t => new { t.AlbumId, t.GenreId });
            builder.Entity<DbArtistGenre>().HasKey(t => new { t.ArtistId, t.GenreId });
            builder.Entity<DbAlbumGroupAlbumPosition>().HasKey(t => new { t.AlbumId, t.GroupId });
            builder.Entity<DbUserPurchasedAlbum>().HasKey(t => new { t.AlbumId, t.UserId });

            builder.ForMySqlUseIdentityColumns();
            builder.ForSqlServerUseIdentityColumns();

            // set up unique indexes on any required tables
            builder.Entity<DbGenre>().HasIndex(x => x.Name).IsUnique(true);
            builder.Entity<DbAlbumGroup>().HasIndex(x => x.Key).IsUnique(true);

        }
    }
}

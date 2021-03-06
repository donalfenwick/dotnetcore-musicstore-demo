﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MusicStoreDemo.Database;

namespace MusicStoreDemo.Database.Migrations
{
    [DbContext(typeof(MusicStoreDbContext))]
    partial class MusicStoreDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.0-rc1-32029")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogin");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("MusicStoreDemo.Database.Entities.DbAlbum", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AlbumCoverImageId");

                    b.Property<int>("ArtistId");

                    b.Property<DateTime>("CreatedUtc");

                    b.Property<string>("DescriptionText")
                        .HasMaxLength(2000);

                    b.Property<string>("Label")
                        .HasMaxLength(200);

                    b.Property<double>("Price");

                    b.Property<string>("Producer")
                        .HasMaxLength(200);

                    b.Property<int>("PublishStatus");

                    b.Property<DateTime>("ReleaseDate");

                    b.Property<string>("Title")
                        .HasMaxLength(200);

                    b.Property<int>("TotalDurationInSeconds");

                    b.Property<DateTime>("UpdatedUtc");

                    b.HasKey("Id");

                    b.HasIndex("AlbumCoverImageId");

                    b.HasIndex("ArtistId");

                    b.ToTable("Album");
                });

            modelBuilder.Entity("MusicStoreDemo.Database.Entities.DbAlbumGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedUtc");

                    b.Property<string>("Key")
                        .HasMaxLength(100);

                    b.Property<string>("Name")
                        .HasMaxLength(200);

                    b.Property<DateTime>("UpdatedUtc");

                    b.HasKey("Id");

                    b.HasIndex("Key")
                        .IsUnique()
                        .HasFilter("[Key] IS NOT NULL");

                    b.ToTable("AlbumGroups");
                });

            modelBuilder.Entity("MusicStoreDemo.Database.Entities.DbArtist", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BioImageId");

                    b.Property<string>("BioText")
                        .HasMaxLength(2000);

                    b.Property<DateTime>("CreatedUtc");

                    b.Property<string>("Name")
                        .HasMaxLength(200);

                    b.Property<int>("PublishStatus");

                    b.Property<DateTime>("UpdatedUtc");

                    b.HasKey("Id");

                    b.HasIndex("BioImageId");

                    b.ToTable("Artist");
                });

            modelBuilder.Entity("MusicStoreDemo.Database.Entities.DbGenre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedUtc");

                    b.Property<string>("Name")
                        .HasMaxLength(200);

                    b.Property<DateTime>("UpdatedUtc");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("Genre");
                });

            modelBuilder.Entity("MusicStoreDemo.Database.Entities.DbImageResource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedUtc");

                    b.Property<byte[]>("Data")
                        .HasMaxLength(2097152);

                    b.Property<string>("Filename")
                        .HasMaxLength(200);

                    b.Property<string>("MimeType")
                        .HasMaxLength(100);

                    b.Property<DateTime>("UpdatedUtc");

                    b.HasKey("Id");

                    b.ToTable("ImageResource");
                });

            modelBuilder.Entity("MusicStoreDemo.Database.Entities.DbRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Description")
                        .HasMaxLength(500);

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("MusicStoreDemo.Database.Entities.DbTrack", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AlbumId");

                    b.Property<DateTime>("CreatedUtc");

                    b.Property<int>("DurationInSeconds");

                    b.Property<string>("Title")
                        .HasMaxLength(200);

                    b.Property<int>("TrackNumber");

                    b.Property<DateTime>("UpdatedUtc");

                    b.HasKey("Id");

                    b.HasIndex("AlbumId");

                    b.ToTable("Track");
                });

            modelBuilder.Entity("MusicStoreDemo.Database.Entities.DbUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<DateTime?>("DateOfBirth");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("Firstname")
                        .HasMaxLength(100);

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("Surname")
                        .HasMaxLength(100);

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("User");
                });

            modelBuilder.Entity("MusicStoreDemo.Database.Entities.DbUserPurchasedAlbum", b =>
                {
                    b.Property<int>("AlbumId");

                    b.Property<string>("UserId");

                    b.Property<DateTime>("CreatedUtc");

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.HasKey("AlbumId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserAlbums");
                });

            modelBuilder.Entity("MusicStoreDemo.Database.Entities.Relationships.DbAlbumGenre", b =>
                {
                    b.Property<int>("AlbumId");

                    b.Property<int>("GenreId");

                    b.Property<DateTime>("CreatedUtc");

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.HasKey("AlbumId", "GenreId");

                    b.HasIndex("GenreId");

                    b.ToTable("AlbumGenres");
                });

            modelBuilder.Entity("MusicStoreDemo.Database.Entities.Relationships.DbAlbumGroupAlbumPosition", b =>
                {
                    b.Property<int>("AlbumId");

                    b.Property<int>("GroupId");

                    b.Property<DateTime>("CreatedUtc");

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("PositionIndex");

                    b.HasKey("AlbumId", "GroupId");

                    b.HasIndex("GroupId");

                    b.ToTable("AlbumGroupListPositions");
                });

            modelBuilder.Entity("MusicStoreDemo.Database.Entities.Relationships.DbArtistGenre", b =>
                {
                    b.Property<int>("ArtistId");

                    b.Property<int>("GenreId");

                    b.Property<DateTime>("CreatedUtc");

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.HasKey("ArtistId", "GenreId");

                    b.HasIndex("GenreId");

                    b.ToTable("ArtistGenres");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("MusicStoreDemo.Database.Entities.DbRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("MusicStoreDemo.Database.Entities.DbUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("MusicStoreDemo.Database.Entities.DbUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("MusicStoreDemo.Database.Entities.DbRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MusicStoreDemo.Database.Entities.DbUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("MusicStoreDemo.Database.Entities.DbUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MusicStoreDemo.Database.Entities.DbAlbum", b =>
                {
                    b.HasOne("MusicStoreDemo.Database.Entities.DbImageResource", "AlbumCoverImage")
                        .WithMany()
                        .HasForeignKey("AlbumCoverImageId");

                    b.HasOne("MusicStoreDemo.Database.Entities.DbArtist", "Artist")
                        .WithMany("Albums")
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MusicStoreDemo.Database.Entities.DbArtist", b =>
                {
                    b.HasOne("MusicStoreDemo.Database.Entities.DbImageResource", "BioImage")
                        .WithMany()
                        .HasForeignKey("BioImageId");
                });

            modelBuilder.Entity("MusicStoreDemo.Database.Entities.DbTrack", b =>
                {
                    b.HasOne("MusicStoreDemo.Database.Entities.DbAlbum", "Album")
                        .WithMany("Tracks")
                        .HasForeignKey("AlbumId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MusicStoreDemo.Database.Entities.DbUserPurchasedAlbum", b =>
                {
                    b.HasOne("MusicStoreDemo.Database.Entities.DbAlbum", "Album")
                        .WithMany("UserPurchases")
                        .HasForeignKey("AlbumId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MusicStoreDemo.Database.Entities.DbUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MusicStoreDemo.Database.Entities.Relationships.DbAlbumGenre", b =>
                {
                    b.HasOne("MusicStoreDemo.Database.Entities.DbAlbum", "Album")
                        .WithMany("AlbumGenres")
                        .HasForeignKey("AlbumId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MusicStoreDemo.Database.Entities.DbGenre", "Genre")
                        .WithMany("AlbumGenres")
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MusicStoreDemo.Database.Entities.Relationships.DbAlbumGroupAlbumPosition", b =>
                {
                    b.HasOne("MusicStoreDemo.Database.Entities.DbAlbum", "Album")
                        .WithMany("GroupPositions")
                        .HasForeignKey("AlbumId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MusicStoreDemo.Database.Entities.DbAlbumGroup", "Group")
                        .WithMany("Items")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MusicStoreDemo.Database.Entities.Relationships.DbArtistGenre", b =>
                {
                    b.HasOne("MusicStoreDemo.Database.Entities.DbArtist", "Artist")
                        .WithMany("ArtistGenres")
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MusicStoreDemo.Database.Entities.DbGenre", "Genre")
                        .WithMany("ArtistGenres")
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}

﻿// <auto-generated />
using System;
using BeestjeOpJeFeestjeDb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BeestjeOpJeFeestjeDb.Migrations
{
    [DbContext(typeof(MyContext))]
    partial class MyContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BeestjeOpJeFeestje.Models.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("address");

                    b.Property<string>("Card")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "1",
                            AccessFailedCount = 0,
                            Address = "Laan van Meerdervoort 100 2517 AN Den Haag Nederland",
                            ConcurrencyStamp = "e8c99514-7030-4f92-9852-ed858ce99cc9",
                            Email = "employee@example.com",
                            EmailConfirmed = false,
                            LockoutEnabled = false,
                            NormalizedEmail = "EMPLOYEE@EXAMPLE.COM",
                            NormalizedUserName = "JURIAN",
                            PasswordHash = "AQAAAAIAAYagAAAAEIMvC/yk9x8lmI4osYPmQY9pCT9MKpoEb4WT5+BmgsASWTzhSvKBU1WpM1QKxGJphw==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "0c2cf842-41d6-408d-afb5-6034a5f9736d",
                            TwoFactorEnabled = false,
                            UserName = "jurian"
                        },
                        new
                        {
                            Id = "2",
                            AccessFailedCount = 0,
                            Address = "Laan van Meerdervoort 5 2517 AN Den Haag Nederland",
                            ConcurrencyStamp = "2a2dad6b-4860-4636-bbe2-3c844b292d6c",
                            Email = "employee@examples.com",
                            EmailConfirmed = false,
                            LockoutEnabled = false,
                            NormalizedEmail = "EMPLOYEE@EXAMPLES.COM",
                            NormalizedUserName = "ETHAN",
                            PasswordHash = "AQAAAAIAAYagAAAAEDf2V98aqczJl13zX0WDz+SdbZFPIXnzVqCITfPIVkgPoCD50Fr+vxJ+1Ju5vFSB5g==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "9d864012-4c46-4b38-80dd-172bf73f74f4",
                            TwoFactorEnabled = false,
                            UserName = "ethan"
                        });
                });

            modelBuilder.Entity("BeestjeOpJeFeestjeDb.Models.Animal", a =>
                {
                    a.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("animal_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(a.Property<int>("Id"));

                    a.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("imageUrl");

                    a.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("name");

                    a.Property<decimal>("Price")
                        .HasPrecision(16, 2)
                        .HasColumnType("decimal(16,2)")
                        .HasColumnName("price");

                    a.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("type");

                    a.HasKey("Id");

                    a.ToTable("Animal", (string)null);

                    a.HasData(
                        new
                        {
                            Id = 1,
                            ImageUrl = "#",
                            Name = "Aap",
                            Price = 150.00m,
                            Type = "Jungle"
                        },
                        new
                        {
                            Id = 2,
                            ImageUrl = "#",
                            Name = "Olifant",
                            Price = 500.00m,
                            Type = "Jungle"
                        },
                        new
                        {
                            Id = 3,
                            ImageUrl = "#",
                            Name = "Zebra",
                            Price = 200.00m,
                            Type = "Jungle"
                        },
                        new
                        {
                            Id = 4,
                            ImageUrl = "#",
                            Name = "Leeuw",
                            Price = 600.00m,
                            Type = "Jungle"
                        },
                        new
                        {
                            Id = 5,
                            ImageUrl = "#",
                            Name = "Hond",
                            Price = 50.00m,
                            Type = "Boerderij"
                        },
                        new
                        {
                            Id = 6,
                            ImageUrl = "#",
                            Name = "Ezel",
                            Price = 100.00m,
                            Type = "Boerderij"
                        },
                        new
                        {
                            Id = 7,
                            ImageUrl = "#",
                            Name = "Koe",
                            Price = 250.00m,
                            Type = "Boerderij"
                        },
                        new
                        {
                            Id = 8,
                            ImageUrl = "#",
                            Name = "Eend",
                            Price = 30.00m,
                            Type = "Boerderij"
                        },
                        new
                        {
                            Id = 9,
                            ImageUrl = "#",
                            Name = "Kuiken",
                            Price = 10.00m,
                            Type = "Boerderij"
                        },
                        new
                        {
                            Id = 10,
                            ImageUrl = "#",
                            Name = "Pinguïn",
                            Price = 100.00m,
                            Type = "Sneeuw"
                        },
                        new
                        {
                            Id = 11,
                            ImageUrl = "#",
                            Name = "IJsbeer",
                            Price = 350.00m,
                            Type = "Sneeuw"
                        },
                        new
                        {
                            Id = 12,
                            ImageUrl = "#",
                            Name = "Zeehond",
                            Price = 120.00m,
                            Type = "Sneeuw"
                        },
                        new
                        {
                            Id = 13,
                            ImageUrl = "#",
                            Name = "Kameel",
                            Price = 400.00m,
                            Type = "Woestijn"
                        },
                        new
                        {
                            Id = 14,
                            ImageUrl = "#",
                            Name = "Slang",
                            Price = 75.00m,
                            Type = "Woestijn"
                        },
                        new
                        {
                            Id = 15,
                            ImageUrl = "#",
                            Name = "T-Rex",
                            Price = 1000.00m,
                            Type = "VIP"
                        },
                        new
                        {
                            Id = 16,
                            ImageUrl = "#",
                            Name = "Unicorn",
                            Price = 1500.00m,
                            Type = "VIP"
                        });
                });

            modelBuilder.Entity("BeestjeOpJeFeestjeDb.Models.Booking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("booking_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AppUserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateOnly>("BookingDate")
                        .HasColumnType("date")
                        .HasColumnName("booking_date");

                    b.Property<decimal>("Discount")
                        .HasPrecision(16, 2)
                        .HasColumnType("decimal(16,2)")
                        .HasColumnName("discount");

                    b.Property<DateOnly>("EventDate")
                        .HasColumnType("date")
                        .HasColumnName("event_date");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.ToTable("Booking", (string)null);
                });

            modelBuilder.Entity("BeestjeOpJeFeestjeDb.Models.BookingAnimal", b =>
                {
                    b.Property<int>("BookingId")
                        .HasColumnType("int");

                    b.Property<int>("AnimalId")
                        .HasColumnType("int");

                    b.HasKey("BookingId", "AnimalId");

                    b.HasIndex("AnimalId");

                    b.ToTable("BookingAnimal");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "1",
                            Name = "boerderij",
                            NormalizedName = "BOERDERIJ"
                        },
                        new
                        {
                            Id = "2",
                            Name = "klant",
                            NormalizedName = "KLANT"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);

                    b.HasData(
                        new
                        {
                            UserId = "1",
                            RoleId = "1"
                        },
                        new
                        {
                            UserId = "2",
                            RoleId = "2"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("BeestjeOpJeFeestjeDb.Models.Booking", b =>
                {
                    b.HasOne("BeestjeOpJeFeestje.Models.AppUser", "AppUser")
                        .WithMany("Bookings")
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("AppUser");
                });

            modelBuilder.Entity("BeestjeOpJeFeestjeDb.Models.BookingAnimal", b =>
                {
                    b.HasOne("BeestjeOpJeFeestjeDb.Models.Animal", "Animal")
                        .WithMany("BookingAnimals")
                        .HasForeignKey("AnimalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BeestjeOpJeFeestjeDb.Models.Booking", "Booking")
                        .WithMany("BookingAnimals")
                        .HasForeignKey("BookingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Animal");

                    b.Navigation("Booking");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("BeestjeOpJeFeestje.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("BeestjeOpJeFeestje.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BeestjeOpJeFeestje.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("BeestjeOpJeFeestje.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BeestjeOpJeFeestje.Models.AppUser", b =>
                {
                    b.Navigation("Bookings");
                });

            modelBuilder.Entity("BeestjeOpJeFeestjeDb.Models.Animal", b =>
                {
                    b.Navigation("BookingAnimals");
                });

            modelBuilder.Entity("BeestjeOpJeFeestjeDb.Models.Booking", b =>
                {
                    b.Navigation("BookingAnimals");
                });
#pragma warning restore 612, 618
        }
    }
}

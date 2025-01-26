using BeestjeOpJeFeestjeDb;
using BeestjeOpJeFeestjeDb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BeestjeOpJeFeestje.Models {
    public class MyContext : IdentityDbContext<AppUser> {
        public MyContext(DbContextOptions<MyContext> options) : base(options) { }

        public MyContext() { }

        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<Animal> Animals { get; set; }
        public virtual DbSet<AppUser> AppUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            // Animal - Booking many-to-many relationship (join table needed)
            modelBuilder.Entity<BookingAnimal>()
                .HasKey(ba => new { ba.BookingId, ba.AnimalId });

            modelBuilder.Entity<BookingAnimal>()
                .HasOne(ba => ba.Booking)
                .WithMany(b => b.BookingAnimals)
                .HasForeignKey(ba => ba.BookingId);

            modelBuilder.Entity<BookingAnimal>()
                .HasOne(ba => ba.Animal)
                .WithMany(a => a.BookingAnimals)
                .HasForeignKey(ba => ba.AnimalId);

            // Booking - AppUser one-to-many relationship
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.AppUser)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.AppUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Animal entity configuration
            modelBuilder.Entity<Animal>(entity => {
                entity.HasKey(e => e.Id);
                entity.ToTable("Animal");
                entity.Property(e => e.Id)
                    .HasColumnName("animal_id").IsRequired();
                entity.Property(e => e.Name)
                    .HasColumnName("name");
                entity.Property(e => e.Type)
                    .HasColumnName("type");
                entity.Property(e => e.Price)
                    .HasColumnName("price")
                    .HasPrecision(16, 2);
                entity.Property(e => e.ImageUrl)
                    .HasColumnName("imageUrl");
            });

            // AppUser entity configuration
            modelBuilder.Entity<AppUser>(entity => {
                entity.HasMany(e => e.Bookings)
                    .WithOne(b => b.AppUser)
                    .HasForeignKey(e => e.AppUserId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.Address)
                    .HasColumnName("address");
            });

            // Booking entity configuration
            modelBuilder.Entity<Booking>(entity => {
                entity.HasKey(e => e.Id);
                entity.ToTable("Booking");
                entity.Property(e => e.Id)
                    .HasColumnName("booking_id").IsRequired();
                entity.Property(e => e.BookingDate)
                    .HasColumnName("booking_date");
                entity.Property(e => e.EventDate)
                    .HasColumnName("event_date");
                entity.Property(e => e.Discount)
                    .HasColumnName("discount")
                    .HasPrecision(16, 2);
                // No need to add foreign key to Animal directly here
            });

            // Adding default users and roles
            addUsersAndRoles(modelBuilder);
            addAnimals(modelBuilder);
        }

        private void addUsersAndRoles(ModelBuilder modelBuilder) {

            var boerderij = new AppUser {
                Id = "1",
                UserName = "jurian",
                NormalizedUserName = "JURIAN",
                Email = "employee@example.com",
                NormalizedEmail = "EMPLOYEE@EXAMPLE.COM",
                Address = "Laan van Meerdervoort 100 2517 AN Den Haag Nederland"
            };
            var klant = new AppUser {
                Id = "2",
                UserName = "ethan",
                NormalizedUserName = "ETHAN",
                Email = "employee@examples.com",
                NormalizedEmail = "EMPLOYEE@EXAMPLES.COM",
                Address = "Laan van Meerdervoort 5 2517 AN Den Haag Nederland"
            };


            var hasher = new PasswordHasher<AppUser>();
            boerderij.PasswordHash = hasher.HashPassword(boerderij, "a");
            klant.PasswordHash = hasher.HashPassword(klant, "a");

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "boerderij", NormalizedName = "BOERDERIJ" },
                new IdentityRole { Id = "2", Name = "klant", NormalizedName = "KLANT" }
            );

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { UserId = "1", RoleId = "1" },
                new IdentityUserRole<string> { UserId = "2", RoleId = "2" }
            );

            modelBuilder.Entity<AppUser>().HasData(boerderij, klant);
        }

        private void addAnimals(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Animal>().HasData(
                // Jungle Animals
                new Animal { Id = 1, Name = "Aap", Type = "Jungle", Price = 150.00m, ImageUrl = "#" },
                new Animal { Id = 2, Name = "Olifant", Type = "Jungle", Price = 500.00m, ImageUrl = "#" },
                new Animal { Id = 3, Name = "Zebra", Type = "Jungle", Price = 200.00m, ImageUrl = "#" },
                new Animal { Id = 4, Name = "Leeuw", Type = "Jungle", Price = 600.00m, ImageUrl = "#" },

                // Farm Animals
                new Animal { Id = 5, Name = "Hond", Type = "Boerderij", Price = 50.00m, ImageUrl = "#" },
                new Animal { Id = 6, Name = "Ezel", Type = "Boerderij", Price = 100.00m, ImageUrl = "#" },
                new Animal { Id = 7, Name = "Koe", Type = "Boerderij", Price = 250.00m, ImageUrl = "#" },
                new Animal { Id = 8, Name = "Eend", Type = "Boerderij", Price = 30.00m, ImageUrl = "#" },
                new Animal { Id = 9, Name = "Kuiken", Type = "Boerderij", Price = 10.00m, ImageUrl = "#" },

                // Snow Animals
                new Animal { Id = 10, Name = "Pinguïn", Type = "Sneeuw", Price = 100.00m, ImageUrl = "#" },
                new Animal { Id = 11, Name = "IJsbeer", Type = "Sneeuw", Price = 350.00m, ImageUrl = "#" },
                new Animal { Id = 12, Name = "Zeehond", Type = "Sneeuw", Price = 120.00m, ImageUrl = "#" },

                // Desert Animals
                new Animal { Id = 13, Name = "Kameel", Type = "Woestijn", Price = 400.00m, ImageUrl = "#" },
                new Animal { Id = 14, Name = "Slang", Type = "Woestijn", Price = 75.00m, ImageUrl = "#" },

                // VIP Animals
                new Animal { Id = 15, Name = "T-Rex", Type = "VIP", Price = 1000.00m, ImageUrl = "#" },
                new Animal { Id = 16, Name = "Unicorn", Type = "VIP", Price = 1500.00m, ImageUrl = "#" }
            );
        }
    }
}

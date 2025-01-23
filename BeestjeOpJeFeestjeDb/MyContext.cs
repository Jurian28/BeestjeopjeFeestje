using BeestjeOpJeFeestjeDb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BeestjeOpJeFeestje.Models {
    public class MyContext : IdentityDbContext<AppUser> {
        public MyContext(DbContextOptions<MyContext> options) : base(options) { }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Animal> Animals { get; set; }

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
                // No need to add foreign key to Animal directly here
            });

            // Adding default users and roles
            addUsersAndRoles(modelBuilder);
        }

        private void addUsersAndRoles(ModelBuilder modelBuilder) {

            var boerderij = new AppUser {
                Id = "1",
                UserName = "jurian",
                NormalizedUserName = "JURIAN",
                Email = "employee@example.com",
                NormalizedEmail = "EMPLOYEE@EXAMPLE.COM",
            };
            var klant = new AppUser {
                Id = "2",
                UserName = "ethan",
                NormalizedUserName = "ETHAN",
                Email = "employee@examples.com",
                NormalizedEmail = "EMPLOYEE@EXAMPLES.COM",
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
    }
}

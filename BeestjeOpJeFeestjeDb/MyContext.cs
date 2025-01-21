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

            // Booking-Animal many-to-many relationship
            modelBuilder.Entity<Booking>()
                .HasMany(b => b.Animals)
                .WithMany(a => a.Bookings)
                .UsingEntity<Dictionary<string, object>>(
                    "BookingAnimal",
                    j => j.HasOne<Animal>()
                          .WithMany()
                          .HasForeignKey("AnimalId")
                          .OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<Booking>()
                          .WithMany()
                          .HasForeignKey("BookingId")
                          .OnDelete(DeleteBehavior.Cascade)
                );

            // Booking-AppUser one-to-many relationship
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.AppUser)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.AppUserId)
                .OnDelete(DeleteBehavior.Restrict);

            addUsersAndRoles(modelBuilder);
        }

        private void addUsersAndRoles(ModelBuilder modelBuilder) {

            var boerderij = new AppUser {
                Id = "1",
                UserName = "Koray Yilmaz",
                NormalizedUserName = "KORAY YILMAZ",
                Email = "employee@example.com",
                NormalizedEmail = "EMPLOYEE@EXAMPLE.COM",
            };
            var klant = new AppUser {
                Id = "2",
                UserName = "Koray Yilmazs",
                NormalizedUserName = "KORAY YILMAZS",
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

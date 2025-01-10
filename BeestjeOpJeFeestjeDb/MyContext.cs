using BeestjeOpJeFeestjeDb.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BeestjeOpJeFeestje.Models {
    public class MyContext : IdentityDbContext<AppUser> {
        public MyContext(DbContextOptions<MyContext> options) : base(options) { }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Animal> Animals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Booking>()
                .HasMany(b => b.Animals)
                .WithMany(a => a.Bookings)
                .UsingEntity<Dictionary<string, object>>(
                    "BookingAnimal", 
                    j => j.HasOne<Animal>().WithMany().HasForeignKey("AnimalId"),
                    j => j.HasOne<Booking>().WithMany().HasForeignKey("BookingId")
                );
        }
    }
}

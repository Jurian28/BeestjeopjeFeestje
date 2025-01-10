using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BeestjeOpJeFeestje.Models {
    public class MyContext : IdentityDbContext<AppUser> {
        public MyContext() : base() { }

        public MyContext(DbContextOptions<MyContext> options) : base(options) {
        }

        public DbSet<Geheim> Geheimen { get; set; }
    }
}

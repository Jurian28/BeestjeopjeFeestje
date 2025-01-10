using BeestjeOpJeFeestjeDb.Models;
using Microsoft.AspNetCore.Identity;

namespace BeestjeOpJeFeestje.Models {
    public class AppUser : IdentityUser {
        public ICollection<Booking> Bookings { get; set; }
    }
}

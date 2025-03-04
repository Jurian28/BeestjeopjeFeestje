using Microsoft.AspNetCore.Identity;

namespace BeestjeOpJeFeestjeDb.Models {
    public class AppUser : IdentityUser {
        //public string Id {  get; set; }
        public string? Card { get; set; } // Geen Zilver Goud Platina
        public ICollection<Booking> Bookings { get; set; }
        public string Address { get; set; }
    }
}

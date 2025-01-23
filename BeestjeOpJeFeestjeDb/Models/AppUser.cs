using BeestjeOpJeFeestjeDb.Models;
using Microsoft.AspNetCore.Identity;

namespace BeestjeOpJeFeestje.Models {
    public class AppUser : IdentityUser {
        public string Id {  get; set; }
        public string? Card {  get; set; }
        public ICollection<Booking> Bookings { get; set; }
    }
}

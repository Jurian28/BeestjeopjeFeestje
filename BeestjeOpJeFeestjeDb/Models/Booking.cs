
using BeestjeOpJeFeestje.Models;

namespace BeestjeOpJeFeestjeDb.Models {
    public class Booking {
        public int Id { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime EventDate { get; set; }

        // Foreign key to AppUser
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        // Navigation property for the many-to-many relationship with Animal
        public ICollection<BookingAnimal> BookingAnimals { get; set; }
    }
}

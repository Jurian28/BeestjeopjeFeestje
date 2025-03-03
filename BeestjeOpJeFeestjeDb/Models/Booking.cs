
using BeestjeOpJeFeestjeDb.Models;

namespace BeestjeOpJeFeestjeDb.Models {
    public class Booking {
        public int Id { get; set; }
        public DateOnly BookingDate { get; set; }
        public DateOnly EventDate { get; set; }
        public decimal Discount { get; set; }

        // Foreign key to AppUser
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        // Navigation property for the many-to-many relationship with Animal
        public ICollection<BookingAnimal> BookingAnimals { get; set; }

        public decimal GetFullPrice() {
            decimal fullPrice = 0;
            foreach (var bookingAnimal in BookingAnimals) {
                fullPrice += bookingAnimal.Animal.Price;
            }
            return fullPrice;
        }
    }
}


using BeestjeOpJeFeestje.Models;

namespace BeestjeOpJeFeestjeDb.Models {
    public class Booking {
        public string Id { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime EventDate { get; set; }
        //public string AppUserId { get; set; }


        public AppUser AppUser { get; set; }

        public ICollection<Animal> Animals { get; set; }
    }
}

namespace BeestjeOpJeFeestjeDb.Models {
    public class BookingAnimal {
        public int BookingId { get; set; }
        public Booking Booking { get; set; }

        public int AnimalId { get; set; }
        public Animal Animal { get; set; }
    }
}

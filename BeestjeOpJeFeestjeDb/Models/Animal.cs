namespace BeestjeOpJeFeestjeDb.Models {
    public class Animal {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; } // e.g., Dog, Cat, etc.

        public ICollection<Booking> Bookings { get; set; }
    }
}

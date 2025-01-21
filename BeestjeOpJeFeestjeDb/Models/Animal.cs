namespace BeestjeOpJeFeestjeDb.Models {
    public class Animal {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; } 
        public Decimal Price {  get; set; }
        public string ImageUrl { get; set; }

        public ICollection<Booking> Bookings { get; set; }
    }
}

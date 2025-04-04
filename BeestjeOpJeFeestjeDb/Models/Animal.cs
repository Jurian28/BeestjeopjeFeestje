using Microsoft.EntityFrameworkCore.Storage;

namespace BeestjeOpJeFeestjeDb.Models {
    public class Animal {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; } = "";
        public decimal Price { get; set; } = 0;
        public string ImageUrl { get; set; } = "";

        public ICollection<BookingAnimal> BookingAnimals { get; set; } = [];
    }
}

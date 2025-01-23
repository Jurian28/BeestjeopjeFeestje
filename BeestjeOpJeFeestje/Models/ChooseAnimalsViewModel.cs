using BeestjeOpJeFeestjeDb.Models;

namespace BeestjeOpJeFeestje.Models {
    public class ChooseAnimalsViewModel {
        public DateOnly Date { get; set; }
        public List<Animal> AvailableAnimals { get; set; }
        public List<int> SelectedAnimals { get; set; }
    }
}

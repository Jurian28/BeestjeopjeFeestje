
using BeestjeOpJeFeestjeDb.Models;

namespace BeestjeOpJeFeestjeBusinessLayer {
    public interface IBookingService {
        DateOnly? GetDate();
        void SetDate(DateOnly date);

        List<Animal> GetAvailableAnimals();

        void AddOrRemoveAnimalFromBooking(int animalId);
        List<int> GetSelectedAnimalIds();
    }
}

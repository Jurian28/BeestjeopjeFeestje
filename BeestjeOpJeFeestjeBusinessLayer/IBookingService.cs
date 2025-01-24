
using BeestjeOpJeFeestjeDb.Models;

namespace BeestjeOpJeFeestjeBusinessLayer {
    public interface IBookingService {
        DateOnly? GetDate();
        void SetDate(DateOnly date);

        List<Animal> GetAvailableAnimals();

        void AddOrRemoveAnimalFromBooking(int animalId);
        List<int> GetSelectedAnimalIds();
        
        bool ValidateAnimals();
        decimal CalculateDiscount();

        Task<Booking> GetBooking(bool unloaded = false);
        void SetAppUserId(string userId);
        Task ConfirmBooking();
        bool ValidateBookingStep(int bookingStep);
        void SetBookingStep(int bookingStep);
    }
}

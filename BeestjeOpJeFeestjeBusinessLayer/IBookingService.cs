
using BeestjeOpJeFeestjeDb.Models;

namespace BeestjeOpJeFeestjeBusinessLayer {
    public interface IBookingService {
        DateOnly? GetDate();
        void SetDate(DateOnly date);

        List<Animal> GetAvailableAnimals();

        void AddOrRemoveAnimalFromBooking(int animalId);
        List<int> GetSelectedAnimalIds();
        public List<Animal> GetSelectedAnimals();

        bool ValidateAnimals(out List<string> modelErrors);
        public bool ValidateUserCard(out List<string> modelErrors);
        decimal CalculateDiscount();

        Task<Booking> GetBooking(bool unloaded = false);
        void SetAppUserId(string userId);
        Task ConfirmBooking();
        bool ValidateBookingStep(int bookingStep); 
        void SetBookingStep(int bookingStep);

        void ResetBooking();
    }
}

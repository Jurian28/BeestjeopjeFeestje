using BeestjeOpJeFeestje.Models;
using BeestjeOpJeFeestjeDb.Models;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Text.Json;

namespace BeestjeOpJeFeestjeBusinessLayer {
    public class BookingService : IBookingService {
        private readonly MyContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private HttpContext HttpContext => _httpContextAccessor.HttpContext;

        public BookingService(MyContext context, IHttpContextAccessor httpContextAccessor) {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private string? getHttpContextString(string key) {
            if (!HttpContext.Session.TryGetValue(key, out var output)) {
                return null;
            }
            return Encoding.UTF8.GetString(output);
        }
        private void setHttpContextString(string key, string value) {
            HttpContext.Session.Set(key, Encoding.UTF8.GetBytes(value));
        }


        public DateOnly? GetDate() {
            string dateString = getHttpContextString("Date");
            return DateOnly.Parse(dateString);
        }

        public void SetDate(DateOnly date) {
            HttpContext.Session.Set("Date", System.Text.Encoding.UTF8.GetBytes(date.ToString("yyyy-MM-dd")));
        }

        public void AddOrRemoveAnimalFromBooking(int animalId) {
            string? animalListJson = getHttpContextString("AnimalList");
            List<int> animalIds = string.IsNullOrEmpty(animalListJson)
                    ? new List<int>()
                    : JsonSerializer.Deserialize<List<int>>(animalListJson);

            if (animalIds.Contains(animalId))
                animalIds.Remove(animalId);
            else
                animalIds.Add(animalId);

            setHttpContextString("AnimalList", JsonSerializer.Serialize(animalIds));
        }


        public List<Animal> GetAvailableAnimals() {
            List<Animal> animals = _context.Animals.ToList();

            return animals;
        }

        public List<int> GetSelectedAnimalIds() {
            string? animalListJson = getHttpContextString("AnimalList");

            if (!string.IsNullOrEmpty(animalListJson)) {
                List<int> animalIds = JsonSerializer.Deserialize<List<int>>(animalListJson);
                return animalIds;
            }

            return new List<int>();
        }


        public bool ValidateAnimals() {
            throw new NotImplementedException();
        }

        public void CalculateDiscount() { 
            throw new NotImplementedException();
            GetSelectedAnimalIds(); // dit zou je kunnen gebruiken
        }
    }
}

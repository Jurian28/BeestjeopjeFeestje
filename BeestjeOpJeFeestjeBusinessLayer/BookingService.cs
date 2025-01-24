using BeestjeOpJeFeestje.Models;
using BeestjeOpJeFeestjeDb.Models;
using Microsoft.AspNetCore.Http;
using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Identity;
using System.Text;
using System.Text.Json;

namespace BeestjeOpJeFeestjeBusinessLayer {
    public class BookingService : IBookingService {
        private readonly MyContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;
        private HttpContext HttpContext => _httpContextAccessor.HttpContext;

        public BookingService(MyContext context, UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor) {
            _context = context;
            _userManager = userManager;
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
            List<int> animalIds = GetSelectedAnimalIds();
            List<string> animalTypes = GetAnimalTypes(animalIds);
            List<string> animalNames = GetAnimalNames(animalIds);
            DateOnly selectedDate = (DateOnly)GetDate();

            // Regel 1: Leeuw/IJsbeer en Boerderijdier kunnen niet samen geboekt worden
            if (animalNames.Contains("Leeuw") || animalNames.Contains("IJsbeer")) {
                if (animalTypes.Contains("Boerderij")) {
                    return false;
                }
            }

            // Regel 2: Pinguïn mag niet in het weekend
            if (animalNames.Contains("Pinguïn") && (selectedDate.DayOfWeek == DayOfWeek.Saturday || selectedDate.DayOfWeek == DayOfWeek.Sunday)) {
                return false;
            }

            // Regel 3: Woestijndieren kunnen niet geboekt worden van oktober-februari
            if (animalTypes.Contains("Woestijn") && (selectedDate.Month >= 10 && selectedDate.Month <= 2)) {
                return false;
            }

            // Regel 4: Sneeuwdieren kunnen niet geboekt worden van juni-augustus
            if (animalTypes.Contains("Sneeuw") && (selectedDate.Month >= 6 && selectedDate.Month <= 8)) {
                return false;
            }

            // Regel 5: Klanten zonder klantenkaart mogen maximaal 3 dieren boeken
            //int maxAnimals = GetMaxAnimalsBasedOnCustomerCard();
            //if (animalIds.Count > maxAnimals) {
            //    return false;
            //}

            //// Regel 6: Klanten met platina klantenkaart kunnen VIP-dieren boeken
            //if (HasPlatinumCard() && animalIds.Any(id => IsVIPAnimal(id))) {
            //    return true; // Valid, VIP dieren mogen worden geboekt
            //}

            return true;
        }

        private List<string> GetAnimalNames(List<int> animalIds) {
            List<string> animalTypes = new List<string>();

            foreach (int id in animalIds) {
                var animal = _context.Animals.FirstOrDefault(a => a.Id == id);
                if (animal != null) {
                    animalTypes.Add(animal.Name);
                }
            }
            return animalTypes;
        }
        private List<string> GetAnimalTypes(List<int> animalIds) {
            List<string> animalTypes = new List<string>();

            foreach (int id in animalIds) {
                var animal = _context.Animals.FirstOrDefault(a => a.Id == id);
                if (animal != null) {
                    animalTypes.Add(animal.Type);
                }
            }

            return animalTypes;
        }

        public decimal CalculateDiscount() {
            decimal discount = 0;
            List<int> animalIds = GetSelectedAnimalIds();
            if(animalIds.Count >= 3) {
                discount = discount + 10;
            }
            if (DateTime.Now.DayOfWeek == DayOfWeek.Monday || DateTime.Now.DayOfWeek == DayOfWeek.Tuesday) {
                discount = discount + 15;
            }
            //mist nog de kaart discount, ik voeg deze toe als de main hierin gemerged is

            foreach (int animalId in animalIds) {
                Animal? animal = _context.Animals.FirstOrDefault(a => a.Id == animalId);
                if (animal != null) {
                    if (animal.Name == "Eend") {
                        Random random = new Random();
                        if(random.Next(6) == 0) {
                            discount = discount + 50;
                        }
                    }
                    char alphabet = 'a';
                    while (animal.Name.Contains(alphabet) || alphabet == 'z') {
                        alphabet = (char)(((int)alphabet) + 1);
                        discount = discount + 2;
                    }
                }
            }

            if(discount > 60) {
                discount = 60;
            }

            return discount;
        }

        private async Task<List<BookingAnimal>> GetBookingAnimals(bool unLoaded = false) {
            List<int> animalIds = GetSelectedAnimalIds();

            List<Animal> animals = _context.Animals.Where(a => animalIds.Contains(a.Id)).ToList();

            List<BookingAnimal> bookingAnimals = animals.Select(animal => new BookingAnimal {
                AnimalId = animal.Id,
            }).ToList();
            if (!unLoaded) {
                foreach (var bookingAnimal in bookingAnimals) { // lazy load
                    await _context.Entry(bookingAnimal).Reference(ba => ba.Animal).LoadAsync();
                }
            }

            return bookingAnimals;
        }

        public async Task<Booking> GetBooking(bool unloaded = false) {
            AppUser appUser = _context.AppUsers.FirstOrDefault(a => a.Id == getHttpContextString("AppUserId"));
            DateOnly? eventDate = GetDate();
            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Today);
            List<BookingAnimal> bookingAnimals = await GetBookingAnimals(unloaded);

            Booking booking = new Booking() {
                BookingAnimals = bookingAnimals,
                AppUser = appUser,
                AppUserId = appUser?.Id,
                EventDate = (DateOnly)eventDate,
                BookingDate = currentDate
            };
            return booking;
        }

        public void SetAppUserId(string userId) {
            setHttpContextString("AppUserId", userId);
        }

        public async Task ConfirmBooking() {
            Booking booking = await GetBooking(true);
            _context.Bookings.Add(booking);
            _context.SaveChanges();
        }

        public bool ValidateBookingStep(int bookingStep) {
            int currentBookingStep = int.Parse(getHttpContextString("BookingStep"));
            return bookingStep == currentBookingStep;
        }

        public void SetBookingStep(int bookingStep) {
            setHttpContextString("BookingStep", bookingStep.ToString());
        }
    }
}

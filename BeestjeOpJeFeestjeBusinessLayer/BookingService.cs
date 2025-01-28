using BeestjeOpJeFeestje.Models;
using BeestjeOpJeFeestjeDb.Models;
using Microsoft.AspNetCore.Http;
using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Identity;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using BeestjeOpJeFeestjeDb;

namespace BeestjeOpJeFeestjeBusinessLayer {
    public class BookingService : IBookingService {
        private readonly MyContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser>? _userManager;
        private HttpContext HttpContext => _httpContextAccessor.HttpContext;

        private decimal _discount;
        private string _user;
        private List<int> _animalIds;
        private string _dateString;

        public BookingService(MyContext context, UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor) {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _animalIds = new List<int>();
        }

        //voor unit test
        public BookingService(MyContext context, IHttpContextAccessor httpContextAccessor) { 
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _animalIds = new List<int>();
        }

        private string? getHttpContextString(string key) {
            if(_userManager == null) {
                return null;
            }
            if (!HttpContext.Session.TryGetValue(key, out var output)) {
                return null;
            }
            return Encoding.UTF8.GetString(output);
        }
        private void setHttpContextString(string key, string value) {
            if (_userManager == null) {
                return;
            }
            HttpContext.Session.Set(key, Encoding.UTF8.GetBytes(value));
        }


        public DateOnly? GetDate() {
            string dateString;
            if (_userManager == null) {
                dateString = _dateString;
                return DateOnly.Parse(dateString);
            }
            dateString = getHttpContextString("Date");
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
            DateOnly? date = GetDate();

            List<Animal> animals = _context.Animals
                    .Include(a => a.BookingAnimals)
                        .ThenInclude(ba => ba.Booking) 
                    .Where(a => a.BookingAnimals.All(ba => ba.Booking.EventDate != date.Value))
                    .ToList();

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


        public bool ValidateAnimals(out List<string> modelErrors) {
            modelErrors = new List<string>();
            List<int> animalIds;

            if (_userManager != null) {
                animalIds = GetSelectedAnimalIds();
            } else {
                animalIds = _animalIds;
            }
            List<string> animalTypes = GetAnimalTypes(animalIds);
            List<string> animalNames = GetAnimalNames(animalIds);
            DateOnly selectedDate = (DateOnly)GetDate();

            if (animalIds.Count == 0) {
                modelErrors.Add("Selecteer een of meerdere dieren.");
                return false;
            }

            // Regel 1: Leeuw/IJsbeer en Boerderijdier kunnen niet samen geboekt worden
            if (animalNames.Contains("Leeuw") || animalNames.Contains("IJsbeer")) {
                if (animalTypes.Contains("Boerderij")) {
                    modelErrors.Add("Een leeuw of ijsbeer kan niet samen met een boerderijdier geboekt worden.");
                }
            }

            // Regel 2: Pinguïn mag niet in het weekend
            if (animalNames.Contains("Pinguïn") && (selectedDate.DayOfWeek == DayOfWeek.Saturday || selectedDate.DayOfWeek == DayOfWeek.Sunday)) {
                modelErrors.Add("Een pinguïn mag niet in het weekend geboekt worden.");
            }

            // Regel 3: Woestijndieren kunnen niet geboekt worden van oktober-februari
            if (animalTypes.Contains("Woestijn") && (selectedDate.Month >= 10 || selectedDate.Month <= 2)) { // TODO bij de maanden doet die het nog niet 
                modelErrors.Add("Woestijndieren kunnen niet geboekt worden van oktober tot februari.");
            }

            // Regel 4: Sneeuwdieren kunnen niet geboekt worden van juni-augustus
            if (animalTypes.Contains("Sneeuw") && (selectedDate.Month >= 6 && selectedDate.Month <= 8)) {
                modelErrors.Add("Sneeuwdieren kunnen niet geboekt worden van juni tot augustus.");
            }


            return modelErrors.Count == 0;
        }


        public bool ValidateUserCard(out List<string> modelErrors) {
            modelErrors = new List<string>();
            List<int> animalIds;
            List<string> animalTypes;
            AppUser appUser;

            if (_userManager != null) {
                animalIds = GetSelectedAnimalIds();
                appUser = _context.AppUsers.FirstOrDefault(a => a.Id == getHttpContextString("AppUserId"));
            }
            else {
                animalIds = _animalIds;
                appUser = _context.AppUsers.FirstOrDefault(a => a.Id == _user);
            }
            animalTypes = GetAnimalTypes(animalIds);

            int maxAnimals = appUser.Card switch {
                "Geen" => 3, 
                "Zilver" => 4,
                "Goud" => int.MaxValue, 
                "Platina" => int.MaxValue, 
                _ => 3 
            };

            // Regel 5: Controleer of het aantal geselecteerde dieren binnen de limiet valt
            if (animalIds.Count > maxAnimals) {
                modelErrors.Add($"Met uw kaart mag u maximaal {maxAnimals} dieren boeken.");
            }

            // Regel 6: Controleer of VIP-dieren alleen geboekt worden door klanten met een platina kaart
            if (animalTypes.Contains("VIP") && appUser.Card != "Platina") {
                modelErrors.Add("VIP-dieren kunnen alleen geboekt worden door klanten met een platina klantenkaart.");
            }

            return modelErrors.Count == 0;
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

        public void CalculateDiscount() {
            decimal discount = 0;
            List<int> animalIds;
            if (_userManager != null) {
                animalIds = GetSelectedAnimalIds();
            }
            else {
                animalIds = _animalIds;
            }
            // discount 1: Meer dan drie animals geboekt
            if (animalIds.Count >= 3) {
                discount = discount + 10;
            }
            // discount 2: De dag is maandag of dinsdag
            if (DateTime.Now.DayOfWeek == DayOfWeek.Monday || DateTime.Now.DayOfWeek == DayOfWeek.Tuesday) {
                discount = discount + 15;
            }
            // discount 3: De klant heeft een klanten kaart
            AppUser? appUser = _context.AppUsers.FirstOrDefault(a => a.Id == _user);
            if (appUser != null) {
                if(appUser.Card != null) {
                    if(appUser.Card != "Geen") {
                        discount = discount + 10;
                    }
                }
            }

            foreach (int animalId in animalIds) {
                Animal? animal = _context.Animals.FirstOrDefault(a => a.Id == animalId);
                if (animal != null && animal.Name != null) {
                    //discount 4: De eend heeft een 1 op 6 kans voor een discount
                    if (animal.Name == "Eend") {
                        Random random = new Random();
                        if(random.Next(6) == 0) {
                            discount = discount + 50;
                        }
                    }
                    //discount 5: Een dier bevat letters van het alphabet vanaf a
                    char alphabetSmall = 'a';
                    char alphabetBig = 'A';
                    while (animal.Name.Contains(alphabetSmall) || alphabetSmall == 'z' || animal.Name.Contains(alphabetBig) || alphabetSmall == 'Z') {
                        alphabetSmall = (char)(((int)alphabetSmall) + 1);
                        alphabetBig = (char)(((int)alphabetBig) + 1);
                        discount = discount + 2;
                    }
                }
            }

            //discount 6: Discount mag niet hoger zijn dan 60
            if(discount > 60) {
                discount = 60;
            }

            _discount = discount;
            setHttpContextString("Discount", discount.ToString());
        }

        public List<Animal> GetSelectedAnimals() {
            List<int> animalIds = GetSelectedAnimalIds();

            List<Animal> animals = _context.Animals.Where(a => animalIds.Contains(a.Id)).ToList();
            return animals;
        }

        private async Task<List<BookingAnimal>> GetBookingAnimals(bool unLoaded = false) {
            List<Animal> animals = GetSelectedAnimals();

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
            decimal discount = GetDiscount();

            Booking booking = new Booking() {
                BookingAnimals = bookingAnimals,
                AppUser = appUser,
                AppUserId = appUser?.Id,
                EventDate = (DateOnly)eventDate,
                BookingDate = currentDate,
                Discount = discount
            };
            return booking;
        }

        public decimal GetDiscount() {
            if (decimal.TryParse(getHttpContextString("Discount"), out decimal result)) {
                return result;
            }
            return 0;
        }

        public decimal GetDiscountForTest() {
            return _discount;
        }

        public void SetAppUserId(string userId) {
            setHttpContextString("AppUserId", userId);
        }

        public void SetDateString(string date) {
            _dateString = date;
        }

        public async Task ConfirmBooking() {
            Booking booking = await GetBooking(true);
            _context.Bookings.Add(booking);
            _context.SaveChanges();
        }

        public bool ValidateBookingStep(int bookingStep) {
            int currentBookingStep = int.Parse(getHttpContextString("BookingStep"));
            return bookingStep <= currentBookingStep;
        }

        public void SetBookingStep(int bookingStep) {
            setHttpContextString("BookingStep", bookingStep.ToString());
        }

        public void SetUser(string user) {
            _user = user;
        }

        public void AddToAnimalList(int id) {
            _animalIds.Add(id);
        }

        public void ResetBooking() {
            HttpContext.Session.Clear();
        }
    }
}

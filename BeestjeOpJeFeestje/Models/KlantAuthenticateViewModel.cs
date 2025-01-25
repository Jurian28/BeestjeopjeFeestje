using BeestjeOpJeFeestjeDb.Models;

namespace BeestjeOpJeFeestje.Models {
    public class KlantAuthenticateViewModel {
        public RegisterForm RegisterForm = new RegisterForm();
        public DateOnly Date {  get; set; }
        public List<Animal> Animals { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace BeestjeOpJeFeestje.Models {
    public class RegisterForm {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [StringLength(100)]
        public string Email { get; set; }
        [StringLength(100)]
        public string PhoneNumber { get; set; }
        [Required]
        [StringLength(100)]
        public string Address { get; set; }
        [StringLength(100)]
        public string Password { get; set; }
        [StringLength(100)]
        public string PasswordRepeat { get; set; }
        public string Card { get; set; }
    }
}

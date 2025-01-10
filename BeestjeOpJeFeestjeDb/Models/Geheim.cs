using System.ComponentModel.DataAnnotations;

namespace BeestjeOpJeFeestje.Models {
    public class Geheim {
        [Key]
        public int Id { get; set; }

        public string Inhoud { get; set; }

        public int SecurityLevel { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace DepozytOpon.Models
{
    public class Opona
    {
        public int Id { get; set; }

        public string? Typ { get; set; }
        public string? Producent { get; set; }
        public string? Rozmiar { get; set; }
        public string? Bieznik { get; set; }
        public string? Sezon { get; set; }
        public string? RokProdukcji { get; set; }
        [Required(ErrorMessage = "Kod Opony jest wymagany")]
        public string KodTowaru { get; set; } = null!;
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DepozytOpon.Models
{
    public class Depozyt
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Numer BOX jest wymagany")]
        public string NumerBOX { get; set; }
        public string? ImieNazwisko { get; set; }
        public string? NumerTelefonu { get; set; }
        public string? MarkaPojazdu { get; set; }
        public string? RejestracjaPojazdu { get; set; }
        public string? OponaId { get; set; }
        public int? Ilosc { get; set; }
        public DateTime DataPrzyjecia { get; set; }
        public string? Notatka { get; set; }

        public string? UtworzonoPrzez { get; set; }
        public DateTime? DataEdycji { get; set; }
        public string? EdytowanoPrzez { get; set; }
    }
}

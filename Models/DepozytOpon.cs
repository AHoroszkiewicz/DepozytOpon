using System;
using System.Collections.Generic;

namespace DepozytOpon.Models
{
    public class Depozyt
    {
        public int Id { get; set; }

        public string NumerBOX { get; set; }
        public string ImieNazwisko { get; set; }
        public string NumerTelefonu { get; set; }
        public string MarkaPojazdu { get; set; }
        public string RejestracjaPojazdu { get; set; }

        // Lista opon powiązana z depozytem (nawet jeśli zwykle jest 1 typ opony,
        // to liczbę sztuk trzymamy w polu Ilość)
        public string OponaId { get; set; }

        public int Ilosc { get; set; }
        public DateTime DataPrzyjecia { get; set; }
        public string Notatka { get; set; }
    }
}

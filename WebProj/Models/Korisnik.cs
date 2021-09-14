using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProj.Models
{
    [Serializable()]
    public class Korisnik
    {
        public string korisnickoIme { get; set; }
        public string lozinka { get; set; }
        public string ime { get; set; }
        public string prezime { get; set; }
        public Enumeracije.Pol pol { get; set; }
        public string email { get; set; }
        public DateTime datumRodjenja { get; set; }
        public Enumeracije.Uloga uloga { get; set; }
        public List<Rezervacija> rezervacije { get; set; }
        public List<Aranzman> aranzmani { get; set; }
    }
}
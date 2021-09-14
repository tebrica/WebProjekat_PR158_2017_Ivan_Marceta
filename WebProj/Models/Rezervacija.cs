using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProj.Models
{
    [Serializable()]
    public class Rezervacija
    {
        public char[] id = new char[15];
        public Korisnik turista { get; set; }
        public Enumeracije.Status status { get; set; }
        public Aranzman aranzman { get; set; }
        public SmestajnaJedinica smestajnaJedinica { get; set; }
    }
}
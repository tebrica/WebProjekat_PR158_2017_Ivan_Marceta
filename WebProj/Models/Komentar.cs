using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProj.Models
{
    [Serializable()]
    public class Komentar
    {
        public Korisnik turista { get; set; }
        public Aranzman aranzman { get; set; }
        public string tekst { get; set; }
        public int ocena { get; set; }
    }
}
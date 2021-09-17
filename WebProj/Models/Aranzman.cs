using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProj.Models
{
    [Serializable()]
    public class Aranzman
    {
        public string naziv { get; set; }
        public Enumeracije.TipAranzmana tipAranzmana { get; set; }
        public Enumeracije.TipPrevoza tipPrevoza { get; set; }
        public string lokacija { get; set; }
        public DateTime datPocetka { get; set; }
        public DateTime datZavrsetka { get; set; }
        public MestoNalazenja mestoNalazenja { get; set; }
        public DateTime vremeNalazenja { get; set; }
        public int maksimalanBrPutnika { get; set; }
        public Smestaj smestaj { get; set; }
        public string opis { get; set; }
        public string program { get; set; }
        public string poster { get; set; }
        public bool obrisan { get; set; }
    }
}
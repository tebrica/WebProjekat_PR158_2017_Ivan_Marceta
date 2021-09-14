using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProj.Models
{
    [Serializable()]
    public class Smestaj
    {
        public Enumeracije.TipSmestaja tipSmestaja { get; set; }
        public string naziv { get; set; }
        public int? brojZvezdica { get; set; }
        public bool bazen { get; set; }
        public bool spaCentar { get; set; }
        public bool invaliditet { get; set; }
        public bool wifi { get; set; }
        public List<SmestajnaJedinica> smestajneJedinice { get; set; }

    }
}
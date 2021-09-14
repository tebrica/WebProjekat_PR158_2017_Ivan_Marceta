using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProj.Models
{
    [Serializable()]
    public class SmestajnaJedinica
    {
        public int dozvoljenBrojGostiju { get; set; }
        public bool kucniLjubimci { get; set; }
        public double cena { get; set; }
    }
}
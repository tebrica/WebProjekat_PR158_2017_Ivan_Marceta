using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProj.Models
{
    [Serializable()]
    public class MestoNalazenja
    {
        public string adresa { get; set; }
        public double geografskaDuzina { get; set; }
        public double geografskaSirina { get; set; }
    }
}
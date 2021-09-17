using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProj.Models
{
    public static class Enumeracije
    {
        public enum TipSmestaja { hotel, motel, vila}
        public enum Status { aktivna, otkazana}
        public enum TipAranzmana { nocSaDoruckom, polupansion, punPansion, olInkluziv, iznajmljenApartman}
        public enum TipPrevoza { bus, avion, busAvion, individualan, ostalo}
        public enum Pol { muski, zenski}
        public enum Uloga { administrator, menadzer, turista}
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProj.Models
{
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public static class Baza
    {
        [System.Xml.Serialization.XmlElementAttribute("Aranzman")]
        public static List<Aranzman> aranzmani = new List<Aranzman>();
        [System.Xml.Serialization.XmlElementAttribute("Korisnik")]
        public static List<Korisnik> korisnici = new List<Korisnik>();
        [System.Xml.Serialization.XmlElementAttribute("Smestaj")]
        public static List<Smestaj> smestaji = new List<Smestaj>();
        [System.Xml.Serialization.XmlElementAttribute("Komentar")]
        public static List<Komentar> komentari = new List<Komentar>();
    }
}
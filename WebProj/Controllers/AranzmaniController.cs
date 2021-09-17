using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebProj.Models;
namespace WebProj.Controllers
{
    public class AranzmaniController : Controller
    {
        // GET: Aranzmani
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Aranzmani()
        {
            List<Aranzman> ar = Baza.aranzmani.OrderBy(x => x.datPocetka).ToList().FindAll(x => x.datPocetka > DateTime.Now).ToList();
            return View("index", ar);

        }
        public ActionResult SviAranzmani()
        {
            List<Aranzman> ar = Baza.aranzmani.OrderBy(x => x.datZavrsetka).ToList();
            return View("SviAranzmani",ar);
        }
        public ActionResult SearchAranzmani(string search)
        {
            return View("Index", Baza.aranzmani.FindAll(x=>x.naziv.Contains(search)).FindAll(x => x.datPocetka > DateTime.Now).ToList());
        }
        public ActionResult SearchAranzmaniSvi(string search)
        {
            return View("SviAranzmani",Baza.aranzmani.FindAll(x => x.naziv.Contains(search)).ToList());
        }
        public ActionResult AranzmanIzIndex(int aranzmanRb)
        {
            aranzmanRb -= 1;
            List<Aranzman> ar = Baza.aranzmani.OrderBy(x => x.datPocetka).ToList().FindAll(x => x.datPocetka > DateTime.Now).ToList();
            return View("Aranzman", ar[aranzmanRb]);
        }
        public ActionResult AranzmanIzSvi(int aranzmanRb)
        {
            aranzmanRb -= 1;
            List<Aranzman> ar = Baza.aranzmani.OrderBy(x => x.datZavrsetka).ToList();
            return View("Aranzman", ar[aranzmanRb]);
        }
        public ActionResult Rezervisi(int smesJedinicaRb, string nazAran)
        {
            SmestajnaJedinica sj = Baza.aranzmani.Find(x => x.naziv == nazAran).smestaj.smestajneJedinice[smesJedinicaRb];
            if(sj.dozvoljenBrojGostiju < 0 && Baza.korisnici.Find(x=>x.korisnickoIme == Request.Cookies["LoggedIn"].Value.Split('_')[0]).rezervacije.Find(x=>x.smestajnaJedinica.id.Equals(sj.id)) != null)
            {
                Baza.korisnici.Find(x => x.korisnickoIme == Request.Cookies["LoggedIn"].Value.Split('_')[0]).rezervacije.Find(x => x.smestajnaJedinica.id.Equals(sj.id)).status = Enumeracije.Status.otkazana;
                
                Baza.smestaji.Find(x => x.smestajneJedinice.Find(y => y.id.Equals(sj.id)) != null).smestajneJedinice.Find(x => x.id == sj.id).dozvoljenBrojGostiju *= -1;
                Baza.aranzmani.Find(x => x.naziv == nazAran).smestaj.smestajneJedinice.Find(x => x.id.Equals(sj.id)).dozvoljenBrojGostiju *= -1;

                XmlHandler.XmlHandler.UpdateFile(Baza.aranzmani);
                XmlHandler.XmlHandler.UpdateFile(Baza.korisnici);
                XmlHandler.XmlHandler.UpdateFile(Baza.smestaji);
                return View("Aranzman", Baza.aranzmani.Find(x => x.naziv.Equals(nazAran)));
            }
            if(sj.dozvoljenBrojGostiju < 0 && Baza.korisnici.Find(x => x.korisnickoIme == Request.Cookies["LoggedIn"].Value.Split('_')[0]).rezervacije.Find(x => x.smestajnaJedinica.id.Equals(sj.id)) == null)
            {
                ViewBag.rezervacija = "smestajna jedinica vec rezervisana";
                return View("Aranzman",Baza.aranzmani.Find(x=>x.naziv.Equals(nazAran)));
            }
            Baza.smestaji.Find(x => x.smestajneJedinice.Find(y => y.id.Equals(sj.id)) != null).smestajneJedinice.Find(x => x.id == sj.id).dozvoljenBrojGostiju *= -1;
            Baza.aranzmani.Find(x => x.naziv == nazAran).smestaj.smestajneJedinice.Find(x => x.id.Equals(sj.id)).dozvoljenBrojGostiju *= -1;
            char[] id = GenerateRandomAlphanumericString();

            while (Baza.korisnici.Find(x => x.rezervacije.Find(y=>y.id.Equals(id)) != null) != null)
            {
                id = GenerateRandomAlphanumericString();
            }
            Rezervacija r = new Rezervacija()
            {
                aranzman = Baza.aranzmani.Find(x => x.naziv.Equals(nazAran)),
                smestajnaJedinica = sj,
                status = Enumeracije.Status.aktivna,
                id = id
            };
            if(Baza.korisnici.Find(x=>x.korisnickoIme == Request.Cookies["LoggedIn"].Value.Split('_')[0]).rezervacije.Find(x => x.smestajnaJedinica.id.Equals(sj.id)) != null)
            {
                Baza.korisnici.Find(x => x.korisnickoIme == Request.Cookies["LoggedIn"].Value.Split('_')[0]).rezervacije.Find(x => x.smestajnaJedinica.id.Equals(sj.id)).status = Enumeracije.Status.aktivna;
            }
            else
            {
                Baza.korisnici.Find(x => x.korisnickoIme == Request.Cookies["LoggedIn"].Value.Split('_')[0]).rezervacije.Add(r);
            }

            XmlHandler.XmlHandler.UpdateFile(Baza.aranzmani);
            XmlHandler.XmlHandler.UpdateFile(Baza.korisnici);
            XmlHandler.XmlHandler.UpdateFile(Baza.smestaji);
            return View("Aranzman", Baza.aranzmani.Find(x=>x.naziv.Equals(nazAran)));
        }
        public static char[] GenerateRandomAlphanumericString(int length = 12)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            var random = new Random();
            var randomString = new string(Enumerable.Repeat(chars, length)
                                                    .Select(s => s[random.Next(s.Length)]).ToArray());
            return randomString.ToCharArray();
        }
        public ActionResult Komentar(string komentar, string ocena, string aranzman)
        {
            Komentar k = new Komentar()
            {
                aranzman = Baza.aranzmani.Find(x => x.naziv.Equals(aranzman)),
                ocena = Int32.Parse(ocena),
                tekst = komentar,
                turista = Baza.korisnici.Find(x => x.korisnickoIme.Equals(Request.Cookies["LoggedIn"].Value.Split('_')[0]))
            };
            Baza.komentari.Add(k);
            XmlHandler.XmlHandler.UpdateFile(Baza.komentari);
            return View("Aranzman", Baza.aranzmani.Find(x => x.naziv.Equals(aranzman)));
        }
        public ActionResult NapAranzmanView()
        {
            return View("NapraviAranzman");
        }
        public ActionResult NapraviAranzman(string naziv, string lokacija, string opis,
            string porgram, string brPutnika, string tipAranzmana, string prevoz,
            DateTime dPocetka, DateTime dZavrsetka, DateTime vrmNal, string adresa,
            string geoDuz, string geoSir, string smestaj, string program)
        {
            if(Baza.aranzmani.Find(x=>x.naziv.Equals(naziv)) != null)
            {
                ViewBag.Message = "Aranzman sa tim imenom vec postoji"; 
                return View();
            }
            try
            { 
                Int32.Parse(brPutnika);
                double.Parse(geoSir);
                double.Parse(geoDuz);
            }
            catch { ViewBag.Message = "unesi redovne brojeve"; return View(); }
            Enumeracije.TipAranzmana ta;
            switch(tipAranzmana){
                case "nocSaDoruckom": 
                    ta = Enumeracije.TipAranzmana.nocSaDoruckom;
                    break;
                case "polupansion":
                    ta = Enumeracije.TipAranzmana.polupansion;
                    break;
                case "punPansion":
                    ta = Enumeracije.TipAranzmana.punPansion;
                    break;
                case "olInkluziv":
                    ta = Enumeracije.TipAranzmana.olInkluziv;
                    break;
                default:
                    ta = Enumeracije.TipAranzmana.iznajmljenApartman;
                    break;
            }
            Enumeracije.TipPrevoza tp;
            switch (prevoz)
            {
                case "bus":
                    tp = Enumeracije.TipPrevoza.bus;
                    break;
                case "avion":
                    tp = Enumeracije.TipPrevoza.avion;
                    break;
                case "individualan":
                    tp = Enumeracije.TipPrevoza.individualan;
                    break;
                default:
                    tp = Enumeracije.TipPrevoza.ostalo;
                    break;
            }
            string po = "https://fthmb.tqn.com/fIxGlTIEndm_hzjjF3vO2UebDJI=/2048x1365/filters:fill(auto,1)/8659015428_f46d5495a1_k-56a6d4515f9b58b7d0e502d3.jpg";
            MestoNalazenja mn = new MestoNalazenja()
            {
                geografskaDuzina = double.Parse(geoDuz),
                geografskaSirina = double.Parse(geoSir),
                adresa = adresa
            };
            Aranzman a = new Aranzman()
            {
                datPocetka = dPocetka,
                datZavrsetka = dZavrsetka,
                maksimalanBrPutnika = Int32.Parse(brPutnika),
                lokacija = lokacija,
                naziv = naziv,
                opis = opis,
                poster = po,
                vremeNalazenja = vrmNal,
                program = program,
                tipAranzmana = ta,
                tipPrevoza = tp,
                smestaj = Baza.smestaji.Find(x => x.naziv.Equals(smestaj)),
                mestoNalazenja = mn
            };
            Baza.aranzmani.Add(a);
            Baza.korisnici.Find(x => x.korisnickoIme.Equals(Request.Cookies["LoggedIn"].Value.Split('_')[0])).aranzmani.Add(a);
            XmlHandler.XmlHandler.UpdateFile(Baza.aranzmani);
            XmlHandler.XmlHandler.UpdateFile(Baza.korisnici);
            return View();
        }
        public ActionResult NapraviSmestaj(string naziv, string brZvezdica, bool bazen, bool spaCentar, bool invaliditet, bool wifi, string tipSmestaja)
        {
            if (Baza.smestaji.Find(x => x.naziv.Equals(naziv)) != null)
            {
                ViewBag.MessageSmestaj = "Smestaj sa tim imenom vec postoji";
                return View();
            }
            Enumeracije.TipSmestaja ts;
            switch (tipSmestaja)
            {
                case "hotel":
                    ts = Enumeracije.TipSmestaja.hotel;
                    break;
                case "motel":
                    ts = Enumeracije.TipSmestaja.motel;
                    break;
                default:
                    ts = Enumeracije.TipSmestaja.vila;
                    break;
            }
            Smestaj s = new Smestaj()
            {
                spaCentar = spaCentar,
                bazen = bazen,
                brojZvezdica = brZvezdica.Length,
                invaliditet = invaliditet,
                naziv = naziv,
                wifi = wifi,
                tipSmestaja = ts,
            };
            Baza.smestaji.Add(s);
            XmlHandler.XmlHandler.UpdateFile(Baza.smestaji);
            return View("NapraviAranzman");
        }
        public ActionResult AzurirajAranzman(string naziv, string lokacija, string opis, 
            string brPutnika, string tipAranzmana, string prevoz,
            DateTime dPocetka, DateTime dZavrsetka, DateTime vrmNal, string program, string smestaj)
        {
            try
            {
                Int32.Parse(brPutnika);
            }
            catch { ViewBag.Message = "unesi redovne brojeve"; return View(); }
            Enumeracije.TipAranzmana ta;
            switch (tipAranzmana)
            {
                case "nocSaDoruckom":
                    ta = Enumeracije.TipAranzmana.nocSaDoruckom;
                    break;
                case "polupansion":
                    ta = Enumeracije.TipAranzmana.polupansion;
                    break;
                case "punPansion":
                    ta = Enumeracije.TipAranzmana.punPansion;
                    break;
                case "olInkluziv":
                    ta = Enumeracije.TipAranzmana.olInkluziv;
                    break;
                default:
                    ta = Enumeracije.TipAranzmana.iznajmljenApartman;
                    break;
            }
            Enumeracije.TipPrevoza tp;
            switch (prevoz)
            {
                case "bus":
                    tp = Enumeracije.TipPrevoza.bus;
                    break;
                case "avion":
                    tp = Enumeracije.TipPrevoza.avion;
                    break;
                case "individualan":
                    tp = Enumeracije.TipPrevoza.individualan;
                    break;
                default:
                    tp = Enumeracije.TipPrevoza.ostalo;
                    break;
            }
            string po = "https://fthmb.tqn.com/fIxGlTIEndm_hzjjF3vO2UebDJI=/2048x1365/filters:fill(auto,1)/8659015428_f46d5495a1_k-56a6d4515f9b58b7d0e502d3.jpg";


            Baza.aranzmani.Find(x => x.naziv.Equals(naziv)).datPocetka = dPocetka;
            Baza.aranzmani.Find(x => x.naziv.Equals(naziv)).datZavrsetka = dZavrsetka;
            Baza.aranzmani.Find(x => x.naziv.Equals(naziv)).maksimalanBrPutnika = Int32.Parse(brPutnika);
            Baza.aranzmani.Find(x => x.naziv.Equals(naziv)).lokacija = lokacija;
            Baza.aranzmani.Find(x => x.naziv.Equals(naziv)).naziv = naziv;
            Baza.aranzmani.Find(x => x.naziv.Equals(naziv)).opis = opis;
            Baza.aranzmani.Find(x => x.naziv.Equals(naziv)).poster = po;
            Baza.aranzmani.Find(x => x.naziv.Equals(naziv)).vremeNalazenja = vrmNal;
            Baza.aranzmani.Find(x => x.naziv.Equals(naziv)).program = program;
            Baza.aranzmani.Find(x => x.naziv.Equals(naziv)).tipAranzmana = ta;
            Baza.aranzmani.Find(x => x.naziv.Equals(naziv)).tipPrevoza = tp;
            Baza.aranzmani.Find(x => x.naziv.Equals(naziv)).smestaj = Baza.smestaji.Find(x => x.naziv.Equals(smestaj));

            Baza.korisnici.Find(x => x.korisnickoIme.Equals(Request.Cookies["LoggedIn"].Value.Split('_')[0])).aranzmani.Find(x=>x.naziv.Equals(naziv)).datPocetka = dPocetka;
            Baza.korisnici.Find(x => x.korisnickoIme.Equals(Request.Cookies["LoggedIn"].Value.Split('_')[0])).aranzmani.Find(x=>x.naziv.Equals(naziv)).datZavrsetka = dZavrsetka;
            Baza.korisnici.Find(x => x.korisnickoIme.Equals(Request.Cookies["LoggedIn"].Value.Split('_')[0])).aranzmani.Find(x=>x.naziv.Equals(naziv)).maksimalanBrPutnika = Int32.Parse(brPutnika);
            Baza.korisnici.Find(x => x.korisnickoIme.Equals(Request.Cookies["LoggedIn"].Value.Split('_')[0])).aranzmani.Find(x=>x.naziv.Equals(naziv)).lokacija = lokacija;
            Baza.korisnici.Find(x => x.korisnickoIme.Equals(Request.Cookies["LoggedIn"].Value.Split('_')[0])).aranzmani.Find(x=>x.naziv.Equals(naziv)).naziv = naziv;
            Baza.korisnici.Find(x => x.korisnickoIme.Equals(Request.Cookies["LoggedIn"].Value.Split('_')[0])).aranzmani.Find(x=>x.naziv.Equals(naziv)).opis = opis;
            Baza.korisnici.Find(x => x.korisnickoIme.Equals(Request.Cookies["LoggedIn"].Value.Split('_')[0])).aranzmani.Find(x=>x.naziv.Equals(naziv)).poster = po;
            Baza.korisnici.Find(x => x.korisnickoIme.Equals(Request.Cookies["LoggedIn"].Value.Split('_')[0])).aranzmani.Find(x=>x.naziv.Equals(naziv)).vremeNalazenja = vrmNal;
            Baza.korisnici.Find(x => x.korisnickoIme.Equals(Request.Cookies["LoggedIn"].Value.Split('_')[0])).aranzmani.Find(x=>x.naziv.Equals(naziv)).program = program;
            Baza.korisnici.Find(x => x.korisnickoIme.Equals(Request.Cookies["LoggedIn"].Value.Split('_')[0])).aranzmani.Find(x=>x.naziv.Equals(naziv)).tipAranzmana = ta;
            Baza.korisnici.Find(x => x.korisnickoIme.Equals(Request.Cookies["LoggedIn"].Value.Split('_')[0])).aranzmani.Find(x=>x.naziv.Equals(naziv)).tipPrevoza = tp;
            Baza.korisnici.Find(x => x.korisnickoIme.Equals(Request.Cookies["LoggedIn"].Value.Split('_')[0])).aranzmani.Find(x=>x.naziv.Equals(naziv)).smestaj = Baza.smestaji.Find(x => x.naziv.Equals(smestaj));
            
            XmlHandler.XmlHandler.UpdateFile(Baza.aranzmani);
            XmlHandler.XmlHandler.UpdateFile(Baza.korisnici);
            return View("Aranzman", Baza.aranzmani.Find(x => x.naziv.Equals(naziv)));
        }
        public ActionResult obrisiaranzman(string name)
        {
            if(Baza.korisnici.Find(x => x.rezervacije.Any(y=>y.aranzman.naziv.Equals(name)))!= null)
            {
                return View("index");
            }

            Baza.komentari.Find(x => x.aranzman.naziv == name).aranzman = null;
            Korisnik k = Baza.korisnici.Find(x => x.korisnickoIme.Equals(Request.Cookies["LoggedIn"].Value.Split('_')[0]));
            Baza.korisnici.Find(x => x.korisnickoIme.Equals(Request.Cookies["LoggedIn"].Value.Split('_')[0])).aranzmani.Remove(k.aranzmani.Find(x => x.naziv.Equals(name)));
            Baza.aranzmani.Find(x => x.naziv.Equals(name)).obrisan = true;
            
            XmlHandler.XmlHandler.UpdateFile(Baza.aranzmani);
            XmlHandler.XmlHandler.UpdateFile(Baza.komentari);
            XmlHandler.XmlHandler.UpdateFile(Baza.korisnici);

            return View("index");
        }
        public ActionResult NapraviSmestajnuJedinicu(string id, string dGostiju, string cena, bool kLjubimci, string smestaj)
        {
            SmestajnaJedinica sj = new SmestajnaJedinica
            {
                cena = Int32.Parse(cena),
                kucniLjubimci = kLjubimci,
                id = id,
                dozvoljenBrojGostiju = Int32.Parse(dGostiju)
            };
            Baza.smestaji.Find(x => x.naziv.Equals(smestaj)).smestajneJedinice.Add(sj);
            XmlHandler.XmlHandler.UpdateFile(Baza.smestaji);
            return View("index");
        }
    }
}
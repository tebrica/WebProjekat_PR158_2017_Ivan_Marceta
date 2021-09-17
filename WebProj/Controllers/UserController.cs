using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebProj.Models;
namespace WebProj.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View("Profil");
        }
        public ActionResult Profil()
        {
            var cookie = Request.Cookies["LoggedIn"];
            return View(Baza.korisnici.Find(x=>x.korisnickoIme.Equals(cookie.Value.Split('_')[0])));
        }
        public ActionResult Update(string email, string kime, string ime, string prz, string psw, DateTime dRodjenja, string pol, string uloga, string pswrepeat, string pswold)
        {
            var cookie = Request.Cookies["LoggedIn"];
            if (Baza.korisnici.Find(x=>x.korisnickoIme.Equals(Request.Cookies["LoggedIn"].Value.Split('_')[0])).lozinka != pswold.GetHashCode().ToString())
            {
                ViewBag.Message = "Stara lozinka netacna";
                return View("Profil", Baza.korisnici.Find(x => x.korisnickoIme.Equals(cookie.Value.Split('_')[0])));
            }
            if (psw != pswrepeat)
            {
                ViewBag.Message = "Lozinke se ne poklapaju!";
                return View("Profil", Baza.korisnici.Find(x => x.korisnickoIme.Equals(cookie.Value.Split('_')[0])));
            }
            if (Baza.korisnici.Find(x => x.korisnickoIme.Equals(kime)) != null || Baza.korisnici.Find(x => x.email.Equals(email)) != null)
            {
                Korisnik tst = Baza.korisnici.Find(x => x.korisnickoIme.Equals(Request.Cookies["LoggedIn"].Value.Split('_')[0]));
                if (tst.korisnickoIme != kime || tst.email != email)
                {
                    ViewBag.Message = "Korisnik vec postoji!";
                    return View("Profil", Baza.korisnici.Find(x => x.korisnickoIme.Equals(cookie.Value.Split('_')[0])));
                }
            }
            Enumeracije.Pol p;
            Enumeracije.Uloga u;
            if (pol == "musko")
            {
                p = Enumeracije.Pol.muski;
            }
            else
            {
                p = Enumeracije.Pol.zenski;
            }
            if (uloga == "turista")
            {
                u = Enumeracije.Uloga.turista;
            }
            else if (uloga == "menadzer")
            {
                u = Enumeracije.Uloga.menadzer;
            }
            else
            {
                u = Enumeracije.Uloga.administrator;
            }
            Korisnik k = new Korisnik()
            {
                datumRodjenja = dRodjenja,
                email = email,
                ime = ime,
                korisnickoIme = kime,
                lozinka = psw.GetHashCode().ToString(),
                pol = p,
                uloga = u,
                prezime = prz,
            };
            Baza.korisnici.Remove(Baza.korisnici.Find(x => x.korisnickoIme.Equals(Request.Cookies["LoggedIn"].Value.Split('_')[0])));
            Baza.korisnici.Add(k);
            XmlHandler.XmlHandler.UpdateFile(Baza.korisnici);

            HttpCookie kuki = new HttpCookie("LoggedIn", kime + "_" + Baza.korisnici.Find(x => x.korisnickoIme == kime).uloga);
            kuki.Expires = DateTime.Now.AddMinutes(1440);
            Response.Cookies.Add(kuki);

            return RedirectToAction("Profil");
        }
    }
}
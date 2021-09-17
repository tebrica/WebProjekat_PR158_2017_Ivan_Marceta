using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebProj.Models;
namespace WebProj.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            new XmlHandler.XmlHandler();
            return View();
        }
        public ActionResult LoginPage()
        {
            return View("Login");
        }
        public ActionResult RegisterPage()
        {
            return View("Index");
        }
        public ActionResult Register(string email, string kime, string ime, string prz, string psw, DateTime dRodjenja, string pol, string uloga, string pswrepeat)
        {
            if (uloga != "menadzer")
            {
                if (Request.Cookies.Get("LoggedIn") != null)
                {
                    ViewBag.message = "Vec si ulogovan";
                    return View("Index");
                }
            }
            if (psw != pswrepeat)
            {
                ViewBag.Message = "Lozinke se ne poklapaju!";
                return View("Index");
            }
            if (Baza.korisnici.Find(x => x.korisnickoIme.Equals(kime)) != null || Baza.korisnici.Find(x => x.email.Equals(email)) != null)
            {
                ViewBag.Message = "Korisnik vec postoji!";
                return View("Index");
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
            else if(uloga == "menadzer")
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
            Baza.korisnici.Add(k);
            XmlHandler.XmlHandler.UpdateFile(Baza.korisnici);
            if (uloga != "menadzer")
            {
                LogInFunction(kime, psw);
            }
            return RedirectToAction("Aranzmani", "Aranzmani");
        }
        public ActionResult LogIn(string kime, string lozinka)
        {
            if (Request.Cookies.Get("LoggedIn") != null)
            {
                ViewBag.message = "Vec si ulogovan";
                return View("Index");
            }
            if (LogInFunction(kime, lozinka))
                return RedirectToAction("Aranzmani", "Aranzmani");
            return View("index");
        }
        public bool LogInFunction(string kime, string lozinka)
        {
            if (Request.Cookies.Get("LoggedIn") != null)
            {
                ViewBag.message = "Vec si ulogovan";
                return false;
            }
            if (Baza.korisnici.Find(x => x.korisnickoIme == kime && x.lozinka == lozinka.GetHashCode().ToString()) != null)
            {
                HttpCookie cookie = new HttpCookie("LoggedIn", kime + "_" + Baza.korisnici.Find(x => x.korisnickoIme == kime).uloga);
                cookie.Expires = DateTime.Now.AddMinutes(1440);
                Response.Cookies.Add(cookie);
            }
            else
            {
                ViewBag.message = "Prvo se Registruj";
                return false;
            }
            return true;
        }
        public ActionResult Logout()
        {
            if (Request.Cookies.Get("LoggedIn") == null)
            {
                ViewBag.message = "Vec si izlogovan";
                return RedirectToAction("RegisterPage");
            }
            HttpCookie cookie = new HttpCookie("LoggedIn");
            cookie.Expires = DateTime.Now.AddMinutes(-1440);
            Response.Cookies.Add(cookie);

            return RedirectToAction("RegisterPage");
        }
    }
}
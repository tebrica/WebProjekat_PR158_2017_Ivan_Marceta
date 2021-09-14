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
            XmlHandler.XmlHandler xh = new XmlHandler.XmlHandler();

            List<Aranzman> ar = Baza.aranzmani.OrderBy(x => x.datPocetka).ToList().FindAll(x => x.datPocetka > DateTime.Now).ToList();
            return View(ar);
        }
        public ActionResult SviAranzmani()
        {
            List<Aranzman> ar = Baza.aranzmani.OrderBy(x => x.datZavrsetka).ToList();
            return View(ar);
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
    }
}
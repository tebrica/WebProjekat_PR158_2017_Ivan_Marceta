public ActionResult Rezervisi(int smesJedinicaRb, string nazAran)
        {
            SmestajnaJedinica sj = Baza.aranzmani.Find(x => x.naziv == nazAran).smestaj.smestajneJedinice[smesJedinicaRb];
            if(Baza.korisnici.Find(x=>x.korisnickoIme == Request.Cookies["LoggedIn"].Value.Split('_')[0]).rezervacije.Find(x=>x.smestajnaJedinica.id.Equals(sj.id)) != null)
            {
                Baza.korisnici.Find(x => x.korisnickoIme == Request.Cookies["LoggedIn"].Value.Split('_')[0]).rezervacije.Find(x => x.smestajnaJedinica.id.Equals(sj.id)).status = Enumeracije.Status.otkazana;
                
                sj.dozvoljenBrojGostiju *= -1;
            }
            if(sj.dozvoljenBrojGostiju < 0 && Baza.korisnici.Find(x => x.korisnickoIme == Request.Cookies["LoggedIn"].Value.Split('_')[0]).rezervacije.Find(x => x.smestajnaJedinica.id.Equals(sj.id)) == null)
            {
                ViewBag.rezervacija = "smestajna jedinica vec rezervisana";
                return View("Aranzman",Baza.aranzmani.Find(x=>x.naziv.Equals(nazAran)));
            }
            if(sj.dozvoljenBrojGostiju > 0)
            {
                sj.dozvoljenBrojGostiju *= -1;
                Baza.smestaji.Find(x => x.smestajneJedinice.Find(y => y.id.Equals(sj.id)) != null).smestajneJedinice.Find(x => x.id == sj.id).dozvoljenBrojGostiju *= -1;
                Baza.aranzmani.Find(x => x.smestaj.smestajneJedinice.Find(y => y.id.Equals(sj.id)) != null).smestaj.smestajneJedinice.Find(x => x.id.Equals(sj.id)).dozvoljenBrojGostiju *= -1;
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
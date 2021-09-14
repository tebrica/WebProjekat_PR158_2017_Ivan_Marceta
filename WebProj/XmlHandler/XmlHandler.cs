using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace WebProj.XmlHandler
{
    public class XmlHandler
    {
        static XmlHandler()
        {
            Models.Baza.aranzmani = TakeOrMake<List<Models.Aranzman>>(Models.Baza.aranzmani);
            Models.Baza.komentari = TakeOrMake<List<Models.Komentar>>(Models.Baza.komentari);
            Models.Baza.korisnici = TakeOrMake<List<Models.Korisnik>>(Models.Baza.korisnici);
            Models.Baza.mestaNalazenja = TakeOrMake<List<Models.MestoNalazenja>>(Models.Baza.mestaNalazenja);
            Models.Baza.rezervacije = TakeOrMake<List<Models.Rezervacija>>(Models.Baza.rezervacije);
            Models.Baza.smestaji = TakeOrMake<List<Models.Smestaj>>(Models.Baza.smestaji);
            Models.Baza.smestajneJedinice = TakeOrMake<List<Models.SmestajnaJedinica>>(Models.Baza.smestajneJedinice);
        }
        private static T TakeOrMake<T>(object p)
        {
            try
            {
                return GetXMLGenericType<T>("C:/Users/i.marceta/source/repos/WebProj/WebProj/Files/file" + p.ToString() + ".xml");
            }
            catch
            {
                UpdateFile(p);
                return GetXMLGenericType<T>("C:/Users/i.marceta/source/repos/WebProj/WebProj/Files/file" + p.ToString() + ".xml");
            }
        }
        private static T GetXMLGenericType<T>(string xmlFile)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            var generatedType = (T)serializer.Deserialize(new StreamReader(xmlFile));
            return (T)Convert.ChangeType(generatedType, typeof(T));
        }
        public static void UpdateFile(object p)
        {
            XmlSerializer x = new XmlSerializer(p.GetType());
            TextWriter writer = new StreamWriter("C:/Users/i.marceta/source/repos/WebProj/WebProj/Files/file" + p.ToString() + ".xml");
            x.Serialize(writer, p);
            writer.Close();
        }
    }
}
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
            Models.Baza.korisnici = TakeOrMake<List<Models.Korisnik>>(Models.Baza.korisnici);
            Models.Baza.smestaji = TakeOrMake<List<Models.Smestaj>>(Models.Baza.smestaji);
            Models.Baza.komentari = TakeOrMake<List<Models.Komentar>>(Models.Baza.komentari);
        }
        private static T TakeOrMake<T>(object p)
        {
            try
            {
                return GetXMLGenericType<T>("C:/Users/i.marceta/source/repos/WebProj/WebProj/Files/file" + p.ToString() + ".xml");
            }
            catch(Exception e)
            {
                UpdateFile(p);
                return GetXMLGenericType<T>("C:/Users/i.marceta/source/repos/WebProj/WebProj/Files/file" + p.ToString() + ".xml");
            }
        }
        private static T GetXMLGenericType<T>(string xmlFile)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            StreamReader sr = new StreamReader(xmlFile);
            var generatedType = (T)serializer.Deserialize(sr);
            sr.Close();
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
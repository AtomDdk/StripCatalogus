using Domeinlaag;
using Domeinlaag.Interfaces;
using Domeinlaag.Model;
using Newtonsoft.Json;
using StripCatalogus___Data_Layer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConsoleApp
{
    public static class Reader
    {
        public static void GoesBrrrr()
        {
            StreamReader re = new StreamReader(@"C:\Users\Maaren\Documents\dump.json");
            JsonTextReader reader = new JsonTextReader(re);
            Newtonsoft.Json.JsonSerializer se = new Newtonsoft.Json.JsonSerializer();
            List<RootJson> parsedData = se.Deserialize<List<RootJson>>(reader);

            CatalogusManager mng = new CatalogusManager(new UnitOfWork());
            Dictionary<int, Uitgeverij> uitgeverijen = new Dictionary<int, Uitgeverij>();
            Dictionary<int, Reeks> reeksen = new Dictionary<int, Reeks>();
            Dictionary<int, Auteur> auteurs = new Dictionary<int, Auteur>();
            var timer = Stopwatch.StartNew();
            //using StreamWriter writer = new StreamWriter(@"C:\Users\davy\Documents\data\log.txt");
            foreach (var strip in parsedData)
            {
                
                timer.Start();
                if (!uitgeverijen.ContainsKey(strip.Uitgeverij.ID))
                    uitgeverijen.Add(strip.Uitgeverij.ID, mng.VoegUitgeverijToe(strip.Uitgeverij.Naam));

                if (!reeksen.ContainsKey(strip.Reeks.ID))
                    reeksen.Add(strip.Reeks.ID, mng.VoegReeksToe(strip.Reeks.Naam));

                List<Auteur> stripAuteurs = new List<Auteur>();
                foreach (var a in strip.Auteurs)
                {
                    if (!auteurs.ContainsKey(a.ID))
                        auteurs.Add(a.ID, mng.VoegAuteurToe(a.Naam));
                    stripAuteurs.Add(auteurs[a.ID]);
                }
                try
                {
                    mng.VoegStripToe(strip.Titel, reeksen[strip.Reeks.ID], strip.Nr, stripAuteurs, uitgeverijen[strip.Uitgeverij.ID]);
                }
                catch (Exception ex)
                {
                    //writer.WriteLine(ex.Message);
                    //writer.WriteLine(strip);
                }
                Console.WriteLine(timer.Elapsed);
            }

        }

        private class ReeksJson
        {
            public int ID { get; set; }
            public string Naam { get; set; }
            public List<object> Strips { get; set; }
            public override bool Equals(object obj)
            {
                return obj is ReeksJson json &&
                       ID == json.ID &&
                       Naam == json.Naam &&
                       EqualityComparer<List<object>>.Default.Equals(Strips, json.Strips);
            }
            public override int GetHashCode()
            {
                return HashCode.Combine(ID, Naam, Strips);
            }
            public override string ToString()
            {
                return $"Reeks: [{ID}, {Naam}] ";
            }
        }
        private class UitgeverijJson
        {
            public int ID { get; set; }
            public string Naam { get; set; }

            public override bool Equals(object obj)
            {
                return obj is UitgeverijJson json &&
                       ID == json.ID &&
                       Naam == json.Naam;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(ID, Naam);
            }
            public override string ToString()
            {
                return $"Uitgeverij: [{ID}, {Naam}] ";
            }
        }

        private class AuteurJson
        {
            public int ID { get; set; }
            public string Naam { get; set; }

            public override bool Equals(object obj)
            {
                return obj is AuteurJson json &&
                       ID == json.ID &&
                       Naam == json.Naam;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(ID, Naam);
            }
            public override string ToString()
            {
                return $"Auteur: [{ID}, {Naam}] ";
            }
        }

        private class RootJson
        {
            public int ID { get; set; }
            public string Titel { get; set; }
            public int? Nr { get; set; }
            public ReeksJson Reeks { get; set; }
            public UitgeverijJson Uitgeverij { get; set; }
            public List<AuteurJson> Auteurs { get; set; }
            public override string ToString()
            {
                string x = null;
                foreach (var a in Auteurs)
                    x += a.ToString();
                return $"Strip: {ID}, {Titel}, {Nr}, {Reeks}, Auteurs:[ {x}], {Uitgeverij}";
            }
        }
    }
}

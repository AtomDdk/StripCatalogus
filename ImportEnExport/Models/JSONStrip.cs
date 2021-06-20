using Domeinlaag.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImportEnExport.Models
{
    public class JSONStrip
    {
        public int ID { get; set; }
        public string Titel { get; set; }
        public int? Nr { get; set; }
        public JSONReeks Reeks { get; set; }
        public JSONUitgeverij Uitgeverij { get; set; }
        public List<JSONAuteur> Auteurs { get; set; }
        public override string ToString()
        {
            string x = null;
            foreach (var a in Auteurs)
                x += a.ToString();
            return $"Strip: {ID}, {Titel}, {Nr}, {Reeks}, Auteurs:[ {x}], {Uitgeverij}";
        }
    }
}

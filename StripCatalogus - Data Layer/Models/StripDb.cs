using System;
using System.Collections.Generic;
using System.Text;

namespace StripCatalogus___Data_Layer.Models
{
    public class StripDb
    {
        public int Id { get; set; }
        public string Titel { get; set; }
        public ReeksDb Reeks { get; set; }
        public int? ReeksNummer { get; set; }
        public IList<AuteurDb> Auteurs { get; set; }
        public UitgeverijDb Uitgeverij { get; set; }
        public int Aantal { get; set; }
    }
}

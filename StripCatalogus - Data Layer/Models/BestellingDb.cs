using Domeinlaag.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace StripCatalogus___Data_Layer.Models
{
    public class BestellingDb
    {
        public int Id { get; set; }
        public DateTime Datum { get; set; }
        public Dictionary<StripDb, int> StripsEnAantal { get; set; } = new Dictionary<StripDb, int>();
    }
}

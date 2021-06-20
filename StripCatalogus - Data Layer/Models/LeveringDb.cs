using System;
using System.Collections.Generic;
using System.Text;

namespace StripCatalogus___Data_Layer.Models
{
    public class LeveringDb
    {
        public int Id { get; set; }
        public Dictionary<StripDb, int> StripsEnAantal { get; set; } = new Dictionary<StripDb, int>();
        public DateTime BestelDatum { get; set; }
        public DateTime LeverDatum { get; set; }
    }
}

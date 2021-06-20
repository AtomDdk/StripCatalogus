using Domeinlaag.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImportEnExport
{
    /// <summary>
    /// Klas om de overdracht van informatie van parser naar programma dat parser gebruikt simpeler te maken.
    /// </summary>
    public class Report
    {
        public int TotaalAantalStrips { get; set; }
        public int AantalIngeladenStrips { get; set; }
        public int AantalNietIngeladenStrips { get; set; }
        public List<string> IngeladenStrips { get; set; } = new List<string>();
        public Dictionary<string, List<string>> NietIngeladenStrips { get; set; } = new Dictionary<string, List<string>>();
        public void AddIngeladenStrip(string strip)
        {
            IngeladenStrips.Add(strip);
        }
        public void AddNietIngeladenStrip(string errorMessage, string strip)
        {
            if (!NietIngeladenStrips.ContainsKey(errorMessage))
                NietIngeladenStrips.Add(errorMessage, new List<string>() { strip });
            else
                NietIngeladenStrips[errorMessage].Add(strip);
        }
        public string ErrorLog()
        {
            string errorLog = null;
            foreach(var erorrMessage in NietIngeladenStrips.Keys)
            {
                errorLog += erorrMessage + "\n";
                foreach (var strip in NietIngeladenStrips[erorrMessage])
                {
                    errorLog += $"    {strip}\n";
                }
            }
            return errorLog;
        }
    }
}

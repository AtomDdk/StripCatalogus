using Domeinlaag;
using Domeinlaag.Interfaces;
using Domeinlaag.Model;
using ImportEnExport.Models;
using Newtonsoft.Json;
using StripCatalogus___Data_Layer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace ImportEnExport
{
    public class Parser
    {
        public ICatalogusManager _mng;
        public Parser(ICatalogusManager mng)
        {
            _mng = mng;
        }

        public void IsFileJson(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath) || filePath.Length < 6 || filePath.Trim().Substring(filePath.Trim().Length - 5, 5).ToLower() != ".json")
                throw new ParserException("Het geselecteerde bestand is geen json.");
        }

        /// <summary>
        /// Import een Json bestand van een meegegeven pad in de database en returnt een report.
        /// </summary>
        /// <param name="filePath"></param>
        public Report Import(string filePath)
        {
            IsFileJson(filePath);
            JsonSerializer se = new JsonSerializer();
            using StreamReader reader = new StreamReader(filePath);
            List<JSONStrip> parsedData = se.Deserialize<List<JSONStrip>>(new JsonTextReader(reader));
            reader.Dispose();

            #region De auteurs, uitgeverijen en reeksen in het parsedData bestand op naam vergelijken met degene die in de Db zitten en toevoegen aan een Dictionary
            //dictionary met als key de Id vanuit het JSON bestand en als value het object uit de databank.
            var auteurs = _mng.GeefAlleAuteurs().Join(
                parsedData.SelectMany(a => a.Auteurs).Distinct(),
                auteurDb => auteurDb.Naam.Trim(),
                auteurJson => auteurJson.Naam.Trim(),
                (auteurDb, auteurJson) => new
                {
                    auteurJson.ID,
                    auteurDb
                }).ToDictionary(x => x.ID, x => x.auteurDb);

            //dictionary met als key de Id vanuit het JSON bestand en als value het object uit de databank.
            var uitgeverijen = _mng.GeefAlleUitgeverijen().Join(
                parsedData.Select(u => u.Uitgeverij).Distinct(),
                uitgeverijDb => uitgeverijDb.Naam.Trim(),
                uitgeverijJson => uitgeverijJson.Naam.Trim(),
                (uitgeverijDb, uitgeverijJson) => new
                {
                    uitgeverijJson.ID,
                    uitgeverijDb
                }).ToDictionary(x => x.ID, x => x.uitgeverijDb);

            //dictionary met als key de Id vanuit het JSON bestand en als value het object uit de databank.
            var reeksen = _mng.GeefAlleReeksen().Join(
                parsedData.Select(r => r.Reeks).Distinct(),
                reeksBd => reeksBd.Naam.Trim(),
                reeksJson => reeksJson.Naam.Trim(),
                (reeksDb, reeksJson) => new
                {
                    reeksJson.ID,
                    reeksDb
                }).ToDictionary(x => x.ID, x => x.reeksDb);
            reeksen.Add(parsedData.Select(s => s.Reeks).Where(r => r.Naam == "<geen serie>").FirstOrDefault().ID, null);
            #endregion

            // alle onderdelen van een strip en de strip zelf in de Db steken en het report vullen.
            Report report = new Report
            {
                TotaalAantalStrips = parsedData.Count()
            };
            foreach (var strip in parsedData)
            {
                if (!uitgeverijen.ContainsKey(strip.Uitgeverij.ID))
                    uitgeverijen.Add(strip.Uitgeverij.ID, _mng.VoegUitgeverijToe(strip.Uitgeverij.Naam.Trim()));

                if (!reeksen.ContainsKey(strip.Reeks.ID))
                    reeksen.Add(strip.Reeks.ID, _mng.VoegReeksToe(strip.Reeks.Naam.Trim()));

                List<Auteur> stripAuteurs = new List<Auteur>();
                foreach (var a in strip.Auteurs)
                {
                    if (!auteurs.ContainsKey(a.ID))
                        auteurs.Add(a.ID, _mng.VoegAuteurToe(a.Naam.Trim()));

                    stripAuteurs.Add(auteurs[a.ID]);
                }
                try
                {
                    Strip stripDomein = _mng.VoegStripToe(strip.Titel.Trim(), reeksen[strip.Reeks.ID], strip.Nr, stripAuteurs, uitgeverijen[strip.Uitgeverij.ID]);
                    report.AantalIngeladenStrips++;
                    report.AddIngeladenStrip(stripDomein.ToString());
                }
                catch (Exception ex)
                {
                    report.AantalNietIngeladenStrips++;
                    report.AddNietIngeladenStrip(ex.Message, strip.ToString());
                }
            }
            if (report.TotaalAantalStrips != report.AantalNietIngeladenStrips + report.AantalIngeladenStrips)
                throw new ParserException("Het aantal strips klopt niet: " + report);

            // het report returnen.
            return report;
        }


        /// <summary>
        /// Vertaalt de strips in de database naar Json-objecten en maakt dan het Json bestand aan.
        /// </summary>
        /// <param name="directoryInfo"></param>
        public void Export(string filePath)
        {
            IsFileJson(filePath);

            // De strips uit de database omzetten naar de Json-objecten die we hebben meegekregen
            IEnumerable<JSONStrip> strips = _mng.GeefAlleStrips().Select(strip => new JSONStrip
            {
                ID = strip.Id,
                Titel = strip.Titel,
                Reeks = strip.Reeks != null ? new JSONReeks { ID = strip.Reeks.Id, Naam = strip.Reeks.Naam } :
                new JSONReeks { ID = 0, Naam = "<geen serie>" },
                Nr = strip.ReeksNummer == null ? null : strip.ReeksNummer,
                Auteurs = strip.GeefAuteurs().Select(a => new JSONAuteur
                {
                    ID = a.Id,
                    Naam = a.Naam
                }).ToList(),
                Uitgeverij = new JSONUitgeverij { ID = strip.Uitgeverij.Id, Naam = strip.Uitgeverij.Naam }
            }).OrderBy(x => x.ID);

            // Json bestand aanmaken
            var serializer = new JsonSerializer();
            using StreamWriter sW = new StreamWriter(filePath);
            serializer.Serialize(new JsonTextWriter(sW), strips);
            sW.Dispose();
        }
    }
}

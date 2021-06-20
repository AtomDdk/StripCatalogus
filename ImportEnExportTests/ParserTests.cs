using Domeinlaag;
using Domeinlaag.Interfaces;
using Domeinlaag.Model;
using ImportEnExport;
using IntegratieTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;

namespace ImportEnExportTests
{
    [TestClass]
    public class ParserTests
    {
        private readonly Mock<ICatalogusManager> _mockMng;
        private readonly Parser _parser;
        public ParserTests()
        {
            _mockMng = new Mock<ICatalogusManager>();
            _parser = new Parser(_mockMng.Object);
        }

        //private void VulDataBankMetDummyData(CatalogusManager Cm)
        //{

        //    Uitgeverij uitgeverij1 = Cm.VoegUitgeverijToe("TestUitgeverij1");
        //    Uitgeverij uitgeverij2 = Cm.VoegUitgeverijToe("TestUitgeverij2");
        //    Auteur auteur1 = Cm.VoegAuteurToe("TestAuteur1");
        //    Auteur auteur2 = Cm.VoegAuteurToe("TestAuteur2");
        //    Reeks reeks1 = Cm.VoegReeksToe("TestReeks1");

        //    Uitgeverij uitgeverij3 = Cm.VoegUitgeverijToe("TestUitgeverij3");
        //    Auteur auteur3 = Cm.VoegAuteurToe("TestAuteur3");

        //    Strip strip1 = Cm.VoegStripToe("TestStrip1", reeks1, 1, new List<Auteur> { auteur1 }, uitgeverij1);
        //    Strip strip2 = Cm.VoegStripToe("TestStrip2", reeks1, 2, new List<Auteur> { auteur1, auteur2 }, uitgeverij2);
        //    Strip strip3 = Cm.VoegStripToe("TestStrip3", null, null, new List<Auteur> { auteur3 }, uitgeverij3);
        //}


        [TestMethod]
        public void IsFileJson_SlechtFile_ThrowsParserException()
        {
            Assert.ThrowsException<ParserException>(() => _parser.IsFileJson("Ditiseenslechtfile.doc"));
        }
        [TestMethod]
        public void Import_FilePathBestaatNiet_ThrowsException()
        {
            Assert.ThrowsException<FileNotFoundException>(() => _parser.Import("ditpilepathBestaatniet.json"));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Domeinlaag.Interfaces;
using Domeinlaag.Model;
using Microsoft.Extensions.Configuration;
using StripCatalogus___Data_Layer.Repositories;

namespace StripCatalogus___Data_Layer
{
    public class UnitOfWork : IUnitOfWork
    {
        protected string _connectionString;
        public UnitOfWork(string db = "Production")
        {
            SetConnectionString(db);
            Uitgeverijen = new UitgeverijRepository(_connectionString);
            Auteurs = new AuteurRepository(_connectionString);
            Strips = new StripRepository(_connectionString);
            Reeksen = new ReeksRepository(_connectionString);
            Bestellingen = new BestellingsRepository(_connectionString);
            Leveringen = new LeveringRepository(_connectionString);
        }
        private void SetConnectionString(string db = "Production")
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("ConnectionStrings.json", optional: false);
            var configuration = builder.Build();

            switch (db)
            {
                case "Production":
                    _connectionString = configuration.GetConnectionString("Production").ToString();
                    break;
                case "Test":
                    _connectionString = configuration.GetConnectionString("Test").ToString();
                    break;
            }
        }
        public IUitgeverijRepository Uitgeverijen { get; set; }
        public IStripRepository Strips { get; set; }
        public IAuteurRepository Auteurs { get; set; }
        public IReeksRepository Reeksen { get; set; }
        public IBestellingRepository Bestellingen { get; set; }
        public ILeveringRepository Leveringen { get; set; }
    }
}

//switch (db)
//{
//    case "Production":
//        //Davy
//        //_connectionString = @"Data Source=DESKTOP-67T82V5\SQLEXPRESS;Initial Catalog=StripCatalogus;Integrated Security=True";
//        //Maaren
//        //_connectionString = @"Data Source=LAPTOP-LPTQS5FD\SQLEXPRESS;Initial Catalog=master;Integrated Security=True";
//        //_connectionString = @"Data Source=DESKTOP-VCI7746\SQLEXPRESS;Initial Catalog=StripCatalogus;Integrated Security=True";
//        //Sven
//        _connectionString = @"Data Source=DESKTOP-VCI7746\SQLEXPRESS;Initial Catalog=StripCatalogus;Integrated Security=True";
//        break;
//    case "Test":
//        //Davy
//        //_connectionString = @"Data Source=DESKTOP-67T82V5\SQLEXPRESS;Initial Catalog=StripCatalogusTest;Integrated Security=True";
//        //Sven
//        _connectionString = @"Data Source=DESKTOP-VCI7746\SQLEXPRESS;Initial Catalog=StripCatalogusTest;Integrated Security=True";
//        break;
//}
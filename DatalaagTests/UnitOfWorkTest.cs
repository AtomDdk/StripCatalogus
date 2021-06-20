using Domeinlaag.Model;
using StripCatalogus___Data_Layer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace IntegratieTests
{
    public class UnitOfWorkTest : UnitOfWork
    {
        public UnitOfWorkTest(bool VulDatabank = false) : base("Test")
        {
            VerwijderAlles();
            if (VulDatabank) QueriesGoBrrrrrr();
        }

        public void QueriesGoBrrrrrr()
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            List<string> queries = new List<string>()
            {
                // uitgeverijen toevoegen
                "INSERT INTO Uitgeverij (Naam) VALUES ('Standaard');", // Id: 1
                "INSERT INTO Uitgeverij (Naam) VALUES ('DC comics');", // Id: 2
                "INSERT INTO Uitgeverij (Naam) VALUES ('Dupuis');",    // Id: 3
                // reeksen toevoegen
                "INSERT INTO Reeks (Naam) VALUES ('Asterix');",      // Id: 1
                "INSERT INTO Reeks (Naam) VALUES ('Batman');",       // Id: 2
                "INSERT INTO Reeks (Naam) VALUES ('Lucky Luke');",   // Id: 3
                // auteurs toevoegen
                "INSERT INTO Auteur (Naam) VALUES ('Goscinny Rene');",    // Id: 1
                "INSERT INTO Auteur (Naam) VALUES ('Uderzo Albert');",    // Id: 2
                "INSERT INTO Auteur (Naam) VALUES ('Zack Snyder');",      // Id: 3
                // Strip toevoegen
                "INSERT INTO Strip (Titel, ReeksId, ReeksNummer, UitgeverijId) VALUES " +       // id 1
                "('Asterix en de Gothen', 1, 1, 3)",
                "INSERT INTO Strip (Titel, ReeksId, ReeksNummer, UitgeverijId) VALUES " +
                "('Asterix en de Brittaniers', 1, 2, 3)",                                       // id 2
                "INSERT INTO Strip (Titel, ReeksId, ReeksNummer, UitgeverijId) VALUES " +
                "('Tales Of Donald Duck', null, null, 2)",                                      // id 3
                "INSERT INTO Strip (Titel, ReeksId, ReeksNummer, UitgeverijId) VALUES " +
                "('De Postkoets', 3, 1, 1)",                                                    // id 4
                // StripAuteurs Linken
                "INSERT INTO StripAuteur (StripId, AuteurId) VALUES (1, 1)",
                "INSERT INTO StripAuteur (StripId, AuteurId) VALUES (1, 2)",
                "INSERT INTO StripAuteur (StripId, AuteurId) VALUES (2, 2)",
                "INSERT INTO StripAuteur (StripId, AuteurId) VALUES (2, 1)",
                "INSERT INTO StripAuteur (StripId, AuteurId) VALUES (3, 3)",
                "INSERT INTO StripAuteur (StripId, AuteurId) VALUES (4, 2)",
                // StripAantallen toevoegen
                "INSERT INTO StripAantal (StripId, Aantal) VALUES (1, 19)",
                "INSERT INTO StripAantal (StripId, Aantal) VALUES (2, 12)",
                "INSERT INTO StripAantal (StripId, Aantal) VALUES (3, 26)",
                "INSERT INTO StripAantal (StripId, Aantal) VALUES (4, 0)",
                // Bestelling toevoegen
                "INSERT INTO Bestelling (Datum) VALUES ('2000-01-01 12:00:00');", // Id:1
                "INSERT INTO Bestelling (Datum) VALUES ('2000-01-01 12:30:00');", // Id:2
                // StripBestelling linken
                "INSERT INTO StripBestelling (StripId, BestellingId, Aantal) VALUES (1, 1, 9);",
                "INSERT INTO StripBestelling (StripId, BestellingId, Aantal) VALUES (2, 1, 2);",
                "INSERT INTO StripBestelling (StripId, BestellingId, Aantal) VALUES (3, 2, 20);",
                "INSERT INTO StripBestelling (StripId, BestellingId, Aantal) VALUES (1, 2, 10);",
                // Levering toevoegen
                $"INSERT INTO Levering (BestelDatum, LeverDatum) VALUES " +
                $"('2000-01-01 12:30:00', '2000-01-01 18:00:00')",   // Id: 1
                $"INSERT INTO Levering (BestelDatum, LeverDatum) VALUES " +
                $"('2000-01-01 12:30:00', '2000-01-01 18:30:00')",   // Id: 2
                // StripLevering linken
                "INSERT INTO StripLevering (StripId, LeveringId, Aantal) VALUES (3, 2, 26);",
                "INSERT INTO StripLevering (StripId, LeveringId, Aantal) VALUES (2, 1, 12);",
                "INSERT INTO StripLevering (StripId, LeveringId, Aantal) VALUES (1, 1, 19);",
            };
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                try
                {
                    foreach (string query in queries)
                    {
                        command.CommandText = query;
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        public void VerwijderAlles()
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            List<string> querys = new List<string>
            {
                "DELETE FROM StripAuteur;",
                "DELETE FROM StripAantal;",
                "DELETE FROM StripBestelling;",
                "DELETE FROM StripLevering;",
                "DELETE FROM StripLevering;", 
                "DELETE FROM Levering; DBCC CHECKIDENT('Levering', RESEED, 0);",
                "DELETE FROM Bestelling; DBCC CHECKIDENT('Bestelling', RESEED, 0);",
                "DELETE FROM Auteur; DBCC CHECKIDENT('Auteur', RESEED, 0); ",
                "DELETE FROM Strip;  DBCC CHECKIDENT('Strip', RESEED, 0); ",
                "DELETE FROM Uitgeverij; DBCC CHECKIDENT('Uitgeverij', RESEED, 0); ",
                "DELETE FROM Reeks; DBCC CHECKIDENT('Reeks', RESEED, 0);"
        };
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                try
                {
                    foreach (string query in querys)
                    {
                        command.CommandText = query;
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        
    }
}

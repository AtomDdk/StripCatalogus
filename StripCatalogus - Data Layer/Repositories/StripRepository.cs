using Domeinlaag.Interfaces;
using Domeinlaag.Model;
using StripCatalogus___Data_Layer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace StripCatalogus___Data_Layer.Repositories
{
    public class StripRepository : IStripRepository
    {
        // fuck git
        private string _connectionString;
        public StripRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
        /// <summary>
        /// Voegt een Strip toe an de databank en returnt een Id.
        /// </summary>
        /// <param name="strip"></param>
        /// <returns></returns>
        public int VoegStripToe(Strip strip)
        {
            SqlConnection connection = GetConnection();
            string queryI = "INSERT INTO Strip (Titel,ReeksId,ReeksNummer,UitgeverijId) output INSERTED.ID VALUES(@Titel,@ReeksId,@ReeksNummer,@UitgeverijId)";
            string queryII = "INSERT INTO StripAuteur (StripId, AuteurId) VALUES (@StripId,@AuteurId)";
            string queryIII = "INSERT INTO Stripaantal (StripId, Aantal) VALUES (@StripId,@Aantal)";
            StripDb stripDb = Mapper.DomainToDb(strip);

            using (SqlCommand command1 = connection.CreateCommand())
            using (SqlCommand command2 = connection.CreateCommand())
            using (SqlCommand command3 = connection.CreateCommand())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                command1.Transaction = transaction;
                command2.Transaction = transaction;
                command3.Transaction = transaction;
                try
                {
                    //strip toevoegen
                    command1.Parameters.Add(new SqlParameter("@Titel", SqlDbType.NVarChar)).Value = stripDb.Titel;
                    command1.Parameters.Add(new SqlParameter("@ReeksId", SqlDbType.Int)).Value = stripDb.Reeks == null ? DBNull.Value : (object)strip.Reeks.Id;
                    command1.Parameters.Add(new SqlParameter("@ReeksNummer", SqlDbType.Int)).Value = stripDb.ReeksNummer == null ? DBNull.Value : (object)strip.ReeksNummer;
                    command1.Parameters.Add(new SqlParameter("@UitgeverijId", SqlDbType.Int)).Value = stripDb.Uitgeverij.Id;
                    command1.CommandText = queryI;
                    int stripId = (int)command1.ExecuteScalar();
                    // auteurs toevoegen
                    command2.Parameters.Add(new SqlParameter("@StripId", SqlDbType.Int));
                    command2.Parameters.Add(new SqlParameter("@AuteurId", SqlDbType.Int));
                    command2.CommandText = queryII;
                    command2.Parameters["@StripId"].Value = stripId;
                    foreach (var auteur in stripDb.Auteurs)
                    {
                        command2.Parameters["@AuteurId"].Value = auteur.Id;
                        command2.ExecuteNonQuery();
                    }
                    // Aantal toevoegen
                    command3.CommandText = queryIII;
                    command3.Parameters.Add(new SqlParameter("@StripId", SqlDbType.Int)).Value = stripId;
                    command3.Parameters.Add(new SqlParameter("@Aantal", SqlDbType.Int)).Value = stripDb.Aantal;
                    command3.ExecuteNonQuery();

                    transaction.Commit();
                    return stripId;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        /// <summary>
        /// Returnt een IEnumerable van alle strips in de databank.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Strip> GeefAlleStrips()
        {
            // ik heb dit aangepast
            Dictionary<int, StripDb> strips = new Dictionary<int, StripDb>();
            Dictionary<int, AuteurDb> auteurs = new Dictionary<int, AuteurDb>();
            Dictionary<int, UitgeverijDb> uitgeverijen = new Dictionary<int, UitgeverijDb>();
            Dictionary<int, ReeksDb> reeksen = new Dictionary<int, ReeksDb>
            {
                { 0, null }
            };
            SqlConnection connection = GetConnection();
            string query = "SELECT s.Id as StripId, s.Titel, s.ReeksId, r.Naam as ReeksNaam, s.ReeksNummer, u.Id as UitgeverijId, u.Naam as UitgeverijNaam," +
                "x.AuteurId as AuteurId, a.Naam as AuteurNaam, sa.Aantal FROM Strip s JOIN StripAantal sa ON sa.StripId = s.Id" +
                " JOIN Uitgeverij u ON u.Id = s.UitgeverijId " +
                "JOIN StripAuteur x ON x.StripId = s.Id JOIN Auteur a ON a.Id = x.AuteurId LEFT JOIN Reeks r ON r.Id = s.ReeksId";
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = query;
                connection.Open();
                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        ReeksDb reeks = null;
                        int? reeksId = reader["ReeksId"] == DBNull.Value ? 0 : (int?)reader["ReeksId"];
                        if (reeksId != 0)
                            reeks = new ReeksDb { Id = (int)reader["ReeksId"], Naam = (string)reader["ReeksNaam"] };
                        
                        if (!reeksen.ContainsKey((int)reeksId)) // checken of de reeks al bestaat
                            reeksen.Add((int)reeksId, reeks);

                        int auteurId = (int)reader["AuteurId"];
                        if (!auteurs.ContainsKey(auteurId)) // checken of de auteur al bestaat
                            auteurs.Add(auteurId, new AuteurDb { Id = auteurId, Naam = (string)reader["AuteurNaam"] });

                        int uitgeverijId = (int)reader["UitgeverijId"];
                        if (!uitgeverijen.ContainsKey(uitgeverijId)) // checken of de uitgeverij al bestaat
                            uitgeverijen.Add(uitgeverijId, new UitgeverijDb { Id = uitgeverijId, Naam = (string)reader["UitgeverijNaam"] });

                        int stripId = (int)reader["StripId"];
                        int? reeksNr = reader["ReeksNummer"] == DBNull.Value ? null : (int?)reader["ReeksNummer"];
                        if (!strips.ContainsKey(stripId)) // kijken of de strip al bestaat. Nodig voor meerdere auteurs.
                        {
                            StripDb strip = new StripDb
                            {
                                Id = stripId,
                                Titel = (string)reader["Titel"],
                                Reeks = reeksen[(int)reeksId],
                                ReeksNummer = reeksNr,
                                Auteurs = new List<AuteurDb>() { auteurs[auteurId] },
                                Uitgeverij = uitgeverijen[uitgeverijId],
                                Aantal = (int)reader["Aantal"]
                            };
                            strips.Add(stripId, strip);
                        }
                        else
                            strips[stripId].Auteurs.Add(auteurs[auteurId]);
                    }
                    reader.Close();
                    return strips.Values.Select(strip => Mapper.DbToDomain(strip));
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
        /// <summary>
        /// Returnt een strip aan de hand van de meegegeven Id. Returnt null indien de Strip niet bestaat.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Strip GeefStripViaId(int Id)
        {
            SqlConnection connection = GetConnection();
            string query = "SELECT s.Id as StripId, s.Titel, s.ReeksId, r.Naam as ReeksNaam, s.ReeksNummer, u.Id as UitgeverijId, u.Naam as UitgeverijNaam, " +
                "x.AuteurId as AuteurId, a.Naam as AuteurNaam , sa.Aantal FROM Strip s JOIN StripAantal sa ON sa.StripId = s.Id" +
                " JOIN Uitgeverij u ON u.Id = s.UitgeverijId " +
                "JOIN StripAuteur x ON x.StripId = s.Id JOIN Auteur a ON a.Id = x.AuteurId LEFT JOIN Reeks r ON r.Id = s.ReeksId WHERE s.Id=@StripId";
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = query;
                command.Parameters.Add(new SqlParameter("@StripId", SqlDbType.Int)).Value = Id;
                connection.Open();
                try
                {
                    int stripId = 0;
                    // deze entry wordt toegevoegd om een null waarde van strip te kunnen teruggeven
                    Dictionary<int, StripDb> strips = new Dictionary<int, StripDb> { { 0, null } };
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int? reeksId = reader["ReeksId"] == DBNull.Value ? 0 : (int?)reader["ReeksId"];
                        ReeksDb reeks = null;
                        if (reeksId != 0)
                            reeks = new ReeksDb { Id = (int)reeksId, Naam = (string)reader["ReeksNaam"] };

                        stripId = (int)reader["StripId"];
                        int aantal = (int)reader["Aantal"];
                        if (!strips.ContainsKey(stripId)) // een volledig nieuwe strip toevoegen aan de dict.
                        {
                            StripDb stripDb = new StripDb
                            {
                                Id = stripId,
                                Aantal = (int)reader["Aantal"],
                                Auteurs = new List<AuteurDb> { new AuteurDb { Id = (int)reader["AuteurId"], Naam = (string)reader["AuteurNaam"] } },
                                Reeks = reeks,
                                ReeksNummer = reader["ReeksNummer"] == DBNull.Value ? null : (int?)reader["ReeksNummer"],
                                Titel = (string)reader["Titel"],
                                Uitgeverij = new UitgeverijDb { Id = (int)reader["UitgeverijId"], Naam = (string)reader["UitgeverijNaam"] }
                            };

                            strips.Add(stripId, stripDb);
                        }
                        else // Als een strip meerdere auteurs heeft worden die hier toegevoegd.
                            strips[stripId].Auteurs.Add(new AuteurDb { Id = (int)reader["AuteurId"], Naam = (string)reader["AuteurNaam"] });
                    }
                    reader.Close();
                    return Mapper.DbToDomain(strips[stripId]);
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
        /// <summary>
        /// Update de meegegeven strip aan de hand van de Id.
        /// </summary>
        /// <param name="strip"></param>
        public void UpdateStrip(Strip strip)
        {
            SqlConnection connection = GetConnection();
            string query1 = "UPDATE Strip SET Titel=@Titel, ReeksId=@ReeksId, ReeksNummer=@ReeksNummer, UitgeverijId=@UitgeverijId WHERE Id=@Id";
            string query2 = "DELETE FROM StripAuteur WHERE StripId=@StripId";
            string query3 = "INSERT INTO StripAuteur (StripId, AuteurId) VALUES (@StripId,@AuteurId)";

            StripDb stripDb = Mapper.DomainToDb(strip);

            using (SqlCommand command1 = connection.CreateCommand())
            using (SqlCommand command2 = connection.CreateCommand())
            using (SqlCommand command3 = connection.CreateCommand())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                command1.Transaction = transaction;
                command2.Transaction = transaction;
                command3.Transaction = transaction;
                try
                {
                    //strip updaten
                    command1.Parameters.Add(new SqlParameter("@Titel", SqlDbType.NVarChar)).Value = stripDb.Titel;
                    command1.Parameters.Add(new SqlParameter("@ReeksId", SqlDbType.Int)).Value = stripDb.Reeks == null ? DBNull.Value : (object)stripDb.Reeks.Id;
                    command1.Parameters.Add(new SqlParameter("@ReeksNummer", SqlDbType.Int)).Value = stripDb.ReeksNummer == null ? DBNull.Value : (object)stripDb.ReeksNummer;
                    command1.Parameters.Add(new SqlParameter("@UitgeverijId", SqlDbType.Int)).Value = stripDb.Uitgeverij.Id;
                    command1.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)).Value = stripDb.Id;
                    command1.CommandText = query1;
                    command1.ExecuteNonQuery();

                    // link tussen auteurs en strip verwijderen
                    command2.Parameters.Add(new SqlParameter("@StripId", SqlDbType.Int)).Value = stripDb.Id;
                    command2.CommandText = query2;
                    command2.ExecuteNonQuery();

                    // link tussen auteurs en strip Toevoegen
                    command3.Parameters.Add(new SqlParameter("@StripId", SqlDbType.Int)).Value = stripDb.Id;
                    command3.Parameters.Add(new SqlParameter("@AuteurId", SqlDbType.Int));
                    command3.CommandText = query3;
                    foreach (var auteur in stripDb.Auteurs)
                    {
                        command3.Parameters["@AuteurId"].Value = auteur.Id;
                        command3.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
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

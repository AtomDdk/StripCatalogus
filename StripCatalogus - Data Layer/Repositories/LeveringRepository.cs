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
    public class LeveringRepository : ILeveringRepository
    {
        private string _connectionString;
        public LeveringRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
        public void VoegLeveringToe(Levering levering)
        {
            SqlConnection connection = GetConnection();
            string queryI = "INSERT INTO Levering (BestelDatum, LeverDatum) output INSERTED.ID VALUES (@BestelDatum, @LeverDatum);";
            string queryII = "INSERT INTO StripLevering (StripId, LeveringId, Aantal) VALUES (@StripId,@LeveringId,@Aantal);";
            string queryIII = "UPDATE StripAantal SET Aantal=@Aantal WHERE StripId=@StripId";
            LeveringDb leveringDb = Mapper.DomainToDb(levering);
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
                    //bestelling toevoegen
                    command1.CommandText = queryI;
                    command1.Parameters.Add(new SqlParameter("@BestelDatum", SqlDbType.DateTime)).Value = leveringDb.BestelDatum;
                    command1.Parameters.Add(new SqlParameter("@LeverDatum", SqlDbType.DateTime)).Value = leveringDb.LeverDatum;
                    int leveringId = (int)command1.ExecuteScalar();

                    // bestellingLink toevoegen
                    command2.CommandText = queryII;
                    command2.Parameters.Add(new SqlParameter("@LeveringId", SqlDbType.Int)).Value = leveringId;
                    command2.Parameters.Add(new SqlParameter("@StripId", SqlDbType.Int));
                    command2.Parameters.Add(new SqlParameter("@Aantal", SqlDbType.Int));
                    foreach (var keyValuePair in leveringDb.StripsEnAantal)
                    {
                        command2.Parameters["@StripId"].Value = keyValuePair.Key.Id;
                        command2.Parameters["@Aantal"].Value = keyValuePair.Value;
                        command2.ExecuteNonQuery();
                    }

                    // Aantallen aanpassen
                    command3.CommandText = queryIII;
                    command3.Parameters.Add(new SqlParameter("@StripId", SqlDbType.Int));
                    command3.Parameters.Add(new SqlParameter("@Aantal", SqlDbType.Int));
                    foreach (var keyValuePair in leveringDb.StripsEnAantal)
                    {
                        command3.Parameters["@StripId"].Value = keyValuePair.Key.Id;
                        command3.Parameters["@Aantal"].Value = keyValuePair.Key.Aantal;
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

        public List<Levering> GeefAlleLeveringen()
        {
            SqlConnection connection = GetConnection();
            string query = "SELECT s.Id as StripId, s.Titel, s.ReeksId, r.Naam as ReeksNaam, s.ReeksNummer, u.Id as UitgeverijId, " +
                "u.Naam as UitgeverijNaam, x.AuteurId as AuteurId, a.Naam as AuteurNaam, sl.Aantal as LeverAantal, l.BestelDatum, l.LeverDatum, " +
                "l.Id as LeveringId, sa.Aantal as StripAantal FROM Strip s JOIN Uitgeverij u ON u.Id = s.UitgeverijId JOIN StripAuteur x ON x.StripId = s.Id" +
                " JOIN Auteur a ON a.Id = x.AuteurId LEFT JOIN Reeks r ON r.Id = s.ReeksId JOIN StripLevering sl on s.Id = sl.StripId" +
                " JOIN Levering l ON l.Id = sl.LeveringId JOIN StripAantal sa ON sa.StripId = s.Id";
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = query;
                connection.Open();
                try
                {
                    Dictionary<int, LeveringDb> leveringen = new Dictionary<int, LeveringDb>();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int? reeksId = reader["ReeksId"] == DBNull.Value ? 0 : (int?)reader["ReeksId"];
                        ReeksDb reeks = null;
                        if (reeksId != 0)
                            reeks = new ReeksDb { Id = (int)reeksId, Naam = (string)reader["ReeksNaam"] };

                        int stripId = (int)reader["StripId"];
                        StripDb stripDb = new StripDb
                        {
                            Id = stripId,
                            Aantal = (int)reader["StripAantal"],
                            Auteurs = new List<AuteurDb> { new AuteurDb { Id = (int)reader["AuteurId"], Naam = (string)reader["AuteurNaam"] } },
                            Reeks = reeks,
                            ReeksNummer = reader["ReeksNummer"] == DBNull.Value ? null : (int?)reader["ReeksNummer"],
                            Titel = (string)reader["Titel"],
                            Uitgeverij = new UitgeverijDb { Id = (int)reader["UitgeverijId"], Naam = (string)reader["UitgeverijNaam"] }
                        };

                        int leveringId = (int)reader["LeveringId"];
                        if (!leveringen.ContainsKey(leveringId))
                        {
                            leveringen.Add(leveringId, new LeveringDb
                            {
                                BestelDatum = (DateTime)reader["BestelDatum"],
                                LeverDatum = (DateTime)reader["LeverDatum"],
                                Id = leveringId,
                                StripsEnAantal = new Dictionary<StripDb, int>
                                {
                                    { stripDb, (int)reader["LeverAantal"] }
                                }
                            });
                        }
                        else if (!leveringen[leveringId].StripsEnAantal.Any(x => x.Key.Id == stripId))
                            leveringen[leveringId].StripsEnAantal.Add(stripDb, (int)reader["LeverAantal"]);
                        else
                            leveringen[leveringId].StripsEnAantal.Where(x => x.Key.Id == stripId).Single().Key
                                .Auteurs.Add(new AuteurDb { Id = (int)reader["AuteurId"], Naam = (string)reader["AuteurNaam"] });
                    }
                    reader.Close();

                    return leveringen.Values.Select(x => Mapper.DbToDomain(x)).ToList();
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

        public Levering GeefLeveringVoorId(int id)
        {
            SqlConnection connection = GetConnection();
            string query = "SELECT s.Id as StripId, s.Titel, s.ReeksId, r.Naam as ReeksNaam, s.ReeksNummer, u.Id as UitgeverijId, " +
                "u.Naam as UitgeverijNaam, x.AuteurId as AuteurId, a.Naam as AuteurNaam, sl.Aantal as LeverAantal, l.BestelDatum, l.LeverDatum, " +
                "l.Id as LeveringId, sa.Aantal as StripAantal FROM Strip s JOIN Uitgeverij u ON u.Id = s.UitgeverijId JOIN StripAuteur x ON x.StripId = s.Id" +
                " JOIN Auteur a ON a.Id = x.AuteurId LEFT JOIN Reeks r ON r.Id = s.ReeksId JOIN StripLevering sl on s.Id = sl.StripId" +
                " JOIN Levering l ON l.Id = sl.LeveringId JOIN StripAantal sa ON sa.StripId = s.Id WHERE l.id=@LeveringId";
            LeveringDb leveringDb = null;
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = query;
                command.Parameters.Add(new SqlParameter("@LeveringId", SqlDbType.Int)).Value = id;
                connection.Open();
                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (leveringDb == null)
                        {
                            leveringDb = new LeveringDb
                            {
                                BestelDatum = (DateTime)reader["BestelDatum"],
                                LeverDatum = (DateTime)reader["LeverDatum"],
                                Id = (int)reader["LeveringId"]
                            };
                        }

                        int? reeksId = reader["ReeksId"] == DBNull.Value ? 0 : (int?)reader["ReeksId"];
                        ReeksDb reeks = null;
                        if (reeksId != 0)
                            reeks = new ReeksDb { Id = (int)reeksId, Naam = (string)reader["ReeksNaam"] };

                        int stripId = (int)reader["StripId"];
                        StripDb stripDb = new StripDb
                        {
                            Id = stripId,
                            Aantal = (int)reader["StripAantal"],
                            Auteurs = new List<AuteurDb> { new AuteurDb { Id = (int)reader["AuteurId"], Naam = (string)reader["AuteurNaam"] } },
                            Reeks = reeks,
                            ReeksNummer = reader["ReeksNummer"] == DBNull.Value ? null : (int?)reader["ReeksNummer"],
                            Titel = (string)reader["Titel"],
                            Uitgeverij = new UitgeverijDb { Id = (int)reader["UitgeverijId"], Naam = (string)reader["UitgeverijNaam"] }
                        };

                        if (!leveringDb.StripsEnAantal.Any(x => x.Key.Id == stripId))
                            leveringDb.StripsEnAantal.Add(stripDb, (int)reader["LeverAantal"]);
                        else
                            leveringDb.StripsEnAantal.Where(x => x.Key.Id == stripId).Single().Key
                                .Auteurs.Add(new AuteurDb { Id = (int)reader["AuteurId"], Naam = (string)reader["AuteurNaam"] });
                    }
                    reader.Close();

                    return Mapper.DbToDomain(leveringDb);
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



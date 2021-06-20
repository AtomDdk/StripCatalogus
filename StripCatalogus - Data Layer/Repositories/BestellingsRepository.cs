using Domeinlaag.Interfaces;
using Domeinlaag.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using StripCatalogus___Data_Layer.Models;

namespace StripCatalogus___Data_Layer.Repositories
{
    public class BestellingsRepository : IBestellingRepository
    {
        private string _connectionString;
        public BestellingsRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public List<Bestelling> GeefAlleBestellingen()
        {
            SqlConnection connection = GetConnection();
            string query = "SELECT s.Id as StripId, s.Titel, s.ReeksId, r.Naam as ReeksNaam, s.ReeksNummer, u.Id as UitgeverijId, " +
                "u.Naam as UitgeverijNaam, x.AuteurId as AuteurId, a.Naam as AuteurNaam, sb.Aantal as BestellingAantal, b.Id as BestellingId, b.Datum," +
                " sa.Aantal as StripAantal FROM Strip s JOIN Uitgeverij u ON u.Id = s.UitgeverijId JOIN StripAuteur x ON x.StripId = s.Id" +
                " JOIN Auteur a ON a.Id = x.AuteurId LEFT JOIN Reeks r ON r.Id = s.ReeksId JOIN StripBestelling sb on s.Id = sb.StripId" +
                " JOIN Bestelling b ON b.Id = sb.BestellingId JOIN StripAantal sa ON s.Id=sa.StripId";
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = query;
                connection.Open();
                try
                {
                    Dictionary<int, BestellingDb> bestellingen = new Dictionary<int, BestellingDb>();
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

                        int bestellingId = (int)reader["BestellingId"];
                        if (!bestellingen.ContainsKey(bestellingId))
                        {
                            bestellingen.Add(bestellingId, new BestellingDb
                            {
                                Datum = (DateTime)reader["Datum"],
                                Id = bestellingId,
                                StripsEnAantal = new Dictionary<StripDb, int>
                                {
                                    { stripDb, (int)reader["BestellingAantal"] }
                                }
                            });
                        }
                        else if (!bestellingen[bestellingId].StripsEnAantal.Any(x => x.Key.Id == stripId))
                            bestellingen[bestellingId].StripsEnAantal.Add(stripDb, (int)reader["BestellingAantal"]);
                        else
                            bestellingen[bestellingId].StripsEnAantal.Where(x => x.Key.Id == stripId).Single().Key
                                .Auteurs.Add(new AuteurDb { Id = (int)reader["AuteurId"], Naam = (string)reader["AuteurNaam"] });
                    }
                    reader.Close();

                    return bestellingen.Values.Select(x => Mapper.DbToDomain(x)).ToList();
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

        public Bestelling GeefBestellingVoorId(int id)
        {
            SqlConnection connection = GetConnection();
            string query = "SELECT s.Id as StripId, s.Titel, s.ReeksId, r.Naam as ReeksNaam, s.ReeksNummer, u.Id as UitgeverijId, " +
                "u.Naam as UitgeverijNaam, x.AuteurId as AuteurId, a.Naam as AuteurNaam, sb.Aantal as BestellingAantal, b.Datum, b.Id as BestellingId," +
                " sa.Aantal as StripAantal FROM Strip s JOIN Uitgeverij u ON u.Id = s.UitgeverijId JOIN StripAuteur x ON x.StripId = s.Id" +
                " JOIN Auteur a ON a.Id = x.AuteurId LEFT JOIN Reeks r ON r.Id = s.ReeksId JOIN StripBestelling sb on s.Id = sb.StripId" +
                " JOIN Bestelling b ON b.Id = sb.BestellingId JOIN StripAantal sa ON s.Id=sa.StripId WHERE b.id=@bestellingId";
            using (SqlCommand command = connection.CreateCommand())
            {
                BestellingDb bestellingDb = null;
                command.CommandText = query;
                command.Parameters.Add(new SqlParameter("@bestellingId", SqlDbType.Int)).Value = id;
                connection.Open();
                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (bestellingDb == null)
                        {
                            bestellingDb = new BestellingDb
                            {
                                Datum = (DateTime)reader["Datum"],
                                Id = (int)reader["BestellingId"],
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

                        if (!bestellingDb.StripsEnAantal.Any(x => x.Key.Id == stripId))
                            bestellingDb.StripsEnAantal.Add(stripDb, (int)reader["BestellingAantal"]);
                        else
                            bestellingDb.StripsEnAantal.Where(x => x.Key.Id == stripId).Single().Key
                                .Auteurs.Add(new AuteurDb { Id = (int)reader["AuteurId"], Naam = (string)reader["AuteurNaam"] });
                    }
                    reader.Close();

                    return Mapper.DbToDomain(bestellingDb);
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

        public void VoegBestellingToe(Bestelling bestelling)
        {
            SqlConnection connection = GetConnection();
            string queryI = "INSERT INTO Bestelling (Datum) output INSERTED.ID VALUES (@Datum);";
            string queryII = "INSERT INTO StripBestelling (StripId, BestellingId, Aantal) VALUES (@StripId,@BestellingId,@Aantal);";
            string queryIII = "UPDATE StripAantal SET Aantal=@Aantal WHERE StripID=@StripId";
            BestellingDb bestellingDb = Mapper.DomainToDb(bestelling);
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
                    command1.Parameters.Add(new SqlParameter("@Datum", SqlDbType.DateTime)).Value = bestellingDb.Datum;
                    int bestellingId = (int)command1.ExecuteScalar();

                    // bestellingLink toevoegen
                    command2.CommandText = queryII;
                    command2.Parameters.Add(new SqlParameter("@BestellingId", SqlDbType.Int)).Value = bestellingId;
                    command2.Parameters.Add(new SqlParameter("@StripId", SqlDbType.Int));
                    command2.Parameters.Add(new SqlParameter("@Aantal", SqlDbType.Int));
                    foreach (var keyValuePair in bestellingDb.StripsEnAantal)
                    {
                        command2.Parameters["@StripId"].Value = keyValuePair.Key.Id;
                        command2.Parameters["@Aantal"].Value = keyValuePair.Value;
                        command2.ExecuteNonQuery();
                    }

                    // Aantal aanpassen
                    command3.CommandText = queryIII;
                    command3.Parameters.Add(new SqlParameter("@StripId", SqlDbType.Int));
                    command3.Parameters.Add(new SqlParameter("@Aantal", SqlDbType.Int));
                    foreach (var keyValuePair in bestellingDb.StripsEnAantal)
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
    }
}

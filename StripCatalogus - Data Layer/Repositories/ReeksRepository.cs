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
    public class ReeksRepository : IReeksRepository
    {
        // fuck git
        private string _connectionString;
        public ReeksRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
        /// <summary>
        /// Geeft een IEnumerable van de nummers van de meegegeven Reeks. Zoekt aan de hand van de Id.
        /// </summary>
        /// <param name="reeks"></param>
        /// <returns></returns>
        public IEnumerable<int> GeefAlleNummersVanReeks(Reeks reeks)
        {
            IList<int> reeksNummers = new List<int>();
            SqlConnection connection = GetConnection();
            string query = "SELECT ReeksNummer FROM Strip WHERE ReeksId=@ReeksId";
            ReeksDb reeksDb = Mapper.DomainToDb(reeks);
            using (SqlCommand command = connection.CreateCommand())
            {
                command.Parameters.Add(new SqlParameter("@ReeksId", SqlDbType.NVarChar)).Value = reeksDb.Id;
                command.CommandText = query;
                connection.Open();
                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                        reeksNummers.Add((int)reader["ReeksNummer"]);
                    reader.Close();
                    return reeksNummers;
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
        /// Geeft een IEnumerable van all reeksen.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Reeks> GeefAlleReeksen()
        {
            IList<ReeksDb> reeksen = new List<ReeksDb>();
            SqlConnection connection = GetConnection();
            string query = "SELECT * FROM Reeks";
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = query;
                connection.Open();
                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                        reeksen.Add(new ReeksDb { Id = (int)reader["Id"], Naam = (string)reader["Naam"] });
                    reader.Close();
                    return reeksen.Select(x => Mapper.DbToDomain(x));
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
        /// Update de meegegeven reeks aan de hand van de id.
        /// </summary>
        /// <param name="reeks"></param>
        public void UpdateReeks(Reeks reeks)
        {
            SqlConnection connection = GetConnection();
            string query = "UPDATE Reeks SET Naam=@Naam WHERE Id=@Id";
            ReeksDb reeksDb = Mapper.DomainToDb(reeks);
            using (SqlCommand command1 = connection.CreateCommand())
            {
                connection.Open();
                try
                {
                    command1.Parameters.Add(new SqlParameter("@Naam", SqlDbType.NVarChar)).Value = reeksDb.Naam;
                    command1.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)).Value = reeksDb.Id;
                    command1.CommandText = query;
                    if (command1.ExecuteNonQuery() <= 0)
                        throw new DataLayerException("Er is niets aangepast in de database. Hoogstwaarschijnlijk bestaat de " +
                            "reeks niet");
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
        /// Voegt een Reeks toe aan de databank en returnt de Id.
        /// </summary>
        /// <param name="reeks"></param>
        /// <returns></returns>
        public int VoegReeksToe(Reeks reeks)
        {
            SqlConnection connection = GetConnection();
            string query = "INSERT INTO Reeks (Naam) output INSERTED.ID VALUES(@Naam)";
            ReeksDb reeksDb = Mapper.DomainToDb(reeks);
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                try
                {
                    command.Parameters.Add(new SqlParameter("@Naam", SqlDbType.NVarChar)).Value = reeksDb.Naam;
                    command.CommandText = query;
                    return (int)command.ExecuteScalar();
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
        /// Geeft een Reeks aan de hand van een Id. Returnt null indien de Reeks niet bestaat.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Reeks GeefReeksOpId(int id)
        {
            string query = "SELECT* FROM Reeks WHERE Id=@Id";
            SqlConnection connection = GetConnection();
            using (SqlCommand command = connection.CreateCommand())
            {
                command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)).Value = id;
                command.CommandText = query;
                connection.Open();
                try
                {
                    ReeksDb reeks = null;
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                        reeks = new ReeksDb { Id = (int)reader["Id"], Naam = (string)reader["Naam"] };
                    reader.Close();
                    return Mapper.DbToDomain(reeks);
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
        /// Geeft een Reeks op de exacte naam. Returnt null indien de Reeks niet bestaat.
        /// </summary>
        /// <param name="naam"></param>
        /// <returns></returns>
        public Reeks GeefReeksViaNaam(string naam)
        {
            string query = "SELECT * FROM Reeks WHERE Naam=@Naam";
            SqlConnection connection = GetConnection();
            using (SqlCommand command = connection.CreateCommand())
            {
                command.Parameters.Add(new SqlParameter("@Naam", SqlDbType.NVarChar)).Value = naam;
                command.CommandText = query;
                connection.Open();
                try
                {
                    ReeksDb reeks = null;
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                        reeks = new ReeksDb { Id = (int)reader["Id"], Naam = (string)reader["Naam"] };
                    reader.Close();
                    return Mapper.DbToDomain(reeks);
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

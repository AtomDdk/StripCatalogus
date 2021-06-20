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
    public class UitgeverijRepository : IUitgeverijRepository
    {
        // fuck git
        private string _connectionString;
        public UitgeverijRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
        /// <summary>
        /// Returnt een Uitgeverij aan de hand van de meegegeven Id. Returnt null indien de Uigeverij niet bestaat.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Uitgeverij GeefUitgeverijViaId(int id)
        {
            string query = "SELECT * FROM Uitgeverij WHERE Id=@Id";
            SqlConnection connection = GetConnection();
            using (SqlCommand command = connection.CreateCommand())
            {
                command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)).Value = id;
                command.CommandText = query;
                connection.Open();
                try
                {
                    UitgeverijDb uitgeverij = null;
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                        uitgeverij = new UitgeverijDb { Id = (int)reader["Id"], Naam = (string)reader["Naam"] };
                    reader.Close();
                    return Mapper.DbToDomain(uitgeverij);
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
        /// Returnt een IEnumerable van alle uitgeverijen.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Uitgeverij> GeefAlleUitgeverijen()
        {
            IList<UitgeverijDb> Uitgeverijen = new List<UitgeverijDb>();
            SqlConnection connection = GetConnection();
            string query = "SELECT * FROM Uitgeverij";
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = query;
                connection.Open();
                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                        Uitgeverijen.Add(new UitgeverijDb { Id = (int)reader["Id"], Naam = (string)reader["Naam"] });
                    reader.Close();
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
            return Uitgeverijen.Select(x => Mapper.DbToDomain(x));
        }
        /// <summary>
        /// Voegt een Uitgeverij toe aan de database en returnt de Id.
        /// </summary>
        /// <param name="uitgeverij"></param>
        /// <returns></returns>
        public int VoegUitgeverijToe(Uitgeverij uitgeverij)
        {
            string query = "INSERT INTO Uitgeverij (naam) output INSERTED.ID VALUES(@Naam)";
            SqlConnection connection = GetConnection();
            UitgeverijDb uitgeverijDb = Mapper.DomainToDb(uitgeverij);
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                try
                {
                    command.Parameters.Add(new SqlParameter("@Naam", SqlDbType.NVarChar)).Value = uitgeverijDb.Naam;
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
        /// Returnt een Uitgeverij aan de hand van de exacte naam. Returnt null indien de Uitgeverij niet bestaat.
        /// </summary>
        /// <param name="naam"></param>
        /// <returns></returns>
        public Uitgeverij GeefUitgeverijViaNaam(string naam)
        {
            string query = "SELECT * FROM Uitgeverij WHERE Naam=@Naam";
            SqlConnection connection = GetConnection();
            using (SqlCommand command = connection.CreateCommand())
            {
                command.Parameters.Add(new SqlParameter("@Naam", SqlDbType.NVarChar)).Value = naam;
                command.CommandText = query;
                connection.Open();
                try
                {
                    UitgeverijDb uitgeverij = null;
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                        uitgeverij = new UitgeverijDb { Id = (int)reader["Id"], Naam = (string)reader["Naam"] };
                    reader.Close();
                    return Mapper.DbToDomain(uitgeverij);
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
        /// Update de meegegeven uitgeverij aan de hand van zijn id.
        /// </summary>
        /// <param name="uitgeverij"></param>
        public void UpdateUitgeverij(Uitgeverij uitgeverij)
        {
            SqlConnection connection = GetConnection();
            string queryI = "UPDATE Uitgeverij SET Naam=@Naam WHERE Id=@Id";
            UitgeverijDb uitgeverijDb = Mapper.DomainToDb(uitgeverij);
            using (SqlCommand command1 = connection.CreateCommand())
            {
                connection.Open();
                try
                {
                    command1.Parameters.Add(new SqlParameter("@Naam", SqlDbType.NVarChar)).Value = uitgeverijDb.Naam;
                    command1.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)).Value = uitgeverijDb.Id;
                    command1.CommandText = queryI;
                    if (command1.ExecuteNonQuery() <= 0)
                        throw new DataLayerException("Er is niets aangepast in de databank. Waarschijnlijk" +
                            "bestaat de uitgeverij niet in de databank.");
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

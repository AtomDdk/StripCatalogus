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
    public class AuteurRepository : IAuteurRepository
    {
        // fuck git
        private string _connectionString;
        public AuteurRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
        /// <summary>
        /// Geeft een auteur van de meegegeven Id. geeft null indien de Id niet bestaat
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Auteur</returns>
        public Auteur GeefAuteurViaId(int id)
        {
            string query = "SELECT * FROM Auteur WHERE Id=@Id";
            SqlConnection connection = GetConnection();
            using (SqlCommand command = connection.CreateCommand())
            {
                command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)).Value = id;
                command.CommandText = query;
                connection.Open();
                try
                {
                    AuteurDb auteur = null;
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read()) 
                        auteur = new AuteurDb { Id = (int)reader["Id"], Naam = (string)reader["Naam"] };
                    reader.Close();
                    return Mapper.DbToDomain(auteur);
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
        /// Geeft een IEnumerable van alle auteurs uit de databank.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Auteur> GeefAlleAuteurs()
        {
            IList<AuteurDb> auteurs = new List<AuteurDb>();
            SqlConnection connection = GetConnection();
            string query = "SELECT * FROM Auteur";
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = query;
                connection.Open();
                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                        auteurs.Add(new AuteurDb { Id = (int)reader["Id"], Naam = (string)reader["Naam"] });
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
            return auteurs.Select(x => Mapper.DbToDomain(x));
        }
        /// <summary>
        /// Voegt een auteur toe in de databank en returnt de Id.
        /// </summary>
        /// <param name="auteur"></param>
        /// <returns></returns>
        public int VoegAuteurToe(Auteur auteur)
        {
            SqlConnection connection = GetConnection();
            string query = "INSERT INTO Auteur (naam) output INSERTED.ID VALUES(@Naam)";
            AuteurDb auteurDb = Mapper.DomainToDb(auteur);
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                try
                {
                    command.Parameters.Add(new SqlParameter("@Naam", SqlDbType.NVarChar)).Value = auteurDb.Naam;
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
        /// Geeft een auteur terug aan de hand van de exacte naam. Returnt null indien de Auteur niet bestaat.
        /// </summary>
        /// <param name="naam"></param>
        /// <returns></returns>
        public Auteur GeefAuteurViaNaam(string naam)
        {
            string query = "SELECT * FROM Auteur WHERE Naam=@Naam";
            SqlConnection connection = GetConnection();
            using (SqlCommand command = connection.CreateCommand())
            {
                command.Parameters.Add(new SqlParameter("@Naam", SqlDbType.NVarChar)).Value = naam;
                command.CommandText = query;
                connection.Open();
                try
                {
                    AuteurDb auteur = null;
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                        auteur = new AuteurDb { Id = (int)reader["Id"], Naam = (string)reader["Naam"] };
                    reader.Close();
                    return Mapper.DbToDomain(auteur);
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
        /// Update de meegegeven auteur aan de hand van zijn id.
        /// </summary>
        /// <param name="auteur"></param>
        public void UpdateAuteur(Auteur auteur)
        {
            SqlConnection connection = GetConnection();
            string queryI = "UPDATE Auteur SET Naam=@Naam WHERE Id=@Id";
            AuteurDb auteurDb = Mapper.DomainToDb(auteur);
            using (SqlCommand command1 = connection.CreateCommand())
            {
                connection.Open();
                try
                {
                    command1.Parameters.Add(new SqlParameter("@Naam", SqlDbType.NVarChar)).Value = auteurDb.Naam;
                    command1.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)).Value = auteurDb.Id;
                    command1.CommandText = queryI;
                    if (command1.ExecuteNonQuery() <= 0)
                        throw new DataLayerException("Er is niets veranderd in de database. " +
                            "hoogstwaarschijnlijk bestaat de auteur niet.");
                }
                catch (DataLayerException ex)
                {
                    throw ex;
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

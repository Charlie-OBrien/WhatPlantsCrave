using Brawndo_Components;
using Brawndo_Components.Data;
using Brawndo_Components.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace WhatPlantsCrave.Infrastructure.Repositories.Sql
{
    /// <summary>
    /// ProductDescription repository implementation using SQL Server stored procedures.
    /// Executes actual database operations via ADO.NET.
    /// </summary>
    public class SqlProductDescriptionRepository : IProductDescriptionRepository
    {
        private readonly AdventureWorksContext _context;

        public SqlProductDescriptionRepository(AdventureWorksContext context)
        {
            _context = context;
        }

        public int Create(string description)
        {
            try
            {
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_ProductDescription_Create", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Description", description);
                var outputParam = new SqlParameter("@ProductDescriptionID", SqlDbType.Int) { Direction = ParameterDirection.Output };
                command.Parameters.Add(outputParam);
                command.ExecuteNonQuery();
                return (int)outputParam.Value;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public bool Delete(int productDescriptionId)
        {
            try
            {
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_ProductDescription_Delete", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProductDescriptionID", productDescriptionId);
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<ProductDescription> GetAll()
        {
            try
            {
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_ProductDescription_GetAll", connection);
                command.CommandType = CommandType.StoredProcedure;
                using var reader = command.ExecuteReader();
                return ReadDescriptions(reader);
            }
            catch (Exception)
            {
                return new List<ProductDescription>();
            }
        }

        public ProductDescription? GetByID(int productDescriptionId)
        {
            try
            {
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_ProductDescription_GetByID", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProductDescriptionID", productDescriptionId);
                using var reader = command.ExecuteReader();
                var descriptions = ReadDescriptions(reader);
                return descriptions.FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool Update(int productDescriptionId, string? description = null)
        {
            try
            {
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_ProductDescription_Update", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProductDescriptionID", productDescriptionId);
                command.Parameters.AddWithValue("@Description", (object?)description ?? DBNull.Value);
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static List<ProductDescription> ReadDescriptions(SqlDataReader reader)
        {
            var results = new List<ProductDescription>();
            while (reader.Read())
            {
                results.Add(new ProductDescription
                {
                    ProductDescriptionId = reader.GetInt32(reader.GetOrdinal("ProductDescriptionID")),
                    Description = reader.GetString(reader.GetOrdinal("Description")),
                    ModifiedDate = reader.GetDateTime(reader.GetOrdinal("ModifiedDate"))
                });
            }
            return results;
        }
    }
}

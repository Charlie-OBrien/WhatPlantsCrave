using Brawndo_Components;
using Brawndo_Components.Data;
using Brawndo_Components.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace WhatPlantsCrave.Infrastructure.Repositories
{
    /// <summary>
    /// ProductModelProductDescription repository implementation using SQL Server stored procedures.
    /// Executes actual database operations via ADO.NET.
    /// </summary>
    public class SqlProductModelProductDescriptionRepository : IProductModelProductDescriptionRepository
    {
        private readonly AdventureWorksContext _context;

        public SqlProductModelProductDescriptionRepository(AdventureWorksContext context)
        {
            _context = context;
        }

        public bool Create(int productModelId, int productDescriptionId, string culture)
        {
            try
            {
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_ProductModelProductDescription_Create", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProductModelID", productModelId);
                command.Parameters.AddWithValue("@ProductDescriptionID", productDescriptionId);
                command.Parameters.AddWithValue("@Culture", culture);
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Delete(int productModelId, int productDescriptionId, string culture)
        {
            try
            {
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_ProductModelProductDescription_Delete", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProductModelID", productModelId);
                command.Parameters.AddWithValue("@ProductDescriptionID", productDescriptionId);
                command.Parameters.AddWithValue("@Culture", culture);
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public ProductModelProductDescription? Get(int productModelId, int productDescriptionId, string culture)
        {
            try
            {
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_ProductModelProductDescription_Get", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProductModelID", productModelId);
                command.Parameters.AddWithValue("@ProductDescriptionID", productDescriptionId);
                command.Parameters.AddWithValue("@Culture", culture);
                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new ProductModelProductDescription
                    {
                        ProductModelId = reader.GetInt32(reader.GetOrdinal("ProductModelID")),
                        ProductDescriptionId = reader.GetInt32(reader.GetOrdinal("ProductDescriptionID")),
                        Culture = reader.GetString(reader.GetOrdinal("Culture")),
                        ModifiedDate = reader.GetDateTime(reader.GetOrdinal("ModifiedDate"))
                    };
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<ProductModelProductDescription> GetAll()
        {
            try
            {
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_ProductModelProductDescription_GetAll", connection);
                command.CommandType = CommandType.StoredProcedure;
                using var reader = command.ExecuteReader();
                return ReadDescriptions(reader);
            }
            catch (Exception)
            {
                return new List<ProductModelProductDescription>();
            }
        }

        public bool Update(int productModelId, int productDescriptionId, string cultureOld, string cultureNew)
        {
            try
            {
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_ProductModelProductDescription_Update", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProductModelID", productModelId);
                command.Parameters.AddWithValue("@ProductDescriptionID", productDescriptionId);
                command.Parameters.AddWithValue("@CultureOld", cultureOld);
                command.Parameters.AddWithValue("@CultureNew", cultureNew);
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static List<ProductModelProductDescription> ReadDescriptions(SqlDataReader reader)
        {
            var results = new List<ProductModelProductDescription>();
            while (reader.Read())
            {
                results.Add(new ProductModelProductDescription
                {
                    ProductModelId = reader.GetInt32(reader.GetOrdinal("ProductModelID")),
                    ProductDescriptionId = reader.GetInt32(reader.GetOrdinal("ProductDescriptionID")),
                    Culture = reader.GetString(reader.GetOrdinal("Culture")),
                    ModifiedDate = reader.GetDateTime(reader.GetOrdinal("ModifiedDate"))
                });
            }
            return results;
        }
    }
}

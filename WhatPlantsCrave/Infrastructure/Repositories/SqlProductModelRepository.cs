using Brawndo_Components;
using Brawndo_Components.Data;
using Brawndo_Components.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace WhatPlantsCrave.Infrastructure.Repositories
{
    /// <summary>
    /// ProductModel repository implementation using SQL Server stored procedures.
    /// Executes actual database operations via ADO.NET.
    /// </summary>
    public class SqlProductModelRepository : IProductModelRepository
    {
        private readonly AdventureWorksContext _context;

        public SqlProductModelRepository(AdventureWorksContext context)
        {
            _context = context;
        }

        public int Create(string name, string? catalogDescription = null)
        {
            try
            {
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_ProductModel_Create", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@CatalogDescription", (object?)catalogDescription ?? DBNull.Value);
                var outputParam = new SqlParameter("@ProductModelID", SqlDbType.Int) { Direction = ParameterDirection.Output };
                command.Parameters.Add(outputParam);
                command.ExecuteNonQuery();
                return (int)outputParam.Value;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public bool Delete(int productModelId)
        {
            try
            {
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_ProductModel_Delete", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProductModelID", productModelId);
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<ProductModel> GetAll()
        {
            try
            {
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_ProductModel_GetAll", connection);
                command.CommandType = CommandType.StoredProcedure;
                using var reader = command.ExecuteReader();
                return ReadModels(reader);
            }
            catch (Exception)
            {
                return new List<ProductModel>();
            }
        }

        public ProductModel? GetByID(int productModelId)
        {
            try
            {
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_ProductModel_GetByID", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProductModelID", productModelId);
                using var reader = command.ExecuteReader();
                var models = ReadModels(reader);
                return models.FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static List<ProductModel> ReadModels(SqlDataReader reader)
        {
            var results = new List<ProductModel>();
            while (reader.Read())
            {
                results.Add(new ProductModel
                {
                    ProductModelId = reader.GetInt32(reader.GetOrdinal("ProductModelID")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    CatalogDescription = reader.IsDBNull(reader.GetOrdinal("CatalogDescription")) ? null : reader.GetString(reader.GetOrdinal("CatalogDescription")),
                    ModifiedDate = reader.GetDateTime(reader.GetOrdinal("ModifiedDate"))
                });
            }
            return results;
        }
    }
}

using Brawndo_Components;
using Brawndo_Components.Data;
using Brawndo_Components.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace WhatPlantsCrave.Infrastructure.Repositories
{
    /// <summary>
    /// ProductCategory repository implementation using SQL Server stored procedures.
    /// Executes actual database operations via ADO.NET.
    /// </summary>
    public class SqlProductCategoryRepository : IProductCategoryRepository
    {
        private readonly AdventureWorksContext _context;

        public SqlProductCategoryRepository(AdventureWorksContext context)
        {
            _context = context;
        }

        public int Create(string name, int? parentProductCategoryId = null)
        {
            try
            {
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_ProductCategory_Create", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@ParentProductCategoryID", (object?)parentProductCategoryId ?? DBNull.Value);
                var outputParam = new SqlParameter("@ProductCategoryID", SqlDbType.Int) { Direction = ParameterDirection.Output };
                command.Parameters.Add(outputParam);
                command.ExecuteNonQuery();
                return (int)outputParam.Value;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public bool Delete(int productCategoryId)
        {
            try
            {
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_ProductCategory_Delete", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProductCategoryID", productCategoryId);
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<ProductCategory> GetAll()
        {
            try
            {
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_ProductCategory_GetAll", connection);
                command.CommandType = CommandType.StoredProcedure;
                using var reader = command.ExecuteReader();
                return ReadCategories(reader);
            }
            catch (Exception)
            {
                return new List<ProductCategory>();
            }
        }

        public ProductCategory? GetByID(int productCategoryId)
        {
            try
            {
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_ProductCategory_GetByID", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProductCategoryID", productCategoryId);
                using var reader = command.ExecuteReader();
                var categories = ReadCategories(reader);
                return categories.FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<ProductCategory> GetRootCategories()
        {
            try
            {
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_ProductCategory_GetRootCategories", connection);
                command.CommandType = CommandType.StoredProcedure;
                using var reader = command.ExecuteReader();
                return ReadCategories(reader);
            }
            catch (Exception)
            {
                return new List<ProductCategory>();
            }
        }

        public bool Update(int productCategoryId, string? name = null, int? parentProductCategoryId = null)
        {
            try
            {
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_ProductCategory_Update", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProductCategoryID", productCategoryId);
                command.Parameters.AddWithValue("@Name", (object?)name ?? DBNull.Value);
                command.Parameters.AddWithValue("@ParentProductCategoryID", (object?)parentProductCategoryId ?? DBNull.Value);
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static List<ProductCategory> ReadCategories(SqlDataReader reader)
        {
            var results = new List<ProductCategory>();
            while (reader.Read())
            {
                results.Add(new ProductCategory
                {
                    ProductCategoryId = reader.GetInt32(reader.GetOrdinal("ProductCategoryID")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    ParentProductCategoryId = reader.IsDBNull(reader.GetOrdinal("ParentProductCategoryID")) ? null : reader.GetInt32(reader.GetOrdinal("ParentProductCategoryID")),
                    Rowguid = reader.GetGuid(reader.GetOrdinal("rowguid")),
                    ModifiedDate = reader.GetDateTime(reader.GetOrdinal("ModifiedDate"))
                });
            }
            return results;
        }
    }
}

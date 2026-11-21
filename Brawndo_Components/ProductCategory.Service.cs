using Brawndo_Components.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Brawndo_Components
{
    public interface IProductCategoryService
    {
        int Create(string name, int? parentProductCategoryId = null);
        bool Delete(int productCategoryId);
        List<ProductCategory> GetAll();
        ProductCategory? GetByID(int productCategoryId);
        List<ProductCategory> GetRootCategories();
        bool Update(int productCategoryId, string? name = null, int? parentProductCategoryId = null);
    }

    public class ProductCategoryService : IProductCategoryService
    {
        private readonly string _connectionString;

        public ProductCategoryService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("AdventureWorksConnection") ?? throw new InvalidOperationException("Connection string 'AdventureWorksConnection' not found.");
        }

        public int Create(string name, int? parentProductCategoryId = null)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
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
                using var connection = new SqlConnection(_connectionString);
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
                using var connection = new SqlConnection(_connectionString);
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
                using var connection = new SqlConnection(_connectionString);
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
                using var connection = new SqlConnection(_connectionString);
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
                using var connection = new SqlConnection(_connectionString);
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

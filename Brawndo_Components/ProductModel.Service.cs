using Brawndo_Components.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Brawndo_Components
{
    public interface IProductModelService
    {
        int Create(string name, string? catalogDescription = null);
        bool Delete(int productModelId);
        List<ProductModel> GetAll();
        ProductModel? GetByID(int productModelId);
    }

    public class ProductModelService : IProductModelService
    {
        private readonly string _connectionString;

        public ProductModelService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("AdventureWorksConnection") ?? throw new InvalidOperationException("Connection string 'AdventureWorksConnection' not found.");
        }

        public int Create(string name, string? catalogDescription = null)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
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
                using var connection = new SqlConnection(_connectionString);
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
                using var connection = new SqlConnection(_connectionString);
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_ProductModel_GetAll", connection);
                command.CommandType = CommandType.StoredProcedure;
                using var reader = command.ExecuteReader();
                var results = new List<ProductModel>();
                while (reader.Read())
                {
                    results.Add(new ProductModel
                    {
                        ProductModelId = reader.GetInt32(reader.GetOrdinal("ProductModelID")),
                        Name = reader.GetString(reader.GetOrdinal("Name")),
                        ModifiedDate = reader.GetDateTime(reader.GetOrdinal("ModifiedDate"))
                    });
                }
                return results;
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
                using var connection = new SqlConnection(_connectionString);
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_ProductModel_GetByID", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProductModelID", productModelId);
                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new ProductModel
                    {
                        ProductModelId = reader.GetInt32(reader.GetOrdinal("ProductModelID")),
                        Name = reader.GetString(reader.GetOrdinal("Name")),
                        CatalogDescription = reader.IsDBNull(reader.GetOrdinal("CatalogDescription")) ? null : reader.GetString(reader.GetOrdinal("CatalogDescription")),
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
    }
}

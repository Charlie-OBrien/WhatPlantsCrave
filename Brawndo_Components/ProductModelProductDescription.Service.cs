using Brawndo_Components.Data;
using Brawndo_Components.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Brawndo_Components
{
    public interface IProductModelProductDescriptionService
    {
        bool Create(int productModelId, int productDescriptionId, string culture);
        bool Delete(int productModelId, int productDescriptionId, string culture);
        ProductModelProductDescription? Get(int productModelId, int productDescriptionId, string culture);
        List<ProductModelProductDescription> GetAll();
        bool Update(int productModelId, int productDescriptionId, string cultureOld, string cultureNew);
    }

    public class ProductModelProductDescriptionService : IProductModelProductDescriptionService
    {
        private readonly AdventureWorksContext _context;

        public ProductModelProductDescriptionService(AdventureWorksContext context)
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
    }
}

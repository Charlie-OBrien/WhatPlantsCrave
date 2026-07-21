using Brawndo_Components.Data;
using Brawndo_Components.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Brawndo_Components
{
    public interface IProductDescriptionService
    {
        int Create(string description);
        bool Delete(int productDescriptionId);
        List<ProductDescription> GetAll();
        ProductDescription? GetByID(int productDescriptionId);
        bool Update(int productDescriptionId, string? description = null);
    }

    public class ProductDescriptionService : IProductDescriptionService
    {
        private readonly AdventureWorksContext _context;

        public ProductDescriptionService(AdventureWorksContext context)
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
                if (reader.Read())
                {
                    return new ProductDescription
                    {
                        ProductDescriptionId = reader.GetInt32(reader.GetOrdinal("ProductDescriptionID")),
                        Description = reader.GetString(reader.GetOrdinal("Description")),
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
    }
}

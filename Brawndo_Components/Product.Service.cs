using Brawndo_Components.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Brawndo_Components
{
    public interface IProductService
    {
        int Create(string name, string productNumber, decimal standardCost, decimal listPrice, DateTime sellStartDate, string? color = null, string? size = null, decimal? weight = null, int? productCategoryId = null, int? productModelId = null, DateTime? sellEndDate = null, DateTime? discontinuedDate = null, string? thumbnailPhotoFileName = null);
        bool Delete(int productId);
        List<Product> GetActive();
        List<Product> GetAll();
        Product? GetByID(int productId);
        List<Product> SearchByName(string searchTerm);
        bool Update(int productId, string? name = null, decimal? standardCost = null, decimal? listPrice = null, string? color = null);
    }

    public class ProductService : IProductService
    {
        private readonly string _connectionString;

        public ProductService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("AdventureWorksConnection") ?? throw new InvalidOperationException("Connection string 'AdventureWorksConnection' not found.");
        }

        public int Create(string name, string productNumber, decimal standardCost, decimal listPrice, DateTime sellStartDate, string? color = null, string? size = null, decimal? weight = null, int? productCategoryId = null, int? productModelId = null, DateTime? sellEndDate = null, DateTime? discontinuedDate = null, string? thumbnailPhotoFileName = null)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_Product_Create", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@ProductNumber", productNumber);
                command.Parameters.AddWithValue("@StandardCost", standardCost);
                command.Parameters.AddWithValue("@ListPrice", listPrice);
                command.Parameters.AddWithValue("@SellStartDate", sellStartDate);
                command.Parameters.AddWithValue("@Color", (object?)color ?? DBNull.Value);
                command.Parameters.AddWithValue("@Size", (object?)size ?? DBNull.Value);
                command.Parameters.AddWithValue("@Weight", (object?)weight ?? DBNull.Value);
                command.Parameters.AddWithValue("@ProductCategoryID", (object?)productCategoryId ?? DBNull.Value);
                command.Parameters.AddWithValue("@ProductModelID", (object?)productModelId ?? DBNull.Value);
                command.Parameters.AddWithValue("@SellEndDate", (object?)sellEndDate ?? DBNull.Value);
                command.Parameters.AddWithValue("@DiscontinuedDate", (object?)discontinuedDate ?? DBNull.Value);
                command.Parameters.AddWithValue("@ThumbnailPhotoFileName", (object?)thumbnailPhotoFileName ?? DBNull.Value);
                var outputParam = new SqlParameter("@ProductID", SqlDbType.Int) { Direction = ParameterDirection.Output };
                command.Parameters.Add(outputParam);
                command.ExecuteNonQuery();
                return (int)outputParam.Value;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public bool Delete(int productId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_Product_Delete", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProductID", productId);
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<Product> GetActive()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_Product_GetActive", connection);
                command.CommandType = CommandType.StoredProcedure;
                using var reader = command.ExecuteReader();
                return ReadProducts(reader);
            }
            catch (Exception)
            {
                return new List<Product>();
            }
        }

        public List<Product> GetAll()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_Product_GetAll", connection);
                command.CommandType = CommandType.StoredProcedure;
                using var reader = command.ExecuteReader();
                return ReadProducts(reader);
            }
            catch (Exception)
            {
                return new List<Product>();
            }
        }

        public Product? GetByID(int productId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_Product_GetByID", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProductID", productId);
                using var reader = command.ExecuteReader();
                var products = ReadProducts(reader);
                return products.FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<Product> SearchByName(string searchTerm)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_Product_SearchByName", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SearchTerm", searchTerm);
                using var reader = command.ExecuteReader();
                return ReadProducts(reader);
            }
            catch (Exception)
            {
                return new List<Product>();
            }
        }

        public bool Update(int productId, string? name = null, decimal? standardCost = null, decimal? listPrice = null, string? color = null)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_Product_Update", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProductID", productId);
                command.Parameters.AddWithValue("@Name", (object?)name ?? DBNull.Value);
                command.Parameters.AddWithValue("@StandardCost", (object?)standardCost ?? DBNull.Value);
                command.Parameters.AddWithValue("@ListPrice", (object?)listPrice ?? DBNull.Value);
                command.Parameters.AddWithValue("@Color", (object?)color ?? DBNull.Value);
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static List<Product> ReadProducts(SqlDataReader reader)
        {
            var results = new List<Product>();
            while (reader.Read())
            {
                results.Add(new Product
                {
                    ProductId = reader.GetInt32(reader.GetOrdinal("ProductID")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    ProductNumber = reader.GetString(reader.GetOrdinal("ProductNumber")),
                    Color = reader.IsDBNull(reader.GetOrdinal("Color")) ? null : reader.GetString(reader.GetOrdinal("Color")),
                    StandardCost = reader.GetDecimal(reader.GetOrdinal("StandardCost")),
                    ListPrice = reader.GetDecimal(reader.GetOrdinal("ListPrice")),
                    Size = reader.IsDBNull(reader.GetOrdinal("Size")) ? null : reader.GetString(reader.GetOrdinal("Size")),
                    Weight = reader.IsDBNull(reader.GetOrdinal("Weight")) ? null : reader.GetDecimal(reader.GetOrdinal("Weight")),
                    ProductCategoryId = reader.IsDBNull(reader.GetOrdinal("ProductCategoryID")) ? null : reader.GetInt32(reader.GetOrdinal("ProductCategoryID")),
                    ProductModelId = reader.IsDBNull(reader.GetOrdinal("ProductModelID")) ? null : reader.GetInt32(reader.GetOrdinal("ProductModelID")),
                    SellStartDate = reader.GetDateTime(reader.GetOrdinal("SellStartDate")),
                    SellEndDate = reader.IsDBNull(reader.GetOrdinal("SellEndDate")) ? null : reader.GetDateTime(reader.GetOrdinal("SellEndDate")),
                    DiscontinuedDate = reader.IsDBNull(reader.GetOrdinal("DiscontinuedDate")) ? null : reader.GetDateTime(reader.GetOrdinal("DiscontinuedDate")),
                    ThumbnailPhotoFileName = reader.IsDBNull(reader.GetOrdinal("ThumbnailPhotoFileName")) ? null : reader.GetString(reader.GetOrdinal("ThumbnailPhotoFileName")),
                    Rowguid = reader.GetGuid(reader.GetOrdinal("rowguid")),
                    ModifiedDate = reader.GetDateTime(reader.GetOrdinal("ModifiedDate"))
                });
            }
            return results;
        }
    }
}

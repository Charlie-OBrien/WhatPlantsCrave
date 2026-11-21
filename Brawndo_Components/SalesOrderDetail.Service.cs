using Brawndo_Components.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Brawndo_Components
{
    public interface ISalesOrderDetailService
    {
        int Create(int salesOrderId, int productId, short orderQty, decimal unitPrice, decimal unitPriceDiscount = 0);
        bool Delete(int salesOrderId, int salesOrderDetailId);
        SalesOrderDetail? Get(int salesOrderId, int salesOrderDetailId);
        List<SalesOrderDetail> GetAll();
        List<SalesOrderDetail> GetByOrder(int salesOrderId);
        bool Update(int salesOrderId, int salesOrderDetailId, short? orderQty = null, decimal? unitPrice = null);
    }

    public class SalesOrderDetailService : ISalesOrderDetailService
    {
        private readonly string _connectionString;

        public SalesOrderDetailService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("AdventureWorksConnection") ?? throw new InvalidOperationException("Connection string 'AdventureWorksConnection' not found.");
        }

        public int Create(int salesOrderId, int productId, short orderQty, decimal unitPrice, decimal unitPriceDiscount = 0)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_SalesOrderDetail_Create", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SalesOrderID", salesOrderId);
                command.Parameters.AddWithValue("@ProductID", productId);
                command.Parameters.AddWithValue("@OrderQty", orderQty);
                command.Parameters.AddWithValue("@UnitPrice", unitPrice);
                command.Parameters.AddWithValue("@UnitPriceDiscount", unitPriceDiscount);
                var outputParam = new SqlParameter("@SalesOrderDetailID", SqlDbType.Int) { Direction = ParameterDirection.Output };
                command.Parameters.Add(outputParam);
                command.ExecuteNonQuery();
                return (int)outputParam.Value;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public bool Delete(int salesOrderId, int salesOrderDetailId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_SalesOrderDetail_Delete", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SalesOrderID", salesOrderId);
                command.Parameters.AddWithValue("@SalesOrderDetailID", salesOrderDetailId);
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public SalesOrderDetail? Get(int salesOrderId, int salesOrderDetailId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_SalesOrderDetail_Get", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SalesOrderID", salesOrderId);
                command.Parameters.AddWithValue("@SalesOrderDetailID", salesOrderDetailId);
                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new SalesOrderDetail
                    {
                        SalesOrderId = reader.GetInt32(reader.GetOrdinal("SalesOrderID")),
                        SalesOrderDetailId = reader.GetInt32(reader.GetOrdinal("SalesOrderDetailID")),
                        OrderQty = reader.GetInt16(reader.GetOrdinal("OrderQty")),
                        ProductId = reader.GetInt32(reader.GetOrdinal("ProductID")),
                        UnitPrice = reader.GetDecimal(reader.GetOrdinal("UnitPrice")),
                        LineTotal = reader.GetDecimal(reader.GetOrdinal("LineTotal"))
                    };
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<SalesOrderDetail> GetAll()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_SalesOrderDetail_GetAll", connection);
                command.CommandType = CommandType.StoredProcedure;
                using var reader = command.ExecuteReader();
                var results = new List<SalesOrderDetail>();
                while (reader.Read())
                {
                    results.Add(new SalesOrderDetail
                    {
                        SalesOrderId = reader.GetInt32(reader.GetOrdinal("SalesOrderID")),
                        SalesOrderDetailId = reader.GetInt32(reader.GetOrdinal("SalesOrderDetailID")),
                        OrderQty = reader.GetInt16(reader.GetOrdinal("OrderQty")),
                        ProductId = reader.GetInt32(reader.GetOrdinal("ProductID")),
                        UnitPrice = reader.GetDecimal(reader.GetOrdinal("UnitPrice")),
                        LineTotal = reader.GetDecimal(reader.GetOrdinal("LineTotal"))
                    });
                }
                return results;
            }
            catch (Exception)
            {
                return new List<SalesOrderDetail>();
            }
        }

        public List<SalesOrderDetail> GetByOrder(int salesOrderId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_SalesOrderDetail_GetByOrder", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SalesOrderID", salesOrderId);
                using var reader = command.ExecuteReader();
                var results = new List<SalesOrderDetail>();
                while (reader.Read())
                {
                    results.Add(new SalesOrderDetail
                    {
                        SalesOrderDetailId = reader.GetInt32(reader.GetOrdinal("SalesOrderDetailID")),
                        OrderQty = reader.GetInt16(reader.GetOrdinal("OrderQty")),
                        UnitPrice = reader.GetDecimal(reader.GetOrdinal("UnitPrice")),
                        LineTotal = reader.GetDecimal(reader.GetOrdinal("LineTotal")),
                        Product = new Product
                        {
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        }
                    });
                }
                return results;
            }
            catch (Exception)
            {
                return new List<SalesOrderDetail>();
            }
        }

        public bool Update(int salesOrderId, int salesOrderDetailId, short? orderQty = null, decimal? unitPrice = null)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_SalesOrderDetail_Update", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SalesOrderID", salesOrderId);
                command.Parameters.AddWithValue("@SalesOrderDetailID", salesOrderDetailId);
                command.Parameters.AddWithValue("@OrderQty", (object?)orderQty ?? DBNull.Value);
                command.Parameters.AddWithValue("@UnitPrice", (object?)unitPrice ?? DBNull.Value);
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

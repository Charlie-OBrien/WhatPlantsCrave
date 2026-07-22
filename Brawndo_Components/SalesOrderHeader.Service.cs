using Brawndo_Components.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Brawndo_Components
{
    public interface ISalesOrderHeaderService
    {
        int Create(byte revisionNumber, DateTime orderDate, DateTime dueDate, int customerId, string shipMethod, decimal subTotal, decimal taxAmt, decimal freight);
        bool Delete(int salesOrderId);
        List<SalesOrderHeader> GetAll(int limit = 100);
        List<SalesOrderHeader> GetByCustomer(int customerId);
        SalesOrderHeader? GetByID(int salesOrderId);
        bool Update(int salesOrderId, byte? status = null, DateTime? shipDate = null);
    }

    public class SalesOrderHeaderService : ISalesOrderHeaderService
    {
        private readonly string _connectionString;

        public SalesOrderHeaderService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("AdventureWorksConnection") ?? throw new InvalidOperationException("Connection string 'AdventureWorksConnection' not found.");
        }

        public int Create(byte revisionNumber, DateTime orderDate, DateTime dueDate, int customerId, string shipMethod, decimal subTotal, decimal taxAmt, decimal freight)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_SalesOrderHeader_Create", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@RevisionNumber", revisionNumber);
                command.Parameters.AddWithValue("@OrderDate", orderDate);
                command.Parameters.AddWithValue("@DueDate", dueDate);
                command.Parameters.AddWithValue("@CustomerID", customerId);
                command.Parameters.AddWithValue("@ShipMethod", shipMethod);
                command.Parameters.AddWithValue("@SubTotal", subTotal);
                command.Parameters.AddWithValue("@TaxAmt", taxAmt);
                command.Parameters.AddWithValue("@Freight", freight);
                var outputParam = new SqlParameter("@SalesOrderID", SqlDbType.Int) { Direction = ParameterDirection.Output };
                command.Parameters.Add(outputParam);
                command.ExecuteNonQuery();
                return (int)outputParam.Value;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public bool Delete(int salesOrderId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_SalesOrderHeader_Delete", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SalesOrderID", salesOrderId);
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<SalesOrderHeader> GetAll(int limit = 100)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_SalesOrderHeader_GetAll", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Limit", limit);
                using var reader = command.ExecuteReader();
                var results = new List<SalesOrderHeader>();
                while (reader.Read())
                {
                    results.Add(new SalesOrderHeader
                    {
                        SalesOrderId = reader.GetInt32(reader.GetOrdinal("SalesOrderID")),
                        OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate")),
                        CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerID")),
                        Status = reader.GetByte(reader.GetOrdinal("Status")),
                        TotalDue = reader.GetDecimal(reader.GetOrdinal("TotalDue"))
                    });
                }
                return results;
            }
            catch (Exception)
            {
                return new List<SalesOrderHeader>();
            }
        }

        public List<SalesOrderHeader> GetByCustomer(int customerId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_SalesOrderHeader_GetByCustomer", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CustomerID", customerId);
                using var reader = command.ExecuteReader();
                var results = new List<SalesOrderHeader>();
                while (reader.Read())
                {
                    results.Add(new SalesOrderHeader
                    {
                        SalesOrderId = reader.GetInt32(reader.GetOrdinal("SalesOrderID")),
                        SalesOrderNumber = reader.GetString(reader.GetOrdinal("SalesOrderNumber")),
                        OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate")),
                        DueDate = reader.GetDateTime(reader.GetOrdinal("DueDate")),
                        Status = reader.GetByte(reader.GetOrdinal("Status")),
                        TotalDue = reader.GetDecimal(reader.GetOrdinal("TotalDue"))
                    });
                }
                return results;
            }
            catch (Exception)
            {
                return new List<SalesOrderHeader>();
            }
        }

        public SalesOrderHeader? GetByID(int salesOrderId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_SalesOrderHeader_GetByID", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SalesOrderID", salesOrderId);
                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new SalesOrderHeader
                    {
                        SalesOrderId = reader.GetInt32(reader.GetOrdinal("SalesOrderID")),
                        OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate")),
                        DueDate = reader.GetDateTime(reader.GetOrdinal("DueDate")),
                        ShipDate = reader.IsDBNull(reader.GetOrdinal("ShipDate")) ? null : reader.GetDateTime(reader.GetOrdinal("ShipDate")),
                        Status = reader.GetByte(reader.GetOrdinal("Status")),
                        CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerID")),
                        SubTotal = reader.GetDecimal(reader.GetOrdinal("SubTotal")),
                        TaxAmt = reader.GetDecimal(reader.GetOrdinal("TaxAmt")),
                        Freight = reader.GetDecimal(reader.GetOrdinal("Freight")),
                        TotalDue = reader.GetDecimal(reader.GetOrdinal("TotalDue")),
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

        public bool Update(int salesOrderId, byte? status = null, DateTime? shipDate = null)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_SalesOrderHeader_Update", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SalesOrderID", salesOrderId);
                command.Parameters.AddWithValue("@Status", (object?)status ?? DBNull.Value);
                command.Parameters.AddWithValue("@ShipDate", (object?)shipDate ?? DBNull.Value);
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

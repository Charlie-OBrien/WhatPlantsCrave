using Brawndo_Components;
using Brawndo_Components.Data;
using Brawndo_Components.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace WhatPlantsCrave.Infrastructure.Repositories.Sql
{
    /// <summary>
    /// SalesOrderHeader repository implementation using SQL Server stored procedures.
    /// Executes actual database operations via ADO.NET.
    /// </summary>
    public class SqlSalesOrderHeaderRepository : ISalesOrderHeaderRepository
    {
        private readonly AdventureWorksContext _context;

        public SqlSalesOrderHeaderRepository(AdventureWorksContext context)
        {
            _context = context;
        }

        public int Create(byte revisionNumber, DateTime orderDate, DateTime dueDate, int customerId, string shipMethod, decimal subTotal, decimal taxAmt, decimal freight)
        {
            try
            {
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
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
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
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
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_SalesOrderHeader_GetAll", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Limit", limit);
                using var reader = command.ExecuteReader();
                return ReadHeaders(reader);
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
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_SalesOrderHeader_GetByCustomer", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CustomerID", customerId);
                using var reader = command.ExecuteReader();
                return ReadHeaders(reader);
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
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_SalesOrderHeader_GetByID", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SalesOrderID", salesOrderId);
                using var reader = command.ExecuteReader();
                var headers = ReadHeaders(reader);
                return headers.FirstOrDefault();
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
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
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

        private static List<SalesOrderHeader> ReadHeaders(SqlDataReader reader)
        {
            var results = new List<SalesOrderHeader>();
            while (reader.Read())
            {
                results.Add(new SalesOrderHeader
                {
                    SalesOrderId = reader.GetInt32(reader.GetOrdinal("SalesOrderID")),
                    SalesOrderNumber = reader.IsDBNull(reader.GetOrdinal("SalesOrderNumber")) ? null : reader.GetString(reader.GetOrdinal("SalesOrderNumber")),
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
                });
            }
            return results;
        }
    }
}

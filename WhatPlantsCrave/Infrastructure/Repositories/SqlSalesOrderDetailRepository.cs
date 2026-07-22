using Brawndo_Components;
using Brawndo_Components.Data;
using Brawndo_Components.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace WhatPlantsCrave.Infrastructure.Repositories
{
    /// <summary>
    /// SalesOrderDetail repository implementation using SQL Server stored procedures.
    /// Executes actual database operations via ADO.NET.
    /// </summary>
    public class SqlSalesOrderDetailRepository : ISalesOrderDetailRepository
    {
        private readonly AdventureWorksContext _context;

        public SqlSalesOrderDetailRepository(AdventureWorksContext context)
        {
            _context = context;
        }

        public int Create(int salesOrderId, int productId, short orderQty, decimal unitPrice, decimal unitPriceDiscount = 0)
        {
            try
            {
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
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
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
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
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
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
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_SalesOrderDetail_GetAll", connection);
                command.CommandType = CommandType.StoredProcedure;
                using var reader = command.ExecuteReader();
                return ReadDetails(reader);
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
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_SalesOrderDetail_GetByOrder", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SalesOrderID", salesOrderId);
                using var reader = command.ExecuteReader();
                return ReadDetails(reader);
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
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
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

        private static List<SalesOrderDetail> ReadDetails(SqlDataReader reader)
        {
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
    }
}

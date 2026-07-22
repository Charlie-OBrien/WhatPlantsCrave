using Brawndo_Components;
using Brawndo_Components.Data;
using Brawndo_Components.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace WhatPlantsCrave.Infrastructure.Repositories.Sql
{
    /// <summary>
    /// CustomerAddress repository implementation using SQL Server stored procedures.
    /// Executes actual database operations via ADO.NET.
    /// </summary>
    public class SqlCustomerAddressRepository : ICustomerAddressRepository
    {
        private readonly AdventureWorksContext _context;

        public SqlCustomerAddressRepository(AdventureWorksContext context)
        {
            _context = context;
        }

        public bool Create(int customerId, int addressId, string addressType)
        {
            try
            {
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_CustomerAddress_Create", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CustomerID", customerId);
                command.Parameters.AddWithValue("@AddressID", addressId);
                command.Parameters.AddWithValue("@AddressType", addressType);
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Delete(int customerId, int addressId)
        {
            try
            {
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_CustomerAddress_Delete", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CustomerID", customerId);
                command.Parameters.AddWithValue("@AddressID", addressId);
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public CustomerAddress? Get(int customerId, int addressId)
        {
            try
            {
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_CustomerAddress_Get", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CustomerID", customerId);
                command.Parameters.AddWithValue("@AddressID", addressId);
                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new CustomerAddress
                    {
                        CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerID")),
                        AddressId = reader.GetInt32(reader.GetOrdinal("AddressID")),
                        AddressType = reader.GetString(reader.GetOrdinal("AddressType")),
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

        public List<CustomerAddress> GetAll()
        {
            try
            {
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_CustomerAddress_GetAll", connection);
                command.CommandType = CommandType.StoredProcedure;
                using var reader = command.ExecuteReader();
                var results = new List<CustomerAddress>();
                while (reader.Read())
                {
                    results.Add(new CustomerAddress
                    {
                        CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerID")),
                        AddressId = reader.GetInt32(reader.GetOrdinal("AddressID")),
                        AddressType = reader.GetString(reader.GetOrdinal("AddressType")),
                        ModifiedDate = reader.GetDateTime(reader.GetOrdinal("ModifiedDate"))
                    });
                }
                return results;
            }
            catch (Exception)
            {
                return new List<CustomerAddress>();
            }
        }

        public List<CustomerAddress> GetByCustomer(int customerId)
        {
            try
            {
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_CustomerAddress_GetByCustomer", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CustomerID", customerId);
                using var reader = command.ExecuteReader();
                var results = new List<CustomerAddress>();
                while (reader.Read())
                {
                    results.Add(new CustomerAddress
                    {
                        CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerID")),
                        AddressId = reader.GetInt32(reader.GetOrdinal("AddressID")),
                        AddressType = reader.GetString(reader.GetOrdinal("AddressType")),
                        Address = new Address
                        {
                            AddressLine1 = reader.GetString(reader.GetOrdinal("AddressLine1")),
                            City = reader.GetString(reader.GetOrdinal("City")),
                            StateProvince = reader.GetString(reader.GetOrdinal("StateProvince"))
                        }
                    });
                }
                return results;
            }
            catch (Exception)
            {
                return new List<CustomerAddress>();
            }
        }

        public bool Update(int customerId, int addressId, string? addressType = null)
        {
            try
            {
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_CustomerAddress_Update", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CustomerID", customerId);
                command.Parameters.AddWithValue("@AddressID", addressId);
                command.Parameters.AddWithValue("@AddressType", (object?)addressType ?? DBNull.Value);
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

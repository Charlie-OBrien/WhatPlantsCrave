using Brawndo_Components;
using Brawndo_Components.Data;
using Brawndo_Components.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace WhatPlantsCrave.Infrastructure.Repositories.Sql
{
    /// <summary>
    /// Customer repository implementation using SQL Server stored procedures.
    /// Executes actual database operations via ADO.NET.
    /// </summary>
    public class SqlCustomerRepository : ICustomerRepository
    {
        private readonly AdventureWorksContext _context;

        public SqlCustomerRepository(AdventureWorksContext context)
        {
            _context = context;
        }

        public int Create(bool nameStyle, string firstName, string lastName, string passwordHash, string passwordSalt, string? title = null, string? middleName = null, string? emailAddress = null, string? phone = null, string? companyName = null)
        {
            try
            {
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_Customer_Create", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@NameStyle", nameStyle);
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@LastName", lastName);
                command.Parameters.AddWithValue("@PasswordHash", passwordHash);
                command.Parameters.AddWithValue("@PasswordSalt", passwordSalt);
                command.Parameters.AddWithValue("@Title", (object?)title ?? DBNull.Value);
                command.Parameters.AddWithValue("@MiddleName", (object?)middleName ?? DBNull.Value);
                command.Parameters.AddWithValue("@EmailAddress", (object?)emailAddress ?? DBNull.Value);
                command.Parameters.AddWithValue("@Phone", (object?)phone ?? DBNull.Value);
                command.Parameters.AddWithValue("@CompanyName", (object?)companyName ?? DBNull.Value);
                var outputParam = new SqlParameter("@CustomerID", SqlDbType.Int) { Direction = ParameterDirection.Output };
                command.Parameters.Add(outputParam);
                command.ExecuteNonQuery();
                return (int)outputParam.Value;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public bool Delete(int customerId)
        {
            try
            {
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_Customer_Delete", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CustomerID", customerId);
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<Customer> GetAll()
        {
            try
            {
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_Customer_GetAll", connection);
                command.CommandType = CommandType.StoredProcedure;
                using var reader = command.ExecuteReader();
                return ReadCustomers(reader);
            }
            catch (Exception)
            {
                return new List<Customer>();
            }
        }

        public Customer? GetByEmail(string emailAddress)
        {
            try
            {
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_Customer_GetByEmail", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@EmailAddress", emailAddress);
                using var reader = command.ExecuteReader();
                var customers = ReadCustomers(reader);
                return customers.FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Customer? GetByID(int customerId)
        {
            try
            {
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_Customer_GetByID", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CustomerID", customerId);
                using var reader = command.ExecuteReader();
                var customers = ReadCustomers(reader);
                return customers.FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<Customer> SearchByName(string searchTerm)
        {
            try
            {
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_Customer_SearchByName", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SearchTerm", searchTerm);
                using var reader = command.ExecuteReader();
                return ReadCustomers(reader);
            }
            catch (Exception)
            {
                return new List<Customer>();
            }
        }

        public bool Update(int customerId, string? firstName = null, string? lastName = null, string? emailAddress = null, string? phone = null)
        {
            try
            {
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_Customer_Update", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CustomerID", customerId);
                command.Parameters.AddWithValue("@FirstName", (object?)firstName ?? DBNull.Value);
                command.Parameters.AddWithValue("@LastName", (object?)lastName ?? DBNull.Value);
                command.Parameters.AddWithValue("@EmailAddress", (object?)emailAddress ?? DBNull.Value);
                command.Parameters.AddWithValue("@Phone", (object?)phone ?? DBNull.Value);
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static List<Customer> ReadCustomers(SqlDataReader reader)
        {
            var results = new List<Customer>();
            while (reader.Read())
            {
                results.Add(new Customer
                {
                    CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerID")),
                    NameStyle = reader.GetBoolean(reader.GetOrdinal("NameStyle")),
                    Title = reader.IsDBNull(reader.GetOrdinal("Title")) ? null : reader.GetString(reader.GetOrdinal("Title")),
                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                    MiddleName = reader.IsDBNull(reader.GetOrdinal("MiddleName")) ? null : reader.GetString(reader.GetOrdinal("MiddleName")),
                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                    CompanyName = reader.IsDBNull(reader.GetOrdinal("CompanyName")) ? null : reader.GetString(reader.GetOrdinal("CompanyName")),
                    EmailAddress = reader.IsDBNull(reader.GetOrdinal("EmailAddress")) ? null : reader.GetString(reader.GetOrdinal("EmailAddress")),
                    Phone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? null : reader.GetString(reader.GetOrdinal("Phone")),
                    Rowguid = reader.GetGuid(reader.GetOrdinal("rowguid")),
                    ModifiedDate = reader.GetDateTime(reader.GetOrdinal("ModifiedDate"))
                });
            }
            return results;
        }
    }
}

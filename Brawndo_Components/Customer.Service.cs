using Brawndo_Components.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Brawndo_Components
{
    public interface ICustomerService
    {
        int Create(bool nameStyle, string firstName, string lastName, string passwordHash, string passwordSalt, string? title = null, string? middleName = null, string? emailAddress = null, string? phone = null, string? companyName = null);
        bool Delete(int customerId);
        List<Customer> GetAll();
        Customer? GetByEmail(string emailAddress);
        Customer? GetByID(int customerId);
        List<Customer> SearchByName(string searchTerm);
        bool Update(int customerId, string? firstName = null, string? lastName = null, string? emailAddress = null, string? phone = null);
    }

    public class CustomerService : ICustomerService
    {
        private readonly string _connectionString;

        public CustomerService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("AdventureWorksConnection") ?? throw new InvalidOperationException("Connection string 'AdventureWorksConnection' not found.");
        }

        public int Create(bool nameStyle, string firstName, string lastName, string passwordHash, string passwordSalt, string? title = null, string? middleName = null, string? emailAddress = null, string? phone = null, string? companyName = null)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
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
                using var connection = new SqlConnection(_connectionString);
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
                using var connection = new SqlConnection(_connectionString);
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_Customer_GetAll", connection);
                command.CommandType = CommandType.StoredProcedure;
                using var reader = command.ExecuteReader();
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
            catch (Exception)
            {
                return new List<Customer>();
            }
        }

        public Customer? GetByEmail(string emailAddress)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_Customer_GetByEmail", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@EmailAddress", emailAddress);
                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new Customer
                    {
                        CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerID")),
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        EmailAddress = reader.IsDBNull(reader.GetOrdinal("EmailAddress")) ? null : reader.GetString(reader.GetOrdinal("EmailAddress")),
                        CompanyName = reader.IsDBNull(reader.GetOrdinal("CompanyName")) ? null : reader.GetString(reader.GetOrdinal("CompanyName"))
                    };
                }
                return null;
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
                using var connection = new SqlConnection(_connectionString);
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_Customer_GetByID", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CustomerID", customerId);
                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new Customer
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
                        PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                        PasswordSalt = reader.GetString(reader.GetOrdinal("PasswordSalt")),
                        Rowguid = reader.GetGuid(reader.GetOrdinal("rowguid")),
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

        public List<Customer> SearchByName(string searchTerm)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_Customer_SearchByName", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SearchTerm", searchTerm);
                using var reader = command.ExecuteReader();
                var results = new List<Customer>();
                while (reader.Read())
                {
                    results.Add(new Customer
                    {
                        CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerID")),
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        EmailAddress = reader.IsDBNull(reader.GetOrdinal("EmailAddress")) ? null : reader.GetString(reader.GetOrdinal("EmailAddress")),
                        CompanyName = reader.IsDBNull(reader.GetOrdinal("CompanyName")) ? null : reader.GetString(reader.GetOrdinal("CompanyName")),
                        ModifiedDate = reader.GetDateTime(reader.GetOrdinal("ModifiedDate"))
                    });
                }
                return results;
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
                using var connection = new SqlConnection(_connectionString);
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
    }
}

using Brawndo_Components.Data;
using Brawndo_Components.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Brawndo_Components
{
    public interface IAddressService
    {
        int Create(string addressLine1, string city, string stateProvince, string countryRegion, string postalCode, string? addressLine2 = null);
        bool Delete(int addressId);
        List<Address> GetAll();
        List<Address> GetByCountryRegion(string countryRegion);
        Address? GetByID(int addressId);
        List<Address> SearchByCity(string city);
        bool Update(int addressId, string? addressLine1 = null, string? city = null, string? stateProvince = null, string? postalCode = null);
    }

    public class AddressService : IAddressService
    {
        private readonly AdventureWorksContext _context;

        public AddressService(AdventureWorksContext context)
        {
            _context = context;
        }

        public int Create(string addressLine1, string city, string stateProvince, string countryRegion, string postalCode, string? addressLine2 = null)
        {
            try
            {
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_Address_Create", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AddressLine1", addressLine1);
                command.Parameters.AddWithValue("@City", city);
                command.Parameters.AddWithValue("@StateProvince", stateProvince);
                command.Parameters.AddWithValue("@CountryRegion", countryRegion);
                command.Parameters.AddWithValue("@PostalCode", postalCode);
                command.Parameters.AddWithValue("@AddressLine2", (object?)addressLine2 ?? DBNull.Value);
                var outputParam = new SqlParameter("@AddressID", SqlDbType.Int) { Direction = ParameterDirection.Output };
                command.Parameters.Add(outputParam);
                command.ExecuteNonQuery();
                return (int)outputParam.Value;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public bool Delete(int addressId)
        {
            try
            {
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_Address_Delete", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AddressID", addressId);
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<Address> GetAll()
        {
            try
            {
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_Address_GetAll", connection);
                command.CommandType = CommandType.StoredProcedure;
                using var reader = command.ExecuteReader();
                var results = new List<Address>();
                while (reader.Read())
                {
                    results.Add(new Address
                    {
                        AddressId = reader.GetInt32(reader.GetOrdinal("AddressID")),
                        AddressLine1 = reader.GetString(reader.GetOrdinal("AddressLine1")),
                        AddressLine2 = reader.IsDBNull(reader.GetOrdinal("AddressLine2")) ? null : reader.GetString(reader.GetOrdinal("AddressLine2")),
                        City = reader.GetString(reader.GetOrdinal("City")),
                        StateProvince = reader.GetString(reader.GetOrdinal("StateProvince")),
                        CountryRegion = reader.GetString(reader.GetOrdinal("CountryRegion")),
                        PostalCode = reader.GetString(reader.GetOrdinal("PostalCode"))
                    });
                }
                return results;
            }
            catch (Exception)
            {
                return new List<Address>();
            }
        }

        public List<Address> GetByCountryRegion(string countryRegion)
        {
            try
            {
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_Address_GetByCountryRegion", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CountryRegion", countryRegion);
                using var reader = command.ExecuteReader();
                var results = new List<Address>();
                while (reader.Read())
                {
                    results.Add(new Address
                    {
                        AddressId = reader.GetInt32(reader.GetOrdinal("AddressID")),
                        AddressLine1 = reader.GetString(reader.GetOrdinal("AddressLine1")),
                        City = reader.GetString(reader.GetOrdinal("City")),
                        StateProvince = reader.GetString(reader.GetOrdinal("StateProvince")),
                        PostalCode = reader.GetString(reader.GetOrdinal("PostalCode"))
                    });
                }
                return results;
            }
            catch (Exception)
            {
                return new List<Address>();
            }
        }

        public Address? GetByID(int addressId)
        {
            try
            {
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_Address_GetByID", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AddressID", addressId);
                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new Address
                    {
                        AddressId = reader.GetInt32(reader.GetOrdinal("AddressID")),
                        AddressLine1 = reader.GetString(reader.GetOrdinal("AddressLine1")),
                        AddressLine2 = reader.IsDBNull(reader.GetOrdinal("AddressLine2")) ? null : reader.GetString(reader.GetOrdinal("AddressLine2")),
                        City = reader.GetString(reader.GetOrdinal("City")),
                        StateProvince = reader.GetString(reader.GetOrdinal("StateProvince")),
                        CountryRegion = reader.GetString(reader.GetOrdinal("CountryRegion")),
                        PostalCode = reader.GetString(reader.GetOrdinal("PostalCode")),
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

        public List<Address> SearchByCity(string city)
        {
            try
            {
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_Address_SearchByCity", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@City", city);
                using var reader = command.ExecuteReader();
                var results = new List<Address>();
                while (reader.Read())
                {
                    results.Add(new Address
                    {
                        AddressId = reader.GetInt32(reader.GetOrdinal("AddressID")),
                        AddressLine1 = reader.GetString(reader.GetOrdinal("AddressLine1")),
                        AddressLine2 = reader.IsDBNull(reader.GetOrdinal("AddressLine2")) ? null : reader.GetString(reader.GetOrdinal("AddressLine2")),
                        City = reader.GetString(reader.GetOrdinal("City")),
                        StateProvince = reader.GetString(reader.GetOrdinal("StateProvince")),
                        CountryRegion = reader.GetString(reader.GetOrdinal("CountryRegion")),
                        PostalCode = reader.GetString(reader.GetOrdinal("PostalCode"))
                    });
                }
                return results;
            }
            catch (Exception)
            {
                return new List<Address>();
            }
        }

        public bool Update(int addressId, string? addressLine1 = null, string? city = null, string? stateProvince = null, string? postalCode = null)
        {
            try
            {
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_Address_Update", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AddressID", addressId);
                command.Parameters.AddWithValue("@AddressLine1", (object?)addressLine1 ?? DBNull.Value);
                command.Parameters.AddWithValue("@City", (object?)city ?? DBNull.Value);
                command.Parameters.AddWithValue("@StateProvince", (object?)stateProvince ?? DBNull.Value);
                command.Parameters.AddWithValue("@PostalCode", (object?)postalCode ?? DBNull.Value);
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

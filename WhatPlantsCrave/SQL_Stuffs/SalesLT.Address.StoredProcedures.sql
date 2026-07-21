-- =============================================
-- Address Table Stored Procedures
-- =============================================

USE [AdventureWorksLT2016]
GO

-- =============================================
-- Create Address
-- =============================================
CREATE PROCEDURE [SalesLT].[usp_Address_Create]
    @AddressLine1 NVARCHAR(60),
    @City NVARCHAR(30),
    @StateProvince NVARCHAR(50),
    @CountryRegion NVARCHAR(50),
    @PostalCode NVARCHAR(15),
    @AddressLine2 NVARCHAR(60) = NULL,
    @AddressID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [SalesLT].[Address]
    ([AddressLine1], [AddressLine2], [City], [StateProvince], [CountryRegion],
     [PostalCode], [rowguid], [ModifiedDate])
    VALUES
    (@AddressLine1, @AddressLine2, @City, @StateProvince, @CountryRegion,
     @PostalCode, NEWID(), GETDATE());

    SET @AddressID = SCOPE_IDENTITY();
END
GO

-- =============================================
-- Get Address by ID
-- =============================================
CREATE PROCEDURE [SalesLT].[usp_Address_GetByID]
    @AddressID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT [AddressID], [AddressLine1], [AddressLine2], [City], [StateProvince],
           [CountryRegion], [PostalCode], [rowguid], [ModifiedDate]
    FROM [SalesLT].[Address]
    WHERE [AddressID] = @AddressID;
END
GO

-- =============================================
-- Get All Addresses
-- =============================================
CREATE PROCEDURE [SalesLT].[usp_Address_GetAll]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT [AddressID], [AddressLine1], [AddressLine2], [City], [StateProvince],
           [CountryRegion], [PostalCode]
    FROM [SalesLT].[Address]
    ORDER BY [CountryRegion], [StateProvince], [City];
END
GO

-- =============================================
-- Search Addresses by City
-- =============================================
CREATE PROCEDURE [SalesLT].[usp_Address_SearchByCity]
    @City NVARCHAR(30)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT [AddressID], [AddressLine1], [AddressLine2], [City], [StateProvince],
           [CountryRegion], [PostalCode]
    FROM [SalesLT].[Address]
    WHERE [City] LIKE '%' + @City + '%'
    ORDER BY [City], [AddressLine1];
END
GO

-- =============================================
-- Get Addresses by Country/Region
-- =============================================
CREATE PROCEDURE [SalesLT].[usp_Address_GetByCountryRegion]
    @CountryRegion NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT [AddressID], [AddressLine1], [City], [StateProvince], [PostalCode]
    FROM [SalesLT].[Address]
    WHERE [CountryRegion] = @CountryRegion
    ORDER BY [StateProvince], [City];
END
GO

-- =============================================
-- Update Address
-- =============================================
CREATE PROCEDURE [SalesLT].[usp_Address_Update]
    @AddressID INT,
    @AddressLine1 NVARCHAR(60) = NULL,
    @City NVARCHAR(30) = NULL,
    @StateProvince NVARCHAR(50) = NULL,
    @PostalCode NVARCHAR(15) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE [SalesLT].[Address]
    SET
        [AddressLine1] = ISNULL(@AddressLine1, [AddressLine1]),
        [City] = ISNULL(@City, [City]),
        [StateProvince] = ISNULL(@StateProvince, [StateProvince]),
        [PostalCode] = ISNULL(@PostalCode, [PostalCode]),
        [ModifiedDate] = GETDATE()
    WHERE [AddressID] = @AddressID;
END
GO

-- =============================================
-- Delete Address
-- =============================================
CREATE PROCEDURE [SalesLT].[usp_Address_Delete]
    @AddressID INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM [SalesLT].[Address]
    WHERE [AddressID] = @AddressID;
END
GO

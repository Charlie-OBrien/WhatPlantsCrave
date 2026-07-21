-- =============================================
-- Customer Table Stored Procedures
-- =============================================

USE [AdventureWorksLT2016]
GO

-- =============================================
-- Create Customer
-- =============================================
CREATE PROCEDURE [SalesLT].[usp_Customer_Create]
    @NameStyle BIT,
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @Title NVARCHAR(8) = NULL,
    @MiddleName NVARCHAR(50) = NULL,
    @EmailAddress NVARCHAR(50) = NULL,
    @Phone NVARCHAR(25) = NULL,
    @CompanyName NVARCHAR(128) = NULL,
    @PasswordHash VARCHAR(128),
    @PasswordSalt VARCHAR(10),
    @CustomerID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [SalesLT].[Customer]
    ([NameStyle], [Title], [FirstName], [MiddleName], [LastName],
     [CompanyName], [EmailAddress], [Phone], [PasswordHash],
     [PasswordSalt], [rowguid], [ModifiedDate])
    VALUES
    (@NameStyle, @Title, @FirstName, @MiddleName, @LastName,
     @CompanyName, @EmailAddress, @Phone, @PasswordHash,
     @PasswordSalt, NEWID(), GETDATE());

    SET @CustomerID = SCOPE_IDENTITY();
END
GO

-- =============================================
-- Get Customer by ID
-- =============================================
CREATE PROCEDURE [SalesLT].[usp_Customer_GetByID]
    @CustomerID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT [CustomerID], [NameStyle], [Title], [FirstName], [MiddleName], [LastName],
           [CompanyName], [EmailAddress], [Phone], [PasswordHash], [PasswordSalt],
           [rowguid], [ModifiedDate]
    FROM [SalesLT].[Customer]
    WHERE [CustomerID] = @CustomerID;
END
GO

-- =============================================
-- Get All Customers
-- =============================================
CREATE PROCEDURE [SalesLT].[usp_Customer_GetAll]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT [CustomerID], [NameStyle], [Title], [FirstName], [MiddleName], [LastName],
           [CompanyName], [EmailAddress], [Phone], [rowguid], [ModifiedDate]
    FROM [SalesLT].[Customer]
    ORDER BY [LastName], [FirstName];
END
GO

-- =============================================
-- Search Customers by Name
-- =============================================
CREATE PROCEDURE [SalesLT].[usp_Customer_SearchByName]
    @SearchTerm NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT [CustomerID], [FirstName], [LastName], [EmailAddress], [CompanyName], [ModifiedDate]
    FROM [SalesLT].[Customer]
    WHERE [FirstName] LIKE '%' + @SearchTerm + '%'
       OR [LastName] LIKE '%' + @SearchTerm + '%'
       OR [CompanyName] LIKE '%' + @SearchTerm + '%'
    ORDER BY [LastName], [FirstName];
END
GO

-- =============================================
-- Get Customer by Email
-- =============================================
CREATE PROCEDURE [SalesLT].[usp_Customer_GetByEmail]
    @EmailAddress NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT [CustomerID], [FirstName], [LastName], [EmailAddress], [CompanyName]
    FROM [SalesLT].[Customer]
    WHERE [EmailAddress] = @EmailAddress;
END
GO

-- =============================================
-- Update Customer
-- =============================================
CREATE PROCEDURE [SalesLT].[usp_Customer_Update]
    @CustomerID INT,
    @FirstName NVARCHAR(50) = NULL,
    @LastName NVARCHAR(50) = NULL,
    @EmailAddress NVARCHAR(50) = NULL,
    @Phone NVARCHAR(25) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE [SalesLT].[Customer]
    SET
        [FirstName] = ISNULL(@FirstName, [FirstName]),
        [LastName] = ISNULL(@LastName, [LastName]),
        [EmailAddress] = COALESCE(@EmailAddress, [EmailAddress]),
        [Phone] = COALESCE(@Phone, [Phone]),
        [ModifiedDate] = GETDATE()
    WHERE [CustomerID] = @CustomerID;
END
GO

-- =============================================
-- Delete Customer
-- =============================================
CREATE PROCEDURE [SalesLT].[usp_Customer_Delete]
    @CustomerID INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM [SalesLT].[Customer]
    WHERE [CustomerID] = @CustomerID;
END
GO

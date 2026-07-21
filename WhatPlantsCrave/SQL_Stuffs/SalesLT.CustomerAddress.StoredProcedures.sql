-- CustomerAddress Stored Procedures
USE [AdventureWorksLT2016]
GO

CREATE PROCEDURE [SalesLT].[usp_CustomerAddress_Create]
    @CustomerID INT,
    @AddressID INT,
    @AddressType NVARCHAR(50)
AS BEGIN
    SET NOCOUNT ON;
    INSERT INTO [SalesLT].[CustomerAddress]([CustomerID], [AddressID], [AddressType], [rowguid], [ModifiedDate])
    VALUES(@CustomerID, @AddressID, @AddressType, NEWID(), GETDATE());
END
GO

CREATE PROCEDURE [SalesLT].[usp_CustomerAddress_Get]
    @CustomerID INT,
    @AddressID INT
AS BEGIN
    SET NOCOUNT ON;
    SELECT [CustomerID], [AddressID], [AddressType], [ModifiedDate]
    FROM [SalesLT].[CustomerAddress]
    WHERE [CustomerID] = @CustomerID AND [AddressID] = @AddressID;
END
GO

CREATE PROCEDURE [SalesLT].[usp_CustomerAddress_GetAll]
AS BEGIN
    SET NOCOUNT ON;
    SELECT [CustomerID], [AddressID], [AddressType], [ModifiedDate]
    FROM [SalesLT].[CustomerAddress]
    ORDER BY [CustomerID], [AddressType];
END
GO

CREATE PROCEDURE [SalesLT].[usp_CustomerAddress_GetByCustomer]
    @CustomerID INT
AS BEGIN
    SET NOCOUNT ON;
    SELECT ca.[CustomerID], ca.[AddressID], ca.[AddressType], a.[AddressLine1], a.[City], a.[StateProvince]
    FROM [SalesLT].[CustomerAddress] ca
    JOIN [SalesLT].[Address] a ON ca.[AddressID] = a.[AddressID]
    WHERE ca.[CustomerID] = @CustomerID
    ORDER BY ca.[AddressType];
END
GO

CREATE PROCEDURE [SalesLT].[usp_CustomerAddress_Update]
    @CustomerID INT,
    @AddressID INT,
    @AddressType NVARCHAR(50) = NULL
AS BEGIN
    SET NOCOUNT ON;
    UPDATE [SalesLT].[CustomerAddress]
    SET [AddressType] = ISNULL(@AddressType, [AddressType]), [ModifiedDate] = GETDATE()
    WHERE [CustomerID] = @CustomerID AND [AddressID] = @AddressID;
END
GO

CREATE PROCEDURE [SalesLT].[usp_CustomerAddress_Delete]
    @CustomerID INT,
    @AddressID INT
AS BEGIN
    SET NOCOUNT ON;
    DELETE FROM [SalesLT].[CustomerAddress]
    WHERE [CustomerID] = @CustomerID AND [AddressID] = @AddressID;
END
GO

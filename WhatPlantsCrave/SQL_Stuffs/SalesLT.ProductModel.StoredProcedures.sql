-- ProductModel Stored Procedures
USE [AdventureWorksLT2016]
GO

CREATE PROCEDURE [SalesLT].[usp_ProductModel_Create]
    @Name NVARCHAR(50),
    @CatalogDescription XML = NULL,
    @ProductModelID INT OUTPUT
AS BEGIN
    SET NOCOUNT ON;
    INSERT INTO [SalesLT].[ProductModel]([Name], [CatalogDescription], [rowguid], [ModifiedDate])
    VALUES(@Name, @CatalogDescription, NEWID(), GETDATE());
    SET @ProductModelID = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE [SalesLT].[usp_ProductModel_GetByID]
    @ProductModelID INT
AS BEGIN
    SET NOCOUNT ON;
    SELECT [ProductModelID], [Name], [CatalogDescription], [ModifiedDate]
    FROM [SalesLT].[ProductModel]
    WHERE [ProductModelID] = @ProductModelID;
END
GO

CREATE PROCEDURE [SalesLT].[usp_ProductModel_GetAll]
AS BEGIN
    SET NOCOUNT ON;
    SELECT [ProductModelID], [Name], [ModifiedDate]
    FROM [SalesLT].[ProductModel]
    ORDER BY [Name];
END
GO

CREATE PROCEDURE [SalesLT].[usp_ProductModel_Update]
    @ProductModelID INT,
    @Name NVARCHAR(50) = NULL,
    @CatalogDescription XML = NULL
AS BEGIN
    SET NOCOUNT ON;
    UPDATE [SalesLT].[ProductModel]
    SET [Name] = ISNULL(@Name, [Name]),
        [CatalogDescription] = COALESCE(@CatalogDescription, [CatalogDescription]),
        [ModifiedDate] = GETDATE()
    WHERE [ProductModelID] = @ProductModelID;
END
GO

CREATE PROCEDURE [SalesLT].[usp_ProductModel_Delete]
    @ProductModelID INT
AS BEGIN
    SET NOCOUNT ON;
    DELETE FROM [SalesLT].[ProductModel]
    WHERE [ProductModelID] = @ProductModelID;
END
GO

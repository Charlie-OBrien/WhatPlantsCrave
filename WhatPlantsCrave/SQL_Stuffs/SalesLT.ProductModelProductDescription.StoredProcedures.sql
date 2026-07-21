-- ProductModelProductDescription Stored Procedures
USE [AdventureWorksLT2016]
GO

CREATE PROCEDURE [SalesLT].[usp_ProductModelProductDescription_Create]
    @ProductModelID INT,
    @ProductDescriptionID INT,
    @Culture NCHAR(6)
AS BEGIN
    SET NOCOUNT ON;
    INSERT INTO [SalesLT].[ProductModelProductDescription]([ProductModelID], [ProductDescriptionID], [Culture], [rowguid], [ModifiedDate])
    VALUES(@ProductModelID, @ProductDescriptionID, @Culture, NEWID(), GETDATE());
END
GO

CREATE PROCEDURE [SalesLT].[usp_ProductModelProductDescription_Get]
    @ProductModelID INT,
    @ProductDescriptionID INT,
    @Culture NCHAR(6)
AS BEGIN
    SET NOCOUNT ON;
    SELECT [ProductModelID], [ProductDescriptionID], [Culture], [ModifiedDate]
    FROM [SalesLT].[ProductModelProductDescription]
    WHERE [ProductModelID] = @ProductModelID AND [ProductDescriptionID] = @ProductDescriptionID AND [Culture] = @Culture;
END
GO

CREATE PROCEDURE [SalesLT].[usp_ProductModelProductDescription_GetAll]
AS BEGIN
    SET NOCOUNT ON;
    SELECT [ProductModelID], [ProductDescriptionID], [Culture], [ModifiedDate]
    FROM [SalesLT].[ProductModelProductDescription]
    ORDER BY [ProductModelID], [Culture];
END
GO

CREATE PROCEDURE [SalesLT].[usp_ProductModelProductDescription_Update]
    @ProductModelID INT,
    @ProductDescriptionID INT,
    @CultureOld NCHAR(6),
    @CultureNew NCHAR(6)
AS BEGIN
    SET NOCOUNT ON;
    UPDATE [SalesLT].[ProductModelProductDescription]
    SET [Culture] = @CultureNew, [ModifiedDate] = GETDATE()
    WHERE [ProductModelID] = @ProductModelID AND [ProductDescriptionID] = @ProductDescriptionID AND [Culture] = @CultureOld;
END
GO

CREATE PROCEDURE [SalesLT].[usp_ProductModelProductDescription_Delete]
    @ProductModelID INT,
    @ProductDescriptionID INT,
    @Culture NCHAR(6)
AS BEGIN
    SET NOCOUNT ON;
    DELETE FROM [SalesLT].[ProductModelProductDescription]
    WHERE [ProductModelID] = @ProductModelID AND [ProductDescriptionID] = @ProductDescriptionID AND [Culture] = @Culture;
END
GO

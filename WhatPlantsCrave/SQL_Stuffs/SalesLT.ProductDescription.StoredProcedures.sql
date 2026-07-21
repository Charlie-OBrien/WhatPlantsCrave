-- ProductDescription Stored Procedures
USE [AdventureWorksLT2016]
GO

CREATE PROCEDURE [SalesLT].[usp_ProductDescription_Create]
    @Description NVARCHAR(400),
    @ProductDescriptionID INT OUTPUT
AS BEGIN
    SET NOCOUNT ON;
    INSERT INTO [SalesLT].[ProductDescription]([Description], [rowguid], [ModifiedDate])
    VALUES(@Description, NEWID(), GETDATE());
    SET @ProductDescriptionID = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE [SalesLT].[usp_ProductDescription_GetByID]
    @ProductDescriptionID INT
AS BEGIN
    SET NOCOUNT ON;
    SELECT [ProductDescriptionID], [Description], [ModifiedDate]
    FROM [SalesLT].[ProductDescription]
    WHERE [ProductDescriptionID] = @ProductDescriptionID;
END
GO

CREATE PROCEDURE [SalesLT].[usp_ProductDescription_GetAll]
AS BEGIN
    SET NOCOUNT ON;
    SELECT [ProductDescriptionID], [Description], [ModifiedDate]
    FROM [SalesLT].[ProductDescription]
    ORDER BY [ProductDescriptionID];
END
GO

CREATE PROCEDURE [SalesLT].[usp_ProductDescription_Update]
    @ProductDescriptionID INT,
    @Description NVARCHAR(400) = NULL
AS BEGIN
    SET NOCOUNT ON;
    UPDATE [SalesLT].[ProductDescription]
    SET [Description] = ISNULL(@Description, [Description]),
        [ModifiedDate] = GETDATE()
    WHERE [ProductDescriptionID] = @ProductDescriptionID;
END
GO

CREATE PROCEDURE [SalesLT].[usp_ProductDescription_Delete]
    @ProductDescriptionID INT
AS BEGIN
    SET NOCOUNT ON;
    DELETE FROM [SalesLT].[ProductDescription]
    WHERE [ProductDescriptionID] = @ProductDescriptionID;
END
GO

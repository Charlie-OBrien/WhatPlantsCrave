-- =============================================
-- ProductCategory Table Stored Procedures
-- =============================================

USE [AdventureWorksLT2016]
GO

-- =============================================
-- Create ProductCategory
-- =============================================
CREATE PROCEDURE [SalesLT].[usp_ProductCategory_Create]
    @Name NVARCHAR(50),
    @ParentProductCategoryID INT = NULL,
    @ProductCategoryID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [SalesLT].[ProductCategory]
    ([Name], [ParentProductCategoryID], [rowguid], [ModifiedDate])
    VALUES
    (@Name, @ParentProductCategoryID, NEWID(), GETDATE());

    SET @ProductCategoryID = SCOPE_IDENTITY();
END
GO

-- =============================================
-- Get ProductCategory by ID
-- =============================================
CREATE PROCEDURE [SalesLT].[usp_ProductCategory_GetByID]
    @ProductCategoryID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT [ProductCategoryID], [Name], [ParentProductCategoryID], [rowguid], [ModifiedDate]
    FROM [SalesLT].[ProductCategory]
    WHERE [ProductCategoryID] = @ProductCategoryID;
END
GO

-- =============================================
-- Get All ProductCategories
-- =============================================
CREATE PROCEDURE [SalesLT].[usp_ProductCategory_GetAll]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT [ProductCategoryID], [Name], [ParentProductCategoryID], [rowguid], [ModifiedDate]
    FROM [SalesLT].[ProductCategory]
    ORDER BY [Name];
END
GO

-- =============================================
-- Get Root Categories (No Parent)
-- =============================================
CREATE PROCEDURE [SalesLT].[usp_ProductCategory_GetRootCategories]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT [ProductCategoryID], [Name], [ParentProductCategoryID], [rowguid], [ModifiedDate]
    FROM [SalesLT].[ProductCategory]
    WHERE [ParentProductCategoryID] IS NULL
    ORDER BY [Name];
END
GO

-- =============================================
-- Update ProductCategory
-- =============================================
CREATE PROCEDURE [SalesLT].[usp_ProductCategory_Update]
    @ProductCategoryID INT,
    @Name NVARCHAR(50) = NULL,
    @ParentProductCategoryID INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE [SalesLT].[ProductCategory]
    SET
        [Name] = ISNULL(@Name, [Name]),
        [ParentProductCategoryID] = COALESCE(@ParentProductCategoryID, [ParentProductCategoryID]),
        [ModifiedDate] = GETDATE()
    WHERE [ProductCategoryID] = @ProductCategoryID;
END
GO

-- =============================================
-- Delete ProductCategory
-- =============================================
CREATE PROCEDURE [SalesLT].[usp_ProductCategory_Delete]
    @ProductCategoryID INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE [SalesLT].[ProductCategory]
    SET [ParentProductCategoryID] = NULL
    WHERE [ParentProductCategoryID] = @ProductCategoryID;

    DELETE FROM [SalesLT].[ProductCategory]
    WHERE [ProductCategoryID] = @ProductCategoryID;
END
GO

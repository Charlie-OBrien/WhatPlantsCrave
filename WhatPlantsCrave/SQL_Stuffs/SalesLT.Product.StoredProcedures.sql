-- =============================================
-- Product Table Stored Procedures
-- =============================================

USE [AdventureWorksLT2016]
GO

-- =============================================
-- Create Product
-- =============================================
CREATE PROCEDURE [SalesLT].[usp_Product_Create]
    @Name NVARCHAR(50),
    @ProductNumber NVARCHAR(25),
    @Color NVARCHAR(15) = NULL,
    @StandardCost MONEY,
    @ListPrice MONEY,
    @Size NVARCHAR(5) = NULL,
    @Weight DECIMAL(8, 2) = NULL,
    @ProductCategoryID INT = NULL,
    @ProductModelID INT = NULL,
    @SellStartDate DATETIME,
    @SellEndDate DATETIME = NULL,
    @DiscontinuedDate DATETIME = NULL,
    @ThumbnailPhotoFileName NVARCHAR(50) = NULL,
    @ProductID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [SalesLT].[Product]
    ([Name], [ProductNumber], [Color], [StandardCost], [ListPrice], [Size], [Weight],
     [ProductCategoryID], [ProductModelID], [SellStartDate], [SellEndDate], [DiscontinuedDate],
     [ThumbnailPhotoFileName], [rowguid], [ModifiedDate])
    VALUES
    (@Name, @ProductNumber, @Color, @StandardCost, @ListPrice, @Size, @Weight,
     @ProductCategoryID, @ProductModelID, @SellStartDate, @SellEndDate, @DiscontinuedDate,
     @ThumbnailPhotoFileName, NEWID(), GETDATE());

    SET @ProductID = SCOPE_IDENTITY();
END
GO

-- =============================================
-- Get Product by ID
-- =============================================
CREATE PROCEDURE [SalesLT].[usp_Product_GetByID]
    @ProductID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT [ProductID], [Name], [ProductNumber], [Color], [StandardCost], [ListPrice],
           [Size], [Weight], [ProductCategoryID], [ProductModelID], [SellStartDate],
           [SellEndDate], [DiscontinuedDate], [ThumbnailPhotoFileName], [rowguid], [ModifiedDate]
    FROM [SalesLT].[Product]
    WHERE [ProductID] = @ProductID;
END
GO

-- =============================================
-- Get All Products
-- =============================================
CREATE PROCEDURE [SalesLT].[usp_Product_GetAll]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT [ProductID], [Name], [ProductNumber], [Color], [StandardCost], [ListPrice],
           [Size], [Weight], [ProductCategoryID], [ProductModelID], [SellStartDate],
           [SellEndDate], [DiscontinuedDate], [ThumbnailPhotoFileName], [rowguid], [ModifiedDate]
    FROM [SalesLT].[Product]
    ORDER BY [Name];
END
GO

-- =============================================
-- Get Active Products
-- =============================================
CREATE PROCEDURE [SalesLT].[usp_Product_GetActive]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT [ProductID], [Name], [ProductNumber], [Color], [StandardCost], [ListPrice],
           [Size], [Weight], [ProductCategoryID], [ProductModelID], [SellStartDate],
           [SellEndDate], [DiscontinuedDate], [ThumbnailPhotoFileName], [rowguid], [ModifiedDate]
    FROM [SalesLT].[Product]
    WHERE [SellStartDate] <= GETDATE()
      AND ([SellEndDate] IS NULL OR [SellEndDate] >= GETDATE())
      AND [DiscontinuedDate] IS NULL
    ORDER BY [Name];
END
GO

-- =============================================
-- Search Products by Name
-- =============================================
CREATE PROCEDURE [SalesLT].[usp_Product_SearchByName]
    @SearchTerm NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT [ProductID], [Name], [ProductNumber], [Color], [StandardCost], [ListPrice],
           [Size], [Weight], [ProductCategoryID], [ProductModelID], [SellStartDate],
           [SellEndDate], [DiscontinuedDate], [ThumbnailPhotoFileName], [rowguid], [ModifiedDate]
    FROM [SalesLT].[Product]
    WHERE [Name] LIKE '%' + @SearchTerm + '%'
    ORDER BY [Name];
END
GO

-- =============================================
-- Update Product
-- =============================================
CREATE PROCEDURE [SalesLT].[usp_Product_Update]
    @ProductID INT,
    @Name NVARCHAR(50) = NULL,
    @StandardCost MONEY = NULL,
    @ListPrice MONEY = NULL,
    @Color NVARCHAR(15) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE [SalesLT].[Product]
    SET
        [Name] = ISNULL(@Name, [Name]),
        [StandardCost] = ISNULL(@StandardCost, [StandardCost]),
        [ListPrice] = ISNULL(@ListPrice, [ListPrice]),
        [Color] = COALESCE(@Color, [Color]),
        [ModifiedDate] = GETDATE()
    WHERE [ProductID] = @ProductID;
END
GO

-- =============================================
-- Delete Product
-- =============================================
CREATE PROCEDURE [SalesLT].[usp_Product_Delete]
    @ProductID INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM [SalesLT].[Product]
    WHERE [ProductID] = @ProductID;
END
GO

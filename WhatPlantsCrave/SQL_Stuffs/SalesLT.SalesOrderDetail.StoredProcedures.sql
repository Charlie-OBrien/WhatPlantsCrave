-- SalesOrderDetail Stored Procedures
USE [AdventureWorksLT2016]
GO

CREATE PROCEDURE [SalesLT].[usp_SalesOrderDetail_Create]
    @SalesOrderID INT,
    @ProductID INT,
    @OrderQty SMALLINT,
    @UnitPrice MONEY,
    @UnitPriceDiscount MONEY = 0,
    @SalesOrderDetailID INT OUTPUT
AS BEGIN
    SET NOCOUNT ON;
    INSERT INTO [SalesLT].[SalesOrderDetail]([SalesOrderID], [ProductID], [OrderQty], [UnitPrice], [UnitPriceDiscount], [rowguid], [ModifiedDate])
    VALUES(@SalesOrderID, @ProductID, @OrderQty, @UnitPrice, @UnitPriceDiscount, NEWID(), GETDATE());
    SET @SalesOrderDetailID = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE [SalesLT].[usp_SalesOrderDetail_Get]
    @SalesOrderID INT,
    @SalesOrderDetailID INT
AS BEGIN
    SET NOCOUNT ON;
    SELECT [SalesOrderID], [SalesOrderDetailID], [OrderQty], [ProductID], [UnitPrice], [LineTotal]
    FROM [SalesLT].[SalesOrderDetail]
    WHERE [SalesOrderID] = @SalesOrderID AND [SalesOrderDetailID] = @SalesOrderDetailID;
END
GO

CREATE PROCEDURE [SalesLT].[usp_SalesOrderDetail_GetAll]
AS BEGIN
    SET NOCOUNT ON;
    SELECT [SalesOrderID], [SalesOrderDetailID], [OrderQty], [ProductID], [UnitPrice], [LineTotal]
    FROM [SalesLT].[SalesOrderDetail]
    ORDER BY [SalesOrderID], [SalesOrderDetailID];
END
GO

CREATE PROCEDURE [SalesLT].[usp_SalesOrderDetail_GetByOrder]
    @SalesOrderID INT
AS BEGIN
    SET NOCOUNT ON;
    SELECT sod.[SalesOrderDetailID], sod.[OrderQty], p.[Name], sod.[UnitPrice], sod.[LineTotal]
    FROM [SalesLT].[SalesOrderDetail] sod
    JOIN [SalesLT].[Product] p ON sod.[ProductID] = p.[ProductID]
    WHERE sod.[SalesOrderID] = @SalesOrderID;
END
GO

CREATE PROCEDURE [SalesLT].[usp_SalesOrderDetail_Update]
    @SalesOrderID INT,
    @SalesOrderDetailID INT,
    @OrderQty SMALLINT = NULL,
    @UnitPrice MONEY = NULL
AS BEGIN
    SET NOCOUNT ON;
    UPDATE [SalesLT].[SalesOrderDetail]
    SET [OrderQty] = ISNULL(@OrderQty, [OrderQty]), [UnitPrice] = ISNULL(@UnitPrice, [UnitPrice]), [ModifiedDate] = GETDATE()
    WHERE [SalesOrderID] = @SalesOrderID AND [SalesOrderDetailID] = @SalesOrderDetailID;
END
GO

CREATE PROCEDURE [SalesLT].[usp_SalesOrderDetail_Delete]
    @SalesOrderID INT,
    @SalesOrderDetailID INT
AS BEGIN
    SET NOCOUNT ON;
    DELETE FROM [SalesLT].[SalesOrderDetail]
    WHERE [SalesOrderID] = @SalesOrderID AND [SalesOrderDetailID] = @SalesOrderDetailID;
END
GO

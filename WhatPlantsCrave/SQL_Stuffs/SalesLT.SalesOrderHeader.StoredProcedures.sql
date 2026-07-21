-- SalesOrderHeader Stored Procedures
USE [AdventureWorksLT2016]
GO

CREATE PROCEDURE [SalesLT].[usp_SalesOrderHeader_Create]
    @RevisionNumber TINYINT,
    @OrderDate DATETIME,
    @DueDate DATETIME,
    @CustomerID INT,
    @ShipMethod NVARCHAR(50),
    @SubTotal MONEY,
    @TaxAmt MONEY,
    @Freight MONEY,
    @SalesOrderID INT OUTPUT
AS BEGIN
    SET NOCOUNT ON;
    INSERT INTO [SalesLT].[SalesOrderHeader]([RevisionNumber], [OrderDate], [DueDate], [CustomerID], [ShipMethod], [SubTotal], [TaxAmt], [Freight], [Status], [OnlineOrderFlag], [rowguid], [ModifiedDate])
    VALUES(@RevisionNumber, @OrderDate, @DueDate, @CustomerID, @ShipMethod, @SubTotal, @TaxAmt, @Freight, 1, 0, NEWID(), GETDATE());
    SET @SalesOrderID = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE [SalesLT].[usp_SalesOrderHeader_GetByID]
    @SalesOrderID INT
AS BEGIN
    SET NOCOUNT ON;
    SELECT [SalesOrderID], [OrderDate], [DueDate], [ShipDate], [Status], [CustomerID], [SubTotal], [TaxAmt], [Freight], [TotalDue], [ModifiedDate]
    FROM [SalesLT].[SalesOrderHeader]
    WHERE [SalesOrderID] = @SalesOrderID;
END
GO

CREATE PROCEDURE [SalesLT].[usp_SalesOrderHeader_GetAll]
    @Limit INT = 100
AS BEGIN
    SET NOCOUNT ON;
    SELECT TOP (@Limit) [SalesOrderID], [OrderDate], [CustomerID], [Status], [TotalDue]
    FROM [SalesLT].[SalesOrderHeader]
    ORDER BY [OrderDate] DESC;
END
GO

CREATE PROCEDURE [SalesLT].[usp_SalesOrderHeader_GetByCustomer]
    @CustomerID INT
AS BEGIN
    SET NOCOUNT ON;
    SELECT [SalesOrderID], [SalesOrderNumber], [OrderDate], [DueDate], [Status], [TotalDue]
    FROM [SalesLT].[SalesOrderHeader]
    WHERE [CustomerID] = @CustomerID
    ORDER BY [OrderDate] DESC;
END
GO

CREATE PROCEDURE [SalesLT].[usp_SalesOrderHeader_Update]
    @SalesOrderID INT,
    @Status TINYINT = NULL,
    @ShipDate DATETIME = NULL
AS BEGIN
    SET NOCOUNT ON;
    UPDATE [SalesLT].[SalesOrderHeader]
    SET [Status] = ISNULL(@Status, [Status]), [ShipDate] = COALESCE(@ShipDate, [ShipDate]), [ModifiedDate] = GETDATE()
    WHERE [SalesOrderID] = @SalesOrderID;
END
GO

CREATE PROCEDURE [SalesLT].[usp_SalesOrderHeader_Delete]
    @SalesOrderID INT
AS BEGIN
    SET NOCOUNT ON;
    DELETE FROM [SalesLT].[SalesOrderHeader]
    WHERE [SalesOrderID] = @SalesOrderID;
END
GO

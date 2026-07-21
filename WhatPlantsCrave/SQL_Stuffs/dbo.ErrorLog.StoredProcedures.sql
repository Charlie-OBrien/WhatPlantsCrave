-- ErrorLog Stored Procedures
USE [AdventureWorksLT2016]
GO

CREATE PROCEDURE [dbo].[usp_ErrorLog_Create]
    @UserName SYSNAME,
    @ErrorNumber INT,
    @ErrorMessage NVARCHAR(4000),
    @ErrorSeverity INT = NULL,
    @ErrorState INT = NULL,
    @ErrorProcedure NVARCHAR(126) = NULL,
    @ErrorLine INT = NULL,
    @ErrorLogID INT OUTPUT
AS BEGIN
    SET NOCOUNT ON;
    INSERT INTO [dbo].[ErrorLog]([ErrorTime], [UserName], [ErrorNumber], [ErrorSeverity], [ErrorState], [ErrorProcedure], [ErrorLine], [ErrorMessage])
    VALUES(GETDATE(), @UserName, @ErrorNumber, @ErrorSeverity, @ErrorState, @ErrorProcedure, @ErrorLine, @ErrorMessage);
    SET @ErrorLogID = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE [dbo].[usp_ErrorLog_GetByID]
    @ErrorLogID INT
AS BEGIN
    SET NOCOUNT ON;
    SELECT [ErrorLogId], [ErrorTime], [UserName], [ErrorNumber], [ErrorSeverity], [ErrorProcedure], [ErrorMessage]
    FROM [dbo].[ErrorLog]
    WHERE [ErrorLogId] = @ErrorLogID;
END
GO

CREATE PROCEDURE [dbo].[usp_ErrorLog_GetAll]
    @Limit INT = 100
AS BEGIN
    SET NOCOUNT ON;
    SELECT TOP (@Limit) [ErrorLogId], [ErrorTime], [UserName], [ErrorNumber], [ErrorMessage]
    FROM [dbo].[ErrorLog]
    ORDER BY [ErrorTime] DESC;
END
GO

CREATE PROCEDURE [dbo].[usp_ErrorLog_GetByUser]
    @UserName SYSNAME
AS BEGIN
    SET NOCOUNT ON;
    SELECT [ErrorLogId], [ErrorTime], [ErrorNumber], [ErrorMessage]
    FROM [dbo].[ErrorLog]
    WHERE [UserName] = @UserName
    ORDER BY [ErrorTime] DESC;
END
GO

CREATE PROCEDURE [dbo].[usp_ErrorLog_GetByErrorNumber]
    @ErrorNumber INT
AS BEGIN
    SET NOCOUNT ON;
    SELECT [ErrorLogId], [ErrorTime], [UserName], [ErrorNumber], [ErrorProcedure], [ErrorMessage]
    FROM [dbo].[ErrorLog]
    WHERE [ErrorNumber] = @ErrorNumber
    ORDER BY [ErrorTime] DESC;
END
GO

CREATE PROCEDURE [dbo].[usp_ErrorLog_Update]
    @ErrorLogID INT,
    @ErrorMessage NVARCHAR(4000) = NULL
AS BEGIN
    SET NOCOUNT ON;
    UPDATE [dbo].[ErrorLog]
    SET [ErrorMessage] = ISNULL(@ErrorMessage, [ErrorMessage])
    WHERE [ErrorLogId] = @ErrorLogID;
END
GO

CREATE PROCEDURE [dbo].[usp_ErrorLog_Delete]
    @ErrorLogID INT
AS BEGIN
    SET NOCOUNT ON;
    DELETE FROM [dbo].[ErrorLog]
    WHERE [ErrorLogId] = @ErrorLogID;
END
GO

CREATE PROCEDURE [dbo].[usp_ErrorLog_ClearOldErrors]
    @DaysToKeep INT = 30
AS BEGIN
    SET NOCOUNT ON;
    DELETE FROM [dbo].[ErrorLog]
    WHERE [ErrorTime] < DATEADD(DAY, -@DaysToKeep, GETDATE());
END
GO

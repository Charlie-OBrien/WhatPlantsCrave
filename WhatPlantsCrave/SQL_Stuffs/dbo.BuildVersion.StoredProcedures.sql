-- BuildVersion Stored Procedures
USE [AdventureWorksLT2016]
GO

CREATE PROCEDURE [dbo].[usp_BuildVersion_Create]
    @DatabaseVersion NVARCHAR(25),
    @VersionDate DATETIME
AS BEGIN
    SET NOCOUNT ON;
    INSERT INTO [dbo].[BuildVersion]([Database Version], [VersionDate], [ModifiedDate])
    VALUES(@DatabaseVersion, @VersionDate, GETDATE());
END
GO

CREATE PROCEDURE [dbo].[usp_BuildVersion_GetByID]
    @SystemInformationID TINYINT
AS BEGIN
    SET NOCOUNT ON;
    SELECT [SystemInformationId], [Database Version], [VersionDate], [ModifiedDate]
    FROM [dbo].[BuildVersion]
    WHERE [SystemInformationId] = @SystemInformationID;
END
GO

CREATE PROCEDURE [dbo].[usp_BuildVersion_GetLatest]
AS BEGIN
    SET NOCOUNT ON;
    SELECT TOP 1 [SystemInformationId], [Database Version], [VersionDate]
    FROM [dbo].[BuildVersion]
    ORDER BY [VersionDate] DESC;
END
GO

CREATE PROCEDURE [dbo].[usp_BuildVersion_GetAll]
AS BEGIN
    SET NOCOUNT ON;
    SELECT [SystemInformationId], [Database Version], [VersionDate], [ModifiedDate]
    FROM [dbo].[BuildVersion]
    ORDER BY [VersionDate] DESC;
END
GO

CREATE PROCEDURE [dbo].[usp_BuildVersion_Update]
    @SystemInformationID TINYINT,
    @DatabaseVersion NVARCHAR(25) = NULL,
    @VersionDate DATETIME = NULL
AS BEGIN
    SET NOCOUNT ON;
    UPDATE [dbo].[BuildVersion]
    SET [Database Version] = ISNULL(@DatabaseVersion, [Database Version]),
        [VersionDate] = ISNULL(@VersionDate, [VersionDate]),
        [ModifiedDate] = GETDATE()
    WHERE [SystemInformationId] = @SystemInformationID;
END
GO

CREATE PROCEDURE [dbo].[usp_BuildVersion_Delete]
    @SystemInformationID TINYINT
AS BEGIN
    SET NOCOUNT ON;
    DELETE FROM [dbo].[BuildVersion]
    WHERE [SystemInformationId] = @SystemInformationID;
END
GO

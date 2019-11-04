IF NOT EXISTS(SELECT * FROM [Information_Schema].[Tables] WHERE Table_Name='Tenants')
BEGIN
    CREATE TABLE [Tenants] (
        [Id] INT IDENTITY(1,1) NOT NULL CONSTRAINT [PK_Tenants] PRIMARY KEY CLUSTERED,
        [Name] NVARCHAR(100) NOT NULL,
        [Alias] NVARCHAR(50) NOT NULL,
        [ExternalId] UNIQUEIDENTIFIER NOT NULL
    );            
END;
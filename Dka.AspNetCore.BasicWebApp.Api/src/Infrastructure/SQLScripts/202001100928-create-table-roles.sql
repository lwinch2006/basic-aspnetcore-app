IF NOT EXISTS(SELECT * FROM [Information_Schema].[Tables] WHERE Table_Name = 'Roles')
    BEGIN
        CREATE TABLE [Roles] (
            [Id] INT IDENTITY (1,1) NOT NULL CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED,
            [Name] NVARCHAR(100) NOT NULL,
            [NormalizedName] NVARCHAR(100) NOT NULL CONSTRAINT [IX_Roles_NormalizedName] UNIQUE NONCLUSTERED,
            [ConcurrencyStamp] NVARCHAR(max) NULL,            
            [Guid] UNIQUEIDENTIFIER NOT NULL
        );
    END;
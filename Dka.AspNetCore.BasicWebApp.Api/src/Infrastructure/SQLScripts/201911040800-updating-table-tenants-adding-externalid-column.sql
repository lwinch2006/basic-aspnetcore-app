IF EXISTS(SELECT * FROM [Information_Schema].[Tables] WHERE Table_Name='Tenants')
BEGIN
    IF (COL_LENGTH('[dbo].[Tenants]', 'ExternalId') IS NULL) 
    BEGIN
        ALTER TABLE [Tenants]
        ADD 
            ExternalId UNIQUEIDENTIFIER
    END
END;
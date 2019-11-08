IF EXISTS(SELECT * FROM [Information_Schema].[Tables] WHERE Table_Name='Tenants')
BEGIN
    IF (COL_LENGTH('[dbo].[Tenants]', 'ExternalId') IS NOT NULL) 
    BEGIN
        EXEC sp_rename 'Tenants.ExternalId', 'Guid', 'COLUMN';
    END           
END;
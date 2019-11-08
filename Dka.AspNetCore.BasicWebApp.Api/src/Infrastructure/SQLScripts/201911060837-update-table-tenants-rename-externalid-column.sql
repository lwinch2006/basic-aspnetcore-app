IF EXISTS(SELECT * FROM [Information_Schema].[Tables] WHERE Table_Name='Tenants')
BEGIN
    IF (COL_LENGTH('[dbo].[Tenants]', 'Guid') IS NOT NULL) 
    BEGIN
        EXEC sp_rename 'Tenants.Guid', 'Guid123', 'COLUMN';
    END           
END;
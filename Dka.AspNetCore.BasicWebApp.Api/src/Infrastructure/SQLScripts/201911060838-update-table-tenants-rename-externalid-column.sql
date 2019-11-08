IF EXISTS(SELECT * FROM [Information_Schema].[Tables] WHERE Table_Name='Tenants')
BEGIN
    IF (COL_LENGTH('[dbo].[Tenants]', 'Guid123') IS NOT NULL) 
    BEGIN
        EXEC sp_rename 'Tenants.Guid123', 'Guid', 'COLUMN';
    END           
END;
IF EXISTS(SELECT * FROM [Information_Schema].[Tables] WHERE Table_Name='Tenants')
BEGIN
    IF (COL_LENGTH('[dbo].[Tenants]', 'Name') IS NULL) AND 
        (COL_LENGTH('[dbo].[Tenants]', 'Alias') IS NULL) 
    BEGIN
        ALTER TABLE [Tenants]
        ADD 
            Name NVARCHAR(100),
            Alias NVARCHAR(50);
    END
END;
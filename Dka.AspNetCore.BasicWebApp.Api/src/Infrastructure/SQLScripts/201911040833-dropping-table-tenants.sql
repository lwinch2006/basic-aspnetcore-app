IF EXISTS(SELECT * FROM [Information_Schema].[Tables] WHERE Table_Name='Tenants')
BEGIN
    DROP TABLE [Tenants];
END;
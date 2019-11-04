IF EXISTS(SELECT * FROM Information_Schema.Tables WHERE Table_Name='Tenants')
BEGIN
    IF (COL_LENGTH('[dbo].[Tenants]', 'Id') IS NOT NULL) 
    BEGIN
        DECLARE @PkName VARCHAR(100) = '';
    
        SELECT @PkName = [name]  
        FROM [sys].[key_constraints]  
        WHERE [type] = 'PK' AND OBJECT_NAME([parent_object_id]) = N'Tenants';  
    
        IF (@PkName IS NOT NULL AND @PkName <> '')
        BEGIN
			EXEC('ALTER TABLE [Tenants] DROP CONSTRAINT ' + @PkName)
        END 
    END
END;
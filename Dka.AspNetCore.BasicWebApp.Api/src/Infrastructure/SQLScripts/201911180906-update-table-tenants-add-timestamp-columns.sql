BEGIN TRY
    BEGIN TRANSACTION;

    IF EXISTS(SELECT * FROM [Information_Schema].[Tables] WHERE Table_Name='Tenants')
    BEGIN
        IF (COL_LENGTH('[dbo].[Tenants]', 'CreateOnUtc') IS NULL) 
        BEGIN
            ALTER TABLE [Tenants]
            ADD CreatedOnUtc DATETIME NOT NULL
        END
        
        IF (COL_LENGTH('[dbo].[Tenants]', 'CreatedBy') IS NULL) 
        BEGIN
            ALTER TABLE [Tenants]
            ADD CreatedBy INT NOT NULL
        END  
        
        IF (COL_LENGTH('[dbo].[Tenants]', 'UpdatedOnUtc') IS NULL) 
        BEGIN
            ALTER TABLE [Tenants]
            ADD UpdatedOnUtc DATETIME;        
        END  
        
        IF (COL_LENGTH('[dbo].[Tenants]', 'UpdatedBy') IS NULL) 
        BEGIN
            ALTER TABLE [Tenants]
            ADD UpdatedBy INT;        
        END                         
    END;

    IF @@TRANCOUNT > 0
    BEGIN
        COMMIT TRANSACTION;
    END;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
    BEGIN
        ROLLBACK TRANSACTION;
    END;
    
    DECLARE @ErrorMessage nvarchar(max), @ErrorSeverity int, @ErrorState int;
    SELECT @ErrorMessage = ERROR_MESSAGE() + ' Line ' + cast(ERROR_LINE() as nvarchar(5)), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();
    RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);          
END CATCH;
    



















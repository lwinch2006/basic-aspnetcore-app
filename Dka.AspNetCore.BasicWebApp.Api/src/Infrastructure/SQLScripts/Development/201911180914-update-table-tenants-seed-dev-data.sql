BEGIN TRY
    BEGIN TRANSACTION;

    IF EXISTS(SELECT * FROM [Information_Schema].[Tables] WHERE Table_Name='Tenants')
    BEGIN
        IF NOT EXISTS (SELECT [Name], [Alias] FROM [Tenants] WHERE [Name] = 'Umbrella Corporation' AND [Alias] = 'umbrella')
        BEGIN
            INSERT INTO [Tenants] ([Name], [Alias], [Guid], [CreatedOnUtc], [CreatedBy], [UpdatedOnUtc], [UpdatedBy])
            VALUES ('Umbrella Corporation', 'umbrella', '9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C', GETUTCDATE(), -1, NULL, NULL)
        END
    
        IF NOT EXISTS (SELECT [Name], [Alias] FROM [Tenants] WHERE [Name] = 'Cyberdyne Systems' AND [Alias] = 'cyberdyne')
        BEGIN
            INSERT INTO [Tenants] ([Name], [Alias], [Guid], [CreatedOnUtc], [CreatedBy], [UpdatedOnUtc], [UpdatedBy])
            VALUES ('Cyberdyne Systems', 'cyberdyne', 'F02E8F1F-0BBA-4049-9ED6-902F610DEE95', GETUTCDATE(), -1, NULL, NULL)
        END
        
        IF NOT EXISTS (SELECT [Name], [Alias] FROM [Tenants] WHERE [Name] = 'OCP' AND [Alias] = 'ocp')
        BEGIN
            INSERT INTO [Tenants] ([Name], [Alias], [Guid], [CreatedOnUtc], [CreatedBy], [UpdatedOnUtc], [UpdatedBy])
            VALUES ('OCP', 'ocp', '5D71D117-F481-41B7-BE4A-AF0BB5A8A20E', GETUTCDATE(), -1, NULL, NULL)
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
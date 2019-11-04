IF NOT EXISTS(SELECT * FROM [Information_Schema].[Tables] WHERE Table_Name='Tenants')
BEGIN
    CREATE TABLE Tenants (
        Id INT NOT NULL,
        PRIMARY KEY (Id)
    );
END;
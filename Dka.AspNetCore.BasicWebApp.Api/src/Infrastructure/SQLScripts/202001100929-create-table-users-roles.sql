IF NOT EXISTS(SELECT * FROM [Information_Schema].[Tables] WHERE Table_Name='UserRoles')
    BEGIN
        CREATE TABLE [UserRoles] (
            [UserId] INT NOT NULL,
            [RoleId] INT NOT NULL,            
            CONSTRAINT [FK_UserRoles_Users] FOREIGN KEY ([UserId]) REFERENCES [Users]([Id]) ON DELETE CASCADE,
            CONSTRAINT [FK_UserRoles_Roles] FOREIGN KEY ([RoleId]) REFERENCES [Roles]([Id]) ON DELETE CASCADE,
            CONSTRAINT [PK_UserRoles] PRIMARY KEY CLUSTERED ([UserId], [RoleId]),
            INDEX [IX_UserRoles_UserId] NONCLUSTERED ([UserId]),
            INDEX [IX_UserRoles_RoleId] NONCLUSTERED ([RoleId])                                       
        );
    END;
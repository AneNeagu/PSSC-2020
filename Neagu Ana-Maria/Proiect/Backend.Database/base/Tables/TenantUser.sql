CREATE TABLE [base].[TenantUser] (
    [TenantId] INT              NOT NULL,
    [UserId]   UNIQUEIDENTIFIER NOT NULL,
    [IsActive] BIT              CONSTRAINT DF_TenantUser_ISActive DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_TenantUser] PRIMARY KEY CLUSTERED ([TenantId] ASC, [UserId] ASC),
    CONSTRAINT [FK_TenantUser_Tenant] FOREIGN KEY ([TenantId]) REFERENCES [base].[Tenant] ([TenantId]),
    CONSTRAINT [FK_TenantUser_User] FOREIGN KEY ([UserId]) REFERENCES [base].[user] ([UserId])
);


GO
CREATE NONCLUSTERED INDEX [FK_TenantUser_Tenant]
    ON [base].[TenantUser]([TenantId] ASC);


GO
CREATE NONCLUSTERED INDEX [FK_TenantUser_User]
    ON [base].[TenantUser]([UserId] ASC);


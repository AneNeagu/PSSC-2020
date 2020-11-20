CREATE TABLE [base].[Tag] (
    [TenantId]    INT            NOT NULL,
    [TagId]       INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (100) NOT NULL,
    [Description] CHAR (255)     NULL,
    CONSTRAINT [PK_Tag] PRIMARY KEY CLUSTERED ([TenantId] ASC, [TagId] ASC),
    CONSTRAINT [FK_Tag_Tenant] FOREIGN KEY ([TenantId]) REFERENCES [base].[Tenant] ([TenantId])
);


GO
CREATE NONCLUSTERED INDEX [FK_Tag_Tenant]
    ON [base].[Tag]([TenantId] ASC);
GO

CREATE UNIQUE NONCLUSTERED INDEX UX_base_Tag_TenantName ON base.Tag(TenantId,Name);
GO



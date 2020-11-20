CREATE TABLE [base].[Tenant] (
    [TenantId]       INT              IDENTITY (1, 1) NOT NULL,
    [Name]           NVARCHAR (50)    NOT NULL,
    [Description]    NVARCHAR (255)   NULL,
    [OrganisationId] UNIQUEIDENTIFIER NULL,
    [RowGuid]       UNIQUEIDENTIFIER ROWGUIDCOL NOT NULL CONSTRAINT DF_base_Tenant_RowGuid DEFAULT (NEWID()),
    SysStartTime datetime2 GENERATED ALWAYS AS ROW START NOT NULL, 
    SysEndTime datetime2 GENERATED ALWAYS AS ROW END NOT NULL, 
    PERIOD FOR SYSTEM_TIME (SysStartTime,SysEndTime), 
    [RowVersion] TIMESTAMP NOT NULL, 
    [SubscriptionId] UNIQUEIDENTIFIER NULL, 
    CONSTRAINT [PK_Tenant] PRIMARY KEY CLUSTERED ([TenantId] ASC)
);


GO

CREATE UNIQUE INDEX [UX_Tenant_Name] ON [base].[Tenant] ([Name])

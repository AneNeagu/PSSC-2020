CREATE VIEW [admin].[Tenant]
WITH SCHEMABINDING
AS

SELECT t.TenantId,
       t.Name,
       t.Description,
       t.OrganisationId,
       t.RowGuid,
       t.SysStartTime,
       t.SysEndTime,
       t.RowVersion,
       t.SubscriptionId
FROM
       base.Tenant AS t;


GO

GRANT SELECT ON OBJECT::[admin].[Tenant] TO [Provisioning] AS [dbo];
GO

GRANT UPDATE ON OBJECT::[admin].[Tenant] TO [Provisioning] AS [dbo];
GO

GRANT INSERT ON OBJECT::[admin].[Tenant] TO [Provisioning] AS [dbo];
GO


GRANT SELECT ON OBJECT::[admin].[Tenant] TO [ForumAdmin] AS [dbo];
GO

GRANT UPDATE ON OBJECT::[admin].[Tenant] TO [ForumAdmin] AS [dbo];
GO

GRANT INSERT ON OBJECT::[admin].[Tenant] TO [ForumAdmin] AS [dbo];
GO

GRANT DELETE ON OBJECT::[admin].[Tenant] TO [ForumAdmin] AS [dbo];
GO


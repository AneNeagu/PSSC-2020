CREATE VIEW [admin].[TenantUser]
WITH SCHEMABINDING
AS
SELECT tu.TenantId, tu.UserId, tu.IsActive FROM base.TenantUser AS tu;


Go

GRANT SELECT ON OBJECT::[admin].[TenantUser] TO [Provisioning] AS [dbo];
GO

GRANT DELETE ON OBJECT::[admin].[TenantUser] TO [Provisioning] AS [dbo];
GO

GRANT INSERT ON OBJECT::[admin].[TenantUser] TO [Provisioning] AS [dbo];
GO

GRANT SELECT ON OBJECT::[admin].[TenantUser] TO [ForumAdmin] AS [dbo];
GO


GRANT INSERT ON OBJECT::[admin].[TenantUser] TO [ForumAdmin] AS [dbo];
GO

GRANT DELETE ON OBJECT::[admin].[TenantUser] TO [ForumAdmin] AS [dbo];
GO

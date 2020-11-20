CREATE VIEW [user].[TenantUser]
WITH SCHEMABINDING
AS
SELECT tu.TenantId, tu.UserId, tu.IsActive FROM base.TenantUser AS tu;
    Go

GRANT SELECT ON OBJECT::[user].[TenantUser] TO [ForumAdmin] AS [dbo];
GO

GRANT UPDATE ON OBJECT::[user].[TenantUser] TO [ForumAdmin] AS [dbo];
GO

GRANT INSERT ON OBJECT::[user].[TenantUser] TO [ForumAdmin] AS [dbo];
GO

GRANT DELETE ON OBJECT::[user].[TenantUser] TO [ForumAdmin] AS [dbo];
GO

GRANT SELECT ON OBJECT::[user].[TenantUser] TO [AppUser] AS [dbo];
GO




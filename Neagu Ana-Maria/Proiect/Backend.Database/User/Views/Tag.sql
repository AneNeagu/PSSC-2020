CREATE VIEW [user].[Tag]
WITH SCHEMABINDING
AS
SELECT t.TenantId, t.TagId, t.Name, t.Description FROM base.Tag AS t;
GO
GRANT SELECT ON OBJECT::[user].[Tag] TO [ForumAdmin] AS [dbo];
GO

GRANT UPDATE ON OBJECT::[user].[Tag] TO [ForumAdmin] AS [dbo];
GO

GRANT INSERT ON OBJECT::[user].[Tag] TO [ForumAdmin] AS [dbo];
GO

GRANT DELETE ON OBJECT::[user].[Tag] TO [ForumAdmin] AS [dbo];
GO

GRANT SELECT ON OBJECT::[user].[Tag] TO [AppUser] AS [dbo];
GO


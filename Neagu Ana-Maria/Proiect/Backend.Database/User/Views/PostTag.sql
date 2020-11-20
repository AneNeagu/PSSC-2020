CREATE VIEW [user].[PostTag]
WITH SCHEMABINDING
AS

SELECT pt.TenantId, pt.PostId, pt.TagId FROM base.PostTag AS pt;
    Go

GRANT SELECT ON OBJECT::[user].[PostTag] TO [ForumAdmin] AS [dbo];
GO

GRANT UPDATE ON OBJECT::[user].[PostTag] TO [ForumAdmin] AS [dbo];
GO

GRANT INSERT ON OBJECT::[user].[PostTag] TO [ForumAdmin] AS [dbo];
GO

GRANT DELETE ON OBJECT::[user].[PostTag] TO [ForumAdmin] AS [dbo];
GO

GRANT SELECT ON OBJECT::[user].[PostTag] TO [AppUser] AS [dbo];
GO

GRANT UPDATE ON OBJECT::[user].[PostTag] TO [AppUser] AS [dbo];
GO

GRANT INSERT ON OBJECT::[user].[PostTag] TO [AppUser] AS [dbo];
GO

GRANT DELETE ON OBJECT::[user].[PostTag] TO [AppUser] AS [dbo];
GO


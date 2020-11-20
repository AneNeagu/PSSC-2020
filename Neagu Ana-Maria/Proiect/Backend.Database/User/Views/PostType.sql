CREATE VIEW [user].[PostType]
WITH SCHEMABINDING
AS
SELECT pt.PostTypeId, pt.Name FROM base.PostType AS pt;
GO

GRANT SELECT ON OBJECT::[user].[PostType] TO [ForumAdmin] AS [dbo];
GO

GRANT UPDATE ON OBJECT::[user].[PostType] TO [ForumAdmin] AS [dbo];
GO

GRANT INSERT ON OBJECT::[user].[PostType] TO [ForumAdmin] AS [dbo];
GO

GRANT DELETE ON OBJECT::[user].[PostType] TO [ForumAdmin] AS [dbo];
GO

GRANT SELECT ON OBJECT::[user].[PostType] TO [AppUser] AS [dbo];
GO


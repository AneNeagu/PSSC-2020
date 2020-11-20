CREATE VIEW [user].[PostView]
WITH SCHEMABINDING
AS
SELECT pv.TenantId,
       pv.UserId,
       pv.PostId,
       pv.Viewed
FROM
       base.PostView AS pv;
    Go

GRANT SELECT ON OBJECT::[user].[PostView] TO [ForumAdmin] AS [dbo];
GO

GRANT UPDATE ON OBJECT::[user].[PostView] TO [ForumAdmin] AS [dbo];
GO

GRANT INSERT ON OBJECT::[user].[PostView] TO [ForumAdmin] AS [dbo];
GO

GRANT DELETE ON OBJECT::[user].[PostView] TO [ForumAdmin] AS [dbo];
GO

GRANT SELECT ON OBJECT::[user].[PostView] TO [AppUser] AS [dbo];
GO

GRANT UPDATE ON OBJECT::[user].[PostView] TO [AppUser] AS [dbo];
GO

GRANT INSERT ON OBJECT::[user].[PostView] TO [AppUser] AS [dbo];
GO

GRANT DELETE ON OBJECT::[user].[PostView] TO [AppUser] AS [dbo];
GO


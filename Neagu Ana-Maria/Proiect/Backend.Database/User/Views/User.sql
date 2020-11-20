CREATE VIEW [user].[user]
WITH SCHEMABINDING
AS
SELECT u.UserId,
       u.Name,
       u.LastAccessed,
       u.DisplayName,
       u.WorkspaceId,
       u.Email,
       u.Avatar,
       u.Biography,
       u.IsAdmin,
       u.RowGuid,
       u.SysStartTime,
       u.SysEndTime,
       u.RowVersion
FROM
       base.[user] AS u;
    Go

GRANT SELECT ON OBJECT::[user].[user] TO [ForumAdmin] AS [dbo];
GO

GRANT UPDATE ON OBJECT::[user].[user] TO [ForumAdmin] AS [dbo];
GO

GRANT INSERT ON OBJECT::[user].[user] TO [ForumAdmin] AS [dbo];
GO

GRANT DELETE ON OBJECT::[user].[user] TO [ForumAdmin] AS [dbo];
GO

GRANT SELECT ON OBJECT::[user].[user] TO [AppUser] AS [dbo];
GO

GRANT UPDATE ON OBJECT::[user].[user] TO [AppUser] AS [dbo];
GO


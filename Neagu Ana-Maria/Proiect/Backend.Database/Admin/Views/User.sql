CREATE VIEW [admin].[User]
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
       base.[User] AS u;

Go

GRANT SELECT ON OBJECT::[admin].[User] TO [Provisioning] AS [dbo];
GO

GRANT UPDATE ON OBJECT::[admin].[User] TO [Provisioning] AS [dbo];
GO

GRANT INSERT ON OBJECT::[admin].[User] TO [Provisioning] AS [dbo];
GO

GRANT SELECT ON OBJECT::[admin].[User]TO [ForumAdmin] AS [dbo];
GO

GRANT UPDATE ON OBJECT::[admin].[User] TO [ForumAdmin] AS [dbo];
GO

GRANT INSERT ON OBJECT::[admin].[User] TO [ForumAdmin] AS [dbo];
GO

GRANT DELETE ON OBJECT::[admin].[User] TO [ForumAdmin] AS [dbo];
GO


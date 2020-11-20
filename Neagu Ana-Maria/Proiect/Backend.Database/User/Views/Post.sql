CREATE VIEW [user].[Post]
WITH SCHEMABINDING
AS
SELECT p.TenantId,
       p.PostId,
       p.PostTypeId,
       p.ParentPostId,
       p.Title,
       p.PostText,
       p.PostedBy,
       p.AcceptedAnswer,
       p.DateCreated,
       p.Closed,
       p.ClosedBy,
       p.ClosedDate,
       p.LastUpdatedBy,
       p.RowGuid,
       p.SysStartTime,
       p.SysEndTime,
       p.RowVersion
FROM
       base.Post AS p;

    Go

GRANT SELECT ON OBJECT::[user].[Post] TO [ForumAdmin] AS [dbo];
GO

GRANT UPDATE ON OBJECT::[user].[Post] TO [ForumAdmin] AS [dbo];
GO

GRANT INSERT ON OBJECT::[user].[Post] TO [ForumAdmin] AS [dbo];
GO

GRANT DELETE ON OBJECT::[user].[Post] TO [ForumAdmin] AS [dbo];
GO

GRANT SELECT ON OBJECT::[user].[Post] TO [AppUser] AS [dbo];
GO

GRANT UPDATE ON OBJECT::[user].[Post] TO [AppUser] AS [dbo];
GO

GRANT INSERT ON OBJECT::[user].[Post] TO [AppUser] AS [dbo];
GO

GRANT DELETE ON OBJECT::[user].[Post] TO [AppUser] AS [dbo];
GO


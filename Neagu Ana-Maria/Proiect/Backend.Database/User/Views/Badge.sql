CREATE VIEW [user].[Badge]
WITH SCHEMABINDING
AS
SELECT b.BadgeId,
       b.Name,
       b.Description,
       b.ImageURL,
       b.PointsRequired
FROM
       base.Badge AS b;


GO
GRANT SELECT ON OBJECT::[user].[Badge] TO [ForumAdmin] AS [dbo];
GO

GRANT UPDATE ON OBJECT::[user].[Badge] TO [ForumAdmin] AS [dbo];
GO

GRANT INSERT ON OBJECT::[user].[Badge] TO [ForumAdmin] AS [dbo];
GO

GRANT DELETE ON OBJECT::[user].[Badge] TO [ForumAdmin] AS [dbo];
GO

GRANT SELECT ON OBJECT::[user].[Badge] TO [AppUser] AS [dbo];
GO




CREATE VIEW [user].[UserBadge]
WITH SCHEMABINDING
AS

SELECT ub.TenantId,
       ub.BadgeId,
       ub.UserId,
       ub.DateEarned
FROM
       base.UserBadge AS ub;


    Go

GRANT SELECT ON OBJECT::[user].[UserBadge] TO [ForumAdmin] AS [dbo];
GO

GRANT UPDATE ON OBJECT::[user].[UserBadge] TO [ForumAdmin] AS [dbo];
GO

GRANT INSERT ON OBJECT::[user].[UserBadge] TO [ForumAdmin] AS [dbo];
GO

GRANT DELETE ON OBJECT::[user].[UserBadge] TO [ForumAdmin] AS [dbo];
GO

GRANT SELECT ON OBJECT::[user].[UserBadge] TO [AppUser] AS [dbo];
GO

GRANT UPDATE ON OBJECT::[user].[UserBadge] TO [AppUser] AS [dbo];
GO

GRANT INSERT ON OBJECT::[user].[UserBadge] TO [AppUser] AS [dbo];
GO

GRANT DELETE ON OBJECT::[user].[UserBadge] TO [AppUser] AS [dbo];
GO



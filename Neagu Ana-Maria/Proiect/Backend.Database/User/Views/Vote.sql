CREATE VIEW [user].[Vote]
WITH SCHEMABINDING
AS

SELECT v.TenantId,
       v.QuestionId,
       v.VoteTypeId,
       v.UserId,
       v.DateCreated,
       v.VoteValue
FROM
       base.Vote AS v;
        Go

GRANT SELECT ON OBJECT::[user].[Vote] TO [ForumAdmin] AS [dbo];
GO

GRANT UPDATE ON OBJECT::[user].[Vote] TO [ForumAdmin] AS [dbo];
GO

GRANT INSERT ON OBJECT::[user].[Vote] TO [ForumAdmin] AS [dbo];
GO

GRANT DELETE ON OBJECT::[user].[Vote] TO [ForumAdmin] AS [dbo];
GO

GRANT SELECT ON OBJECT::[user].[Vote] TO [AppUser] AS [dbo];
GO

GRANT UPDATE ON OBJECT::[user].[Vote] TO [AppUser] AS [dbo];
GO

GRANT INSERT ON OBJECT::[user].[Vote] TO [AppUser] AS [dbo];
GO

GRANT DELETE ON OBJECT::[user].[Vote] TO [AppUser] AS [dbo];
GO


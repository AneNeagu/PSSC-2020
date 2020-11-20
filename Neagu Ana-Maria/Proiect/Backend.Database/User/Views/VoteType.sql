CREATE VIEW [user].[VoteType]
WITH SCHEMABINDING
AS

SELECT vt.VoteTypeId,
       vt.Name,
       vt.Description,
       vt.DefaultVoteValue
FROM
       base.VoteType AS vt;
GO
GRANT SELECT ON OBJECT::[user].[VoteType] TO [ForumAdmin] AS [dbo];
GO

GRANT UPDATE ON OBJECT::[user].[VoteType] TO [ForumAdmin] AS [dbo];
GO

GRANT INSERT ON OBJECT::[user].[VoteType] TO [ForumAdmin] AS [dbo];
GO

GRANT DELETE ON OBJECT::[user].[VoteType] TO [ForumAdmin] AS [dbo];
GO

GRANT SELECT ON OBJECT::[user].[VoteType] TO [AppUser] AS [dbo];
GO


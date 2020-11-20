MERGE INTO [User].[PostType] AS Target
USING (VALUES
    (1, 'Question'),
    (2, 'Answer'),
    (3, 'Comment')
)
AS Source ([PostTypeId], [Name])
ON Target.[PostTypeId] = Source.[PostTypeId]
WHEN MATCHED THEN
UPDATE SET
    [Name] = Source.[Name]
WHEN NOT MATCHED BY TARGET THEN
INSERT ([PostTypeId], [Name])
VALUES ([PostTypeId], [Name]);

IF NOT EXISTS ( SELECT * FROM [User].VoteType WHERE VoteTypeId = 1 )
    INSERT INTO [User].VoteType(VoteTypeId, Name, Description, DefaultVoteValue)
    VALUES (1, 'VoteUp', 'Vote the answer up', 20)
GO

IF NOT EXISTS ( SELECT * FROM [User].VoteType WHERE VoteTypeId = 2 )
    INSERT INTO [User].VoteType(VoteTypeId, Name, Description, DefaultVoteValue)
    VALUES (2, 'VoteDown', 'Vote the answer down', -20)
GO
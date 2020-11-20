CREATE TABLE [base].[Vote] (
    [TenantId]    INT              NOT NULL,
    [QuestionId]  INT              NOT NULL,
    [VoteTypeId]  INT              NOT NULL,
    [UserId]      UNIQUEIDENTIFIER NOT NULL,
    [DateCreated] DATETIME         DEFAULT (getdate()) NOT NULL,
    [VoteValue]   INT              DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Vote] PRIMARY KEY CLUSTERED ([TenantId] ASC, [QuestionId] ASC,  [UserId] ASC),
    CONSTRAINT [FK_Vote_Post] FOREIGN KEY ([TenantId], [QuestionId]) REFERENCES [base].[Post] ([TenantId], [PostId]),
    CONSTRAINT [FK_Vote_TenantUser] FOREIGN KEY ([TenantId], [UserId]) REFERENCES [base].[TenantUser] ([TenantId], [UserId]),
    CONSTRAINT [FK_Vote_VoteType] FOREIGN KEY ([VoteTypeId]) REFERENCES [base].[VoteType] ([VoteTypeId])
);


GO
CREATE NONCLUSTERED INDEX [FK_Vote_Post]
    ON [base].[Vote]([QuestionId] ASC, [TenantId] ASC);


GO
CREATE NONCLUSTERED INDEX [FK_Vote_VoteType]
    ON [base].[Vote]([VoteTypeId] ASC);


GO
CREATE NONCLUSTERED INDEX [FK_Vote_TenantUser]
    ON [base].[Vote]([TenantId] ASC, [UserId] ASC);


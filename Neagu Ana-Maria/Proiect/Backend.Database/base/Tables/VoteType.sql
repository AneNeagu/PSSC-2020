CREATE TABLE [base].[VoteType] (
    [VoteTypeId]       INT            NOT NULL,
    [Name]             NVARCHAR (100) NOT NULL,
    [Description]      NVARCHAR (255) NULL,
    [DefaultVoteValue] INT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_VoteType] PRIMARY KEY CLUSTERED ([VoteTypeId] ASC)
);


GO

CREATE UNIQUE INDEX [UX_VoteType_Name] ON [base].[VoteType] ([Name])

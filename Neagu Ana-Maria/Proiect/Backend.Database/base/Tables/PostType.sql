CREATE TABLE [base].[PostType] (
    [PostTypeId] TINYINT       NOT NULL,
    [Name]       NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_PostType] PRIMARY KEY CLUSTERED ([PostTypeId] ASC)
);


GO

CREATE UNIQUE INDEX [UX_base_PostType_Name] ON [base].[PostType] ([Name])

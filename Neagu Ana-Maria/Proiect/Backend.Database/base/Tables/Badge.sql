CREATE TABLE [base].[Badge] (
    [BadgeId]        INT            IDENTITY (1, 1) NOT NULL,
    [Name]           NVARCHAR (100) NOT NULL,
    [Description]    NVARCHAR (MAX) NULL,
    [ImageURL]       NVARCHAR (255) NULL,
    [PointsRequired] INT            NULL,
    CONSTRAINT [PK_base_Badge] PRIMARY KEY CLUSTERED ([BadgeId] ASC)
);


GO

CREATE UNIQUE INDEX [UX_base_Badge_Name] ON [base].[Badge] ([Name])

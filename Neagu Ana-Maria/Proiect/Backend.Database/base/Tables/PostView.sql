CREATE TABLE [base].[PostView] (
    [TenantId]   INT              NOT NULL,
    [UserId]     UNIQUEIDENTIFIER NOT NULL,
    [PostId] INT              NOT NULL,
    [Viewed]     DATETIME         CONSTRAINT DF_PostView_Viewed DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_PostView] PRIMARY KEY CLUSTERED ([TenantId] ASC, [UserId] ASC, [PostId] ASC, [Viewed] ASC),
    CONSTRAINT [FK_PostView_Post] FOREIGN KEY ([TenantId], [PostId]) REFERENCES [base].[Post] ([TenantId], [PostId]),
    CONSTRAINT [FK_PostView_TenantUser] FOREIGN KEY ([TenantId], [UserId]) REFERENCES [base].[TenantUser] ([TenantId], [UserId])
);


GO
CREATE NONCLUSTERED INDEX [FK_PostView_Post]
    ON [base].[PostView]([TenantId] ASC, [PostId] ASC);


GO
CREATE NONCLUSTERED INDEX [FK_PostView_TenantUser]
    ON [base].[PostView]([TenantId] ASC, [UserId] ASC);


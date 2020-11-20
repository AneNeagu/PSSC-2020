CREATE TABLE [base].[PostTag] (
    [TenantId]   INT NOT NULL,
    [PostId] INT NOT NULL,
    [TagId]      INT NOT NULL,
    CONSTRAINT [PK_PostTag] PRIMARY KEY CLUSTERED ([TenantId] ASC, [PostId] ASC, [TagId] ASC),
    CONSTRAINT [FK_PostTag_Post] FOREIGN KEY ([TenantId], [PostId]) REFERENCES [base].[Post] ([TenantId], [PostId]),
    CONSTRAINT [FK_PostTag_Tag] FOREIGN KEY ([TenantId], [TagId]) REFERENCES [base].[Tag] ([TenantId], [TagId])
);


GO
CREATE NONCLUSTERED INDEX [FK_base_PostTag_base_Post]
    ON [base].[PostTag]([TenantId] ASC, [PostId] ASC);


GO
CREATE NONCLUSTERED INDEX [FK_base_PostTag_base_Tag]
    ON [base].[PostTag]([TagId] ASC, [TenantId] ASC);


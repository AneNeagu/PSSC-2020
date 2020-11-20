CREATE TABLE [base].[UserBadge] (
    [TenantId]   INT              NOT NULL,
    [BadgeId]    INT              NOT NULL,
    [UserId]     UNIQUEIDENTIFIER NOT NULL,
    [DateEarned] DATETIME         DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_UserBadge] PRIMARY KEY CLUSTERED ([TenantId] ASC, [BadgeId] ASC, [UserId] ASC),
    CONSTRAINT [FK_UserBadge_Badge] FOREIGN KEY ([BadgeId]) REFERENCES [base].[Badge] ([BadgeId]),
    CONSTRAINT [FK_UserBadge_TenantUser] FOREIGN KEY ([TenantId], [UserId]) REFERENCES [base].[TenantUser] ([TenantId], [UserId])
);


GO
CREATE NONCLUSTERED INDEX [FK_UserBadge_Badge]
    ON [base].[UserBadge]([BadgeId] ASC);


GO
CREATE NONCLUSTERED INDEX [FK_UserBadge_TenantUser]
    ON [base].[UserBadge]([TenantId] ASC, [UserId] ASC);


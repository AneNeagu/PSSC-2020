CREATE TABLE [base].[Post] (
    [TenantId]       INT              NOT NULL,
    [PostId]     INT              IDENTITY (1, 1) NOT NULL,
    [PostTypeId]     TINYINT          NOT NULL,
    [ParentPostId]   INT              NULL,
    [Title]          NVARCHAR (255)   NOT NULL,
    [PostText]       NVARCHAR (MAX)   NOT NULL,
    [PostedBy]       UNIQUEIDENTIFIER NOT NULL,
    [AcceptedAnswer] BIT              DEFAULT ((0)) NOT NULL,
    [DateCreated]    DATETIME         DEFAULT (getdate()) NOT NULL,
    [Closed]         BIT              DEFAULT ((0)) NOT NULL,
    [ClosedBy]       UNIQUEIDENTIFIER NULL,
    [ClosedDate]     DATETIME         NULL,
    [LastUpdatedBy]  UNIQUEIDENTIFIER NULL,
    [RowGuid]       UNIQUEIDENTIFIER ROWGUIDCOL NOT NULL CONSTRAINT DF_base_Post_RowGuid DEFAULT (NEWID()),
    SysStartTime datetime2 GENERATED ALWAYS AS ROW START NOT NULL, 
    SysEndTime datetime2 GENERATED ALWAYS AS ROW END NOT NULL, 
    PERIOD FOR SYSTEM_TIME (SysStartTime,SysEndTime),  
    [RowVersion] TIMESTAMP NOT NULL, 
    CONSTRAINT [PK_Post] PRIMARY KEY CLUSTERED ([TenantId] ASC, [PostId] ASC),
    CONSTRAINT [FK_Post_Post] FOREIGN KEY ([TenantId], [ParentPostId]) REFERENCES [base].[Post] ([TenantId], [PostId]),
    CONSTRAINT [FK_Post_PostType] FOREIGN KEY ([PostTypeId]) REFERENCES [base].[PostType] ([PostTypeId]),
    CONSTRAINT [FK_Post_TenantUser] FOREIGN KEY ([TenantId], [PostedBy]) REFERENCES [base].[TenantUser] ([TenantId], [UserId]),
    CONSTRAINT [FK_Post_TenantUser_LastUpdatedBy] FOREIGN KEY ([TenantId], [LastUpdatedBy]) REFERENCES [base].[TenantUser] ([TenantId], [UserId]),
    CONSTRAINT [FK_Post_TenantUser_ClosedBy] FOREIGN KEY ([TenantId], [ClosedBy]) REFERENCES [base].[TenantUser] ([TenantId], [UserId])
);


GO
CREATE NONCLUSTERED INDEX [FK_Post_TenantUser]
    ON [base].[Post]([TenantId] ASC, [PostedBy] ASC);


GO
CREATE NONCLUSTERED INDEX [FK_Post_Post]
    ON [base].[Post]([ParentPostId] ASC, [TenantId] ASC);


GO
CREATE NONCLUSTERED INDEX [FK_Post_TenantUser8]
    ON [base].[Post]([TenantId] ASC, [ClosedBy] ASC);


GO
CREATE NONCLUSTERED INDEX [FK_Post_PostType]
    ON [base].[Post]([PostTypeId] ASC);


GO
CREATE NONCLUSTERED INDEX [FK_Post_TenantUser15]
    ON [base].[Post]([TenantId] ASC, [LastUpdatedBy] ASC);


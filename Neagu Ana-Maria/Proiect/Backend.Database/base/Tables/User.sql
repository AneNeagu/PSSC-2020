CREATE TABLE [base].[user] (
    [UserId]       UNIQUEIDENTIFIER NOT NULL,
    [Name]         NVARCHAR (200)   NOT NULL,
    [LastAccessed] DATETIME         NULL,
    [DisplayName]  NVARCHAR (200)   NOT NULL,
    [WorkspaceId]  UNIQUEIDENTIFIER NOT NULL,
    [Email]        NVARCHAR (255)   NULL,
    [Avatar]       NVARCHAR (255)   NULL,
    [Biography]    NVARCHAR (MAX)   NULL,
    IsAdmin         BIT NOT NULL CONSTRAINT DF_base_user_isAdmin DEFAULT 0,
    [RowGuid]       UNIQUEIDENTIFIER ROWGUIDCOL NOT NULL CONSTRAINT DF_base_User_RowGuid DEFAULT (NEWID()),
    SysStartTime datetime2 GENERATED ALWAYS AS ROW START NOT NULL, 
    SysEndTime datetime2 GENERATED ALWAYS AS ROW END NOT NULL, 
    PERIOD FOR SYSTEM_TIME (SysStartTime,SysEndTime), 
    [RowVersion] TIMESTAMP NOT NULL, 
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([UserId] ASC)
);


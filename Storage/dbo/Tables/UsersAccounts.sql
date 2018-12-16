CREATE TABLE [dbo].[UserAccounts] (
    [Id]           VARCHAR (128) NOT NULL,
    [Username]     NVARCHAR (MAX) NOT NULL,
	[Email] VARCHAR(80) NOT NULL, 
	[PrefferedLanguage] VARCHAR(8) NOT NULL, 
    [PasswordHash] VARBINARY (256) NOT NULL,
    [Salt]         VARBINARY (24) NOT NULL,
    [Active] BIT NOT NULL , 
    [VerificationToken] UNIQUEIDENTIFIER NULL, 
    CONSTRAINT [PK_UserAccounts] PRIMARY KEY CLUSTERED ([Id] ASC)
);


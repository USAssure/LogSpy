CREATE TABLE [dbo].[Log]
(
	  [Id] bigint identity(1,1) not null primary key
	, [AppName] varchar(50) not null
	, [MachineName] varchar(50) not null
	, [RecordedDate] datetime not null
	, [Level] varchar(10) not null
	, [Type] varchar(50) not null
	, [IpAddress] varchar(40) null
	, [Host] varchar(100) null
	, [Url] varchar(500) null
	, [UserName] varchar(100) null
	, [HttpMethod] varchar(10) null
	, [Message] nvarchar(max) null
	, [Exception] nvarchar(max) null
	, [Payload] nvarchar(max) null
	, [Keep] bit default(0) not null
	, [KeepUser] varchar(100) null
)

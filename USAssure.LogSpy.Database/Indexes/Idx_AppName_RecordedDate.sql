CREATE INDEX [Idx_AppName_RecordedDate]	ON [dbo].[Log]
(
	[AppName] asc,
	[RecordedDate] desc
);
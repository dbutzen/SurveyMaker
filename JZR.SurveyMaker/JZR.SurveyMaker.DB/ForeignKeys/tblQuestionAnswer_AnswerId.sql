ALTER TABLE [dbo].[tblQuestionAnswer]
	ADD CONSTRAINT [tblQuestionAnswer_AnswerId]
	FOREIGN KEY (AnswerId)
	REFERENCES [tblAnswer] (Id)

ALTER TABLE [dbo].[tblQuestionAnswer]
	ADD CONSTRAINT [tblQuestionAnswer_QuestionId]
	FOREIGN KEY (QuestionId)
	REFERENCES [tblQuestion] (Id) ON DELETE CASCADE

﻿CREATE TABLE [dbo].[tblQuestionAnswer]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [AnswerID] UNIQUEIDENTIFIER NOT NULL, 
    [QuestionID] UNIQUEIDENTIFIER NOT NULL, 
    [IsCorrect] BIT NOT NULL,
    UNIQUE ([AnswerID], [QuestionID])
)

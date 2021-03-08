﻿CREATE TABLE [dbo].[tblResponse]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [QuestionId] UNIQUEIDENTIFIER NOT NULL, 
    [AnswerId] UNIQUEIDENTIFIER NOT NULL, 
    [ResponseDate] DATETIME NOT NULL, 
    CONSTRAINT [tblResponse_QuestionId] FOREIGN KEY (QuestionId) REFERENCES tblQuestion(Id), 
    CONSTRAINT [tblResponse_AnswerId] FOREIGN KEY (AnswerId) REFERENCES tblAnswer(Id)
)
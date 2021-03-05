CREATE TABLE [dbo].[tblActivation]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [QuestionId] UNIQUEIDENTIFIER NOT NULL, 
    [StartDate] DATETIME NOT NULL, 
    [EndDate] DATETIME NOT NULL, 
    [ActivationCode] VARCHAR(6) NOT NULL, 
    CONSTRAINT [tblActivation_QuestionId] FOREIGN KEY (QuestionId) REFERENCES [tblQuestion] (Id) ON DELETE CASCADE
)

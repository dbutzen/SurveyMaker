CREATE TABLE [dbo].[tblActivation]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [QuestionId] UNIQUEIDENTIFIER NOT NULL, 
    [StartDate] DATE NOT NULL, 
    [EndDate] DATE NOT NULL, 
    [ActivationCode] VARCHAR(6) NOT NULL, 
    CONSTRAINT [tblActivation_QuestionId] FOREIGN KEY (QuestionId) REFERENCES [tblQuestion] (Id) ON DELETE CASCADE
)

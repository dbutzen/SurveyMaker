BEGIN

/* First */
DECLARE @QuestionIdActivation uniqueidentifier;
Select @QuestionIdActivation from tblQuestion where Question = 'How many rings are on the Olympic flag?'

INSERT INTO dbo.tblActivation (Id, QuestionId, StartDate, EndDate, ActivationCode)
VALUES (NEWID(), @QuestionIdActivation, '2020-09-25', '2021-09-23', '1q2w3e')

/* Second */

SELECT @QuestionIdActivation from tblQuestion where Question = 'Are giant pandas a type of bear?'

INSERT INTO dbo.tblActivation (Id, QuestionId, StartDate, EndDate, ActivationCode)
VALUES(NEWID(), @QuestionIdActivation, '2021-03-01', '2021-03-08', 'q2wer4')

/* Third */

SELECT @QuestionIdActivation from tblQuestion where Question = 'How many holes are on a standard bowling ball?'

INSERT INTO tblActivation (Id, QuestionId, StartDate, EndDate, ActivationCode)
VALUES(NEWID(), @QuestionIdActivation, '2020-07-11', '2021-09-10', 'q2wer4')


END
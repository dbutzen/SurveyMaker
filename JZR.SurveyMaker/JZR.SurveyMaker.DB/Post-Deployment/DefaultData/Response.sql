BEGIN

/* First */
DECLARE @QuestionIdResponse uniqueidentifier;
DECLARE @answerId uniqueidentifier;

Select @QuestionIdResponse from tblQuestion where Question = 'How many rings are on the Olympic flag?'
Select @answerId from tblAnswer where Answer = '5'

INSERT INTO dbo.tblResponse (Id, QuestionId, AnswerId, ResponseDate)
VALUES (NEWID(), @QuestionIdResponse, @answerId, '2021-03-01')

/* Second */
Select @QuestionIdResponse from tblQuestion where Question = 'How many rings are on the Olympic flag?'
Select @answerId from tblAnswer where Answer = 'None'

INSERT INTO dbo.tblResponse (Id, QuestionId, AnswerId, ResponseDate)
VALUES (NEWID(), @QuestionIdResponse, @answerId, '2021-03-02')

/* Third */

Select @QuestionIdResponse from tblQuestion where Question = 'How many holes are on a standard bowling ball?'
Select @answerId from tblAnswer where Answer = '3'

INSERT INTO dbo.tblResponse (Id, QuestionId, AnswerId, ResponseDate)
VALUES (NEWID(), @QuestionIdResponse, @answerId, '2020-09-03')


END
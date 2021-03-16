BEGIN

/* First */
DECLARE @QuestionIdResponse uniqueidentifier;
DECLARE @answerId uniqueidentifier;

Select @QuestionIdResponse = Id from tblQuestion where Question = 'How many rings are on the Olympic flag?'
Select @answerId = Id from tblAnswer where Answer = '5'

INSERT INTO dbo.tblResponse (Id, QuestionId, AnswerId, ResponseDate)
VALUES (NEWID(), @QuestionIdResponse, @answerId, '2021-03-01')

/* Second */
Select @QuestionIdResponse = Id from tblQuestion where Question = 'How many rings are on the Olympic flag?'
Select @answerId = Id from tblAnswer where Answer = 'None'

INSERT INTO dbo.tblResponse (Id, QuestionId, AnswerId, ResponseDate)
VALUES (NEWID(), @QuestionIdResponse, @answerId, '2021-03-02')

/* Third */

Select @QuestionIdResponse = Id from tblQuestion where Question = 'How many holes are on a standard bowling ball?'
Select @answerId = Id from tblAnswer where Answer = '3'

INSERT INTO dbo.tblResponse (Id, QuestionId, AnswerId, ResponseDate)
VALUES (NEWID(), @QuestionIdResponse, @answerId, '2020-09-03')

/* Fourth */

Select @QuestionIdResponse = Id from tblQuestion where Question = 'Are giant pandas a type of bear?'
Select @answerId = Id from tblAnswer where Answer = 'Yes'

INSERT INTO dbo.tblResponse (Id, QuestionId, AnswerId, ResponseDate)
VALUES (NEWID(), @QuestionIdResponse, @answerId, '2021-03-02')

/* Fifth */

Select @QuestionIdResponse = Id from tblQuestion where Question = 'Are giant pandas a type of bear?'
Select @answerId = Id from tblAnswer where Answer = 'No'

INSERT INTO dbo.tblResponse (Id, QuestionId, AnswerId, ResponseDate)
VALUES (NEWID(), @QuestionIdResponse, @answerId, '2021-03-07')

/* Sixth */

Select @QuestionIdResponse = Id from tblQuestion where Question = 'How many holes are on a standard bowling ball?'
Select @answerId = Id from tblAnswer where Answer = '2'

INSERT INTO dbo.tblResponse (Id, QuestionId, AnswerId, ResponseDate)
VALUES (NEWID(), @QuestionIdResponse, @answerId, '2021-09-09')


END
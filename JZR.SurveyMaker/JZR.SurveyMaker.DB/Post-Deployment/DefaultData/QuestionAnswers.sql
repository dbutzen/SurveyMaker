BEGIN
	DECLARE @QuestionId UNIQUEIDENTIFIER;
	DECLARE @Ans_None UNIQUEIDENTIFIER;
	DECLARE @Ans_2 UNIQUEIDENTIFIER;
	DECLARE @Ans_3 UNIQUEIDENTIFIER;
	DECLARE @Ans_4 UNIQUEIDENTIFIER;
	DECLARE @Ans_5 UNIQUEIDENTIFIER;
	DECLARE @Ans_7 UNIQUEIDENTIFIER;
	DECLARE @Ans_Primate UNIQUEIDENTIFIER;
	DECLARE @Ans_Lizard UNIQUEIDENTIFIER;
	DECLARE @Ans_Bird UNIQUEIDENTIFIER;
	DECLARE @Ans_Yes UNIQUEIDENTIFIER;
	DECLARE @Ans_No UNIQUEIDENTIFIER;


	SELECT @Ans_None = Id from tblAnswer WHERE Answer = 'None';
	SELECT @Ans_2 = Id from tblAnswer WHERE Answer = '2';
	SELECT @Ans_3 = Id from tblAnswer WHERE Answer = '3';
	SELECT @Ans_4 = Id from tblAnswer WHERE Answer = '4';
	SELECT @Ans_5 = Id from tblAnswer WHERE Answer = '5';
	SELECT @Ans_7 = Id from tblAnswer WHERE Answer = '7';
	SELECT @Ans_Primate = Id from tblAnswer WHERE Answer = 'A primate';
	SELECT @Ans_Lizard = Id from tblAnswer WHERE Answer = 'A lizard';
	SELECT @Ans_Bird = Id from tblAnswer WHERE Answer = 'A bird';
	SELECT @Ans_Yes = Id from tblAnswer WHERE Answer = 'Yes';
	SELECT @Ans_No = Id from tblAnswer WHERE Answer = 'No';


	-- How many rings are on the Olympic flag?
	SELECT @QuestionId = Id FROM tblQuestion WHERE Question = 'How many rings are on the Olympic flag?'

	INSERT INTO tblQuestionAnswer
		(ID, QuestionID, AnswerID, IsCorrect)
	VALUES
		(NEWID(), @QuestionId, @Ans_None, 0),
		(NEWID(), @QuestionId, @Ans_4, 0),
		(NEWID(), @QuestionId, @Ans_5, 1),
		(NEWID(), @QuestionId, @Ans_7, 0);

	-- What is a tarsier?
	SELECT @QuestionId = Id FROM tblQuestion WHERE Question = 'What is a tarsier?'

	INSERT INTO tblQuestionAnswer
		(ID, QuestionID, AnswerID, IsCorrect)
	VALUES
		(NEWID(), @QuestionId, @Ans_Primate, 1),
		(NEWID(), @QuestionId, @Ans_Lizard, 0),
		(NEWID(), @QuestionId, @Ans_Bird, 0);


	-- Would a Catholic living in the United States ever celebrate Easter in May?
	SELECT @QuestionId = Id FROM tblQuestion WHERE Question = 'Would a Catholic living in the United States ever celebrate Easter in May?'

	INSERT INTO tblQuestionAnswer
		(ID, QuestionID, AnswerID, IsCorrect)
	VALUES
		(NEWID(), @QuestionId, @Ans_Yes, 0),
		(NEWID(), @QuestionId, @Ans_No, 1);

	-- How many holes are on a standard bowling ball?
	SELECT @QuestionId = Id FROM tblQuestion WHERE Question = 'How many holes are on a standard bowling ball?'

	INSERT INTO tblQuestionAnswer
		(ID, QuestionID, AnswerID, IsCorrect)
	VALUES
		(NEWID(), @QuestionId, @Ans_3, 1),
		(NEWID(), @QuestionId, @Ans_7, 0),
		(NEWID(), @QuestionId, @Ans_2, 0),
		(NEWID(), @QuestionId, @Ans_5, 0);



	-- Are giant pandas a type of bear?
	SELECT @QuestionId = Id FROM tblQuestion WHERE Question = 'Are giant pandas a type of bear?'

	INSERT INTO tblQuestionAnswer
		(ID, QuestionID, AnswerID, IsCorrect)
	VALUES
		(NEWID(), @QuestionId, @Ans_Yes, 1),
		(NEWID(), @QuestionId, @Ans_No, 0)

END
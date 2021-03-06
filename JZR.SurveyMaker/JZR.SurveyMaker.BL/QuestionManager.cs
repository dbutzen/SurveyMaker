using JZR.SurveyMaker.BL.Models;
using JZR.SurveyMaker.PL;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JZR.SurveyMaker.BL
{
    public static class QuestionManager
    {
        public async static Task<int> Insert(Question question, bool rollback = false)
        {
            try
            {
                int results = 0;

                await Task.Run(() =>
                {
                IDbContextTransaction transaction = null;

                    using (SurveyEntities dc = new SurveyEntities())
                    {
                        if (rollback) transaction = dc.Database.BeginTransaction();

                        tblQuestion newrow = new tblQuestion();

                        newrow.Id = Guid.NewGuid();
                        newrow.Question = question.Text;

                        question.Id = newrow.Id;

                        dc.tblQuestions.Add(newrow);

                        results = dc.SaveChanges();

                        if (rollback) transaction.Rollback();
                    }
                });

                return results;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async static Task<Guid> Insert(string questionText, bool rollback = false)
        {
            try
            {
                Question question = new Question { Text = questionText };
                await Insert(question);
                return question.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async static Task<int> Delete(Guid id, bool rollback = false)
        {
            try
            {
                int results = 0;
                await Task.Run(() =>
                {
                    IDbContextTransaction transaction = null;
                    using (SurveyEntities dc = new SurveyEntities())
                    {
                        tblQuestion row = dc.tblQuestions.FirstOrDefault(q => q.Id == id);

                        if (row != null)
                        {
                            if (rollback) transaction = dc.Database.BeginTransaction();

                            dc.tblQuestions.Remove(row);

                            results = dc.SaveChanges();
                            if (rollback) transaction.Rollback();
                        }
                        else
                        {
                            throw new Exception("Row was not found");
                        }
                    }
                });
                return results;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async static Task<int> Update(Question question, bool rollback = false)
        {
            try
            {
                int results = 0;
                await Task.Run(() =>
                {
                    IDbContextTransaction transaction = null;
                    using (SurveyEntities dc = new SurveyEntities())
                    {
                        tblQuestion row = dc.tblQuestions.FirstOrDefault(q => q.Id == question.Id);

                        if (row != null)
                        {
                            if (rollback) transaction = dc.Database.BeginTransaction();

                            row.Question = question.Text;

                            results = dc.SaveChanges();

                            if (rollback) transaction.Rollback();
                        }
                        else
                        {
                            throw new Exception("Row was not found");
                        }
                    }
                });
                return results;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async static Task<Question> LoadById(Guid id)
        {
            try
            {
                Question question = new Question();
                await Task.Run(() =>
                {
                    using (SurveyEntities dc = new SurveyEntities())
                    {
                        tblQuestion tblQuestion = dc.tblQuestions.FirstOrDefault(q => q.Id == id);
                        

                        if (tblQuestion != null)
                        {
                            question.Id = tblQuestion.Id;
                            question.Text = tblQuestion.Question;

                            
                        }
                        else
                        {
                            throw new Exception("Could not find the row.");
                        }
                    }
                });
                return question;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async static Task<List<Question>> Load()
        {
            try
            {
                List<Question> questions = new List<Question>();
                await Task.Run(() =>
                {
                    using (SurveyEntities dc = new SurveyEntities())
                    {
                        dc.tblQuestions.OrderBy(q => q.Question)
                        .ToList().ForEach(q =>
                        {
                            var question = new Question { Id = q.Id, Text = q.Question };
                            question.Answers = new List<Answer>();
                            q.TblQuestionAnswers.ToList().ForEach(qa => question.Answers.Add(new Answer
                            {
                                Id = qa.AnswerId,
                                IsCorrect = qa.IsCorrect,
                                Text = qa.Answer.Answer
                            }));

                            questions.Add(question);

                        });
                    }
                });
                return questions;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async static Task<Question> LoadByText(string questionText)
        {
            try
            {
                Question question = new Question();
                await Task.Run(() =>
                {
                    using (SurveyEntities dc = new SurveyEntities())
                    {
                        tblQuestion tblQuestion = dc.tblQuestions.FirstOrDefault(q => q.Question == questionText);

                        if (tblQuestion != null)
                        {
                            question.Id = tblQuestion.Id;
                            question.Text = tblQuestion.Question;

                        }
                        else
                        {
                            throw new Exception("Could not find the row.");
                        }
                    }
                });
                return question;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

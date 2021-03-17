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

                            question.Answers = new List<Answer>();
                            tblQuestion.TblQuestionAnswers.ToList().ForEach(qa => question.Answers.Add(new Answer
                            {
                                Id = qa.Id,
                                Text = qa.Answer.Answer,
                                IsCorrect = qa.IsCorrect
                            }));


                            question.Activations = new List<Activation>();

                            tblQuestion.TblActivations.ToList().ForEach(a => question.Activations.Add(new Activation
                            {
                                Id = a.Id,
                                QuestionId = a.QuestionId,
                                StartDate = a.StartDate,
                                EndDate = a.EndDate,
                                ActivationCode = a.ActivationCode
                            }));
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

                            question.Activations = new List<Activation>();
                            q.TblActivations.ToList().ForEach(a => question.Activations.Add(new Activation
                            {
                                Id = a.Id,
                                QuestionId = a.QuestionId,
                                StartDate = a.StartDate,
                                EndDate = a.EndDate,
                                ActivationCode = a.ActivationCode
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

        public async static Task<Question> LoadByActivationCode(string activationCode)
        {
            try
            {
                Question question = new Question();

                await Task.Run(() =>
                {
                    using (SurveyEntities dc = new SurveyEntities())
                    {

                        var activation = dc.tblActivations.FirstOrDefault(a => a.ActivationCode == activationCode);

                        if (activation != null)
                        {
                            if (DateTime.Today >= activation.StartDate && DateTime.Today <= activation.EndDate)
                            {
                                var row = (from a in dc.tblActivations
                                           join q in dc.tblQuestions on a.QuestionId equals q.Id
                                           where a.ActivationCode == activation.ActivationCode
                                           select q).FirstOrDefault();

                                if (row != null)
                                {
                                    question.Id = row.Id;
                                    question.Text = row.Question;

                                    question.Activations = new List<Activation>();
                                    var activationRow = row.TblActivations.FirstOrDefault(a => a.QuestionId == question.Id && a.ActivationCode == activationCode);
                                    question.Activations.Add(new Activation
                                    { 
                                        Id = activationRow.Id,
                                        QuestionId = activationRow.QuestionId,
                                        StartDate = activationRow.StartDate,
                                        EndDate = activationRow.EndDate,
                                        ActivationCode = activationRow.ActivationCode
                                    });

                                    question.Answers = new List<Answer>();
                                    row.TblQuestionAnswers.ToList().ForEach(qa => question.Answers.Add(new Answer
                                    {
                                        Id = qa.Answer.Id,
                                        Text = qa.Answer.Answer,
                                        IsCorrect = qa.IsCorrect
                                    }));
                                }
                                else
                                {
                                    throw new Exception("Question could not be found.");
                                }
                            }
                            else if (DateTime.Today < activation.StartDate)
                            {
                                throw new Exception("Activation code is not active.");
                            }
                            else
                                throw new Exception("Activation code is expired.");
                        }
                        else
                        {
                            throw new Exception("Activation code is not valid.");
                        }
                    };

                });

                return question;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

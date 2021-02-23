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
                IDbContextTransaction transaction = null;

                using (SurveyEntities dc = new SurveyEntities())
                {
                    if (rollback) transaction = dc.Database.BeginTransaction();

                    tblQuestion newrow = new tblQuestion();

                    newrow.Id = Guid.NewGuid();
                    newrow.Question = question.Text;

                    question.Id = newrow.Id;

                    dc.tblQuestions.Add(newrow);

                    int results = dc.SaveChanges();

                    if (rollback) transaction.Rollback();

                    return results;
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async static Task<int> Delete(Guid id, bool rollback = false)
        {
            try
            {
                IDbContextTransaction transaction = null;
                using (SurveyEntities dc = new SurveyEntities())
                {
                    tblQuestion row = dc.tblQuestions.FirstOrDefault(q => q.Id == id);
                    int results = 0;

                    if (row != null)
                    {
                        if (rollback) transaction = dc.Database.BeginTransaction();

                        dc.tblQuestions.Remove(row);

                        results = dc.SaveChanges();
                        if (rollback) transaction.Rollback();

                        return results;
                    }
                    else
                    {
                        throw new Exception("Row was not found");
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async static Task<int> Update(Question question, bool rollback = false)
        {
            try
            {
                IDbContextTransaction transaction = null;
                using (SurveyEntities dc = new SurveyEntities())
                {
                    tblQuestion row = dc.tblQuestions.FirstOrDefault(q => q.Id == question.Id);
                    int results = 0;
                    if (row != null)
                    {
                        if (rollback) transaction = dc.Database.BeginTransaction();

                        row.Question = question.Text;

                        results = dc.SaveChanges();

                        if (rollback) transaction.Rollback();

                        return results;

                    }
                    else
                    {
                        throw new Exception("Row was not found");
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async static Task<Question> LoadById(Guid id)
        {
            try
            {
                using (SurveyEntities dc = new SurveyEntities())
                {
                    tblQuestion tblQuestion = dc.tblQuestions.FirstOrDefault(q => q.Id == id);
                    Question question = new Question();

                    if (tblQuestion != null)
                    {
                        question.Id = tblQuestion.Id;
                        question.Text = tblQuestion.Question;

                        return question;
                    }
                    else
                    {
                        throw new Exception("Could not find the row.");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async static Task<IEnumerable<Question>> Load()
        {
            try
            {
                List<Question> questions = new List<Question>();

                using (SurveyEntities dc = new SurveyEntities())
                {
                    dc.tblQuestions
                        .OrderBy(q => q.Question)
                        .ToList()
                        .ForEach(async q => questions.Add(new Question
                        {
                            Id = q.Id,
                            Text = q.Question,
                            Answers = (List<Answer>)await AnswerManager.Load(q.Id)
                        }));

                    return questions;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async static Task<Question> LoadByText(string questionText)
        {
            try
            {
                using (SurveyEntities dc = new SurveyEntities())
                {
                    tblQuestion tblQuestion = dc.tblQuestions.FirstOrDefault(q => q.Question == questionText);
                    Question question = new Question();
                    if (tblQuestion != null)
                    {
                        question.Id = tblQuestion.Id;
                        question.Text = tblQuestion.Question;
                        return question;
                    }
                    else
                    {
                        throw new Exception("Could not find the row.");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

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
    public static class QuestionAnswerManager
    {
        public async static Task<int> Insert(Question question, bool rollback = false)
        {
            try
            {
                int results = 0;
                await Task.Run(() =>
                {
                    try
                    {
                        IDbContextTransaction transaction = null;

                        using (SurveyEntities dc = new SurveyEntities())
                        {
                            if (rollback) transaction = dc.Database.BeginTransaction();

                            question.Answers.ForEach(a =>
                            {
                                tblQuestionAnswer newrow = new tblQuestionAnswer();
                                newrow.Id = Guid.NewGuid();
                                newrow.AnswerId = a.Id;
                                newrow.QuestionId = question.Id;
                                newrow.IsCorrect = a.IsCorrect;
                                var row = dc.tblQuestionAnswers.FirstOrDefault(qa => qa.QuestionId == question.Id && qa.AnswerId == a.Id);
                                dc.tblQuestionAnswers.Add(newrow);

                            });

                            results = dc.SaveChanges();
                            if (rollback) transaction.Rollback();
                        }
                    }
                    catch (Exception)
                    {

                        results = -1;
                    }
                });

                return results;
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
                        tblQuestionAnswer row = dc.tblQuestionAnswers.FirstOrDefault(qa => qa.Id == id);

                        if (row != null)
                        {
                            if (rollback) transaction = dc.Database.BeginTransaction();

                            dc.tblQuestionAnswers.Remove(row);

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

        public async static Task<int> Delete(Guid questionId, Guid answerId, bool rollback = false)
        {
            try
            {
                int results = 0;
                await Task.Run(() =>
                {
                    IDbContextTransaction transaction = null;
                    using (SurveyEntities dc = new SurveyEntities())
                    {
                        tblQuestionAnswer row = dc.tblQuestionAnswers.FirstOrDefault(qa => qa.QuestionId == questionId && qa.AnswerId == answerId);



                        if (row != null)
                        {
                            if (rollback) transaction = dc.Database.BeginTransaction();

                            dc.tblQuestionAnswers.Remove(row);

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

        public async static Task<int> DeleteByQuestionId(Guid questionId, bool rollback = false)
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
                        var rows = dc.tblQuestionAnswers.Where(qa => qa.QuestionId == questionId);
                        rows.ToList().ForEach(r =>
                        {
                            dc.tblQuestionAnswers.Remove(r);
                        });

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

    }
}

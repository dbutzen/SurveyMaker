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
                IDbContextTransaction transaction = null;

                using (SurveyEntities dc = new SurveyEntities())
                {
                    if (rollback) transaction = dc.Database.BeginTransaction();
                    int results = 0;
                    question.Answers.ForEach(a => {
                        tblQuestionAnswer newrow = new tblQuestionAnswer();
                        newrow.Id = new Guid();
                        newrow.AnswerId = a.Id;
                        newrow.QuestionId = question.Id;
                        newrow.IsCorrect = a.IsCorrect;
                        dc.tblQuestionAnswers.Add(newrow);
                        results += dc.SaveChanges();
                    });

                    if (rollback) transaction.Rollback();

                    return results;
                }
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
                    tblQuestionAnswer row = dc.tblQuestionAnswers.FirstOrDefault(qa => qa.Id == id);
                    
                    int results = 0;

                    if (row != null)
                    {
                        if (rollback) transaction = dc.Database.BeginTransaction();

                        dc.tblQuestionAnswers.Remove(row);

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

        public async static Task<int> Delete(Guid questionId, Guid answerId, bool rollback = false)
        {
            try
            {
                IDbContextTransaction transaction = null;
                using (SurveyEntities dc = new SurveyEntities())
                {
                    tblQuestionAnswer row = dc.tblQuestionAnswers.FirstOrDefault(qa => qa.QuestionId == questionId && qa.AnswerId == answerId);

                    int results = 0;

                    if (row != null)
                    {
                        if (rollback) transaction = dc.Database.BeginTransaction();

                        dc.tblQuestionAnswers.Remove(row);

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
    }
}

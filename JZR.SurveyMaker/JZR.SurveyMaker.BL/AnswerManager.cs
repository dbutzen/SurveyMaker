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
    public static class AnswerManager
    {
        public async static Task<int> Insert(Answer answer, bool rollback = false)
        {
            try
            {
                IDbContextTransaction transaction = null;

                using (SurveyEntities dc = new SurveyEntities())
                {
                    if (rollback) transaction = dc.Database.BeginTransaction();

                    tblAnswer newrow = new tblAnswer();

                    newrow.Id = Guid.NewGuid();
                    newrow.Answer = answer.Text;

                    answer.Id = newrow.Id;

                    dc.tblAnswers.Add(newrow);

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

        public async static Task<Guid> Insert(string answerText, bool rollback = false)
        {
            try
            {
                Answer answer = new Answer { Text = answerText};
                await Insert(answer);
                return answer.Id;
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
                    tblAnswer row = dc.tblAnswers.FirstOrDefault(a => a.Id == id);
                    int results = 0;

                    if (row != null)
                    {
                        if (rollback) transaction = dc.Database.BeginTransaction();

                        dc.tblAnswers.Remove(row);

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

        public async static Task<int> Update(Answer answer, bool rollback = false)
        {
            try
            {
                IDbContextTransaction transaction = null;
                using (SurveyEntities dc = new SurveyEntities())
                {
                    tblAnswer row = dc.tblAnswers.FirstOrDefault(a => a.Id == answer.Id);
                    int results = 0;
                    if (row != null)
                    {
                        if (rollback) transaction = dc.Database.BeginTransaction();

                        row.Answer = answer.Text;

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

        public async static Task<Answer> LoadById(Guid id)
        {
            try
            {
                using (SurveyEntities dc = new SurveyEntities())
                {
                    tblAnswer tblAnswer = dc.tblAnswers.FirstOrDefault(q => q.Id == id);
                    Answer answer = new Answer();

                    if (tblAnswer != null)
                    {
                        answer.Id = tblAnswer.Id;
                        answer.Text = tblAnswer.Answer;

                        return answer;
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

        public async static Task<IEnumerable<Answer>> Load()
        {
            try
            {
                List<Answer> answers = new List<Answer>();

                using (SurveyEntities dc = new SurveyEntities())
                {
                    dc.tblAnswers
                        .OrderBy(a => a.Answer)
                        .ToList()
                        .ForEach(a => answers.Add(new Answer
                        {
                            Id = a.Id,
                            Text = a.Answer
                        }));

                    return answers;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async static Task<IEnumerable<Answer>> Load(Guid questionId)
        {
            try
            {
                List<Answer> answers = new List<Answer>();

                using (SurveyEntities dc = new SurveyEntities())
                {
                    var questionAnswers = dc.tblQuestionAnswers.ToList().Where(qa => qa.QuestionId == questionId);
                    questionAnswers.ToList().ForEach(qa => answers.Add(new Answer
                    {
                        Id = qa.AnswerId,
                        Text = qa.Answer.Answer,
                        IsCorrect = qa.IsCorrect
                    }));
                    return answers;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async static Task<Answer> LoadByText(string answerText)
        {
            try
            {
                using (SurveyEntities dc = new SurveyEntities())
                {
                    tblAnswer tblAnswer = dc.tblAnswers.FirstOrDefault(a => a.Answer == answerText);
                    Answer answer = new Answer();
                    if (tblAnswer != null)
                    {
                        answer.Id = tblAnswer.Id;
                        answer.Text = tblAnswer.Answer;
                        return answer;
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

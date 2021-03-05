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
                int results = 0;
                await Task.Run(() =>
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

        public async static Task<Guid> Insert(string answerText, bool rollback = false)
        {
            try
            {
                Answer answer = new Answer { Text = answerText};
                await Insert(answer, rollback);
                return answer.Id;
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
                        tblAnswer row = dc.tblAnswers.FirstOrDefault(a => a.Id == id);
                        
                        if (row != null)
                        {
                            if (rollback) transaction = dc.Database.BeginTransaction();
                            dc.tblAnswers.Remove(row);
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

        public async static Task<int> Update(Answer answer, bool rollback = false)
        {
            try
            {
                int results = 0;
                await Task.Run(() =>
                {
                    IDbContextTransaction transaction = null;
                    using (SurveyEntities dc = new SurveyEntities())
                    {
                        tblAnswer row = dc.tblAnswers.FirstOrDefault(a => a.Id == answer.Id);
                        
                        if (row != null)
                        {
                            if (rollback) transaction = dc.Database.BeginTransaction();

                            row.Answer = answer.Text;

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

        public async static Task<Answer> LoadById(Guid id)
        {
            try
            {
                Answer answer = new Answer();
                await Task.Run(() =>
                {
                    using (SurveyEntities dc = new SurveyEntities())
                    {
                        tblAnswer tblAnswer = dc.tblAnswers.FirstOrDefault(q => q.Id == id);
                        

                        if (tblAnswer != null)
                        {
                            answer.Id = tblAnswer.Id;
                            answer.Text = tblAnswer.Answer;

                        }
                        else
                        {
                            throw new Exception("Could not find the row.");
                        }
                    }
                });

                return answer;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async static Task<List<Answer>> Load()
        {
            try
            {
                List<Answer> answers = new List<Answer>();
                await Task.Run(() =>
                {
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
                    }
                });
                return answers;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async static Task<IEnumerable<Answer>> Load(Guid questionId)
        {
            try
            {
                List<Answer> answers = new List<Answer>();
                await Task.Run(() =>
                {


                    using (SurveyEntities dc = new SurveyEntities())
                    {
                        var questionAnswers = dc.tblQuestionAnswers.ToList().Where(qa => qa.QuestionId == questionId);
                        questionAnswers.ToList().ForEach(qa => answers.Add(new Answer
                        {
                            Id = qa.AnswerId,
                            Text = qa.Answer.Answer,
                            IsCorrect = qa.IsCorrect
                        }));

                    }
                });

                return answers;
            }
                
            catch (Exception)
            {
                throw;
            }
        }

        public async static Task<Answer> LoadByText(string answerText)
        {
            try
            {
                Answer answer = new Answer();
                await Task.Run(() =>
                {
                    using (SurveyEntities dc = new SurveyEntities())
                    {
                        tblAnswer tblAnswer = dc.tblAnswers.FirstOrDefault(a => a.Answer == answerText);
                        
                        if (tblAnswer != null)
                        {
                            answer.Id = tblAnswer.Id;
                            answer.Text = tblAnswer.Answer;
                            
                        }
                        else
                        {
                            throw new Exception("Could not find the row.");
                        }
                    }
                });
                return answer;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

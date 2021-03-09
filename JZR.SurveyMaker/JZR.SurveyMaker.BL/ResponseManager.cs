using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JZR.SurveyMaker.BL.Models;
using JZR.SurveyMaker.PL;
using Microsoft.EntityFrameworkCore.Storage;

namespace JZR.SurveyMaker.BL
{
    public static class ResponseManager
    {
        public async static Task<int> Insert(Response response, bool rollback = false)
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

                        tblResponse newrow = new tblResponse();

                        newrow.Id = Guid.NewGuid();
                        newrow.QuestionId = response.QuestionId;
                        newrow.AnswerId = response.AnswerId;
                        newrow.ResponseDate = response.ResponseDate;

                        response.Id = newrow.Id;

                        dc.tblResponses.Add(newrow);

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

        public async static Task<Guid> Insert(DateTime responseDate, bool rollback = false)
        {
            try
            {
                Response response = new Response { ResponseDate = responseDate};
                await Insert(response, rollback);
                return response.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async static Task<IEnumerable<Response>> LoadByQuestionId(Guid id)
        {
            try
            {
                List<Response> responses = new List<Response>();

                await Task.Run(() =>
                {
                    using (SurveyEntities dc = new SurveyEntities())
                    {
                        var questionResponses = dc.tblResponses.ToList().Where(r => r.QuestionId == id);
                        questionResponses.ToList().ForEach(r => responses.Add(new Response
                        {
                            Id = r.Id,
                            QuestionId = r.QuestionId,
                            AnswerId = r.AnswerId,
                            ResponseDate = r.ResponseDate
                        }));
                    }
                });

                return responses;

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

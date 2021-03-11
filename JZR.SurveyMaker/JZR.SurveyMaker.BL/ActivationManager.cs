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
    public static class ActivationManager
    {
        public async static Task<int> Insert(Activation activation, bool rollback = false)
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

                            tblActivation newrow = new tblActivation();

                            newrow.Id = Guid.NewGuid();
                            newrow.QuestionId = activation.QuestionId;
                            newrow.StartDate = activation.StartDate;
                            newrow.EndDate = activation.EndDate;
                            newrow.ActivationCode = activation.ActivationCode;

                            activation.Id = newrow.Id;

                            dc.tblActivations.Add(newrow);
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async static Task<Guid> Insert(string activationCode, bool rollback = false)
        {
            try
            {
                Activation activation = new Activation { ActivationCode = activationCode };
                await Insert(activation, rollback);
                return activation.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async static Task<IEnumerable<Activation>> Load()
        {
            try
            {
                List<Activation> activations = new List<Activation>();

                await Task.Run(() =>
                {
                    using (SurveyEntities dc = new SurveyEntities())
                    {
                        dc.tblActivations
                            .ToList()
                            .ForEach(a => activations.Add(new Activation
                            {
                                Id = a.Id,
                                QuestionId = a.QuestionId,
                                StartDate = a.StartDate,
                                EndDate = a.EndDate,
                                ActivationCode = a.ActivationCode,
                            }));
                    }
                });
                return activations.OrderBy(a => a.StartDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async static Task<int> Update(Activation activation, bool rollback = false)
        {
            try
            {
                int results = 0;
                await Task.Run(() =>
                {
                    IDbContextTransaction transaction = null;
                    using (SurveyEntities dc = new SurveyEntities())
                    {
                        tblActivation row = dc.tblActivations.FirstOrDefault(a => a.Id == activation.Id);
                        
                        if (row != null)
                        {
                            if (rollback) transaction = dc.Database.BeginTransaction();

                            row.QuestionId = activation.QuestionId;
                            row.StartDate = activation.StartDate;
                            row.EndDate = activation.EndDate;
                            row.ActivationCode = activation.ActivationCode;

                            results = dc.SaveChanges();
                            if (rollback) transaction.Rollback();
                            
                        }
                        else
                        {
                            throw new Exception("Row was not found.");
                        }
                    }
                });
                return results;
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
                int results = 0;
                await Task.Run(() =>
                {
                    IDbContextTransaction transaction = null;
                    using (SurveyEntities dc = new SurveyEntities())
                    {
                        tblActivation row = dc.tblActivations.FirstOrDefault(a => a.Id == id);

                        if (row != null)
                        {
                            if (rollback) transaction = dc.Database.BeginTransaction();

                            dc.tblActivations.Remove(row);

                            results = dc.SaveChanges();
                            if (rollback) transaction.Rollback();

                        }
                        else
                        {
                            throw new Exception("Row was not found.");
                        }
                    }
                });
                return results;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

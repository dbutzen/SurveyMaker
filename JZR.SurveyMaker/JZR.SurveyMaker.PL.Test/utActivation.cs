using JZR.SurveyMaker.PL;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JZR.VehicleTracker.PL.Test
{
    [TestClass]
    public class utActivation
    {
        protected SurveyEntities dc;
        protected IDbContextTransaction transaction;

        [TestInitialize]
        public void TestInitialize()
        {
            dc = new SurveyEntities();
            transaction = dc.Database.BeginTransaction();
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            transaction.Rollback();
            transaction.Dispose();
        }


        [TestMethod]
        public void LoadTest()
        {
            Assert.AreEqual(3, dc.tblActivations.Count());
        }

        [TestMethod]
        public void InsertTest()
        {

            tblActivation newrow = new tblActivation();
            newrow.Id = Guid.NewGuid();
            newrow.QuestionId = dc.tblQuestions.FirstOrDefault(q => q.Question.Contains("tarsier")).Id;
            newrow.StartDate = DateTime.Now;
            newrow.EndDate = DateTime.Now;
            newrow.ActivationCode = "123456";
            dc.tblActivations.Add(newrow);
            int result = dc.SaveChanges();

            Assert.IsTrue(result > 0);

        }

        [TestMethod]
        public void UpdateTest()
        {
            InsertTest();

            var questionId = dc.tblQuestions.FirstOrDefault(q => q.Question.Contains("tarsier")).Id;

            tblActivation existingrow = dc.tblActivations.FirstOrDefault(q => q.QuestionId == questionId);
            Guid id = existingrow.Id;
            if (existingrow != null)
            {
                existingrow.StartDate = new DateTime(2000, 1, 1);
                dc.SaveChanges();
            }

            tblActivation row = dc.tblActivations.FirstOrDefault(q => q.Id == id);

            Assert.AreEqual(existingrow.QuestionId, row.QuestionId);

        }

        [TestMethod]
        public void DeleteTest()
        {

            InsertTest();


            var questionId = dc.tblQuestions.FirstOrDefault(q => q.Question.Contains("tarsier")).Id;
            tblActivation row = dc.tblActivations.FirstOrDefault(q => q.QuestionId == questionId);

            if (row != null)
            {
                dc.tblActivations.Remove(row);
                dc.SaveChanges();
            }

            tblActivation deletedrow = dc.tblActivations.FirstOrDefault(q => q.QuestionId == questionId);

            Assert.IsNull(deletedrow);
        }
    }
}
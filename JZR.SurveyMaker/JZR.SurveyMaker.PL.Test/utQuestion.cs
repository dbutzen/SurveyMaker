using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JZR.VehicleTracker.PL;
using System.Linq;
using Microsoft.EntityFrameworkCore.Storage;
using JZR.SurveyMaker.PL;

namespace JZR.VehicleTracker.PL.Test
{
    [TestClass]
    public class utQuestion
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
            Assert.AreEqual(5, dc.tblQuestions.Count());
        }

        [TestMethod]
        public void InsertTest()
        {

            tblQuestion newrow = new tblQuestion();
            newrow.Id = Guid.NewGuid();
            newrow.Question = "NewQuestion";

            dc.tblQuestions.Add(newrow);
            int result = dc.SaveChanges();

            Assert.IsTrue(result > 0);

        }

        [TestMethod]
        public void UpdateTest()
        {
            InsertTest();

            tblQuestion existingrow = dc.tblQuestions.FirstOrDefault(q => q.Question.Contains("tarsier"));
            Guid id = existingrow.Id;
            if (existingrow != null)
            {
                existingrow.Question = "UpdatedQuestion";
                dc.SaveChanges();
            }

            tblQuestion row = dc.tblQuestions.FirstOrDefault(q => q.Id == id);

            Assert.AreEqual(existingrow.Question, row.Question);

        }

        [TestMethod]
        public void DeleteTest()
        {

            InsertTest();

            tblQuestion row = dc.tblQuestions.FirstOrDefault(q => q.Question == "NewQuestion");
            if (row != null)
            {
                dc.tblQuestions.Remove(row);
                dc.SaveChanges();
            }

            tblQuestion deletedrow = dc.tblQuestions.FirstOrDefault(q => q.Question == "NewQuestion");

            Assert.IsNull(deletedrow);
        }
    }
}


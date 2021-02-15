using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JZR.VehicleTracker.PL;
using System.Linq;
using Microsoft.EntityFrameworkCore.Storage;
using JZR.SurveyMaker.PL;

namespace JZR.VehicleTracker.PL.Test
{
    [TestClass]
    public class utAnswer
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
            Assert.AreEqual(11, dc.tblAnswers.Count());
        }

        [TestMethod]
        public void InsertTest()
        {

            tblAnswer newrow = new tblAnswer();
            newrow.Id = Guid.NewGuid();
            newrow.Answer = "NewAnswer";

            dc.tblAnswers.Add(newrow);
            int result = dc.SaveChanges();

            Assert.IsTrue(result > 0);

        }

        [TestMethod]
        public void UpdateTest()
        {
            InsertTest();

            tblAnswer existingrow = dc.tblAnswers.FirstOrDefault(a => a.Answer == "Yes");
            Guid id = existingrow.Id;
            if (existingrow != null)
            {
                existingrow.Answer = "Maybe";
                dc.SaveChanges();
            }

            tblAnswer row = dc.tblAnswers.FirstOrDefault(a => a.Id == id);

            Assert.AreEqual(existingrow.Answer, row.Answer);

        }

        [TestMethod]
        public void DeleteTest()
        {

            InsertTest();

            tblAnswer row = dc.tblAnswers.FirstOrDefault(a => a.Answer == "NewAnswer");
            if (row != null)
            {
                dc.tblAnswers.Remove(row);
                dc.SaveChanges();
            }

            tblAnswer deletedrow = dc.tblAnswers.FirstOrDefault(c => c.Answer == "NewAnswer");

            Assert.IsNull(deletedrow);
        }
    }
}


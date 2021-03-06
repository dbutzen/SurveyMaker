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
    public class utResponse
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
            Assert.AreEqual(3, dc.tblResponses.Count());
        }

        [TestMethod]
        public void InsertTest()
        {

            tblResponse newrow = new tblResponse();
            newrow.Id = Guid.NewGuid();
            newrow.QuestionId = dc.tblQuestions.FirstOrDefault(q => q.Question.Contains("tarsier")).Id;
            newrow.ResponseDate = DateTime.Now;
            newrow.AnswerId = dc.tblAnswers.FirstOrDefault(a => a.Answer.Contains("Yes")).Id;
            dc.tblResponses.Add(newrow);
            int result = dc.SaveChanges();

            Assert.IsTrue(result > 0);

        }

        [TestMethod]
        public void UpdateTest()
        {
            InsertTest();

            var questionId = dc.tblQuestions.FirstOrDefault(q => q.Question.Contains("tarsier")).Id;
            var answerId = dc.tblAnswers.FirstOrDefault(a => a.Answer == "Yes").Id;

            tblResponse existingrow = dc.tblResponses.FirstOrDefault(q => q.QuestionId == questionId && q.AnswerId == answerId);
            Guid id = existingrow.Id;
            if (existingrow != null)
            {
                existingrow.ResponseDate = new DateTime(2000, 1, 1);
                dc.SaveChanges();
            }

            tblResponse row = dc.tblResponses.FirstOrDefault(q => q.Id == id);

            Assert.AreEqual(existingrow.QuestionId, row.QuestionId);

        }

        [TestMethod]
        public void DeleteTest()
        {

            InsertTest();


            var questionId = dc.tblQuestions.FirstOrDefault(q => q.Question.Contains("tarsier")).Id;
            var answerId = dc.tblAnswers.FirstOrDefault(a => a.Answer.Contains("Yes")).Id;
            tblResponse row = dc.tblResponses.FirstOrDefault(q => q.QuestionId == questionId && q.AnswerId == answerId);

            if (row != null)
            {
                dc.tblResponses.Remove(row);
                dc.SaveChanges();
            }

            tblResponse deletedrow = dc.tblResponses.FirstOrDefault(q => q.QuestionId == questionId);

            Assert.IsNull(deletedrow);
        }
    }
}
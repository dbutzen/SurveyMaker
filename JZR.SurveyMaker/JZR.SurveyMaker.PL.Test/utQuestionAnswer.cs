using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JZR.VehicleTracker.PL;
using System.Linq;
using Microsoft.EntityFrameworkCore.Storage;
using JZR.SurveyMaker.PL;

namespace JZR.VehicleTracker.PL.Test
{
    [TestClass]
    public class utQuestionAnswer
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
            Assert.AreEqual(15, dc.tblQuestionAnswers.Count());
        }

        [TestMethod]
        public void InsertTest()
        {

            tblQuestionAnswer newrow = new tblQuestionAnswer();
            newrow.Id = Guid.NewGuid();
            newrow.AnswerId = dc.tblAnswers.FirstOrDefault(a => a.Answer == "Yes").Id;
            newrow.QuestionId = dc.tblQuestions.FirstOrDefault(q => q.Question.Contains("tarsier")).Id;
            newrow.IsCorrect = false;
            dc.tblQuestionAnswers.Add(newrow);
            int result = dc.SaveChanges();

            Assert.IsTrue(result > 0);

        }

        [TestMethod]
        public void UpdateTest()
        {
            InsertTest();

            var answerId = dc.tblAnswers.FirstOrDefault(a => a.Answer == "Yes").Id;
            var questionId = dc.tblQuestions.FirstOrDefault(q => q.Question.Contains("tarsier")).Id;

            tblQuestionAnswer existingrow = dc.tblQuestionAnswers.FirstOrDefault(q => q.AnswerId == answerId && q.QuestionId == questionId);
            Guid id = existingrow.Id;
            if (existingrow != null)
            {
                existingrow.IsCorrect = true;
                dc.SaveChanges();
            }

            tblQuestionAnswer row = dc.tblQuestionAnswers.FirstOrDefault(q => q.Id == id);

            Assert.AreEqual(existingrow.AnswerId, row.AnswerId);

        }

        [TestMethod]
        public void DeleteTest()
        {

            InsertTest();
            var answerId = dc.tblAnswers.FirstOrDefault(a => a.Answer == "Yes").Id;
            var questionId = dc.tblQuestions.FirstOrDefault(q => q.Question.Contains("tarsier")).Id;
            tblQuestionAnswer row = dc.tblQuestionAnswers.FirstOrDefault(q => q.AnswerId == answerId && q.QuestionId == questionId);
            if (row != null)
            {
                dc.tblQuestionAnswers.Remove(row);
                dc.SaveChanges();
            }

            tblQuestionAnswer deletedrow = dc.tblQuestionAnswers.FirstOrDefault(q => q.AnswerId == answerId && q.QuestionId == questionId);

            Assert.IsNull(deletedrow);
        }
    }
}


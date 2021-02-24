using JZR.SurveyMaker.BL.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JZR.SurveyMaker.BL.Test
{
    [TestClass]
    public class utQuestion
    {

        [TestMethod]
        public void LoadTest()
        {
            var task = QuestionManager.Load();
            task.Wait();
            var results = task.Result;
            Assert.AreEqual(5, results.Count);
        }

        [TestMethod]
        public  void InsertTest()
        {
            var task = QuestionManager.Insert(new Question { Text = "NewQuestion" }, true);
            task.Wait();
            Assert.IsTrue(task.Result > 0);
        }

        [TestMethod]
        public void UpdateTest()
        {
            var task = QuestionManager.Load();
            task.Wait();
            var questions = task.Result;
            Question question = questions.FirstOrDefault(q => q.Text.Contains("tarsier"));
            question.Text = "Updated Question";
            var results = QuestionManager.Update(question, true);
            results.Wait();
            Assert.IsTrue(results.Result > 0);
        }

        [TestMethod]
        public  void DeleteTest()
        {
            var task = QuestionManager.Load();
            task.Wait();
            var questions = task.Result;
            Question question = questions.FirstOrDefault(q => q.Text.Contains("tarsier"));
            var results = QuestionManager.Delete(question.Id, true);
            results.Wait();
            Assert.IsTrue(results.Result > 0);
        }
    }
}

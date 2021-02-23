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
            Task.Run(async () =>
            {
                var task = await QuestionManager.Load();
                IEnumerable<Question> questions = task;
                Assert.AreEqual(5, questions.ToList().Count);
            });
        }

        [TestMethod]
        public void InsertTest()
        {
            Task.Run(async () =>
            {
                int results = await QuestionManager.Insert(new Question { Text = "NewQuestion" }, true);
                Assert.IsTrue(results > 0);
            });
        }

        [TestMethod]
        public void UpdateTest()
        {
            var task = QuestionManager.Load();
            IEnumerable<Question> questions = task.Result;
            Question question = questions.FirstOrDefault(q => q.Text.Contains("tarsier"));
            question.Text = "Updated Question";
            var results = QuestionManager.Update(question, true);
            Assert.IsTrue(results.Result > 0);
        }

        [TestMethod]
        public void DeleteTest()
        {
            var task = QuestionManager.Load();
            IEnumerable<Question> questions = task.Result;
            Question question = questions.FirstOrDefault(q => q.Text.Contains("tarsier"));
            var results = QuestionManager.Delete(question.Id, true);
            Assert.IsTrue(results.Result > 0);
        }
    }
}

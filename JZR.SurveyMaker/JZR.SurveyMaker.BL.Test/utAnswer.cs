using JZR.SurveyMaker.BL.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JZR.SurveyMaker.BL.Test
{
    [TestClass]
    public class utAnswer
    {

        [TestMethod]
        public void LoadTest()
        {
            Task.Run(async () =>
            {
                var task = await AnswerManager.Load();
                IEnumerable<Answer> answers = task;
                Assert.AreEqual(11, answers.ToList().Count);
            });
        }

        [TestMethod]
        public void InsertTest()
        {
            Task.Run(async () =>
            {
                int results = await AnswerManager.Insert(new Answer { Text = "NewAnswer" }, true);
                Assert.IsTrue(results > 0);
            });
        }

        [TestMethod]
        public void UpdateTest()
        {
            Task.Run(async () =>
            {
                var task = await AnswerManager.Load();
                IEnumerable<Answer> answers = task;
                Answer answer = answers.FirstOrDefault(a => a.Text == "Yes");
                answer.Text = "Updated Answer";
                int results = await AnswerManager.Update(answer, true);
                Assert.IsTrue(results > 0);
            });
        }

        [TestMethod]
        public void DeleteTest()
        {
            Task.Run(async () =>
            {
                var task = await AnswerManager.Load();
                IEnumerable<Answer> answers = task;
                Answer answer = answers.FirstOrDefault(a => a.Text == "Yes");
                int results = await AnswerManager.Delete(answer.Id, true);
                Assert.IsTrue(results > 0);
            });
        }
    }
}

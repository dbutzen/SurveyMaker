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
            var task = AnswerManager.Load();
            task.Wait();
            var answers = task.Result;
            Assert.AreEqual(11, answers.ToList().Count);
        }

        [TestMethod]
        public void InsertTest()
        {
            var task =  AnswerManager.Insert(new Answer { Text = "NewAnswer" }, true);
            task.Wait();
            Assert.IsTrue(task.Result > 0);
        }

        [TestMethod]
        public void UpdateTest()
        {
            var task = AnswerManager.Load();
            task.Wait();
            var answers = task.Result;
            Answer answer = answers.FirstOrDefault(a => a.Text == "Yes");
            answer.Text = "Updated Answer";
            var results = AnswerManager.Update(answer, true);
            results.Wait();
            Assert.IsTrue(results.Result > 0);
        }

        [TestMethod]
        public void DeleteTest()
        {
            var task = AnswerManager.Load();
            task.Wait();
            var answers = task.Result;
            Answer answer = answers.FirstOrDefault(a => a.Text == "Yes");
            var results = AnswerManager.Delete(answer.Id, true);
            results.Wait();
            Assert.IsTrue(results.Result > 0);
        }
    }
}

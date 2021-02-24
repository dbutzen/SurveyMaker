using JZR.SurveyMaker.BL.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JZR.SurveyMaker.BL.Test
{
    [TestClass]
    public class utQuestionAnswer
    {

        [TestMethod]
        public void InsertTest()
        {
            var qtask = QuestionManager.LoadByText("How many holes are on a standard bowling ball?");
            qtask.Wait();
            var question = qtask.Result;

            var atask = AnswerManager.LoadByText("No");
            atask.Wait();
            var answer = atask.Result;
            answer.IsCorrect = true;
            question.Answers.Add(answer);
            var results = QuestionAnswerManager.Insert(question, true);
            results.Wait();
            Assert.IsTrue(results.Result > 0);
        }

        [TestMethod]
        public void DeleteTest()
        {
            var task = QuestionManager.Load();
            task.Wait();
            var questions = task.Result;
            
            var question = questions.FirstOrDefault(q => q.Text.Contains("tarsier"));
            var results = QuestionAnswerManager.Delete(question.Id, question.Answers[0].Id, true);
            results.Wait();
            Assert.IsTrue(results.Result > 0);
        }
    }
}

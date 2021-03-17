using JZR.SurveyMaker.BL.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JZR.SurveyMaker.BL.Test
{
    [TestClass]
    public class utResponse
    {

        [TestMethod]
        public void LoadByQuestionIdTest()
        {
            var task = QuestionManager.LoadByText("How many holes are on a standard bowling ball?");
            task.Wait();
            var question = task.Result;

            var atask = AnswerManager.LoadByText("A primate");
            atask.Wait();
            var answer = atask.Result;

            var dtask = ResponseManager.Insert(new Response { QuestionId = question.Id, AnswerId = answer.Id, ResponseDate = DateTime.Now }, true);
            dtask.Wait();
            var response = dtask.Result;

            var rtask = ResponseManager.LoadByQuestionId(question.Id);
            rtask.Wait();
            var responses = (List<Response>)rtask.Result;
            Assert.AreEqual(2, responses.ToList().Count);
             
        }

        [TestMethod]
        public void InsertTest()
        {
            var qtask = QuestionManager.LoadByText("What is a tarsier?");
            qtask.Wait();
            var question = qtask.Result;

            var atask = AnswerManager.LoadByText("A primate");
            atask.Wait();
            var answer = atask.Result;

            var task = ResponseManager.Insert(new Response { QuestionId = question.Id, AnswerId = answer.Id, ResponseDate = DateTime.Now}, true);
            task.Wait();
            Assert.IsTrue(task.Result > 0);
        }

    }
}

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
            Task.Run(async () =>
            {
                var question = new Question();
                question.Id = Guid.NewGuid();
                question.Text = "Is this a new question?";
                var answer = await AnswerManager.LoadByText("Yes");
                answer.IsCorrect = true;
                question.Answers.Add(answer);
                question.Answers.Add(await AnswerManager.LoadByText("No"));
                int results = await QuestionAnswerManager.Insert(question, true);
                Assert.IsTrue(results > 0);
            });
        }

        [TestMethod]
        public void DeleteTest()
        {
            Task.Run(async () =>
            {
                var task = await QuestionManager.Load();
                IEnumerable<Question> questions = task;
                Question question = questions.FirstOrDefault(q => q.Text.Contains("tarsier"));
                int results = await QuestionAnswerManager.Delete(question.Id, question.Answers[0].Id, true);
                Assert.IsTrue(results > 0);
            });
        }
    }
}

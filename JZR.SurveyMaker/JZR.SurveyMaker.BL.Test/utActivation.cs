using JZR.SurveyMaker.BL.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JZR.SurveyMaker.BL.Test
{
    [TestClass]
    public class utActivation
    {

        [TestMethod]
        public void InsertTest()
        {
            var qtask = QuestionManager.LoadByText("What is a tarsier?");
            qtask.Wait();
            var question = qtask.Result;

            var task = ActivationManager.Insert(new Activation { ActivationCode = "123456", QuestionId = question.Id, QuestionText = question.Text, StartDate = DateTime.Now, EndDate = DateTime.Now }, true);
            task.Wait();
            Assert.IsTrue(task.Result > 0);
        }
        
        [TestMethod]
        public void UpdateTest()
        {
            var task = ActivationManager.Load();
            task.Wait();
            var activations = task.Result;

            Activation activation = activations.FirstOrDefault(a => a.ActivationCode == "1q2w3e");
            activation.ActivationCode = "UPDATE";
            var results = ActivationManager.Update(activation, true);
            
            results.Wait();
            Assert.IsTrue(results.Result > 0);
        }

        [TestMethod]
        public void DeleteTest()
        {
            var task = ActivationManager.Load();
            task.Wait();
            var activations = task.Result;

            Activation activation = activations.FirstOrDefault(a => a.ActivationCode == "1q2w3e");
            var results = ActivationManager.Delete(activation.Id, true);
            
            results.Wait();
            Assert.IsTrue(results.Result > 0);

        }
    }
}

using JZR.SurveyMaker.BL.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace TeamC.SurveyMaker.API.Test
{
    [TestClass]
    public class utQuestion
    {
        private HttpClient InitializeClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://teamcsurveymakerapi.azurewebsites.net/api/");
            return client;
        }

        private JArray GetData(string controller)
        {
            HttpClient client = InitializeClient();
            HttpResponseMessage response;
            string result;
            response = client.GetAsync(controller).Result;
            result = response.Content.ReadAsStringAsync().Result;
            return (JArray)JsonConvert.DeserializeObject(result);
        }

        [TestMethod]
        public void PostTest()
        {
            var question = new Question();
            question.Id= Guid.NewGuid();
            question.Text = "What day is tomorrow?";

            var client = InitializeClient();
            string serializedObject = JsonConvert.SerializeObject(question);
            var content = new StringContent(serializedObject);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = client.PostAsync("Question?rollback=true", content).Result;
            var results = int.Parse(response.Content.ReadAsStringAsync().Result);

            Assert.IsTrue(results > 0);
        }

        [TestMethod]
        public void GetTest()
        {
            var questions = GetData("Question").ToObject<List<Question>>().ToList().Count;

            Assert.AreEqual(6, questions);
        }

        [TestMethod]
        public void GetByIdTest()
        {
            var questionByText = GetData("Question").ToObject<List<Question>>().FirstOrDefault(q => q.Text == "What is a tarsier?");
            var questionById = GetData("Question").ToObject<List<Question>>().FirstOrDefault(q => q.Id == questionByText.Id);

            Assert.IsTrue(questionByText.Text == questionById.Text);
        }

        [TestMethod]
        public void PutTest()
        {
            var question = GetData("Question").ToObject<List<Question>>().FirstOrDefault(q => q.Text == "What is a tarsier?");
            question.Text = "What day is it?";

            var client = InitializeClient();
            var serializedObject = JsonConvert.SerializeObject(question);
            var content = new StringContent(serializedObject);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = client.PutAsync($"Question/{question.Id}?rollback=true", content).Result;
            var results = int.Parse(response.Content.ReadAsStringAsync().Result);

            Assert.IsTrue(results > 0);
        }

        [TestMethod]
        public void LoadByActivationCodeTest()
        {
            var activation = GetData("Activation").ToObject<List<Activation>>().FirstOrDefault(a => a.ActivationCode == "1q2w3e");
            var question = GetData("Question").ToObject<List<Question>>().FirstOrDefault(q => q.Id == activation.QuestionId);

            Assert.IsTrue(activation.QuestionId == question.Id);
        }
    }
}

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
    public class utActivation
    {
        private HttpClient InitializeClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://teamcsurveymakerapi.azurewebsites.net/api/");
            return client;
        }

        [TestMethod]
        public void PostTest()
        {
            var questions = GetData("Question").ToObject<List<Question>>();
            var questionId = questions.FirstOrDefault(q => q.Text == "What is a tarsier?").Id;

            var activation = new Activation();
            activation.QuestionId = questionId;
            activation.StartDate = DateTime.Today.AddDays(30);
            activation.EndDate = DateTime.Today.AddDays(60);
            activation.ActivationCode = "0o9i8u";

            var client = InitializeClient();
            var serializedObject = JsonConvert.SerializeObject(activation);
            var content = new StringContent(serializedObject);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = client.PostAsync("Activation?rollback=true", content).Result;
            var results = int.Parse(response.Content.ReadAsStringAsync().Result);

            Assert.IsTrue(results > 0);
        }


        [TestMethod]
        public void GetTest()
        {
            var activations = GetData("Activation").ToObject<List<Activation>>().ToList().Count;

            Assert.AreEqual(3, activations);
        }


        [TestMethod]
        public void PutTest()
        {
            var activation = GetData("Activation").ToObject<List<Activation>>().FirstOrDefault(a => a.ActivationCode == "q2wer5");
            activation.ActivationCode = "0o9i8u";

            var client = InitializeClient();
            var serializedObject = JsonConvert.SerializeObject(activation);
            var content = new StringContent(serializedObject);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = client.PutAsync($"Activation/{activation.Id}?rollback=true", content).Result;
            var results = int.Parse(response.Content.ReadAsStringAsync().Result);

            Assert.IsTrue(results > 0);
        }


        [TestMethod]
        public void DeleteTest()
        {
            var rows = GetData("Activation").ToObject<List<Activation>>();
            var id = rows.FirstOrDefault(a => a.ActivationCode == "q2wer5").Id;
            var client = InitializeClient();
            var response = client.DeleteAsync($"Activation/{id}?rollback=true").Result;
            var results = int.Parse(response.Content.ReadAsStringAsync().Result);
            Assert.IsTrue(results > 0);
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
    }
}

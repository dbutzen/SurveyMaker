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
    public class utResponse
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
            var question = GetData("Question").ToObject<List<Question>>().FirstOrDefault(q => q.Text == "What is a tarsier?");
            var answer = question.Answers.FirstOrDefault(a => a.Text == "A primate");

            var qResponse = new Response();
            qResponse.Id = Guid.NewGuid();
            qResponse.QuestionId = question.Id;
            qResponse.AnswerId = answer.Id;
            qResponse.ResponseDate = DateTime.Now;

            var client = InitializeClient();
            var serializedObject = JsonConvert.SerializeObject(qResponse);
            var content = new StringContent(serializedObject);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = client.PostAsync("Response?rollback=true", content).Result;
            var results = int.Parse(response.Content.ReadAsStringAsync().Result);

            Assert.IsTrue(results > 0);
        }
    }
}

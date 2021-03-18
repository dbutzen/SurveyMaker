using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using JZR.SurveyMaker.BL.Models;
using TeamC.SurveyMaker.Quizzer.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TeamC.SurveyMaker.Quizzer.Controllers
{
    public class QuizzerController : Controller
    {

        private static HttpClient InitializeClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://teamcsurveymakerapi.azurewebsites.net/api/");
            return client;
        }
        // GET: QuizzerController
        public ActionResult Index()
        {
            try
            {
                string activationCode = Request.Form["txtActivationCode"].ToString();
                QuizzerViewModel.activationCode = activationCode;


                HttpClient client = InitializeClient();
                HttpResponseMessage response;
                string result;
                dynamic items;

                Question question = null;
                QuizzerViewModel quizzerViewModel = new QuizzerViewModel();

                response = client.GetAsync("Question/" + activationCode).Result;

                result = response.Content.ReadAsStringAsync().Result;
                items = (JObject)JsonConvert.DeserializeObject(result);
                question = items.ToObject<Question>();


                QuizzerViewModel.question = question;

                return View(question);
            }
            catch (Exception ex)
            {

                throw new Exception("Error with activation code. Code might not exists.");
            }
            
            

        }

        public ActionResult Submit()
        {
            string strAnswerId = Request.Form["selectedButton"];

            Response response = new Response();
            response.Id = Guid.NewGuid();
            response.QuestionId = QuizzerViewModel.question.Id;
            response.Answers = QuizzerViewModel.question.Answers;
            response.ResponseDate = DateTime.Now;
            response.AnswerId = new Guid(strAnswerId);

            var client = InitializeClient();
            var serializedObject = JsonConvert.SerializeObject(response);
            var content = new StringContent(serializedObject);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var postResponse = client.PostAsync("Response?rollback=true", content).Result;

            return View();
        }

        // GET: QuizzerController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: QuizzerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: QuizzerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: QuizzerController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: QuizzerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: QuizzerController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: QuizzerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

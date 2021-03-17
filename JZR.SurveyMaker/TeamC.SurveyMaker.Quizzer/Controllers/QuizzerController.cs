using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

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

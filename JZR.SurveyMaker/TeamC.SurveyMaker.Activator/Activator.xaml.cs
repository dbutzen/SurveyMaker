﻿using JZR.SurveyMaker.BL.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TeamC.SurveyMaker.Activator
{
    /// <summary>
    /// Interaction logic for Activator.xaml
    /// </summary>
    public partial class Activator : Window
    {
        List<Question> questions;

        public Activator()
        {
            InitializeComponent();

            questions = new List<Question>();

            Reload();
        }

        private void Reload()
        {
            HttpClient client = InitializeClient();
            HttpResponseMessage response;
            string result;
            dynamic items;

            response = client.GetAsync("Question").Result;
            result = response.Content.ReadAsStringAsync().Result;
            items = (JArray)JsonConvert.DeserializeObject(result);
            questions = items.ToObject<List<Question>>();

            Rebind();
        }

        private void Rebind()
        {
            //dgvQuestions.ItemsSource = null;
            //dgvQuestions.ItemsSource = questions;
        }

        private HttpClient InitializeClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44327/api/");
            return client;
        }
    }
}

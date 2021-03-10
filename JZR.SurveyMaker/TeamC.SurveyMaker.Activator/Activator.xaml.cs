using JZR.SurveyMaker.BL.Models;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
        Question selectedQuestion;

        public Activator()
        {
            InitializeComponent();

            questions = new List<Question>();

            InitialLoad();
        }

        private async void InitialLoad()
        {
            await ReloadAsync();
        }

        private async Task ReloadAsync()
        {
            await Task.Run(() =>
            {
                HttpClient client = InitializeClient();
                HttpResponseMessage response;
                string result;
                dynamic items;

                response = client.GetAsync("Question").Result;
                result = response.Content.ReadAsStringAsync().Result;
                items = (JArray)JsonConvert.DeserializeObject(result);
                questions = items.ToObject<List<Question>>();
            });
            Rebind();
        }

        private void Rebind()
        {
            dgvQuestions.ItemsSource = null;
            dgvQuestions.ItemsSource = questions;
        }

        private HttpClient InitializeClient()
        {
            var client = new HttpClient();
            // LOCAL API
            //client.BaseAddress = new Uri("https://localhost:44327/api/");

            // WEB API
            client.BaseAddress = new Uri("https://teamcsurveymakerapi.azurewebsites.net/api/");
            return client;
        }

        private void dgvQuestions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedQuestion = (Question)dgvQuestions.SelectedItem;
            dgvActivations.ItemsSource = null;
            dgvActivations.ItemsSource = selectedQuestion.Activations;
        }

        private async void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var manage = new ucManageActivation();
            await DialogHost.Show(manage);
        }

        private async void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var activation = (Activation)((Button)sender).DataContext;
            var manage = new ucManageActivation(activation);
            await DialogHost.Show(manage);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

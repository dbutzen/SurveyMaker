using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TeamC.Quizzer.Mobile.Models;
using TeamC.Quizzer.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeamC.Quizzer.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResponsePage : ContentPage
    {
        Question question;
        ObservableCollection<ResponseViewModel> responseView;
        public ResponsePage(Question question)
        {
            InitializeComponent();
            this.question = question;
            Load();
        }


        private void Load()
        {
            CloseMessageBar();
            responseView = new ObservableCollection<ResponseViewModel>();
            txtQuestion.Text = question.Text;
            var c = 0;
            foreach (var a in question.Answers)
            {
                string letter;
                switch (++c)
                {
                    case 1: letter = "A"; break;
                    case 2: letter = "B"; break;
                    case 3: letter = "C"; break;
                    default: letter = "D"; break;
                }
                responseView.Add(new ResponseViewModel { IsSelected = false, Letter = letter, Answer = a });
            }
            cvAnswers.ItemsSource = null;
            cvAnswers.ItemsSource = responseView;
        }

        private async void btnSubmit_Clicked(object sender, EventArgs e)
        {
            try
            {
                CloseMessageBar();
                await Submit();
            }
            catch (Exception ex)
            {
                ShowMessage($"Error: {ex.Message}", Brush.LightCoral);
            }
        }

        private async Task Submit()
        {
            try
            {
                if (responseView.ToList().Any(r => r.IsSelected))
                {
                    DisableControls();
                    var selectedAnswer = responseView.FirstOrDefault(r => r.IsSelected == true).Answer;
                    var quizzerResponse = new Response();
                    quizzerResponse.QuestionId = question.Id;
                    quizzerResponse.AnswerId = selectedAnswer.Id;
                    quizzerResponse.ResponseDate = DateTime.Now;

                    HttpClient client = InitializeClient();
                    string serializedObject = JsonConvert.SerializeObject(quizzerResponse);
                    var content = new StringContent(serializedObject);
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    var response = new HttpResponseMessage();

                    await Task.Run(() =>
                    {
                        response = client.PostAsync("Response", content).Result;
                    });

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        if (selectedAnswer.IsCorrect)
                            ShowMessage("Submitted: Your answer is correct!", Brush.Green);
                        else
                            ShowMessage($"Submitted: The correct answer is {responseView.FirstOrDefault(r => r.Answer.IsCorrect).Letter}.", Brush.LightCoral);
                    }
                    else
                    {
                        throw new Exception(response.Content.ReadAsStringAsync().Result);
                    }
                }
                else
                {
                    throw new Exception("No answer is selected.");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private HttpClient InitializeClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://teamcsurveymakerapi.azurewebsites.net/api/");
            return client;
        }
        private void ShowMessage(string message, Brush backColor, bool showCloseButton = true)
        {
            txtMessage.Text = message;
            grdMessageBar.Background = backColor;
            grdMessageBar.IsVisible = true;
            btnClose.IsVisible = showCloseButton;
        }

        private void CloseMessageBar()
        {
            grdMessageBar.IsVisible = false;
        }

        private void btnClose_Clicked(object sender, EventArgs e)
        {
            CloseMessageBar();
        }

        private void DisableControls()
        {

            ShowMessage("Please wait...", Brush.LightBlue);
            slQuestion.IsEnabled = false;
            btnSubmit.IsEnabled = false;
        }

        private void EnableControls()
        {
            slQuestion.IsEnabled = true;
            btnSubmit.IsEnabled = true;
        }

        private void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            CloseMessageBar();
        }
    }
}
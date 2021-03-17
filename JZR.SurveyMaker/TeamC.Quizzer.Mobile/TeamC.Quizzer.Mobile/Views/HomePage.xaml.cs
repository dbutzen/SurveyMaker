using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TeamC.Quizzer.Mobile.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeamC.Quizzer.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        Question question;
        public HomePage()
        {
            InitializeComponent();
        }


        private async Task Load()
        {
            try
            {
                question = null;
                DisableControls();
                await Task.Run(() =>
                {
                    if (!string.IsNullOrEmpty(txtActivationCode.Text))
                    {
                        HttpClient client = InitializeClient();
                        HttpResponseMessage response;
                        string result;
                        response = client.GetAsync($"Question/{txtActivationCode.Text}").Result;
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            result = response.Content.ReadAsStringAsync().Result;
                            question = JsonConvert.DeserializeObject<Question>(result);
                        }
                        else
                        {
                            throw new Exception(response.Content.ReadAsStringAsync().Result);
                        }

                    }
                });
                if (question != null)
                {
                    CloseMessageBar();
                    await Navigation.PushAsync(new ResponsePage(question) { Title = "Question" });
                    txtActivationCode.Text = string.Empty;
                }

            }
            catch (Exception ex)
            {

                ShowMessage($"Error: {ex.Message}", Brush.LightCoral);
            }
            EnableControls();
        }

        private async void btnSubmit_Clicked(object sender, EventArgs e)
        {
            await Load();
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


        private HttpClient InitializeClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://teamcsurveymakerapi.azurewebsites.net/api/");
            return client;
        }

        private void DisableControls()
        {
            
            ShowMessage("Please wait...", Brush.LightBlue);
            txtActivationCode.IsEnabled = false;
            btnSubmit.IsEnabled = false;
        }

        private void EnableControls()
        {
            txtActivationCode.IsEnabled = true;
            btnSubmit.IsEnabled = true;
        }

        private void txtActivationCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            CloseMessageBar();
            btnSubmit.IsEnabled = !string.IsNullOrEmpty(txtActivationCode.Text);
        }
    }
}
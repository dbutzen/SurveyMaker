using JZR.SurveyMaker.BL.Models;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TeamC.SurveyMaker.Activator
{
    /// <summary>
    /// Interaction logic for ucManageActivation.xaml
    /// </summary>
    public partial class ucManageActivation : UserControl
    {

        Activation activation;
        Guid questionId;
        public ucManageActivation(Guid questionId, Activation activation = null)
        {
            InitializeComponent();
            this.activation = activation;
            this.questionId = questionId;
            InitialLoad();

        }

        private void InitialLoad()
        {
            if (activation != null)
            {
                txtTitle.Text = "Edit Activation";
                btnSave.Visibility = Visibility.Visible;
                btnAdd.Visibility = Visibility.Collapsed;
            }
            else
            {
                txtTitle.Text = "New Activation";
                btnSave.Visibility = Visibility.Collapsed;
                btnAdd.Visibility = Visibility.Visible;
                activation = new Activation();
            }
            Load();
        }

        private void Load()
        {
            CloseMessageBar();
            txtActivationCode.Text = activation.ActivationCode;
            dpStartDate.SelectedDate = activation.StartDate != DateTime.MinValue ? activation.StartDate : DateTime.Today;
            dpEndDate.SelectedDate = activation.EndDate != DateTime.MinValue ? activation.EndDate : DateTime.Today;
        }

        private void date_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            CloseMessageBar();
            dpEndDate.SelectedDate = dpStartDate.SelectedDate > dpEndDate.SelectedDate ? dpStartDate.SelectedDate : dpEndDate.SelectedDate;
            dpStartDate.SelectedDate = dpEndDate.SelectedDate < dpStartDate.SelectedDate ? dpEndDate.SelectedDate : dpStartDate.SelectedDate;

        }

        private void txtActivationCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            CloseMessageBar();
        }

        private bool SelectedDateIsValid()
        {
            try
            {
                // This will validate the selected date range is available or not
                // Only one active activation code is allowed per question
                var results = GetData("Activation").ToObject<List<Activation>>();
                // Load all the activations of a question except for the one that is being updated
                var questionActivations = (results).Where(a => a.QuestionId == questionId && a.ActivationCode != activation.ActivationCode);

                foreach (var a in questionActivations)
                {
                    var startDateIsValid = dpStartDate.SelectedDate < a.StartDate || dpStartDate.SelectedDate > a.EndDate;
                    var endDateIsValid = dpEndDate.SelectedDate < a.StartDate || dpEndDate.SelectedDate > a.EndDate;
                    if (!(startDateIsValid && endDateIsValid))
                        return false;
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task InitializeActivation()
        {
            try
            {
                // Check if activation code is available
                var results = new List<Activation>();
                results = await Task.Run(() => GetData("Activation").ToObject<List<Activation>>());
                var result = results.FirstOrDefault(r => r.ActivationCode == txtActivationCode.Text);
                // Activation must be null or the activation code must belong to the activation that is being updated
                if (result == null || activation.ActivationCode == txtActivationCode.Text)
                {
                    if (txtActivationCode.Text.Length == 6)
                    {
                        if (SelectedDateIsValid())
                        {
                            if (activation == null)
                                activation = new Activation();
                            activation.QuestionId = questionId;
                            activation.ActivationCode = txtActivationCode.Text;
                            activation.StartDate = (DateTime)dpStartDate.SelectedDate;
                            activation.EndDate = (DateTime)dpEndDate.SelectedDate;
                        }
                        else
                        {
                            throw new Exception("Date range will conflict with another activation.");
                        }
                    }
                    else
                    {
                        throw new Exception("Activation code must be 6 characters.");
                    }
                }
                else
                {
                    throw new Exception("Activation code is already in use.");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async void btnAdd_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                DisableControls();
                await InitializeActivation();
                HttpClient client = InitializeClient();
                string serializedObject = JsonConvert.SerializeObject(activation);
                var content = new StringContent(serializedObject);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var response = new HttpResponseMessage();
                
                await Task.Run(() =>
                {
                    response = client.PostAsync("Activation", content).Result;
                });
                
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Clear();
                    ShowMessage("Success: Activation has been added.", Brushes.Green);
                }
                else
                {
                    throw new Exception(response.Content.ReadAsStringAsync().Result);
                }
            }
            catch (Exception ex)
            {

                ShowMessage($"Error: {ex.Message}", Brushes.LightCoral);
            }
            EnabledContols();
        }


        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DisableControls();
                await InitializeActivation();
                HttpClient client = InitializeClient();
                string serializedObject = JsonConvert.SerializeObject(activation);
                var content = new StringContent(serializedObject);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var response = new HttpResponseMessage();
                await Task.Run(() =>
                {
                    response = client.PutAsync($"Activation/{activation.Id}", content).Result;
                });

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    ShowMessage("Success: Activation has been updated.", Brushes.Green);
                }
                else
                {
                    throw new Exception(response.Content.ReadAsStringAsync().Result);
                }
            }
            catch (Exception ex)
            {

                ShowMessage($"Error: {ex.Message}", Brushes.LightCoral);
            }
            EnabledContols();
        }

        private JArray GetData(string controller)
        {
            try
            {
                HttpClient client = InitializeClient();
                HttpResponseMessage response;
                string result;
                response = client.GetAsync(controller).Result;
                result = response.Content.ReadAsStringAsync().Result;
                return (JArray)JsonConvert.DeserializeObject(result);
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

        private void Clear()
        {
            txtActivationCode.Text = string.Empty;
            dpStartDate.SelectedDate = DateTime.Today;
            dpEndDate.SelectedDate = DateTime.Today;
            txtActivationCode.SelectAll();
            txtActivationCode.Focus();
        }

        private void ShowMessage(string message, Brush backColor)
        {
            txtMessage.Text = message;
            grdMessageBar.Background = backColor;
            grdMessageBar.Visibility = Visibility.Visible;
        }

        private void CloseMessageBar()
        {
            grdMessageBar.Visibility = Visibility.Collapsed;
        }

        private void btnCloseMessageBar_Click(object sender, RoutedEventArgs e)
        {
            CloseMessageBar();
        }


        private void DisableControls()
        {
            CloseMessageBar();
            pbLoadingBar.Visibility = Visibility.Visible;
            btnClose.IsEnabled = false;
            grdControls.IsEnabled = false;
        }

        private void EnabledContols()
        {
            pbLoadingBar.Visibility = Visibility.Collapsed;
            btnClose.IsEnabled = true;
            grdControls.IsEnabled = true;
        }
    }
}

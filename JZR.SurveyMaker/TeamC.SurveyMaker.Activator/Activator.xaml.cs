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

            DisableControls();
            CloseMessageBar();
            var isConnected = false;
            var message = string.Empty;
            await Task.Run(() =>
            {
                try
                {
                    questions = GetData("Question").ToObject<List<Question>>();
                    isConnected = true;
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }
            });
            if (!isConnected)
                ShowMessage($"Error: {message}", Brushes.LightCoral);
            Rebind();
        }

        private void Rebind()
        {
            cboQuestions.ItemsSource = null;
            cboQuestions.ItemsSource = questions;
            EnableControls();

            if (cboQuestions.Items.Count > 0)
            {
                cboQuestions.SelectedIndex = 0;
            }
        }

        private HttpClient InitializeClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://teamcsurveymakerapi.azurewebsites.net/api/");
            return client;
        }


        private async void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            CloseMessageBar();
            var manage = new ucManageActivation(selectedQuestion.Id);
            await DialogHost.Show(manage);
            var lastQuestionId = selectedQuestion.Id;
            await ReloadAsync();
            cboQuestions.SelectedValue = lastQuestionId;
        }

        private async void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            CloseMessageBar();
            var activation = (Activation)((Button)sender).DataContext;
            var manage = new ucManageActivation(selectedQuestion.Id, activation);
            await DialogHost.Show(manage);
            var lastQuestionId = selectedQuestion.Id;
            await ReloadAsync();
            cboQuestions.SelectedValue = lastQuestionId;

        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CloseMessageBar();
                var dialog = new ucDialog();
                await DialogHost.Show(dialog);
                if (dialog.MessageBoxResult == MessageBoxResult.Yes)
                {
                    var activation = (Activation)dgvActivations.SelectedItem;
                    HttpClient client = InitializeClient();
                    HttpResponseMessage response = client.DeleteAsync($"Activation/{activation.Id}").Result;
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var lastQuestionId = selectedQuestion.Id;
                        await ReloadAsync();
                        cboQuestions.SelectedValue = lastQuestionId;
                        ShowMessage("Activation has been removed.", Brushes.Orange);
                    }
                    else
                    {
                        throw new Exception(response.Content.ReadAsStringAsync().Result);
                    }
                }

            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, Brushes.LightCoral);
            }
        }

        private void cboQuestions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CloseMessageBar();
            if (cboQuestions.SelectedIndex > -1)
            {
                selectedQuestion = (Question)cboQuestions.SelectedItem;
                btnAdd.IsEnabled = selectedQuestion != null;
                dgvActivations.ItemsSource = null;
                dgvActivations.ItemsSource = selectedQuestion.Activations.OrderBy(a => a.StartDate);
            }

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

        private void dgvActivations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CloseMessageBar();
        }

        private async void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            await ReloadAsync();
        }


        private void DisableControls()
        {
            CloseMessageBar();
            pbLoadingCircle.Visibility = Visibility.Visible;
            grdMainGrid.IsEnabled = false;
        }

        private void EnableControls()
        {
            pbLoadingCircle.Visibility = Visibility.Collapsed;
            grdMainGrid.IsEnabled = true;
        }
    }
}

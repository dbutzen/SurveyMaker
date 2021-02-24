using JZR.SurveyMaker.BL;
using JZR.SurveyMaker.BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace JZR.SurveyMaker.UI
{
    /// <summary>
    /// Interaction logic for SurveyMaker.xaml
    /// </summary>
    public partial class SurveyMaker : Window
    {

        List<Question> questions;

        public SurveyMaker()
        {
            InitializeComponent();
            InitialLoad();
        }


        private async void InitialLoad()
        {
            await ReloadAsync();
        }

        private async void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            await ReloadAsync();
            ShowMessage($"Loaded {questions.Count} question(s)", System.Drawing.Color.CornflowerBlue);
        }

        private async Task ReloadAsync()
        {
            CloseMessageBar();
            questions = await QuestionManager.Load();
            Rebind();
        }

        private void Rebind()
        {
            grdQuestions.ItemsSource = null;
            grdQuestions.ItemsSource = questions;
            
        }

        private void ShowMessage(string message, System.Drawing.Color backColor)
        {
            txtMessage.Text = message;
            grdMessageBar.Background = new SolidColorBrush(Color.FromArgb(backColor.A, backColor.R, backColor.G, backColor.B));
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



        private async void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            CloseMessageBar();
            var selectedQuestion = (Question)(((Button)sender).DataContext);
            var screen = new MaintainQuestionAnswer(selectedQuestion);
            screen.Owner = this;
            screen.ShowDialog();
            await ReloadAsync();
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            CloseMessageBar();
            var result = MessageBox.Show("Are you sure you want to delete?", "Delete Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                var selectedQuestion = (Question)(((Button)sender).DataContext);
                await QuestionManager.Delete(selectedQuestion.Id);
                await ReloadAsync();
                ShowMessage($"Question Deleted", System.Drawing.Color.Orange);
            }
            

        }

        private async void btnMainQuestions_Click(object sender, RoutedEventArgs e)
        {
            CloseMessageBar();
            var screen = new Manager(ScreenMode.Question);
            screen.Owner = this;
            screen.ShowDialog();
            await ReloadAsync();
        }

        private async void btnMaintainAnswers_Click(object sender, RoutedEventArgs e)
        {
            CloseMessageBar();
            var screen = new Manager(ScreenMode.Answer);
            screen.Owner = this;
            screen.ShowDialog();
            await ReloadAsync();
        }

        private async void btnManage_Click(object sender, RoutedEventArgs e)
        {
            CloseMessageBar();
            var screen = new MaintainQuestionAnswer();
            screen.Owner = this;
            screen.ShowDialog();
            await ReloadAsync();
        }
    }
}

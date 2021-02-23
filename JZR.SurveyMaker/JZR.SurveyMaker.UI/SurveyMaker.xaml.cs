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
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            ReloadAsync();

        }

        private async void ReloadAsync()
        {
            questions = (List<Question>)await QuestionManager.Load();
            Rebind();
            //ShowMessage("Loaded", System.Drawing.Color.CornflowerBlue);
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ReloadAsync();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
             var result = MessageBox.Show("Are you sure you want to delete?", "Delete Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                var selectedQuestion = (Question)(((Button)sender).DataContext);
                await QuestionManager.Delete(selectedQuestion.Id);
                ReloadAsync();
            }
            

        }

        private void btnMainQuestions_Click(object sender, RoutedEventArgs e)
        {
            var screen = new MaintainAttributes(ScreenMode.Question);
            screen.Owner = this;
            screen.ShowDialog();
        }

        private void btnMaintainAnswers_Click(object sender, RoutedEventArgs e)
        {
            var screen = new MaintainAttributes(ScreenMode.Answer);
            screen.Owner = this;
            screen.ShowDialog();
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            var screen = new MaintainQuestionAnswer();
            screen.Owner = this;
            screen.Show();
        }
    }
}

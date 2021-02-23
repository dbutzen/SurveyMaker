using JZR.SurveyMaker.BL;
using JZR.SurveyMaker.BL.Models;
using JZR.SurveyMaker.UI.Custom;
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
using System.Windows.Shapes;

namespace JZR.SurveyMaker.UI
{
    /// <summary>
    /// Interaction logic for MaintainQuestionAnswer.xaml
    /// </summary>
    public partial class MaintainQuestionAnswer : Window
    {
        List<Question> questions;
        List<Answer> answers;
        public MaintainQuestionAnswer()
        {
            InitializeComponent();
            ReloadAsync();
        }

        private async void ReloadAsync()
        {
            questions = (List<Question>)await QuestionManager.Load();
            answers = (List<Answer>)await AnswerManager.Load();
            Rebind();
        }

        private void Rebind()
        {
            cboQuestions.ItemsSource = null;
            cboQuestions.ItemsSource = questions;

            if (cboQuestions.Items.Count > 1)
                cboQuestions.SelectedIndex = 0;
        }

        private void cboQuestions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboQuestions.SelectedIndex > -1)
            {
                icCustomControl.ItemsSource = null;
                var selectedQuestion = (Question)cboQuestions.SelectedItem;
                var customItems = new List<CustomControlView>();
                selectedQuestion.Answers.ForEach(a =>
                {
                    var item = new CustomControlView();
                    item.IsCorrectAnswer = a.IsCorrect;
                    item.SelectedAnswer = a;
                    item.Answers = answers;
                    customItems.Add(item);
                });

                icCustomControl.ItemsSource = customItems;
            }
        }
    }
}

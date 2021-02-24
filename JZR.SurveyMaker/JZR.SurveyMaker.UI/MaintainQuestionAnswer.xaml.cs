using JZR.SurveyMaker.BL;
using JZR.SurveyMaker.BL.Models;
using JZR.SurveyMaker.UI.Custom;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        List<CustomControlView> customItems;
        Question selectedQuestion;
        public MaintainQuestionAnswer(Question question = null)
        {
            InitializeComponent();
            selectedQuestion = question;
            InitialLoad();


        }

        private async void InitialLoad()
        {
            var question= selectedQuestion;
            await ReloadAsync();
            if (question != null && cboQuestions.Items.Count > 0)
                cboQuestions.SelectedValue = question.Id;

            
        }

        private async Task ReloadAsync()
        {
            questions = await QuestionManager.Load();
            answers = await AnswerManager.Load();

            Rebind();
        }

        private void Rebind()
        {
            CloseMessageBar();
            cboQuestions.ItemsSource = null;
            cboQuestions.ItemsSource = questions;

            if (cboQuestions.Items.Count > 1)
                cboQuestions.SelectedIndex = 0;
        }

        private void cboQuestions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CloseMessageBar();
            if (cboQuestions.SelectedIndex > -1)
            {
                icCustomControl.ItemsSource = null;
                selectedQuestion = (Question)cboQuestions.SelectedItem;
                customItems = new List<CustomControlView>();
                selectedQuestion.Answers.ForEach(a =>
                {
                    var item = new CustomControlView();
                    item.Id = customItems.Count + 1;
                    item.IsCorrectAnswer = a.IsCorrect;
                    item.SelectedAnswer = a;
                    item.Answers = new ObservableCollection<Answer>(answers);
                    customItems.Add(item);
                });

                icCustomControl.ItemsSource = customItems;
                btnAddCustomControl.IsEnabled = customItems.Count < 4;
            }
        }

        private async void btnManageAnswers_Click(object sender, RoutedEventArgs e)
        {
            var screen = new Manager(ScreenMode.Answer);
            screen.Owner = this;
            screen.ShowDialog();
            await ReloadAsync();
        }

        private async void btnManageQuestions_Click(object sender, RoutedEventArgs e)
        {
            var screen = new Manager(ScreenMode.Question);
            screen.Owner = this;
            screen.ShowDialog();
            await ReloadAsync();
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            CloseMessageBar();
            var control = (CustomControlView)((Button)sender).DataContext;
            var items = customItems.ToList();
            items.RemoveAll(c => c.Id == control.Id);
            customItems = items;
            icCustomControl.ItemsSource = customItems;
            btnAddCustomControl.IsEnabled = customItems.Count < 4;

        }

        private void btnAddCustomControl_Click(object sender, RoutedEventArgs e)
        {
            CloseMessageBar();
            // Create custom controls
            if (customItems.Count < 4)
            {
                icCustomControl.ItemsSource = null;
                var item = new CustomControlView();
                //item.SelectedAnswer = answers[0];
                item.Id = customItems.Count + 1;
                item.SelectedAnswer = new Answer();
                item.Answers = new ObservableCollection<Answer>(answers);
                customItems.Add(item);
                icCustomControl.ItemsSource = customItems;

            }
            btnAddCustomControl.IsEnabled = customItems.Count < 4;

        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            CloseMessageBar();
            try
            {
                if (customItems.Count(c => c.SelectedAnswer.Id != Guid.Empty)>= 2)
                {
                    if (customItems.Any(c => c.IsCorrectAnswer && c.SelectedAnswer.Id != Guid.Empty))
                    {
                        await QuestionAnswerManager.DeleteByQuestionId(selectedQuestion.Id);
                        selectedQuestion.Answers = new List<Answer>();
                        customItems.ForEach(c =>
                        {
                            if (c.SelectedAnswer.Id != Guid.Empty)
                            {
                                var answer = new Answer();
                                answer.Id = c.SelectedAnswer.Id;
                                answer.IsCorrect = c.IsCorrectAnswer;
                                selectedQuestion.Answers.Add(answer);
                            }
                        });
                        var results = await QuestionAnswerManager.Insert(selectedQuestion);
                        //var selectedQuestionId = selectedQuestion.Id;

                        //await ReloadAsync();
                        //if (cboQuestions.Items.Count > 0)
                        //{
                        //    cboQuestions.SelectedValue = selectedQuestionId;
                        //}
                        if (results > 0)
                            ShowMessage("Saved!", System.Drawing.Color.SeaGreen);
                        else
                            throw new Exception("Duplicate answers are not allowed");
                    }
                    else
                    {
                        throw new Exception("No correct answer is selected");
                    }
                }
                else
                {
                    throw new Exception("A question must have at least 2 associated answers");;
                }
            }
            catch (Exception ex)
            {

                ShowMessage(ex.Message, System.Drawing.Color.LightCoral);
            }

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

    }
}

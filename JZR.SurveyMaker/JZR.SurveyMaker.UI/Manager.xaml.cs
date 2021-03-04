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
using System.Windows.Shapes;

namespace JZR.SurveyMaker.UI
{
    public enum ScreenMode { Question = 0, Answer = 1}
    public partial class Manager : Window
    {
        ScreenMode screenMode;

        List<Question> questions;
        List<Answer> answers;
        public Manager(ScreenMode screenMode)
        {
            this.screenMode = screenMode;
            InitializeComponent();
            InitialLoad();
            var adj = this.screenMode == ScreenMode.Answer ? "an" : "a";
            txtModeName.Text = $"Enter {adj} {this.screenMode}";
            this.Title = $"Manage {this.screenMode}s";
        }

        private async void InitialLoad()
        {
            await ReloadAsync();
        }

        private async Task ReloadAsync()
        {
            CloseMessageBar();
            cboAttributes.ItemsSource = null;
            txtAttribute.Text = string.Empty;
            switch (screenMode)
            {
                case ScreenMode.Question:
                    questions = await QuestionManager.Load();
                    cboAttributes.ItemsSource = questions;
                    break;
                case ScreenMode.Answer:
                    answers = await AnswerManager.Load();
                    cboAttributes.ItemsSource = answers;
                    break;
            }
            if (cboAttributes.Items.Count > 0)
                cboAttributes.SelectedIndex = 0;
        }

        private void cboAttributes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CloseMessageBar();
            if (cboAttributes.SelectedIndex > -1)
            {
                if (screenMode == ScreenMode.Question)
                    txtAttribute.Text = ((Question)cboAttributes.SelectedItem).Text;
                else
                    txtAttribute.Text = ((Answer)cboAttributes.SelectedItem).Text;
            }
        }

        private void txtAttribute_TextChanged(object sender, TextChangedEventArgs e)
        {
            CloseMessageBar();
            if (screenMode == ScreenMode.Question)
            {
                btnAdd.IsEnabled = !questions.Any(q => q.Text.ToUpper() == txtAttribute.Text.ToUpper()) && !string.IsNullOrEmpty(txtAttribute.Text);
                
            }
            else
            {
                btnAdd.IsEnabled = !answers.Any(a => a.Text.ToUpper() == txtAttribute.Text.ToUpper()) && !string.IsNullOrEmpty(txtAttribute.Text);
            }
            btnUpdate.IsEnabled = btnAdd.IsEnabled;
        }

        private async void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            CloseMessageBar();
            try
            {
                if (!string.IsNullOrEmpty(txtAttribute.Text))
                {
                    if (screenMode == ScreenMode.Question)
                    {
                        var question = new Question() { Text = txtAttribute.Text };
                        await QuestionManager.Insert(question);
                        await ReloadAsync();
                        cboAttributes.SelectedValue = question.Id;
                        ShowMessage("Question Added", Brushes.SeaGreen);
                    }
                    else
                    {
                        var answer = new Answer() { Text = txtAttribute.Text };
                        await AnswerManager.Insert(answer);
                        await ReloadAsync();
                        await Task.Delay(100);
                        cboAttributes.SelectedValue = answer.Id;
                        ShowMessage("Answer Added", Brushes.SeaGreen);

                    }
                }
            }
            catch (Exception ex)
            {

                ShowMessage(ex.Message, Brushes.LightCoral);
            }
        }

        private async void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            CloseMessageBar();
            try
            {
                if (cboAttributes.SelectedIndex > -1)
                {

                    if (screenMode == ScreenMode.Question)
                    {
                        var question = (Question)cboAttributes.SelectedItem;
                        question.Text = txtAttribute.Text;
                        await QuestionManager.Update(question);
                        await ReloadAsync();
                        cboAttributes.SelectedValue = question.Id;
                        ShowMessage("Question Updated", Brushes.CornflowerBlue);
                    }
                    else
                    {
                        var answer = (Answer)cboAttributes.SelectedItem;
                        answer.Text = txtAttribute.Text;
                        await AnswerManager.Update(answer);
                        await ReloadAsync();
                        cboAttributes.SelectedValue = answer.Id;
                        ShowMessage("Answer Updated", Brushes.CornflowerBlue);
                    }

                }
            }
            catch (Exception ex)
            {

                ShowMessage(ex.Message, Brushes.LightCoral);
            }
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            CloseMessageBar();
            try
            {
                if (cboAttributes.SelectedIndex > -1)
                {
                    var results = MessageBox.Show("Are you sure you want to delete?", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (results == MessageBoxResult.Yes)
                    {
                        if (screenMode == ScreenMode.Question)
                        {
                            var question = (Question)cboAttributes.SelectedItem;
                            await QuestionManager.Delete(question.Id);
                            await ReloadAsync();
                            ShowMessage("Question Deleted", Brushes.Orange);

                        }
                        else
                        {
                            var answer = (Answer)cboAttributes.SelectedItem;
                            await AnswerManager.Delete(answer.Id);
                            await ReloadAsync();
                            ShowMessage("Answer Deleted", Brushes.Orange);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                ShowMessage(ex.Message, Brushes.LightCoral);
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
    }
}

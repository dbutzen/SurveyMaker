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
    public partial class MaintainAttributes : Window
    {
        ScreenMode screenMode;

        List<Question> questions;
        List<Answer> answers;
        public MaintainAttributes(ScreenMode screenMode)
        {
            this.screenMode = screenMode;
            InitializeComponent();

            ReloadAsync();

            txtModeName.Text = $"{this.screenMode}s";
            this.Title = $"Maintain {this.screenMode}s";
            txtModeName.Text = this.Title;
        }

        private async void ReloadAsync()
        {
            cboAttributes.ItemsSource = null;
            txtAttribute.Text = string.Empty;
            switch (screenMode)
            {
                case ScreenMode.Question:
                    questions = (List<Question>)await QuestionManager.Load();
                    cboAttributes.ItemsSource = questions;
                    break;
                case ScreenMode.Answer:
                    answers = (List<Answer>)await AnswerManager.Load();
                    cboAttributes.ItemsSource = answers;
                    break;
            }
            if (cboAttributes.Items.Count > 0)
                cboAttributes.SelectedIndex = 0;
        }

        private void cboAttributes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
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
            
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (screenMode == ScreenMode.Question)
                txtAttribute.Text = ((Question)cboAttributes.SelectedItem).Text;
            else
                txtAttribute.Text = ((Answer)cboAttributes.SelectedItem).Text;
        }

        private async void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (cboAttributes.SelectedIndex > -1)
            {

                if (screenMode == ScreenMode.Question)
                {
                    var question = (Question)cboAttributes.SelectedItem;
                    question.Text = txtAttribute.Text;
                    await QuestionManager.Update(question);
                    ReloadAsync();
                    cboAttributes.SelectedValue = question.Id;
                }
                else
                {
                    var answer = (Answer)cboAttributes.SelectedItem;
                    answer.Text = txtAttribute.Text;
                    await AnswerManager.Update(answer);
                    ReloadAsync();
                    cboAttributes.SelectedValue = answer.Id;
                }

            }
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (cboAttributes.SelectedIndex > -1)
            {
                var results = MessageBox.Show("Are you sure you want to delete?","Delete",MessageBoxButton.YesNo,MessageBoxImage.Question);
                if (results == MessageBoxResult.Yes)
                {
                    if (screenMode == ScreenMode.Question)
                    {
                        var question = (Question)cboAttributes.SelectedItem;
                        await QuestionManager.Delete(question.Id);
                        ReloadAsync();

                    }
                    else
                    {
                        var answer = (Answer)cboAttributes.SelectedItem;
                        await AnswerManager.Delete(answer.Id);
                        ReloadAsync();
                    }
                }
            }
        }
    }
}

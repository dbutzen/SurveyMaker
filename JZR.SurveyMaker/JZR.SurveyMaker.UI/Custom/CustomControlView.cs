using JZR.SurveyMaker.BL.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JZR.SurveyMaker.UI.Custom
{
    public class CustomControlView
    {
        public int Id { get; set; }
        public bool IsCorrectAnswer { get; set; }
        public Answer SelectedAnswer { get; set; } 
        public ObservableCollection<Answer> Answers { get; set; }

    }
}

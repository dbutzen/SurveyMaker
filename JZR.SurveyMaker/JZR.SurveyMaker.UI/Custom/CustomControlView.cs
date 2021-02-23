using JZR.SurveyMaker.BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JZR.SurveyMaker.UI.Custom
{
    public class CustomControlView
    {
        public bool IsCorrectAnswer { get; set; }
        public Answer SelectedAnswer { get; set; } 
        public List<Answer> Answers { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JZR.SurveyMaker.BL.Models
{
    public class Question
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public List<Answer> Answers { get; set; } = new List<Answer>();
        public List<Activation> Activations { get; set;} 

        public Answer CorrectAnswer
        {
            get
            {
                return Answers.FirstOrDefault(a => a.IsCorrect);
            }
        }
    }
}

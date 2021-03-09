using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JZR.SurveyMaker.BL.Models
{
    public class Activation
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ActivationCode { get; set;} 

        public string QuestionText { get; set; }
    }
}

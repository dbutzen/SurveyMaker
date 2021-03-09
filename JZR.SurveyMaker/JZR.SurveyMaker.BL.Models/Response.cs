using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JZR.SurveyMaker.BL.Models
{
    public class Response
    {
        public Guid Id { get; set; }
        public Guid QuestionId{ get; set; }
        public Guid AnswerId { get; set; }
        public DateTime ResponseDate { get; set; }
        public List<Answer> Answers { get; set; } = new List<Answer>();
                
    }
}

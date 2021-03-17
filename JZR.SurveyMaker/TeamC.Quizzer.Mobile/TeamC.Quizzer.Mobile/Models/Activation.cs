using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamC.Quizzer.Mobile.Models
{
    public class Activation
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ActivationCode { get; set;} 

        public string QuestionText { get; set; }

        // Additional Property
        public string Status
        {
            get 
            {
                if (DateTime.Today >= StartDate && DateTime.Today <= EndDate)
                    return "Active";
                else if (DateTime.Today < StartDate)
                    return "Inactive";
                else
                    return "Expired";
            }
        }
    }
}

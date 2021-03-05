using System;
using System.Collections.Generic;

#nullable disable

namespace JZR.SurveyMaker.PL
{
    public partial class tblActivation
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ActivationCode { get; set; }

        public virtual tblQuestion Question { get; set; }
    }
}

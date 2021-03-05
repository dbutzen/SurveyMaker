using System;
using System.Collections.Generic;

#nullable disable

namespace JZR.SurveyMaker.PL
{
    public partial class tblAnswer
    {
        public tblAnswer()
        {
            TblQuestionAnswers = new HashSet<tblQuestionAnswer>();
            TblResponses = new HashSet<tblResponse>();
        }

        public Guid Id { get; set; }
        public string Answer { get; set; }

        public virtual ICollection<tblQuestionAnswer> TblQuestionAnswers { get; set; }
        public virtual ICollection<tblResponse> TblResponses { get; set; }
    }
}

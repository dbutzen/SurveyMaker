using System;
using System.Collections.Generic;

#nullable disable

namespace JZR.SurveyMaker.PL
{
    public partial class tblQuestion
    {
        public tblQuestion()
        {
            TblQuestionAnswers = new HashSet<tblQuestionAnswer>();
        }

        public Guid Id { get; set; }
        public string Question { get; set; }

        public virtual ICollection<tblQuestionAnswer> TblQuestionAnswers { get; set; }
    }
}

using System;
using System.Collections.Generic;

#nullable disable

namespace JZR.SurveyMaker.PL
{
    public partial class TblAnswer
    {
        public TblAnswer()
        {
            TblQuestionAnswers = new HashSet<TblQuestionAnswer>();
        }

        public Guid Id { get; set; }
        public string Answer { get; set; }

        public virtual ICollection<TblQuestionAnswer> TblQuestionAnswers { get; set; }
    }
}

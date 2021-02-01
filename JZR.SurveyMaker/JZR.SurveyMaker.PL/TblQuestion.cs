using System;
using System.Collections.Generic;

#nullable disable

namespace JZR.SurveyMaker.PL
{
    public partial class TblQuestion
    {
        public TblQuestion()
        {
            TblQuestionAnswers = new HashSet<TblQuestionAnswer>();
        }

        public Guid Id { get; set; }
        public string Question { get; set; }

        public virtual ICollection<TblQuestionAnswer> TblQuestionAnswers { get; set; }
    }
}

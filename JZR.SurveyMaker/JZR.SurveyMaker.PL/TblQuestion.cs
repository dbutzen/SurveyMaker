using System;
using System.Collections.Generic;

#nullable disable

namespace JZR.SurveyMaker.PL
{
    public partial class tblQuestion
    {
        public tblQuestion()
        {
            TblActivations = new HashSet<tblActivation>();
            TblQuestionAnswers = new HashSet<tblQuestionAnswer>();
            TblResponses = new HashSet<tblResponse>();
        }

        public Guid Id { get; set; }
        public string Question { get; set; }

        public virtual ICollection<tblActivation> TblActivations { get; set; }
        public virtual ICollection<tblQuestionAnswer> TblQuestionAnswers { get; set; }
        public virtual ICollection<tblResponse> TblResponses { get; set; }
    }
}

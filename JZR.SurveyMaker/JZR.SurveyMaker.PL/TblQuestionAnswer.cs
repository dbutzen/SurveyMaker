using System;
using System.Collections.Generic;

#nullable disable

namespace JZR.SurveyMaker.PL
{
    public partial class TblQuestionAnswer
    {
        public Guid Id { get; set; }
        public Guid AnswerId { get; set; }
        public Guid QuestionId { get; set; }
        public bool IsCorrect { get; set; }

        public virtual TblAnswer Answer { get; set; }
        public virtual TblQuestion Question { get; set; }
    }
}

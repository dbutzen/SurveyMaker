using System;
using System.Collections.Generic;
using System.Text;
using TeamC.Quizzer.Mobile.Models;

namespace TeamC.Quizzer.Mobile.ViewModels
{
    public class ResponseViewModel
    {
        public bool IsSelected { get; set; }
        public string Letter { get; set; }
        public Answer Answer { get; set; }

        public string DisplayLetter { get { return $"{Letter})"; } }
    }
}

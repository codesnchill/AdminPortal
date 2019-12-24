using System;
using System.Collections.Generic;

namespace AdminPortal.Models
{
    public partial class QuestionChoice
    {
        public int ChoiceId { get; set; }
        public string Choice { get; set; }
        public int GameId { get; set; }
        public int QuestionId { get; set; }

        public string Answer { get; set; }
        public string Question { get; set; }


     
    }
}

using System;
using System.Collections.Generic;

namespace AdminPortal.Models
{
    public partial class Question
    {
        public Question()
        {
            Choices = new HashSet<QuestionChoice>();
        }

        public int QuestionId { get; set; }
        public int Gameid { get; set; }
        public string Question1 { get; set; }
        public int Points { get; set; }
        public string ImageUrl { get; set; }
        public string Answer { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public byte[] UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string DeletedBy { get; set; }

        public Game Game { get; set; }
        public ICollection<QuestionChoice> Choices { get; set; }
    }
}

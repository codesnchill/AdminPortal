using System;
using System.Collections.Generic;

namespace AdminPortal.Models
{
    public partial class Game
    {
        public Game()
        {
            Question = new HashSet<Question>();
        }

        public int GameId { get; set; }
        public string GameName { get; set; }

        public ICollection<Question> Question { get; set; }
    }
}

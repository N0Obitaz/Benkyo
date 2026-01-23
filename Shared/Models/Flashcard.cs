using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class Flashcard
    {
        public string? Id { get; set; }
        public string? Question { get; set; }
        public string? Answer { get; set; }


        public string? StudysetId { get; set; }
        public string? FlashcardColor { get; set; }

        public string? Tag { get; set; }


    }
}

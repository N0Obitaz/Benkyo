using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class Studyset
    {
        public string? Id { get; set; }

        public string? StudySetName { get; set; }
        
        //changed Lesson list into flashcard only
        public List<Flashcard>? Flashcards{ get; set; }

        public string? UserId { get; set; }

        public string? StudySetColor { get; set; }
    }
}

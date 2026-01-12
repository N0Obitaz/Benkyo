using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class Lesson
    {
        public string? Id { get; set; }
        public string? LessonName { get; set; }
        public List<Flashcard>? Flashcards { get; set; }

        public string? StudysetId { get; set; }
    }
}

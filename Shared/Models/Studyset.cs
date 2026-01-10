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

        public List<Lesson>? Lessons{ get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{


    public class ColorOption
    {

        public string Id { get; set; } = string.Empty;     // e.g., "blue"
        public string Name { get; set; } = string.Empty;   // e.g., "Mathematics"
        public string Hex { get; set; } = string.Empty;    // e.g., "#60a5fa"
        public string TailwindClass { get; set; } = string.Empty; // e.g., "bg-study-blue"



    }

    public class ColorOptions
    {
        public readonly List<ColorOption> Options = new()
        {
            new() { Id = "blue", Name = "Logic", Hex = "#60a5fa", TailwindClass = "bg-study-blue" },
        new() { Id = "green", Name = "Nature", Hex = "#34d399", TailwindClass = "bg-study-green" },
        new() { Id = "purple", Name = "Arts", Hex = "#a78bfa", TailwindClass = "bg-study-purple" },
        new() { Id = "rose", Name = "Urgent", Hex = "#fb7185", TailwindClass = "bg-study-rose" },
        new() { Id = "amber", Name = "Social", Hex = "#f59e0b", TailwindClass = "bg-study-amber" },
        new() { Id = "slate", Name = "General", Hex = "#94a3b8", TailwindClass = "bg-study-slate" }
        };
        public ColorOption GetById(string id) =>
                        Options.FirstOrDefault(option => option.Id.Equals(id, StringComparison.OrdinalIgnoreCase))
                        ?? Options[0]; 
    }


}

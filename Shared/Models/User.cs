using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class User
    {
        public string? Uid { get; set; }
        public string? Token { get; set; } = string.Empty;
        public DateTime? Expiration { get; set; }
        public string? Email { get; set; }

        public string? Name { get; set; }

        public string? Password { get; set; }

        public int Experience { get; set; }

        public int Level { get; set; }

        public string? Role { get; set; }
    }
}

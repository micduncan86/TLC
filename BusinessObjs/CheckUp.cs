using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLC.Data
{
    public class CheckUp: BaseEntity
    {
        public int CheckUpId { get; set; }
        public int TeamId { get; set; }
        public int TeamMemberId { get; set; }
        public DateTime CheckUpDate { get; set; }
        public string Method { get; set; }
        public string Outcome { get; set; }
        public bool RequiresAction { get; set; }
        public string Actions { get; set; }
    }
}

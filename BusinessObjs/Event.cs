using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace TLC.Data
{
    public class Event
    {
        public int EventId { get; set; }
        public string Title { get; set; }
        public DateTime EventDate { get; set; }
        public bool isComplete { get; set; }
        public int TeamId { get; set; }
        public string Notes { get; set; }

        [NotMapped]
        public Team Team { get; set; }
      
    }
}

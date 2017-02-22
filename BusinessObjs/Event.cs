using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace TLC.Data
{
    public class Event : BaseEntity
    {
        public int EventId { get; set; }
        public int TeamId { get; set; }
        public string Title { get; set; }
        public DateTime EventDate { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public bool Completed { get; set; }
        public bool Cancelled { get; set; }
        

      [NotMapped]
      public string Status
        {
            get
            {
                return !Completed && !Cancelled ? "Pending" : !Completed ? "Cancelled" : "Completed";
            }
        }
        [NotMapped]
        public Team Team { get; set; }
      
    }
}

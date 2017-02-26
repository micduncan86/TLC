using System;

namespace TLC.Data
{
    public class BaseEntity
    {
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public DateTime? AddedDate { get; set; }
        public int? AddedById { get; set; }        
    }
}

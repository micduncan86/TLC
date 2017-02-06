using System;

namespace TLC.Data
{
    public class BaseEntity
    {
        private DateTime? dateAdded = null;
        public DateTime AddedDate
        {
            get
            {
                return this.dateAdded.HasValue ? this.dateAdded.Value : DateTime.Now;
            }
            set
            {
                this.dateAdded = value;
            }
        }
        public int AddedById { get; set; }
    }
}

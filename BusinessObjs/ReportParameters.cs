using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLC.Data
{
    public partial class ReportParameters
    {
        #region Properties
        public string Report { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TeamId { get; set; }
        public int MemberId { get; set; }

        #endregion

    }
}

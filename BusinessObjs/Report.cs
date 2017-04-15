using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TLC.Data
{
    public partial class Report
    {
        #region Properties
        public int ReportId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }

        [NotMapped]
        public ReportRepository.rptNames ReportType
        {
            get
            {
                return ReportRepository.ReportType(this.Name);                
            }
        }
        #endregion

        #region Constructors
        public Report() : this(string.Empty, string.Empty, string.Empty)
        {

        }
        public Report(string _name, string _description = "", string _fileName = "")
        {
            Name = _name;
            Description = _description;
            FileName = _fileName;
            ReportId = -1;
        }
        #endregion

        #region Methods
     
        public object GetData(ReportParameters rptParams = null)
        {
            return ReportRepository.GetData(this.ReportType, rptParams);
        }

        #endregion


    }
}

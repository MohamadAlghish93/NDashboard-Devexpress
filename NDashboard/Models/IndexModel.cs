using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NDashboard.Models
{
    public class IndexModel
    {
        public ReportModel[] Reports { get; set; }
        public string SelectedReportUrl { get; set; }
    }
}
﻿using System.Collections.Generic;
using DevExpress.XtraReports.Web.Extensions;
using DevExpress.Xpo;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Web.Extensions;
using NDashboard.DAL;
using System.Linq;
using System.IO;
using System.Web.Hosting;
using NDashboard.Constant;
using HelperNVS.FileManagement;

namespace NDashboard
{
    public class ReportStorageWebExtension1 : DevExpress.XtraReports.Web.Extensions.ReportStorageWebExtension
    {
        public override bool CanSetData(string url)
        {
            // Check if the URL is available in the report storage.
            using (var session = SessionFactory.Create())
            {
                return session.GetObjectByKey<ReportEntity>(url) != null;
            }
        }
        public override bool IsValidUrl(string url)
        {
            // Check if the specified URL is valid for the current report storage.
            // In this example, a URL should be a string containing a numeric value that is used as a data row primary key.
            return true;
        }

        public override byte[] GetData(string url)
        {
            // Get the report data from the storage.
            using (var session = SessionFactory.Create())
            {
                var report = session.GetObjectByKey<ReportEntity>(url);
                MemoryStream vs = this.ReadFromFile(url);
                return vs.ToArray();
                //return report.Layout;
            }
        }

        public override Dictionary<string, string> GetUrls()
        {
            // Get URLs and display names for all reports available in the storage
            using (var session = SessionFactory.Create())
            {
                return session.Query<ReportEntity>().ToDictionary<ReportEntity, string, string>(report => report.Url, report => report.Url);
            }
        }

        public override void SetData(DevExpress.XtraReports.UI.XtraReport report, string url)
        {
            // Write a report to the storage under the specified URL.
            using (var session = SessionFactory.Create())
            {
                var reportEntity = session.GetObjectByKey<ReportEntity>(url);
               

                MemoryStream ms = new MemoryStream();
                report.SaveLayout(ms);
                //reportEntity.Layout = ms.ToArray();
                this.SaveToFile(reportEntity.Url, ms);

                session.CommitChanges();
            }
        }

        public override string SetNewData(DevExpress.XtraReports.UI.XtraReport report, string defaultUrl)
        {
            // Save a report to the storage under a new URL. 
            // The defaultUrl parameter contains the report display name specified by a user.
            if (CanSetData(defaultUrl))
                SetData(report, defaultUrl);
            else
                using (var session = SessionFactory.Create())
                {
                    MemoryStream ms = new MemoryStream();
                    report.SaveLayout(ms);

                    var reportEntity = new ReportEntity(session)
                    {
                        Url = defaultUrl
                    };
                    this.SaveToFile(reportEntity.Url, ms);
                    session.CommitChanges();
                }
            return defaultUrl;
        }

        public void SaveToFile(string name, MemoryStream ms)
        {
            string currentPath = HostingEnvironment.ApplicationPhysicalPath;
            string messageError = string.Empty;
            currentPath = Path.Combine(currentPath, ApplicationConstant.LOCATION_REPORT_SAVE_NAME);
            name = name + ApplicationConstant.EXETENSTION_REPORT_SAVE_NAME;
            currentPath = Path.Combine(currentPath, name);
            FileManagement fileManagement = new FileManagement();
            if (fileManagement.MemoryStreamWrite(currentPath, ms, ref messageError))
            {

            }
        }

        public MemoryStream ReadFromFile(string name)
        {
            string currentPath = HostingEnvironment.ApplicationPhysicalPath;
            string messageError = string.Empty;
            currentPath = Path.Combine(currentPath, ApplicationConstant.LOCATION_REPORT_SAVE_NAME);
            name = name + ApplicationConstant.EXETENSTION_REPORT_SAVE_NAME;
            currentPath = Path.Combine(currentPath, name);
            FileManagement fileManagement = new FileManagement();
            return (fileManagement.MemoryStreamRead(currentPath, ref messageError));
        }
    }
}

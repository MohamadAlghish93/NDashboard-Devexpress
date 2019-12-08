using DevExpress.Xpo;
using NDashboard.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NDashboard.Models;
using DevExpress.Web.Mvc;
using DevExpress.XtraReports.Native;
using DevExpress.XtraReports.UI;
using DevExpress.DataAccess.Sql;
using System.Web.Configuration;
using HelperNVS.FileManagement;
using HelperNVS.DataBaseManagement;
using System.Web.Hosting;
using System.IO;
using NDashboard.Constant;

namespace NDashboard.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index() 
        {
            var connectionString = WebConfigurationManager.ConnectionStrings["ConnectionString"];
            if (connectionString == null)
            {
                return RedirectToAction("Index", "Setting");
            }
            if (string.IsNullOrWhiteSpace(connectionString.ConnectionString))
            {
                return RedirectToAction("Index", "Setting");
            }

            using (var session = SessionFactory.Create())
            {
                var reports = session.Query<ReportEntity>()
                    .Select(x => new ReportModel
                    {
                        Url = x.Url
                    })
                    .ToArray();
                var firstReport = reports.FirstOrDefault();
                var model = new IndexModel
                {
                    SelectedReportUrl = firstReport != null ? firstReport.Url : String.Empty,
                    Reports = reports,
                    EnableEdit = true
                };

                return View("Index", model);
            }
        }

        //[HttpGet]
        //public ActionResult Index(string positionUID)
        //{
        //    var connectionString = WebConfigurationManager.ConnectionStrings["ConnectionString"];
        //    if (connectionString == null)
        //    {
        //        return RedirectToAction("Index", "Setting");
        //    }
        //    if (string.IsNullOrWhiteSpace(connectionString.ConnectionString))
        //    {
        //        return RedirectToAction("Index", "Setting");
        //    }

        //    using (var session = SessionFactory.Create())
        //    {
        //        var reports = session.Query<ReportEntity>()
        //            .Select(x => new ReportModel
        //            {
        //                Url = x.Url
        //            })
        //            .ToArray();
        //        var firstReport = reports.FirstOrDefault();
        //        var model = new IndexModel
        //        {
        //            SelectedReportUrl = firstReport != null ? firstReport.Url : String.Empty,
        //            Reports = reports,
        //            EnableEdit = false
        //        };

        //        //
        //        DataBaseManagement dataBase = new DataBaseManagement();
        //        List<long> AttrList = dataBase.GetUserPositionAttributes(positionUID, connectionString.ConnectionString);
        //        foreach (var item in AttrList)
        //        {
        //            long val = item & ApplicationConstant.ATT_PREMISSION_VAL;
        //            if (val == ApplicationConstant.ATT_PREMISSION_VAL) 
        //            {
        //                model.EnableEdit = true;
        //                break;
        //            }
        //        }

        //        //

        //        return View("Index", model);
        //    }
        //}

        [HttpGet]
        public ActionResult Delete(string url)
        {
            using (var session = SessionFactory.Create())
            {
                var report = session.GetObjectByKey<ReportEntity>(url);
                string currentPath = HostingEnvironment.ApplicationPhysicalPath;
                string messageError = string.Empty;
                currentPath = Path.Combine(currentPath, ApplicationConstant.LOCATION_REPORT_SAVE_NAME);
                url = url + ApplicationConstant.EXETENSTION_REPORT_SAVE_NAME;
                currentPath = Path.Combine(currentPath, url);
                FileManagement fileManagement = new FileManagement();
                fileManagement.DeleteFileByPath(currentPath,ref messageError);
                
                session.Delete(report);
                session.CommitChanges();
            }
            return Index();
        }

        [HttpGet]
        public ActionResult Design(string url)
        {
            return View("Design", new DesignModel { Url = url, DataSource = CreateSqlDataSource() });
        }

        [HttpGet]
        public ActionResult New()
        {
            return View("New", new DesignModel { Url = string.Empty, DataSource = CreateSqlDataSource() });
        }

        [HttpGet]
        public ActionResult ViewExist(string url)
        {

            return View("ViewExist", new DesignModel { Url = url, DataSource = CreateSqlDataSource() });
        }

        SqlDataSource CreateSqlDataSource()
        {
            SqlDataSource ds = new SqlDataSource("ConnectionString");
            ds.Name = "TarasolDB";
            //ds.Queries.Add(new CustomSqlQuery("CUSTOMER", "SELECT * FROM [CUSTOMER]"));
            ds.RebuildResultSchema();
            return ds;
        }
    }
}
using NDashboard.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using DevExpress.XtraReports.Native;
using DevExpress.XtraReports.UI;
using NDashboard.Models;
using DevExpress.Xpo;
using DevExpress.DataAccess.Sql;

namespace NDashboard.Controllers
{
    public class DesignerController : Controller
    {
        // GET: Designer
        [HttpGet]
        public ActionResult Index()
        {
            using (var session = SessionFactory.Create())
            {
                var reports = session.Query<TSOUTERREPORT>()
                    .Select(x => new ReportModel
                    {
                        Url = x.Url
                    })
                    .ToArray();
                var firstReport = reports.FirstOrDefault();
                var model = new IndexModel
                {
                    SelectedReportUrl = firstReport != null ? firstReport.Url : String.Empty,
                    Reports = reports
                };
                return View("Index", model);
            }
        }

        [HttpPost]
        public ActionResult Delete(string url)
        {
            using (var session = SessionFactory.Create())
            {
                var report = session.GetObjectByKey<TSOUTERREPORT>(url);
                session.Delete(report);

                session.CommitChanges();
            }
            return Index();
        }

        [HttpPost]
        public ActionResult Design(string url)
        {
            return View("Design", new DesignModel { Url = url, DataSource = CreateSqlDataSource() });
        }

        [HttpPost]
        public ActionResult New(string url)
        {
            return View("New", new DesignModel { Url = url, DataSource = CreateSqlDataSource() });
        }

        [HttpPost]
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
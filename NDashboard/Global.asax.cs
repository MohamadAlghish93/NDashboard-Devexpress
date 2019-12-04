using DevExpress.XtraReports.Security;
using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace NDashboard {
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication {
        protected void Application_Start() {
            DevExpress.XtraReports.Web.QueryBuilder.Native.QueryBuilderBootstrapper.SessionState = System.Web.SessionState.SessionStateBehavior.ReadOnly;
            DevExpress.XtraReports.Web.ReportDesigner.Native.ReportDesignerBootstrapper.SessionState = System.Web.SessionState.SessionStateBehavior.ReadOnly;
            DevExpress.XtraReports.Web.ReportDesigner.DefaultReportDesignerContainer.EnableCustomSql();
            
            DevExpress.XtraReports.Web.WebDocumentViewer.Native.WebDocumentViewerBootstrapper.SessionState = System.Web.SessionState.SessionStateBehavior.Disabled;
            AreaRegistration.RegisterAllAreas();

            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            
            ModelBinders.Binders.DefaultBinder = new DevExpress.Web.Mvc.DevExpressEditorsBinder();

            string culture = "ar";
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(culture);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(culture);

            DevExpress.Web.ASPxWebControl.CallbackError += Application_Error;
            DevExpress.Web.Mvc.MVCxWebDocumentViewer.StaticInitialize();
            DevExpress.Web.Mvc.MVCxReportDesigner.StaticInitialize();
            DevExpress.XtraReports.Web.Extensions.ReportStorageWebExtension.RegisterExtensionGlobal(new ReportStorageWebExtension1());
            DevExpress.Security.Resources.AccessSettings.ReportingSpecificResources.SetRules(SerializationFormatRule.Allow(SerializationFormat.Code, SerializationFormat.Xml));
        }

        protected void Application_Error(object sender, EventArgs e) {
            Exception exception = System.Web.HttpContext.Current.Server.GetLastError();
            //TODO: Handle Exception
        }
    }
}
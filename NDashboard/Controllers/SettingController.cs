using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using NDashboard.Models;

namespace NDashboard.Controllers
{
    public class SettingController : Controller
    {
        // GET: Setting
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SaveSetting(SettingModel settingModel)
        {
            // save in XML file
            bool isNew = true;
            string name = "ConnectionString";
            string path = Server.MapPath("~/Web.Config");
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlNodeList list = doc.DocumentElement.SelectNodes(string.Format("connectionStrings/add[@name='{0}']", name));
            XmlNode node;
            isNew = list.Count == 0;
            if (isNew)
            {
                node = doc.CreateNode(XmlNodeType.Element, "add", null);
                XmlAttribute attribute = doc.CreateAttribute("name");
                attribute.Value = name;
                node.Attributes.Append(attribute);

                attribute = doc.CreateAttribute("connectionString");
                attribute.Value = "";
                node.Attributes.Append(attribute);

                attribute = doc.CreateAttribute("providerName");
                attribute.Value = "System.Data.SqlClient";
                node.Attributes.Append(attribute);
            }
            else
            {
                node = list[0];
            }
            string conString = node.Attributes["connectionString"].Value;
            SqlConnectionStringBuilder conStringBuilder = new SqlConnectionStringBuilder(conString);
            conStringBuilder.InitialCatalog = settingModel.DatabaseName;
            conStringBuilder.DataSource = settingModel.ServerName;
            conStringBuilder.IntegratedSecurity = false;
            conStringBuilder.UserID = settingModel.UserName;
            conStringBuilder.Password = settingModel.Password;
            conStringBuilder.PersistSecurityInfo = true;
            node.Attributes["connectionString"].Value = conStringBuilder.ConnectionString;
            if (isNew)
            {
                doc.DocumentElement.SelectNodes("connectionStrings")[0].AppendChild(node);
            }
            doc.Save(path);
            //


            var config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
            if (config.AppSettings.Settings["TarasolLink"] != null)
            {
                config.AppSettings.Settings["TarasolLink"].Value = settingModel.tarasolURL;
            }
            else { 
                config.AppSettings.Settings.Add("TarasolLink", settingModel.tarasolURL); 
            }
            config.Save();

            return Json(new { success = true });
        }
    }
}
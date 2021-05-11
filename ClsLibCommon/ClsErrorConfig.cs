using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

namespace ClsCommon
{
    public class ClsErrorConfig 
    {
        public Exception SendException(Exception exs, string SystemName)
        {
            //Session["ComputerUser"] = "<b>User:</b> " + Environment.UserDomainName + @"\" + Environment.UserName;

            //Session["ComputerName"] = "<b>Computer Name:</b> " + Environment.MachineName;

            //Session["Message"] = "<b>Error Message:</b> " + exs.Message;

            //Session["TargetSite"] = "<b>Target Event Method:</b> " + exs.TargetSite;

            //Session["StackTrace"] = "<b>Error Details:</b> " + exs.StackTrace.ToString();

            //Session["SystemName"] = "Document Control Management System";

            //Session["WebErrorUrl"] = HttpContext.Current.Request.Url.AbsoluteUri.ToString();

            //Control.Dispose();

            //Session["url"] = null;

            //Session["url"] = Request.UrlReferrer.AbsoluteUri.ToString();

            //Response.Redirect(ConfigurationManager.AppSettings["ORWebAddress"].ToString()

            //    + ConfigurationManager.AppSettings["PageError"].ToString());
            
            return exs;
        }
    }
}

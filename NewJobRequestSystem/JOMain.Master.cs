using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NewJobRequestSystem
{
    public partial class JOMain : System.Web.UI.MasterPage
    {
        private void Show(string msg)
        {
            Page page = HttpContext.Current.Handler as Page;
            if (page != null)
            {
                ScriptManager.RegisterStartupScript(page, page.GetType(), "Message", "alert('" + msg + "');", true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["EmpID"] == null)
            {
                Response.Redirect("Default.aspx");
            }

            lblName.InnerText = //Session["UserRole"].ToString();
            Session["FullName_LnameFirst"].ToString();
        }

        protected void lnkHome(object sender, EventArgs e)
        {
            if (Session["InchargeID"] != null && !string.IsNullOrEmpty(Session["InchargeID"].ToString().Trim()))
            {
                Response.Redirect("AdminHome.aspx");
            }
            else
            {
                Response.Redirect("HomePage.aspx");
            }
        }

        protected void Logout(object sender, EventArgs e)
        {
            Session.Clear();

            Session.Abandon();

            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            Response.Cache.SetNoStore();

            Response.Redirect("Default.aspx");
        }
    }
}
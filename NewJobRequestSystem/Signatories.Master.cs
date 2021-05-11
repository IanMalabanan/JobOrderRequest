using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NewJobRequestSystem
{
    public partial class Signatories : System.Web.UI.MasterPage
    {
        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["FName"] != null)
            {
                lblUser.InnerText = Session["FName"].ToString().Trim();
                lblPosition.InnerText = Session["Position"].ToString().Trim();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClsCommon;
using Telerik.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.Data.Entity.SqlServer;
using System.ComponentModel;
using System.Data.Entity;
using System.Configuration;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Drawing;

namespace NewJobRequestSystem
{
    public partial class HomePage : System.Web.UI.Page
    {
        JobRequestDBEntities dbContext = new JobRequestDBEntities();

        HRISEntities dbHRISContext = new HRISEntities();

        ClsGenerateRandomString random = new ClsGenerateRandomString();

        public static string filename = string.Empty, filetype = string.Empty
            , approverempid = string.Empty, approveremail = string.Empty
            , approverposition = string.Empty, approverdeptcode = string.Empty
            , approverdeptname = string.Empty, approversectcode = string.Empty
            , approversectname = string.Empty, approverfullname_lnamefirst = string.Empty
            , approverfullname_fnamefirst = string.Empty, userempid = string.Empty
            , useremail = string.Empty, incharge = string.Empty, jrcode = string.Empty;

        protected void gridMyRecords_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                Label status = (e.Item.FindControl("txtStatus") as Label);

                GridDataItem item = e.Item as GridDataItem;

                if (status.Text.Trim() == "Completed")
                {
                    e.Item.Attributes.CssStyle.Value = "background-color: #44bb19;color:white";

                    item["action"].BackColor = Color.FromKnownColor(KnownColor.WhiteSmoke);
                }
                else if (status.Text.Trim() == "Denied_Cancelled")
                {
                    e.Item.Attributes.CssStyle.Value = "background-color: #da3e14; color: white";

                    item["action"].BackColor = Color.FromKnownColor(KnownColor.WhiteSmoke);
                }
                else if (status.Text.Trim() == "For Checking")
                {
                    e.Item.Attributes.CssStyle.Value = "background-color: #e68b33;color:white";

                    item["action"].BackColor = Color.FromKnownColor(KnownColor.WhiteSmoke);
                }
                else if (status.Text.Trim() == "For Assessment")
                {
                    e.Item.Attributes.CssStyle.Value = "background-color: #33E0FF;color:black";

                    item["action"].BackColor = Color.FromKnownColor(KnownColor.WhiteSmoke);
                }
                else if (status.Text.Trim() == "For Noting")
                {
                    e.Item.Attributes.CssStyle.Value = "background-color: #FFE000;color:black";

                    item["action"].BackColor = Color.FromKnownColor(KnownColor.WhiteSmoke);
                }

                else if (status.Text.Trim() == "For Approval")
                {
                    e.Item.Attributes.CssStyle.Value = "background-color: #D305F7;color:white";

                    item["action"].BackColor = Color.FromKnownColor(KnownColor.WhiteSmoke);
                }

                else if (status.Text.Trim() == "Pending")
                {
                    e.Item.Attributes.CssStyle.Value = "background-color: #778899;color:white";

                    item["action"].BackColor = Color.FromKnownColor(KnownColor.WhiteSmoke);
                }

                else if (status.Text.Trim() == "On-Going")
                {
                    e.Item.Attributes.CssStyle.Value = "background-color: #008B8B;color:white";

                    item["action"].BackColor = Color.FromKnownColor(KnownColor.WhiteSmoke);
                }

                else if (status.Text.Trim() == "For Employee Acceptance")
                {
                    e.Item.Attributes.CssStyle.Value = "background-color: #05F7CF;color:black";

                    item["action"].BackColor = Color.FromKnownColor(KnownColor.WhiteSmoke);
                }
                else if (status.Text.Trim() == "Attachment Approval")
                {
                    e.Item.Attributes.CssStyle.Value = "background-color: #b35900;color:white";

                    item["action"].BackColor = Color.FromKnownColor(KnownColor.WhiteSmoke);
                }
                else
                {

                }
            }
        }

        protected void gridDepartmentRecord_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                Label status = (e.Item.FindControl("txtStatus") as Label);

                GridDataItem item = e.Item as GridDataItem;

                if (status.Text.Trim() == "Completed")
                {
                    e.Item.Attributes.CssStyle.Value = "background-color: #44bb19;color:white";

                    item["action"].BackColor = Color.FromKnownColor(KnownColor.WhiteSmoke);
                }
                else if (status.Text.Trim() == "Denied_Cancelled")
                {
                    e.Item.Attributes.CssStyle.Value = "background-color: #da3e14; color: white";

                    item["action"].BackColor = Color.FromKnownColor(KnownColor.WhiteSmoke);
                }
                else if (status.Text.Trim() == "For Checking")
                {
                    e.Item.Attributes.CssStyle.Value = "background-color: #e68b33;color:white";

                    item["action"].BackColor = Color.FromKnownColor(KnownColor.WhiteSmoke);
                }
                else if (status.Text.Trim() == "For Assessment")
                {
                    e.Item.Attributes.CssStyle.Value = "background-color: #33E0FF;color:black";

                    item["action"].BackColor = Color.FromKnownColor(KnownColor.WhiteSmoke);
                }
                else if (status.Text.Trim() == "For Noting")
                {
                    e.Item.Attributes.CssStyle.Value = "background-color: #FFE000;color:black";

                    item["action"].BackColor = Color.FromKnownColor(KnownColor.WhiteSmoke);
                }

                else if (status.Text.Trim() == "For Approval")
                {
                    e.Item.Attributes.CssStyle.Value = "background-color: #D305F7;color:white";

                    item["action"].BackColor = Color.FromKnownColor(KnownColor.WhiteSmoke);
                }

                else if (status.Text.Trim() == "Pending")
                {
                    e.Item.Attributes.CssStyle.Value = "background-color: #778899;color:white";

                    item["action"].BackColor = Color.FromKnownColor(KnownColor.WhiteSmoke);
                }

                else if (status.Text.Trim() == "On-Going")
                {
                    e.Item.Attributes.CssStyle.Value = "background-color: #008B8B;color:white";

                    item["action"].BackColor = Color.FromKnownColor(KnownColor.WhiteSmoke);
                }

                else if (status.Text.Trim() == "For Employee Acceptance")
                {
                    e.Item.Attributes.CssStyle.Value = "background-color: #05F7CF;color:black";

                    item["action"].BackColor = Color.FromKnownColor(KnownColor.WhiteSmoke);
                }
                else if (status.Text.Trim() == "Attachment Approval")
                {
                    e.Item.Attributes.CssStyle.Value = "background-color: #b35900;color:white";

                    item["action"].BackColor = Color.FromKnownColor(KnownColor.WhiteSmoke);
                }
                
                else
                {

                }
            }
        }

        private void Show(string msg)
        {
            Page page = HttpContext.Current.Handler as Page;
            if (page != null)
            {
                ScriptManager.RegisterStartupScript(page, page.GetType(), "Message", "alert('" + msg + "');", true);
            }
        }
        
        private void LoadYear()
        {
            for (int i = DateTime.Now.Year; i >= 2010; i--)
            {
                ddlSummaryYear.Items.Add(new Telerik.Web.UI.DropDownListItem(i.ToString(), i.ToString()));
            }

            ddlSummaryYear.Items.Insert(0, new Telerik.Web.UI.DropDownListItem("Select Year", DBNull.Value.ToString()));

            ddlSummaryYear.SelectedValue = DateTime.Now.Year.ToString().TrimEnd();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadYear();
            }

            getRecords.ConnectionString = ClsConfig.JobRequestConnectionString;

            getDepartmentRecords.ConnectionString = ClsConfig.JobRequestConnectionString;

        }

        protected void lnkviewdetails_Click(object sender, EventArgs e)
        {
            LinkButton lnkviewdetails = (LinkButton)sender;

            jrcode = lnkviewdetails.CommandArgument.ToString().Trim();

            var verifyRequest = dbContext.tblRequests.Where(x => x.JR_Code == jrcode);

            foreach (var item in verifyRequest)
            {
                tblRequest tblData = item as tblRequest;

                //ForChecking
                if ((Convert.ToBoolean(tblData.IsRejected) == false && Convert.ToBoolean(tblData.IsCancelled) == false)
                    && Convert.ToBoolean(tblData.Is_Submitted) == true &&
                    Convert.ToBoolean(tblData.Is_Checked) == false &&
                    Convert.ToBoolean(tblData.Is_Assessed) == false &&
                    Convert.ToBoolean(tblData.Is_Noted) == false &&
                    Convert.ToBoolean(tblData.Is_Approved) == false &&
                    Convert.ToBoolean(tblData.Is_JobAccepted) == false &&
                    Convert.ToBoolean(tblData.IsCompleted) == false
                     )
                {
                    Response.Redirect("UserForChecking.aspx?RCode=" + jrcode);
                }
                //ForAssessment
                else
                if (Convert.ToBoolean(tblData.Is_Submitted) == true &&
                    Convert.ToBoolean(tblData.Is_Checked) == true &&
                    Convert.ToBoolean(tblData.Is_Assessed) == false &&
                    Convert.ToBoolean(tblData.Is_Noted) == false &&
                    Convert.ToBoolean(tblData.Is_Approved) == false &&
                    Convert.ToBoolean(tblData.Is_JobAccepted) == false &&
                    Convert.ToBoolean(tblData.IsCompleted) == false
                    && (Convert.ToBoolean(tblData.IsRejected) == false && Convert.ToBoolean(tblData.IsCancelled) == false))
                {
                    Response.Redirect("UserForAssessment.aspx?RCode=" + jrcode);
                }
                //ForNoting
                else if (Convert.ToBoolean(tblData.Is_Submitted) == true &&
                    Convert.ToBoolean(tblData.Is_Checked) == true &&
                    Convert.ToBoolean(tblData.Is_Assessed) == true &&
                    Convert.ToBoolean(tblData.Is_Noted) == false &&
                    Convert.ToBoolean(tblData.Is_Approved) == false &&
                    Convert.ToBoolean(tblData.Is_JobAccepted) == false &&
                    Convert.ToBoolean(tblData.IsCompleted) == false
                    && (Convert.ToBoolean(tblData.IsRejected) == false && Convert.ToBoolean(tblData.IsCancelled) == false))
                {
                    Response.Redirect("UserForNoting.aspx?RCode=" + jrcode);
                }
                //ForApproval
                else if (Convert.ToBoolean(tblData.Is_Submitted) == true &&
                    Convert.ToBoolean(tblData.Is_Checked) == true &&
                    Convert.ToBoolean(tblData.Is_Assessed) == true &&
                    Convert.ToBoolean(tblData.Is_Noted) == true &&
                    Convert.ToBoolean(tblData.Is_Approved) == false &&
                    Convert.ToBoolean(tblData.Is_JobAccepted) == false &&
                    Convert.ToBoolean(tblData.IsCompleted) == false
                    && (Convert.ToBoolean(tblData.IsRejected) == false && Convert.ToBoolean(tblData.IsCancelled) == false))
                {
                    Response.Redirect("UserForApproval.aspx?RCode=" + jrcode);
                }
                //ViewDetailsWithJONo
                else if (Convert.ToBoolean(tblData.Is_Submitted) == true &&
                    Convert.ToBoolean(tblData.Is_Checked) == true &&
                    Convert.ToBoolean(tblData.Is_Assessed) == true &&
                    Convert.ToBoolean(tblData.Is_Noted) == true &&
                    Convert.ToBoolean(tblData.Is_Approved) == true &&
                    (Convert.ToBoolean(tblData.Is_JobAccepted) == false || (Convert.ToBoolean(tblData.Is_JobAccepted) == true) &&
                    Convert.ToBoolean(tblData.IsCompleted) == false
                    && (Convert.ToBoolean(tblData.IsRejected) == false && Convert.ToBoolean(tblData.IsCancelled) == false)))

                {
                    Response.Redirect("UserJOGenerated.aspx?RCode=" + jrcode);
                }
                //ViewDenied
                else if ((Convert.ToBoolean(tblData.IsRejected) == true || Convert.ToBoolean(tblData.IsCancelled) == true))
                {
                    Response.Redirect("UserDeniedAndCancelled.aspx?RCode=" + jrcode);
                }
                //ViewCompleted
                else if (Convert.ToBoolean(tblData.Is_Submitted) == true &&
                    Convert.ToBoolean(tblData.Is_Checked) == true &&
                    Convert.ToBoolean(tblData.Is_Assessed) == true &&
                    Convert.ToBoolean(tblData.Is_Noted) == true &&
                    Convert.ToBoolean(tblData.Is_Approved) == true &&
                    Convert.ToBoolean(tblData.Is_JobAccepted) == true &&
                    Convert.ToBoolean(tblData.IsCompleted) == true
                    && (Convert.ToBoolean(tblData.IsRejected) == false && Convert.ToBoolean(tblData.IsCancelled) == false))
                {
                    Response.Redirect("UserCompleted.aspx?RCode=" + jrcode);
                }
                else
                {

                }
            }
        }
    }
}
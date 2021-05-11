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

namespace NewJobRequestSystem
{
    public partial class AdminHome : System.Web.UI.Page
    {
        JobRequestDBEntities dbContext = new JobRequestDBEntities();

        HRISEntities dbHRISContext = new HRISEntities();

        private void GetTallyCountsForDE()
        {
            int _year = Convert.ToInt32(ddlSummaryYear.SelectedValue.ToString().Trim());

            newJO.InnerText = dbContext.tblRequests.Where(x => x.Is_Submitted == true
            && x.Is_Checked == false && x.Is_Assessed == false && x.Is_Noted == false
            && x.Is_JobAccepted == false && x.Is_Approved == false && x.Is_AttachmentApproved == false && x.IsCompleted == false
            && x.IsRejected == false && x.IsCancelled == false && x.Incharge_ID == "05" && x.Request_Date.Value.Year == _year).ToList().Count.ToString();

            joForAssessment.InnerText = dbContext.tblRequests.Where(x => x.Is_Submitted == true
            && x.Is_Checked == true && x.Is_Assessed == false && x.Is_Noted == false
            && x.Is_JobAccepted == false && x.Is_Approved == false && x.Is_AttachmentApproved == false && x.IsCompleted == false
            && x.IsRejected == false && x.IsCancelled == false && x.Incharge_ID == "05" && x.Request_Date.Value.Year == _year).ToList().Count.ToString();
            
            joDenied.InnerText = dbContext.tblRequests.Where(x => (x.IsRejected == true || x.IsCancelled == true) 
            && x.Incharge_ID == "05" && x.Request_Date.Value.Year == _year).ToList().Count.ToString();

            joForNoting.InnerText = dbContext.tblRequests.Where(x => x.Is_Submitted == true
            && x.Is_Checked == true && x.Is_Assessed == true && x.Is_Noted == false
            && x.Is_JobAccepted == false && x.Is_Approved == false && x.Is_AttachmentApproved == false && x.IsCompleted == false
            && x.IsRejected == false && x.IsCancelled == false && x.Incharge_ID == "05" && x.Request_Date.Value.Year == _year).ToList().Count.ToString();
            
            joForApproval.InnerText = dbContext.tblRequests.Where(x => x.Is_Submitted == true
            && x.Is_Checked == true && x.Is_Assessed == true && x.Is_Noted == true
            && x.Is_JobAccepted == false && x.Is_Approved == false && x.Is_AttachmentApproved == false && x.IsCompleted == false
            && x.IsRejected == false && x.IsCancelled == false && x.Incharge_ID == "05"
            && x.Request_Date.Value.Year == _year).ToList().Count.ToString();
            
            joWaitingLists.InnerText = dbContext.tblRequests.Where(x => x.Is_Submitted == true
            && x.Is_Checked == true && x.Is_Assessed == true && x.Is_Noted == true
            && x.Is_JobAccepted == false && x.Is_Approved == true && x.Is_AttachmentApproved == false && x.IsCompleted == false
            && x.IsRejected == false && x.IsCancelled == false && x.Incharge_ID == "05" && x.Request_Date.Value.Year == _year).ToList().Count.ToString();

            joforAttachmentApproval.InnerText = dbContext.tblRequests.Where(x => x.Is_Submitted == true
             && x.Is_Checked == true && x.Is_Assessed == true && x.Is_Noted == true
             && x.Is_JobAccepted == true && x.Is_Approved == true && x.Is_AttachmentApproved == false && x.IsCompleted == false
             && x.IsRejected == false && x.IsCancelled == false && x.Incharge_ID == "05"
             && x.Request_Date.Value.Year == _year).ToList().Count.ToString();
            
            joInProgress.InnerText = dbContext.tblRequests.Where(x => x.Is_Submitted == true
            && x.Is_Checked == true && x.Is_Assessed == true && x.Is_Noted == true
            && x.Is_JobAccepted == true && x.Is_Approved == true && x.Is_AttachmentApproved == true && x.IsCompleted == false
            && x.IsRejected == false && x.IsCancelled == false && x.Incharge_ID == "05" && x.Request_Date.Value.Year == _year).ToList().Count.ToString();

            completeRequests.InnerText = dbContext.tblRequests.Where(x => x.Is_Submitted == true
            && x.Is_Checked == true && x.Is_Assessed == true && x.Is_Noted == true
            && x.Is_JobAccepted == true && x.Is_Approved == true && x.Is_AttachmentApproved == true && x.IsCompleted == true
            && x.IsRejected == false && x.IsCancelled == false && x.Incharge_ID == "05" && x.Request_Date.Value.Year == _year).ToList().Count.ToString();

        }

        private void GetTallyCounts()
        {
            int _year = Convert.ToInt32(ddlSummaryYear.SelectedValue.ToString().Trim());

            newJO.InnerText = dbContext.tblRequests.Where(x => x.Is_Submitted == true
            && x.Is_Checked == false && x.Is_Assessed == false && x.Is_Noted == false
            && x.Is_JobAccepted == false && x.Is_Approved == false && x.Is_AttachmentApproved == false && x.IsCompleted == false
            && x.IsRejected == false && x.IsCancelled == false && x.Incharge_ID.Trim() != "05"
            && x.Request_Date.Value.Year == _year).ToList().Count.ToString();

            joForAssessment.InnerText = dbContext.tblRequests.Where(x => x.Is_Submitted == true
            && x.Is_Checked == true && x.Is_Assessed == false && x.Is_Noted == false
            && x.Is_JobAccepted == false && x.Is_Approved == false && x.Is_AttachmentApproved == false && x.IsCompleted == false
            && x.IsRejected == false && x.IsCancelled == false && x.Incharge_ID.Trim() != "05"
            && x.Request_Date.Value.Year == _year).ToList().Count.ToString();

            joDenied.InnerText = dbContext.tblRequests.Where(x => (x.IsRejected == true || x.IsCancelled == true) 
            && x.Incharge_ID.Trim() != "05" && x.Request_Date.Value.Year == _year).ToList().Count.ToString();

            joForNoting.InnerText = dbContext.tblRequests.Where(x => x.Is_Submitted == true
            && x.Is_Checked == true && x.Is_Assessed == true && x.Is_Noted == false
            && x.Is_JobAccepted == false && x.Is_Approved == false && x.Is_AttachmentApproved == false && x.IsCompleted == false
            && x.IsRejected == false && x.IsCancelled == false && x.Incharge_ID.Trim() != "05" && x.Request_Date.Value.Year == _year).ToList().Count.ToString();

            joForApproval.InnerText = dbContext.tblRequests.Where(x => x.Is_Submitted == true
            && x.Is_Checked == true && x.Is_Assessed == true && x.Is_Noted == true
            && x.Is_JobAccepted == false && x.Is_Approved == false && x.Is_AttachmentApproved == false && x.IsCompleted == false
            && x.IsRejected == false && x.IsCancelled == false && x.Incharge_ID.Trim() != "05" && x.Request_Date.Value.Year == _year).ToList().Count.ToString();

            joWaitingLists.InnerText = dbContext.tblRequests.Where(x => x.Is_Submitted == true
            && x.Is_Checked == true && x.Is_Assessed == true && x.Is_Noted == true
            && x.Is_JobAccepted == false && x.Is_Approved == true && x.Is_AttachmentApproved == false && x.IsCompleted == false
            && x.IsRejected == false && x.IsCancelled == false && x.Incharge_ID.Trim() != "05"
            && x.Request_Date.Value.Year == _year).ToList().Count.ToString();

            joforAttachmentApproval.InnerText = dbContext.tblRequests.Where(x => x.Is_Submitted == true
             && x.Is_Checked == true && x.Is_Assessed == true && x.Is_Noted == true
             && x.Is_JobAccepted == true && x.Is_Approved == true && x.Is_AttachmentApproved == false && x.IsCompleted == false
             && x.IsRejected == false && x.IsCancelled == false && x.Incharge_ID != "05"
             && x.Request_Date.Value.Year == _year).ToList().Count.ToString();

            joInProgress.InnerText = dbContext.tblRequests.Where(x => x.Is_Submitted == true
            && x.Is_Checked == true && x.Is_Assessed == true && x.Is_Noted == true
            && x.Is_JobAccepted == true && x.Is_Approved == true && x.Is_AttachmentApproved == true && x.IsCompleted == false
            && x.IsRejected == false && x.IsCancelled == false && x.Incharge_ID.Trim() != "05"
            && x.Request_Date.Value.Year == _year).ToList().Count.ToString();

            completeRequests.InnerText = dbContext.tblRequests.Where(x => x.Is_Submitted == true
            && x.Is_Checked == true && x.Is_Assessed == true && x.Is_Noted == true
            && x.Is_JobAccepted == true && x.Is_Approved == true && x.Is_AttachmentApproved == true && x.IsCompleted == true
            && x.IsRejected == false && x.IsCancelled == false && x.Incharge_ID.Trim() != "05"
            && x.Request_Date.Value.Year == _year).ToList().Count.ToString();
        }

        private void GetTallyCountsNew()
        {
            int _year = Convert.ToInt32(ddlSummaryYear.SelectedValue.ToString().Trim());

            newJO.InnerText = dbContext.tblRequests.Where(x => x.Is_Submitted == true
            && x.Is_Checked == false 
            && x.Is_Assessed == false 
            && x.Is_Noted == false
            && x.Is_JobAccepted == false 
            && x.Is_Approved == false 
            //&& x.Is_AttachmentApproved == false 
            && x.IsCompleted == false
            && x.IsRejected == false 
            && x.IsCancelled == false 
            && x.Request_Date.Value.Year == _year).ToList().Count.ToString();

            joForAssessment.InnerText = dbContext.tblRequests.Where(x => x.Is_Submitted == true
            && x.Is_Checked == true 
            && x.Is_Assessed == false 
            && x.Is_Noted == false
            && x.Is_JobAccepted == false 
            && x.Is_Approved == false 
            //&& x.Is_AttachmentApproved == false 
            && x.IsCompleted == false
            && (x.IsRejected == false || x.IsCancelled == false) 
            && x.Request_Date.Value.Year == _year).ToList().Count.ToString();

            joDenied.InnerText = dbContext.tblRequests.Where(x => (x.IsRejected == true || x.IsCancelled == true)
            //&& x.Incharge_ID.Trim() != "05" 
            && x.Request_Date.Value.Year == _year).ToList().Count.ToString();

            joForNoting.InnerText = dbContext.tblRequests.Where(x => x.Is_Submitted == true
            && x.Is_Checked == true 
            && x.Is_Assessed == true 
            && x.Is_Noted == false
            && x.Is_JobAccepted == false 
            && x.Is_Approved == false 
            //&& x.Is_AttachmentApproved == false 
            && x.IsCompleted == false
            && x.IsRejected == false 
            && x.IsCancelled == false  
            && x.Request_Date.Value.Year == _year).ToList().Count.ToString();

            joForApproval.InnerText = dbContext.tblRequests.Where(x => x.Is_Submitted == true
            && x.Is_Checked == true 
            && x.Is_Assessed == true 
            && x.Is_Noted == true
            && x.Is_JobAccepted == false 
            && x.Is_Approved == false 
            //&& x.Is_AttachmentApproved == false 
            && x.IsCompleted == false
            && x.IsRejected == false 
            && x.IsCancelled == false  
            && x.Request_Date.Value.Year == _year).ToList().Count.ToString();

            joWaitingLists.InnerText = dbContext.tblRequests.Where(x => x.Is_Submitted == true
            && x.Is_Checked == true 
            && x.Is_Assessed == true 
            && x.Is_Noted == true
            && x.Is_JobAccepted == false 
            && x.Is_Approved == true 
            //&& x.Is_AttachmentApproved == false 
            && x.IsCompleted == false
            && x.IsRejected == false 
            && x.IsCancelled == false 
            && x.Request_Date.Value.Year == _year).ToList().Count.ToString();

            joforAttachmentApproval.InnerText = dbContext.tblRequests.Where(x => x.Is_Submitted == true
             && x.Is_Checked == true 
             && x.Is_Assessed == true 
             && x.Is_Noted == true
             && x.Is_JobAccepted == true 
             && x.Is_Approved == true 
             && x.Is_AttachmentApproved == false 
             && x.IsCompleted == false
             && x.IsRejected == false 
             && x.IsCancelled == false 
             && x.Request_Date.Value.Year == _year).ToList().Count.ToString();

            joInProgress.InnerText = dbContext.tblRequests.Where(x => x.Is_Submitted == true
            && x.Is_Checked == true 
            && x.Is_Assessed == true 
            && x.Is_Noted == true
            && x.Is_JobAccepted == true 
            && x.Is_Approved == true 
            //&& x.Is_AttachmentApproved == true 
            && x.IsCompleted == false
            && x.IsRejected == false 
            && x.IsCancelled == false 
            && x.Request_Date.Value.Year == _year).ToList().Count.ToString();

            completeRequests.InnerText = dbContext.tblRequests.Where(x => x.Is_Submitted == true
            && x.Is_Checked == true && x.Is_Assessed == true && x.Is_Noted == true
            && x.Is_JobAccepted == true 
            && x.Is_Approved == true 
            //&& x.Is_AttachmentApproved == true 
            && x.IsCompleted == true
            && x.IsRejected == false && x.IsCancelled == false 
            && x.Request_Date.Value.Year == _year).ToList().Count.ToString();
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

            //if (Session["InchargeID"].ToString().Trim() != "05")
            //{
            //    GetTallyCounts();
            //}
            //else
            //{
            //    GetTallyCountsForDE();
            //}

            GetTallyCountsNew();
        }

        protected void lnkCommands(object sender, EventArgs e)
        {
            LinkButton lnk = (LinkButton)sender;

            switch (lnk.ID)
            {
                case "lnkForChecking":
                    Response.Redirect("ForChecking.aspx?RequiredYear=" + ddlSummaryYear.SelectedValue.ToString());
                    break;
                case "lnkForAssessment":
                    Response.Redirect("Assessment.aspx?RequiredYear=" + ddlSummaryYear.SelectedValue.ToString());
                    break;
                case "lnkForNoting":
                    Response.Redirect("ForNoting.aspx?RequiredYear=" + ddlSummaryYear.SelectedValue.ToString());
                    break;
                case "lnkForApproval":
                    Response.Redirect("ForApproval.aspx?RequiredYear=" + ddlSummaryYear.SelectedValue.ToString());
                    break;
                case "lnkPending":
                    Response.Redirect("PendingJobOrder.aspx?RequiredYear=" + ddlSummaryYear.SelectedValue.ToString());
                    break;
                case "lnkAttachmentApproval":
                    Response.Redirect("AttachmentApproval.aspx?RequiredYear=" + ddlSummaryYear.SelectedValue.ToString());
                    break;
                case "lnkOnGoingJobOrder":
                    Response.Redirect("OnGoingJobOrder.aspx?RequiredYear=" + ddlSummaryYear.SelectedValue.ToString());
                    break;
                case "lnkCompleted":
                    Response.Redirect("Completed.aspx?RequiredYear=" + ddlSummaryYear.SelectedValue.ToString());
                    break;
                case "lnkDenied":
                    Response.Redirect("Denied.aspx?RequiredYear=" + ddlSummaryYear.SelectedValue.ToString());
                    break;
                default:
                    break;
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
    }
}
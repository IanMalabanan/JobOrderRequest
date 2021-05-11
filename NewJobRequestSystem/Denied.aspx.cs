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
    public partial class Denied : System.Web.UI.Page
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

        protected void Close(object sender, EventArgs e)
        {
            divDetails.Visible = false;

            divRecords.Visible = true;

            jrcode = string.Empty;

            Session["RCode"] = null;
        }

        protected void ClosePage(object sender, EventArgs e)
        {
            Response.Redirect("AdminHome.aspx");
        }

        protected void lnkviewdetails_Click(object sender, EventArgs e)
        {
            LinkButton lnkviewdetails = (LinkButton)sender;

            jrcode = lnkviewdetails.CommandArgument.ToString().Trim();

            Session["RCode"] = jrcode;

            divDetails.Visible = true;

            divRecords.Visible = false;

            GetRequestDetails();
        }

        private void Show(string msg)
        {
            Page page = HttpContext.Current.Handler as Page;
            if (page != null)
            {
                ScriptManager.RegisterStartupScript(page, page.GetType(), "Message", "alert('" + msg + "');", true);
            }
        }

        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));

            DataTable table = new DataTable();

            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            foreach (T item in data)
            {
                DataRow row = table.NewRow();

                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;

                table.Rows.Add(row);
            }

            return table;
        }

        private void GetRequestorDetails(string _empid)
        {
            var getrequestor = dbContext.tblUsers.Where(x => x.EmpID == _empid);

            foreach (var item in getrequestor)
            {
                tblUser tblData = item as tblUser;

                useremail = tblData.EmailAdd;
                
            }

            var dbRequestor = dbHRISContext.SKPI_GetAllEmployeesByEmpID(_empid).ToList();

            foreach (DataRow row in ConvertToDataTable(dbRequestor).Rows)
            {
                tbReqName.Text = row["FullName_FnameFirst"].ToString();

                tbDepartment.Text = row["Dept_Desc"].ToString();

                tbSection.Text = row["Sect_Desc"].ToString();

                Preparer.InnerText = row["FullName_FnameFirst"].ToString().Trim();
            }
        }

        private void GetTypeOfRequest(string jrcode)
        {
            var getRequest = dbContext.tblRequestType_Selected.Where(x => x.JR_Code == jrcode).ToList();

            for (int x = 0; x < ConvertToDataTable(getRequest).Rows.Count; x++)
            {
                var item = ConvertToDataTable(getRequest).Rows[x]["RequestTypeCode"].ToString();
                for (int y = 0; y < rdoReqType.Items.Count; y++)
                {
                    if (rdoReqType.Items[y].Value == item)
                    {
                        rdoReqType.Items[y].Selected = true;
                        break;
                    }
                }
            }
        }

        private void GetRequestDetails()
        {
            string remarksRejected = string.Empty;

            var a = (from data in dbContext.tblRequests
                     join incharge in dbContext.tblIncharges on data.Incharge_ID equals incharge.Incharge_ID                     
                     join assess in dbContext.tblAssessments on data.AssessmentCode equals assess.AssessmentCode
                     into UP
                     from assess in UP.DefaultIfEmpty()
                     where data.JR_Code.Trim() == jrcode.Trim()
                     select new
                     {
                         Request_Date = data.Request_Date,
                         EmpID = data.EmpID,
                         InchargeCode = incharge.Incharge_ID,
                         Incharge = incharge.Incharge,
                         Customer = data.Customer,
                         PartName = data.Partname,
                         PartCode = data.Partcode,
                         JigType = data.JigType,
                         Quantity = data.Quantity,
                         QtyProdPerHr = data.QtyProdPerHr,
                         Monthly_Req = data.Monthly_Req,
                         Machine_Capacity = data.Machine_Capacity,
                         NextForecast = data.NextForeCast,
                         Problem_Desc = data.Problem_Desc,
                         AssessmentCode = data.AssessmentCode,
                         AssessmentDesc = assess.Assessment,
                         AssessmentRemarks = data.AssessmentRemarks,
                         IsRejected = data.IsRejected,
                         IsCancelled = data.IsCancelled,
                         Checker = data.Checker,
                         AssessBy = data.AssessedBy,
                         Noter = data.Noter,
                         Manager = data.Manager,
                         AcceptBy = data.FinalAcceptanceBy,
                         PreparerRemarks = data.Req_Remarks,
                         CheckerRemarks = data.CheckerRemarks,
                         AssesstorRemarks = data.AssessedRemarks,
                         NoterRemarks = data.NoterRemarks,
                         ApproverRemarks = data.ManagerRemarks,
                         ReceiverRemarks = data.AcceptRemarks,
                         RejectRemarks = data.RejectRemarks,
                         DateChecked = data.DateChecked,
                         DateAssessed = data.DateAssessed,
                         DateNoted = data.DateNoted,
                         DateApprove = data.DateApproved,
                         DateReceived = data.DateAccepted,
                         IsExistingJig = data.IsExistingJig
                     }
                     ).ToList();


            foreach (DataRow row in ConvertToDataTable(a).Rows)
            {
                tbdateRequest.Text = Convert.ToDateTime(row[0]).ToString("dd") + "-" + Convert.ToDateTime(row[0]).ToString("MMM") + "-" + Convert.ToDateTime(row[0]).ToString("yyyy");
                userempid = row[1].ToString().Trim();
                incharge = row[2].ToString().Trim();
                tbItemName.Text = row[5].ToString();
                tbItemQuantity.Text = row[8].ToString();
                tbCustomer.Text = row[4].ToString();
                tbPartName.Text = row[5].ToString();
                tbPartCode.Text = row[6].ToString();
                tbTypeOfJig.Text = row[7].ToString();
                tbQuantity_Jigs.Text = row[8].ToString();
                qtyhrtxt.Text = row[9].ToString();
                mreqtxt.Text = row[10].ToString();
                mcaptxt.Text = row[11].ToString();
                tbNextForecast.Text = row[12].ToString();
                tbjustification.Text = row[13].ToString();

                tbAssessment.Text = row[15].ToString().Trim();

                //if (row[14].ToString().Trim() == "CP")
                //{
                //    tbAssessmentRemarks.Text = row[16].ToString().Trim();
                //    colAssessRemarks.Visible = true;
                //}
                //else
                //{
                //    tbAssessmentRemarks.Text = "N/A";
                //    colAssessRemarks.Visible = false;
                //}

                if (string.IsNullOrEmpty(row[16].ToString().Trim()))
                {
                    tbAssessmentRemarks.Text = "N/A";
                }
                else
                {
                    tbAssessmentRemarks.Text = row[16].ToString().Trim();
                }

                if (row.IsNull(35))
                {
                    tbIsExistingJig.Text = "N/A";
                }
                else
                {
                    if (Convert.ToBoolean(row[35]) == true)
                    {
                        tbIsExistingJig.Text = "Yes";
                    }
                    else
                    {
                        tbIsExistingJig.Text = "No";
                    }
                }

                if (Convert.ToBoolean(row[17]) == true && Convert.ToBoolean(row[18]) == false)
                {
                    rowRejected.Visible = true;
                    rowCancelled.Visible = false;

                    remarksRejected = "This request has been rejected. <br /> See below details:";

                    remarksRejected += "<br />" + "<br />" + row[30].ToString().Replace("&nbsp;", " ").Replace("<br />", "\r\n");

                    lblRemarks.Text = remarksRejected;
                }
                else if (Convert.ToBoolean(row[17]) == false && Convert.ToBoolean(row[18]) == true)
                {
                    rowRejected.Visible = false;
                    rowCancelled.Visible = true;
                }
                else
                {
                    rowRejected.Visible = false;
                    rowCancelled.Visible = false;
                }

                Checker.InnerText = row[19].ToString().Trim();

                Assesstor.InnerText = row[20].ToString().Trim();

                Noter.InnerText = row[21].ToString().Trim();

                Approver.InnerText = row[22].ToString().Trim();

                Receiver.InnerText = row[23].ToString().Trim();

                lblPreparerRemarks.InnerText = row[24].ToString().Trim();

                lblCheckerRemarks.InnerText = row[25].ToString().Trim();

                lblAssesstorRemarks.InnerText = row[26].ToString().Trim();

                lblNoterRemarks.InnerText = row[27].ToString().Trim();

                lblApproverRemarks.InnerText = row[28].ToString().Trim();

                lblReceiverRemarks.InnerText = row[29].ToString().Trim();

                lblDateSigned_Preparer.InnerText = string.IsNullOrEmpty(row[0].ToString()) ? string.Empty 
                    : Convert.ToDateTime(row[0]).ToString("yyyy-MMM-dd");

                lblDateSigned_Checker.InnerText = string.IsNullOrEmpty(row[30].ToString()) ? string.Empty 
                    : Convert.ToDateTime(row[30]).ToString("yyyy-MMM-dd");

                lblDateSigned_Assesstor.InnerText = string.IsNullOrEmpty(row[31].ToString()) ? string.Empty 
                    : Convert.ToDateTime(row[31]).ToString("yyyy-MMM-dd");

                lblDateSigned_Noter.InnerText = string.IsNullOrEmpty(row[32].ToString()) ? string.Empty 
                    : Convert.ToDateTime(row[32]).ToString("yyyy-MMM-dd");

                lblDateSigned_Approver.InnerText = string.IsNullOrEmpty(row[33].ToString()) ? string.Empty 
                    : Convert.ToDateTime(row[33]).ToString("yyyy-MMM-dd");

                lblDateSigned_Receiver.InnerText = string.IsNullOrEmpty(row[34].ToString()) ? string.Empty 
                    : Convert.ToDateTime(row[34]).ToString("yyyy-MMM-dd");
            }

            SQLDSGetAttachment.ConnectionString = ClsConfig.JobRequestConnectionString;

            if (incharge == "01" || incharge == "02" || incharge == "03")
            {
                rdoInCharge.SelectedValue = "07";
            }
            else if (incharge == "04" || incharge == "05" || incharge == "06")
            { rdoInCharge.SelectedValue = "08"; }
            else
            {
                rdoInCharge.SelectedValue = incharge;
            }

            GetRequestorDetails(userempid);

            GetTypeOfRequest(Session["RCode"].ToString().Trim());

            if (rdoInCharge.SelectedValue == "04" || rdoInCharge.SelectedValue == "05" || rdoInCharge.SelectedValue == "08")
            {
                rowJigs.Visible = true;
                rowFacilities.Visible = false;
            }
            else
            {
                rowJigs.Visible = false;
                rowFacilities.Visible = true;
            }
        }

        private void DeleteUnusedCodes()
        {
            var lstCodes = dbContext.tblUniqueCodes.Where(p => SqlFunctions.DateDiff("dd", p.creationdatetime, DateTime.Now) >= 2 && p.isused == false).ToList();

            if (lstCodes != null)
            {
                dbContext.tblUniqueCodes.RemoveRange(lstCodes);

                dbContext.SaveChanges();
            }
        }

        protected void DownloadFile(object sender, EventArgs e)
        {
            LinkButton lnkDownloadAttachment = (LinkButton)sender;

            int id = Convert.ToInt32(lnkDownloadAttachment.CommandArgument);

            tblAttachment empdata = new tblAttachment();

            var selectQuery = dbContext.tblAttachments.Where(x => x.id == id);

            foreach (var item in selectQuery)
            {
                tblAttachment myData = item as tblAttachment;

                DownloadFileFromDatabase(myData.attachmentfile, myData.attachmentname, myData.contenttype);
            }
        }

        private void DownloadFileFromDatabase(string databytes, string attachmentname, string contenttype)
        {
            //Download Data From Database

            byte[] bytes = null;

            bytes = Convert.FromBase64String(databytes);

            Response.Clear();

            Response.Buffer = true;

            Response.Charset = "";

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            Response.ContentType = contenttype;//ContentType;

            Response.AppendHeader("Content-Disposition", "attachment; filename=" + attachmentname);

            Response.BinaryWrite(bytes);

            Response.Flush();

            Response.End();
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

            DeleteUnusedCodes();

            SQLDSGetAttachment.ConnectionString = ClsConfig.JobRequestConnectionString;

            getRecords.ConnectionString = ClsConfig.JobRequestConnectionString;

            if (!IsPostBack)
            {
                rdoInCharge.DataSource = dbContext.tblIncharges.Where(x => x.Incharge_ID == "07" || x.Incharge_ID == "08").ToList();
                rdoInCharge.DataBind();

                rdoReqType.DataSource = dbContext.tblRequestTypes.ToList();
                rdoReqType.DataBind();
            }
        }
    }
}
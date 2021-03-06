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
    public partial class UserCompleted : System.Web.UI.Page
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

        protected void btnSendRemarks_Click(object sender, EventArgs e)
        {
            if (tbEmpRemarks.Text.Length == 0)
            {
                Show("Your remarks is a must");
                tbEmpRemarks.Focus();
            }
            else
            {
                SendRemarksToAssesstor();
            }
        }

        protected void Close(object sender, EventArgs e)
        {
            divDetails.Visible = false;

            //divRecords.Visible = true;

            jrcode = string.Empty;

            //Session["RCode"] = null;
        }

        protected void ClosePage(object sender, EventArgs e)
        {
            Response.Redirect("HomePage.aspx");
        }

        private void SendRemarksToAssesstor()
        {
            string body = string.Empty;

            body = "Dear Sir/Ma'am," + "<br /><br />" + "Good Day!";

            body += "<br /><br />" + "Requestor has a concern on the finsihed job order.";

            body += "<br /><br />" + "Job Order No: " + tbJobOrderNo.Text.Trim();

            body += "<br /><br />" + "Concern: " + tbEmpRemarks.Text.Trim();

            body += "<br /><br />" + "Thank You.";

            body += "<br /><br />" + "Note: This is a system generated email. Please do not reply. Thank you";

            var asstr = (from data in dbContext.tblUsers
                         join inchargePerson in dbContext.tblInchargePersons on data.EmpID equals inchargePerson.EmpID
                         into UP
                         from inchargePerson in UP.DefaultIfEmpty()
                         where inchargePerson.Incharge_ID == rdoInCharge.SelectedValue && inchargePerson.IsAssesstor == true
                         select new
                         {
                             EmpID = data.EmpID,
                             EmailAdd = data.EmailAdd,
                             InchargeCode = inchargePerson.Incharge_ID,
                             IsAssesstor = inchargePerson.IsAssesstor,
                             IsNoter = inchargePerson.IsNoter
                         }
                     ).ToList();

            foreach (DataRow dr in ConvertToDataTable(asstr).Rows)
            {
                using (MailMessage mm = new MailMessage())
                {
                    string sub = "Online Job Order Request: Accepted By Requestor";

                    mm.Subject = sub.ToUpper();

                    mm.Body = body;

                    mm.From = new MailAddress(ConfigurationManager.AppSettings["MailSenderEmailAddress"].ToString(),

                        ConfigurationManager.AppSettings["MailSenderName"].ToString());

                    mm.To.Add(dr["EmailAdd"].ToString().Trim());

                    SmtpClient smtp = new SmtpClient();

                    smtp.Host = ConfigurationManager.AppSettings["MailServer"].ToString();

                    smtp.EnableSsl = true;

                    mm.IsBodyHtml = true;

                    NetworkCredential NetworkCred = new NetworkCredential(ConfigurationManager.AppSettings["MailSenderEmailAddress"].ToString(),
                        ConfigurationManager.AppSettings["MailSenderEmailPassword"].ToString());

                    smtp.Credentials = NetworkCred;

                    smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);

                    ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate,
                                                                                X509Chain chain, SslPolicyErrors sslPolicyErrors)
                    { return true; };

                    smtp.Send(mm);

                }
            }
        }

        private void SendToAssesstor()
        {
            string body = string.Empty;

            body = "Dear Sir/Ma'am," + "<br /><br />" + "Good Day!";

            body += "<br /><br />" + "A job request has been accepted by the requestor.";

            body += "<br /><br />" + "Thank You.";

            body += "<br /><br />" + "Note: This is a system generated email. Please do not reply. Thank you";

            var asstr = (from data in dbContext.tblUsers
                         join inchargePerson in dbContext.tblInchargePersons on data.EmpID equals inchargePerson.EmpID
                         into UP
                         from inchargePerson in UP.DefaultIfEmpty()
                         where inchargePerson.Incharge_ID == rdoInCharge.SelectedValue && inchargePerson.IsAssesstor == true
                         select new
                         {
                             EmpID = data.EmpID,
                             EmailAdd = data.EmailAdd,
                             InchargeCode = inchargePerson.Incharge_ID,
                             IsAssesstor = inchargePerson.IsAssesstor,
                             IsNoter = inchargePerson.IsNoter
                         }
                     ).ToList();

            foreach (DataRow dr in ConvertToDataTable(asstr).Rows)
            {
                using (MailMessage mm = new MailMessage())
                {
                    string sub = "Online Job Order Request: Accepted By Requestor";

                    mm.Subject = sub.ToUpper();

                    mm.Body = body;

                    mm.From = new MailAddress(ConfigurationManager.AppSettings["MailSenderEmailAddress"].ToString(),

                        ConfigurationManager.AppSettings["MailSenderName"].ToString());

                    mm.To.Add(dr["EmailAdd"].ToString().Trim());

                    SmtpClient smtp = new SmtpClient();

                    smtp.Host = ConfigurationManager.AppSettings["MailServer"].ToString();

                    smtp.EnableSsl = true;

                    mm.IsBodyHtml = true;

                    NetworkCredential NetworkCred = new NetworkCredential(ConfigurationManager.AppSettings["MailSenderEmailAddress"].ToString(),
                        ConfigurationManager.AppSettings["MailSenderEmailPassword"].ToString());

                    smtp.Credentials = NetworkCred;

                    smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);

                    ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate,
                                                                                X509Chain chain, SslPolicyErrors sslPolicyErrors)
                    { return true; };

                    smtp.Send(mm);

                }
            }
        }

        protected void btnaccept_Click(object sender, ImageClickEventArgs e)
        {
            var accept = dbContext.tblRequests.Where(x => x.JR_Code == jrcode).FirstOrDefault();

            if (accept != null)
            {
                accept.FinalAcceptanceBy = Session["FullName_LnameFirst"].ToString().Trim();

                accept.AcceptRemarks = "SIGNED";

                accept.DateAccepted = DateTime.Now;

                accept.CreationDateTime = DateTime.Now;

                dbContext.Entry(accept).State = EntityState.Modified;

                dbContext.SaveChanges();
            }

            SendToAssesstor();

            divDetails.Visible = true;

            GetRequestDetails();

            Show("Successfully Accepted");
        }

        protected void lnkviewdetails_Click(object sender, EventArgs e)
        {
            LinkButton lnkviewdetails = (LinkButton)sender;

            jrcode = lnkviewdetails.CommandArgument.ToString().Trim();

            //Session["RCode"] = jrcode;

            divDetails.Visible = true;

            //divRecords.Visible = false;

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
                         JRNo = data.JRF_No,
                         Completion_Date = data.Completion_Date,
                         CompletionRemarks = data.CompletionRemarks,
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
                         IsCompleted = data.IsCompleted,
                         DateChecked = data.DateChecked,
                         DateAssessed = data.DateAssessed,
                         DateNoted = data.DateNoted,
                         DateApprove = data.DateApproved,
                         DateReceived = data.DateAccepted,
                         IsExistingJig = data.IsExistingJig,
                         RequestorsRemarks = data.RequestorsRemarks
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

                tbPartCode.Text = row[6].ToString();

                tbPartName.Text = row[5].ToString();

                tbTypeOfJig.Text = row[7].ToString();

                tbQuantity_Jigs.Text = row[8].ToString();

                qtyhrtxt.Text = row[9].ToString();

                mreqtxt.Text = row[10].ToString();

                mcaptxt.Text = row[11].ToString();

                tbNextForecast.Text = row[12].ToString();

                tbjustification.Text = row[13].ToString().Replace("&nbsp;", " ").Replace("<br/>", "\r\n");

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

                if (row.IsNull(37))
                {
                    tbIsExistingJig.Text = "N/A";
                }
                else
                {
                    if (Convert.ToBoolean(row[37]) == true)
                    {
                        tbIsExistingJig.Text = "Yes";
                    }
                    else
                    {
                        tbIsExistingJig.Text = "No";
                    }
                }

                tbJobOrderNo.Text = row[17].ToString();

                tbDateAccomplished.Text = Convert.ToDateTime(row[18]).ToString("dd") + "-" + Convert.ToDateTime(row[18]).ToString("MMM") + "-" + Convert.ToDateTime(row[18]).ToString("yyyy");

                tbRemarks.Text = row[19].ToString();

                //Preparer.InnerText = tbReqName.Text.Trim();

                Checker.InnerText = row[20].ToString().Trim();

                Assesstor.InnerText = row[21].ToString().Trim();

                Noter.InnerText = row[22].ToString().Trim();

                Approver.InnerText = row[23].ToString().Trim();

                Receiver.InnerText = row[24].ToString().Trim();

                lblPreparerRemarks.InnerText = row[25].ToString().Trim();

                lblCheckerRemarks.InnerText = row[26].ToString().Trim();

                lblAssesstorRemarks.InnerText = row[27].ToString().Trim();

                lblNoterRemarks.InnerText = row[28].ToString().Trim();

                lblApproverRemarks.InnerText = row[29].ToString().Trim();

                lblReceiverRemarks.InnerText = row[30].ToString().Trim();

                if (Convert.ToBoolean(row[31]) == true && row[24].ToString().Trim() == "N/A")
                {
                    rowReceiverRemarks.Visible = false;
                    btnaccept.Visible = true;
                    tbEmpRemarks.Text = string.Empty;
                    rowRemarks.Visible = true;
                }
                else
                {
                    rowReceiverRemarks.Visible = true;
                    btnaccept.Visible = false;
                    tbEmpRemarks.Text = string.Empty;
                    rowRemarks.Visible = false;
                }

                lblDateSigned_Preparer.InnerText = string.IsNullOrEmpty(row[0].ToString()) ? string.Empty : Convert.ToDateTime(row[0]).ToString("yyyy-MMM-dd");

                lblDateSigned_Checker.InnerText = string.IsNullOrEmpty(row[32].ToString()) ? string.Empty : Convert.ToDateTime(row[32]).ToString("yyyy-MMM-dd");

                lblDateSigned_Assesstor.InnerText = string.IsNullOrEmpty(row[33].ToString()) ? string.Empty : Convert.ToDateTime(row[33]).ToString("yyyy-MMM-dd");

                lblDateSigned_Noter.InnerText = string.IsNullOrEmpty(row[34].ToString()) ? string.Empty : Convert.ToDateTime(row[34]).ToString("yyyy-MMM-dd");

                lblDateSigned_Approver.InnerText = string.IsNullOrEmpty(row[35].ToString()) ? string.Empty : Convert.ToDateTime(row[35]).ToString("yyyy-MMM-dd");

                lblDateSigned_Receiver.InnerText = string.IsNullOrEmpty(row[36].ToString()) ? string.Empty : Convert.ToDateTime(row[36]).ToString("yyyy-MMM-dd");

                tbEmpRemarks.Text = row[38].ToString().Trim();
            }

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

            GetTypeOfRequest(jrcode);

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

        protected void Page_Load(object sender, EventArgs e)
        {
            DeleteUnusedCodes();

            //getRecords.ConnectionString = ClsConfig.JobRequestConnectionString;

            if (!IsPostBack)
            {
                rdoInCharge.DataSource = dbContext.tblIncharges.Where(x => x.Incharge_ID == "07" || x.Incharge_ID == "08").ToList();
                rdoInCharge.DataBind();

                rdoReqType.DataSource = dbContext.tblRequestTypes.ToList();
                rdoReqType.DataBind();
            }

            jrcode = Request.QueryString["RCode"].ToString().Trim();

            SQLDSGetAttachment.ConnectionString = ClsConfig.JobRequestConnectionString;

            SQLDSGetAttachment2.ConnectionString = ClsConfig.JobRequestConnectionString;

            //Session["RCode"] = jrcode;

            divDetails.Visible = true;

            //divRecords.Visible = false;

            GetRequestDetails();



        }
    }
}
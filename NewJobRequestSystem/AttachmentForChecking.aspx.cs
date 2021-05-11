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
    public partial class AttachmentForChecking : System.Web.UI.Page
    {
        JobRequestDBEntities dbContext = new JobRequestDBEntities();

        HRISEntities dbHRISContext = new HRISEntities();

        ClsGenerateRandomString random = new ClsGenerateRandomString();

        public static string filename = string.Empty, filetype = string.Empty
            , approverempid = string.Empty, approverempid2 = string.Empty, approveremail = string.Empty
            , approverposition = string.Empty, approverdeptcode = string.Empty
            , approverdeptname = string.Empty, approversectcode = string.Empty
            , approversectname = string.Empty, approverfullname_lnamefirst = string.Empty
            , approverfullname_fnamefirst = string.Empty, userempid = string.Empty
            , useremail = string.Empty, incharge = string.Empty, jrcode = string.Empty;

        private void Show(string msg)
        {
            Page page = HttpContext.Current.Handler as Page;
            if (page != null)
            {
                ScriptManager.RegisterStartupScript(page, page.GetType(), "Message", "alert('" + msg + "');", true);
            }
        }

        private Boolean SaveEmailLogs(string url, string role, string emailadd, string emailfrom, string sentto, string code)
        {
            var logs = dbContext.tblEmailLogs.Where(x => x.UserRole == role && x.JR_Code == code).FirstOrDefault();

            if (logs != null)
            {
                logs.EmailUrl = url;
                logs.UserRole = role;
                logs.EmailAdd = emailadd;
                logs.EmailFrom = emailfrom;
                logs.SentTo = sentto;
                logs.JR_Code = code;
                logs.CreationDate = DateTime.Now;

                dbContext.Entry(logs).State = EntityState.Modified;

                dbContext.SaveChanges();
            }
            else
            {
                tblEmailLog tbl = new tblEmailLog()
                {
                    EmailUrl = url,
                    UserRole = role,
                    EmailAdd = emailadd,
                    EmailFrom = emailfrom,
                    SentTo = sentto,
                    JR_Code = code,
                    CreationDate = DateTime.Now
                };

                dbContext.tblEmailLogs.Add(tbl);

                dbContext.SaveChanges();
            }

            return true;
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
            string remarksHold = string.Empty;

            var a = (from data in dbContext.tblRequests
                     join incharge in dbContext.tblIncharges on data.Incharge_ID equals incharge.Incharge_ID
                     into UP
                     join assess in dbContext.tblAssessments on data.AssessmentCode equals assess.AssessmentCode
                     from incharge in UP.DefaultIfEmpty()
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
                         HoldRemarks = data.HoldRemarks,
                         IsHold = data.IsHold,
                         HoldBy = data.HoldBy
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

                tbjustification.Text = row[13].ToString().Replace("&nbsp;", " ").Replace("<br/>", "\r\n");

                tbAssessment.Text = row[15].ToString().Trim();

                if (row[14].ToString().Trim() == "CP")
                {
                    tbAssessmentRemarks.Text = row[16].ToString().Trim();
                    colAssessRemarks.Visible = true;
                }
                else
                {
                    tbAssessmentRemarks.Text = "N/A";
                    colAssessRemarks.Visible = false;
                }

                if (Convert.ToBoolean(row[18]) == true)
                {
                    rowHold.Visible = true;

                    remarksHold = "This request is set as hold. See below details:";

                    remarksHold += "<br />" + "<br />" + row[17].ToString().Replace("&nbsp;", " ").Replace("<br />", "\r\n");

                    lblHoldRemarks.Text = remarksHold;
                }
                else
                {
                    rowHold.Visible = false;
                }

            }



            rdoInCharge.SelectedValue = incharge;

            GetRequestorDetails(userempid);

            GetTypeOfRequest(Session["RCode"].ToString().Trim());

            if (rdoInCharge.SelectedValue == "04" || rdoInCharge.SelectedValue == "05")
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

        private void SendRejectedEmail()
        {
            string link = string.Empty, body = string.Empty;

            body = "Dear Sir/Ma'am," + "<br /><br />" + "Good Day!";

            body += "<br /><br />" + "Please be informed that your request has been rejected by " + approverfullname_lnamefirst + ".";

            body += "<br />" + "<br />" + "See Details Below For The Reason.";

            body += "<br />" + "<br />" + "\"" + tbRejectedReason.Text + "\"";

            body += "<br /><br />" + "Thank You.";

            body += "<br /><br />" + "Note: This is a system generated email. Please do not reply. Thank you";

            var rx = new Regex(@"(?<=\w)\w");

            var newString2 = rx.Replace(Session["FullName_FnameFirst"].ToString(), new MatchEvaluator(m => m.Value.ToLowerInvariant()));

            using (MailMessage mm = new MailMessage())
            {
                string sub = "Online Job Order Request: Rejected";

                mm.Subject = sub.ToUpper();

                mm.Body = body;

                var newString = rx.Replace(Session["FullName_FnameFirst"].ToString(), new MatchEvaluator(m => m.Value.ToLowerInvariant()));

                mm.From = new MailAddress(ConfigurationManager.AppSettings["MailSenderEmailAddress"].ToString(),

                    ConfigurationManager.AppSettings["MailSenderName"].ToString());

                mm.To.Add(useremail);

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

        private void SendHoldEmail()
        {
            string link = string.Empty, body = string.Empty;

            body = "Dear Sir/Ma'am," + "<br /><br />" + "Good Day!";

            body += "<br /><br />" + "Please be informed that your request is set as hold by " + approverfullname_lnamefirst + ".";

            body += "<br />" + "<br />" + "See Details Below For The Reason.";

            body += "<br />" + "<br />" + "\"" + tbHoldReason.Text + "\"";

            body += "<br /><br />" + "Thank You.";

            body += "<br /><br />" + "Note: This is a system generated email. Please do not reply. Thank you";

            var rx = new Regex(@"(?<=\w)\w");

            var newString2 = rx.Replace(Session["FullName_FnameFirst"].ToString(), new MatchEvaluator(m => m.Value.ToLowerInvariant()));

            using (MailMessage mm = new MailMessage())
            {
                string sub = "Online Job Order Request: Rejected";

                mm.Subject = sub.ToUpper();

                mm.Body = body;

                var newString = rx.Replace(Session["FullName_FnameFirst"].ToString(), new MatchEvaluator(m => m.Value.ToLowerInvariant()));

                mm.From = new MailAddress(ConfigurationManager.AppSettings["MailSenderEmailAddress"].ToString(),

                    ConfigurationManager.AppSettings["MailSenderName"].ToString());

                mm.To.Add(useremail);

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

        private void SendToRequestor()
        {
            string body = string.Empty;

            body = "Dear Sir/Ma'am," + "<br /><br />" + "Good Day!";

            body += "<br /><br />" + "Please be informed that your job order request has been noted.";

            body += "<br /><br />" + "Thank You.";

            body += "<br /><br />" + "Note: This is a system generated email. Please do not reply. Thank you";


            using (MailMessage mm = new MailMessage())
            {
                string sub = "Online Job Order Request: Approval Notification";

                mm.Subject = sub.ToUpper();

                mm.Body = body;

                mm.From = new MailAddress(ConfigurationManager.AppSettings["MailSenderEmailAddress"].ToString(),

                    ConfigurationManager.AppSettings["MailSenderName"].ToString());

                mm.To.Add(useremail);

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

        protected void Page_Load(object sender, EventArgs e)
        {
            btnShowReject.Attributes["href"] = "javascript:void(0);";
            btnShowReject.Attributes["onclick"] = String.Format("return ShowRejectForm('{0}','{1}','{2}');"
                , Request.QueryString["EmpID"].ToString().Trim(), Request.QueryString["RCode"].ToString().Trim(), "Checker_Attachment");

            btnShowHold.Attributes["href"] = "javascript:void(0);";
            btnShowHold.Attributes["onclick"] = String.Format("return ShowHoldForm('{0}','{1}','{2}');"
                , Request.QueryString["EmpID"].ToString().Trim(), Request.QueryString["RCode"].ToString().Trim(), "Checker_Attachment");

            DeleteUnusedCodes();

            if (!IsPostBack)
            {
                rdoInCharge.DataSource = dbContext.tblIncharges.ToList();
                rdoInCharge.DataBind();

                rdoReqType.DataSource = dbContext.tblRequestTypes.ToList();
                rdoReqType.DataBind();
            }

            approverempid = Request.QueryString["EmpID"].ToString().Trim();

            var getchecker = dbContext.tblUsers.Where(x => x.EmpID == approverempid2);

            foreach (var item in getchecker)
            {
                tblUser tblData = item as tblUser;

                approverempid = tblData.EmpID;

                approveremail = tblData.EmailAdd;
            }

            var dbuser = dbHRISContext.SKPI_GetAllEmployeesByEmpID(approverempid).ToList();

            foreach (DataRow row in ConvertToDataTable(dbuser).Rows)
            {
                approverposition = row["Pos_Desc"].ToString();

                Session["Position"] = row["Pos_Desc"].ToString();

                approverdeptcode = row["Dept_Code"].ToString();

                approverdeptname = row["Dept_Desc"].ToString();

                approversectcode = row["Sect_Code"].ToString();

                approversectname = row["Sect_Desc"].ToString();

                Session["FName"] = row["FirstName"].ToString();

                approverfullname_lnamefirst = row["FullName_LnameFirst"].ToString();

                approverfullname_fnamefirst = row["FullName_FnameFirst"].ToString();

                Session["FullName_LnameFirst"] = row["FullName_LnameFirst"].ToString();

                Session["FullName_FnameFirst"] = row["FullName_FnameFirst"].ToString();
            }

            Session["RCode"] = Request.QueryString["RCode"].ToString();

            jrcode = Session["RCode"].ToString();

            SQLDSGetAttachment.ConnectionString = ClsConfig.JobRequestConnectionString;

            SQLDSGetAttachment2.ConnectionString = ClsConfig.JobRequestConnectionString;


            //var verifyRequest = dbContext.tblAttachments.Where(x => x.jr_code == jrcode && x.typeofattachment == "Admin" && x.IsPrepared == true && x.IsChecked == true && x.IsApproved == false).Count();

            //var verifyRequest2 = dbContext.tblAttachments.Where(x => x.jr_code == jrcode && x.typeofattachment == "Admin" && x.IsPrepared == false && x.IsChecked == false && x.IsApproved == false).Count();

            //var verifyRequest3 = dbContext.tblAttachments.Where(x => x.jr_code == jrcode && x.typeofattachment == "Admin" && x.IsPrepared == true && x.IsChecked == false && x.IsApproved == false).Count();

            //if (verifyRequest > 0)
            //{
            //    divSuccess.Visible = true;
            //    divApplication.Visible = false;
            //    divRejected.Visible = false;
            //    divCancelled.Visible = false;
            //}
            //else if (verifyRequest2 > 0)
            //{
            //    divSuccess.Visible = false;
            //    divApplication.Visible = false;
            //    divRejected.Visible = false;
            //    divCancelled.Visible = false;
            //}
            ////else if (Convert.ToBoolean(tblData.IsRejected) == true)
            ////{
            ////    divSuccess.Visible = false;
            ////    divApplication.Visible = false;
            ////    divRejected.Visible = true;
            ////    divCancelled.Visible = false;
            ////}
            ////else if (Convert.ToBoolean(tblData.IsCancelled) == true)
            ////{
            ////    divSuccess.Visible = false;
            ////    divApplication.Visible = false;
            ////    divRejected.Visible = false;
            ////    divCancelled.Visible = true;
            ////}
            //else if (verifyRequest3 > 0)
            //{
            //    divSuccess.Visible = false;
            //    divApplication.Visible = true;
            //    divRejected.Visible = false;
            //    divCancelled.Visible = false;

            //    GetRequestDetails();
            //}

            var app = dbContext.tblRequests.Where(x => x.JR_Code == jrcode).FirstOrDefault();
            
            if (app != null)
            {
                if (app.AttachementStatus == "PREPARED")
                {
                    divSuccess.Visible = false;
                    divApplication.Visible = true;
                    divRejected.Visible = false;
                    divCancelled.Visible = false;

                    GetRequestDetails();
                }
                else
                {
                    divSuccess.Visible = true;
                    divApplication.Visible = false;
                    divRejected.Visible = false;
                    divCancelled.Visible = false;
                }
            }
        }

        protected void DownloadFile(object sender, EventArgs e)
        {
            LinkButton lnkDownloadAttachment = (LinkButton)sender;

            int id = Convert.ToInt32(lnkDownloadAttachment.CommandArgument);

            var accept = dbContext.tblAttachments.Where(x => x.id == id).FirstOrDefault();

            if (accept != null)
            {
                accept.IsCheckerReviewed = true;

                accept.datecreated = DateTime.Now;

                dbContext.Entry(accept).State = EntityState.Modified;

                dbContext.SaveChanges();
            }

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

        protected void Approve(object sender, EventArgs e)
        {
            string link = string.Empty, body = string.Empty;

            var count = dbContext.tblAttachments.Where(x => x.jr_code == jrcode
                        && x.typeofattachment.Trim() == "Admin" && x.IsCheckerReviewed == false && x.IsApproverReviewed == false).Count();

            if (count > 0)
            {
                Show("Some attachments are not yet reviewed. Must download and review the attachments first.");
            }
            else
            {
                var app = dbContext.tblRequests.Where(x => x.JR_Code == jrcode).FirstOrDefault();

                if (app != null)
                {
                    app.IsHold = false;

                    app.AttachementStatus = "NOTED";

                    app.CreationDateTime = DateTime.Now;

                    dbContext.Entry(app).State = EntityState.Modified;

                    dbContext.SaveChanges();
                }

                var accept = dbContext.tblAttachments.Where(x => x.jr_code == jrcode
                            && x.typeofattachment.Trim() == "Admin").FirstOrDefault();

                if (accept != null)
                {
                    accept.CheckedBy = approverfullname_fnamefirst;

                    accept.IsChecked = true;

                    accept.datecreated = DateTime.Now;

                    dbContext.Entry(accept).State = EntityState.Modified;

                    dbContext.SaveChanges();
                }

                link = Environment.NewLine + "http://" + HttpContext.Current.Request.Url.Authority + "/"
                         + ConfigurationManager.AppSettings["AttachmentApproverPage"];

                var asstr = (from data in dbContext.tblUsers
                             join approvers in dbContext.tblApprovers on data.EmpID equals approvers.EmpID
                             into UP
                             from approvers in UP.DefaultIfEmpty()
                             where approvers.OrderValue == 1
                             select new
                             {
                                 EmpID = data.EmpID,
                                 EmailAdd = data.EmailAdd
                             }
                         ).ToList();

                foreach (DataRow dr in ConvertToDataTable(asstr).Rows)
                {
                    body = "Dear Sir/Ma'am," + "<br /><br />" + "Good Day!";

                    body += "<br /><br />" + "I Have Prepared A Job Order Request Attachment/s For Your Checking And Approval.";

                    body += "<br /><br />" + "You may click the link below to redirect to main page. ";

                    body += "<br /><br />" + link + "?EmpID=" + dr["EmpID"].ToString().Trim() + "&RCode=" + Session["RCode"].ToString().Trim();

                    body += "<br /><br />" + "Thank You.";

                    body += "<br /><br />" + "Note: This is a system generated email. Please do not reply. Thank you";

                    var rx = new Regex(@"(?<=\w)\w");

                    var newString2 = rx.Replace(Session["FullName_FnameFirst"].ToString(), new MatchEvaluator(m => m.Value.ToLowerInvariant()));

                    using (MailMessage mm = new MailMessage())
                    {
                        string sub = "Online Job Order Request: Requesting For Approval";

                        mm.Subject = sub.ToUpper();

                        mm.Body = body;

                        var newString = rx.Replace(Session["FullName_FnameFirst"].ToString(), new MatchEvaluator(m => m.Value.ToLowerInvariant()));

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

                //SendToRequestor();

                divSuccess.Visible = true;

                divApplication.Visible = false;
            }
        }

        protected void Reject(object sender, EventArgs e)
        {
            var app = dbContext.tblRequests.Where(x => x.JR_Code == jrcode).FirstOrDefault();

            if (app != null)
            {
                app.Is_Noted = true;

                app.Noter = approverfullname_fnamefirst.ToString();

                app.NoterRemarks = "REJECTED";

                app.DateNoted = DateTime.Now.Date;

                app.IsHold = false;

                app.RejectRemarks = tbRejectedReason.Text.Trim().Replace("\r\n", "<br />").Replace(" ", "&nbsp;").Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;").Replace("'", "^");

                app.HoldBy = DBNull.Value.ToString();

                app.HoldRemarks = DBNull.Value.ToString();

                app.CreationDateTime = DateTime.Now;

                dbContext.Entry(app).State = EntityState.Modified;

                dbContext.SaveChanges();
            }

            SendRejectedEmail();

            divSuccess.Visible = false;
            divApplication.Visible = false;
            divRejected.Visible = true;
            divCancelled.Visible = false;
        }

        protected void Hold(object sender, EventArgs e)
        {
            var app = dbContext.tblRequests.Where(x => x.JR_Code == jrcode).FirstOrDefault();

            if (app != null)
            {
                app.IsHold = true;

                app.HoldBy = DBNull.Value.ToString();

                app.HoldRemarks = DBNull.Value.ToString();

                app.CreationDateTime = DateTime.Now;

                app.CreationDateTime = DateTime.Now;

                dbContext.Entry(app).State = EntityState.Modified;

                dbContext.SaveChanges();
            }

            var accept = dbContext.tblAttachments.Where(x => x.jr_code == jrcode
                        && x.typeofattachment.Trim() == "Admin").FirstOrDefault();

            if (accept != null)
            {
                accept.IsChecked = false;

                accept.IsApproved = false;

                accept.datecreated = DateTime.Now;

                dbContext.Entry(accept).State = EntityState.Modified;

                dbContext.SaveChanges();
            }

            SendHoldEmail();

            divSuccess.Visible = false;
            divApplication.Visible = true;
            divRejected.Visible = false;
            divCancelled.Visible = false;

            GetRequestDetails();
        }
    }
}
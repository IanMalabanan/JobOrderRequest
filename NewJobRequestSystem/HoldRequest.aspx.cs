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
    public partial class HoldRequest : System.Web.UI.Page
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

        private void GetRequestDetails()
        {
            var a = (from data in dbContext.tblRequests
                     join incharge in dbContext.tblIncharges on data.Incharge_ID equals incharge.Incharge_ID
                     into UP
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
                         Problem_Desc = data.Problem_Desc
                     }
                     ).ToList();


            foreach (DataRow row in ConvertToDataTable(a).Rows)
            {
                userempid = row[1].ToString().Trim();
                incharge = row[2].ToString().Trim();
            }

            GetRequestorDetails(userempid);
        }

        private void GetRequestorDetails(string _empid)
        {
            var getrequestor = dbContext.tblUsers.Where(x => x.EmpID == _empid);

            foreach (var item in getrequestor)
            {
                tblUser tblData = item as tblUser;

                useremail = tblData.EmailAdd;
            }
        }

        private void SendRejectedEmail()
        {
            string link = string.Empty, body = string.Empty;

            body = "Dear Sir/Ma'am," + "<br /><br />" + "Good Day!";

            body += "<br /><br />" + "Please be informed that your request has been rejected by " + Session["FullName_FnameFirst"].ToString().Trim() + ".";

            body += "<br />" + "<br />" + "See Details Below For The Reason.";

            body += "<br />" + "<br />" + "\"" + tbHold.Text + "\"";

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

        public void RejectByChecker()
        {
            var app = dbContext.tblRequests.Where(x => x.JR_Code == jrcode).FirstOrDefault();

            if (app != null)
            {
                app.IsRejected = true;

                app.Checker = Session["FullName_FnameFirst"].ToString().Trim();

                app.CheckerRemarks = "REJECTED";

                app.DateChecked = DateTime.Now.Date;

                app.IsHold = false;

                app.RejectRemarks = tbHold.Text.Trim().Replace("\r\n", "<br />").Replace(" ", "&nbsp;").Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;").Replace("'", "^");

                app.HoldBy = DBNull.Value.ToString();

                app.HoldRemarks = DBNull.Value.ToString();

                app.CreationDateTime = DateTime.Now;

                dbContext.Entry(app).State = EntityState.Modified;

                dbContext.SaveChanges();
            }

            SendRejectedEmail();

            string script = "window.onload = function() { CloseWindow(); };";

            ClientScript.RegisterStartupScript(this.GetType(), "CloseWindow", script, true);
        }

        public void RejectByApprover()
        {
            var app = dbContext.tblRequests.Where(x => x.JR_Code == jrcode).FirstOrDefault();

            if (app != null)
            {
                app.IsRejected = true;

                app.Manager = approverfullname_fnamefirst.ToString();

                app.ManagerRemarks = "REJECTED";

                app.DateApproved = DateTime.Now.Date;

                app.IsHold = false;

                app.RejectRemarks = tbHold.Text.Trim().Replace("\r\n", "<br />").Replace(" ", "&nbsp;").Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;").Replace("'", "^");

                app.HoldBy = DBNull.Value.ToString();

                app.HoldRemarks = DBNull.Value.ToString();

                app.CreationDateTime = DateTime.Now;

                dbContext.Entry(app).State = EntityState.Modified;

                dbContext.SaveChanges();
            }

            SendRejectedEmail();

            string script = "window.onload = function() { CloseWindow(); };";

            ClientScript.RegisterStartupScript(this.GetType(), "CloseWindow", script, true);
        }

        public void RejectByNoter()
        {
            var app = dbContext.tblRequests.Where(x => x.JR_Code == jrcode).FirstOrDefault();

            if (app != null)
            {
                app.IsRejected = true;

                app.Noter = approverfullname_fnamefirst.ToString();

                app.NoterRemarks = "REJECTED";

                app.DateNoted = DateTime.Now.Date;

                app.IsHold = false;

                app.RejectRemarks = tbHold.Text.Trim().Replace("\r\n", "<br />").Replace(" ", "&nbsp;").Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;").Replace("'", "^");

                app.HoldBy = DBNull.Value.ToString();

                app.HoldRemarks = DBNull.Value.ToString();

                app.CreationDateTime = DateTime.Now;

                dbContext.Entry(app).State = EntityState.Modified;

                dbContext.SaveChanges();
            }

            SendRejectedEmail();
        }

        private void SendHoldEmail()
        {
            string link = string.Empty, body = string.Empty;

            body = "Dear Sir/Ma'am," + "<br /><br />" + "Good Day!";

            body += "<br /><br />" + "Please be informed that your request is set as hold by " + Session["FullName_FnameFirst"].ToString().Trim() + ".";

            body += "<br />" + "<br />" + "See Details Below For The Reason.";

            body += "<br />" + "<br />" + "\"" + tbHold.Text + "\"";

            body += "<br /><br />" + "Thank You.";

            body += "<br /><br />" + "Note: This is a system generated email. Please do not reply. Thank you";

            var rx = new Regex(@"(?<=\w)\w");

            var newString2 = rx.Replace(Session["FullName_FnameFirst"].ToString(), new MatchEvaluator(m => m.Value.ToLowerInvariant()));

            using (MailMessage mm = new MailMessage())
            {
                string sub = "Online Job Order Request: On-Hold";

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

        private void HoldByChecker()
        {
            var app = dbContext.tblRequests.Where(x => x.JR_Code == jrcode).FirstOrDefault();

            if (app != null)
            {
                app.IsHold = true;

                app.AttachementStatus = "ON-HOLD";

                app.HoldBy = Session["FullName_FnameFirst"].ToString().Trim();

                app.HoldRemarks = tbHold.Text.Trim().Replace("\r\n", "<br />").Replace(" ", "&nbsp;").Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;").Replace("'", "^");

                app.CreationDateTime = DateTime.Now;

                app.CreationDateTime = DateTime.Now;

                dbContext.Entry(app).State = EntityState.Modified;

                dbContext.SaveChanges();
            }

            var accept = dbContext.tblAttachments.Where(x => x.jr_code == jrcode
                        && x.typeofattachment.Trim() == "Admin").FirstOrDefault();

            if (accept != null)
            {
                accept.PreparedBy = null;

                accept.IsPrepared = false;

                accept.CheckedBy = null;

                accept.IsChecked = false;

                accept.ApprovedBy = null;

                accept.IsApproved = false;

                accept.datecreated = DateTime.Now;

                dbContext.Entry(accept).State = EntityState.Modified;

                dbContext.SaveChanges();
            }
        }

        private void HoldByApprover()
        {
            var app = dbContext.tblRequests.Where(x => x.JR_Code == jrcode).FirstOrDefault();

            if (app != null)
            {
                app.IsHold = true;

                app.AttachementStatus = "ON-HOLD";

                app.HoldBy = Session["FullName_FnameFirst"].ToString().Trim();

                app.HoldRemarks = tbHold.Text.Trim().Replace("\r\n", "<br />").Replace(" ", "&nbsp;").Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;").Replace("'", "^");

                app.CreationDateTime = DateTime.Now;
                
                dbContext.Entry(app).State = EntityState.Modified;

                dbContext.SaveChanges();
            }

            var accept = dbContext.tblAttachments.Where(x => x.jr_code == jrcode
                        && x.typeofattachment.Trim() == "Admin").FirstOrDefault();

            if (accept != null)
            {
                accept.PreparedBy = null;

                accept.IsPrepared = false;

                accept.CheckedBy = null;

                accept.IsChecked = false;

                accept.ApprovedBy = null;

                accept.IsApproved = false;

                accept.datecreated = DateTime.Now;

                dbContext.Entry(accept).State = EntityState.Modified;

                dbContext.SaveChanges();
            }
        }

        protected void btnReject_OnClick(object sender, EventArgs e)
        {
            if (Request.QueryString["Action"].ToString().Trim() == "Checker_Attachment")
            {
                HoldByChecker();
            }
            else if (Request.QueryString["Action"].ToString().Trim() == "Approver_Attachment")
            {
                HoldByApprover();
            }
            else
            {
                var app = dbContext.tblRequests.Where(x => x.JR_Code == jrcode).FirstOrDefault();

                if (app != null)
                {
                    app.IsHold = true;
                    
                    app.HoldBy = Session["FullName_FnameFirst"].ToString().Trim();

                    app.HoldRemarks = tbHold.Text.Trim().Replace("\r\n", "<br />").Replace(" ", "&nbsp;").Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;").Replace("'", "^");

                    app.CreationDateTime = DateTime.Now;

                    dbContext.Entry(app).State = EntityState.Modified;

                    dbContext.SaveChanges();
                }
            }

            SendHoldEmail();

            string script = "window.onload = function() { CloseWindow(); };";

            ClientScript.RegisterStartupScript(this.GetType(), "CloseWindow", script, true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            jrcode = Request.QueryString["RCode"].ToString().Trim();

            GetRequestDetails();
        }
    }
}
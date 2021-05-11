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
    public partial class ChangeApprover : System.Web.UI.Page
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

        public DataTable VerifyIfEmployeeIsAvailable(string empid)
        {
            var query = dbContext.VerifyIfEmployeeIsAvailable(empid).ToList();

            return ConvertToDataTable(query);
        }

        private Boolean UpdateApprover(string url, string role, string emailadd, string emailfrom, string sentto, string uc)
        {
            string query = string.Empty;

            query += "declare @role varchar(15) = '" + role +"'; " +
                "if exists(select * from tblEmailLogs where JR_Code = '" + uc + "' and UserRole = '" + role + "') ";
            query += "begin ";
            query += "  UPDATE dbo.tblEmailLogs set EmailAdd='" + emailadd + "', EmailUrl = REPLACE(EmailUrl, SentTo, '" + sentto + "'), SentTo='" + sentto + "' ";
            query += "      where JR_Code = '" + uc + "' and UserRole = '" + role + "' ";
            query += "      if ltrim(rtrim(@role))='Checker'  ";
            query += "      begin ";
            query += "          update dbo.tblRequest set ";
            query += "          Checker = ";
            query += "          (SELECT ";
            query += "          (RTRIM(e.FirstName) + ' ' + RTRIM(e.LastName)) ";
            query += "          FROM [HRIS].[dbo].t_EmpMaster e ";
            query += "          where e.EmpID = '" + sentto + "' ) ";
            query += "          where JR_Code = '" + uc + "' ";
            query += "      end ";
            query += "      else if ltrim(rtrim(@role))='Noter'  ";
            query += "      begin ";
            query += "          update dbo.tblRequest set ";
            query += "          Noter = ";
            query += "          (SELECT ";
            query += "          (RTRIM(e.FirstName) + ' ' + RTRIM(e.LastName)) ";
            query += "          FROM [HRIS].[dbo].t_EmpMaster e ";
            query += "          where e.EmpID = '" + sentto + "' ) ";
            query += "          where JR_Code = '" + uc + "' ";
            query += "      end ";
            query += "end ";
            query += "else ";
            query += "begin ";
            query += "  INSERT INTO tblEmailLogs(EmailUrl, UserRole, EmailAdd, EmailFrom, SentTo, JR_Code) ";
            query += "      VALUES('" + url + "', '" + role + "','" + emailadd + "','" + emailfrom + "','" + sentto + "','" + uc + "') ";
            query += "  if ltrim(rtrim(@role))='Checker'  ";
            query += "  begin ";
            query += "      update dbo.tblRequest set ";
            query += "      Checker = ";
            query += "      (SELECT ";
            query += "      (RTRIM(e.FirstName) + ' ' + RTRIM(e.LastName)) ";
            query += "      FROM [HRIS].[dbo].t_EmpMaster e ";
            query += "      where e.EmpID = '" + sentto + "' ) ";
            query += "      where JR_Code = '" + uc + "' ";
            query += "  end ";
            query += "  else if ltrim(rtrim(@role))='Noter'  ";
            query += "  begin ";
            query += "      update dbo.tblRequest set ";
            query += "      Noter = ";
            query += "      (SELECT ";
            query += "      (RTRIM(e.FirstName) + ' ' + RTRIM(e.LastName)) ";
            query += "      FROM [HRIS].[dbo].t_EmpMaster e ";
            query += "      where e.EmpID = '" + sentto + "' ) ";
            query += "      where JR_Code = '" + uc + "' ";
            query += "  end ";
            query += "end ";

            SqlCommand cmd = new SqlCommand(query);

            cmd.CommandType = CommandType.Text;

            SqlHelper.ExecuteNonQuery(ClsConfig.JobRequestConnectionString, cmd);
                        
            return true;
        }

        private void SubmitRequest(string code, string sentto, string email)
        {
            var signerQuery = dbContext.tblRequests.Where(x => x.JR_Code == jrcode).ToList();

            foreach (var item in signerQuery)
            {
                tblRequest tblData = item as tblRequest;

                if (tblData.Is_Submitted == true &&
                    tblData.Is_Checked == false &&
                    tblData.Is_Assessed == false &&
                    tblData.Is_Approved == false &&
                    tblData.Is_Noted == false)
                {
                    SendEmailToChecker(sentto.Trim(), code.Trim(), email.Trim());
                }
                else if (tblData.Is_Submitted == true &&
                         tblData.Is_Checked == true &&
                         tblData.Is_Assessed == true &&
                         tblData.Is_Approved == false &&
                         tblData.Is_Noted == false)
                {
                    SendEmailToNoter(sentto.Trim(), code.Trim(), email.Trim());
                }
            }
        }

        private void SendEmailToChecker(string EmpID, string UniqueCode, string EmailAdd)
        {
            string body;

            body = "Dear Sir/Ma'am," + "<br /><br />" + "Good Day!";

            body += "<br /><br />" + "I Have Prepared A Job Order Request For Your Checking And Approval.";

            body += "<br /><br />" + "You may click the link below to redirect to main page. ";

            body += "<br /><br />" + "http://" + HttpContext.Current.Request.Url.Authority + "/"
                     + ConfigurationManager.AppSettings["CheckerPage"] + "?EmpID=" + EmpID.Trim() + "&RCode=" + UniqueCode.Trim();

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

                mm.To.Add(EmailAdd);

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

        private void SendEmailToNoter(string EmpID, string UniqueCode, string EmailAdd)
        {
            string body;

            body = "Dear Sir/Ma'am," + "<br /><br />" + "Good Day!";

            body += "<br /><br />" + "I Have Prepared A Job Order Request For Your Checking And Approval.";

            body += "<br /><br />" + "You may click the link below to redirect to main page. ";

            body += "<br /><br />" + "http://" + HttpContext.Current.Request.Url.Authority + "/"
                     + ConfigurationManager.AppSettings["NoterPage"] + "?EmpID=" + EmpID.Trim() + "&RCode=" + UniqueCode.Trim();

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

                mm.To.Add(EmailAdd);

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

        protected void cboApprover_DataBound(object sender, EventArgs e)
        {
            ((Literal)cboApprover.Footer.FindControl("RadComboItemsCount")).Text = Convert.ToString(cboApprover.Items.Count);
        }

        protected void cboApprover_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            e.Item.Text = ((DataRowView)e.Item.DataItem)["FullName_LnameFirst"].ToString();

            e.Item.Value = ((DataRowView)e.Item.DataItem)["EmpID"].ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SQLDSGetAllApprover.ConnectionString = ClsConfig.JobRequestConnectionString;

            if (Request.QueryString["Role"].ToString().Trim() == "Checker")
            {
                cboApprover.DataSourceID = "SQLDSGetAllApprover";
            }
            else if (Request.QueryString["Role"].ToString().Trim() == "Noter")
            {
                cboApprover.DataSourceID = "SQLDSGetAllApprover";
            }
        }

        protected void btnChange_Click(object sender, EventArgs e)
        {
            string url = string.Empty;

            if (string.IsNullOrEmpty(lblID.Text))
            {

            }
            else
            {
                SqlCommand cmd = new SqlCommand("Select * from tblUsers where EmpID='" + lblID.Text + "'");

                cmd.CommandType = CommandType.Text;

                foreach (DataRow dr in SqlHelper.ExecuteDataReader(ClsConfig.JobRequestConnectionString, cmd).Rows)
                {
                    if (Request.QueryString["Role"].ToString().Trim() == "Checker")
                    {
                        url = "http://" + HttpContext.Current.Request.Url.Authority + "/" + ConfigurationManager.AppSettings["CheckerPage"]
                            + "?EmpID=" + dr["EmpID"].ToString().Trim() + "&RCode=" + Request.QueryString["RCode"].ToString().Trim();
                    }                    
                    else if (Request.QueryString["Role"].ToString().Trim() == "Noter")
                    {
                        url = "http://" + HttpContext.Current.Request.Url.Authority + "/" + ConfigurationManager.AppSettings["NoterPage"]
                            + "?EmpID=" + dr["EmpID"].ToString().Trim() + "&RCode=" + Request.QueryString["RCode"].ToString().Trim();
                    }

                    Boolean res;

                    res = UpdateApprover(url, Request.QueryString["Role"].ToString().Trim()
                        , dr["EmailAdd"].ToString().Trim(), Session["FullName_FnameFirst"].ToString().Trim()
                        , dr["EmpID"].ToString().Trim(), Request.QueryString["RCode"].ToString().Trim());

                    //SubmitRequest(Request.QueryString["RCode"].ToString().Trim(), dr["EmpID"].ToString().Trim(), dr["EmailAdd"].ToString().Trim());
                }

                ScriptManager.RegisterStartupScript(this, GetType(), "close", "CloseWindow();", true);
            }
        }

        protected void cboApprover_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            lblID.Text = cboApprover.SelectedValue.ToString().Trim();

            if (!string.IsNullOrEmpty(lblID.Text))
            {
                int count = dbContext.VerifyIfEmployeeIsAvailable(lblID.Text.Trim()).ToList().Count();//Convert.ToInt32(VerifyIfEmployeeIsAvailable(lblID.Text).Rows[0][0].ToString());

                if (count == 0)
                {
                    lblIsAvailable.Text = "Employee Is Not In The Company";
                }
                else
                {
                    lblIsAvailable.Text = "Employee Is In The Company";
                }
            }

            btnChange.Enabled = true;
        }
    }
}
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
    public partial class AttachmentApproval : System.Web.UI.Page
    {
        JobRequestDBEntities dbContext = new JobRequestDBEntities();

        HRISEntities dbHRISContext = new HRISEntities();

        ClsGenerateRandomString random = new ClsGenerateRandomString();

        public static string filename = string.Empty, filetype = string.Empty
                ,approverempid = string.Empty, approveremail = string.Empty
                ,approverposition = string.Empty, approverdeptcode = string.Empty
                ,approverdeptname = string.Empty, approversectcode = string.Empty
                ,approversectname = string.Empty, approverfullname_lnamefirst = string.Empty
                ,approverfullname_fnamefirst = string.Empty, userempid = string.Empty
                ,useremail = string.Empty, incharge = string.Empty, jrcode = string.Empty;

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

        protected void gridRecords_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if(e.Item is GridDataItem)
            {
                Label rrNos = (e.Item.FindControl("txtRR") as Label);

                LinkButton lnkResendRequest = (e.Item.FindControl("lnkResendRequest") as LinkButton); 

                if (!string.IsNullOrEmpty(rrNos.Text.Trim()) && 
                    rrNos.Text.Trim() == "FOR UPLOADING OF ATTACHMENT")
                {
                    lnkResendRequest.Visible = false;
                }
                else
                {
                    lnkResendRequest.Visible = true;
                }
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
                         ReceiverRemarks = data.AcceptRemarks
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

                tbJobOrderNo.Text = row[17].ToString();

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
            }

            SQLDSGetAttachment.ConnectionString = ClsConfig.JobRequestConnectionString;

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

                //dbContext.SaveChanges();
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

                //dbContext.SaveChanges();
            }

            return true;
        }

        protected void Close(object sender, EventArgs e)
        {
            divDetails.Visible = false;

            divRecords.Visible = true;

            jrcode = string.Empty;

            Session["RCode"] = null;

            gridRecords.Rebind();
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

            GetRequestDetails();

            string id = Session["EmpID"].ToString().Trim();

            int dbCount = dbContext.tblInchargePersons.Where(x => x.EmpID == id && x.IsAssesstor == true).Count();

            if (dbCount == 1)
            {
                divDetails.Visible = true;

                divRecords.Visible = false;
            }
            else if (dbCount > 1)
            {
                divDetails.Visible = true;

                divRecords.Visible = false;
            }
            else
            {
                divDetails.Visible = false;

                divRecords.Visible = true;

                gridRecords.Rebind();

                Show("You are not authorize to assess this request.");
            }
        }

        protected void lnkResend_Click(object sender, EventArgs e)
        {
            string link = string.Empty, body = string.Empty, jono = string.Empty;

            LinkButton lnkResendRequest = (LinkButton)sender;

            jrcode = lnkResendRequest.CommandArgument.ToString().Trim();

            var verify = dbContext.tblAttachments.Where(x => x.jr_code == jrcode && x.typeofattachment.Trim() == "Admin"
                                    && x.IsPrepared == true && x.IsChecked == true && x.IsApproved == false).Count();

            if (verify >= 1)
            {
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

                    body += "<br /><br />" + "I Have Prepared A Job Order Request Attachment/s For Your Checking and Approval.";

                    body += "<br /><br />" + "You may click the link below to redirect to main page. ";

                    body += "<br /><br />" + link + "?EmpID=" + dr["EmpID"].ToString().Trim() + "&RCode=" + Session["RCode"].ToString().Trim();

                    body += "<br /><br />" + "Thank You.";

                    body += "<br /><br />" + "Note: This is a system generated email. Please do not reply. Thank you";


                    var rx = new Regex(@"(?<=\w)\w");

                    var newString2 = rx.Replace(Session["FullName_FnameFirst"].ToString(), new MatchEvaluator(m => m.Value.ToLowerInvariant()));

                    using (MailMessage mm = new MailMessage())
                    {
                        string sub = "Online Job Order Request: Requesting For Attachment Approval";

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
            }
            else
            {
                var app = dbContext.tblRequests.Where(x => x.JR_Code == jrcode).FirstOrDefault();

                if (app != null)
                {
                    app.IsHold = false;

                    app.AttachementStatus = "PREPARED";

                    app.CreationDateTime = DateTime.Now;

                    dbContext.Entry(app).State = EntityState.Modified;

                    dbContext.SaveChanges();
                }


                var accept = dbContext.tblAttachments.Where(x => x.jr_code == jrcode
                        && x.typeofattachment.Trim() == "Admin").FirstOrDefault();

                if (accept != null)
                {
                    accept.PreparedBy = Session["FullName_FnameFirst"].ToString();

                    accept.IsPrepared = true;

                    accept.datecreated = DateTime.Now;

                    dbContext.Entry(accept).State = EntityState.Modified;

                    dbContext.SaveChanges();
                }

                link = Environment.NewLine + "http://" + HttpContext.Current.Request.Url.Authority + "/"
                             + ConfigurationManager.AppSettings["AttachmentNoterPage"];

                var asstr = (from data in dbContext.tblUsers
                             join inchargePerson in dbContext.tblInchargePersons on data.EmpID equals inchargePerson.EmpID
                             into UP
                             from inchargePerson in UP.DefaultIfEmpty()
                             where inchargePerson.Incharge_ID == rdoInCharge.SelectedValue && inchargePerson.IsNoter == true
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
                    body = "Dear Sir/Ma'am," + "<br /><br />" + "Good Day!";

                    body += "<br /><br />" + "I Have Prepared A Job Order Request Attachment/s For Your Checking and Approval.";

                    body += "<br /><br />" + "You may click the link below to redirect to main page. ";

                    body += "<br /><br />" + link + "?EmpID=" + dr["EmpID"].ToString().Trim() + "&RCode=" + Session["RCode"].ToString().Trim();

                    body += "<br /><br />" + "Thank You.";

                    body += "<br /><br />" + "Note: This is a system generated email. Please do not reply. Thank you";


                    var rx = new Regex(@"(?<=\w)\w");

                    var newString2 = rx.Replace(Session["FullName_FnameFirst"].ToString(), new MatchEvaluator(m => m.Value.ToLowerInvariant()));

                    using (MailMessage mm = new MailMessage())
                    {
                        string sub = "Online Job Order Request: Requesting For Attachment Approval";

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
            }
        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            LinkButton lnkDelete = (LinkButton)sender;

            int id = Convert.ToInt32(lnkDelete.CommandArgument);

            var delete = dbContext.tblAttachments.Where(p => p.id == id).FirstOrDefault();

            dbContext.tblAttachments.Remove(delete);

            dbContext.SaveChanges();

            Show("Record is deleted!");

            gridAttachment.Rebind();

            gridAttachment2.Rebind();
        }

        protected void UploadMultipleFileExisting(object sender, EventArgs e)
        {
            if (uploadFile.UploadedFiles.Count == 0)
            {
                Show("Select File First");
            }
            else
            {
                //Convert File To Base64String Before Saving to Database

                string fileext = string.Empty;

                foreach (UploadedFile file in uploadFile.UploadedFiles)
                {
                    byte[] bytes = new byte[file.ContentLength];

                    file.InputStream.Read(bytes, 0, bytes.Length);

                    filename = file.GetName().Replace(",", "_").Replace(" ", "_");

                    //filetype = file.ContentType;

                    fileext = Path.GetExtension(filename);

                    if (fileext == ".pdf")
                    {
                        filetype = "application/pdf";
                    }
                    else if (fileext == ".xls")
                    {
                        filetype = "application/vnd.ms-excel";
                    }
                    else if (fileext == ".xlsx")
                    {
                        filetype = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    }
                    else if (fileext.ToLower() == ".pptx")
                    {
                        filetype = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                    }
                    else if (fileext.ToLower() == ".ppt")
                    {
                        filetype = "application/vnd.ms-powerpoint";
                    }
                    else if (fileext.ToLower() == ".docx")
                    {
                        filetype = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    }
                    else if (fileext.ToLower() == ".doc")
                    {
                        filetype = "application/msword";
                    }
                    else if (fileext.ToLower() == ".jpeg" || fileext == ".jpg")
                    {
                        filetype = "image/jpeg";
                    }

                    else if (fileext.ToLower() == ".png")
                    {
                        filetype = "image/png";
                    }

                    Response.Charset = string.Empty;

                    string code = Session["RCode"].ToString().Trim();

                    //var a = dbContext.tblAttachments.Where(x => x.attachmentname == filename && x.jr_code == code
                    //&& x.typeofattachment == "Admin").FirstOrDefault();

                    //if(a != null)
                    //{
                        tblAttachment tbl = new tblAttachment()
                        {
                            attachmentfile = Convert.ToBase64String(bytes, 0, bytes.Length),
                            attachmentname = filename,
                            contenttype = filetype,
                            jr_code = code,//Session["RCode"].ToString().Trim(),
                            typeofattachment = "Admin",
                            IsChecked = false,
                            IsPrepared = false,
                            IsApproved = false,
                            IsCheckerReviewed = false,
                            IsApproverReviewed = false,
                            issubmitted = true,
                            datecreated = DateTime.Now
                        };

                        dbContext.tblAttachments.Add(tbl);

                        dbContext.SaveChanges();
                    //}                    
                }

                gridAttachment2.Rebind();
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

        protected void btnaccept_Click(object sender, EventArgs e)
        {
            string link = string.Empty, body = string.Empty, jono = string.Empty;

            if (RadCheckBox1.Checked == true)
            {
                var app = dbContext.tblRequests.Where(x => x.JR_Code == jrcode).FirstOrDefault();

                if (app != null)
                {
                    app.IsHold = false;

                    app.Is_AttachmentApproved = true;

                    app.AttachementStatus = "APPROVED";

                    app.CreationDateTime = DateTime.Now;

                    dbContext.Entry(app).State = EntityState.Modified;

                    dbContext.SaveChanges();
                }

                divDetails.Visible = false;

                divRecords.Visible = true;

                jrcode = string.Empty;

                Session["RCode"] = null;

                gridRecords.Rebind();

                Show("JO has been successfully accepted");
            }
            else
            {
                if (gridAttachment2.MasterTableView.Items.Count > 0)
                {
                    var verify = dbContext.tblAttachments.Where(x => x.jr_code == jrcode && x.typeofattachment.Trim() == "Admin"
                                    && x.IsPrepared == true && x.IsChecked == true && x.IsApproved == false).Count();

                    if (verify >= 1)
                    {
                        link = Environment.NewLine + "http://" + "192.168.1.42:5033"///HttpContext.Current.Request.Url.Authority 
                            + "/"
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

                            body += "<br /><br />" + "I Have Prepared A Job Order Request Attachment/s For Your Checking and Approval.";

                            body += "<br /><br />" + "You may click the link below to redirect to main page. ";

                            body += "<br /><br />" + link + "?EmpID=" + dr["EmpID"].ToString().Trim() + "&RCode=" + Session["RCode"].ToString().Trim();

                            body += "<br /><br />" + "Thank You.";

                            body += "<br /><br />" + "Note: This is a system generated email. Please do not reply. Thank you";


                            var rx = new Regex(@"(?<=\w)\w");

                            var newString2 = rx.Replace(Session["FullName_FnameFirst"].ToString(), new MatchEvaluator(m => m.Value.ToLowerInvariant()));

                            using (MailMessage mm = new MailMessage())
                            {
                                string sub = "Online Job Order Request: Requesting For Attachment Approval";

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
                    }
                    else
                    {
                        //string link = string.Empty, body = string.Empty;


                        var app = dbContext.tblRequests.Where(x => x.JR_Code == jrcode).FirstOrDefault();

                        if (app != null)
                        {
                            app.IsHold = false;

                            app.AttachementStatus = "PREPARED";

                            app.CreationDateTime = DateTime.Now;

                            dbContext.Entry(app).State = EntityState.Modified;

                            dbContext.SaveChanges();
                        }


                        var accept = dbContext.tblAttachments.Where(x => x.jr_code == jrcode
                                && x.typeofattachment.Trim() == "Admin").FirstOrDefault();

                        if (accept != null)
                        {
                            accept.PreparedBy = Session["FullName_FnameFirst"].ToString();

                            accept.IsPrepared = true;

                            accept.datecreated = DateTime.Now;

                            dbContext.Entry(accept).State = EntityState.Modified;

                            dbContext.SaveChanges();
                        }

                        link = Environment.NewLine + "http://" + "192.168.1.42:5033"///HttpContext.Current.Request.Url.Authority 
                            + "/"
                                     + ConfigurationManager.AppSettings["AttachmentNoterPage"];

                        var asstr = (from data in dbContext.tblUsers
                                     join inchargePerson in dbContext.tblInchargePersons on data.EmpID equals inchargePerson.EmpID
                                     into UP
                                     from inchargePerson in UP.DefaultIfEmpty()
                                     where inchargePerson.Incharge_ID == incharge && inchargePerson.IsNoter == true
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
                            body = "Dear Sir/Ma'am," + "<br /><br />" + "Good Day!";

                            body += "<br /><br />" + "I Have Prepared A Job Order Request Attachment/s For Your Checking and Approval.";

                            body += "<br /><br />" + "You may click the link below to redirect to main page. ";

                            body += "<br /><br />" + link + "?EmpID=" + dr["EmpID"].ToString().Trim() + "&RCode=" + Session["RCode"].ToString().Trim();

                            body += "<br /><br />" + "Thank You.";

                            body += "<br /><br />" + "Note: This is a system generated email. Please do not reply. Thank you";


                            var rx = new Regex(@"(?<=\w)\w");

                            var newString2 = rx.Replace(Session["FullName_FnameFirst"].ToString(), new MatchEvaluator(m => m.Value.ToLowerInvariant()));

                            using (MailMessage mm = new MailMessage())
                            {
                                string sub = "Online Job Order Request: Requesting For Attachment Approval";

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
                    }

                    divDetails.Visible = false;

                    divRecords.Visible = true;

                    jrcode = string.Empty;

                    Session["RCode"] = null;

                    gridRecords.Rebind();

                    Show("Successfully Send To The Next Approver");
                }
                else
                {
                    Show("Must Upload Attachment First");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadYear();

                rdoInCharge.DataSource = dbContext.tblIncharges.ToList();
                rdoInCharge.DataBind();

                rdoReqType.DataSource = dbContext.tblRequestTypes.ToList();
                rdoReqType.DataBind();
            }

            SQLDSGetAttachment.ConnectionString = ClsConfig.JobRequestConnectionString;

            SQLDSGetAttachment2.ConnectionString = ClsConfig.JobRequestConnectionString;

            getRecords.ConnectionString = ClsConfig.JobRequestConnectionString;
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
    }
}
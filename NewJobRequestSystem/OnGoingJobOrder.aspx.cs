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
    public partial class OnGoingJobOrder : System.Web.UI.Page
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
            , useremail = string.Empty, incharge = string.Empty, jrcode = string.Empty
            , att_preparer = string.Empty, att_checker = string.Empty, att_approver = string.Empty;

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

        protected void btnaccept_Click(object sender, ImageClickEventArgs e)
        {
            if (!dpAccomplish.SelectedDate.HasValue)
            {
                Show("Please set date accomplish.");
            }
            //else if (gridAttachment2.MasterTableView.Items.Count == 0)
            //{
            //    Show("Please add an attachment.");
            //}
            else
            {
                var completed = dbContext.tblRequests.Where(x => x.JR_Code == jrcode).FirstOrDefault();

                if (completed != null)
                {
                    completed.IsCompleted = true;

                    completed.CompletionRemarks = tbRemarks.Text.Trim();

                    completed.Completion_Date = dpAccomplish.SelectedDate.Value;

                    completed.CreationDateTime = DateTime.Now;

                    dbContext.Entry(completed).State = EntityState.Modified;

                    dbContext.SaveChanges();
                }

                EmailRequestor();

                Show("Request Has Been Successfully Finished");

                divDetails.Visible = false;

                divRecords.Visible = true;

                jrcode = string.Empty;

                Session["RCode"] = null;

                gridRecords.Rebind();
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

        private void EmailRequestor()
        {
            string link = string.Empty, body = string.Empty;

            link = Environment.NewLine + "http://" + HttpContext.Current.Request.Url.Authority;

            body = "Dear Sir/Ma'am," + "<br /><br />" + "Good Day!";

            body += "<br /><br />" + "Please be informed your job order request with a request number of " + tbJobOrderNo.Text.Trim() + " has been completed.";

            body += "<br /><br />" + "Kindly login to your account to receive and accept the completion of the request.";

            body += "<br /><br />" + "You may click the link below to redirect to main page. ";

            body += "<br /><br />" + link;

            body += "<br /><br />" + "Thank You.";

            body += "<br /><br />" + "Note: This is a system generated email. Please do not reply. Thank you";

            var rx = new Regex(@"(?<=\w)\w");

            var newString2 = rx.Replace(Session["FullName_FnameFirst"].ToString(), new MatchEvaluator(m => m.Value.ToLowerInvariant()));

            using (MailMessage mm = new MailMessage())
            {
                string sub = "Online Job Order Request: Completed Request";

                mm.Subject = sub.ToUpper();

                mm.Body = body;

                var newString = rx.Replace(Session["FullName_FnameFirst"].ToString(), new MatchEvaluator(m => m.Value.ToLowerInvariant()));

                mm.From = new MailAddress(ConfigurationManager.AppSettings["MailSenderEmailAddress"].ToString(),

                    ConfigurationManager.AppSettings["MailSenderName"].ToString());

                mm.To.Add(useremail.Trim());

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

                if (row.IsNull(36))
                {
                    tbIsExistingJig.Text = "N/A";
                }
                else
                {
                    if (Convert.ToBoolean(row[36]) == true)
                    {
                        tbIsExistingJig.Text = "Yes";
                    }
                    else
                    {
                        tbIsExistingJig.Text = "No";
                    }
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

                lblDateSigned_Preparer.InnerText = string.IsNullOrEmpty(row[0].ToString()) ? string.Empty : Convert.ToDateTime(row[0]).ToString("yyyy-MMM-dd");

                lblDateSigned_Checker.InnerText = string.IsNullOrEmpty(row[31].ToString()) ? string.Empty : Convert.ToDateTime(row[31]).ToString("yyyy-MMM-dd");

                lblDateSigned_Assesstor.InnerText = string.IsNullOrEmpty(row[32].ToString()) ? string.Empty : Convert.ToDateTime(row[32]).ToString("yyyy-MMM-dd");

                lblDateSigned_Noter.InnerText = string.IsNullOrEmpty(row[33].ToString()) ? string.Empty : Convert.ToDateTime(row[33]).ToString("yyyy-MMM-dd");

                lblDateSigned_Approver.InnerText = string.IsNullOrEmpty(row[34].ToString()) ? string.Empty : Convert.ToDateTime(row[34]).ToString("yyyy-MMM-dd");

                lblDateSigned_Receiver.InnerText = string.IsNullOrEmpty(row[35].ToString()) ? string.Empty : Convert.ToDateTime(row[35]).ToString("yyyy-MMM-dd");
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

        protected void Page_Load(object sender, EventArgs e)
        {
            SQLDSGetAttachment.ConnectionString = ClsConfig.JobRequestConnectionString;

            SQLDSGetAttachment2.ConnectionString = ClsConfig.JobRequestConnectionString;

            getRecords.ConnectionString = ClsConfig.JobRequestConnectionString;

            if (!IsPostBack)
            {
                rdoInCharge.DataSource = dbContext.tblIncharges.Where(x => x.Incharge_ID == "07" || x.Incharge_ID == "08").ToList();
                rdoInCharge.DataBind();

                rdoReqType.DataSource = dbContext.tblRequestTypes.ToList();
                rdoReqType.DataBind();
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

                    tblAttachment empdata = new tblAttachment();

                    var selectQuery2 = dbContext.tblAttachments.Where(x => x.jr_code == jrcode.Trim() && x.typeofattachment == "Admin" 
                    && x.IsPrepared == true && x.IsChecked == true && x.IsApproved == true).OrderBy(p => p.id).FirstOrDefault();


                    if(selectQuery2 != null)
                    {
                        tblAttachment myData = selectQuery2 as tblAttachment;

                        att_preparer = myData.PreparedBy;

                        att_checker = myData.CheckedBy;

                        att_approver = myData.ApprovedBy;
                    }
                    else
                    {
                        att_preparer = "N/A";

                        att_checker = "N/A";

                        att_approver = "N/A";
                    }

                    //if (dbContext.tblAttachments.Where(x => x.jr_code == Session["RCode"].ToString().Trim() && x.typeofattachment == "Admin" && x.IsPrepared == true && x.IsChecked == true && x.IsApproved == true).ToList().Count() > 0)
                    //{
                    tblAttachment tbl = new tblAttachment()
                    {
                        attachmentfile = Convert.ToBase64String(bytes, 0, bytes.Length),
                        attachmentname = filename,
                        contenttype = filetype,
                        jr_code = Session["RCode"].ToString().Trim(),
                        typeofattachment = "Admin",
                        PreparedBy = att_preparer,
                        CheckedBy = att_checker,
                        ApprovedBy = att_approver,
                        IsChecked = true,
                        IsPrepared = true,
                        IsApproved = true,
                        IsCheckerReviewed = true,
                        IsApproverReviewed = true,
                        issubmitted = true,
                        datecreated = DateTime.Now
                    };
                    //}

                    dbContext.tblAttachments.Add(tbl);

                    dbContext.SaveChanges();

                    gridAttachment2.Rebind();
                }
            }
        }
    }
}
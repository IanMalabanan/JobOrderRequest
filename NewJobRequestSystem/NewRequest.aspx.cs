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
using System.Text.RegularExpressions;
using System.Configuration;
using System.ComponentModel;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Data.Entity;

namespace NewJobRequestSystem
{
    public partial class NewRequest : System.Web.UI.Page
    {
        JobRequestDBEntities dbContext = new JobRequestDBEntities();

        HRISEntities dbHRISContext = new HRISEntities();

        ClsGenerateRandomString random = new ClsGenerateRandomString();

        public static string filename = string.Empty, filetype = string.Empty, appr_name = string.Empty;  


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

        public DataTable GetSV_HeadBySubordinate(string empid)
        {
            var a = dbContext.GetSV_HeadBySubordinate(empid).ToList();

            return ConvertToDataTable(a);
        }

        public DataTable GetManager(string empid)
        {
            var a = dbContext.GetManager(empid).ToList();

            return ConvertToDataTable(a);
        }

        public DataTable GetVicePresident()
        {
            var a = dbContext.GetVicePresident().ToList();

            return ConvertToDataTable(a);
        }

        public DataTable GetManagerForSmart()
        {
            var a = dbContext.GetManagerForSmart().ToList();

            return ConvertToDataTable(a);
        }

        public DataTable GetManagerForIT()
        {
            var a = dbContext.GetManagerForIT().ToList();

            return ConvertToDataTable(a);
        }

        private DataTable GetManagerForSafety()
        {
            var a = dbContext.GetManagerForSafety().ToList();

            return ConvertToDataTable(a);
        }

        private DataTable GetJigType()
        {
            SqlCommand cmd = new SqlCommand("SELECT JigType FROM [dbo].[tblJigType]");

            cmd.CommandType = CommandType.Text;

            return SqlHelper.ExecuteDataReader(ClsConfig.JobRequestConnectionString, cmd);
        }

        private DataTable GetCustomers()
        {
            SqlCommand cmd = new SqlCommand("GetCustomerList");

            cmd.CommandType = CommandType.StoredProcedure;

            return SqlHelper.ExecuteDataReader(ClsConfig.SomsConnectionString, cmd);
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
               
        private void Show(string msg)
        {
            Page page = HttpContext.Current.Handler as Page;
            if (page != null)
            {
                ScriptManager.RegisterStartupScript(page, page.GetType(), "Message", "alert('" + msg + "');", true);
            }
        }

        private Boolean VerifyFields()
        {
            if (tbjustification.Text.Length == 0)
            {
                Show("Please insert justification.");
                return false;
            }

            if (rdoInCharge.SelectedIndex == -1)
            {
                Show("Please select incharge.");
                return false;
            }


            if(rdoReqType.SelectedItems.Count == 0)
            {
                Show("Please select type of request.");
                return false;
            }

            if(gridAttachment.MasterTableView.Items.Count == 0)
            {
                Show("Please upload some attachment.");
                return false;
            }
            
            if (rdoInCharge.SelectedValue == "04" || rdoInCharge.SelectedValue == "05" || rdoInCharge.SelectedValue == "08")
            {
                if (cboCustomer.Text.Trim().Length == 0)
                {
                    Show("Please insert customer.");
                    return false;
                }

                if (tbPartCode.Text.Trim().Length == 0)
                {
                    Show("Please insert part code.");
                    return false;
                }

                if (tbPartName.Text.Trim().Length == 0)
                {
                    Show("Please insert partname.");
                    return false;
                }

                if (cboTypeOfJig.Text.Trim().Length == 0)
                {
                    Show("Please insert jig type.");
                    return false;
                }

                if (tbQuantity_Jigs.Text.Trim().Length == 0)
                {
                    Show("Please insert quantity.");
                    return false;
                }

                if (qtyhrtxt.Text.Trim().Length == 0)
                {
                    Show("Please insert quantity per hour.");
                    return false;
                }

                if (mreqtxt.Text.Trim().Length == 0)
                {
                    Show("Please insert monthly requirement.");
                    return false;
                }

                if (mcaptxt.Text.Trim().Length == 0)
                {
                    Show("Please insert machine capacity.");
                    return false;
                }

                if (tbNextForecast.Text.Trim().Length == 0)
                {
                    Show("Please insert next forecast.");
                    return false;
                }
            }
            else
            {
                if (tbItemName.Text.Trim().Length == 0)
                {
                    Show("Please insert item name.");
                    return false;
                }

                if (tbItemQuantity.Text.Trim().Length == 0)
                {
                    Show("Please insert quantity.");
                    return false;
                }
            }

            return true;
        }

        private void ClearFields()
        {
            tbItemName.Text = tbItemQuantity.Text
                     = tbPartCode.Text
                     = tbPartName.Text
                     = tbQuantity_Jigs.Text
                     = qtyhrtxt.Text
                     = mreqtxt.Text
                     = mcaptxt.Text
                     = tbNextForecast.Text = string.Empty;

            cboCustomer.SelectedIndex = cboTypeOfJig.SelectedIndex = -1;
        }

        private void UpdateAttachmentStatus()
        {
            string code = Session["RCode"].ToString().Trim();
            var query = dbContext.tblAttachments.Where(x => x.issubmitted == false
            && x.jr_code == code).ToList();
            query.ForEach(a => a.issubmitted = true);
            
            dbContext.SaveChanges();

            //string code2 = Session["RCode"].ToString().Trim();
            //var query2 = dbContext.tblUniqueCodes.Where(x => x.isused == false
            //&& x.jr_code == code).ToList();
            //query2.ForEach(a => a.isused = true);

            //dbContext.SaveChanges();
        }

        private Boolean _SaveRequest()
        {
            Boolean _isValid = false;

            _isValid = VerifyFields();

            if (!_isValid)
                return false;
            
            DataTable dtApprover;

            string link = string.Empty, body = string.Empty;

            Regex r = new Regex("Manager");
            bool containsManager = r.IsMatch(Session["UserRole"].ToString().TrimEnd());

            Regex r2 = new Regex("Supervisor");
            bool containsSupervisor = r2.IsMatch(Session["UserRole"].ToString().TrimEnd());

            Regex r1 = new Regex("Employee|Head");
            bool containsEmployee = r1.IsMatch(Session["UserRole"].ToString().TrimEnd());

            if (containsManager == true && containsSupervisor == false && containsEmployee == false)
            {
                if (Session["deptcode"].ToString().Trim() == "Z")
                {
                    dtApprover = GetManagerForSafety();

                    link = Environment.NewLine + "http://" + HttpContext.Current.Request.Url.Authority + "/"
                     + ConfigurationManager.AppSettings["CheckerPage"];
                }
                else if (Session["deptcode"].ToString().Trim() == "X" || Session["deptcode"].ToString().Trim() == "H")
                {
                    dtApprover = GetManager(Session["empid"].ToString());

                    link = Environment.NewLine + "http://" + HttpContext.Current.Request.Url.Authority + "/"
                     + ConfigurationManager.AppSettings["CheckerPage"];
                }
                else
                {
                    dtApprover = GetManager(Session["empid"].ToString());

                    link = Environment.NewLine + "http://" + HttpContext.Current.Request.Url.Authority + "/"
                     + ConfigurationManager.AppSettings["CheckerPage"];
                }
            }
            else if (containsManager == false && containsSupervisor == true && containsEmployee == false)
            {
                if (Session["deptcode"].ToString().Trim() == "Z")
                {
                    dtApprover = GetManagerForSafety();
                }
                else if (Session["deptcode"].ToString().Trim() == "D")
                {
                    dtApprover = GetManagerForSmart();
                }
                else
                {
                    dtApprover = GetManager(Session["empid"].ToString().TrimEnd());
                }

                link = Environment.NewLine + "http://" + HttpContext.Current.Request.Url.Authority + "/"
                     + ConfigurationManager.AppSettings["CheckerPage"];
            }
            else if (containsManager == false && containsSupervisor == false && containsEmployee == true)
            {
                if (Session["deptcode"].ToString().Trim() == "I")
                {
                    dtApprover = GetManagerForIT();

                    link = Environment.NewLine + "http://" + HttpContext.Current.Request.Url.Authority + "/"
                     + ConfigurationManager.AppSettings["CheckerPage"];
                }
                else
                {
                    dtApprover = GetSV_HeadBySubordinate(Session["empid"].ToString());

                    link = Environment.NewLine + "http://" + HttpContext.Current.Request.Url.Authority + "/"
                     + ConfigurationManager.AppSettings["CheckerPage"];
                }
            }
            else
            {
                dtApprover = GetSV_HeadBySubordinate(Session["empid"].ToString());

                link = Environment.NewLine + "http://" + HttpContext.Current.Request.Url.Authority + "/"
                     + ConfigurationManager.AppSettings["CheckerPage"];
            }
            

            foreach (DataRow dr in dtApprover.Rows)
            {
                appr_name = dr["FullName_FnameFirst"].ToString().Trim();
            }

            //string appempid = string.Empty;

            //foreach (DataRow dr in ConvertToDataTable(asstr).Rows)
            //{
            //    appempid = dr["EmpID"].ToString().Trim();
            //}

            //var dbuser = dbHRISContext.SKPI_GetAllEmployeesByEmpID(appempid.Trim()).ToList();

            //if (ConvertToDataTable(dbuser).Rows.Count > 0)
            //{
            //    foreach (DataRow row in ConvertToDataTable(dbuser).Rows)
            //    {
            //        nextApprover = row["FullName_FnameFirst"].ToString().Trim();
            //    }
            //}

            SaveRequest();

            foreach (DataRow dr in dtApprover.Rows)
            {
                
                Boolean ress;

                ress = SaveEmailLogs(link + "?EmpID=" + dr["EmpID"].ToString().Trim() + "&RCode=" + Session["RCode"].ToString().Trim(), "Checker", dr["EmailAdd"].ToString().Trim(), Session["FullName_LnameFirst"].ToString().Trim()
                    , dr["EmpID"].ToString().Trim(), Session["RCode"].ToString().Trim());

                body = "Dear Sir/Ma'am," + "<br /><br />" + "Good Day!";

                body += "<br /><br />" + "I Have Prepared A Job Order Request For Your Checking And Approval.";

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

            divSuccess.Visible = true;

            divApplication.Visible = false;

            return true;
        }

        private void SaveRequest()
        {
            double s1, s2, s3, s4, s5, s6;

            if (rdoInCharge.SelectedValue == "04" || rdoInCharge.SelectedValue == "05" || rdoInCharge.SelectedValue == "08")
            {
                if (double.TryParse(tbQuantity_Jigs.Text, out var outParse))
                {
                    s1 = Convert.ToDouble(tbQuantity_Jigs.Text.Trim());
                }
                else
                {
                    s1 = 0;
                }

                if (double.TryParse(qtyhrtxt.Text, out var outParse2))
                {
                    s2 = Convert.ToDouble(qtyhrtxt.Text.Trim());
                }
                else
                {
                    s2 = 0;
                }

                if (double.TryParse(mreqtxt.Text, out var outParse3))
                {
                    s3 = Convert.ToDouble(mreqtxt.Text.Trim());
                }
                else
                {
                    s3 = 0;
                }

                if (double.TryParse(mcaptxt.Text, out var outParse4))
                {
                    s4 = Convert.ToDouble(mcaptxt.Text.Trim());
                }
                else
                {
                    s4 = 0;
                }

                if (double.TryParse(tbNextForecast.Text, out var outPars5))
                {
                    s5 = Convert.ToDouble(tbNextForecast.Text.Trim());
                }
                else
                {
                    s5 = 0;
                }

                tblRequest tbl = new tblRequest()
                {
                    EmpID = Session["EmpID"].ToString().Trim(),
                    DeptCode = Session["DeptCode"].ToString().Trim(),
                    SectCode = Session["SectCode"].ToString().Trim(),
                    Request_Date = DateTime.Now.Date,
                    Incharge_ID = rdoInCharge.SelectedValue.ToString().Trim(),
                    Customer = cboCustomer.SelectedValue.ToString().Trim(),
                    Partcode = tbPartCode.Text.Trim(),
                    Partname = tbPartName.Text.Trim(),
                    JigType = cboTypeOfJig.SelectedValue.ToString().Trim(),
                    Quantity = s1,
                    QtyProdPerHr = s2,
                    Monthly_Req = s3,
                    Machine_Capacity = s4,
                    NextForeCast = s5,
                    Problem_Desc = tbjustification.Text.Trim().Replace("\r\n", "<br/>").Replace(" ", "&nbsp;").Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;").Replace("'", "^"),
                    Is_Submitted = true,
                    IsHold = false,
                    Is_Checked = false,
                    Is_Reviewed = false,
                    Is_Assessed = false,
                    Is_JobAccepted = false,
                    Is_Approved = false,
                    Is_Noted = false,
                    Is_AttachmentApproved = false,
                    IsCompleted = false,
                    JR_Code = Session["RCode"].ToString().Trim(),
                    IsCancelled = false,
                    IsRejected = false,
                    Req_Remarks = "SIGNED",
                    Checker = appr_name.Trim(),
                    AssessedBy = "N/A",
                    Noter = "N/A",
                    Manager = "N/A",
                    FinalAcceptanceBy = "N/A",
                    IsSentToApprover_First = false,
                    IsSentToApprover_Second = false,
                    IsSentToApprover_Third = false,
                    CreationDateTime = DateTime.Now
                };

                dbContext.tblRequests.Add(tbl);

                dbContext.SaveChanges();
            }
            else
            {
                if (double.TryParse(tbItemQuantity.Text, out var outParse))
                {
                    s6 = Convert.ToDouble(tbItemQuantity.Text.Trim());
                }
                else
                {
                    s6 = 0;
                }

                tblRequest tbl = new tblRequest()
                {
                    EmpID = Session["EmpID"].ToString().Trim(),
                    DeptCode = Session["DeptCode"].ToString().Trim(),
                    SectCode = Session["SectCode"].ToString().Trim(),
                    Request_Date = DateTime.Now.Date,
                    Incharge_ID = rdoInCharge.SelectedValue.ToString().Trim(),
                    Partname = tbItemName.Text.Trim(),
                    Quantity = s6,
                    Problem_Desc = tbjustification.Text.Trim().Replace(" ", "&nbsp;").Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;").Replace("'", "^").Replace("\r\n", "<br/>"),
                    Is_Submitted = true,
                    IsHold = false,
                    Is_Checked = false,
                    Is_Reviewed = false,
                    Is_Assessed = false,
                    Is_JobAccepted = false,
                    Is_Approved = false,
                    Is_AttachmentApproved = false,
                    Is_Noted = false,
                    IsCompleted = false,
                    JR_Code = Session["RCode"].ToString().Trim(),
                    IsCancelled = false,
                    IsRejected = false,
                    Req_Remarks = "SIGNED",                    
                    Checker = appr_name.Trim(),
                    AssessedBy = "N/A",
                    Noter = "N/A",
                    Manager = "N/A",
                    FinalAcceptanceBy = "N/A",
                    IsSentToApprover_First = false,
                    IsSentToApprover_Second = false,
                    IsSentToApprover_Third = false,
                    CreationDateTime = DateTime.Now
                };

                dbContext.tblRequests.Add(tbl);

                dbContext.SaveChanges();
            }

            foreach (ButtonListItem item in rdoReqType.Items)
            {
                if (item.Selected)
                {
                    tblRequestType_Selected tbl = new tblRequestType_Selected()
                    {
                        JR_Code = Session["RCode"].ToString(),
                        RequestTypeCode = item.Value.ToString()
                    };

                    dbContext.tblRequestType_Selected.Add(tbl);
                }
            }

            dbContext.SaveChanges();

            UpdateAttachmentStatus();
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

        private string GenerateCode()
        {
            string code = string.Empty;

            Random rs = new Random();

            int rInt = rs.Next(15, 350);

            code = random.RandomAlphanumericString(rInt);

            var rcode = dbContext.tblUniqueCodes.Where(x => x.jr_code == code //&& (x.isused == true || x.isused == false)
            ).FirstOrDefault();

            if (rcode != null)
            {
                Random rs2 = new Random();

                int rInt2 = rs2.Next(15, 350);

                code = random.RandomAlphanumericString(rInt2);
            }
            else
            {
                tblUniqueCode r = new tblUniqueCode()
                {
                    jr_code = code,
                    isused = true,
                    creationdatetime = DateTime.Now
                };

                dbContext.tblUniqueCodes.Add(r);

                dbContext.SaveChanges();
            }

            return code;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["EmpID"] == null)
            {
                Response.Redirect("Default.aspx");
            }
            else
            {
                DeleteUnusedCodes();

                if (!IsPostBack)
                {
                    rdoInCharge.DataSource = dbContext.tblIncharges.Where(x => x.Incharge_ID == "07" || x.Incharge_ID == "08").ToList();
                    rdoInCharge.DataBind();

                    rdoReqType.DataSource = dbContext.tblRequestTypes.ToList();
                    rdoReqType.DataBind();

                    Session["RCode"] = GenerateCode();

                    rdoInCharge.SelectedIndex = 0;

                    cboCustomer.DataSource = GetCustomers();
                    cboCustomer.DataBind();

                    cboTypeOfJig.DataSource = GetJigType();
                    cboTypeOfJig.DataBind();
                }

                SQLDSGetAttachment.ConnectionString = ClsConfig.JobRequestConnectionString;

                tbdateRequest.Text = DateTime.Now.Date.ToString("dd") + "-" + DateTime.Now.Date.ToString("MMM") + "-" + DateTime.Now.Date.ToString("yyyy");

                tbReqName.Text = Session["FullName_FnameFirst"].ToString().Trim();

                tbDepartment.Text = Session["DeptName"].ToString().Trim();

                tbSection.Text = Session["SectName"].ToString().Trim();
            }
        }

        protected void rdoInCharge_SelectedIndexChanged(object sender, EventArgs e)
        {
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

            ClearFields();
        }

        protected void RadGrid1_ItemDeleted(object sender, Telerik.Web.UI.GridDeletedEventArgs e)
        {
            GridDataItem dataItem = (GridDataItem)e.Item;

            //String id = dataItem.GetDataKeyValue("id").ToString();

            if (e.Exception != null)
            {
                e.ExceptionHandled = true;

                Show("Record cannot be deleted. Reason: " + e.Exception.Message);
            }
            else
            {
                Show("Record is deleted!");

                gridAttachment.Rebind();
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

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Boolean res;

            res = _SaveRequest();            
        }

        protected void btnGoToMain_Click(object sender, EventArgs e)
        {
            Response.Redirect("HomePage.aspx");
        }

        protected void cboTypeOfJig_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            string sqlSelectCommand = "SELECT JigType FROM tblJigType WHERE JigType " +
                                      "LIKE @text + '%' ORDER By JigType";

            SqlCommand cmd = new SqlCommand(sqlSelectCommand);

            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("@text", e.Text);

            cboTypeOfJig.DataSource = SqlHelper.ExecuteDataReader(ClsConfig.SomsConnectionString, cmd);
            cboTypeOfJig.DataBind();
        }

        protected void cboCustomer_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            string sqlSelectCommand = "SELECT Cust_ID, Cust_Name, Cust_Acronym, Street_Address,  [IsSubCon], " +
                                      "[Address] = (RTRIM([Street_Address]) + ', ' + RTRIM([City]) + ', ' + RTRIM([Province]) + ', ' + RTRIM([Country])),  " +
                                      "[ShortAddress] = (RTRIM([City]) + ', ' + RTRIM([Province])),  " +
                                      "[TPiCS_CustCode] FROM Customer WHERE Active = 1 and Cust_Acronym " +
                                      "LIKE @text + '%' ORDER By Cust_Name";

            SqlCommand cmd = new SqlCommand(sqlSelectCommand);

            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("@text", e.Text);

            cboCustomer.DataSource = SqlHelper.ExecuteDataReader(ClsConfig.SomsConnectionString, cmd);
            cboCustomer.DataBind();
        }

        protected void cboTypeOfJig_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
           
        }

        protected void cboCustomer_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            //Show(cboCustomer.SelectedValue.ToString().Trim());
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

                    tblAttachment tbl = new tblAttachment()
                    {
                        attachmentfile = Convert.ToBase64String(bytes, 0, bytes.Length),
                        attachmentname = filename,
                        contenttype = filetype,
                        jr_code = Session["RCode"].ToString().Trim(),
                        typeofattachment = "User",
                        IsPrepared = false,
                        IsChecked = false,
                        IsApproved = false,
                        issubmitted = false,
                        datecreated = DateTime.Now
                    };

                    dbContext.tblAttachments.Add(tbl);

                    dbContext.SaveChanges();

                    gridAttachment.Rebind();
                }
            }
        }
    }
}
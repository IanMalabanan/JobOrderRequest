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
    public partial class Assessment : System.Web.UI.Page
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
            , nextApprover = string.Empty;

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

            int dbCount = 0;              
            
            if (incharge == "01" || incharge == "02" || incharge == "03")
            {
                //rdoInCharge.SelectedValue = "07";
                dbCount = dbContext.tblInchargePersons.Where(x => x.EmpID == id && x.IsAssesstor == true && x.Incharge_ID == "07").Count();
            }
            else if (incharge == "04" || incharge == "05" || incharge == "06")
            {
                //rdoInCharge.SelectedValue = "08";
                dbCount = dbContext.tblInchargePersons.Where(x => x.EmpID == id && x.IsAssesstor == true && x.Incharge_ID == "08").Count();
            }
            else
            {
                //rdoInCharge.SelectedValue = incharge;
                dbCount = dbContext.tblInchargePersons.Where(x => x.EmpID == id && x.IsAssesstor == true && x.Incharge_ID == incharge).Count();
            }

            //if (dbCount == 1)
            //{
            //    divDetails.Visible = true;

            //    divRecords.Visible = false;
            //}
            //else 
            if (dbCount >= 1)
            {
                divDetails.Visible = true;

                divRecords.Visible = false;

                chkEditInCharged.Checked = false;

                btnUpdateAssestor.Visible = false;
            }
            else
            {
                divDetails.Visible = false;

                divRecords.Visible = true;

                gridRecords.Rebind();

                Show("You are not authorize to assess this request.");
            }
        }

        protected void ddlAssessment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlAssessment.SelectedValue.ToString().Trim() == "CP")
            {
                //colAssessRemarks.Visible = true;
                tbAssessmentRemarks.Text = string.Empty;
            }
            else
            {
                //colAssessRemarks.Visible = false;
                tbAssessmentRemarks.Text = string.Empty;
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
                         Checker = data.Checker,
                         AssessBy = data.AssessedBy,
                         Noter = data.Noter,
                         Manager = data.Manager,
                         AcceptBy = data.FinalAcceptanceBy,
                         IsCancelled = data.IsCancelled,
                         IsRejected = data.IsRejected,
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
                         DateReceived = data.DateAccepted
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

                Checker.InnerText = row[14].ToString().Trim();

                Assesstor.InnerText = row[15].ToString().Trim();

                Noter.InnerText = row[16].ToString().Trim();

                Approver.InnerText = row[17].ToString().Trim();

                Receiver.InnerText = row[18].ToString().Trim();

                lblPreparerRemarks.InnerText = row[21].ToString().Trim();

                lblCheckerRemarks.InnerText = row[22].ToString().Trim();

                lblAssesstorRemarks.InnerText = row[23].ToString().Trim();

                lblNoterRemarks.InnerText = row[24].ToString().Trim();

                lblApproverRemarks.InnerText = row[25].ToString().Trim();

                lblReceiverRemarks.InnerText = row[26].ToString().Trim();

                lblDateSigned_Preparer.InnerText = Convert.ToDateTime(row[0]).ToString("yyyy-MMM-dd");

                lblDateSigned_Checker.InnerText = Convert.ToDateTime(row[27]).ToString("yyyy-MMM-dd");

                lblDateSigned_Assesstor.InnerText = string.IsNullOrEmpty(row[28].ToString()) ? string.Empty : Convert.ToDateTime(row[28]).ToString("yyyy-MMM-dd");

                lblDateSigned_Noter.InnerText = string.IsNullOrEmpty(row[29].ToString()) ? string.Empty : Convert.ToDateTime(row[29]).ToString("yyyy-MMM-dd");

                lblDateSigned_Approver.InnerText = string.IsNullOrEmpty(row[30].ToString()) ? string.Empty : Convert.ToDateTime(row[30]).ToString("yyyy-MMM-dd");

                lblDateSigned_Receiver.InnerText = string.IsNullOrEmpty(row[31].ToString()) ? string.Empty : Convert.ToDateTime(row[31]).ToString("yyyy-MMM-dd");
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

            //rdoInCharge.SelectedValue = incharge;

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

            if (rdoInCharge.SelectedValue == "04" || rdoInCharge.SelectedValue == "05" || rdoInCharge.SelectedValue == "08")
            {
                rowJigs.Visible = true;
                rowFacilities.Visible = false;
                ddlCheckJig.Enabled = true;
            }
            else
            {
                rowJigs.Visible = false;
                rowFacilities.Visible = true;
                ddlCheckJig.Enabled = false;
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

            //SQLDSGetAttachment2.ConnectionString = ClsConfig.JobRequestConnectionString;

            getRecords.ConnectionString = ClsConfig.JobRequestConnectionString;

            if (!IsPostBack)
            {
                rdoInCharge.DataSource = dbContext.tblIncharges.Where(x => x.Incharge_ID == "07" || x.Incharge_ID == "08").ToList();
                rdoInCharge.DataBind();

                rdoReqType.DataSource = dbContext.tblRequestTypes.ToList();
                rdoReqType.DataBind();

                ddlAssessment.DataSource = dbContext.tblAssessments.OrderBy(x => x.ID).ToList();
                ddlAssessment.DataBind();
            }
        }

        private void Save(string appr)
        {
            if ((rdoInCharge.SelectedValue == "04" || rdoInCharge.SelectedValue == "05" || rdoInCharge.SelectedValue == "08")
                && (ddlCheckJig.SelectedIndex == -1 || ddlCheckJig.SelectedIndex == 0))
            {
                Show("Select if jig is exists.");
            }
            else
            {
                string link = string.Empty, body = string.Empty;

                link = Environment.NewLine + "http://" + HttpContext.Current.Request.Url.Authority + "/"
                         + ConfigurationManager.AppSettings["NoterPage"];

                //var app = dbContext.tblRequests.Where(x => x.JR_Code == jrcode).FirstOrDefault();

                //if (app != null)
                //{
                //    app.Is_Assessed = true;

                //    app.AssessedBy = Session["FullName_LnameFirst"].ToString();

                //    app.AssessedRemarks = "SIGNED";

                //    app.DateAssessed = DateTime.Now.Date;

                //    app.Noter = appr;//nextApprover.Trim();

                //    app.IsHold = false;

                //    app.AssessmentCode = ddlAssessment.SelectedValue.ToString().Trim();

                //    if (ddlAssessment.SelectedValue.ToString().Trim() == "CP")
                //    {
                //        app.AssessmentRemarks = tbAssessmentRemarks.Text.Trim();

                //        app.IsCancelled = true;
                //    }
                //    else
                //    {
                //        app.AssessmentRemarks = "N/A";

                //        app.IsCancelled = false;
                //    }

                //    dbContext.Entry(app).State = EntityState.Modified;

                //    //dbContext.SaveChanges();
                //}

                SqlCommand cmd = new SqlCommand("update tblRequest set " +
                       "Is_Assessed=@Is_Assessed, " +
                       "AssessedBy=@AssessedBy, " +
                       "AssessedRemarks =@AssessedRemarks," +
                       "DateAssessed=@Date, " +
                       "Noter=@Noter, " +
                       "IsHold=@IsHold," +
                       "IsExistingJig=convert(bit, @IsExistingJig)," +
                       "AssessmentRemarks=@AssessmentRemarks, " +
                       "IsCancelled=@IsCancelled ," +
                       "AssessmentCode = @AssessmentCode " +
                       "where JR_Code=@JR_Code");

                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("@AssessedBy", Session["FullName_LnameFirst"].ToString().Trim());
                cmd.Parameters.AddWithValue("@Noter", appr);

                if (ddlAssessment.SelectedValue.ToString().Trim() == "CP")
                {
                    cmd.Parameters.AddWithValue("@AssessedRemarks", "REJECTED");

                    cmd.Parameters.AddWithValue("@IsCancelled", true);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@AssessedRemarks", "SIGNED");

                    cmd.Parameters.AddWithValue("@IsCancelled", false);
                }

                if (tbAssessmentRemarks.Text.Length == 0)
                {
                    cmd.Parameters.AddWithValue("@AssessmentRemarks", "N/A");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@AssessmentRemarks", tbAssessmentRemarks.Text.Trim());
                }

                if (rdoInCharge.SelectedValue == "04" || rdoInCharge.SelectedValue == "05" || rdoInCharge.SelectedValue == "08")
                {
                    cmd.Parameters.AddWithValue("@IsExistingJig", string.IsNullOrEmpty(ddlCheckJig.SelectedValue.ToString().Trim()) ? DBNull.Value.ToString() : ddlCheckJig.SelectedValue.ToString().Trim());

                }
                else
                {
                    cmd.Parameters.AddWithValue("@IsExistingJig", DBNull.Value);
                }

                cmd.Parameters.AddWithValue("@JR_Code", jrcode);
                cmd.Parameters.AddWithValue("@Date", DateTime.Now.Date);
                cmd.Parameters.AddWithValue("@IsHold", false);
                cmd.Parameters.AddWithValue("@Is_Assessed", true);
                cmd.Parameters.AddWithValue("@AssessmentCode", ddlAssessment.SelectedValue.ToString().Trim());

                SqlHelper.ExecuteNonQuery(ClsConfig.JobRequestConnectionString, cmd);

                if (ddlAssessment.SelectedValue.ToString().Trim() != "CP")
                {
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
                        Boolean ress;

                        ress = SaveEmailLogs(link + "?EmpID=" + dr["EmpID"].ToString().Trim() + "&RCode=" + Session["RCode"].ToString().Trim(), "Noter", dr["EmailAdd"].ToString().Trim(), Session["FullName_LnameFirst"].ToString().Trim()
                            , dr["EmpID"].ToString().Trim(), Session["RCode"].ToString().Trim());

                        body = "Dear Sir/Ma'am," + "<br /><br />" + "Good Day!";

                        body += "<br /><br />" + "I Have Prepared A Job Order Request For Your Checking and Approval.";

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
                        //}

                        dbContext.SaveChanges();




                    }

                    if (tbAssessmentRemarks.Text.Length == 0)
                    {
                        SendToRequestor();
                    }
                    else
                    {
                        SendRemarksToRequestor();
                    }
                }
            }
        }

        protected void Approve(object sender, EventArgs e)
        {
            string link = string.Empty, body = string.Empty;

            link = Environment.NewLine + "http://" + HttpContext.Current.Request.Url.Authority + "/"
                     + ConfigurationManager.AppSettings["NoterPage"];

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

            var asstr2 = (from data in dbContext.tblUsers
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

            string appempid = string.Empty;

            foreach (DataRow dr in ConvertToDataTable(asstr2).Rows)
            {
                appempid = dr["EmpID"].ToString().Trim();
            }

            var dbuser = dbHRISContext.SKPI_GetAllEmployeesByEmpID(appempid.Trim()).ToList();

            if (ConvertToDataTable(dbuser).Rows.Count > 0)
            {
                foreach (DataRow row in ConvertToDataTable(dbuser).Rows)
                {
                    nextApprover = row["FullName_FnameFirst"].ToString().Trim();
                }
            }

            Save(nextApprover);

            Show("Record Successfully Assessed");

            gridRecords.Rebind();

            divDetails.Visible = false;

            divRecords.Visible = true;

            jrcode = string.Empty;

            Session["RCode"] = null;
            //}
        }

        //private void SendRemarksToAssesstor()
        //{
        //    string body = string.Empty;

        //    body = "Dear Sir/Ma'am," + "<br /><br />" + "Good Day!";

        //    body += "<br /><br />" + "Requestor has a concern on the finsihed job order.";

        //    body += "<br /><br />" + "Job Order No: " + tbJobOrderNo.Text.Trim();

        //    body += "<br /><br />" + "Concern: " + tbEmpRemarks.Text.Trim();

        //    body += "<br /><br />" + "Thank You.";

        //    body += "<br /><br />" + "Note: This is a system generated email. Please do not reply. Thank you";

        //    var asstr = (from data in dbContext.tblUsers
        //                 join inchargePerson in dbContext.tblInchargePersons on data.EmpID equals inchargePerson.EmpID
        //                 into UP
        //                 from inchargePerson in UP.DefaultIfEmpty()
        //                 where inchargePerson.Incharge_ID == rdoInCharge.SelectedValue && inchargePerson.IsAssesstor == true
        //                 select new
        //                 {
        //                     EmpID = data.EmpID,
        //                     EmailAdd = data.EmailAdd,
        //                     InchargeCode = inchargePerson.Incharge_ID,
        //                     IsAssesstor = inchargePerson.IsAssesstor,
        //                     IsNoter = inchargePerson.IsNoter
        //                 }
        //             ).ToList();

        //    foreach (DataRow dr in ConvertToDataTable(asstr).Rows)
        //    {
        //        using (MailMessage mm = new MailMessage())
        //        {
        //            string sub = "Online Job Order Request: Accepted By Requestor";

        //            mm.Subject = sub.ToUpper();

        //            mm.Body = body;

        //            mm.From = new MailAddress(ConfigurationManager.AppSettings["MailSenderEmailAddress"].ToString(),

        //                ConfigurationManager.AppSettings["MailSenderName"].ToString());

        //            mm.To.Add(dr["EmailAdd"].ToString().Trim());

        //            SmtpClient smtp = new SmtpClient();

        //            smtp.Host = ConfigurationManager.AppSettings["MailServer"].ToString();

        //            smtp.EnableSsl = true;

        //            mm.IsBodyHtml = true;

        //            NetworkCredential NetworkCred = new NetworkCredential(ConfigurationManager.AppSettings["MailSenderEmailAddress"].ToString(),
        //                ConfigurationManager.AppSettings["MailSenderEmailPassword"].ToString());

        //            smtp.Credentials = NetworkCred;

        //            smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);

        //            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate,
        //                                                                        X509Chain chain, SslPolicyErrors sslPolicyErrors)
        //            { return true; };

        //            smtp.Send(mm);

        //        }
        //    }
        //}

        private void SendRemarksToRequestor()
        {
            string body = string.Empty;

            body = "Dear Sir/Ma'am," + "<br /><br />" + "Good Day!";

            body += "<br /><br />" + "Please be informed that your job order request has been assessed but the assesstor has some remarks that you need to consider.";

            body += "<br /><br />" + "Problem: " + tbjustification.Text.Trim();

            body += "<br /><br />" + "Concern: " + tbAssessmentRemarks.Text.Trim();

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

        private void SendToRequestor()
        {
            string body = string.Empty;

            body = "Dear Sir/Ma'am," + "<br /><br />" + "Good Day!";

            body += "<br /><br />" + "Please be informed that your job order request has been assessed.";

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

        //protected void UploadMultipleFileExisting(object sender, EventArgs e)
        //{
        //    if (uploadFile.UploadedFiles.Count == 0)
        //    {
        //        Show("Select File First");
        //    }
        //    else
        //    {
        //        //Convert File To Base64String Before Saving to Database

        //        string fileext = string.Empty;

        //        foreach (UploadedFile file in uploadFile.UploadedFiles)
        //        {
        //            byte[] bytes = new byte[file.ContentLength];

        //            file.InputStream.Read(bytes, 0, bytes.Length);

        //            filename = file.GetName().Replace(",", "_").Replace(" ", "_");

        //            //filetype = file.ContentType;

        //            fileext = Path.GetExtension(filename);

        //            if (fileext == ".pdf")
        //            {
        //                filetype = "application/pdf";
        //            }
        //            else if (fileext == ".xls")
        //            {
        //                filetype = "application/vnd.ms-excel";
        //            }
        //            else if (fileext == ".xlsx")
        //            {
        //                filetype = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //            }
        //            else if (fileext.ToLower() == ".pptx")
        //            {
        //                filetype = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
        //            }
        //            else if (fileext.ToLower() == ".ppt")
        //            {
        //                filetype = "application/vnd.ms-powerpoint";
        //            }
        //            else if (fileext.ToLower() == ".docx")
        //            {
        //                filetype = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
        //            }
        //            else if (fileext.ToLower() == ".doc")
        //            {
        //                filetype = "application/msword";
        //            }
        //            else if (fileext.ToLower() == ".jpeg" || fileext == ".jpg")
        //            {
        //                filetype = "image/jpeg";
        //            }

        //            else if (fileext.ToLower() == ".png")
        //            {
        //                filetype = "image/png";
        //            }

        //            Response.Charset = string.Empty;

        //            tblAttachment tbl = new tblAttachment()
        //            {
        //                attachmentfile = Convert.ToBase64String(bytes, 0, bytes.Length),
        //                attachmentname = filename,
        //                contenttype = filetype,
        //                jr_code = Session["RCode"].ToString().Trim(),
        //                typeofattachment = "Admin",
        //                issubmitted = true,
        //                datecreated = DateTime.Now
        //            };

        //            dbContext.tblAttachments.Add(tbl);

        //            dbContext.SaveChanges();

        //            //gridAttachment2.Rebind();
        //        }
        //    }
        //}

        protected void rdoInCharge_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chkEditInCharged.Checked == true)
            {
                incharge = rdoInCharge.SelectedValue.ToString().Trim();

            }
            else
            {
                GetRequestDetails();
            }
        }

        protected void chkEditInCharged_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEditInCharged.Checked == true)
            {
                btnUpdateAssestor.Visible = true;
                rdoInCharge.Enabled = true;

            }
            else
            {
                btnUpdateAssestor.Visible = false;

                rdoInCharge.Enabled = false;

                GetRequestDetails();
            }
        }

        protected void ChangeIncharge(object sender, EventArgs e)
        {
            string link = string.Empty, body = string.Empty;

            dbContext = new JobRequestDBEntities();

            var a = dbContext.tblRequests.Where(x => x.JR_Code == jrcode).FirstOrDefault();

            if (a != null)
            {
                tblRequest tbldata = a as tblRequest;

                tbldata.Incharge_ID = incharge;

                dbContext.Entry(tbldata).State = EntityState.Modified;

                dbContext.SaveChanges();
            }

            link = Environment.NewLine + "http://" + HttpContext.Current.Request.Url.Authority + "/DirectAssessment.aspx";

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
                Boolean ress;

                ress = SaveEmailLogs(link + "?EmpID=" + dr["EmpID"].ToString().Trim() + "&RCode=" + jrcode
                    , "Assesstor", dr["EmailAdd"].ToString().Trim(), approverfullname_fnamefirst.ToString().Trim()
                    , dr["EmpID"].ToString().Trim(), Session["RCode"].ToString().Trim());

                body = "Dear Sir/Ma'am," + "<br /><br />" + "Good Day!";

                body += "<br /><br />" + "I Have Prepared A Job Order Request For Your Assessment.";

                body += "<br /><br />" + "You may click the link below to redirect to main page. ";

                body += "<br /><br />" + link + "?EmpID=" + dr["EmpID"].ToString().Trim() + "&RCode=" + jrcode;

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

            dbContext.SaveChanges();

            SendToRequestor_Transferred();

            Show("Record Successfully Transferred");

            divRecords.Visible = true;

            gridRecords.Rebind();
        }

        private void SendToRequestor_Transferred()
        {
            string body = string.Empty;

            body = "Dear Sir/Ma'am," + "<br /><br />" + "Good Day!";

            body += "<br /><br />" + "Please be informed that your job order request is now on the assessment of " + rdoInCharge.SelectedItem.Text;

            body += "<br /><br />" + "Thank You.";

            body += "<br /><br />" + "Note: This is a system generated email. Please do not reply. Thank you";


            using (MailMessage mm = new MailMessage())
            {
                string sub = "Online Job Order Request: In-Charge Transfer";

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
    }
}
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
using System.ComponentModel.DataAnnotations;

namespace NewJobRequestSystem
{
    public partial class RegisterEmployee : System.Web.UI.Page
    {
        JobRequestDBEntities dbContext = new JobRequestDBEntities();

        ClsGenerateRandomString random = new ClsGenerateRandomString();

        public static string filename = string.Empty, filetype = string.Empty;

        public bool IsValidEmail(string source)
        {
            return new EmailAddressAttribute().IsValid(source);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Session["EmpID"] = "230695-12534";

            //Session["Position"] = "Sr. IT Officer";
            
            SQLDSGetAllApprover.ConnectionString = ClsConfig.JobRequestConnectionString;

            if (Session["EmpID"] != null)
            {
                tbEmpID.Text = Session["EmpID"].ToString().Trim();

                tbEmailAddress.Focus();
            }
            else
            {
                Response.Redirect("UserLogin.aspx");
            }
        }

        private void Register()
        {
            if (tbEmailAddress.Text.Length == 0 || !IsValidEmail(tbEmailAddress.Text.Trim()))
            {
                ShowMessage("Email address is a must.");
            }
            //else if (tbSuperiorEmailAdd.Text.Length == 0 || !IsValidEmail(tbSuperiorEmailAdd.Text.Trim()))
            //{
            //    ShowMessage("Email address is a must.");
            //}
            else
            {
                string saveUser = "If not exists(select * from dbo.tblUsers where EmpID = @EmpID) " +
                    "begin " +
                    "Insert into dbo.tblUser(EmpID,UserRole,EmailAdd)" +
                    "values(@EmpID,@Usertype,@EmailAdd) " +
                    "delete from dbo.tblLeaderSubordinates where EmpID = @EmpID " +
                    "Insert into dbo.tblLeaderSubordinates(EmpID,LeaderEmpID)values(@EmpID,@SuperiorEmpID)" +
                    "end "
                    ;

                SqlCommand cmdUser = new SqlCommand(saveUser);

                cmdUser.CommandType = CommandType.Text;

                cmdUser.Parameters.AddWithValue("@EmpID", tbEmpID.Text.Trim());

                Regex r = new Regex("Manager");
                bool containsManager = r.IsMatch(Session["Position"].ToString().TrimEnd());

                Regex r2 = new Regex("Supervisor");
                bool containsSupervisor = r2.IsMatch(Session["Position"].ToString().TrimEnd());

                Regex r1 = new Regex("Head");
                bool containsHead = r1.IsMatch(Session["Position"].ToString().TrimEnd());

                Regex r3 = new Regex("Employee");
                bool containsEmployee = r1.IsMatch(Session["Position"].ToString().TrimEnd());


                if (containsManager == true && containsSupervisor == false && containsHead == false && containsEmployee == false)
                {
                    cmdUser.Parameters.AddWithValue("@Usertype", "Manager");
                }
                else if (containsManager == false && containsSupervisor == true && containsHead == false && containsEmployee == false)
                {
                    cmdUser.Parameters.AddWithValue("@Usertype", "Supervisor");
                }
                else if (containsManager == false && containsSupervisor == false && containsHead == true && containsEmployee == false)
                {
                    cmdUser.Parameters.AddWithValue("@Usertype", "Head");
                }
                else
                {
                    cmdUser.Parameters.AddWithValue("@Usertype", "Employee");
                }

                cmdUser.Parameters.AddWithValue("@Email", tbEmailAddress.Text.Trim());

                cmdUser.Parameters.AddWithValue("@SuperiorEmpID", RadDropDownApprovers.SelectedValue.ToString().Trim());

                SqlHelper.ExecuteNonQuery(ClsConfig.JobRequestConnectionString, cmdUser);

                ShowMessage("Employee Successfully Registered");

                Session["EmpID"] = null;

                Response.Redirect("UserLogin.aspx");
            }
        }

        protected void ShowMessage(string mess)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "Message", "alert('" + mess + "'); ", true);
        }

        protected void btnsignin_Click(object sender, EventArgs e)
        {
            Register();
        }
    }
}
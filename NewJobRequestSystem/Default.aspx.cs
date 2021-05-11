using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.ComponentModel;

namespace NewJobRequestSystem
{
    public partial class Default : System.Web.UI.Page
    {
        JobRequestDBEntities dbContext = new JobRequestDBEntities();

        HRISEntities dbHRISContext = new HRISEntities();

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

        protected void ShowMessage(string mess)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "Message", "alert('" + mess + "'); ", true);
        }

        protected void Login(object sender, EventArgs e)
        {
            var getchecker = (from data in dbContext.tblUsers
                              join incharge in dbContext.tblInchargePersons on data.EmpID equals incharge.EmpID
                              into UP
                              from incharge in UP.DefaultIfEmpty()
                              where data.EmpID.Trim() == tbEmpID.Text.Trim()
                              select new
                              {
                                  EmpID = data.EmpID,
                                  EmailAdd = data.EmailAdd,
                                  UserRole = data.UserRole,
                                  InchargeCode = incharge.Incharge_ID
                              }
                     ).ToList();

            if (ConvertToDataTable(getchecker).Rows.Count == 0)
            {
                var dbuser = dbHRISContext.SKPI_GetAllEmployeesByEmpID(tbEmpID.Text.Trim()).ToList();
                
                if(ConvertToDataTable(dbuser).Rows.Count > 0)
                {
                    foreach (DataRow row in ConvertToDataTable(dbuser).Rows)
                    {
                        Session["Position"] = row["Pos_Desc"].ToString();
                    }

                    Session["EmpID"] = tbEmpID.Text.Trim();

                    Response.Redirect("RegisterEmployee.aspx");
                }
                else
                {
                    ShowMessage("Invalid Employee ID");
                }                
            }
            else
            {
                foreach (DataRow row in ConvertToDataTable(getchecker).Rows)
                {
                    Session["EmpID"] = row[0].ToString();

                    Session["UserEmail"] = row[1].ToString();

                    Session["UserRole"] = row[2].ToString();

                    Session["InchargeID"] = row[3].ToString();
                }

                var dbuser = dbHRISContext.SKPI_GetAllEmployeesByEmpID(tbEmpID.Text.Trim()).ToList();

                foreach (DataRow row in ConvertToDataTable(dbuser).Rows)
                {
                    Session["Position"] = row["Pos_Desc"].ToString();

                    Session["DeptCode"] = row["Dept_Code"].ToString();

                    Session["DeptName"] = row["Dept_Desc"].ToString();

                    Session["SectCode"] = row["Sect_Code"].ToString();

                    Session["SectName"] = row["Sect_Desc"].ToString();

                    Session["FName"] = row["FirstName"].ToString();

                    Session["FullName_LnameFirst"] = row["FullName_LnameFirst"].ToString();

                    Session["FullName_FnameFirst"] = row["FullName_FnameFirst"].ToString();
                }

                if(Session["InchargeID"] != null && !string.IsNullOrEmpty(Session["InchargeID"].ToString().Trim()))
                {
                    Response.Redirect("AdminHome.aspx");
                }
                else
                {
                    Response.Redirect("HomePage.aspx");
                }                
            }
        }

        protected void Login3(object sender, EventArgs e)
        {
            DataTable dt1, dt2;

            SqlCommand cmd = new SqlCommand("Select * from tblUsers where EmpID=@EmpID");

            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("@EmpID", tbEmpID.Text.Trim());

            dt1 = SqlHelper.ExecuteDataReader(ClsConfig.JobRequestConnectionString, cmd);

            if (dt1.Rows.Count == 0)
            {
                //RadWindowManager1.RadAlert("No Records Found!", null, null, "Notification", null);

                SqlCommand cmd1 = new SqlCommand("SKPI_GetAllEmployeesByEmpID");

                cmd1.CommandType = System.Data.CommandType.StoredProcedure;

                cmd1.Parameters.AddWithValue("@empid", tbEmpID.Text.Trim());

                dt2 = SqlHelper.ExecuteDataReader(ClsConfig.PISConnectionString, cmd1);

                foreach (DataRow row in dt2.Rows)
                {
                    Session["Position"] = row["Pos_Desc"].ToString();
                }

                Session["EmpID"] = tbEmpID.Text.Trim();

                //Response.Redirect("RegisterEmployee.aspx");

            }
            else
            {
                foreach (DataRow dr in dt1.Rows)
                {
                    //Session["UserRole"] = dr["UserType"].ToString();

                    Session["EmpID"] = dr["EmpID"].ToString();

                    Session["UserEmail"] = dr["EmailAdd"].ToString();
                }

                SqlCommand cmd1 = new SqlCommand("SKPI_GetAllEmployeesByEmpID");

                cmd1.CommandType = System.Data.CommandType.StoredProcedure;

                cmd1.Parameters.AddWithValue("@empid", Session["EmpID"].ToString().Trim());

                dt2 = SqlHelper.ExecuteDataReader(ClsConfig.PISConnectionString, cmd1);

                foreach (DataRow row in dt2.Rows)
                {
                    Session["Position"] = row["Pos_Desc"].ToString();

                    Session["DeptCode"] = row["Dept_Code"].ToString();

                    Session["DeptName"] = row["Dept_Desc"].ToString();

                    Session["SectCode"] = row["Sect_Code"].ToString();

                    Session["SectName"] = row["Sect_Desc"].ToString();

                    Session["FName"] = row["FirstName"].ToString();

                    Session["FullName_LnameFirst"] = row["FullName_LnameFirst"].ToString();

                    Session["FullName_FnameFirst"] = row["FullName_FnameFirst"].ToString();
                }

                Response.Redirect("HomePage.aspx");
            }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRMS1
{
    public partial class Login : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["username"] = "";
           
        }
        protected void Button1_Click(object sender, EventArgs e)
        {

            var Uname = Request.Form.Get("name").Trim();
            var data = (from a in db.Tbl_UserRegistartions where a.username.Trim() == Request.Form.Get("name").Trim() && a.Password.Trim() == Request.Form.Get("pasdsword").Trim() select a).SingleOrDefault();
            if (data == null)
            {
                var emp = (from a in db.Tbl_EmployeeRegistrations where a.UserName.Trim() == Request.Form.Get("name").Trim() && a.Password.Trim() == Request.Form.Get("pasdsword").Trim() select a).SingleOrDefault();
                if (emp == null)
                {
                    Response.Write("Login Failed");

                }
                else
                {
                    Session["username"] = Uname;
                    Session["usertype"] = "Employee";
                    Session["empid"] = emp.Emp_id.ToString();
                    Response.Redirect("/Home/DashBoard");
                }

            }
            else
            {
                var admin = (from b in db.Tbl_UserRegistartions where b.username == Request.Form.Get("name").Trim() select b.UserID).SingleOrDefault();
                if (admin != 0)
                {
                    Session["username"] = Uname;
                    Session["usertype"] = "Admin";
                    Session["empid"] = admin.ToString();
                    Response.Redirect("/Home/DashBoard");
                }
                else
                {
                    Response.Write("Login Failed");
                }
            }
           
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Linq;
using System.Drawing;

namespace WebApplication2.Login
{
    public partial class Login : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["username"] = "";
            if (IsPostBack)
            {
                Label1.Visible = false;
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
           
                var data = (from a in db.Tbl_Users where a.username.Trim() == Request.Form.Get("name").Trim() && a.Password.Trim() == Request.Form.Get("pasdsword").Trim() select a).SingleOrDefault();
            var Uname = Request.Form.Get("name").Trim();
                if (data == null)
                {

                Label1.ForeColor = Color.Red;
                Label1.Visible = true;
                Label1.Text = "Login Failed";
            }
                else
                {
                Label1.ForeColor = Color.Red;
                Label1.Visible = true;
                Label1.Text = "Success";
                Session["username"] = Uname;
                Response.Redirect("Home/DashBoard");
           
                
            }
            }
           
        }
    }

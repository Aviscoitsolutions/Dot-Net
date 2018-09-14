using ExampleService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace avtrackRegistration
{
    public partial class FrmForgotPassword : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        public FrmForgotPassword()
        {
            InitializeComponent();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
if(txtUsername.Text=="")
            {
                MessageBox.Show("Please Enter User Name");
                txtUsername.Focus();
            }
else if(txtpassword.Text=="")
            {
                MessageBox.Show("Please Enter New Password");
                txtpassword.Focus();
            }
else
            {
                var data = (from a in db.Tbl_Users where a.username == txtUsername.Text.Trim() select a).ToList();
                if (data.Any())
                {
                    Tbl_User t = db.Tbl_Users.Single(u => u.username.Trim() == txtUsername.Text.Trim());
                    t.Password = txtpassword.Text.Trim();
                    db.SubmitChanges();
                    MessageBox.Show("Password Changed Succesfully");
                    txtpassword.Text = "";
                    txtUsername.Text = "";
                    txtUsername.Focus();
                }
                else
                { 
                        MessageBox.Show("User Not Found!!");
                txtUsername.Focus();
            }
                
            }

           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

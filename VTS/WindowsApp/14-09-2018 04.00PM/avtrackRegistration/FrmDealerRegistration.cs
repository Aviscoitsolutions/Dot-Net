using ExampleService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace avtrackRegistration
{
    public partial class FrmDealerRegistration : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        public FrmDealerRegistration()
        {
            InitializeComponent();
        }

        private void FrmDealerRegistration_Load(object sender, EventArgs e)
        {

        }
        public void clear()
        {
            txtName.Text = "";
            Txtmobile.Text = "";
            TxtPassword.Text = "";
            TxtUserName.Text = "";
            txtcompanyname.Text = "";
            txtemail.Text = "";
            txtName.Focus();
        }
        public int MaxId()
        {
            try
            {
                var id = Convert.ToInt32((from a in db.Tbl_DealerRegistrations select a.Dealer_Id).Max());
                return id + 1;


            }
            catch (Exception)
            {
                return 1;
            }
        }

        private void Txtmobile_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
               && !char.IsDigit(e.KeyChar)
               && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

      
        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
                e.Handled = true;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var data1 = (from a in db.Tbl_DealerRegistrations where a.UserName == TxtUserName.Text select a);
                var checkname = (from a in db.Tbl_DealerRegistrations where a.DealerName ==txtName.Text select a);

                if (txtName.Text == "")
                {
                    MessageBox.Show("Please Enter Name");
                    txtName.Focus();
                }
                else if (txtcompanyname.Text == "")
                {
                    MessageBox.Show("Please Enter Company Name");
                    txtcompanyname.Focus();
                }
                else if (Txtmobile.Text == "")
                {
                    MessageBox.Show("Please Enter Mobile No");
                    Txtmobile.Focus();
                }
                else if (Txtmobile.Text.Length < 10)
                {
                    MessageBox.Show("Please Enter Correct Contact Number");
                    Txtmobile.Focus();

                }
                else if (txtemail.Text == "")
                {
                    MessageBox.Show("Please Enter Email-Address");
                    txtemail.Focus();
                }
                else if (TxtUserName.Text == "")
                {
                    MessageBox.Show("Please Enter UserName");
                    TxtUserName.Focus();
                }
                else if (TxtPassword.Text == "")
                {
                    MessageBox.Show("Please Enter Password");
                    TxtPassword.Focus();
                }
                else if (checkname.Any())
                {
                    MessageBox.Show("Dealer Already Exist!!");
                    txtName.Focus();
                }

                else if (data1.Any())
                {
                    MessageBox.Show("User Name Already Exist!!");
                    TxtUserName.Focus();
                }

                else
                {
                    string email = txtemail.Text;
                    Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                    Match match = regex.Match(email);
                    if (match.Success)
                    {
                        Tbl_DealerRegistration t = new Tbl_DealerRegistration();
                        t.Dealer_Id = MaxId();
                        t.DealerName = txtName.Text.Trim();
                        t.Company_Name = txtcompanyname.Text.Trim();
                        t.Email_ID = txtemail.Text.Trim();
                        t.UserName = TxtUserName.Text.Trim();
                        t.Password = TxtPassword.Text.Trim();
                        db.Tbl_DealerRegistrations.InsertOnSubmit(t);
                        db.SubmitChanges();
                        MessageBox.Show("Record Save Successfully");
                        clear();
                    }
                    else
                    {
                        MessageBox.Show("Please Enter valid Email-Address");
                        txtemail.Focus();
                        //txtemail.ForeColor = "Red";
                    }


                }
            }
            catch (Exception ex)
            {


            }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

      
    }
}

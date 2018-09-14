using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExampleService;
namespace avtrackRegistration
{
    
    public partial class FrmDeviceRegistration : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
      
        public FrmDeviceRegistration()
        {
            InitializeComponent();
        }
        public void clear()
        {
            txtDeviceName.Text = "";
            cmbusername.Text = "--Select--";
            txtapn.Text = "";
            Txtsimno.Text = "";
            Txtimei.Text = "";
            txtDeviceModel.Text = "";
            txtvehicletype.Text = "";
            cmbdealer.Text = "--Select--";
            cmbusername.Focus();
        }
        private void Cancel_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                

                if (cmbusername.Text == "--Select--"||cmbusername.Text=="")
                {
                    MessageBox.Show("Please Select User Name");
                    cmbusername.Focus();
                }
                else if (txtDeviceName.Text == "")
                {
                    MessageBox.Show("Please Enter Device Name /Vehicle No");
                    txtDeviceName.Focus();
                }
                else if (cmbdealer.Text == "--Select--" || cmbdealer.Text == "")
                {
                    MessageBox.Show("Please Select Dealer Name");
                    cmbdealer.Focus();
                }
                else if (Txtimei.Text == "")
                {
                    MessageBox.Show("Please Enter Device IMEI No");
                    Txtimei.Focus();
                }
               else if (Txtimei.Text.Length < 13)
                {
                    MessageBox.Show("Please Enter Correct IMEI No");
                    Txtimei.Focus();

                }
                else if (txtvehicletype.Text == "")
                {
                    MessageBox.Show("Please Enter Vehicle Type");
                    txtvehicletype.Focus();
                }
                else if (Txtsimno.Text == "")
                {
                    MessageBox.Show("Please Enter Sim No");
                    Txtsimno.Focus();
                }
               else if (Txtsimno.Text.Length < 10)
                {
                    MessageBox.Show("Please Enter Correct Sim No");
                    Txtsimno.Focus();

                }
                else if (txtapn.Text == "")
                {
                    MessageBox.Show("Please APN No");
                    txtapn.Focus();
                }
                else if (txtDeviceModel.Text == "")
                {
                    MessageBox.Show("Please Device Model");
                    txtDeviceModel.Focus();
                }
               
                else
                {
                    var data1 = (from a in db.Tbl_DeviceRegistrations where a.DeviceIMEI == Convert.ToInt64(Txtimei.Text.Trim()) select a).ToList();
                    if (data1.Any())
                    {
                        MessageBox.Show("This IMEI Already Registerd");
                        Txtimei.Focus();
                    }
                    else
                    {
                        Tbl_DeviceRegistration t = new Tbl_DeviceRegistration();
                        t.UserId = Convert.ToInt32(cmbusername.SelectedValue);
                        t.DeviceIMEI = Convert.ToInt64(Txtimei.Text.Trim());
                        t.DeviceName = txtDeviceName.Text.Trim();
                        t.SimNo = Txtsimno.Text.Trim();
                        t.APN = txtapn.Text.Trim();
                        t.Date = DateTime.Now;
                        t.DeviceModel = txtDeviceModel.Text.Trim();
                        t.Vehicle_Type = txtvehicletype.Text.Trim();
                        t.stetus = "1";
                        t.Dealer_ID =Convert.ToInt32(cmbdealer.SelectedValue);
                        db.Tbl_DeviceRegistrations.InsertOnSubmit(t);
                        db.SubmitChanges();
                        MessageBox.Show("Device Added Successfully");
                        clear();
                    }

                }
            }
            catch (Exception)
            {


            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void txtDeviceName_KeyPress(object sender, KeyPressEventArgs e)
        {
          
        }

        private void Txtimei_Validated(object sender, EventArgs e)
        {
            
        }

        private void Txtimei_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                 && !char.IsDigit(e.KeyChar)
                 && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void Txtsimno_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                 && !char.IsDigit(e.KeyChar)
                 && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void Txtsimno_Validated(object sender, EventArgs e)
        {
           
        }

        private void FrmDeviceRegistration_Load(object sender, EventArgs e)
        {
            bindusers();
            binddealer();
           
        }
        public void bindusers()
        {
            var data = (from a in db.Tbl_Users
                        where a.Name != null
                        select new
                        {
                            a.Name,
                            a.UserID
                        }).ToList();
            cmbusername.DisplayMember = "Name";
            cmbusername.ValueMember = "UserId";
            cmbusername.DataSource = data;
        }
        public void binddealer()
        {
            var data1 = (from a in db.Tbl_DealerRegistrations

                         select new
                         {
                             a.DealerName,
                             a.Dealer_Id
                         }).ToList();
            cmbdealer.DisplayMember = "DealerName";
            cmbdealer.ValueMember = "Dealer_Id";
            cmbdealer.DataSource = data1;
        }

      

     

        private void txtxvehicletype_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
                e.Handled = true;
        }
    }
}

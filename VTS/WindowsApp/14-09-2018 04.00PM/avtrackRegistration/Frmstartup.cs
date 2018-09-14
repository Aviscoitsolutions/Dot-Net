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
    public partial class Frmstartup : Form
    {
        public Frmstartup()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmUserRegistration frm = new frmUserRegistration();
            frm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FrmDeviceRegistration frm = new FrmDeviceRegistration();
            frm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FrmForgotPassword frm = new FrmForgotPassword();
            frm.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FrmDealerRegistration frm = new FrmDealerRegistration();
            frm.Show();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRMS1
{
    public partial class billing1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Label1.Text =Session["amount"].ToString();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
         
            string s = Session["amount"].ToString();
            Double amount = Convert.ToDouble(s);

            //String text = key.Value.ToString() + "|" + txnid1.Text + "|" + amount + "|" + "ABC" + "|" + txtname.Text + "|" + txtemail.Text + "|" + "1" + "|" + "1" + "|" + "1" + "|" + "1" + "|" + "1" + "||||||" + salt.Value.ToString();
            ////Response.Write(text);
            //byte[] message = Encoding.UTF8.GetBytes(text);

            //UnicodeEncoding UE = new UnicodeEncoding();
            //byte[] hashValue;
            //SHA512Managed hashString = new SHA512Managed();
            //string hex = "";
            //hashValue = hashString.ComputeHash(message);
            //foreach (byte x in hashValue)
            //{
            //    hex += String.Format("{0:x2}", x);
            //}
            //hash.Value = hex;

            //System.Collections.Hashtable data = new System.Collections.Hashtable(); // adding values in gash table for data post
            //data.Add("hash", hex.ToString());
            //data.Add("txnid", txnid1.Text);
            //data.Add("key", key.Value);
            //// string AmountForm = ;// eliminating trailing zeros
            //data.Add("amount", amount);
            //data.Add("firstname", txtname.Text.Trim());
            //data.Add("email", txtemail.Text.Trim());
            //data.Add("phone", 8624823143);
            //data.Add("productinfo", "ABC");
            //data.Add("udf1", "1");
            //data.Add("udf2", "1");
            //data.Add("udf3", "1");
            //data.Add("udf4", "1");
            //data.Add("udf5", "1");

            //data.Add("surl", "http://localhost:61194/Home/Dashboard");
            //data.Add("furl", "http://localhost:61194/billing1.aspx");
            //string strForm = PreparePOSTForm("https://www.sandbox.paypal.com/cgi-bin/webscr", data);
           // Page.Controls.Add(new LiteralControl(strForm));
        }
        private string PreparePOSTForm(string url, System.Collections.Hashtable data)      // post form
        {
            //Set a name for the form
            string formID = "PostForm";
            //Build the form using the specified data to be posted.
            StringBuilder strForm = new StringBuilder();
            strForm.Append("<form id=\"" + formID + "\" name=\"" +
                           formID + "\" action=\"" + url +
                           "\" method=\"POST\">");

            foreach (System.Collections.DictionaryEntry key in data)
            {

                strForm.Append("<input type=\"hidden\" name=\"" + key.Key +
                               "\" value=\"" + key.Value + "\">");
            }


            strForm.Append("</form>");
            //Build the JavaScript which will do the Posting operation.
            StringBuilder strScript = new StringBuilder();
            strScript.Append("<script language='javascript'>");
            strScript.Append("var v" + formID + " = document." +
                             formID + ";");
            strScript.Append("v" + formID + ".submit();");
            strScript.Append("</script>");
            //Return the form and the script concatenated.
            //(The order is important, Form then JavaScript)
            return strForm.ToString() + strScript.ToString();
        }
    }
    


}
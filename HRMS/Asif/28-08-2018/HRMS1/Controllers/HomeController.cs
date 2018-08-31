
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI;
using HRMS1.Models;
using Microsoft.Reporting.WebForms;
using System.Security.Cryptography;

namespace HRMS1.Controllers
{
    public class HomeController : Controller
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        public string username;
       
        public static string NumberToWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }
        public ActionResult Subscription()
        {
            Session["amount"] = "";
            Session["name"] ="";
            Session["address"] ="";
            Session["city"] ="";
            Session["state"] = "";
            Session["email"] ="";
            var username = (string)Session["username"];
            var usertype = (string)Session["usertype"];
            var empid = (string)Session["empid"];
            if (usertype == "Employee")
            {
                if (username == "" || username == null)
                {
                    Response.Redirect("/Login.aspx");
                }

                ViewBag.username = username.Trim();
                ViewBag.usertype = usertype.Trim();
                ViewBag.empid = empid.Trim();
                return View();
            }
            else
            {
                return Redirect("/Home/DashBoards");
            }
        }
            public ActionResult DashBoard( )
        {
            var username = (string)Session["username"];
            var usertype = (string)Session["usertype"];
            var empid = (string)Session["empid"];
            if (usertype == "Employee")
            {
                if (username == "" || username == null)
                {
                    Response.Redirect("/Login.aspx");
                }

                ViewBag.username = username.Trim();
                ViewBag.usertype = usertype.Trim();
                ViewBag.empid = empid.Trim();
              
                return View();
            }
            else             
            {
                return Redirect("/Home/DashBoards");
            }

        }
        public ActionResult DashBoards()
        {
            var username = (string)Session["username"];
            var usertype = (string)Session["usertype"];
            var empid = (string)Session["empid"];
          
                if (username == "" || username == null)
                {
                    Response.Redirect("/Login.aspx");
                }

                ViewBag.username = username.Trim();
                ViewBag.usertype = usertype.Trim();
                ViewBag.empid = empid.Trim();
            var total = (from a in db.Tbl_EmployeeRegistrations select a).Count();
            var present = (from a in db.Tbl_Attendances where Convert.ToDateTime(a.Date).Date == DateTime.Now.Date select a).Count();
            var report = 100 * present / total;
            ViewBag.present = report;
            var absent = total - present;
            ViewBag.absent = 100*absent/total;
            return View();

            }
            public ActionResult ShiftPlanning()
        {
            return View();
        }
        public class getall
        {
           public string username { get; set; }
           public string password { get; set; }
        }
        public class EmployeeReg
        {
           
        }

        public ActionResult Attendance()
        {
            var username = (string)Session["username"];
            var usertype = (string)Session["usertype"];
            var empid = (string)Session["empid"];

            if (username == "" || username == null)
            {
                Response.Redirect("/Login.aspx");
            }
            
            ViewBag.username = username.Trim();
            ViewBag.usertype = usertype.Trim();
            ViewBag.empid = empid.Trim();
            if (usertype != "Employee")
            {
                return Redirect("/Home/Attendances");
            }
            return View();
        }
        public ActionResult Attendances()
        {
            var username = (string)Session["username"];
            var usertype = (string)Session["usertype"];
            var empid = (string)Session["empid"];

            if (username == "" || username == null)
            {
                Response.Redirect("/Login.aspx");
            }

            ViewBag.username = username.Trim();
            ViewBag.usertype = usertype.Trim();
            ViewBag.empid = empid.Trim();

            return View();
        }
        public ActionResult resetclockout()
        {
            var username = (string)Session["username"];
            var usertype = (string)Session["usertype"];
            var empid = (string)Session["empid"];

            if (username == "" || username == null)
            {
                Response.Redirect("/Login.aspx");
            }

            ViewBag.username = username.Trim();
            ViewBag.usertype = usertype.Trim();
            ViewBag.empid = empid.Trim();

            return View();
        }
        public int attendancemaxid()
        {
            try
            {
                var Id = (from a in db.Tbl_Attendances select a.DetailId).Max();
                return Id= Id + 1;
            }
            catch (Exception)
            {

                return 1;
            }
        }
        public int billingmaxid()
        {
            try
            {
                var Id = (from a in db.Tbl_Billings select a.BillId).Max();
                return Id = Id + 1;
            }
            catch (Exception)
            {

                return 1;
            }
        }
        public ActionResult Leaves()
        {
            var username = (string)Session["username"];
            var usertype = (string)Session["usertype"];
            var empid = (string)Session["empid"];

            if (username == "" || username == null)
            {
                Response.Redirect("/Login.aspx");
            }
            ViewBag.username = username.Trim();
            ViewBag.usertype = usertype.Trim();
            ViewBag.empid = empid.Trim();
            if(usertype!="Employee")
            {
                return Redirect("/Home/leave");
            }
            
            return View();
        }
        public ActionResult PayRoll()
        {
            var username = (string)Session["username"];
            var usertype = (string)Session["usertype"];
            var empid = (string)Session["empid"];

            if (username == "" || username == null)
            {
                Response.Redirect("/Login.aspx");
            }
            ViewBag.username = username.Trim();
            ViewBag.usertype = usertype.Trim();
            ViewBag.empid = empid.Trim();
          

            return View();
        }

      
        public ActionResult Employee( string id)
        {
            var username = (string)Session["username"];
            var usertype = (string)Session["usertype"];
            var empid = (string)Session["empid"];
          
            if (username == "" || username == null)
            {
                Response.Redirect("/Login.aspx");
            }
            ViewBag.username = username.Trim();
            ViewBag.usertype = usertype.Trim();
            ViewBag.empid = empid.Trim();
            ViewBag.symbol = "@";
            try
            {
                var Id = (from a in db.Tbl_EmployeeRegistrations select a.Emp_id).Max();
                ViewBag.maxid =Id + 1;
            }
            catch (Exception)
            {

                ViewBag.maxid = 1;
            }
          
            return View();
        }
        public ActionResult EditEmployee()
        {
            var username = (string)Session["username"];
            var usertype = (string)Session["usertype"];
            var empid = (string)Session["empid"];

            if (username == "" || username == null)
            {
                Response.Redirect("/Login.aspx");
            }
            ViewBag.username = username.Trim();
            ViewBag.usertype = usertype.Trim();
            ViewBag.empid = empid.Trim();
            ViewBag.symbol = "@";
            try
            {
                var Id = (from a in db.Tbl_EmployeeRegistrations select a.Emp_id).Max();
                ViewBag.maxid = Id + 1;
            }
            catch (Exception)
            {

                ViewBag.maxid = 1;
            }
            return View();
        }
        public ActionResult Company_Profile()
        {
            return View();
        }
        public ActionResult Billing( string amount)
        {
           
            var username = (string)Session["username"];
            var usertype = (string)Session["usertype"];
            var empid = (string)Session["empid"];

            if (username == "" || username == null)
            {
                Response.Redirect("/Login.aspx");
            }
            ViewBag.username = username.Trim();
            ViewBag.usertype = usertype.Trim();
            ViewBag.empid = empid.Trim();
            var am = amount.Replace("Rs.", "");
            //Session["amount"] = am;
            //Response.Redirect("/billing1.aspx");
            ViewBag.amount = am;
           
            return View();
        }
        public ActionResult Setting()
        {
            return View();
        }
        public ActionResult mailbox1()
        {
            return View();
        }
        public ActionResult Manage_Timesheet()
        {
            return View();
        }
        public ActionResult Clockedin_list()
        {
            return View();
        }
        public ActionResult success()
        {
          
            var usertype = (string)Session["usertype"];
            var empid = (string)Session["empid"];
            if ((string)Session["amount"] != "")
            {
                Tbl_Billing t = new Tbl_Billing();
                t.BillId = billingmaxid();
                t.Usertype = usertype.Trim();
                t.UserId = Convert.ToInt32(empid);
                t.Amount = (string)Session["amount"];
                t.BillName = (string)Session["name"];
                t.Address = (string)Session["address"];
                t.City = (string)Session["city"];
                t.State = (string)Session["state"];
                t.email = (string)Session["email"];
                t.Date = DateTime.Now;
                db.Tbl_Billings.InsertOnSubmit(t);
                db.SubmitChanges();
                Session["amount"] = "";
                Session["name"] = "";
                Session["address"] = "";
                Session["city"] = "";
                Session["state"] = "";
                Session["email"] = "";
                return View();
            }
            else
            {
                return View();
            }
        }
        public ActionResult Reports()
        {
            var username = (string)Session["username"];
            var usertype = (string)Session["usertype"];
            var empid = (string)Session["empid"];

            if (username == "" || username == null)
            {
                Response.Redirect("/Login.aspx");
            }
            ViewBag.username = username.Trim();
            ViewBag.usertype = usertype.Trim();
            ViewBag.empid = empid.Trim();

            return View();
           
        }
       
        public ActionResult leave()
        {
            var username = (string)Session["username"];
            var usertype = (string)Session["usertype"];
            var empid = (string)Session["empid"];

            if (username == "" || username == null)
            {
                Response.Redirect("/Login.aspx");
            }
            ViewBag.username = username.Trim();
            ViewBag.usertype = usertype.Trim();
            ViewBag.empid = empid.Trim();

            return View();
        }
        public JsonResult verifyuser(getall getall)
        {
            var data = (from a in db.Tbl_UserRegistartions where a.username.Trim() == getall.username.Trim() && a.Password.Trim() == getall.password.Trim() select a).SingleOrDefault();
            //var Uname = Request.Form.Get("name").Trim();
            if (data == null)
            {

                var message = "Failed";
               return Json(message, JsonRequestBehavior.AllowGet);
            }
            else
            {
                ViewBag.username = getall.username;
                Response.Redirect("/Home/DashBoard");
                return Json(null, JsonRequestBehavior.AllowGet);

            }

        }
      
        //public void payform(billing billing)
        //{
        //    //  Double amount = Convert.ToDouble(billing.amount);

        //    String text = billing.key + "|" + billing.txnid + "|" + billing.amount + "|" + "Women Tops" + "|" + "asif" + "|" + "Email1" + "|" + "1" + "|" + "1" + "|" + "1" + "|" + "1" + "|" + "1" + "||||||" + billing.salt;
        //    //  //Response.Write(text);
        //    byte[] message = Encoding.UTF8.GetBytes(text);

        //    UnicodeEncoding UE = new UnicodeEncoding();
        //    byte[] hashValue;
        //    SHA512Managed hashString = new SHA512Managed();
        //    string hex = "";
        //    hashValue = hashString.ComputeHash(message);
        //    foreach (byte x in hashValue)
        //    {
        //        hex += String.Format("{0:x2}", x);
        //    }
        //    //hashValue = hex;

        //    System.Collections.Hashtable data = new System.Collections.Hashtable(); // adding values in gash table for data post
        //    data.Add("hash", hex.ToString());
        //    data.Add("txnid", billing.txnid);
        //    data.Add("key", billing.key);
        //    // string AmountForm = ;// eliminating trailing zeros
        //    data.Add("amount",billing.amount);
        //    data.Add("firstname", "Asif");
        //    data.Add("email", "Email1");
        //    data.Add("phone", "8624823143");
        //    data.Add("productinfo", "Women Tops");
        //    data.Add("udf1", "1");
        //    data.Add("udf2", "1");
        //    data.Add("udf3", "1");
        //    data.Add("udf4", "1");
        //    data.Add("udf5", "1");

        //    data.Add("surl", "http://localhost:50189/SuccessPayment.aspx");
        //    data.Add("furl", "http://localhost:50189/FailurePayment.aspx");
        //     RedirectToRoute("https://sandboxsecure.payu.in/_payment",data);
        //    //Page.Controls.Add(new LiteralControl(strForm));



        //    //RemotePost myremotepost = new RemotePost();
        //    //string key = billing.key;
        //    //string salt = billing.salt;

        //    ////posting all the parameters required for integration.

        //    //myremotepost.Url = "https://sandboxsecure.payu.in/_payment";
        //    //myremotepost.Add("key", "");
        //    //string txnid = "1";
        //    //myremotepost.Add("txnid", txnid);
        //    //myremotepost.Add("amount", billing.amount);
        //    //myremotepost.Add("productinfo", "SSS");
        //    //myremotepost.Add("firstname", "Asif");
        //    //myremotepost.Add("phone", "8624823143");
        //    //myremotepost.Add("email", "nadafasif79@gmail.com");
        //    //myremotepost.Add("surl", "http://localhost:/Home/Dashboard");//Change the success url here depending upon the port number of your local system.
        //    //myremotepost.Add("furl", "http://localhost:/Home/Billing");//Change the failure url here depending upon the port number of your local system.
        //    ////myremotepost.Add("service_provider", "payu_paisa");

        //    //String text1 = billing.key + "|" + billing.txnid + "|" + billing.amount + "|" + "Women Tops" + "|" + "asif" + "|" + "Email1" + "|" + "1" + "|" + "1" + "|" + "1" + "|" + "1" + "|" + "1" + "||||||" + billing.salt;
        //    //string hash = hex;
        //    //myremotepost.Add("hash", hash);
        //    //myremotepost.Post();



        //}
        //private string PreparePOSTForm(string url, System.Collections.Hashtable data)      // post form
        //{
        //    //Set a name for the form
        //    string formID = "PostForm";
        //    //Build the form using the specified data to be posted.
        //    StringBuilder strForm = new StringBuilder();
        //    strForm.Append("<form id=\"" + formID + "\" name=\"" +
        //                   formID + "\" action=\"" + url +
        //                   "\" method=\"POST\">");

        //    foreach (System.Collections.DictionaryEntry key in data)
        //    {

        //        strForm.Append("<input type=\"hidden\" name=\"" + key.Key +
        //                       "\" value=\"" + key.Value + "\">");
        //    }


        //    strForm.Append("</form>");
        //    //Build the JavaScript which will do the Posting operation.
        //    StringBuilder strScript = new StringBuilder();
        //    strScript.Append("<script language='javascript'>");
        //    strScript.Append("var v" + formID + " = document." +
        //                     formID + ";");
        //    strScript.Append("v" + formID + ".submit();");
        //    strScript.Append("</script>");
        //    //Return the form and the script concatenated.
        //    //(The order is important, Form then JavaScript)
        //    return strForm.ToString() + strScript.ToString();
        //}
        //public class RemotePost
        //{
        //    private System.Collections.Specialized.NameValueCollection Inputs = new System.Collections.Specialized.NameValueCollection();


        //    public string Url = "";
        //    public string Method = "post";
        //    public string FormName = "form1";

        //    public void Add(string name, string value)
        //    {
        //        Inputs.Add(name, value);
        //    }

        //    public void Post()
        //    {
        //        System.Web.HttpContext.Current.Response.Clear();

        //        System.Web.HttpContext.Current.Response.Write("<html><head>");

        //        System.Web.HttpContext.Current.Response.Write(string.Format("</head><body onload=\"document.{0}.submit()\">", FormName));
        //        System.Web.HttpContext.Current.Response.Write(string.Format("<form name=\"{0}\" method=\"{1}\" action=\"{2}\" >", FormName, Method, Url));
        //        for (int i = 0; i < Inputs.Keys.Count; i++)
        //        {
        //            System.Web.HttpContext.Current.Response.Write(string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\">", Inputs.Keys[i], Inputs[Inputs.Keys[i]]));
        //        }
        //        System.Web.HttpContext.Current.Response.Write("</form>");
        //        System.Web.HttpContext.Current.Response.Write("</body></html>");

        //        System.Web.HttpContext.Current.Response.End();
        //    }
        //}
        public JsonResult createsessions(billingssession billingssession)
        {
            Session["amount"] = billingssession.amount.Trim();
            Session["name"] = billingssession.name.Trim();
            Session["address"] = billingssession.address.Trim();
            Session["city"] = billingssession.city.Trim();
            Session["state"] = billingssession.state.Trim();
            Session["email"] = billingssession.email.Trim();
            return Json(null, JsonRequestBehavior.AllowGet);
        }
            public JsonResult submitattendance(EmployeeRegistration employee)
        {
            if (employee.button == "clockin")
            {
                Tbl_Attendance t = new Tbl_Attendance();
                t.DetailId = attendancemaxid();
                t.Emp_ID = Convert.ToInt32(employee.EmpId);
                t.ClockIn = DateTime.Now;
                t.Date = DateTime.Now;
                db.Tbl_Attendances.InsertOnSubmit(t);
                db.SubmitChanges();
              
            }
           else if (employee.button == "clockout")
            {
                Tbl_Attendance t = db.Tbl_Attendances.Single(s => s.Emp_ID == Convert.ToInt32(employee.EmpId) && Convert.ToDateTime(s.Date).Date == DateTime.Now.Date);
               
                t.ClockOut = DateTime.Now;
                db.SubmitChanges();
                expire();

            }
            else  if (employee.button == "breakstart")
            {
               
                Tbl_Attendance t = db.Tbl_Attendances.Single(s => s.Emp_ID == Convert.ToInt32(employee.EmpId)&&Convert.ToDateTime( s.Date).Date==DateTime.Now.Date);
                t.breakstart = DateTime.Now;
                t.btnstart = "0";
                t.btnend = "1";
                db.SubmitChanges();
            }
            else if (employee.button == "breakend")
            {

                Tbl_Attendance t = db.Tbl_Attendances.Single(s => s.Emp_ID == Convert.ToInt32(employee.EmpId) && Convert.ToDateTime(s.Date).Date == DateTime.Now.Date);
                t.breakend = DateTime.Now;
                db.SubmitChanges();
                var data = (from a in db.Tbl_Attendances where a.Emp_ID == Convert.ToInt32(employee.EmpId) && Convert.ToDateTime(a.Date).Date == DateTime.Now.Date select a).SingleOrDefault();
                double totalMinutes = (Convert.ToDateTime(data.breakend) - Convert.ToDateTime(data.breakstart)).TotalMinutes;
                Tbl_Attendance t1 = db.Tbl_Attendances.Single(s => s.Emp_ID == Convert.ToInt32(employee.EmpId) && Convert.ToDateTime(s.Date).Date == DateTime.Now.Date);
                int total = Convert.ToInt32(data.totalbreak + totalMinutes);
                t1.totalbreak = total;
                t.btnstart = "1";
                t.btnend = "0";
                db.SubmitChanges();
                return Json(total, JsonRequestBehavior.AllowGet);

            }
            var message = "Done";
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        public JsonResult checkattendance(EmployeeRegistration employee)
        {
            var message = "";
            var data = (from a in db.Tbl_Attendances where a.Emp_ID == Convert.ToInt32(employee.EmpId) &&Convert.ToDateTime( a.ClockIn).Date == DateTime.Now.Date select a).SingleOrDefault();
            if(data!=null)
            {
                 message = "yes";
                return Json(message, JsonRequestBehavior.AllowGet);
            }
          message = "no";
            return Json(message, JsonRequestBehavior.AllowGet);

        }

        public JsonResult getsalarydone(EmployeeRegistration employee)
        {
            var message="";
            var data = (from a in db.Tbl_Payrolls where a.EmpID == Convert.ToInt32(employee.EmpId) &&Convert.ToDateTime( a.Date).Date == DateTime.Now.Date select a).SingleOrDefault();
            if (data != null)
            {
                message = "Done";
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            else
            {
                message = "not";
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            
        }
       
        public JsonResult getattendance(EmployeeRegistration employee)
        {
            var data = (from a in db.Tbl_Attendances where a.Emp_ID==Convert.ToInt32(employee.EmpId)&& Convert.ToDateTime(a.Date).Date == DateTime.Now.Date select new
            { a.ClockIn,
            a.ClockOut,
            a.totalbreak,
            a.btnend,
            a.btnstart,
            a.breakstart,
            a.breakend,
            totalhr=DateTime.Now-
                Convert.ToDateTime(a.ClockIn) ,
            }).SingleOrDefault();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult changepassworddata(EmployeeRegistration employee)
        {
            var b = (string)Session["username"];
            string data1;

            if (b == "" || b == null)
            {
                Response.Write("Your session has een expired please login again");
                Response.Redirect("/Login.aspx");

            }
            else
            {
                var usertype = (string)Session["usertype"];
                var empid = (string)Session["empid"];
                if (usertype == "Employee")
                {
                    var data = (from a in db.Tbl_EmployeeRegistrations where a.Emp_id ==Convert.ToInt32( empid) select a).SingleOrDefault();
                    if (data.Password.Trim() == employee.oldpassword.Trim())
                    {
                        Tbl_EmployeeRegistration t = db.Tbl_EmployeeRegistrations.Single(u =>u.Emp_id == Convert.ToInt32(empid));
                        t.Password = Convert.ToString(employee.newpassword).Trim();
                        db.SubmitChanges();
                        data1 = "Changed";
                        Session["username"] = "";
                        Session["usertype"]="";
         Session["empid"]="";
                        return Json(data1, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        data1 = "Not";
                        return Json(data1, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    var data = (from a in db.Tbl_UserRegistartions where a.UserID == Convert.ToInt32(empid) select a).SingleOrDefault();
                    if (data.Password.Trim() == employee.oldpassword.Trim())
                    {
                        Tbl_UserRegistartion t = db.Tbl_UserRegistartions.Single(u => u.UserID == Convert.ToInt32(empid));
                        t.Password = Convert.ToString(employee.newpassword).Trim();
                        db.SubmitChanges();
                        Session["username"] = "";
                        Session["usertype"] = "";
                        Session["empid"] = "";
                        data1 = "Changed";
                        return Json(data1, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        data1 = "Not";
                        return Json(data1, JsonRequestBehavior.AllowGet);
                    }
                }

            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getlastsalary(EmployeeRegistration employee)
        {
            var data = (from a in db.Tbl_Payrolls
                        orderby a.PayrollID descending
                        where a.EmpID == Convert.ToInt32(employee.EmpId)
                        select new
                        {
       EmpId=a.EmpID,
       empname=a.Tbl_EmployeeRegistration.Emp_Name,
       Designation=a.Tbl_EmployeeRegistration.Designation,
       department=a.Tbl_EmployeeRegistration.Department,
       
       DateTo =Convert.ToDateTime( a.Dateto).AddDays(1),
        DateFrom =Convert.ToDateTime( a.Datefrom).AddDays(1),
        basicsalary =a.BasicSalary,
      totaldays =a.TotalDays,

  presentdays =a.PresentDays,
       leavedays =a.leavdays,
     salaryperday =a.salaryperday,
         otherpay =a.otherpay,
        totalpay =a.TotalPay,
        leavededuct =a.LeaveDeduction,
        TDS =a.TDS,
        profTax =a.ProfTax,
         otherdeduction =a.OtherDeduction,
        totalearning =a.TotalEarning,

    }).FirstOrDefault();
       return Json(data, JsonRequestBehavior.AllowGet);
    }

            public ActionResult ChangePassword()
        {

            var a = (string)Session["username"];


            if (a == "" || a == null)
            {
                Response.Write("Your session has een expired please login again");
                Response.Redirect("/Login.aspx");

            }
            else
            {


                return View();
            }
            return View();

        }
        public JsonResult reset(EmployeeRegistration employee)
        {
            Tbl_Attendance t = db.Tbl_Attendances.Single(s => s.Emp_ID == Convert.ToInt32(employee.EmpId) && Convert.ToDateTime(s.Date).Date ==DateTime.Now.Date);
            t.ClockOut = null;
            db.SubmitChanges();
            var data = "Done";
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getbday(EmployeeRegistration employee)
        {
            try
            {
                var data = (from a in db.Tbl_EmployeeRegistrations
                            where Convert.ToDateTime(a.Emp_DOB).Month == DateTime.Now.Month&& Convert.ToDateTime(a.Emp_DOB).Day==DateTime.Now.Day
                            select new
                            {
                                name = a.Emp_Name.Trim(),
                             
                            }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public JsonResult getwannvisary(EmployeeRegistration employee)
       {
            try
            {
                var data = (from a in db.Tbl_EmployeeRegistrations
                            where Convert.ToDateTime(a.Emp_DOJ).Month == DateTime.Now.Month && Convert.ToDateTime(a.Emp_DOJ).Day == DateTime.Now.Day
                            select new
                            {
                                name = a.Emp_Name.Trim(),

                            }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public JsonResult  resetclock(EmployeeRegistration employee)
        {
            try
            {
                var data = (from a in db.Tbl_Attendances where a.Emp_ID == Convert.ToInt32(employee.EmpId) && Convert.ToDateTime(a.Date).Date == Convert.ToDateTime(employee.redate) select a).SingleOrDefault();
                string date = Convert.ToDateTime(data.ClockOut).Date.ToString();
                string newdate = date.Replace("00:00:00", employee.hours + ":" + employee.min + ":" + "00");
                Tbl_Attendance t = db.Tbl_Attendances.Single(s => s.Emp_ID == Convert.ToInt32(employee.EmpId) && Convert.ToDateTime(s.Date).Date == Convert.ToDateTime(employee.redate).Date);
                t.ClockOut = null;

                t.ClockOut = Convert.ToDateTime(newdate);

                db.SubmitChanges();
                var data1 = (from a in db.Tbl_Attendances
                            where a.Emp_ID == Convert.ToInt32(employee.EmpId)&& Convert.ToDateTime(a.Date).Date == Convert.ToDateTime(employee.redate)
                             select new
                            {
                                a.DetailId,
                                a.Emp_ID,
                                a.ClockIn,
                                a.ClockOut,
                                a.totalbreak,
                                a.btnend,
                                a.btnstart,
                                a.breakstart,
                                a.breakend,
                                a.Date
                            }).ToList();
                return Json(data1, JsonRequestBehavior.AllowGet);
              
            }
            catch (Exception)
            {
                var data = "No Records Found!!";
                return Json(data, JsonRequestBehavior.AllowGet);

            }
          
        }
        public JsonResult getdep(EmployeeRegistration employee)
        {
            var data = (from a in db.Tbl_EmployeeRegistrations select new {
                Department = a.Department.Trim(),
                total=db.Tbl_EmployeeRegistrations.Where(t=>t.Department==a.Department).Count(),
            }).Distinct().ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
            public JsonResult getattendances(EmployeeRegistration employee)
        {
            var data = (from a in db.Tbl_Attendances
                        where Convert.ToDateTime(a.Date).Date == DateTime.Now.Date
                        select new
                        {a.DetailId,
                            a.Emp_ID,
                            a.ClockIn,
                            a.ClockOut,
                            a.totalbreak,
                            a.btnend,
                            a.btnstart,
                            a.breakstart,
                            a.breakend,
                        }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        
         public JsonResult payrolldata(EmployeeRegistration employee)
        {
            try
            {
                var data = (from a in db.Tbl_Attendances
                            where a.Emp_ID == Convert.ToInt32(employee.EmpId) && a.Date > employee.datefrom.Date && a.Date < employee.todate
                            select new
                            {
                                empname = a.Tbl_EmployeeRegistration.Emp_Name,
                                designation = a.Tbl_EmployeeRegistration.Designation,
                                dep = a.Tbl_EmployeeRegistration.Department,
                                salary = a.Tbl_EmployeeRegistration.BasicSalary,
                                totaldays = db.Tbl_Attendances.Where(s => s.Emp_ID == Convert.ToInt32(employee.EmpId) && s.Date > employee.datefrom.Date && s.Date < employee.todate).Count(),
                             

                            }).ToList();

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                var message = "Records Not Found!!";
                return Json(message, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult subscriptionrecipt(string id)
        {
            try
            {
                var empid = (string)Session["empid"];
                var data = (from a in db.Tbl_Billings
                            where a.UserId == Convert.ToInt32(empid)
                            orderby a.BillId descending
                            select new
                            {
                                Bill_ID = db.Tbl_Billings.OrderByDescending(t => t.BillId).Where(t => t.UserId == Convert.ToInt32(empid)).Select(t => t.BillId).FirstOrDefault(),
                                date = db.Tbl_Billings.OrderByDescending(t => t.BillId).Where(t => t.UserId == Convert.ToInt32(empid)).Select(t => t.Date).FirstOrDefault(),
                                BillName = db.Tbl_Billings.OrderByDescending(t => t.BillId).Where(t => t.UserId == Convert.ToInt32(empid)).Select(t => t.BillName).FirstOrDefault(),
                                Amount = db.Tbl_Billings.OrderByDescending(t => t.BillId).Where(t => t.UserId == Convert.ToInt32(empid)).Select(t => t.Amount).FirstOrDefault(),
                                address = db.Tbl_Billings.OrderByDescending(t => t.BillId).Where(t => t.UserId == Convert.ToInt32(empid)).Select(t => t.Address).FirstOrDefault(),
                                city = db.Tbl_Billings.OrderByDescending(t => t.BillId).Where(t => t.UserId == Convert.ToInt32(empid)).Select(t => t.City).FirstOrDefault(),
                                state = db.Tbl_Billings.OrderByDescending(t => t.BillId).Where(t => t.UserId == Convert.ToInt32(empid)).Select(t => t.State).FirstOrDefault(),
                                email= db.Tbl_Billings.OrderByDescending(t => t.BillId).Where(t => t.UserId == Convert.ToInt32(empid)).Select(t => t.email).FirstOrDefault(),
                            }).ToList();
             
                LocalReport lc1 = new LocalReport();
                lc1.ReportPath = Server.MapPath("/Reports/RptSubsciptionRecipt.rdlc");
                ReportDataSource rd1 = new ReportDataSource("SubsciptionRecipt", data.ToList());
                rd1.Value = data;
                lc1.DataSources.Add(rd1);
                string reporttype = "pdf";
                string mimetype1;
                string encoding1;
                string filenameextention1;


                filenameextention1 = "pdf";
                string[] strems1;
                Warning[] warnings1;
                byte[] renderdbyte1;
                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                    , out strems1, out warnings1);
                Response.AddHeader("content-disposition", "inline;filename=SubsciptionRecipt." + filenameextention1);
                return File(renderdbyte1, "." + filenameextention1);
            }
            catch (Exception ex)
            {

                throw;
            }


        }
        public ActionResult salaryslip(string id,string reporttype1)
        {
            try
            {
                db.CommandTimeout = 240;

                var data = (from a in db.Tbl_Payrolls
                            orderby a.PayrollID descending
                            where a.EmpID == Convert.ToInt32(id)
                            select new
                            {
                                Id = a.EmpID,
                                Name = a.Tbl_EmployeeRegistration.Emp_Name,
                                Title = a.Tbl_EmployeeRegistration.Designation,
                                Designation = a.Tbl_EmployeeRegistration.Designation,
                                Dep = a.Tbl_EmployeeRegistration.Department,
                                Todate = Convert.ToDateTime(a.Dateto),
                                Fromdate = Convert.ToDateTime(a.Datefrom),
                                BasicSalary = a.BasicSalary / 2,
                                Total = a.BasicSalary,
                                otherpay = a.otherpay,
                                totalpay = a.TotalPay,
                                Leavededuct = Convert.ToInt32(a.LeaveDeduction),
                                TDS = a.TDS,
                                profTax = a.ProfTax,
                                otherdeduction = a.OtherDeduction,
                                NetPay = a.TotalEarning,
                                bankaccountname = a.Tbl_EmployeeRegistration.BankAccountName,
                                bankname = a.Tbl_EmployeeRegistration.BankName,
                                acNo = a.Tbl_EmployeeRegistration.AccountNo,
                                DOJ = a.Tbl_EmployeeRegistration.Emp_DOJ,
                                PaymentDate = a.Date,
                                Earninginword = a.TotalEarninginword.ToUpper(),
                              
                                //MedicalAllow= ((a.BasicSalary / 2)/2.5)-(((a.BasicSalary / 2)/2.5)/2.9),


                            }).ToList();
                LocalReport lc1 = new LocalReport();
                lc1.ReportPath = Server.MapPath("/Reports/RptSalaryslip.rdlc");
                ReportDataSource rd1 = new ReportDataSource("Salaryslips", data.ToList());
                rd1.Value = data;
                lc1.DataSources.Add(rd1);
                string reporttype = "pdf";
                string mimetype1;
                string encoding1;
                string filenameextention1;


                filenameextention1 = "pdf";
                string[] strems1;
                Warning[] warnings1;
                byte[] renderdbyte1;
                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                    , out strems1, out warnings1);
                Response.AddHeader("content-disposition", "inline;filename=Salary." + filenameextention1);
                return File(renderdbyte1, "." + filenameextention1);
            }
            catch (Exception ex)
            {

                throw;
            }


        }


            public JsonResult lastsalary(EmployeeRegistration employee)
        {
            var data = (from a in db.Tbl_Payrolls orderby a.PayrollID descending where a.EmpID == Convert.ToInt32(employee.EmpId) select new
            {
                Dateto = Convert.ToDateTime(a.Dateto).AddDays(1),
            }).FirstOrDefault();
            if (data == null)
            {
                var message = "null";
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Getleavedata(EmployeeRegistration employee)
        {
            var data = (from a in db.Tbl_EmpLeaves
                        where a.Emp_Id == Convert.ToInt32(employee.EmpId)
                        select new
                        {
                            profilepath = a.Tbl_EmployeeRegistration.profilepath,
                            totalleaves = db.Tbl_EmpLeaves.Where(t => t.Emp_Id == Convert.ToInt32(employee.EmpId)).Select(t => t.LeaveDetailId).Sum(),
                            lastleavestetus = db.Tbl_EmpLeaves.OrderByDescending(t=>t.LeaveDetailId).Where(t => t.Emp_Id == Convert.ToInt32(employee.EmpId)).Select(t => t.Stetus).First(),
                            totalapprove= db.Tbl_EmpLeaves.OrderByDescending(t => t.LeaveDetailId).Where(t => t.Emp_Id == Convert.ToInt32(employee.EmpId)&&t.Stetus==1).Select(t => t.Stetus).Count(),
                        }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Getbillingdata(EmployeeRegistration employee)
        {
            var data = (from a in db.Tbl_Billings
                        where a.UserId == Convert.ToInt32(employee.EmpId)
                        orderby a.BillId descending
                        select new
                        {
                            Bill_ID = a.BillId,
                            date = a.Date,
                            BillName = a.BillName,
                            Amount = a.Amount,
                            address = a.Address,
                            city = a.City,
                            state = a.State
                        }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult profile(EmployeeRegistration employee)
        {
            var data = (from a in db.Tbl_EmployeeRegistrations
                        where a.Emp_id == Convert.ToInt32(employee.EmpId)
                        select new
                        {
                            profilepath = a.profilepath,

                        }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult gettimesheet(EmployeeRegistration employee)
        {
            try
            {
                if (employee.EmpId != ""&& employee.EmpId != null)
                {
                   
                    if (employee.datefrom.ToString() != "01-01-01 00:00:00")
                    {
                        var data = (from a in db.Tbl_Attendances
                                    where a.Emp_ID==Convert.ToInt32(employee.EmpId) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= employee.datefrom.Date && Convert.ToDateTime(a.ClockIn).Date <= employee.todate.Date
                                    orderby a.ClockIn ascending
                                    select new
                                    {
                                        id = a.Emp_ID,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                        date = Convert.ToDateTime(a.ClockIn).Date

                                    }).ToList();
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        DateTime baseDate = DateTime.Today;

                        var today = baseDate;
                        var yesterday = baseDate.AddDays(-1);
                        var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
                        var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
                        var previousWeekStart = thisWeekStart.AddDays(-7);
                        var previousWeekEnd = thisWeekStart.AddDays(1);
                        var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
                        var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
                        var firstDayLastMonth = thisMonthStart.AddMonths(-1);
                        var lastDayLastMonth = thisMonthStart.AddSeconds(-1);
                        if (employee.dropdown == "-1")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where a.Emp_ID == Convert.ToInt32(employee.EmpId)  && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date == DateTime.Now.Date.AddDays(-1)
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            id = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                            date = Convert.ToDateTime(a.ClockIn).Date

                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        else if (employee.dropdown == "1")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where a.Emp_ID == Convert.ToInt32(employee.EmpId) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date == DateTime.Now.Date
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            id = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                            date = Convert.ToDateTime(a.ClockIn).Date

                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        else if (employee.dropdown == "7")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where a.Emp_ID == Convert.ToInt32(employee.EmpId)  && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= thisWeekStart && Convert.ToDateTime(a.ClockIn).Date <= thisWeekEnd
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            id = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                            date = Convert.ToDateTime(a.ClockIn).Date

                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        else if (employee.dropdown == "<8")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where a.Emp_ID == Convert.ToInt32(employee.EmpId)  && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= previousWeekStart && Convert.ToDateTime(a.ClockIn).Date <= previousWeekEnd
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            id = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                            date = Convert.ToDateTime(a.ClockIn).Date

                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        else if (employee.dropdown == "<30")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where a.Emp_ID == Convert.ToInt32(employee.EmpId)  && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= firstDayLastMonth && Convert.ToDateTime(a.ClockIn).Date <= lastDayLastMonth
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            id = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                            date = Convert.ToDateTime(a.ClockIn).Date

                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        else if (employee.dropdown == "30")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where a.Emp_ID == Convert.ToInt32(employee.EmpId)  && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= thisMonthStart && Convert.ToDateTime(a.ClockIn).Date <= thisMonthEnd
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            id = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                            date = Convert.ToDateTime(a.ClockIn).Date

                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    if (employee.datefrom.ToString() != "01-01-01 00:00:00")
                    {
                        var data = (from a in db.Tbl_Attendances
                                    where a.ClockIn != null  && Convert.ToDateTime(a.ClockIn).Date >= employee.datefrom.Date && Convert.ToDateTime(a.ClockIn).Date <= employee.todate.Date
                                    orderby a.ClockIn ascending
                                    select new
                                    {
                                        id = a.Emp_ID,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                        date = Convert.ToDateTime(a.ClockIn).Date

                                    }).ToList();
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        DateTime baseDate = DateTime.Today;

                        var today = baseDate;
                        var yesterday = baseDate.AddDays(-1);
                        var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
                        var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
                        var previousWeekStart = thisWeekStart.AddDays(-7);
                        var previousWeekEnd = thisWeekStart.AddDays(1);
                        var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
                        var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
                        var firstDayLastMonth = thisMonthStart.AddMonths(-1);
                        var lastDayLastMonth = thisMonthStart.AddSeconds(-1);
                        if (employee.dropdown == "-1")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where a.ClockIn != null  && Convert.ToDateTime(a.ClockIn).Date == DateTime.Now.Date.AddDays(-1)
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            id = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                            date = Convert.ToDateTime(a.ClockIn).Date

                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        else if (employee.dropdown == "1")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date == DateTime.Now.Date
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            id = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                            date = Convert.ToDateTime(a.ClockIn).Date

                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        else if (employee.dropdown == "7")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where Convert.ToDateTime( a.ClockIn).Date != DateTime.Now.Date && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= thisWeekStart && Convert.ToDateTime(a.ClockIn).Date <= thisWeekEnd
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            id = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                            date = Convert.ToDateTime(a.ClockIn).Date

                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        else if (employee.dropdown == "<8")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= previousWeekStart && Convert.ToDateTime(a.ClockIn).Date <= previousWeekEnd
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            id = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                            date = Convert.ToDateTime(a.ClockIn).Date

                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        else if (employee.dropdown == "<30")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date  && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= firstDayLastMonth && Convert.ToDateTime(a.ClockIn).Date <= lastDayLastMonth
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            id = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                            date = Convert.ToDateTime(a.ClockIn).Date

                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        else if (employee.dropdown == "30")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date  && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= thisMonthStart && Convert.ToDateTime(a.ClockIn).Date <= thisMonthEnd
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            id = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                            date = Convert.ToDateTime(a.ClockIn).Date

                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                var id = (from b in db.Tbl_EmployeeRegistrations where b.Emp_Name.Trim() == employee.EmpId.Trim() select b.Emp_id).SingleOrDefault();
               
                if (employee.datefrom.ToString() != "01-01-01 00:00:00")
                {
                  
                    var data = (from a in db.Tbl_Attendances
                                where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.Emp_ID == Convert.ToInt32(id) && a.ClockIn != null  && Convert.ToDateTime(a.ClockIn).Date >= employee.datefrom.Date && Convert.ToDateTime(a.ClockIn).Date <= employee.todate.Date
                                orderby a.ClockIn ascending
                                select new
                                {
                                    id = a.Emp_ID,
                                    name = a.Tbl_EmployeeRegistration.Emp_Name,
                                    totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                    date = Convert.ToDateTime(a.ClockIn).Date

                                }).ToList();
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    DateTime baseDate = DateTime.Today;

                    var today = baseDate;
                    var yesterday = baseDate.AddDays(-1);
                    var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
                    var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
                    var previousWeekStart = thisWeekStart.AddDays(-7);
                    var previousWeekEnd = thisWeekStart.AddDays(1);
                    var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
                    var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
                    var firstDayLastMonth = thisMonthStart.AddMonths(-1);
                    var lastDayLastMonth = thisMonthStart.AddSeconds(-1);
                    if (employee.dropdown == "-1")
                    {
                        var data = (from a in db.Tbl_Attendances
                                    where a.Emp_ID == Convert.ToInt32(id)  && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date == DateTime.Now.Date.AddDays(-1)
                                    orderby a.ClockIn ascending
                                    select new
                                    {
                                        id = a.Emp_ID,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                        date = Convert.ToDateTime(a.ClockIn).Date

                                    }).ToList();
                        return Json(data, JsonRequestBehavior.AllowGet);

                    }
                    else if (employee.dropdown == "1")
                    {
                        var data = (from a in db.Tbl_Attendances
                                    where a.Emp_ID == Convert.ToInt32(id)  && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date == DateTime.Now.Date
                                    orderby a.ClockIn ascending
                                    select new
                                    {
                                        id = a.Emp_ID,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                        date = Convert.ToDateTime(a.ClockIn).Date

                                    }).ToList();
                        return Json(data, JsonRequestBehavior.AllowGet);

                    }
                    else if (employee.dropdown == "7")
                    {
                        var data = (from a in db.Tbl_Attendances
                                    where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.Emp_ID == Convert.ToInt32(id) && a.ClockIn != null  && Convert.ToDateTime(a.ClockIn).Date >= thisWeekStart && Convert.ToDateTime(a.ClockIn).Date <= thisWeekEnd
                                    orderby a.ClockIn ascending
                                    select new
                                    {
                                        id = a.Emp_ID,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                        date = Convert.ToDateTime(a.ClockIn).Date

                                    }).ToList();
                        return Json(data, JsonRequestBehavior.AllowGet);

                    }
                    else if (employee.dropdown == "<8")
                    {
                        var data = (from a in db.Tbl_Attendances
                                    where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.Emp_ID == Convert.ToInt32(id) && a.ClockIn != null  && Convert.ToDateTime(a.ClockIn).Date >= previousWeekStart && Convert.ToDateTime(a.ClockIn).Date <= previousWeekEnd
                                    orderby a.ClockIn ascending
                                    select new
                                    {
                                        id = a.Emp_ID,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                        date = Convert.ToDateTime(a.ClockIn).Date

                                    }).ToList();
                        return Json(data, JsonRequestBehavior.AllowGet);

                    }
                    else if (employee.dropdown == "<30")
                    {
                        var data = (from a in db.Tbl_Attendances
                                    where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.Emp_ID == Convert.ToInt32(id) && a.ClockIn != null  && Convert.ToDateTime(a.ClockIn).Date >= firstDayLastMonth && Convert.ToDateTime(a.ClockIn).Date <= lastDayLastMonth
                                    orderby a.ClockIn ascending
                                    select new
                                    {
                                        id = a.Emp_ID,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                        date = Convert.ToDateTime(a.ClockIn).Date

                                    }).ToList();
                        return Json(data, JsonRequestBehavior.AllowGet);

                    }
                    else if (employee.dropdown == "30")
                    {
                        var data = (from a in db.Tbl_Attendances
                                    where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.Emp_ID == Convert.ToInt32(id) && a.ClockIn != null  && Convert.ToDateTime(a.ClockIn).Date >= thisMonthStart && Convert.ToDateTime(a.ClockIn).Date <= thisMonthEnd
                                    orderby a.ClockIn ascending
                                    select new
                                    {
                                        id = a.Emp_ID,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                        date = Convert.ToDateTime(a.ClockIn).Date

                                    }).ToList();
                        return Json(data, JsonRequestBehavior.AllowGet);

                    }
                    return Json(null, JsonRequestBehavior.AllowGet);
                }

            }
        }
        public JsonResult gettimesheetsummery(EmployeeRegistration employee)
        {
            try
            {
                if (employee.EmpId != "" && employee.EmpId != null)
                {

                    if (employee.datefrom.ToString() != "01-01-01 00:00:00")
                    {
                        var data = (from a in db.Tbl_Attendances
                                    where a.Emp_ID == Convert.ToInt32(employee.EmpId) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= employee.datefrom.Date && Convert.ToDateTime(a.ClockIn).Date <= employee.todate.Date
                                    orderby a.ClockIn ascending
                                    select new
                                    {
                                        id = a.Emp_ID,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        dep=a.Tbl_EmployeeRegistration.Department,
                                        date = Convert.ToDateTime(a.ClockIn).Date,
                                        start=a.ClockIn,
                                        end=a.ClockOut,
                                        totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),

                                    }).ToList();
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        DateTime baseDate = DateTime.Today;

                        var today = baseDate;
                        var yesterday = baseDate.AddDays(-1);
                        var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
                        var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
                        var previousWeekStart = thisWeekStart.AddDays(-7);
                        var previousWeekEnd = thisWeekStart.AddDays(1);
                        var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
                        var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
                        var firstDayLastMonth = thisMonthStart.AddMonths(-1);
                        var lastDayLastMonth = thisMonthStart.AddSeconds(-1);
                        if (employee.dropdown == "-1")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where a.Emp_ID == Convert.ToInt32(employee.EmpId) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date == DateTime.Now.Date.AddDays(-1)
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            id = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            date = Convert.ToDateTime(a.ClockIn).Date,
                                            start = a.ClockIn,
                                            end = a.ClockOut,
                                            totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),

                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        else if (employee.dropdown == "1")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where a.Emp_ID == Convert.ToInt32(employee.EmpId) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date == DateTime.Now.Date
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            id = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            date = Convert.ToDateTime(a.ClockIn).Date,
                                            start = a.ClockIn,
                                            end = a.ClockOut,
                                            totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        else if (employee.dropdown == "7")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where a.Emp_ID == Convert.ToInt32(employee.EmpId) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= thisWeekStart && Convert.ToDateTime(a.ClockIn).Date <= thisWeekEnd
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            id = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            date = Convert.ToDateTime(a.ClockIn).Date,
                                            start = a.ClockIn,
                                            end = a.ClockOut,
                                            totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),

                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        else if (employee.dropdown == "<8")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where a.Emp_ID == Convert.ToInt32(employee.EmpId) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= previousWeekStart && Convert.ToDateTime(a.ClockIn).Date <= previousWeekEnd
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            id = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            date = Convert.ToDateTime(a.ClockIn).Date,
                                            start = a.ClockIn,
                                            end = a.ClockOut,
                                            totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),

                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        else if (employee.dropdown == "<30")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where a.Emp_ID == Convert.ToInt32(employee.EmpId) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= firstDayLastMonth && Convert.ToDateTime(a.ClockIn).Date <= lastDayLastMonth
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            id = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            date = Convert.ToDateTime(a.ClockIn).Date,
                                            start = a.ClockIn,
                                            end = a.ClockOut,
                                            totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),

                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        else if (employee.dropdown == "30")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where a.Emp_ID == Convert.ToInt32(employee.EmpId) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= thisMonthStart && Convert.ToDateTime(a.ClockIn).Date <= thisMonthEnd
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            id = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            date = Convert.ToDateTime(a.ClockIn).Date,
                                            start = a.ClockIn,
                                            end = a.ClockOut,
                                            totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),

                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    if (employee.datefrom.ToString() != "01-01-01 00:00:00")
                    {
                        var data = (from a in db.Tbl_Attendances
                                    where a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= employee.datefrom.Date && Convert.ToDateTime(a.ClockIn).Date <= employee.todate.Date
                                    orderby a.ClockIn ascending
                                    select new
                                    {
                                        id = a.Emp_ID,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        dep = a.Tbl_EmployeeRegistration.Department,
                                        date = Convert.ToDateTime(a.ClockIn).Date,
                                        start = a.ClockIn,
                                        end = a.ClockOut,
                                        totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),

                                    }).ToList();
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        DateTime baseDate = DateTime.Today;

                        var today = baseDate;
                        var yesterday = baseDate.AddDays(-1);
                        var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
                        var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
                        var previousWeekStart = thisWeekStart.AddDays(-7);
                        var previousWeekEnd = thisWeekStart.AddDays(1);
                        var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
                        var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
                        var firstDayLastMonth = thisMonthStart.AddMonths(-1);
                        var lastDayLastMonth = thisMonthStart.AddSeconds(-1);
                        if (employee.dropdown == "-1")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date == DateTime.Now.Date.AddDays(-1)
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            id = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            date = Convert.ToDateTime(a.ClockIn).Date,
                                            start = a.ClockIn,
                                            end = a.ClockOut,
                                            totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),

                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        else if (employee.dropdown == "1")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date == DateTime.Now.Date
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            id = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            date = Convert.ToDateTime(a.ClockIn).Date,
                                            start = a.ClockIn,
                                            end = a.ClockOut,
                                            totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),

                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        else if (employee.dropdown == "7")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= thisWeekStart && Convert.ToDateTime(a.ClockIn).Date <= thisWeekEnd
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            id = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            date = Convert.ToDateTime(a.ClockIn).Date,
                                            start = a.ClockIn,
                                            end = a.ClockOut,
                                            totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),

                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        else if (employee.dropdown == "<8")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= previousWeekStart && Convert.ToDateTime(a.ClockIn).Date <= previousWeekEnd
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            id = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            date = Convert.ToDateTime(a.ClockIn).Date,
                                            start = a.ClockIn,
                                            end = a.ClockOut,
                                            totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        else if (employee.dropdown == "<30")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= firstDayLastMonth && Convert.ToDateTime(a.ClockIn).Date <= lastDayLastMonth
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            id = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            date = Convert.ToDateTime(a.ClockIn).Date,
                                            start = a.ClockIn,
                                            end = a.ClockOut,
                                            totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),

                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        else if (employee.dropdown == "30")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= thisMonthStart && Convert.ToDateTime(a.ClockIn).Date <= thisMonthEnd
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            id = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            date = Convert.ToDateTime(a.ClockIn).Date,
                                            start = a.ClockIn,
                                            end = a.ClockOut,
                                            totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),

                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                var id = (from b in db.Tbl_EmployeeRegistrations where b.Emp_Name.Trim() == employee.EmpId.Trim() select b.Emp_id).SingleOrDefault();

                if (employee.datefrom.ToString() != "01-01-01 00:00:00")
                {

                    var data = (from a in db.Tbl_Attendances
                                where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.Emp_ID == Convert.ToInt32(id) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= employee.datefrom.Date && Convert.ToDateTime(a.ClockIn).Date <= employee.todate.Date
                                orderby a.ClockIn ascending
                                select new
                                {
                                    id = a.Emp_ID,
                                    name = a.Tbl_EmployeeRegistration.Emp_Name,
                                    dep = a.Tbl_EmployeeRegistration.Department,
                                    date = Convert.ToDateTime(a.ClockIn).Date,
                                    start = a.ClockIn,
                                    end = a.ClockOut,
                                    totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),

                                }).ToList();
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    DateTime baseDate = DateTime.Today;

                    var today = baseDate;
                    var yesterday = baseDate.AddDays(-1);
                    var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
                    var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
                    var previousWeekStart = thisWeekStart.AddDays(-7);
                    var previousWeekEnd = thisWeekStart.AddDays(1);
                    var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
                    var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
                    var firstDayLastMonth = thisMonthStart.AddMonths(-1);
                    var lastDayLastMonth = thisMonthStart.AddSeconds(-1);
                    if (employee.dropdown == "-1")
                    {
                        var data = (from a in db.Tbl_Attendances
                                    where a.Emp_ID == Convert.ToInt32(id) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date == DateTime.Now.Date.AddDays(-1)
                                    orderby a.ClockIn ascending
                                    select new
                                    {
                                        id = a.Emp_ID,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        dep = a.Tbl_EmployeeRegistration.Department,
                                        date = Convert.ToDateTime(a.ClockIn).Date,
                                        start = a.ClockIn,
                                        end = a.ClockOut,
                                        totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),

                                    }).ToList();
                        return Json(data, JsonRequestBehavior.AllowGet);

                    }
                    else if (employee.dropdown == "1")
                    {
                        var data = (from a in db.Tbl_Attendances
                                    where a.Emp_ID == Convert.ToInt32(id) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date == DateTime.Now.Date
                                    orderby a.ClockIn ascending
                                    select new
                                    {
                                        id = a.Emp_ID,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        dep = a.Tbl_EmployeeRegistration.Department,
                                        date = Convert.ToDateTime(a.ClockIn).Date,
                                        start = a.ClockIn,
                                        end = a.ClockOut,
                                        totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                    }).ToList();
                        return Json(data, JsonRequestBehavior.AllowGet);

                    }
                    else if (employee.dropdown == "7")
                    {
                        var data = (from a in db.Tbl_Attendances
                                    where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.Emp_ID == Convert.ToInt32(id) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= thisWeekStart && Convert.ToDateTime(a.ClockIn).Date <= thisWeekEnd
                                    orderby a.ClockIn ascending
                                    select new
                                    {
                                        id = a.Emp_ID,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        dep = a.Tbl_EmployeeRegistration.Department,
                                        date = Convert.ToDateTime(a.ClockIn).Date,
                                        start = a.ClockIn,
                                        end = a.ClockOut,
                                        totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),

                                    }).ToList();
                        return Json(data, JsonRequestBehavior.AllowGet);

                    }
                    else if (employee.dropdown == "<8")
                    {
                        var data = (from a in db.Tbl_Attendances
                                    where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.Emp_ID == Convert.ToInt32(id) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= previousWeekStart && Convert.ToDateTime(a.ClockIn).Date <= previousWeekEnd
                                    orderby a.ClockIn ascending
                                    select new
                                    {
                                        id = a.Emp_ID,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        dep = a.Tbl_EmployeeRegistration.Department,
                                        date = Convert.ToDateTime(a.ClockIn).Date,
                                        start = a.ClockIn,
                                        end = a.ClockOut,
                                        totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),

                                    }).ToList();
                        return Json(data, JsonRequestBehavior.AllowGet);

                    }
                    else if (employee.dropdown == "<30")
                    {
                        var data = (from a in db.Tbl_Attendances
                                    where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.Emp_ID == Convert.ToInt32(id) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= firstDayLastMonth && Convert.ToDateTime(a.ClockIn).Date <= lastDayLastMonth
                                    orderby a.ClockIn ascending
                                    select new
                                    {
                                        id = a.Emp_ID,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        dep = a.Tbl_EmployeeRegistration.Department,
                                        date = Convert.ToDateTime(a.ClockIn).Date,
                                        start = a.ClockIn,
                                        end = a.ClockOut,
                                        totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),

                                    }).ToList();
                        return Json(data, JsonRequestBehavior.AllowGet);

                    }
                    else if (employee.dropdown == "30")
                    {
                        var data = (from a in db.Tbl_Attendances
                                    where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.Emp_ID == Convert.ToInt32(id) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= thisMonthStart && Convert.ToDateTime(a.ClockIn).Date <= thisMonthEnd
                                    orderby a.ClockIn ascending
                                    select new
                                    {
                                        id = a.Emp_ID,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        dep = a.Tbl_EmployeeRegistration.Department,
                                        date = Convert.ToDateTime(a.ClockIn).Date,
                                        start = a.ClockIn,
                                        end = a.ClockOut,
                                        totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),

                                    }).ToList();
                        return Json(data, JsonRequestBehavior.AllowGet);

                    }
                    return Json(null, JsonRequestBehavior.AllowGet);
                }

            }
        }
        public JsonResult getattendsummery(EmployeeRegistration employee)
        {
            try
            {
                if (employee.EmpId != "" && employee.EmpId != null)
                {

                    if (employee.datefrom.ToString() != "01-01-01 00:00:00")
                    {
                        var data = (from a in db.Tbl_Attendances
                                    where a.Emp_ID == Convert.ToInt32(employee.EmpId) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= employee.datefrom.Date && Convert.ToDateTime(a.ClockIn).Date <= employee.todate.Date
                                    orderby a.ClockIn ascending
                                    select new
                                    {
                                        id = a.Emp_ID,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        dep = a.Tbl_EmployeeRegistration.Department,
                                        date = Convert.ToDateTime(a.Date).Date,
                                        clockin= Convert.ToDateTime(a.ClockIn).Date,

                                    }).ToList();
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        DateTime baseDate = DateTime.Today;

                        var today = baseDate;
                        var yesterday = baseDate.AddDays(-1);
                        var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
                        var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
                        var previousWeekStart = thisWeekStart.AddDays(-7);
                        var previousWeekEnd = thisWeekStart.AddDays(1);
                        var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
                        var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
                        var firstDayLastMonth = thisMonthStart.AddMonths(-1);
                        var lastDayLastMonth = thisMonthStart.AddSeconds(-1);
                        if (employee.dropdown == "-1")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where a.Emp_ID == Convert.ToInt32(employee.EmpId) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date == DateTime.Now.Date.AddDays(-1)
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            id = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            date = Convert.ToDateTime(a.Date).Date,
                                            clockin = Convert.ToDateTime(a.ClockIn).Date
                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        else if (employee.dropdown == "1")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where a.Emp_ID == Convert.ToInt32(employee.EmpId) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date == DateTime.Now.Date
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            id = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            date = Convert.ToDateTime(a.Date).Date,
                                            clockin = Convert.ToDateTime(a.ClockIn).Date
                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        else if (employee.dropdown == "7")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where a.Emp_ID == Convert.ToInt32(employee.EmpId) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= thisWeekStart && Convert.ToDateTime(a.ClockIn).Date <= thisWeekEnd
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            id = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            date = Convert.ToDateTime(a.Date).Date,
                                            clockin = Convert.ToDateTime(a.ClockIn).Date
                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        else if (employee.dropdown == "<8")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where a.Emp_ID == Convert.ToInt32(employee.EmpId) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= previousWeekStart && Convert.ToDateTime(a.ClockIn).Date <= previousWeekEnd
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            id = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            date = Convert.ToDateTime(a.Date).Date,
                                            clockin = Convert.ToDateTime(a.ClockIn).Date

                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        else if (employee.dropdown == "<30")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where a.Emp_ID == Convert.ToInt32(employee.EmpId) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= firstDayLastMonth && Convert.ToDateTime(a.ClockIn).Date <= lastDayLastMonth
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            id = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            date = Convert.ToDateTime(a.Date).Date,
                                            clockin = Convert.ToDateTime(a.ClockIn).Date
                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        else if (employee.dropdown == "30")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where a.Emp_ID == Convert.ToInt32(employee.EmpId) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= thisMonthStart && Convert.ToDateTime(a.ClockIn).Date <= thisMonthEnd
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            id = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            date = Convert.ToDateTime(a.Date).Date,
                                            clockin = Convert.ToDateTime(a.ClockIn).Date

                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    if (employee.datefrom.ToString() != "01-01-01 00:00:00")
                    {
                        var data = (from a in db.Tbl_Attendances
                                    where a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= employee.datefrom.Date && Convert.ToDateTime(a.ClockIn).Date <= employee.todate.Date
                                    orderby a.ClockIn ascending
                                    select new
                                    {
                                        id = a.Emp_ID,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        dep = a.Tbl_EmployeeRegistration.Department,
                                        date = Convert.ToDateTime(a.Date).Date,
                                        clockin = Convert.ToDateTime(a.ClockIn).Date

                                    }).ToList();
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        DateTime baseDate = DateTime.Today;

                        var today = baseDate;
                        var yesterday = baseDate.AddDays(-1);
                        var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
                        var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
                        var previousWeekStart = thisWeekStart.AddDays(-7);
                        var previousWeekEnd = thisWeekStart.AddDays(1);
                        var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
                        var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
                        var firstDayLastMonth = thisMonthStart.AddMonths(-1);
                        var lastDayLastMonth = thisMonthStart.AddSeconds(-1);
                        if (employee.dropdown == "-1")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date == DateTime.Now.Date.AddDays(-1)
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            id = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            date = Convert.ToDateTime(a.Date).Date,
                                            clockin = Convert.ToDateTime(a.ClockIn).Date

                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        else if (employee.dropdown == "1")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date == DateTime.Now.Date
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            id = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            date = Convert.ToDateTime(a.Date).Date,
                                            clockin = Convert.ToDateTime(a.ClockIn).Date

                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        else if (employee.dropdown == "7")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= thisWeekStart && Convert.ToDateTime(a.ClockIn).Date <= thisWeekEnd
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            id = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            date = Convert.ToDateTime(a.Date).Date,
                                            clockin = Convert.ToDateTime(a.ClockIn).Date

                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        else if (employee.dropdown == "<8")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= previousWeekStart && Convert.ToDateTime(a.ClockIn).Date <= previousWeekEnd
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            id = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            date = Convert.ToDateTime(a.Date).Date,
                                            clockin = Convert.ToDateTime(a.ClockIn).Date
                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        else if (employee.dropdown == "<30")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= firstDayLastMonth && Convert.ToDateTime(a.ClockIn).Date <= lastDayLastMonth
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            id = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            date = Convert.ToDateTime(a.Date).Date,
                                            clockin = Convert.ToDateTime(a.ClockIn).Date

                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        else if (employee.dropdown == "30")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= thisMonthStart && Convert.ToDateTime(a.ClockIn).Date <= thisMonthEnd
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            id = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            date = Convert.ToDateTime(a.Date).Date,
                                            clockin = Convert.ToDateTime(a.ClockIn).Date

                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                var id = (from b in db.Tbl_EmployeeRegistrations where b.Emp_Name.Trim() == employee.EmpId.Trim() select b.Emp_id).SingleOrDefault();

                if (employee.datefrom.ToString() != "01-01-01 00:00:00")
                {

                    var data = (from a in db.Tbl_Attendances
                                where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.Emp_ID == Convert.ToInt32(id) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= employee.datefrom.Date && Convert.ToDateTime(a.ClockIn).Date <= employee.todate.Date
                                orderby a.ClockIn ascending
                                select new
                                {
                                    id = a.Emp_ID,
                                    name = a.Tbl_EmployeeRegistration.Emp_Name,
                                    dep = a.Tbl_EmployeeRegistration.Department,
                                    date = Convert.ToDateTime(a.Date).Date,
                                    clockin = Convert.ToDateTime(a.ClockIn).Date

                                }).ToList();
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    DateTime baseDate = DateTime.Today;

                    var today = baseDate;
                    var yesterday = baseDate.AddDays(-1);
                    var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
                    var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
                    var previousWeekStart = thisWeekStart.AddDays(-7);
                    var previousWeekEnd = thisWeekStart.AddDays(1);
                    var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
                    var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
                    var firstDayLastMonth = thisMonthStart.AddMonths(-1);
                    var lastDayLastMonth = thisMonthStart.AddSeconds(-1);
                    if (employee.dropdown == "-1")
                    {
                        var data = (from a in db.Tbl_Attendances
                                    where a.Emp_ID == Convert.ToInt32(id) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date == DateTime.Now.Date.AddDays(-1)
                                    orderby a.ClockIn ascending
                                    select new
                                    {
                                        id = a.Emp_ID,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        dep = a.Tbl_EmployeeRegistration.Department,
                                        date = Convert.ToDateTime(a.Date).Date,
                                        clockin = Convert.ToDateTime(a.ClockIn).Date

                                    }).ToList();
                        return Json(data, JsonRequestBehavior.AllowGet);

                    }
                    else if (employee.dropdown == "1")
                    {
                        var data = (from a in db.Tbl_Attendances
                                    where a.Emp_ID == Convert.ToInt32(id) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date == DateTime.Now.Date
                                    orderby a.ClockIn ascending
                                    select new
                                    {
                                        id = a.Emp_ID,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        dep = a.Tbl_EmployeeRegistration.Department,
                                        date = Convert.ToDateTime(a.Date).Date,
                                        clockin = Convert.ToDateTime(a.ClockIn).Date
                                    }).ToList();
                        return Json(data, JsonRequestBehavior.AllowGet);

                    }
                    else if (employee.dropdown == "7")
                    {
                        var data = (from a in db.Tbl_Attendances
                                    where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.Emp_ID == Convert.ToInt32(id) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= thisWeekStart && Convert.ToDateTime(a.ClockIn).Date <= thisWeekEnd
                                    orderby a.ClockIn ascending
                                    select new
                                    {
                                        id = a.Emp_ID,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        dep = a.Tbl_EmployeeRegistration.Department,
                                        date = Convert.ToDateTime(a.Date).Date,
                                        clockin = Convert.ToDateTime(a.ClockIn).Date

                                    }).ToList();
                        return Json(data, JsonRequestBehavior.AllowGet);

                    }
                    else if (employee.dropdown == "<8")
                    {
                        var data = (from a in db.Tbl_Attendances
                                    where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.Emp_ID == Convert.ToInt32(id) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= previousWeekStart && Convert.ToDateTime(a.ClockIn).Date <= previousWeekEnd
                                    orderby a.ClockIn ascending
                                    select new
                                    {
                                        id = a.Emp_ID,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        dep = a.Tbl_EmployeeRegistration.Department,
                                        date = Convert.ToDateTime(a.Date).Date,
                                        clockin = Convert.ToDateTime(a.ClockIn).Date

                                    }).ToList();
                        return Json(data, JsonRequestBehavior.AllowGet);

                    }
                    else if (employee.dropdown == "<30")
                    {
                        var data = (from a in db.Tbl_Attendances
                                    where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.Emp_ID == Convert.ToInt32(id) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= firstDayLastMonth && Convert.ToDateTime(a.ClockIn).Date <= lastDayLastMonth
                                    orderby a.ClockIn ascending
                                    select new
                                    {
                                        id = a.Emp_ID,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        dep = a.Tbl_EmployeeRegistration.Department,
                                        date = Convert.ToDateTime(a.Date).Date,
                                        clockin = Convert.ToDateTime(a.ClockIn).Date

                                    }).ToList();
                        return Json(data, JsonRequestBehavior.AllowGet);

                    }
                    else if (employee.dropdown == "30")
                    {
                        var data = (from a in db.Tbl_Attendances
                                    where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.Emp_ID == Convert.ToInt32(id) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= thisMonthStart && Convert.ToDateTime(a.ClockIn).Date <= thisMonthEnd
                                    orderby a.ClockIn ascending
                                    select new
                                    {
                                        id = a.Emp_ID,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        dep = a.Tbl_EmployeeRegistration.Department,
                                        date = Convert.ToDateTime(a.Date).Date,
                                        clockin = Convert.ToDateTime(a.ClockIn).Date

                                    }).ToList();
                        return Json(data, JsonRequestBehavior.AllowGet);

                    }
                    return Json(null, JsonRequestBehavior.AllowGet);
                }

            }
        }

        public JsonResult getaleaves(EmployeeRegistration employee)
        {
            try
            {
                if (employee.EmpId != "" && employee.EmpId != null)
                {

                    if (employee.datefrom.ToString() != "01-01-01 00:00:00")
                    {
                        var data = (from a in db.Tbl_EmpLeaves
                                    where a.Emp_Id == Convert.ToInt32(employee.EmpId) &&  Convert.ToDateTime(a.From_Date).Date >= employee.datefrom.Date && Convert.ToDateTime(a.ToDate).Date <= employee.todate.Date && a.Stetus == 1

                                    select new
                                    {
                                        id = a.Emp_Id,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        dep = a.Tbl_EmployeeRegistration.Department,
                                        from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                        to = Convert.ToDateTime(a.ToDate).Date,
                                        from2 = Convert.ToDateTime(a.From_Date).Date,
                                        type=a.LeaveType

                                    }).ToList();
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        DateTime baseDate = DateTime.Today;

                        var today = baseDate;
                        var yesterday = baseDate.AddDays(-1);
                        var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
                        var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
                        var previousWeekStart = thisWeekStart.AddDays(-7);
                        var previousWeekEnd = thisWeekStart.AddDays(1);
                        var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
                        var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
                        var firstDayLastMonth = thisMonthStart.AddMonths(-1);
                        var lastDayLastMonth = thisMonthStart.AddSeconds(-1);
                        if (employee.dropdown == "-1")
                        {
                            var data = (from a in db.Tbl_EmpLeaves
                                        where a.Emp_Id == Convert.ToInt32(employee.EmpId) && Convert.ToDateTime(a.From_Date).Date == DateTime.Now.Date.AddDays(-1) && a.Stetus == 1

                                        select new
                                        {

                                            id = a.Emp_Id,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                            to = Convert.ToDateTime(a.ToDate).Date,
                                            from2 = Convert.ToDateTime(a.From_Date).Date,
                                            type = a.LeaveType
                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        else if (employee.dropdown == "1")
                        {
                            var data = (from a in db.Tbl_EmpLeaves
                                        where a.Emp_Id == Convert.ToInt32(employee.EmpId)  && Convert.ToDateTime(a.From_Date).Date == DateTime.Now.Date && a.Stetus == 1

                                        select new
                                        {

                                            id = a.Emp_Id,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                            to = Convert.ToDateTime(a.ToDate).Date,
                                            from2 = Convert.ToDateTime(a.From_Date).Date,
                                            type = a.LeaveType
                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        else if (employee.dropdown == "7")
                        {
                            var data = (from a in db.Tbl_EmpLeaves
                                        where a.Emp_Id == Convert.ToInt32(employee.EmpId)  && Convert.ToDateTime(a.From_Date).Date >= thisWeekStart && Convert.ToDateTime(a.ToDate).Date <= thisWeekEnd && a.Stetus == 1

                                        select new
                                        {

                                            id = a.Emp_Id,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                            to = Convert.ToDateTime(a.ToDate).Date,
                                            from2 = Convert.ToDateTime(a.From_Date).Date,
                                            type = a.LeaveType
                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        else if (employee.dropdown == "<8")
                        {
                            var data = (from a in db.Tbl_EmpLeaves
                                        where a.Emp_Id == Convert.ToInt32(employee.EmpId)  && Convert.ToDateTime(a.From_Date).Date >= previousWeekStart && Convert.ToDateTime(a.ToDate).Date <= previousWeekEnd && a.Stetus == 1

                                        select new
                                        {

                                            id = a.Emp_Id,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                            to = Convert.ToDateTime(a.ToDate).Date,
                                            from2 = Convert.ToDateTime(a.From_Date).Date,
                                            type = a.LeaveType

                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        else if (employee.dropdown == "<30")
                        {
                            var data = (from a in db.Tbl_EmpLeaves
                                        where a.Emp_Id == Convert.ToInt32(employee.EmpId) && Convert.ToDateTime(a.From_Date).Date >= firstDayLastMonth && Convert.ToDateTime(a.ToDate).Date <= lastDayLastMonth && a.Stetus == 1

                                        select new
                                        {


                                            id = a.Emp_Id,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                            to = Convert.ToDateTime(a.ToDate).Date,
                                            from2 = Convert.ToDateTime(a.From_Date).Date,
                                            type = a.LeaveType
                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        else if (employee.dropdown == "30")
                        {
                            var data = (from a in db.Tbl_EmpLeaves
                                        where a.Emp_Id == Convert.ToInt32(employee.EmpId)&& Convert.ToDateTime(a.From_Date).Date >= thisMonthStart && Convert.ToDateTime(a.ToDate).Date <= thisMonthEnd
                                       && a.Stetus == 1
                                        select new
                                        {

                                            id = a.Emp_Id,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                            to = Convert.ToDateTime(a.ToDate).Date,
                                            from2 = Convert.ToDateTime(a.From_Date).Date,
                                            type = a.LeaveType

                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    if (employee.datefrom.ToString() != "01-01-01 00:00:00")
                    {
                        var data = (from a in db.Tbl_EmpLeaves
                                    where  Convert.ToDateTime(a.From_Date).Date >= employee.datefrom.Date && Convert.ToDateTime(a.ToDate).Date <= employee.todate.Date
                                  && a.Stetus == 1
                                    select new
                                    {

                                        id = a.Emp_Id,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        dep = a.Tbl_EmployeeRegistration.Department,
                                        from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                        to = Convert.ToDateTime(a.ToDate).Date,
                                        from2 = Convert.ToDateTime(a.From_Date).Date,
                                        type = a.LeaveType

                                    }).ToList();
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        DateTime baseDate = DateTime.Today;

                        var today = baseDate;
                        var yesterday = baseDate.AddDays(-1);
                        var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
                        var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
                        var previousWeekStart = thisWeekStart.AddDays(-7);
                        var previousWeekEnd = thisWeekStart.AddDays(1);
                        var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
                        var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
                        var firstDayLastMonth = thisMonthStart.AddMonths(-1);
                        var lastDayLastMonth = thisMonthStart.AddSeconds(-1);
                        if (employee.dropdown == "-1")
                        {
                            var data = (from a in db.Tbl_EmpLeaves
                                        where  Convert.ToDateTime(a.From_Date).Date == DateTime.Now.Date.AddDays(-1)
                                      && a.Stetus == 1
                                        select new
                                        {

                                            id = a.Emp_Id,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                            to = Convert.ToDateTime(a.ToDate).Date,
                                            from2 = Convert.ToDateTime(a.From_Date).Date,
                                            type = a.LeaveType

                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        else if (employee.dropdown == "1")
                        {
                            var data = (from a in db.Tbl_EmpLeaves
                                        where Convert.ToDateTime(a.From_Date).Date == DateTime.Now.Date
                                        && a.Stetus == 1
                                        select new
                                        {

                                            id = a.Emp_Id,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                            to = Convert.ToDateTime(a.ToDate).Date,
                                            from2 = Convert.ToDateTime(a.From_Date).Date,
                                            type = a.LeaveType

                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        else if (employee.dropdown == "7")
                        {
                            var data = (from a in db.Tbl_EmpLeaves
                                        where  Convert.ToDateTime(a.From_Date).Date >= thisWeekStart && Convert.ToDateTime(a.ToDate).Date <= thisWeekEnd
                                       && a.Stetus == 1
                                        select new
                                        {

                                            id = a.Emp_Id,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                            to = Convert.ToDateTime(a.ToDate).Date,
                                            from2 = Convert.ToDateTime(a.From_Date).Date,
                                            type = a.LeaveType

                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        else if (employee.dropdown == "<8")
                        {
                            var data = (from a in db.Tbl_EmpLeaves
                                        where  Convert.ToDateTime(a.From_Date).Date >= previousWeekStart && Convert.ToDateTime(a.ToDate).Date <= previousWeekEnd
                                      && a.Stetus == 1
                                        select new
                                        {
                                            id = a.Emp_Id,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                            to = Convert.ToDateTime(a.ToDate).Date,
                                            from2 = Convert.ToDateTime(a.From_Date).Date,
                                            type = a.LeaveType
                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        else if (employee.dropdown == "<30")
                        {
                            var data = (from a in db.Tbl_EmpLeaves
                                        where  Convert.ToDateTime(a.From_Date).Date >= firstDayLastMonth && Convert.ToDateTime(a.ToDate).Date <= lastDayLastMonth
                                       && a.Stetus == 1
                                        select new
                                        {
                                            id = a.Emp_Id,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                            to = Convert.ToDateTime(a.ToDate).Date,
                                            from2 = Convert.ToDateTime(a.From_Date).Date,
                                            type = a.LeaveType

                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        else if (employee.dropdown == "30")
                        {
                            var data = (from a in db.Tbl_EmpLeaves
                                        where  Convert.ToDateTime(a.From_Date).Date >= thisMonthStart && Convert.ToDateTime(a.ToDate).Date <= thisMonthEnd
                                      && a.Stetus == 1
                                        select new
                                        {
                                            id = a.Emp_Id,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                            to = Convert.ToDateTime(a.ToDate).Date,
                                            from2 = Convert.ToDateTime(a.From_Date).Date,
                                            type = a.LeaveType

                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                var id = (from b in db.Tbl_EmployeeRegistrations where b.Emp_Name.Trim() == employee.EmpId.Trim() select b.Emp_id).SingleOrDefault();

                if (employee.datefrom.ToString() != "01-01-01 00:00:00")
                {

                    var data = (from a in db.Tbl_EmpLeaves
                                where  a.Emp_Id == Convert.ToInt32(id)  && Convert.ToDateTime(a.From_Date).Date >= employee.datefrom.Date && Convert.ToDateTime(a.ToDate).Date <= employee.todate.Date && a.Stetus == 1

                                select new
                                {
                                    id = a.Emp_Id,
                                    name = a.Tbl_EmployeeRegistration.Emp_Name,
                                    dep = a.Tbl_EmployeeRegistration.Department,
                                    from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                    to = Convert.ToDateTime(a.ToDate).Date,
                                    from2 = Convert.ToDateTime(a.From_Date).Date,
                                    type = a.LeaveType

                                }).ToList();
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    DateTime baseDate = DateTime.Today;
                    var today = baseDate;
                    var yesterday = baseDate.AddDays(-1);
                    var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
                    var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
                    var previousWeekStart = thisWeekStart.AddDays(-7);
                    var previousWeekEnd = thisWeekStart.AddDays(1);
                    var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
                    var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
                    var firstDayLastMonth = thisMonthStart.AddMonths(-1);
                    var lastDayLastMonth = thisMonthStart.AddSeconds(-1);
                    if (employee.dropdown == "-1")
                    {
                        var data = (from a in db.Tbl_EmpLeaves
                                    where a.Emp_Id == Convert.ToInt32(id)  && Convert.ToDateTime(a.From_Date).Date == DateTime.Now.Date.AddDays(-1) && a.Stetus == 1

                                    select new
                                    {
                                        id = a.Emp_Id,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        dep = a.Tbl_EmployeeRegistration.Department,
                                        from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                        to = Convert.ToDateTime(a.ToDate).Date,
                                        from2 = Convert.ToDateTime(a.From_Date).Date,
                                        type = a.LeaveType

                                    }).ToList();
                        return Json(data, JsonRequestBehavior.AllowGet);

                    }
                    else if (employee.dropdown == "1")
                    {
                        var data = (from a in db.Tbl_EmpLeaves
                                    where a.Emp_Id == Convert.ToInt32(id)  && Convert.ToDateTime(a.From_Date).Date == DateTime.Now.Date && a.Stetus == 1

                                    select new
                                    {
                                        id = a.Emp_Id,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        dep = a.Tbl_EmployeeRegistration.Department,
                                        from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                        to = Convert.ToDateTime(a.ToDate).Date,
                                        from2 = Convert.ToDateTime(a.From_Date).Date,
                                        type = a.LeaveType
                                    }).ToList();
                        return Json(data, JsonRequestBehavior.AllowGet);

                    }
                    else if (employee.dropdown == "7")
                    {
                        var data = (from a in db.Tbl_EmpLeaves
                                    where  a.Emp_Id == Convert.ToInt32(id) &&  Convert.ToDateTime(a.From_Date).Date >= thisWeekStart && Convert.ToDateTime(a.ToDate).Date <= thisWeekEnd && a.Stetus == 1

                                    select new
                                    {
                                        id = a.Emp_Id,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        dep = a.Tbl_EmployeeRegistration.Department,
                                        from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                        to = Convert.ToDateTime(a.ToDate).Date,
                                        from2 = Convert.ToDateTime(a.From_Date).Date,
                                        type = a.LeaveType

                                    }).ToList();
                        return Json(data, JsonRequestBehavior.AllowGet);

                    }
                    else if (employee.dropdown == "<8")
                    {
                        var data = (from a in db.Tbl_EmpLeaves
                                    where  a.Emp_Id == Convert.ToInt32(id) && Convert.ToDateTime(a.From_Date).Date >= previousWeekStart && Convert.ToDateTime(a.ToDate).Date <= previousWeekEnd && a.Stetus == 1

                                    select new
                                    {
                                        id = a.Emp_Id,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        dep = a.Tbl_EmployeeRegistration.Department,
                                        from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                        to = Convert.ToDateTime(a.ToDate).Date,
                                        from2 = Convert.ToDateTime(a.From_Date).Date,
                                        type = a.LeaveType

                                    }).ToList();
                        return Json(data, JsonRequestBehavior.AllowGet);

                    }
                    else if (employee.dropdown == "<30")
                    {
                        var data = (from a in db.Tbl_EmpLeaves
                                    where   a.Emp_Id == Convert.ToInt32(id) && Convert.ToDateTime(a.From_Date).Date >= firstDayLastMonth && Convert.ToDateTime(a.ToDate).Date <= lastDayLastMonth && a.Stetus==1
                                  
                                    select new
                                    {
                                        id = a.Emp_Id,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        dep = a.Tbl_EmployeeRegistration.Department,
                                        from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                        to = Convert.ToDateTime(a.ToDate).Date,
                                        from2 = Convert.ToDateTime(a.From_Date).Date,
                                        type = a.LeaveType

                                    }).ToList();
                        return Json(data, JsonRequestBehavior.AllowGet);

                    }
                    else if (employee.dropdown == "30")
                    {
                        var data = (from a in db.Tbl_EmpLeaves
                                    where a.Emp_Id == Convert.ToInt32(id) && Convert.ToDateTime(a.From_Date).Date >= thisMonthStart && Convert.ToDateTime(a.ToDate).Date <= thisMonthEnd&&a.Stetus==1
                                    
                                    select new
                                    {
                                        id = a.Emp_Id,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        dep = a.Tbl_EmployeeRegistration.Department,
                                        from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                        to = Convert.ToDateTime(a.ToDate).Date,
                                        from2 = Convert.ToDateTime(a.From_Date).Date,
                                        type=a.LeaveType

                                    }).ToList();
                        return Json(data, JsonRequestBehavior.AllowGet);

                    }
                    return Json(null, JsonRequestBehavior.AllowGet);
                }

            }
        }
        public JsonResult employeelist(EmployeeRegistration employee)
        {
            var data = (from a in db.Tbl_EmployeeRegistrations
                        select new
                        {
                            id = a.Emp_id,
                            name = a.Emp_Name,
                            doj = a.Emp_DOJ,
                            dep = a.Department,
                            des = a.Designation,
                            mob = a.Emp_Mobile,
                            salary=a.BasicSalary
                        }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult printemployeelist( string reporttype, string button)
        {

            var data = (from a in db.Tbl_EmployeeRegistrations
                        select new
                        {
                            id = a.Emp_id,
                            name = a.Emp_Name,
                            doj = a.Emp_DOJ,
                            dep = a.Department,
                            des = a.Designation,
                            mob = a.Emp_Mobile,
                            salary = a.BasicSalary
                        }).ToList();
          
            if (reporttype == "pdf")
                        {
                            if (button == "tsprint")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptEmployeeList.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("EmployeeList", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=EmployeeList." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            else
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptEmployeeList.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("EmployeeList", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "attachment;filename=EmployeeList." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                        }
                        else if (reporttype == "excel")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/RptEmployeeList.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("EmployeeList", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "Excel";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "xls";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=EmployeeList." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }
            return (null);
                      
                    }

                    public JsonResult getanniversary(EmployeeRegistration employee)
        {
            try
            {
                if (employee.EmpId != "" && employee.EmpId != null)
                {

                    if (employee.datefrom.ToString() != "01-01-01 00:00:00")
                    {
                        var data = (from a in db.Tbl_EmployeeRegistrations
                                    where a.Emp_id == Convert.ToInt32(employee.EmpId) && Convert.ToDateTime(a.Emp_DOJ).Month>=employee.datefrom.Month && Convert.ToDateTime(a.Emp_DOJ).Day >= employee.datefrom.Day&& Convert.ToDateTime(a.Emp_DOJ).Month <= employee.todate.Month && Convert.ToDateTime(a.Emp_DOJ).Day<= employee.todate.Day
                                    select new
                                    {
                                        id = a.Emp_id,
                                        name = a.Emp_Name,
                                        dep = a.Department,
                                        date = Convert.ToDateTime(a.Emp_DOJ).Date,
                                      

                                    }).ToList();
                        return Json(data, JsonRequestBehavior.AllowGet);
                      
                    }
                    else
                    {
                        DateTime baseDate = DateTime.Today;

                        var today = baseDate;
                        var yesterday = baseDate.AddDays(-1);
                        var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
                        var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
                        var previousWeekStart = thisWeekStart.AddDays(-7);
                        var previousWeekEnd = thisWeekStart.AddDays(1);
                        var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
                        var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
                        var firstDayLastMonth = thisMonthStart.AddMonths(-1);
                        var lastDayLastMonth = thisMonthStart.AddSeconds(-1);
                        if (employee.dropdown == "-1")
                        {
                           
                            var data = (from a in db.Tbl_EmployeeRegistrations
                                        where a.Emp_id == Convert.ToInt32(employee.EmpId) && Convert.ToDateTime(a.Emp_DOJ).Month >= DateTime.Now.AddDays(-1).Month && Convert.ToDateTime(a.Emp_DOJ).Day >= DateTime.Now.AddDays(-1).Day && Convert.ToDateTime(a.Emp_DOJ).Month <= DateTime.Now.AddDays(-1).Month && Convert.ToDateTime(a.Emp_DOJ).Day <= DateTime.Now.AddDays(-1).Day
                                        select new
                                        {
                                            id = a.Emp_id,
                                            name = a.Emp_Name,
                                            dep = a.Department,
                                            date = Convert.ToDateTime(a.Emp_DOJ).Date,


                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        else if (employee.dropdown == "1")
                        {
                           
                            var data = (from a in db.Tbl_EmployeeRegistrations
                                        where a.Emp_id == Convert.ToInt32(employee.EmpId) && Convert.ToDateTime(a.Emp_DOJ).Month >= DateTime.Now.Month && Convert.ToDateTime(a.Emp_DOJ).Day >= DateTime.Now.Day && Convert.ToDateTime(a.Emp_DOJ).Month <= DateTime.Now.Month && Convert.ToDateTime(a.Emp_DOJ).Day <= DateTime.Now.Day

                                        select new
                                        {
                                            id = a.Emp_id,
                                            name = a.Emp_Name,
                                            dep = a.Department,
                                            date = Convert.ToDateTime(a.Emp_DOJ).Date,


                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        else if (employee.dropdown == "7")
                        {

                            var data = (from a in db.Tbl_EmployeeRegistrations
                                        where a.Emp_id == Convert.ToInt32(employee.EmpId) && Convert.ToDateTime(a.Emp_DOJ).Month >= thisWeekStart.Month && Convert.ToDateTime(a.Emp_DOJ).Day >= thisWeekStart.Day && Convert.ToDateTime(a.Emp_DOJ).Month <= thisWeekEnd.Month && Convert.ToDateTime(a.Emp_DOJ).Day <= thisWeekEnd.Day
                                        select new
                                        {
                                            id = a.Emp_id,
                                            name = a.Emp_Name,
                                            dep = a.Department,
                                            date = Convert.ToDateTime(a.Emp_DOJ).Date,
                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        else if (employee.dropdown == "<8")
                        {
                            
                            var data = (from a in db.Tbl_EmployeeRegistrations
                                        where a.Emp_id == Convert.ToInt32(employee.EmpId) && Convert.ToDateTime(a.Emp_DOJ).Month >= previousWeekStart.Month && Convert.ToDateTime(a.Emp_DOJ).Day >= previousWeekStart.Day && Convert.ToDateTime(a.Emp_DOJ).Month <= previousWeekStart.Month && Convert.ToDateTime(a.Emp_DOJ).Day <= previousWeekEnd.Day
                                        select new
                                        {
                                            id = a.Emp_id,
                                            name = a.Emp_Name,
                                            dep = a.Department,
                                            date = Convert.ToDateTime(a.Emp_DOJ).Date,
                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        else if (employee.dropdown == "<30")
                        {
                          
                            var data = (from a in db.Tbl_EmployeeRegistrations
                                        where a.Emp_id == Convert.ToInt32(employee.EmpId) && Convert.ToDateTime(a.Emp_DOJ).Month >= firstDayLastMonth.Month && Convert.ToDateTime(a.Emp_DOJ).Day >= firstDayLastMonth.Day && Convert.ToDateTime(a.Emp_DOJ).Month <= lastDayLastMonth.Month && Convert.ToDateTime(a.Emp_DOJ).Day <= lastDayLastMonth.Day
                                        select new
                                        {
                                            id = a.Emp_id,
                                            name = a.Emp_Name,
                                            dep = a.Department,
                                            date = Convert.ToDateTime(a.Emp_DOJ).Date,
                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);


                        }
                        else if (employee.dropdown == "30")
                        {
                            
                            var data = (from a in db.Tbl_EmployeeRegistrations
                                        where a.Emp_id == Convert.ToInt32(employee.EmpId) && Convert.ToDateTime(a.Emp_DOJ).Month >= thisMonthStart.Month && Convert.ToDateTime(a.Emp_DOJ).Day >= thisMonthStart.Day && Convert.ToDateTime(a.Emp_DOJ).Month <= thisMonthEnd.Month && Convert.ToDateTime(a.Emp_DOJ).Day <= thisMonthEnd.Day
                                        select new
                                        {
                                            id = a.Emp_id,
                                            name = a.Emp_Name,
                                            dep = a.Department,
                                            date = Convert.ToDateTime(a.Emp_DOJ).Date,
                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    if (employee.datefrom.ToString() != "01-01-01 00:00:00")
                    {

                        var data = (from a in db.Tbl_EmployeeRegistrations
                                    where  Convert.ToDateTime(a.Emp_DOJ).Month >= employee.datefrom.Month && Convert.ToDateTime(a.Emp_DOJ).Day >= employee.datefrom.Day && Convert.ToDateTime(a.Emp_DOJ).Month <= employee.todate.Month && Convert.ToDateTime(a.Emp_DOJ).Day <= employee.todate.Day
                                    select new
                                    {
                                        id = a.Emp_id,
                                        name = a.Emp_Name,
                                        dep = a.Department,
                                        date = Convert.ToDateTime(a.Emp_DOJ).Date,


                                    }).ToList();
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        DateTime baseDate = DateTime.Today;

                        var today = baseDate;
                        var yesterday = baseDate.AddDays(-1);
                        var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
                        var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
                        var previousWeekStart = thisWeekStart.AddDays(-7);
                        var previousWeekEnd = thisWeekStart.AddDays(1);
                        var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
                        var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
                        var firstDayLastMonth = thisMonthStart.AddMonths(-1);
                        var lastDayLastMonth = thisMonthStart.AddSeconds(-1);
                        if (employee.dropdown == "-1")
                        {
                            var data = (from a in db.Tbl_EmployeeRegistrations
                                        where Convert.ToDateTime(a.Emp_DOJ).Month >= DateTime.Now.AddDays(-1).Month && Convert.ToDateTime(a.Emp_DOJ).Day >= DateTime.Now.AddDays(-1).Day && Convert.ToDateTime(a.Emp_DOJ).Month <= DateTime.Now.AddDays(-1).Month && Convert.ToDateTime(a.Emp_DOJ).Day <= DateTime.Now.AddDays(-1).Day
                                        select new
                                        {
                                            id = a.Emp_id,
                                            name = a.Emp_Name,
                                            dep = a.Department,
                                            date = Convert.ToDateTime(a.Emp_DOJ).Date,


                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);


                        }
                        else if (employee.dropdown == "1")
                        {
                            var data = (from a in db.Tbl_EmployeeRegistrations
                                        where Convert.ToDateTime(a.Emp_DOJ).Month >= DateTime.Now.Month && Convert.ToDateTime(a.Emp_DOJ).Day >= DateTime.Now.Day && Convert.ToDateTime(a.Emp_DOJ).Month <= DateTime.Now.Month && Convert.ToDateTime(a.Emp_DOJ).Day <= DateTime.Now.Day

                                        select new
                                        {
                                            id = a.Emp_id,
                                            name = a.Emp_Name,
                                            dep = a.Department,
                                            date = Convert.ToDateTime(a.Emp_DOJ).Date,


                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        else if (employee.dropdown == "7")
                        {

                            var data = (from a in db.Tbl_EmployeeRegistrations
                                        where Convert.ToDateTime(a.Emp_DOJ).Month >= thisWeekStart.Month && Convert.ToDateTime(a.Emp_DOJ).Day >= thisWeekStart.Day && Convert.ToDateTime(a.Emp_DOJ).Month <= thisWeekEnd.Month && Convert.ToDateTime(a.Emp_DOJ).Day <= thisWeekEnd.Day
                                        select new
                                        {
                                            id = a.Emp_id,
                                            name = a.Emp_Name,
                                            dep = a.Department,
                                            date = Convert.ToDateTime(a.Emp_DOJ).Date,
                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }
                        else if (employee.dropdown == "<8")
                        {
                            var data = (from a in db.Tbl_EmployeeRegistrations
                                        where  Convert.ToDateTime(a.Emp_DOJ).Month >= previousWeekStart.Month && Convert.ToDateTime(a.Emp_DOJ).Day >= previousWeekStart.Day && Convert.ToDateTime(a.Emp_DOJ).Month <= previousWeekStart.Month && Convert.ToDateTime(a.Emp_DOJ).Day <= previousWeekEnd.Day
                                        select new
                                        {
                                            id = a.Emp_id,
                                            name = a.Emp_Name,
                                            dep = a.Department,
                                            date = Convert.ToDateTime(a.Emp_DOJ).Date,
                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }
                        else if (employee.dropdown == "<30")
                        {
                            var data = (from a in db.Tbl_EmployeeRegistrations
                                        where Convert.ToDateTime(a.Emp_DOJ).Month >= firstDayLastMonth.Month && Convert.ToDateTime(a.Emp_DOJ).Day >= firstDayLastMonth.Day && Convert.ToDateTime(a.Emp_DOJ).Month <= lastDayLastMonth.Month && Convert.ToDateTime(a.Emp_DOJ).Day <= lastDayLastMonth.Day
                                        select new
                                        {
                                            id = a.Emp_id,
                                            name = a.Emp_Name,
                                            dep = a.Department,
                                            date = Convert.ToDateTime(a.Emp_DOJ).Date,
                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        else if (employee.dropdown == "30")
                        {
                            var data = (from a in db.Tbl_EmployeeRegistrations
                                        where  Convert.ToDateTime(a.Emp_DOJ).Month >= thisMonthStart.Month && Convert.ToDateTime(a.Emp_DOJ).Day >= thisMonthStart.Day && Convert.ToDateTime(a.Emp_DOJ).Month <= thisMonthEnd.Month && Convert.ToDateTime(a.Emp_DOJ).Day <= thisMonthEnd.Day
                                        select new
                                        {
                                            id = a.Emp_id,
                                            name = a.Emp_Name,
                                            dep = a.Department,
                                            date = Convert.ToDateTime(a.Emp_DOJ).Date,
                                        }).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                var id = (from b in db.Tbl_EmployeeRegistrations where b.Emp_Name.Trim() == employee.EmpId.Trim() select b.Emp_id).SingleOrDefault();

                if (employee.datefrom.ToString() != "01-01-01 00:00:00")
                {

                    var data = (from a in db.Tbl_EmployeeRegistrations
                                where a.Emp_id == Convert.ToInt32(id) && Convert.ToDateTime(a.Emp_DOJ).Month >= employee.datefrom.Month && Convert.ToDateTime(a.Emp_DOJ).Day >= employee.datefrom.Day && Convert.ToDateTime(a.Emp_DOJ).Month <= employee.todate.Month && Convert.ToDateTime(a.Emp_DOJ).Day <= employee.todate.Day
                                select new
                                {
                                    id = a.Emp_id,
                                    name = a.Emp_Name,
                                    dep = a.Department,
                                    date = Convert.ToDateTime(a.Emp_DOJ).Date,


                                }).ToList();
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    DateTime baseDate = DateTime.Today;

                    var today = baseDate;
                    var yesterday = baseDate.AddDays(-1);
                    var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
                    var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
                    var previousWeekStart = thisWeekStart.AddDays(-7);
                    var previousWeekEnd = thisWeekStart.AddDays(1);
                    var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
                    var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
                    var firstDayLastMonth = thisMonthStart.AddMonths(-1);
                    var lastDayLastMonth = thisMonthStart.AddSeconds(-1);
                    if (employee.dropdown == "-1")
                    {
                        var data = (from a in db.Tbl_EmployeeRegistrations
                                    where a.Emp_id == Convert.ToInt32(id) && Convert.ToDateTime(a.Emp_DOJ).Month >= DateTime.Now.AddDays(-1).Month && Convert.ToDateTime(a.Emp_DOJ).Day >= DateTime.Now.AddDays(-1).Day && Convert.ToDateTime(a.Emp_DOJ).Month <= DateTime.Now.AddDays(-1).Month && Convert.ToDateTime(a.Emp_DOJ).Day <= DateTime.Now.AddDays(-1).Day
                                    select new
                                    {
                                        id = a.Emp_id,
                                        name = a.Emp_Name,
                                        dep = a.Department,
                                        date = Convert.ToDateTime(a.Emp_DOJ).Date,


                                    }).ToList();
                        return Json(data, JsonRequestBehavior.AllowGet);

                    }
                    else if (employee.dropdown == "1")
                    {
                        var data = (from a in db.Tbl_EmployeeRegistrations
                                    where a.Emp_id == Convert.ToInt32(id) && Convert.ToDateTime(a.Emp_DOJ).Month >= DateTime.Now.Month && Convert.ToDateTime(a.Emp_DOJ).Day >= DateTime.Now.Day && Convert.ToDateTime(a.Emp_DOJ).Month <= DateTime.Now.Month && Convert.ToDateTime(a.Emp_DOJ).Day <= DateTime.Now.Day

                                    select new
                                    {
                                        id = a.Emp_id,
                                        name = a.Emp_Name,
                                        dep = a.Department,
                                        date = Convert.ToDateTime(a.Emp_DOJ).Date,


                                    }).ToList();
                        return Json(data, JsonRequestBehavior.AllowGet);

                    }
                    else if (employee.dropdown == "7")
                    {
                        var data = (from a in db.Tbl_EmployeeRegistrations
                                    where a.Emp_id == Convert.ToInt32(id) && Convert.ToDateTime(a.Emp_DOJ).Month >= thisWeekStart.Month && Convert.ToDateTime(a.Emp_DOJ).Day >= thisWeekStart.Day && Convert.ToDateTime(a.Emp_DOJ).Month <= thisWeekEnd.Month && Convert.ToDateTime(a.Emp_DOJ).Day <= thisWeekEnd.Day
                                    select new
                                    {
                                        id = a.Emp_id,
                                        name = a.Emp_Name,
                                        dep = a.Department,
                                        date = Convert.ToDateTime(a.Emp_DOJ).Date,
                                    }).ToList();
                        return Json(data, JsonRequestBehavior.AllowGet);

                    }
                    else if (employee.dropdown == "<8")
                    {
                        var data = (from a in db.Tbl_EmployeeRegistrations
                                    where a.Emp_id == Convert.ToInt32(id) && Convert.ToDateTime(a.Emp_DOJ).Month >= previousWeekStart.Month && Convert.ToDateTime(a.Emp_DOJ).Day >= previousWeekStart.Day && Convert.ToDateTime(a.Emp_DOJ).Month <= previousWeekStart.Month && Convert.ToDateTime(a.Emp_DOJ).Day <= previousWeekEnd.Day
                                    select new
                                    {
                                        id = a.Emp_id,
                                        name = a.Emp_Name,
                                        dep = a.Department,
                                        date = Convert.ToDateTime(a.Emp_DOJ).Date,
                                    }).ToList();
                        return Json(data, JsonRequestBehavior.AllowGet);

                    }
                    else if (employee.dropdown == "<30")
                    {
                        var data = (from a in db.Tbl_EmployeeRegistrations
                                    where a.Emp_id == Convert.ToInt32(id) && Convert.ToDateTime(a.Emp_DOJ).Month >= firstDayLastMonth.Month && Convert.ToDateTime(a.Emp_DOJ).Day >= firstDayLastMonth.Day && Convert.ToDateTime(a.Emp_DOJ).Month <= lastDayLastMonth.Month && Convert.ToDateTime(a.Emp_DOJ).Day <= lastDayLastMonth.Day
                                    select new
                                    {
                                        id = a.Emp_id,
                                        name = a.Emp_Name,
                                        dep = a.Department,
                                        date = Convert.ToDateTime(a.Emp_DOJ).Date,
                                    }).ToList();
                        return Json(data, JsonRequestBehavior.AllowGet);

                    }
                    else if (employee.dropdown == "30")
                    {
                        var data = (from a in db.Tbl_EmployeeRegistrations
                                    where a.Emp_id == Convert.ToInt32(id) && Convert.ToDateTime(a.Emp_DOJ).Month >= thisMonthStart.Month && Convert.ToDateTime(a.Emp_DOJ).Day >= thisMonthStart.Day && Convert.ToDateTime(a.Emp_DOJ).Month <= thisMonthEnd.Month && Convert.ToDateTime(a.Emp_DOJ).Day <= thisMonthEnd.Day
                                    select new
                                    {
                                        id = a.Emp_id,
                                        name = a.Emp_Name,
                                        dep = a.Department,
                                        date = Convert.ToDateTime(a.Emp_DOJ).Date,
                                    }).ToList();
                        return Json(data, JsonRequestBehavior.AllowGet);

                    }
                    return Json(null, JsonRequestBehavior.AllowGet);
                }

            }
        }
        public ActionResult printleaves(string EmpId, string Emp_Name, DateTime datefrom, DateTime todate, string dropdown, string reporttype, string button)
        {
            try
            {
                if (EmpId != "" && EmpId != null)
                {

                    if (datefrom.ToString() != "01-01-01 00:00:00")
                    {
                        var data = (from a in db.Tbl_EmpLeaves
                                    where a.Emp_Id == Convert.ToInt32(EmpId) && Convert.ToDateTime(a.From_Date).Date >= datefrom.Date && Convert.ToDateTime(a.ToDate).Date <= todate.Date && a.Stetus == 1

                                    select new
                                    {
                                        id = a.Emp_Id,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        dep = a.Tbl_EmployeeRegistration.Department,
                                        from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                        to = Convert.ToDateTime(a.ToDate).Date,

                                    }).ToList();

                        if (reporttype == "pdf")
                        {
                            if (button == "tsprint")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=Leaves." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            else
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "attachment;filename=Leaves." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                        }
                        else if (reporttype == "excel")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "Excel";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "xls";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=Leaves." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }

                        return (null);
                    }
                    else
                    {
                        DateTime baseDate = DateTime.Today;

                        var today = baseDate;
                        var yesterday = baseDate.AddDays(-1);
                        var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
                        var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
                        var previousWeekStart = thisWeekStart.AddDays(-7);
                        var previousWeekEnd = thisWeekStart.AddDays(1);
                        var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
                        var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
                        var firstDayLastMonth = thisMonthStart.AddMonths(-1);
                        var lastDayLastMonth = thisMonthStart.AddSeconds(-1);
                        if (dropdown == "-1")
                        {
                            var data = (from a in db.Tbl_EmpLeaves
                                        where a.Emp_Id == Convert.ToInt32(EmpId) && Convert.ToDateTime(a.From_Date).Date == DateTime.Now.Date.AddDays(-1) && a.Stetus == 1

                                        select new
                                        {

                                            id = a.Emp_Id,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                            to = Convert.ToDateTime(a.ToDate).Date,
                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=Leaves." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=Leaves." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=Leaves." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }

                            return (null);

                        }
                        else if (dropdown == "1")
                        {
                            var data = (from a in db.Tbl_EmpLeaves
                                        where a.Emp_Id == Convert.ToInt32(EmpId) && Convert.ToDateTime(a.From_Date).Date == DateTime.Now.Date && a.Stetus == 1

                                        select new
                                        {

                                            id = a.Emp_Id,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                            to = Convert.ToDateTime(a.ToDate).Date,
                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=Leaves." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=Leaves." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=Leaves." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }

                            return (null);

                        }
                        else if (dropdown == "7")
                        {
                            var data = (from a in db.Tbl_EmpLeaves
                                        where a.Emp_Id == Convert.ToInt32(EmpId) && Convert.ToDateTime(a.From_Date).Date >= thisWeekStart && Convert.ToDateTime(a.ToDate).Date <= thisWeekEnd && a.Stetus == 1

                                        select new
                                        {

                                            id = a.Emp_Id,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                            to = Convert.ToDateTime(a.ToDate).Date,
                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=Leaves." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=Leaves." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=Leaves." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }

                            return (null);

                        }
                        else if (dropdown == "<8")
                        {
                            var data = (from a in db.Tbl_EmpLeaves
                                        where a.Emp_Id == Convert.ToInt32(EmpId) && Convert.ToDateTime(a.From_Date).Date >= previousWeekStart && Convert.ToDateTime(a.ToDate).Date <= previousWeekEnd && a.Stetus == 1

                                        select new
                                        {

                                            id = a.Emp_Id,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,

                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=Leaves." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=Leaves." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=Leaves." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }

                            return (null);

                        }
                        else if (dropdown == "<30")
                        {
                            var data = (from a in db.Tbl_EmpLeaves
                                        where a.Emp_Id == Convert.ToInt32(EmpId) && Convert.ToDateTime(a.From_Date).Date >= firstDayLastMonth && Convert.ToDateTime(a.ToDate).Date <= lastDayLastMonth && a.Stetus == 1

                                        select new
                                        {


                                            id = a.Emp_Id,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                            to = Convert.ToDateTime(a.ToDate).Date,
                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=Leaves." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=Leaves." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=Leaves." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }

                            return (null);

                        }
                        else if (dropdown == "30")
                        {
                            var data = (from a in db.Tbl_EmpLeaves
                                        where a.Emp_Id == Convert.ToInt32(EmpId) && Convert.ToDateTime(a.From_Date).Date >= thisMonthStart && Convert.ToDateTime(a.ToDate).Date <= thisMonthEnd
                                       && a.Stetus == 1
                                        select new
                                        {

                                            id = a.Emp_Id,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays+1,
                                            to = Convert.ToDateTime(a.ToDate).Date,

                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=Leaves." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=Leaves." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=Leaves." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }

                            return (null);

                        }
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    if (datefrom.ToString() != "01-01-01 00:00:00")
                    {
                        var data = (from a in db.Tbl_EmpLeaves
                                    where Convert.ToDateTime(a.From_Date).Date >= datefrom.Date && Convert.ToDateTime(a.ToDate).Date <= todate.Date
                                  && a.Stetus == 1
                                    select new
                                    {

                                        id = a.Emp_Id,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        dep = a.Tbl_EmployeeRegistration.Department,
                                        from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                        to = Convert.ToDateTime(a.ToDate).Date,

                                    }).ToList();
                        if (reporttype == "pdf")
                        {
                            if (button == "tsprint")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=Leaves." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            else
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "attachment;filename=Leaves." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                        }
                        else if (reporttype == "excel")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "Excel";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "xls";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=Leaves." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }

                        return (null);
                    }
                    else
                    {
                        DateTime baseDate = DateTime.Today;

                        var today = baseDate;
                        var yesterday = baseDate.AddDays(-1);
                        var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
                        var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
                        var previousWeekStart = thisWeekStart.AddDays(-7);
                        var previousWeekEnd = thisWeekStart.AddDays(1);
                        var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
                        var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
                        var firstDayLastMonth = thisMonthStart.AddMonths(-1);
                        var lastDayLastMonth = thisMonthStart.AddSeconds(-1);
                        if (dropdown == "-1")
                        {
                            var data = (from a in db.Tbl_EmpLeaves
                                        where Convert.ToDateTime(a.From_Date).Date == DateTime.Now.Date.AddDays(-1)
                                      && a.Stetus == 1
                                        select new
                                        {

                                            id = a.Emp_Id,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                            to = Convert.ToDateTime(a.ToDate).Date,

                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=Leaves." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=Leaves." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=Leaves." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }

                            return (null);

                        }
                        else if (dropdown == "1")
                        {
                            var data = (from a in db.Tbl_EmpLeaves
                                        where Convert.ToDateTime(a.From_Date).Date == DateTime.Now.Date
                                        && a.Stetus == 1
                                        select new
                                        {

                                            id = a.Emp_Id,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                            to = Convert.ToDateTime(a.ToDate).Date,

                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=Leaves." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=Leaves." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=Leaves." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }

                            return (null);

                        }
                        else if (dropdown == "7")
                        {
                            var data = (from a in db.Tbl_EmpLeaves
                                        where Convert.ToDateTime(a.From_Date).Date >= thisWeekStart && Convert.ToDateTime(a.ToDate).Date <= thisWeekEnd
                                       && a.Stetus == 1
                                        select new
                                        {

                                            id = a.Emp_Id,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                            to = Convert.ToDateTime(a.ToDate).Date,

                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=Leaves." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=Leaves." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=Leaves." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }

                            return (null);

                        }
                        else if (dropdown == "<8")
                        {
                            var data = (from a in db.Tbl_EmpLeaves
                                        where Convert.ToDateTime(a.From_Date).Date >= previousWeekStart && Convert.ToDateTime(a.ToDate).Date <= previousWeekEnd
                                      && a.Stetus == 1
                                        select new
                                        {
                                            id = a.Emp_Id,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                            to = Convert.ToDateTime(a.ToDate).Date,
                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=Leaves." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=Leaves." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=Leaves." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }

                            return (null);

                        }
                        else if (dropdown == "<30")
                        {
                            var data = (from a in db.Tbl_EmpLeaves
                                        where Convert.ToDateTime(a.From_Date).Date >= firstDayLastMonth && Convert.ToDateTime(a.ToDate).Date <= lastDayLastMonth
                                       && a.Stetus == 1
                                        select new
                                        {
                                            id = a.Emp_Id,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                            to = Convert.ToDateTime(a.ToDate).Date,

                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=Leaves." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=Leaves." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=Leaves." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }

                            return (null);

                        }
                        else if (dropdown == "30")
                        {
                            var data = (from a in db.Tbl_EmpLeaves
                                        where Convert.ToDateTime(a.From_Date).Date >= thisMonthStart && Convert.ToDateTime(a.ToDate).Date <= thisMonthEnd
                                      && a.Stetus == 1
                                        select new
                                        {
                                            id = a.Emp_Id,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                            to = Convert.ToDateTime(a.ToDate).Date,

                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=Leaves." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=Leaves." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=Leaves." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }

                            return (null);

                        }
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                var id = (from b in db.Tbl_EmployeeRegistrations where b.Emp_Name.Trim() == EmpId.Trim() select b.Emp_id).SingleOrDefault();

                if (datefrom.ToString() != "01-01-01 00:00:00")
                {

                    var data = (from a in db.Tbl_EmpLeaves
                                where a.Emp_Id == Convert.ToInt32(id) && Convert.ToDateTime(a.From_Date).Date >= datefrom.Date && Convert.ToDateTime(a.ToDate).Date <= todate.Date && a.Stetus == 1

                                select new
                                {
                                    id = a.Emp_Id,
                                    name = a.Tbl_EmployeeRegistration.Emp_Name,
                                    dep = a.Tbl_EmployeeRegistration.Department,
                                    from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                    to = Convert.ToDateTime(a.ToDate).Date,

                                }).ToList();
                    if (reporttype == "pdf")
                    {
                        if (button == "tsprint")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "pdf";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "pdf";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=Leaves." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }
                        else
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "pdf";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "pdf";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "attachment;filename=Leaves." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }
                    }
                    else if (reporttype == "excel")
                    {
                        LocalReport lc1 = new LocalReport();
                        lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                        ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                        rd1.Value = data;
                        lc1.DataSources.Add(rd1);
                        reporttype = "Excel";
                        string mimetype1;
                        string encoding1;
                        string filenameextention1;


                        filenameextention1 = "xls";
                        string[] strems1;
                        Warning[] warnings1;
                        byte[] renderdbyte1;
                        renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                            , out strems1, out warnings1);
                        Response.AddHeader("content-disposition", "inline;filename=Leaves." + filenameextention1);
                        return File(renderdbyte1, "." + filenameextention1);
                    }

                    return (null);
                }
                else
                {
                    DateTime baseDate = DateTime.Today;

                    var today = baseDate;
                    var yesterday = baseDate.AddDays(-1);
                    var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
                    var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
                    var previousWeekStart = thisWeekStart.AddDays(-7);
                    var previousWeekEnd = thisWeekStart.AddDays(1);
                    var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
                    var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
                    var firstDayLastMonth = thisMonthStart.AddMonths(-1);
                    var lastDayLastMonth = thisMonthStart.AddSeconds(-1);
                    if (dropdown == "-1")
                    {
                        var data = (from a in db.Tbl_EmpLeaves
                                    where a.Emp_Id == Convert.ToInt32(id) && Convert.ToDateTime(a.From_Date).Date == DateTime.Now.Date.AddDays(-1) && a.Stetus == 1

                                    select new
                                    {
                                        id = a.Emp_Id,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        dep = a.Tbl_EmployeeRegistration.Department,
                                        from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                        to = Convert.ToDateTime(a.ToDate).Date,

                                    }).ToList();
                        if (reporttype == "pdf")
                        {
                            if (button == "tsprint")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=Leaves." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            else
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "attachment;filename=Leaves." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                        }
                        else if (reporttype == "excel")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "Excel";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "xls";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=Leaves." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }

                        return (null);

                    }
                    else if (dropdown == "1")
                    {
                        var data = (from a in db.Tbl_EmpLeaves
                                    where a.Emp_Id == Convert.ToInt32(id) && Convert.ToDateTime(a.From_Date).Date == DateTime.Now.Date && a.Stetus == 1

                                    select new
                                    {
                                        id = a.Emp_Id,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        dep = a.Tbl_EmployeeRegistration.Department,
                                        from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                        to = Convert.ToDateTime(a.ToDate).Date,
                                    }).ToList();
                        if (reporttype == "pdf")
                        {
                            if (button == "tsprint")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=Leaves." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            else
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "attachment;filename=Leaves." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                        }
                        else if (reporttype == "excel")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "Excel";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "xls";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=Leaves." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }

                        return (null);

                    }
                    else if (dropdown == "7")
                    {
                        var data = (from a in db.Tbl_EmpLeaves
                                    where a.Emp_Id == Convert.ToInt32(id) && Convert.ToDateTime(a.From_Date).Date >= thisWeekStart && Convert.ToDateTime(a.ToDate).Date <= thisWeekEnd && a.Stetus == 1

                                    select new
                                    {
                                        id = a.Emp_Id,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        dep = a.Tbl_EmployeeRegistration.Department,
                                        from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                        to = Convert.ToDateTime(a.ToDate).Date,

                                    }).ToList();
                        if (reporttype == "pdf")
                        {
                            if (button == "tsprint")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=Leaves." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            else
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "attachment;filename=Leaves." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                        }
                        else if (reporttype == "excel")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "Excel";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "xls";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=Leaves." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }

                        return (null);

                    }
                    else if (dropdown == "<8")
                    {
                        var data = (from a in db.Tbl_EmpLeaves
                                    where a.Emp_Id == Convert.ToInt32(id) && Convert.ToDateTime(a.From_Date).Date >= previousWeekStart && Convert.ToDateTime(a.ToDate).Date <= previousWeekEnd && a.Stetus == 1

                                    select new
                                    {
                                        id = a.Emp_Id,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        dep = a.Tbl_EmployeeRegistration.Department,
                                        from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                        to = Convert.ToDateTime(a.ToDate).Date,

                                    }).ToList();
                        if (reporttype == "pdf")
                        {
                            if (button == "tsprint")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=Leaves." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            else
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "attachment;filename=Leaves." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                        }
                        else if (reporttype == "excel")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "Excel";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "xls";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=Leaves." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }

                        return (null);

                    }
                    else if (dropdown == "<30")
                    {
                        var data = (from a in db.Tbl_EmpLeaves
                                    where a.Emp_Id == Convert.ToInt32(id) && Convert.ToDateTime(a.From_Date).Date >= firstDayLastMonth && Convert.ToDateTime(a.ToDate).Date <= lastDayLastMonth && a.Stetus == 1

                                    select new
                                    {
                                        id = a.Emp_Id,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        dep = a.Tbl_EmployeeRegistration.Department,
                                        from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                        to = Convert.ToDateTime(a.ToDate).Date,

                                    }).ToList();
                        if (reporttype == "pdf")
                        {
                            if (button == "tsprint")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=Leaves." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            else
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "attachment;filename=Leaves." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                        }
                        else if (reporttype == "excel")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "Excel";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "xls";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=Leaves." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }

                        return (null);

                    }
                    else if (dropdown == "30")
                    {
                        var data = (from a in db.Tbl_EmpLeaves
                                    where a.Emp_Id == Convert.ToInt32(id) && Convert.ToDateTime(a.From_Date).Date >= thisMonthStart && Convert.ToDateTime(a.ToDate).Date <= thisMonthEnd && a.Stetus == 1

                                    select new
                                    {
                                        id = a.Emp_Id,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        dep = a.Tbl_EmployeeRegistration.Department,
                                        from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                        to = Convert.ToDateTime(a.ToDate).Date,

                                    }).ToList();
                        if (reporttype == "pdf")
                        {
                            if (button == "tsprint")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=Leaves." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            else
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "attachment;filename=Leaves." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                        }
                        else if (reporttype == "excel")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/RptLeaves.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "Excel";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "xls";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=Leaves." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }

                        return (null);

                    }
                    return Json(null, JsonRequestBehavior.AllowGet);
                }

            }
        }
        public ActionResult printleavessummary(string EmpId, string Emp_Name, DateTime datefrom, DateTime todate, string dropdown, string reporttype, string button)
        {
            try
            {
                if (EmpId != "" && EmpId != null)
                {

                    if (datefrom.ToString() != "01-01-01 00:00:00")
                    {
                        var data = (from a in db.Tbl_EmpLeaves
                                    where a.Emp_Id == Convert.ToInt32(EmpId) && Convert.ToDateTime(a.From_Date).Date >= datefrom.Date && Convert.ToDateTime(a.ToDate).Date <= todate.Date && a.Stetus == 1

                                    select new
                                    {
                                        id = a.Emp_Id,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        dep = a.Tbl_EmployeeRegistration.Department,
                                        from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                        to = Convert.ToDateTime(a.ToDate).Date,
                                        from2 = Convert.ToDateTime(a.From_Date).Date,
                                        type = a.LeaveType

                                    }).ToList();

                        if (reporttype == "pdf")
                        {
                            if (button == "tsprint")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=Leavesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            else
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "attachment;filename=Leavesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                        }
                        else if (reporttype == "excel")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "Excel";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "xls";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=Leavesummery." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }

                        return (null);
                    }
                    else
                    {
                        DateTime baseDate = DateTime.Today;

                        var today = baseDate;
                        var yesterday = baseDate.AddDays(-1);
                        var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
                        var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
                        var previousWeekStart = thisWeekStart.AddDays(-7);
                        var previousWeekEnd = thisWeekStart.AddDays(1);
                        var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
                        var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
                        var firstDayLastMonth = thisMonthStart.AddMonths(-1);
                        var lastDayLastMonth = thisMonthStart.AddSeconds(-1);
                        if (dropdown == "-1")
                        {
                            var data = (from a in db.Tbl_EmpLeaves
                                        where a.Emp_Id == Convert.ToInt32(EmpId) && Convert.ToDateTime(a.From_Date).Date == DateTime.Now.Date.AddDays(-1) && a.Stetus == 1

                                        select new
                                        {

                                            id = a.Emp_Id,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                            to = Convert.ToDateTime(a.ToDate).Date,
                                            from2 = Convert.ToDateTime(a.From_Date).Date,
                                            type = a.LeaveType
                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=Leavesummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=Leavesummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=Leavesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }

                            return (null);

                        }
                        else if (dropdown == "1")
                        {
                            var data = (from a in db.Tbl_EmpLeaves
                                        where a.Emp_Id == Convert.ToInt32(EmpId) && Convert.ToDateTime(a.From_Date).Date == DateTime.Now.Date && a.Stetus == 1

                                        select new
                                        {

                                            id = a.Emp_Id,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                            to = Convert.ToDateTime(a.ToDate).Date,
                                            from2 = Convert.ToDateTime(a.From_Date).Date,
                                            type = a.LeaveType
                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=Leavesummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=Leavesummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=Leavesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }

                            return (null);

                        }
                        else if (dropdown == "7")
                        {
                            var data = (from a in db.Tbl_EmpLeaves
                                        where a.Emp_Id == Convert.ToInt32(EmpId) && Convert.ToDateTime(a.From_Date).Date >= thisWeekStart && Convert.ToDateTime(a.ToDate).Date <= thisWeekEnd && a.Stetus == 1

                                        select new
                                        {

                                            id = a.Emp_Id,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                            to = Convert.ToDateTime(a.ToDate).Date,
                                            from2 = Convert.ToDateTime(a.From_Date).Date,
                                            type = a.LeaveType
                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=Leavesummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=Leavesummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=Leavesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }

                            return (null);

                        }
                        else if (dropdown == "<8")
                        {
                            var data = (from a in db.Tbl_EmpLeaves
                                        where a.Emp_Id == Convert.ToInt32(EmpId) && Convert.ToDateTime(a.From_Date).Date >= previousWeekStart && Convert.ToDateTime(a.ToDate).Date <= previousWeekEnd && a.Stetus == 1

                                        select new
                                        {

                                            id = a.Emp_Id,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                            to = Convert.ToDateTime(a.ToDate).Date,
                                            from2 = Convert.ToDateTime(a.From_Date).Date,
                                            type = a.LeaveType
                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=Leavesummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=Leavesummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=Leavesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }

                            return (null);

                        }
                        else if (dropdown == "<30")
                        {
                            var data = (from a in db.Tbl_EmpLeaves
                                        where a.Emp_Id == Convert.ToInt32(EmpId) && Convert.ToDateTime(a.From_Date).Date >= firstDayLastMonth && Convert.ToDateTime(a.ToDate).Date <= lastDayLastMonth && a.Stetus == 1

                                        select new
                                        {


                                            id = a.Emp_Id,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                            to = Convert.ToDateTime(a.ToDate).Date,
                                            from2 = Convert.ToDateTime(a.From_Date).Date,
                                            type = a.LeaveType
                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=Leavesummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=Leavesummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=Leavesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }

                            return (null);

                        }
                        else if (dropdown == "30")
                        {
                            var data = (from a in db.Tbl_EmpLeaves
                                        where a.Emp_Id == Convert.ToInt32(EmpId) && Convert.ToDateTime(a.From_Date).Date >= thisMonthStart && Convert.ToDateTime(a.ToDate).Date <= thisMonthEnd
                                       && a.Stetus == 1
                                        select new
                                        {

                                            id = a.Emp_Id,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                            to = Convert.ToDateTime(a.ToDate).Date,
                                            from2 = Convert.ToDateTime(a.From_Date).Date,
                                            type = a.LeaveType

                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=Leavesummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=Leavesummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=Leavesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }

                            return (null);

                        }
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    if (datefrom.ToString() != "01-01-01 00:00:00")
                    {
                        var data = (from a in db.Tbl_EmpLeaves
                                    where Convert.ToDateTime(a.From_Date).Date >= datefrom.Date && Convert.ToDateTime(a.ToDate).Date <= todate.Date
                                  && a.Stetus == 1
                                    select new
                                    {

                                        id = a.Emp_Id,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        dep = a.Tbl_EmployeeRegistration.Department,
                                        from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                        to = Convert.ToDateTime(a.ToDate).Date,
                                        from2 = Convert.ToDateTime(a.From_Date).Date,
                                        type = a.LeaveType

                                    }).ToList();
                        if (reporttype == "pdf")
                        {
                            if (button == "tsprint")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=Leavesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            else
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "attachment;filename=Leavesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                        }
                        else if (reporttype == "excel")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "Excel";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "xls";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=Leavesummery." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }

                        return (null);
                    }
                    else
                    {
                        DateTime baseDate = DateTime.Today;

                        var today = baseDate;
                        var yesterday = baseDate.AddDays(-1);
                        var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
                        var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
                        var previousWeekStart = thisWeekStart.AddDays(-7);
                        var previousWeekEnd = thisWeekStart.AddDays(1);
                        var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
                        var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
                        var firstDayLastMonth = thisMonthStart.AddMonths(-1);
                        var lastDayLastMonth = thisMonthStart.AddSeconds(-1);
                        if (dropdown == "-1")
                        {
                            var data = (from a in db.Tbl_EmpLeaves
                                        where Convert.ToDateTime(a.From_Date).Date == DateTime.Now.Date.AddDays(-1)
                                      && a.Stetus == 1
                                        select new
                                        {

                                            id = a.Emp_Id,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                            to = Convert.ToDateTime(a.ToDate).Date,
                                            from2 = Convert.ToDateTime(a.From_Date).Date,
                                            type = a.LeaveType
                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=Leavesummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=Leavesummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=Leavesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }

                            return (null);

                        }
                        else if (dropdown == "1")
                        {
                            var data = (from a in db.Tbl_EmpLeaves
                                        where Convert.ToDateTime(a.From_Date).Date == DateTime.Now.Date
                                        && a.Stetus == 1
                                        select new
                                        {

                                            id = a.Emp_Id,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                            to = Convert.ToDateTime(a.ToDate).Date,
                                            from2 = Convert.ToDateTime(a.From_Date).Date,
                                            type = a.LeaveType
                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=Leavesummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=Leavesummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=Leavesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }

                            return (null);

                        }
                        else if (dropdown == "7")
                        {
                            var data = (from a in db.Tbl_EmpLeaves
                                        where Convert.ToDateTime(a.From_Date).Date >= thisWeekStart && Convert.ToDateTime(a.ToDate).Date <= thisWeekEnd
                                       && a.Stetus == 1
                                        select new
                                        {

                                            id = a.Emp_Id,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                            to = Convert.ToDateTime(a.ToDate).Date,
                                            from2 = Convert.ToDateTime(a.From_Date).Date,
                                            type = a.LeaveType
                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=Leavesummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=Leavesummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=Leavesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }

                            return (null);

                        }
                        else if (dropdown == "<8")
                        {
                            var data = (from a in db.Tbl_EmpLeaves
                                        where Convert.ToDateTime(a.From_Date).Date >= previousWeekStart && Convert.ToDateTime(a.ToDate).Date <= previousWeekEnd
                                      && a.Stetus == 1
                                        select new
                                        {
                                            id = a.Emp_Id,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                            to = Convert.ToDateTime(a.ToDate).Date,
                                            from2 = Convert.ToDateTime(a.From_Date).Date,
                                            type = a.LeaveType
                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=Leavesummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=Leavesummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=Leavesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }

                            return (null);

                        }
                        else if (dropdown == "<30")
                        {
                            var data = (from a in db.Tbl_EmpLeaves
                                        where Convert.ToDateTime(a.From_Date).Date >= firstDayLastMonth && Convert.ToDateTime(a.ToDate).Date <= lastDayLastMonth
                                       && a.Stetus == 1
                                        select new
                                        {
                                            id = a.Emp_Id,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                            to = Convert.ToDateTime(a.ToDate).Date,
                                            from2 = Convert.ToDateTime(a.From_Date).Date,
                                            type = a.LeaveType

                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=Leavesummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=Leavesummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=Leavesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }

                            return (null);

                        }
                        else if (dropdown == "30")
                        {
                            var data = (from a in db.Tbl_EmpLeaves
                                        where Convert.ToDateTime(a.From_Date).Date >= thisMonthStart && Convert.ToDateTime(a.ToDate).Date <= thisMonthEnd
                                      && a.Stetus == 1
                                        select new
                                        {
                                            id = a.Emp_Id,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department,
                                            from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                            to = Convert.ToDateTime(a.ToDate).Date,
                                            from2 = Convert.ToDateTime(a.From_Date).Date,
                                            type = a.LeaveType

                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=Leavesummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=Leavesummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=Leavesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }

                            return (null);

                        }
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                var id = (from b in db.Tbl_EmployeeRegistrations where b.Emp_Name.Trim() == EmpId.Trim() select b.Emp_id).SingleOrDefault();

                if (datefrom.ToString() != "01-01-01 00:00:00")
                {

                    var data = (from a in db.Tbl_EmpLeaves
                                where a.Emp_Id == Convert.ToInt32(id) && Convert.ToDateTime(a.From_Date).Date >= datefrom.Date && Convert.ToDateTime(a.ToDate).Date <= todate.Date && a.Stetus == 1

                                select new
                                {
                                    id = a.Emp_Id,
                                    name = a.Tbl_EmployeeRegistration.Emp_Name,
                                    dep = a.Tbl_EmployeeRegistration.Department,
                                    from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                    to = Convert.ToDateTime(a.ToDate).Date,
                                    from2 = Convert.ToDateTime(a.From_Date).Date,
                                    type = a.LeaveType

                                }).ToList();
                    if (reporttype == "pdf")
                    {
                        if (button == "tsprint")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "pdf";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "pdf";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=Leavesummery." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }
                        else
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "pdf";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "pdf";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "attachment;filename=Leavesummery." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }
                    }
                    else if (reporttype == "excel")
                    {
                        LocalReport lc1 = new LocalReport();
                        lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                        ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                        rd1.Value = data;
                        lc1.DataSources.Add(rd1);
                        reporttype = "Excel";
                        string mimetype1;
                        string encoding1;
                        string filenameextention1;


                        filenameextention1 = "xls";
                        string[] strems1;
                        Warning[] warnings1;
                        byte[] renderdbyte1;
                        renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                            , out strems1, out warnings1);
                        Response.AddHeader("content-disposition", "inline;filename=Leavesummery." + filenameextention1);
                        return File(renderdbyte1, "." + filenameextention1);
                    }

                    return (null);
                }
                else
                {
                    DateTime baseDate = DateTime.Today;

                    var today = baseDate;
                    var yesterday = baseDate.AddDays(-1);
                    var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
                    var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
                    var previousWeekStart = thisWeekStart.AddDays(-7);
                    var previousWeekEnd = thisWeekStart.AddDays(1);
                    var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
                    var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
                    var firstDayLastMonth = thisMonthStart.AddMonths(-1);
                    var lastDayLastMonth = thisMonthStart.AddSeconds(-1);
                    if (dropdown == "-1")
                    {
                        var data = (from a in db.Tbl_EmpLeaves
                                    where a.Emp_Id == Convert.ToInt32(id) && Convert.ToDateTime(a.From_Date).Date == DateTime.Now.Date.AddDays(-1) && a.Stetus == 1

                                    select new
                                    {
                                        id = a.Emp_Id,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        dep = a.Tbl_EmployeeRegistration.Department,
                                        from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                        to = Convert.ToDateTime(a.ToDate).Date,
                                        from2 = Convert.ToDateTime(a.From_Date).Date,
                                        type = a.LeaveType

                                    }).ToList();
                        if (reporttype == "pdf")
                        {
                            if (button == "tsprint")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=Leavesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            else
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "attachment;filename=Leavesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                        }
                        else if (reporttype == "excel")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "Excel";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "xls";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=Leavesummery." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }

                        return (null);

                    }
                    else if (dropdown == "1")
                    {
                        var data = (from a in db.Tbl_EmpLeaves
                                    where a.Emp_Id == Convert.ToInt32(id) && Convert.ToDateTime(a.From_Date).Date == DateTime.Now.Date && a.Stetus == 1

                                    select new
                                    {
                                        id = a.Emp_Id,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        dep = a.Tbl_EmployeeRegistration.Department,
                                        from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                        to = Convert.ToDateTime(a.ToDate).Date,
                                        from2 = Convert.ToDateTime(a.From_Date).Date,
                                        type = a.LeaveType
                                    }).ToList();
                        if (reporttype == "pdf")
                        {
                            if (button == "tsprint")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=Leavesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            else
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "attachment;filename=Leavesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                        }
                        else if (reporttype == "excel")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "Excel";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "xls";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=Leavesummery." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }

                        return (null);

                    }
                    else if (dropdown == "7")
                    {
                        var data = (from a in db.Tbl_EmpLeaves
                                    where a.Emp_Id == Convert.ToInt32(id) && Convert.ToDateTime(a.From_Date).Date >= thisWeekStart && Convert.ToDateTime(a.ToDate).Date <= thisWeekEnd && a.Stetus == 1

                                    select new
                                    {
                                        id = a.Emp_Id,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        dep = a.Tbl_EmployeeRegistration.Department,
                                        from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                        to = Convert.ToDateTime(a.ToDate).Date,
                                        from2 = Convert.ToDateTime(a.From_Date).Date,
                                        type = a.LeaveType

                                    }).ToList();
                        if (reporttype == "pdf")
                        {
                            if (button == "tsprint")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=Leavesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            else
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "attachment;filename=Leavesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                        }
                        else if (reporttype == "excel")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "Excel";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "xls";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=Leavesummery." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }

                        return (null);

                    }
                    else if (dropdown == "<8")
                    {
                        var data = (from a in db.Tbl_EmpLeaves
                                    where a.Emp_Id == Convert.ToInt32(id) && Convert.ToDateTime(a.From_Date).Date >= previousWeekStart && Convert.ToDateTime(a.ToDate).Date <= previousWeekEnd && a.Stetus == 1

                                    select new
                                    {
                                        id = a.Emp_Id,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        dep = a.Tbl_EmployeeRegistration.Department,
                                        from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                        to = Convert.ToDateTime(a.ToDate).Date,
                                        from2 = Convert.ToDateTime(a.From_Date).Date,
                                        type = a.LeaveType

                                    }).ToList();
                        if (reporttype == "pdf")
                        {
                            if (button == "tsprint")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=Leavesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            else
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "attachment;filename=Leavesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                        }
                        else if (reporttype == "excel")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "Excel";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "xls";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=Leavesummery." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }

                        return (null);

                    }
                    else if (dropdown == "<30")
                    {
                        var data = (from a in db.Tbl_EmpLeaves
                                    where a.Emp_Id == Convert.ToInt32(id) && Convert.ToDateTime(a.From_Date).Date >= firstDayLastMonth && Convert.ToDateTime(a.ToDate).Date <= lastDayLastMonth && a.Stetus == 1

                                    select new
                                    {
                                        id = a.Emp_Id,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        dep = a.Tbl_EmployeeRegistration.Department,
                                        from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                        to = Convert.ToDateTime(a.ToDate).Date,
                                        from2 = Convert.ToDateTime(a.From_Date).Date,
                                        type = a.LeaveType

                                    }).ToList();
                        if (reporttype == "pdf")
                        {
                            if (button == "tsprint")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=Leavesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            else
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "attachment;filename=Leavesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                        }
                        else if (reporttype == "excel")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "Excel";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "xls";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=Leavesummery." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }

                        return (null);

                    }
                    else if (dropdown == "30")
                    {
                        var data = (from a in db.Tbl_EmpLeaves
                                    where a.Emp_Id == Convert.ToInt32(id) && Convert.ToDateTime(a.From_Date).Date >= thisMonthStart && Convert.ToDateTime(a.ToDate).Date <= thisMonthEnd && a.Stetus == 1

                                    select new
                                    {
                                        id = a.Emp_Id,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        dep = a.Tbl_EmployeeRegistration.Department,
                                        from1 = (Convert.ToDateTime(a.ToDate) - Convert.ToDateTime(a.From_Date)).TotalDays + 1,
                                        to = Convert.ToDateTime(a.ToDate).Date,
                                        from2 = Convert.ToDateTime(a.From_Date).Date,
                                        type = a.LeaveType

                                    }).ToList();
                        if (reporttype == "pdf")
                        {
                            if (button == "tsprint")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=Leavesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            else
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "attachment;filename=Leavesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                        }
                        else if (reporttype == "excel")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/RptLeavesummery.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("Leaves", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "Excel";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "xls";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=Leavesummery." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }

                        return (null);

                    }
                    return Json(null, JsonRequestBehavior.AllowGet);
                }

            }
        }
        public ActionResult printanniversary(string EmpId, string Emp_Name, DateTime datefrom, DateTime todate, string dropdown, string reporttype, string button)
        {
            try
            {
                if (EmpId != "" && EmpId != null)
                {

                    if (datefrom.ToString() != "01-01-01 00:00:00")
                    {
                        var data = (from a in db.Tbl_EmployeeRegistrations
                                    where a.Emp_id == Convert.ToInt32(EmpId) && Convert.ToDateTime(a.Emp_DOJ).Month >= datefrom.Month && Convert.ToDateTime(a.Emp_DOJ).Day >= datefrom.Day && Convert.ToDateTime(a.Emp_DOJ).Month <= todate.Month && Convert.ToDateTime(a.Emp_DOJ).Day <= todate.Day
                                    select new
                                    {
                                        id = a.Emp_id,
                                        name = a.Emp_Name,
                                        dep = a.Department,
                                        date = Convert.ToDateTime(a.Emp_DOJ).Date,


                                    }).ToList();
                      
                         if (reporttype == "pdf")
                        {
                            if (button == "tsprint")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=annivarsary." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            else
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "attachment;filename=annivarsary." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                        }
                        else if (reporttype == "excel")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                             reporttype = "Excel";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "xls";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=annivarsary." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }

                        return (null);

                    }
                    else
                    {
                        DateTime baseDate = DateTime.Today;

                        var today = baseDate;
                        var yesterday = baseDate.AddDays(-1);
                        var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
                        var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
                        var previousWeekStart = thisWeekStart.AddDays(-7);
                        var previousWeekEnd = thisWeekStart.AddDays(1);
                        var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
                        var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
                        var firstDayLastMonth = thisMonthStart.AddMonths(-1);
                        var lastDayLastMonth = thisMonthStart.AddSeconds(-1);
                        if (dropdown == "-1")
                        {

                            var data = (from a in db.Tbl_EmployeeRegistrations
                                        where a.Emp_id == Convert.ToInt32(EmpId) && Convert.ToDateTime(a.Emp_DOJ).Month >= DateTime.Now.AddDays(-1).Month && Convert.ToDateTime(a.Emp_DOJ).Day >= DateTime.Now.AddDays(-1).Day && Convert.ToDateTime(a.Emp_DOJ).Month <= DateTime.Now.AddDays(-1).Month && Convert.ToDateTime(a.Emp_DOJ).Day <= DateTime.Now.AddDays(-1).Day
                                        select new
                                        {
                                            id = a.Emp_id,
                                            name = a.Emp_Name,
                                            dep = a.Department,
                                            date = Convert.ToDateTime(a.Emp_DOJ).Date,


                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=annivarsary." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=annivarsary." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=annivarsary." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }

                            return (null);

                        }
                        else if (dropdown == "1")
                        {

                            var data = (from a in db.Tbl_EmployeeRegistrations
                                        where a.Emp_id == Convert.ToInt32(EmpId) && Convert.ToDateTime(a.Emp_DOJ).Month >= DateTime.Now.Month && Convert.ToDateTime(a.Emp_DOJ).Day >= DateTime.Now.Day && Convert.ToDateTime(a.Emp_DOJ).Month <= DateTime.Now.Month && Convert.ToDateTime(a.Emp_DOJ).Day <= DateTime.Now.Day

                                        select new
                                        {
                                            id = a.Emp_id,
                                            name = a.Emp_Name,
                                            dep = a.Department,
                                            date = Convert.ToDateTime(a.Emp_DOJ).Date,


                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=annivarsary." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=annivarsary." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=annivarsary." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }

                            return (null);

                        }
                        else if (dropdown == "7")
                        {

                            var data = (from a in db.Tbl_EmployeeRegistrations
                                        where a.Emp_id == Convert.ToInt32(EmpId) && Convert.ToDateTime(a.Emp_DOJ).Month >= thisWeekStart.Month && Convert.ToDateTime(a.Emp_DOJ).Day >= thisWeekStart.Day && Convert.ToDateTime(a.Emp_DOJ).Month <= thisWeekEnd.Month && Convert.ToDateTime(a.Emp_DOJ).Day <= thisWeekEnd.Day
                                        select new
                                        {
                                            id = a.Emp_id,
                                            name = a.Emp_Name,
                                            dep = a.Department,
                                            date = Convert.ToDateTime(a.Emp_DOJ).Date,
                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=annivarsary." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=annivarsary." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=annivarsary." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }

                            return (null);

                        }
                        else if (dropdown == "<8")
                        {

                            var data = (from a in db.Tbl_EmployeeRegistrations
                                        where a.Emp_id == Convert.ToInt32(EmpId) && Convert.ToDateTime(a.Emp_DOJ).Month >= previousWeekStart.Month && Convert.ToDateTime(a.Emp_DOJ).Day >= previousWeekStart.Day && Convert.ToDateTime(a.Emp_DOJ).Month <= previousWeekStart.Month && Convert.ToDateTime(a.Emp_DOJ).Day <= previousWeekEnd.Day
                                        select new
                                        {
                                            id = a.Emp_id,
                                            name = a.Emp_Name,
                                            dep = a.Department,
                                            date = Convert.ToDateTime(a.Emp_DOJ).Date,
                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=annivarsary." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=annivarsary." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=annivarsary." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }

                            return (null);

                        }
                        else if (dropdown == "<30")
                        {

                            var data = (from a in db.Tbl_EmployeeRegistrations
                                        where a.Emp_id == Convert.ToInt32(EmpId) && Convert.ToDateTime(a.Emp_DOJ).Month >= firstDayLastMonth.Month && Convert.ToDateTime(a.Emp_DOJ).Day >= firstDayLastMonth.Day && Convert.ToDateTime(a.Emp_DOJ).Month <= lastDayLastMonth.Month && Convert.ToDateTime(a.Emp_DOJ).Day <= lastDayLastMonth.Day
                                        select new
                                        {
                                            id = a.Emp_id,
                                            name = a.Emp_Name,
                                            dep = a.Department,
                                            date = Convert.ToDateTime(a.Emp_DOJ).Date,
                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=annivarsary." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=annivarsary." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=annivarsary." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }

                            return (null);


                        }
                        else if (dropdown == "30")
                        {

                            var data = (from a in db.Tbl_EmployeeRegistrations
                                        where a.Emp_id == Convert.ToInt32(EmpId) && Convert.ToDateTime(a.Emp_DOJ).Month >= thisMonthStart.Month && Convert.ToDateTime(a.Emp_DOJ).Day >= thisMonthStart.Day && Convert.ToDateTime(a.Emp_DOJ).Month <= thisMonthEnd.Month && Convert.ToDateTime(a.Emp_DOJ).Day <= thisMonthEnd.Day
                                        select new
                                        {
                                            id = a.Emp_id,
                                            name = a.Emp_Name,
                                            dep = a.Department,
                                            date = Convert.ToDateTime(a.Emp_DOJ).Date,
                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=annivarsary." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=annivarsary." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=annivarsary." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }

                            return (null);

                        }
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    if (datefrom.ToString() != "01-01-01 00:00:00")
                    {

                        var data = (from a in db.Tbl_EmployeeRegistrations
                                    where Convert.ToDateTime(a.Emp_DOJ).Month >= datefrom.Month && Convert.ToDateTime(a.Emp_DOJ).Day >= datefrom.Day && Convert.ToDateTime(a.Emp_DOJ).Month <= todate.Month && Convert.ToDateTime(a.Emp_DOJ).Day <= todate.Day
                                    select new
                                    {
                                        id = a.Emp_id,
                                        name = a.Emp_Name,
                                        dep = a.Department,
                                        date = Convert.ToDateTime(a.Emp_DOJ).Date,


                                    }).ToList();
                        if (reporttype == "pdf")
                        {
                            if (button == "tsprint")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=annivarsary." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            else
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "attachment;filename=annivarsary." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                        }
                        else if (reporttype == "excel")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "Excel";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "xls";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=annivarsary." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }

                        return (null);
                    }
                    else
                    {
                        DateTime baseDate = DateTime.Today;

                        var today = baseDate;
                        var yesterday = baseDate.AddDays(-1);
                        var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
                        var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
                        var previousWeekStart = thisWeekStart.AddDays(-7);
                        var previousWeekEnd = thisWeekStart.AddDays(1);
                        var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
                        var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
                        var firstDayLastMonth = thisMonthStart.AddMonths(-1);
                        var lastDayLastMonth = thisMonthStart.AddSeconds(-1);
                        if (dropdown == "-1")
                        {
                            var data = (from a in db.Tbl_EmployeeRegistrations
                                        where Convert.ToDateTime(a.Emp_DOJ).Month >= DateTime.Now.AddDays(-1).Month && Convert.ToDateTime(a.Emp_DOJ).Day >= DateTime.Now.AddDays(-1).Day && Convert.ToDateTime(a.Emp_DOJ).Month <= DateTime.Now.AddDays(-1).Month && Convert.ToDateTime(a.Emp_DOJ).Day <= DateTime.Now.AddDays(-1).Day
                                        select new
                                        {
                                            id = a.Emp_id,
                                            name = a.Emp_Name,
                                            dep = a.Department,
                                            date = Convert.ToDateTime(a.Emp_DOJ).Date,


                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=annivarsary." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=annivarsary." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=annivarsary." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }

                            return (null);


                        }
                        else if (dropdown == "1")
                        {
                            var data = (from a in db.Tbl_EmployeeRegistrations
                                        where Convert.ToDateTime(a.Emp_DOJ).Month >= DateTime.Now.Month && Convert.ToDateTime(a.Emp_DOJ).Day >= DateTime.Now.Day && Convert.ToDateTime(a.Emp_DOJ).Month <= DateTime.Now.Month && Convert.ToDateTime(a.Emp_DOJ).Day <= DateTime.Now.Day

                                        select new
                                        {
                                            id = a.Emp_id,
                                            name = a.Emp_Name,
                                            dep = a.Department,
                                            date = Convert.ToDateTime(a.Emp_DOJ).Date,


                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=annivarsary." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=annivarsary." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=annivarsary." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }

                            return (null);

                        }
                        else if (dropdown == "7")
                        {

                            var data = (from a in db.Tbl_EmployeeRegistrations
                                        where Convert.ToDateTime(a.Emp_DOJ).Month >= thisWeekStart.Month && Convert.ToDateTime(a.Emp_DOJ).Day >= thisWeekStart.Day && Convert.ToDateTime(a.Emp_DOJ).Month <= thisWeekEnd.Month && Convert.ToDateTime(a.Emp_DOJ).Day <= thisWeekEnd.Day
                                        select new
                                        {
                                            id = a.Emp_id,
                                            name = a.Emp_Name,
                                            dep = a.Department,
                                            date = Convert.ToDateTime(a.Emp_DOJ).Date,
                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=annivarsary." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=annivarsary." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=annivarsary." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }

                            return (null);
                        }
                        else if (dropdown == "<8")
                        {
                            var data = (from a in db.Tbl_EmployeeRegistrations
                                        where Convert.ToDateTime(a.Emp_DOJ).Month >= previousWeekStart.Month && Convert.ToDateTime(a.Emp_DOJ).Day >= previousWeekStart.Day && Convert.ToDateTime(a.Emp_DOJ).Month <= previousWeekStart.Month && Convert.ToDateTime(a.Emp_DOJ).Day <= previousWeekEnd.Day
                                        select new
                                        {
                                            id = a.Emp_id,
                                            name = a.Emp_Name,
                                            dep = a.Department,
                                            date = Convert.ToDateTime(a.Emp_DOJ).Date,
                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=annivarsary." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=annivarsary." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=annivarsary." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }

                            return (null);
                        }
                        else if (dropdown == "<30")
                        {
                            var data = (from a in db.Tbl_EmployeeRegistrations
                                        where Convert.ToDateTime(a.Emp_DOJ).Month >= firstDayLastMonth.Month && Convert.ToDateTime(a.Emp_DOJ).Day >= firstDayLastMonth.Day && Convert.ToDateTime(a.Emp_DOJ).Month <= lastDayLastMonth.Month && Convert.ToDateTime(a.Emp_DOJ).Day <= lastDayLastMonth.Day
                                        select new
                                        {
                                            id = a.Emp_id,
                                            name = a.Emp_Name,
                                            dep = a.Department,
                                            date = Convert.ToDateTime(a.Emp_DOJ).Date,
                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=annivarsary." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=annivarsary." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=annivarsary." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }

                            return (null);

                        }
                        else if (dropdown == "30")
                        {
                            var data = (from a in db.Tbl_EmployeeRegistrations
                                        where Convert.ToDateTime(a.Emp_DOJ).Month >= thisMonthStart.Month && Convert.ToDateTime(a.Emp_DOJ).Day >= thisMonthStart.Day && Convert.ToDateTime(a.Emp_DOJ).Month <= thisMonthEnd.Month && Convert.ToDateTime(a.Emp_DOJ).Day <= thisMonthEnd.Day
                                        select new
                                        {
                                            id = a.Emp_id,
                                            name = a.Emp_Name,
                                            dep = a.Department,
                                            date = Convert.ToDateTime(a.Emp_DOJ).Date,
                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=annivarsary." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=annivarsary." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=annivarsary." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }

                            return (null);

                        }
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                var id = (from b in db.Tbl_EmployeeRegistrations where b.Emp_Name.Trim() == EmpId.Trim() select b.Emp_id).SingleOrDefault();

                if (datefrom.ToString() != "01-01-01 00:00:00")
                {

                    var data = (from a in db.Tbl_EmployeeRegistrations
                                where a.Emp_id == Convert.ToInt32(id) && Convert.ToDateTime(a.Emp_DOJ).Month >= datefrom.Month && Convert.ToDateTime(a.Emp_DOJ).Day >= datefrom.Day && Convert.ToDateTime(a.Emp_DOJ).Month <= todate.Month && Convert.ToDateTime(a.Emp_DOJ).Day <= todate.Day
                                select new
                                {
                                    id = a.Emp_id,
                                    name = a.Emp_Name,
                                    dep = a.Department,
                                    date = Convert.ToDateTime(a.Emp_DOJ).Date,


                                }).ToList();
                    if (reporttype == "pdf")
                    {
                        if (button == "tsprint")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "pdf";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "pdf";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=annivarsary." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }
                        else
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "pdf";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "pdf";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "attachment;filename=annivarsary." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }
                    }
                    else if (reporttype == "excel")
                    {
                        LocalReport lc1 = new LocalReport();
                        lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                        ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                        rd1.Value = data;
                        lc1.DataSources.Add(rd1);
                        reporttype = "Excel";
                        string mimetype1;
                        string encoding1;
                        string filenameextention1;


                        filenameextention1 = "xls";
                        string[] strems1;
                        Warning[] warnings1;
                        byte[] renderdbyte1;
                        renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                            , out strems1, out warnings1);
                        Response.AddHeader("content-disposition", "inline;filename=annivarsary." + filenameextention1);
                        return File(renderdbyte1, "." + filenameextention1);
                    }

                    return (null);
                }
                else
                {
                    DateTime baseDate = DateTime.Today;

                    var today = baseDate;
                    var yesterday = baseDate.AddDays(-1);
                    var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
                    var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
                    var previousWeekStart = thisWeekStart.AddDays(-7);
                    var previousWeekEnd = thisWeekStart.AddDays(1);
                    var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
                    var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
                    var firstDayLastMonth = thisMonthStart.AddMonths(-1);
                    var lastDayLastMonth = thisMonthStart.AddSeconds(-1);
                    if (dropdown == "-1")
                    {
                        var data = (from a in db.Tbl_EmployeeRegistrations
                                    where a.Emp_id == Convert.ToInt32(id) && Convert.ToDateTime(a.Emp_DOJ).Month >= DateTime.Now.AddDays(-1).Month && Convert.ToDateTime(a.Emp_DOJ).Day >= DateTime.Now.AddDays(-1).Day && Convert.ToDateTime(a.Emp_DOJ).Month <= DateTime.Now.AddDays(-1).Month && Convert.ToDateTime(a.Emp_DOJ).Day <= DateTime.Now.AddDays(-1).Day
                                    select new
                                    {
                                        id = a.Emp_id,
                                        name = a.Emp_Name,
                                        dep = a.Department,
                                        date = Convert.ToDateTime(a.Emp_DOJ).Date,


                                    }).ToList();
                        if (reporttype == "pdf")
                        {
                            if (button == "tsprint")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=annivarsary." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            else
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "attachment;filename=annivarsary." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                        }
                        else if (reporttype == "excel")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "Excel";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "xls";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=annivarsary." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }

                        return (null);

                    }
                    else if (dropdown == "1")
                    {
                        var data = (from a in db.Tbl_EmployeeRegistrations
                                    where a.Emp_id == Convert.ToInt32(id) && Convert.ToDateTime(a.Emp_DOJ).Month >= DateTime.Now.Month && Convert.ToDateTime(a.Emp_DOJ).Day >= DateTime.Now.Day && Convert.ToDateTime(a.Emp_DOJ).Month <= DateTime.Now.Month && Convert.ToDateTime(a.Emp_DOJ).Day <= DateTime.Now.Day

                                    select new
                                    {
                                        id = a.Emp_id,
                                        name = a.Emp_Name,
                                        dep = a.Department,
                                        date = Convert.ToDateTime(a.Emp_DOJ).Date,


                                    }).ToList();
                        if (reporttype == "pdf")
                        {
                            if (button == "tsprint")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=annivarsary." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            else
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "attachment;filename=annivarsary." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                        }
                        else if (reporttype == "excel")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "Excel";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "xls";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=annivarsary." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }

                        return (null);

                    }
                    else if (dropdown == "7")
                    {
                        var data = (from a in db.Tbl_EmployeeRegistrations
                                    where a.Emp_id == Convert.ToInt32(id) && Convert.ToDateTime(a.Emp_DOJ).Month >= thisWeekStart.Month && Convert.ToDateTime(a.Emp_DOJ).Day >= thisWeekStart.Day && Convert.ToDateTime(a.Emp_DOJ).Month <= thisWeekEnd.Month && Convert.ToDateTime(a.Emp_DOJ).Day <= thisWeekEnd.Day
                                    select new
                                    {
                                        id = a.Emp_id,
                                        name = a.Emp_Name,
                                        dep = a.Department,
                                        date = Convert.ToDateTime(a.Emp_DOJ).Date,
                                    }).ToList();
                        if (reporttype == "pdf")
                        {
                            if (button == "tsprint")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=annivarsary." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            else
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "attachment;filename=annivarsary." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                        }
                        else if (reporttype == "excel")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "Excel";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "xls";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=annivarsary." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }

                        return (null);

                    }
                    else if (dropdown == "<8")
                    {
                        var data = (from a in db.Tbl_EmployeeRegistrations
                                    where a.Emp_id == Convert.ToInt32(id) && Convert.ToDateTime(a.Emp_DOJ).Month >= previousWeekStart.Month && Convert.ToDateTime(a.Emp_DOJ).Day >= previousWeekStart.Day && Convert.ToDateTime(a.Emp_DOJ).Month <= previousWeekStart.Month && Convert.ToDateTime(a.Emp_DOJ).Day <= previousWeekEnd.Day
                                    select new
                                    {
                                        id = a.Emp_id,
                                        name = a.Emp_Name,
                                        dep = a.Department,
                                        date = Convert.ToDateTime(a.Emp_DOJ).Date,
                                    }).ToList();
                        if (reporttype == "pdf")
                        {
                            if (button == "tsprint")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=annivarsary." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            else
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "attachment;filename=annivarsary." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                        }
                        else if (reporttype == "excel")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "Excel";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "xls";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=annivarsary." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }

                        return (null);

                    }
                    else if (dropdown == "<30")
                    {
                        var data = (from a in db.Tbl_EmployeeRegistrations
                                    where a.Emp_id == Convert.ToInt32(id) && Convert.ToDateTime(a.Emp_DOJ).Month >= firstDayLastMonth.Month && Convert.ToDateTime(a.Emp_DOJ).Day >= firstDayLastMonth.Day && Convert.ToDateTime(a.Emp_DOJ).Month <= lastDayLastMonth.Month && Convert.ToDateTime(a.Emp_DOJ).Day <= lastDayLastMonth.Day
                                    select new
                                    {
                                        id = a.Emp_id,
                                        name = a.Emp_Name,
                                        dep = a.Department,
                                        date = Convert.ToDateTime(a.Emp_DOJ).Date,
                                    }).ToList();
                        if (reporttype == "pdf")
                        {
                            if (button == "tsprint")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=annivarsary." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            else
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "attachment;filename=annivarsary." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                        }
                        else if (reporttype == "excel")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "Excel";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "xls";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=annivarsary." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }

                        return (null);

                    }
                    else if (dropdown == "30")
                    {
                        var data = (from a in db.Tbl_EmployeeRegistrations
                                    where a.Emp_id == Convert.ToInt32(id) && Convert.ToDateTime(a.Emp_DOJ).Month >= thisMonthStart.Month && Convert.ToDateTime(a.Emp_DOJ).Day >= thisMonthStart.Day && Convert.ToDateTime(a.Emp_DOJ).Month <= thisMonthEnd.Month && Convert.ToDateTime(a.Emp_DOJ).Day <= thisMonthEnd.Day
                                    select new
                                    {
                                        id = a.Emp_id,
                                        name = a.Emp_Name,
                                        dep = a.Department,
                                        date = Convert.ToDateTime(a.Emp_DOJ).Date,
                                    }).ToList();
                        if (reporttype == "pdf")
                        {
                            if (button == "tsprint")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=annivarsary." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            else
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "attachment;filename=annivarsary." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                        }
                        else if (reporttype == "excel")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/Rptanniversary.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("annivarsary", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "Excel";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "xls";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=annivarsary." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }

                        return (null);

                    }
                    return Json(null, JsonRequestBehavior.AllowGet);
                }

            }
        }




        public ActionResult timesheetprint(string EmpId,string Emp_Name,DateTime datefrom,DateTime todate,string dropdown,string reporttype,string button)
        {
            try
            {
                if (EmpId != "" && EmpId != null)
                {

                    if (datefrom.ToString() != "01-01-01 00:00:00")
                    {
                        var data = (from a in db.Tbl_Attendances
                                    where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.Emp_ID == Convert.ToInt32(EmpId) && a.ClockIn != null  && Convert.ToDateTime(a.ClockIn).Date >= datefrom.Date && Convert.ToDateTime(a.ClockIn).Date <= todate.Date
                                    orderby a.ClockIn ascending
                                    select new
                                    {
                                        empid = a.Emp_ID,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                        date = Convert.ToDateTime(a.ClockIn).Date,
                                        currentdate=DateTime.Now,
                                        dep = a.Tbl_EmployeeRegistration.Department.Trim()

                                    }).ToList();
                        if (reporttype == "pdf")
                        {
                            if (button == "tsprint")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=timesheet." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            else
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "attachment;filename=timesheet." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                        }
                        else if (reporttype == "excel")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                             reporttype = "Excel";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "xls";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=timesheet." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }
                        return (null);
                    }
                    else
                    {
                        DateTime baseDate = DateTime.Today;

                        var today = baseDate;
                        var yesterday = baseDate.AddDays(-1);
                        var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
                        var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
                        var previousWeekStart = thisWeekStart.AddDays(-7);
                        var previousWeekEnd = thisWeekStart.AddDays(1);
                        var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
                        var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
                        var firstDayLastMonth = thisMonthStart.AddMonths(-1);
                        var lastDayLastMonth = thisMonthStart.AddSeconds(-1);
                        if (dropdown == "-1")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date &&  a.Emp_ID == Convert.ToInt32(EmpId) && a.ClockIn != null  && Convert.ToDateTime(a.ClockIn).Date == DateTime.Now.Date.AddDays(-1)
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            empid = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                            date = Convert.ToDateTime(a.ClockIn).Date,
                                            currentdate = DateTime.Now,
                                            dep = a.Tbl_EmployeeRegistration.Department.Trim()

                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=timesheet." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=timesheet." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=timesheet." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            return (null);

                        }
                        else if (dropdown == "1")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date &&  a.Emp_ID == Convert.ToInt32(EmpId) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date == DateTime.Now.Date
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            empid = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                            date = Convert.ToDateTime(a.ClockIn).Date,
                                            currentdate = DateTime.Now,
                                            dep = a.Tbl_EmployeeRegistration.Department.Trim()

                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=timesheet." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=timesheet." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=timesheet." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            return (null);

                        }
                        else if (dropdown == "7")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.Emp_ID == Convert.ToInt32(EmpId) && a.ClockIn != null  && Convert.ToDateTime(a.ClockIn).Date >= thisWeekStart && Convert.ToDateTime(a.ClockIn).Date <= thisWeekEnd
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            empid = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                            date = Convert.ToDateTime(a.ClockIn).Date,
                                            currentdate = DateTime.Now,
                                            dep = a.Tbl_EmployeeRegistration.Department.Trim()

                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=timesheet." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=timesheet." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=timesheet." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            return (null);

                        }
                        else if (dropdown == "<8")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.Emp_ID == Convert.ToInt32(EmpId) && a.ClockIn != null  && Convert.ToDateTime(a.ClockIn).Date >= previousWeekStart && Convert.ToDateTime(a.ClockIn).Date <= previousWeekEnd
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            empid = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                            date = Convert.ToDateTime(a.ClockIn).Date,
                                            currentdate = DateTime.Now,
                                            dep = a.Tbl_EmployeeRegistration.Department.Trim()

                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=timesheet." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=timesheet." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=timesheet." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            return (null);

                        }
                        else if (dropdown == "<30")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.Emp_ID == Convert.ToInt32(EmpId) && a.ClockIn != null  && Convert.ToDateTime(a.ClockIn).Date >= firstDayLastMonth && Convert.ToDateTime(a.ClockIn).Date <= lastDayLastMonth
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            empid = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                            date = Convert.ToDateTime(a.ClockIn).Date,
                                            currentdate = DateTime.Now,
                                            dep = a.Tbl_EmployeeRegistration.Department.Trim()

                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=timesheet." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=timesheet." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=timesheet." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            return (null);

                        }
                        else if (dropdown == "30")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date &&  a.Emp_ID == Convert.ToInt32(EmpId) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= thisMonthStart && Convert.ToDateTime(a.ClockIn).Date <= thisMonthEnd
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            empid = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                            date = Convert.ToDateTime(a.ClockIn).Date,
                                            currentdate = DateTime.Now,
                                            dep = a.Tbl_EmployeeRegistration.Department.Trim()

                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=timesheet." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=timesheet." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=timesheet." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            return (null);

                        }
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    if (datefrom.ToString() != "01-01-01 00:00:00")
                    {
                        var data = (from a in db.Tbl_Attendances
                                    where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= datefrom.Date && Convert.ToDateTime(a.ClockIn).Date <= todate.Date
                                    orderby a.ClockIn ascending
                                    select new
                                    {
                                        empid = a.Emp_ID,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                        date = Convert.ToDateTime(a.ClockIn).Date,
                                        currentdate = DateTime.Now,
                                        dep = a.Tbl_EmployeeRegistration.Department.Trim()

                                    }).ToList();
                        if (reporttype == "pdf")
                        {
                            if (button == "tsprint")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=timesheet." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            else
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "attachment;filename=timesheet." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                        }
                        else if (reporttype == "excel")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "Excel";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "xls";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=timesheet." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }
                        return (null);
                    }
                    else
                    {
                        DateTime baseDate = DateTime.Today;

                        var today = baseDate;
                        var yesterday = baseDate.AddDays(-1);
                        var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
                        var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
                        var previousWeekStart = thisWeekStart.AddDays(-7);
                        var previousWeekEnd = thisWeekStart.AddDays(1);
                        var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
                        var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
                        var firstDayLastMonth = thisMonthStart.AddMonths(-1);
                        var lastDayLastMonth = thisMonthStart.AddSeconds(-1);
                        if (dropdown == "-1")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date  && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date == DateTime.Now.Date.AddDays(-1)
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            empid = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                            date = Convert.ToDateTime(a.ClockIn).Date,
                                            currentdate = DateTime.Now,
                                            dep = a.Tbl_EmployeeRegistration.Department.Trim()

                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=timesheet." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=timesheet." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=timesheet." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            return (null);

                        }
                        else if (dropdown == "1")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where a.ClockIn != null  && Convert.ToDateTime(a.ClockIn).Date == DateTime.Now.Date
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            empid = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                            date = Convert.ToDateTime(a.ClockIn).Date,
                                            currentdate = DateTime.Now,
                                            dep = a.Tbl_EmployeeRegistration.Department.Trim()

                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=timesheet." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=timesheet." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=timesheet." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            return (null);

                        }
                        else if (dropdown == "7")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date  && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= thisWeekStart && Convert.ToDateTime(a.ClockIn).Date <= thisWeekEnd
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            empid = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                            date = Convert.ToDateTime(a.ClockIn).Date,
                                            currentdate = DateTime.Now,
                                            dep = a.Tbl_EmployeeRegistration.Department.Trim()

                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=timesheet." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=timesheet." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=timesheet." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            return (null);

                        }
                        else if (dropdown == "<8")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= previousWeekStart && Convert.ToDateTime(a.ClockIn).Date <= previousWeekEnd
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            empid = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                            date = Convert.ToDateTime(a.ClockIn).Date,
                                            currentdate = DateTime.Now,
                                            dep = a.Tbl_EmployeeRegistration.Department.Trim()

                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=timesheet." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=timesheet." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=timesheet." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            return (null);

                        }
                        else if (dropdown == "<30")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date  && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= firstDayLastMonth && Convert.ToDateTime(a.ClockIn).Date <= lastDayLastMonth
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            empid = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                            date = Convert.ToDateTime(a.ClockIn).Date,
                                            currentdate = DateTime.Now,
                                            dep = a.Tbl_EmployeeRegistration.Department.Trim()

                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=timesheet." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=timesheet." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=timesheet." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            return (null);
                        }
                        else if (dropdown == "30")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date  && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= thisMonthStart && Convert.ToDateTime(a.ClockIn).Date <= thisMonthEnd
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            empid = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                            date = Convert.ToDateTime(a.ClockIn).Date,
                                            currentdate = DateTime.Now,
                                            dep = a.Tbl_EmployeeRegistration.Department.Trim()

                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=timesheet." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=timesheet." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=timesheet." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            return (null);

                        }
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                var id = (from b in db.Tbl_EmployeeRegistrations where b.Emp_Name.Trim() == EmpId.Trim() select b.Emp_id).SingleOrDefault();

                if (datefrom.ToString() != "01-01-01 00:00:00")
                {

                    var data = (from a in db.Tbl_Attendances
                                where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.Emp_ID == Convert.ToInt32(id) && a.ClockIn != null  && Convert.ToDateTime(a.ClockIn).Date >= datefrom.Date && Convert.ToDateTime(a.ClockIn).Date <= todate.Date
                                orderby a.ClockIn ascending
                                select new
                                {
                                    empid = a.Emp_ID,
                                    name = a.Tbl_EmployeeRegistration.Emp_Name,
                                    totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                    date = Convert.ToDateTime(a.ClockIn).Date,
                                    currentdate = DateTime.Now,
                                    dep = a.Tbl_EmployeeRegistration.Department.Trim()

                                }).ToList();
                    if (reporttype == "pdf")
                    {
                        if (button == "tsprint")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "pdf";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "pdf";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=timesheet." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }
                        else
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "pdf";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "pdf";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "attachment;filename=timesheet." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }
                    }
                    else if (reporttype == "excel")
                    {
                        LocalReport lc1 = new LocalReport();
                        lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                        ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                        rd1.Value = data;
                        lc1.DataSources.Add(rd1);
                        reporttype = "Excel";
                        string mimetype1;
                        string encoding1;
                        string filenameextention1;


                        filenameextention1 = "xls";
                        string[] strems1;
                        Warning[] warnings1;
                        byte[] renderdbyte1;
                        renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                            , out strems1, out warnings1);
                        Response.AddHeader("content-disposition", "inline;filename=timesheet." + filenameextention1);
                        return File(renderdbyte1, "." + filenameextention1);
                    }
                    return (null);
                }
                else
                {
                    DateTime baseDate = DateTime.Today;

                    var today = baseDate;
                    var yesterday = baseDate.AddDays(-1);
                    var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
                    var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
                    var previousWeekStart = thisWeekStart.AddDays(-7);
                    var previousWeekEnd = thisWeekStart.AddDays(1);
                    var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
                    var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
                    var firstDayLastMonth = thisMonthStart.AddMonths(-1);
                    var lastDayLastMonth = thisMonthStart.AddSeconds(-1);
                    if (dropdown == "-1")
                    {
                        var data = (from a in db.Tbl_Attendances
                                    where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.Emp_ID == Convert.ToInt32(id) && a.ClockIn != null  && Convert.ToDateTime(a.ClockIn).Date == DateTime.Now.Date.AddDays(-1)
                                    orderby a.ClockIn ascending
                                    select new
                                    {
                                        empid = a.Emp_ID,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                        date = Convert.ToDateTime(a.ClockIn).Date,
                                        dep = a.Tbl_EmployeeRegistration.Department.Trim()

                                    }).ToList();
                        if (reporttype == "pdf")
                        {
                            if (button == "tsprint")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=timesheet." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            else
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "attachment;filename=timesheet." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                        }
                        else if (reporttype == "excel")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "Excel";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "xls";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=timesheet." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }
                        return (null);

                    }
                    else if (dropdown == "1")
                    {
                        var data = (from a in db.Tbl_Attendances
                                    where a.Emp_ID == Convert.ToInt32(id) && a.ClockIn != null  && Convert.ToDateTime(a.ClockIn).Date == DateTime.Now.Date
                                    orderby a.ClockIn ascending
                                    select new
                                    {
                                        empid = a.Emp_ID,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                        date = Convert.ToDateTime(a.ClockIn).Date,
                                        currentdate = DateTime.Now,
                                        dep = a.Tbl_EmployeeRegistration.Department.Trim()

                                    }).ToList();
                        if (reporttype == "pdf")
                        {
                            if (button == "tsprint")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=timesheet." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            else
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "attachment;filename=timesheet." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                        }
                        else if (reporttype == "excel")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "Excel";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "xls";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=timesheet." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }
                        return (null);

                    }
                    else if (dropdown == "7")
                    {
                        var data = (from a in db.Tbl_Attendances
                                    where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.Emp_ID == Convert.ToInt32(id) && a.ClockIn != null  && Convert.ToDateTime(a.ClockIn).Date >= thisWeekStart && Convert.ToDateTime(a.ClockIn).Date <= thisWeekEnd
                                    orderby a.ClockIn ascending
                                    select new
                                    {
                                        empid = a.Emp_ID,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                        date = Convert.ToDateTime(a.ClockIn).Date,
                                        currentdate = DateTime.Now,
                                        dep = a.Tbl_EmployeeRegistration.Department.Trim()

                                    }).ToList();
                        if (reporttype == "pdf")
                        {
                            if (button == "tsprint")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=timesheet." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            else
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "attachment;filename=timesheet." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                        }
                        else if (reporttype == "excel")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "Excel";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "xls";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=timesheet." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }
                        return (null);

                    }
                    else if (dropdown == "<8")
                    {
                        var data = (from a in db.Tbl_Attendances
                                    where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.Emp_ID == Convert.ToInt32(id) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= previousWeekStart && Convert.ToDateTime(a.ClockIn).Date <= previousWeekEnd
                                    orderby a.ClockIn ascending
                                    select new
                                    {
                                        empid = a.Emp_ID,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                        date = Convert.ToDateTime(a.ClockIn).Date,
                                        currentdate = DateTime.Now,
                                        dep = a.Tbl_EmployeeRegistration.Department.Trim()

                                    }).ToList();
                        if (reporttype == "pdf")
                        {
                            if (button == "tsprint")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=timesheet." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            else
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "attachment;filename=timesheet." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                        }
                        else if (reporttype == "excel")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "Excel";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "xls";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=timesheet." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }
                        return (null);

                    }
                    else if (dropdown == "<30")
                    {
                        var data = (from a in db.Tbl_Attendances
                                    where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date &&  a.Emp_ID == Convert.ToInt32(id) && a.ClockIn != null  && Convert.ToDateTime(a.ClockIn).Date >= firstDayLastMonth && Convert.ToDateTime(a.ClockIn).Date <= lastDayLastMonth
                                    orderby a.ClockIn ascending
                                    select new
                                    {
                                        empid = a.Emp_ID,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                        date = Convert.ToDateTime(a.ClockIn).Date,
                                        currentdate = DateTime.Now,
                                        dep = a.Tbl_EmployeeRegistration.Department.Trim()

                                    }).ToList();
                        if (reporttype == "pdf")
                        {
                            if (button == "tsprint")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=timesheet." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            else
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "attachment;filename=timesheet." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                        }
                        else if (reporttype == "excel")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "Excel";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "xls";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=timesheet." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }
                        return (null);

                    }
                    else if (dropdown == "30")
                    {
                        var data = (from a in db.Tbl_Attendances
                                    where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.Emp_ID == Convert.ToInt32(id) && a.ClockIn != null  && Convert.ToDateTime(a.ClockIn).Date >= thisMonthStart && Convert.ToDateTime(a.ClockIn).Date <= thisMonthEnd
                                    orderby a.ClockIn ascending
                                    select new
                                    {
                                        empid = a.Emp_ID,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                        date = Convert.ToDateTime(a.ClockIn).Date,
                                        currentdate = DateTime.Now,
                                        dep =a.Tbl_EmployeeRegistration.Department.Trim()

                                    }).ToList();
                        if (reporttype == "pdf")
                        {
                            if (button == "tsprint")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=timesheet." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            else
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "attachment;filename=timesheet." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                        }
                        else if (reporttype == "excel")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("timesheet", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "Excel";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "xls";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=timesheet." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }
                        return (null);

                    }
                    return Json(null, JsonRequestBehavior.AllowGet);
                }

            }
        }
        public ActionResult attendancesummeryprint(string EmpId, string Emp_Name, DateTime datefrom, DateTime todate, string dropdown, string reporttype, string button)
        {
            try
            {
                if (EmpId != "" && EmpId != null)
                {

                    if (datefrom.ToString() != "01-01-01 00:00:00")
                    {
                        var data = (from a in db.Tbl_Attendances
                                    where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.Emp_ID == Convert.ToInt32(EmpId) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= datefrom.Date && Convert.ToDateTime(a.ClockIn).Date <= todate.Date
                                    orderby a.ClockIn ascending
                                    select new
                                    {
                                        empid = a.Emp_ID,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        date = Convert.ToDateTime(a.ClockIn).Date,
                                        currentdate = DateTime.Now,
                                        dep = a.Tbl_EmployeeRegistration.Department.Trim(),
                                        status= "Present"

                                    }).ToList();
                        if (reporttype == "pdf")
                        {
                            if (button == "tsprint")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=attendancesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            else
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "attachment;filename=attendancesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                        }
                        else if (reporttype == "excel")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "Excel";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "xls";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=attendancesummery." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }
                        return (null);
                    }
                    else
                    {
                        DateTime baseDate = DateTime.Today;

                        var today = baseDate;
                        var yesterday = baseDate.AddDays(-1);
                        var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
                        var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
                        var previousWeekStart = thisWeekStart.AddDays(-7);
                        var previousWeekEnd = thisWeekStart.AddDays(1);
                        var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
                        var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
                        var firstDayLastMonth = thisMonthStart.AddMonths(-1);
                        var lastDayLastMonth = thisMonthStart.AddSeconds(-1);
                        if (dropdown == "-1")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.Emp_ID == Convert.ToInt32(EmpId) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date == DateTime.Now.Date.AddDays(-1)
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            empid = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            date = Convert.ToDateTime(a.ClockIn).Date,
                                            currentdate = DateTime.Now,
                                            dep = a.Tbl_EmployeeRegistration.Department.Trim(),
                                            status = "Present",
                                      

                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=attendancesummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=attendancesummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=attendancesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            return (null);

                        }
                        else if (dropdown == "1")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.Emp_ID == Convert.ToInt32(EmpId) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date == DateTime.Now.Date
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            empid = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            date = Convert.ToDateTime(a.ClockIn).Date,
                                            currentdate = DateTime.Now,
                                            dep = a.Tbl_EmployeeRegistration.Department.Trim(),
                                            status = "Present"

                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=attendancesummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=attendancesummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=attendancesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            return (null);

                        }
                        else if (dropdown == "7")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.Emp_ID == Convert.ToInt32(EmpId) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= thisWeekStart && Convert.ToDateTime(a.ClockIn).Date <= thisWeekEnd
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            empid = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            date = Convert.ToDateTime(a.ClockIn).Date,
                                            currentdate = DateTime.Now,
                                            dep = a.Tbl_EmployeeRegistration.Department.Trim(),
                                            status = "Present"

                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=attendancesummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=attendancesummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=attendancesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            return (null);

                        }
                        else if (dropdown == "<8")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.Emp_ID == Convert.ToInt32(EmpId) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= previousWeekStart && Convert.ToDateTime(a.ClockIn).Date <= previousWeekEnd
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            empid = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            date = Convert.ToDateTime(a.ClockIn).Date,
                                            currentdate = DateTime.Now,
                                            dep = a.Tbl_EmployeeRegistration.Department.Trim(),
                                            status = "Present"

                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=attendancesummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=attendancesummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=attendancesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            return (null);

                        }
                        else if (dropdown == "<30")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.Emp_ID == Convert.ToInt32(EmpId) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= firstDayLastMonth && Convert.ToDateTime(a.ClockIn).Date <= lastDayLastMonth
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            empid = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            date = Convert.ToDateTime(a.ClockIn).Date,
                                            currentdate = DateTime.Now,
                                            dep = a.Tbl_EmployeeRegistration.Department.Trim(),
                                            status = "Present"

                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=attendancesummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=attendancesummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=attendancesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            return (null);

                        }
                        else if (dropdown == "30")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.Emp_ID == Convert.ToInt32(EmpId) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= thisMonthStart && Convert.ToDateTime(a.ClockIn).Date <= thisMonthEnd
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            empid = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            date = Convert.ToDateTime(a.ClockIn).Date,
                                            currentdate = DateTime.Now,
                                            dep = a.Tbl_EmployeeRegistration.Department.Trim(),
                                            status = "Present"

                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=attendancesummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=attendancesummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=attendancesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            return (null);

                        }
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    if (datefrom.ToString() != "01-01-01 00:00:00")
                    {
                        var data = (from a in db.Tbl_Attendances
                                    where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= datefrom.Date && Convert.ToDateTime(a.ClockIn).Date <= todate.Date
                                    orderby a.ClockIn ascending
                                    select new
                                    {
                                        empid = a.Emp_ID,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        date = Convert.ToDateTime(a.ClockIn).Date,
                                        currentdate = DateTime.Now,
                                        dep = a.Tbl_EmployeeRegistration.Department.Trim(),
                                        status = "Present"

                                    }).ToList();
                        if (reporttype == "pdf")
                        {
                            if (button == "tsprint")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=attendancesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            else
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "attachment;filename=attendancesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                        }
                        else if (reporttype == "excel")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "Excel";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "xls";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=attendancesummery." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }
                        return (null);
                    }
                    else
                    {
                        DateTime baseDate = DateTime.Today;

                        var today = baseDate;
                        var yesterday = baseDate.AddDays(-1);
                        var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
                        var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
                        var previousWeekStart = thisWeekStart.AddDays(-7);
                        var previousWeekEnd = thisWeekStart.AddDays(1);
                        var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
                        var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
                        var firstDayLastMonth = thisMonthStart.AddMonths(-1);
                        var lastDayLastMonth = thisMonthStart.AddSeconds(-1);
                        if (dropdown == "-1")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date == DateTime.Now.Date.AddDays(-1)
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            empid = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            date = Convert.ToDateTime(a.ClockIn).Date,
                                            currentdate = DateTime.Now,
                                            dep = a.Tbl_EmployeeRegistration.Department.Trim(),
                                            status = "Present"

                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=attendancesummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=attendancesummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=attendancesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            return (null);

                        }
                        else if (dropdown == "1")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date == DateTime.Now.Date
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            empid = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            status = "Present",
                                            date = Convert.ToDateTime(a.ClockIn).Date,
                                            currentdate = DateTime.Now,
                                            dep = a.Tbl_EmployeeRegistration.Department.Trim()

                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=attendancesummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=attendancesummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=attendancesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            return (null);

                        }
                        else if (dropdown == "7")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= thisWeekStart && Convert.ToDateTime(a.ClockIn).Date <= thisWeekEnd
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            empid = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            date = Convert.ToDateTime(a.ClockIn).Date,
                                            currentdate = DateTime.Now,
                                            dep = a.Tbl_EmployeeRegistration.Department.Trim(),
                                            status = "Present"

                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=attendancesummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=attendancesummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=attendancesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            return (null);

                        }
                        else if (dropdown == "<8")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= previousWeekStart && Convert.ToDateTime(a.ClockIn).Date <= previousWeekEnd
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            empid = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            date = Convert.ToDateTime(a.ClockIn).Date,
                                            currentdate = DateTime.Now,
                                            dep = a.Tbl_EmployeeRegistration.Department.Trim(),
                                            status = "Present"

                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=attendancesummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=attendancesummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=attendancesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            return (null);

                        }
                        else if (dropdown == "<30")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= firstDayLastMonth && Convert.ToDateTime(a.ClockIn).Date <= lastDayLastMonth
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            empid = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            date = Convert.ToDateTime(a.ClockIn).Date,
                                            currentdate = DateTime.Now,
                                            dep = a.Tbl_EmployeeRegistration.Department.Trim(),
                                            status = "Present"

                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=attendancesummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=attendancesummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=attendancesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            return (null);
                        }
                        else if (dropdown == "30")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= thisMonthStart && Convert.ToDateTime(a.ClockIn).Date <= thisMonthEnd
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            empid = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            date = Convert.ToDateTime(a.ClockIn).Date,
                                            currentdate = DateTime.Now,
                                            dep = a.Tbl_EmployeeRegistration.Department.Trim(),
                                            status = "Present"

                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=attendancesummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=attendancesummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=attendancesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            return (null);

                        }
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                var id = (from b in db.Tbl_EmployeeRegistrations where b.Emp_Name.Trim() == EmpId.Trim() select b.Emp_id).SingleOrDefault();

                if (datefrom.ToString() != "01-01-01 00:00:00")
                {

                    var data = (from a in db.Tbl_Attendances
                                where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.Emp_ID == Convert.ToInt32(id) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= datefrom.Date && Convert.ToDateTime(a.ClockIn).Date <= todate.Date
                                orderby a.ClockIn ascending
                                select new
                                {
                                    empid = a.Emp_ID,
                                    name = a.Tbl_EmployeeRegistration.Emp_Name,
                                    date = Convert.ToDateTime(a.ClockIn).Date,
                                    currentdate = DateTime.Now,
                                    dep = a.Tbl_EmployeeRegistration.Department.Trim(),
                                    status = "Present"

                                }).ToList();
                    if (reporttype == "pdf")
                    {
                        if (button == "tsprint")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "pdf";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "pdf";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=attendancesummery." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }
                        else
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "pdf";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "pdf";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "attachment;filename=attendancesummery." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }
                    }
                    else if (reporttype == "excel")
                    {
                        LocalReport lc1 = new LocalReport();
                        lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                        ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                        rd1.Value = data;
                        lc1.DataSources.Add(rd1);
                        reporttype = "Excel";
                        string mimetype1;
                        string encoding1;
                        string filenameextention1;


                        filenameextention1 = "xls";
                        string[] strems1;
                        Warning[] warnings1;
                        byte[] renderdbyte1;
                        renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                            , out strems1, out warnings1);
                        Response.AddHeader("content-disposition", "inline;filename=attendancesummery." + filenameextention1);
                        return File(renderdbyte1, "." + filenameextention1);
                    }
                    return (null);
                }
                else
                {
                    DateTime baseDate = DateTime.Today;

                    var today = baseDate;
                    var yesterday = baseDate.AddDays(-1);
                    var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
                    var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
                    var previousWeekStart = thisWeekStart.AddDays(-7);
                    var previousWeekEnd = thisWeekStart.AddDays(1);
                    var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
                    var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
                    var firstDayLastMonth = thisMonthStart.AddMonths(-1);
                    var lastDayLastMonth = thisMonthStart.AddSeconds(-1);
                    if (dropdown == "-1")
                    {
                        var data = (from a in db.Tbl_Attendances
                                    where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.Emp_ID == Convert.ToInt32(id) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date == DateTime.Now.Date.AddDays(-1)
                                    orderby a.ClockIn ascending
                                    select new
                                    {
                                        empid = a.Emp_ID,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        date = Convert.ToDateTime(a.ClockIn).Date,
                                        currentdate = DateTime.Now,
                                        dep = a.Tbl_EmployeeRegistration.Department.Trim(),
                                        status = "Present"

                                    }).ToList();
                        if (reporttype == "pdf")
                        {
                            if (button == "tsprint")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=attendancesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            else
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "attachment;filename=attendancesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                        }
                        else if (reporttype == "excel")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "Excel";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "xls";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=attendancesummery." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }
                        return (null);

                    }
                    else if (dropdown == "1")
                    {
                        var data = (from a in db.Tbl_Attendances
                                    where a.Emp_ID == Convert.ToInt32(id) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date == DateTime.Now.Date
                                    orderby a.ClockIn ascending
                                    select new
                                    {
                                        empid = a.Emp_ID,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        date = Convert.ToDateTime(a.ClockIn).Date,
                                        currentdate = DateTime.Now,
                                        dep = a.Tbl_EmployeeRegistration.Department.Trim(),
                                        status = "Present"

                                    }).ToList();
                        if (reporttype == "pdf")
                        {
                            if (button == "tsprint")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=attendancesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            else
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "attachment;filename=attendancesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                        }
                        else if (reporttype == "excel")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "Excel";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "xls";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=attendancesummery." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }
                        return (null);

                    }
                    else if (dropdown == "7")
                    {
                        var data = (from a in db.Tbl_Attendances
                                    where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.Emp_ID == Convert.ToInt32(id) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= thisWeekStart && Convert.ToDateTime(a.ClockIn).Date <= thisWeekEnd
                                    orderby a.ClockIn ascending
                                    select new
                                    {
                                        empid = a.Emp_ID,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        date = Convert.ToDateTime(a.ClockIn).Date,
                                        currentdate = DateTime.Now,
                                        dep = a.Tbl_EmployeeRegistration.Department.Trim(),
                                        status = "Present"

                                    }).ToList();
                        if (reporttype == "pdf")
                        {
                            if (button == "tsprint")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=attendancesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            else
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "attachment;filename=attendancesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                        }
                        else if (reporttype == "excel")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "Excel";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "xls";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=attendancesummery." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }
                        return (null);

                    }
                    else if (dropdown == "<8")
                    {
                        var data = (from a in db.Tbl_Attendances
                                    where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.Emp_ID == Convert.ToInt32(id) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= previousWeekStart && Convert.ToDateTime(a.ClockIn).Date <= previousWeekEnd
                                    orderby a.ClockIn ascending
                                    select new
                                    {
                                        empid = a.Emp_ID,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        date = Convert.ToDateTime(a.ClockIn).Date,
                                        currentdate = DateTime.Now,
                                        dep = a.Tbl_EmployeeRegistration.Department.Trim(),
                                        status = "Present"

                                    }).ToList();
                        if (reporttype == "pdf")
                        {
                            if (button == "tsprint")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=attendancesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            else
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "attachment;filename=attendancesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                        }
                        else if (reporttype == "excel")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "Excel";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "xls";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=attendancesummery." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }
                        return (null);

                    }
                    else if (dropdown == "<30")
                    {
                        var data = (from a in db.Tbl_Attendances
                                    where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.Emp_ID == Convert.ToInt32(id) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= firstDayLastMonth && Convert.ToDateTime(a.ClockIn).Date <= lastDayLastMonth
                                    orderby a.ClockIn ascending
                                    select new
                                    {
                                        empid = a.Emp_ID,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        date = Convert.ToDateTime(a.ClockIn).Date,
                                        currentdate = DateTime.Now,
                                        dep = a.Tbl_EmployeeRegistration.Department.Trim(),
                                        status = "Present"

                                    }).ToList();
                        if (reporttype == "pdf")
                        {
                            if (button == "tsprint")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=attendancesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            else
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "attachment;filename=attendancesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                        }
                        else if (reporttype == "excel")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "Excel";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "xls";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=attendancesummery." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }
                        return (null);

                    }
                    else if (dropdown == "30")
                    {
                        var data = (from a in db.Tbl_Attendances
                                    where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.Emp_ID == Convert.ToInt32(id) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= thisMonthStart && Convert.ToDateTime(a.ClockIn).Date <= thisMonthEnd
                                    orderby a.ClockIn ascending
                                    select new
                                    {
                                        empid = a.Emp_ID,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        date = Convert.ToDateTime(a.ClockIn).Date,
                                        currentdate = DateTime.Now,
                                        dep = a.Tbl_EmployeeRegistration.Department.Trim(),
                                        status = "Present"

                                    }).ToList();
                        if (reporttype == "pdf")
                        {
                            if (button == "tsprint")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=attendancesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            else
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "attachment;filename=attendancesummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                        }
                        else if (reporttype == "excel")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/Rptattendancesummery.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("attendancesummery", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "Excel";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "xls";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=attendancesummery." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }
                        return (null);

                    }
                    return Json(null, JsonRequestBehavior.AllowGet);
                }

            }
        }
        public ActionResult timesheetsummeryprint(string EmpId, string Emp_Name, DateTime datefrom, DateTime todate, string dropdown, string reporttype, string button)
        {
            try
            {
                if (EmpId != "" && EmpId != null)
                {

                    if (datefrom.ToString() != "01-01-01 00:00:00")
                    {
                        var data = (from a in db.Tbl_Attendances
                                    where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.Emp_ID == Convert.ToInt32(EmpId) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= datefrom.Date && Convert.ToDateTime(a.ClockIn).Date <= todate.Date
                                    orderby a.ClockIn ascending
                                    select new
                                    {
                                        empid = a.Emp_ID,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        dep = a.Tbl_EmployeeRegistration.Department.Trim(),
                                        date = Convert.ToDateTime(a.ClockIn).Date,
                                        start = a.ClockIn,
                                        end = a.ClockOut,
                                        totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                        currentdate = DateTime.Now,
                                     
                                    }).ToList();
                        if (reporttype == "pdf")
                        {
                            if (button == "tsprint")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=timesheetsummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            else
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "attachment;filename=timesheetsummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                        }
                        else if (reporttype == "excel")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "Excel";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "xls";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=timesheetsummery." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }
                        return (null);
                    }
                    else
                    {
                        DateTime baseDate = DateTime.Today;

                        var today = baseDate;
                        var yesterday = baseDate.AddDays(-1);
                        var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
                        var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
                        var previousWeekStart = thisWeekStart.AddDays(-7);
                        var previousWeekEnd = thisWeekStart.AddDays(1);
                        var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
                        var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
                        var firstDayLastMonth = thisMonthStart.AddMonths(-1);
                        var lastDayLastMonth = thisMonthStart.AddSeconds(-1);
                        if (dropdown == "-1")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.Emp_ID == Convert.ToInt32(EmpId) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date == DateTime.Now.Date.AddDays(-1)
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            empid = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department.Trim(),
                                            date = Convert.ToDateTime(a.ClockIn).Date,
                                            start = a.ClockIn,
                                            end = a.ClockOut,
                                            totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                            currentdate = DateTime.Now,

                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=timesheetsummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=timesheetsummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=timesheetsummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            return (null);

                        }
                        else if (dropdown == "1")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.Emp_ID == Convert.ToInt32(EmpId) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date == DateTime.Now.Date
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            empid = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department.Trim(),
                                            date = Convert.ToDateTime(a.ClockIn).Date,
                                            start = a.ClockIn,
                                            end = a.ClockOut,
                                            totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                            currentdate = DateTime.Now,

                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=timesheetsummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=timesheetsummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=timesheetsummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            return (null);

                        }
                        else if (dropdown == "7")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.Emp_ID == Convert.ToInt32(EmpId) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= thisWeekStart && Convert.ToDateTime(a.ClockIn).Date <= thisWeekEnd
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            empid = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department.Trim(),
                                            date = Convert.ToDateTime(a.ClockIn).Date,
                                            start = a.ClockIn,
                                            end = a.ClockOut,
                                            totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                            currentdate = DateTime.Now,

                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=timesheetsummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=timesheet." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=timesheetsummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            return (null);

                        }
                        else if (dropdown == "<8")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.Emp_ID == Convert.ToInt32(EmpId) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= previousWeekStart && Convert.ToDateTime(a.ClockIn).Date <= previousWeekEnd
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            empid = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department.Trim(),
                                            date = Convert.ToDateTime(a.ClockIn).Date,
                                            start = a.ClockIn,
                                            end = a.ClockOut,
                                            totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                            currentdate = DateTime.Now,

                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=timesheetsummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=timesheetsummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=timesheetsummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            return (null);

                        }
                        else if (dropdown == "<30")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.Emp_ID == Convert.ToInt32(EmpId) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= firstDayLastMonth && Convert.ToDateTime(a.ClockIn).Date <= lastDayLastMonth
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            empid = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department.Trim(),
                                            date = Convert.ToDateTime(a.ClockIn).Date,
                                            start = a.ClockIn,
                                            end = a.ClockOut,
                                            totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                            currentdate = DateTime.Now,

                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=timesheetsummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=timesheetsummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=timesheetsummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            return (null);

                        }
                        else if (dropdown == "30")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.Emp_ID == Convert.ToInt32(EmpId) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= thisMonthStart && Convert.ToDateTime(a.ClockIn).Date <= thisMonthEnd
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            empid = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department.Trim(),
                                            date = Convert.ToDateTime(a.ClockIn).Date,
                                            start = a.ClockIn,
                                            end = a.ClockOut,
                                            totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                            currentdate = DateTime.Now,

                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=timesheetsummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=timesheetsummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=timesheetsummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            return (null);

                        }
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    if (datefrom.ToString() != "01-01-01 00:00:00")
                    {
                        var data = (from a in db.Tbl_Attendances
                                    where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= datefrom.Date && Convert.ToDateTime(a.ClockIn).Date <= todate.Date
                                    orderby a.ClockIn ascending
                                    select new
                                    {
                                        empid = a.Emp_ID,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        dep = a.Tbl_EmployeeRegistration.Department.Trim(),
                                        date = Convert.ToDateTime(a.ClockIn).Date,
                                        start = a.ClockIn,
                                        end = a.ClockOut,
                                        totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                        currentdate = DateTime.Now,

                                    }).ToList();
                        if (reporttype == "pdf")
                        {
                            if (button == "tsprint")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=timesheetsummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            else
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "attachment;filename=timesheetsummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                        }
                        else if (reporttype == "excel")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "Excel";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "xls";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=timesheetsummery." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }
                        return (null);
                    }
                    else
                    {
                        DateTime baseDate = DateTime.Today;

                        var today = baseDate;
                        var yesterday = baseDate.AddDays(-1);
                        var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
                        var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
                        var previousWeekStart = thisWeekStart.AddDays(-7);
                        var previousWeekEnd = thisWeekStart.AddDays(1);
                        var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
                        var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
                        var firstDayLastMonth = thisMonthStart.AddMonths(-1);
                        var lastDayLastMonth = thisMonthStart.AddSeconds(-1);
                        if (dropdown == "-1")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date == DateTime.Now.Date.AddDays(-1)
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            empid = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department.Trim(),
                                            date = Convert.ToDateTime(a.ClockIn).Date,
                                            start = a.ClockIn,
                                            end = a.ClockOut,
                                            totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                            currentdate = DateTime.Now,

                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=timesheetsummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=timesheetsummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=timesheetsummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            return (null);

                        }
                        else if (dropdown == "1")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date == DateTime.Now.Date
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            empid = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department.Trim(),
                                            date = Convert.ToDateTime(a.ClockIn).Date,
                                            start = a.ClockIn,
                                            end = a.ClockOut,
                                            totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                            currentdate = DateTime.Now,

                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=timesheetsummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=timesheetsummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=timesheetsummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            return (null);

                        }
                        else if (dropdown == "7")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= thisWeekStart && Convert.ToDateTime(a.ClockIn).Date <= thisWeekEnd
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            empid = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department.Trim(),
                                            date = Convert.ToDateTime(a.ClockIn).Date,
                                            start = a.ClockIn,
                                            end = a.ClockOut,
                                            totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                            currentdate = DateTime.Now,

                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=timesheetsummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=timesheetsummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=timesheetsummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            return (null);

                        }
                        else if (dropdown == "<8")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= previousWeekStart && Convert.ToDateTime(a.ClockIn).Date <= previousWeekEnd
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            empid = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department.Trim(),
                                            date = Convert.ToDateTime(a.ClockIn).Date,
                                            start = a.ClockIn,
                                            end = a.ClockOut,
                                            totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                            currentdate = DateTime.Now,

                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=timesheetsummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=timesheetsummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=timesheetsummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            return (null);

                        }
                        else if (dropdown == "<30")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= firstDayLastMonth && Convert.ToDateTime(a.ClockIn).Date <= lastDayLastMonth
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            empid = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department.Trim(),
                                            date = Convert.ToDateTime(a.ClockIn).Date,
                                            start = a.ClockIn,
                                            end = a.ClockOut,
                                            totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                            currentdate = DateTime.Now,

                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=timesheet." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=timesheetsummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=timesheetsummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            return (null);
                        }
                        else if (dropdown == "30")
                        {
                            var data = (from a in db.Tbl_Attendances
                                        where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= thisMonthStart && Convert.ToDateTime(a.ClockIn).Date <= thisMonthEnd
                                        orderby a.ClockIn ascending
                                        select new
                                        {
                                            empid = a.Emp_ID,
                                            name = a.Tbl_EmployeeRegistration.Emp_Name,
                                            dep = a.Tbl_EmployeeRegistration.Department.Trim(),
                                            date = Convert.ToDateTime(a.ClockIn).Date,
                                            start = a.ClockIn,
                                            end = a.ClockOut,
                                            totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                            currentdate = DateTime.Now,

                                        }).ToList();
                            if (reporttype == "pdf")
                            {
                                if (button == "tsprint")
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "inline;filename=timesheetsummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                                else
                                {
                                    LocalReport lc1 = new LocalReport();
                                    lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                                    ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                                    rd1.Value = data;
                                    lc1.DataSources.Add(rd1);
                                    reporttype = "pdf";
                                    string mimetype1;
                                    string encoding1;
                                    string filenameextention1;


                                    filenameextention1 = "pdf";
                                    string[] strems1;
                                    Warning[] warnings1;
                                    byte[] renderdbyte1;
                                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                        , out strems1, out warnings1);
                                    Response.AddHeader("content-disposition", "attachment;filename=timesheetsummery." + filenameextention1);
                                    return File(renderdbyte1, "." + filenameextention1);
                                }
                            }
                            else if (reporttype == "excel")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "Excel";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "xls";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=timesheetsummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            return (null);

                        }
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                var id = (from b in db.Tbl_EmployeeRegistrations where b.Emp_Name.Trim() == EmpId.Trim() select b.Emp_id).SingleOrDefault();

                if (datefrom.ToString() != "01-01-01 00:00:00")
                {

                    var data = (from a in db.Tbl_Attendances
                                where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.Emp_ID == Convert.ToInt32(id) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= datefrom.Date && Convert.ToDateTime(a.ClockIn).Date <= todate.Date
                                orderby a.ClockIn ascending
                                select new
                                {
                                    empid = a.Emp_ID,
                                    name = a.Tbl_EmployeeRegistration.Emp_Name,
                                    dep = a.Tbl_EmployeeRegistration.Department.Trim(),
                                    date = Convert.ToDateTime(a.ClockIn).Date,
                                    start = a.ClockIn,
                                    end = a.ClockOut,
                                    totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                    currentdate = DateTime.Now,

                                }).ToList();
                    if (reporttype == "pdf")
                    {
                        if (button == "tsprint")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "pdf";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "pdf";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=timesheetsummery." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }
                        else
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "pdf";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "pdf";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "attachment;filename=timesheetsummery." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }
                    }
                    else if (reporttype == "excel")
                    {
                        LocalReport lc1 = new LocalReport();
                        lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                        ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                        rd1.Value = data;
                        lc1.DataSources.Add(rd1);
                        reporttype = "Excel";
                        string mimetype1;
                        string encoding1;
                        string filenameextention1;


                        filenameextention1 = "xls";
                        string[] strems1;
                        Warning[] warnings1;
                        byte[] renderdbyte1;
                        renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                            , out strems1, out warnings1);
                        Response.AddHeader("content-disposition", "inline;filename=timesheetsummery." + filenameextention1);
                        return File(renderdbyte1, "." + filenameextention1);
                    }
                    return (null);
                }
                else
                {
                    DateTime baseDate = DateTime.Today;

                    var today = baseDate;
                    var yesterday = baseDate.AddDays(-1);
                    var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
                    var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
                    var previousWeekStart = thisWeekStart.AddDays(-7);
                    var previousWeekEnd = thisWeekStart.AddDays(1);
                    var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
                    var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
                    var firstDayLastMonth = thisMonthStart.AddMonths(-1);
                    var lastDayLastMonth = thisMonthStart.AddSeconds(-1);
                    if (dropdown == "-1")
                    {
                        var data = (from a in db.Tbl_Attendances
                                    where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.Emp_ID == Convert.ToInt32(id) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date == DateTime.Now.Date.AddDays(-1)
                                    orderby a.ClockIn ascending
                                    select new
                                    {
                                        empid = a.Emp_ID,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        dep = a.Tbl_EmployeeRegistration.Department.Trim(),
                                        date = Convert.ToDateTime(a.ClockIn).Date,
                                        start = a.ClockIn,
                                        end = a.ClockOut,
                                        totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                        currentdate = DateTime.Now,

                                    }).ToList();
                        if (reporttype == "pdf")
                        {
                            if (button == "tsprint")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=timesheetsummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            else
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "attachment;filename=timesheetsummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                        }
                        else if (reporttype == "excel")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "Excel";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "xls";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=timesheetsummery." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }
                        return (null);

                    }
                    else if (dropdown == "1")
                    {
                        var data = (from a in db.Tbl_Attendances
                                    where a.Emp_ID == Convert.ToInt32(id) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date == DateTime.Now.Date
                                    orderby a.ClockIn ascending
                                    select new
                                    {
                                        empid = a.Emp_ID,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        dep = a.Tbl_EmployeeRegistration.Department.Trim(),
                                        date = Convert.ToDateTime(a.ClockIn).Date,
                                        start = a.ClockIn,
                                        end = a.ClockOut,
                                        totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                        currentdate = DateTime.Now,

                                    }).ToList();
                        if (reporttype == "pdf")
                        {
                            if (button == "tsprint")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=timesheetsummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            else
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "attachment;filename=timesheetsummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                        }
                        else if (reporttype == "excel")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "Excel";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "xls";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=timesheetsummery." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }
                        return (null);

                    }
                    else if (dropdown == "7")
                    {
                        var data = (from a in db.Tbl_Attendances
                                    where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.Emp_ID == Convert.ToInt32(id) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= thisWeekStart && Convert.ToDateTime(a.ClockIn).Date <= thisWeekEnd
                                    orderby a.ClockIn ascending
                                    select new
                                    {
                                        empid = a.Emp_ID,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        dep = a.Tbl_EmployeeRegistration.Department.Trim(),
                                        date = Convert.ToDateTime(a.ClockIn).Date,
                                        start = a.ClockIn,
                                        end = a.ClockOut,
                                        totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                        currentdate = DateTime.Now,

                                    }).ToList();
                        if (reporttype == "pdf")
                        {
                            if (button == "tsprint")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=timesheetsummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            else
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "attachment;filename=timesheetsummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                        }
                        else if (reporttype == "excel")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "Excel";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "xls";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=timesheetsummery." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }
                        return (null);

                    }
                    else if (dropdown == "<8")
                    {
                        var data = (from a in db.Tbl_Attendances
                                    where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.Emp_ID == Convert.ToInt32(id) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= previousWeekStart && Convert.ToDateTime(a.ClockIn).Date <= previousWeekEnd
                                    orderby a.ClockIn ascending
                                    select new
                                    {
                                        empid = a.Emp_ID,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        dep = a.Tbl_EmployeeRegistration.Department.Trim(),
                                        date = Convert.ToDateTime(a.ClockIn).Date,
                                        start = a.ClockIn,
                                        end = a.ClockOut,
                                        totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                        currentdate = DateTime.Now,

                                    }).ToList();
                        if (reporttype == "pdf")
                        {
                            if (button == "tsprint")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=timesheetsummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            else
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "attachment;filename=timesheetsummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                        }
                        else if (reporttype == "excel")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "Excel";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "xls";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=timesheetsummery." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }
                        return (null);

                    }
                    else if (dropdown == "<30")
                    {
                        var data = (from a in db.Tbl_Attendances
                                    where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.Emp_ID == Convert.ToInt32(id) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= firstDayLastMonth && Convert.ToDateTime(a.ClockIn).Date <= lastDayLastMonth
                                    orderby a.ClockIn ascending
                                    select new
                                    {
                                        empid = a.Emp_ID,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        dep = a.Tbl_EmployeeRegistration.Department.Trim(),
                                        date = Convert.ToDateTime(a.ClockIn).Date,
                                        start = a.ClockIn,
                                        end = a.ClockOut,
                                        totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                        currentdate = DateTime.Now,

                                    }).ToList();
                        if (reporttype == "pdf")
                        {
                            if (button == "tsprint")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=timesheetsummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            else
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "attachment;filename=timesheetsummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                        }
                        else if (reporttype == "excel")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "Excel";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "xls";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=timesheetsummery." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }
                        return (null);

                    }
                    else if (dropdown == "30")
                    {
                        var data = (from a in db.Tbl_Attendances
                                    where Convert.ToDateTime(a.ClockIn).Date != DateTime.Now.Date && a.Emp_ID == Convert.ToInt32(id) && a.ClockIn != null && Convert.ToDateTime(a.ClockIn).Date >= thisMonthStart && Convert.ToDateTime(a.ClockIn).Date <= thisMonthEnd
                                    orderby a.ClockIn ascending
                                    select new
                                    {
                                        empid = a.Emp_ID,
                                        name = a.Tbl_EmployeeRegistration.Emp_Name,
                                        dep = a.Tbl_EmployeeRegistration.Department.Trim(),
                                        date = Convert.ToDateTime(a.ClockIn).Date,
                                        start = a.ClockIn,
                                        end = a.ClockOut,
                                        totalhr = Convert.ToDouble(Convert.ToDateTime(a.ClockOut ?? a.ClockIn).Subtract(Convert.ToDateTime(a.ClockIn)).TotalHours),
                                        currentdate = DateTime.Now,

                                    }).ToList();
                        if (reporttype == "pdf")
                        {
                            if (button == "tsprint")
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheet.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("Rpttimesheetsummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "inline;filename=timesheet." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                            else
                            {
                                LocalReport lc1 = new LocalReport();
                                lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                                ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                                rd1.Value = data;
                                lc1.DataSources.Add(rd1);
                                reporttype = "pdf";
                                string mimetype1;
                                string encoding1;
                                string filenameextention1;


                                filenameextention1 = "pdf";
                                string[] strems1;
                                Warning[] warnings1;
                                byte[] renderdbyte1;
                                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                    , out strems1, out warnings1);
                                Response.AddHeader("content-disposition", "attachment;filename=timesheetsummery." + filenameextention1);
                                return File(renderdbyte1, "." + filenameextention1);
                            }
                        }
                        else if (reporttype == "excel")
                        {
                            LocalReport lc1 = new LocalReport();
                            lc1.ReportPath = Server.MapPath("/Reports/Rpttimesheetsummery.rdlc");
                            ReportDataSource rd1 = new ReportDataSource("timesheetsummery", data.ToList());
                            rd1.Value = data;
                            lc1.DataSources.Add(rd1);
                            reporttype = "Excel";
                            string mimetype1;
                            string encoding1;
                            string filenameextention1;


                            filenameextention1 = "xls";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline;filename=timesheetsummery." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }
                        return (null);

                    }
                    return Json(null, JsonRequestBehavior.AllowGet);
                }

            }
        }
        public JsonResult GetleavedatabyDep(EmployeeRegistration employee)
        {
            try
            {
                var data = (from a in db.Tbl_EmployeeRegistrations
                            where a.Designation == employee.Designation
                            group a by a.Emp_id into g
                            select new
                            {
                                id = g.First().Emp_id,
                                name = g.First().Emp_Name,
                                datefrom =db.Tbl_EmpLeaves.OrderByDescending(t=>t.LeaveDetailId).Where(t=>t.Emp_Id==g.First().Emp_id&&t.Stetus==0).Select(t=>t.From_Date).FirstOrDefault(),
                                dateto = db.Tbl_EmpLeaves.OrderByDescending(t => t.LeaveDetailId).Where(t => t.Emp_Id == g.First().Emp_id && t.Stetus == 0).Select(t => t.ToDate).FirstOrDefault(),
                                type = db.Tbl_EmpLeaves.OrderByDescending(t => t.LeaveDetailId).Where(t => t.Emp_Id == g.First().Emp_id && t.Stetus == 0).Select(t => t.LeaveType).FirstOrDefault(),
                                comment = db.Tbl_EmpLeaves.OrderByDescending(t => t.LeaveDetailId).Where(t => t.Emp_Id == g.First().Emp_id && t.Stetus == 0).Select(t => t.Comment).FirstOrDefault(),
                            }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public JsonResult showleavesReuest(EmployeeRegistration employee)
        {
            var data = (from a in db.Tbl_EmpLeaves
                        orderby a.LeaveDetailId descending
                        group a by a.Emp_Id into g
                       
                        where g.First().Stetus == 0 select
                     
                        new
            {
                id = g.First().Emp_Id,
                name= g.First().Tbl_EmployeeRegistration.Emp_Name,
                datefrom= g.First().From_Date,
                dateto=Convert.ToDateTime(g.First().ToDate),
                type= g.First().LeaveType,
                comment= g.First().Comment,
                detailid=g.First().LeaveDetailId,
            
            }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Designation(EmployeeRegistration employee)
        {
            var data = (from a in db.Tbl_EmployeeRegistrations select a.Designation).Distinct().ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult approveLeave(EmployeeRegistration employee)
        {
            Tbl_EmpLeave t = db.Tbl_EmpLeaves.OrderByDescending(s => s.LeaveDetailId).FirstOrDefault(s => s.Emp_Id == Convert.ToInt32(employee.EmpId));
            t.Stetus = 1;
            db.SubmitChanges();
            var message = "Done";
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        public JsonResult rejectLeave(EmployeeRegistration employee)
        {
            Tbl_EmpLeave t = db.Tbl_EmpLeaves.OrderByDescending(s => s.LeaveDetailId).FirstOrDefault(s => s.Emp_Id == Convert.ToInt32(employee.EmpId));
            t.Stetus = 2;
            db.SubmitChanges();
            var message = "Done";
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Getemployeedata(EmployeeRegistration employee)
        {
            /*ar findid = (from a in db.Tbl_EmployeeRegistrations where a.UserName == employee.EmpId select a.Emp_id).SingleOrDefault();*/
            try
            {
                var data = (from a in db.Tbl_EmpEduDetails
                            where a.Emp_id == Convert.ToInt32(employee.EmpId)
                            select new
                            {
                                a.Emp_id,
                                a.Tbl_EmployeeRegistration.AccountNo,
                                a.Tbl_EmployeeRegistration.BankAccountName,
                                a.Tbl_EmployeeRegistration.BankName,

                                a.Tbl_EmployeeRegistration.BasicSalary,
                                a.Tbl_EmployeeRegistration.BranchName,
                                a.Tbl_EmployeeRegistration.Department,
                                a.Tbl_EmployeeRegistration.Designation,
                                a.Tbl_EmployeeRegistration.EmailId,
                                Emp_DOB=Convert.ToDateTime( a.Tbl_EmployeeRegistration.Emp_DOB).AddDays(1),
                                Emp_DOJ =Convert.ToDateTime( a.Tbl_EmployeeRegistration.Emp_DOJ).AddDays(1),
                                a.Tbl_EmployeeRegistration.Emp_FatherName,
                                a.Tbl_EmployeeRegistration.Emp_Homeno,
                                a.Tbl_EmployeeRegistration.Emp_Mobile,
                                a.Tbl_EmployeeRegistration.Emp_Name,
                                a.Tbl_EmployeeRegistration.Emp_Passport_No,
                                a.Tbl_EmployeeRegistration.Gender,
                                a.Tbl_EmployeeRegistration.IFSCcode,
                                a.Tbl_EmployeeRegistration.MeritialStetus,
                                a.Tbl_EmployeeRegistration.PanNo,
                                a.Tbl_EmployeeRegistration.Password,
                                a.Tbl_EmployeeRegistration.UserName,
                                a.Tbl_EmployeeRegistration.profilepath,
                                a.Board_name,
                                a.Detail_Id,
                                a.G_City,
                                a.G_colgname,
                                a.G_marks,
                                a.G_State,
                                a.G_University,
                                a.G_yearOfPassing,
                                a.PGMarks,
                                a.PG_City,
                                a.PG_Colgname,
                                a.PG_State,
                                a.PG_Universityname,
                                a.PG_yearofPassing,
                                a.school_name,
                                a.S_City,
                                a.S_Marks,
                                a.S_state,
                                a.S_yaerofpassing,
                                a._12_Board,
                                a._12_City,
                                a._12_ColgName,
                                a._12_marks,
                                a._12_State,
                                a._12_yearofpassing,
                            }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                var data = "No Records Found!!!";
                return Json(data, JsonRequestBehavior.AllowGet);
            }

        }
       
        public JsonResult salary(salary salary)
        {
            Tbl_Payroll t = new Tbl_Payroll();
            t.PayrollID = salarymaxid();
            t.Datefrom = salary.DateFrom;
            t.Dateto = salary.DateTo;
            t.EmpID = salary.EmpId;
            t.BasicSalary =Convert.ToDouble(String.Format("{0:0.00}", salary.basicsalary));
            t.leavdays = salary.leavedays;
            t.LeaveDeduction = salary.leavededuct;
            t.OtherDeduction = salary.otherdeduction;
            t.otherpay = salary.otherpay;
            t.PresentDays = salary.presentdays;
            t.ProfTax = salary.profTax;
            t.salaryperday = Convert.ToDouble(String.Format("{0:0.00}", salary.salaryperday)); 
            t.TotalDays = salary.totaldays;
            t.TotalDays = salary.totaldays;
            t.TotalEarning = Convert.ToDouble(String.Format("{0:0.00}", salary.totalearning)); 
            t.TotalPay = Convert.ToDouble(String.Format("{0:0.00}", salary.totalpay)); 
            t.Date = DateTime.Now;
            t.TDS = salary.TDS;
           t.TotalEarninginword=NumberToWords(Convert.ToInt32(salary.totalearning));
            db.Tbl_Payrolls.InsertOnSubmit(t);
            db.SubmitChanges();


            var message = "Records Submited Successfully";
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        public int salarymaxid()
        {
            try
            {
                var Id = (from a in db.Tbl_Payrolls select a.PayrollID).Max();
                return Id + 1;
            }
            catch (Exception)
            {

                return 1;
            }
        }
        public  int leavemaxid()
        {
            try
            {
                var Id = (from a in db.Tbl_EmpLeaves select a.LeaveDetailId).Max();
               return  Id+1;
            }
            catch (Exception)
            {

                return 1;
            }
        }
public JsonResult  RequestLeave(Sendleaves employee)
        {
            Tbl_EmpLeave t = new Tbl_EmpLeave();
            t.LeaveDetailId = leavemaxid();
            t.Emp_Id = employee.EmpId;
            t.From_Date = employee.DateFrom;
            t.ToDate = employee.DateTo;
            t.Comment = employee.comment;
            t.Stetus = 0;
            t.LeaveType = employee.leavetype;
            t.Date = DateTime.Now;
           db.Tbl_EmpLeaves.InsertOnSubmit(t);
            db.SubmitChanges();
            var message = "Request Submited Successfully";
            return Json(message, JsonRequestBehavior.AllowGet);
        }

            public JsonResult checkusername(EmployeeRegistration employee)
        {
            var check="a";
            var data = (from a in db.Tbl_EmployeeRegistrations where a.UserName == employee.username.Trim() select a).SingleOrDefault();
            if(data==null)
            {
                check = "false";
                return Json(check, JsonRequestBehavior.AllowGet);
            }
            else
            {
                check = "true";
                return Json(check, JsonRequestBehavior.AllowGet);
            }
          
        }

        public int maxedudetailsId()
        {
            try
            {
                var Id = (from a in db.Tbl_EmpEduDetails select a.Detail_Id).Max();
                return Id = Id + 1;
            }
            catch (Exception)
            {

                return 1;
            }
        }
        public JsonResult expire()
        {
            Session["username"] = "";
          
           Session["usertype"]="";
          Session["empid"]="";
            var b = (string)Session["username"];
            if (b == "" || b == null)
            {
                Response.Redirect("/Login.aspx");
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Document(string id,string profile,string pancard,string aadhar,string joining,string joiningext,string offer,string offerext,string resume,string resext, string address,string passport)
        {
            var response = "Records Submited Sucessfully";
            
            // extract only the fielname

            Tbl_EmployeeRegistration t1 = db.Tbl_EmployeeRegistrations.Single(t => t.Emp_id == Convert.ToInt32(id));
            if (profile != "")
            {
                t1.profilepath = profile.Trim();
            }
            if (aadhar != "")
            {
                t1.AadharCard = aadhar.Trim();
            }
            if (address != "")
            {
                t1.AddressProof = address.Trim();
            }
            if (pancard != "")
            {
                t1.PanCard = pancard.Trim();
            }
            if (joining != "")
            {
                t1.JoiningLetter = joining.Trim();
                t1.joiningext = joiningext.Trim();
            }
            if (offer != "")
            {
                t1.OfferLetter = offer.Trim();
                t1.offerLext = offerext.Trim();
            }
            if (resume != "")
            {
                
                t1.Resume =resume.Trim();
                t1.resumetype = resext.Trim();
               
            }
            if (passport != "")
            {
                t1.passport = passport.Trim();
            }
            db.SubmitChanges();

       

            return Json(response, JsonRequestBehavior.AllowGet);

        }
        public FileResult DownloadFile( string button,int id)
        {

            try
            {
                string convert = "";
                var filenameextention1 = "";
                byte[] doc;
                var data = (from a in db.Tbl_EmployeeRegistrations where a.Emp_id == id select a).SingleOrDefault();
                if (button == "Resume")
                {
                    if (data.resumetype.Trim() == "pdf")
                    {
                        convert = data.Resume.Replace("data:application/pdf;base64,", String.Empty);
                        byte[] pdf = Convert.FromBase64String(convert);

                        //var filenameextention1 = "pdf";
                        //return File(bytes, "." + filenameextention1);
                        filenameextention1 = "pdf";
                        Response.AddHeader("content-disposition", "attachment;filename=Resume." + filenameextention1);
                        return File(pdf, "." + filenameextention1);
                    }
                    else if (data.resumetype.Trim() == "doc" || data.resumetype == "docs")
                    {
                        convert = data.Resume.Replace("data:;base64,", String.Empty);
                    }

                    doc = Convert.FromBase64String(convert);

                    //var filenameextention1 = "pdf";
                    //return File(bytes, "." + filenameextention1);
                    filenameextention1 = "doc";
                    Response.AddHeader("content-disposition", "attachment;filename=Resume." + filenameextention1);
                    return File(doc, "." + filenameextention1);
                }
                else if (button == "offer")
                {
                    if (data.offerLext.Trim() == "pdf")
                    {
                        convert = data.OfferLetter.Replace("data:application/pdf;base64,", String.Empty);
                        byte[] pdf = Convert.FromBase64String(convert);

                        //var filenameextention1 = "pdf";
                        //return File(bytes, "." + filenameextention1);
                        filenameextention1 = "pdf";
                        Response.AddHeader("content-disposition", "attachment;filename=OfferLetter." + filenameextention1);
                        return File(pdf, "." + filenameextention1);
                    }
                    else if (data.offerLext.Trim() == "doc" || data.offerLext == "docs")
                    {
                        convert = data.OfferLetter.Replace("data:;base64,", String.Empty);
                    }

                    doc = Convert.FromBase64String(convert);

                    //var filenameextention1 = "pdf";
                    //return File(bytes, "." + filenameextention1);
                    filenameextention1 = "doc";
                    Response.AddHeader("content-disposition", "attachment;filename=OfferLetter." + filenameextention1);
                    return File(doc, "." + filenameextention1);
                }
                else if (button == "joining")
                {
                    if (data.joiningext.Trim() == "pdf")
                    {
                        convert = data.JoiningLetter.Replace("data:application/pdf;base64,", String.Empty);
                        byte[] pdf = Convert.FromBase64String(convert);

                        //var filenameextention1 = "pdf";
                        //return File(bytes, "." + filenameextention1);
                        filenameextention1 = "pdf";
                        Response.AddHeader("content-disposition", "attachment;filename=JoiningLetter." + filenameextention1);
                        return File(pdf, "." + filenameextention1);
                    }
                    else if (data.joiningext.Trim() == "doc" || data.joiningext == "docs")
                    {
                        convert = data.JoiningLetter.Replace("data:;base64,", String.Empty);
                    }

                    doc = Convert.FromBase64String(convert);

                    //var filenameextention1 = "pdf";
                    //return File(bytes, "." + filenameextention1);
                    filenameextention1 = "doc";
                    Response.AddHeader("content-disposition", "attachment;filename=JoiningLetter." + filenameextention1);
                    return File(doc, "." + filenameextention1);
                }
                else if (button == "pan")
                {
                    if (data.PanCard.Contains("data:image/png;base64,"))
                    {
                        convert = data.PanCard.Replace("data:image/png;base64,", String.Empty);
                        filenameextention1 = "png";
                    }
                    else if(data.PanCard.Contains("data:image/jpeg;base64,"))
                    {
                        convert = data.PanCard.Replace("data:image/jpeg;base64,", String.Empty);
                        filenameextention1 = "png";
                    }
                    else if (data.PanCard.Contains("data:application/pdf;base64,"))
                    {
                        convert = data.PanCard.Replace("data:application/pdf;base64,", String.Empty);
                        filenameextention1 = "pdf";
                    }

                    byte[] png = Convert.FromBase64String(convert);
                    png = Convert.FromBase64String(convert);
                   
                    //var filenameextention1 = "pdf";
                    //return File(bytes, "." + filenameextention1);

                    Response.AddHeader("content-disposition", "attachment;filename=pancard." + filenameextention1);
                    return File(png, "." + filenameextention1);
                }
                else if (button == "passport")
                {

                    if (data.PanCard.Contains("data:image/png;base64,"))
                    {
                        convert = data.PanCard.Replace("data:image/png;base64,", String.Empty);
                        filenameextention1 = "png";
                    }
                    else if (data.PanCard.Contains("data:image/jpeg;base64,"))
                    {
                        convert = data.PanCard.Replace("data:image/jpeg;base64,", String.Empty);
                        filenameextention1 = "png";
                    }
                    else if (data.PanCard.Contains("data:application/pdf;base64,"))
                    {
                        convert = data.PanCard.Replace("data:application/pdf;base64,", String.Empty);
                        filenameextention1 = "pdf";
                    }
                    byte[] png = Convert.FromBase64String(convert);
                    png = Convert.FromBase64String(convert);
                   
                    //var filenameextention1 = "pdf";
                    //return File(bytes, "." + filenameextention1);

                    Response.AddHeader("content-disposition", "attachment;filename=passport." + filenameextention1);
                    return File(png, "." + filenameextention1);
                }
                else if (button == "aadhar")
                {

                    if (data.PanCard.Contains("data:image/png;base64,"))
                    {
                        convert = data.PanCard.Replace("data:image/png;base64,", String.Empty);
                        filenameextention1 = "png";
                    }
                    else if (data.PanCard.Contains("data:image/jpeg;base64,"))
                    {
                        convert = data.PanCard.Replace("data:image/jpeg;base64,", String.Empty);
                        filenameextention1 = "png";
                    }
                    else if (data.PanCard.Contains("data:application/pdf;base64,"))
                    {
                        convert = data.PanCard.Replace("data:application/pdf;base64,", String.Empty);
                        filenameextention1 = "pdf";
                    }
                    byte[] png = Convert.FromBase64String(convert);
                    png = Convert.FromBase64String(convert);
                   
                    //var filenameextention1 = "pdf";
                    //return File(bytes, "." + filenameextention1);

                    Response.AddHeader("content-disposition", "attachment;filename=AadharCard." + filenameextention1);
                    return File(png, "." + filenameextention1);
                }
                else if (button == "address")
                {

                    if (data.PanCard.Contains("data:image/png;base64,"))
                    {
                        convert = data.PanCard.Replace("data:image/png;base64,", String.Empty);
                        filenameextention1 = "png";
                    }
                    else if (data.PanCard.Contains("data:image/jpeg;base64,"))
                    {
                        convert = data.PanCard.Replace("data:image/jpeg;base64,", String.Empty);
                        filenameextention1 = "png";
                    }
                    else if (data.PanCard.Contains("data:application/pdf;base64,"))
                    {
                        convert = data.PanCard.Replace("data:application/pdf;base64,", String.Empty);
                        filenameextention1 = "pdf";
                    }
                    byte[] png = Convert.FromBase64String(convert);
                    png = Convert.FromBase64String(convert);
                   
                    //var filenameextention1 = "pdf";
                    //return File(bytes, "." + filenameextention1);

                    Response.AddHeader("content-disposition", "attachment;filename=AddressProof." + filenameextention1);
                    return File(png, "." + filenameextention1);
                }

            }
            catch (Exception)
            {

                Response.Write("Employee ID Not Found!!");
                Response.Redirect("/Home/Employee");
            }

            return (null);

        }


        //public JsonResult updatedocs(string id, string profile, string pancard, string aadhar, string joining, string offer, string resume,string resext, string address, string passport)
        //{
        //    var response = "Done";
          
        //        // extract only the fielname
               
        //                Tbl_EmployeeRegistration t1 = db.Tbl_EmployeeRegistrations.Single(t => t.Emp_id == Convert.ToInt32(id));
        //    if (profile != "")
        //    {
        //        t1.profilepath = profile.Trim();
        //    }
        //    if (aadhar != "")
        //    {
        //        t1.AadharCard = aadhar.Trim();
        //    }
        //    if (address != "")
        //    {
        //        t1.AddressProof = address.Trim();
        //    }
        //    if (pancard != "")
        //    {
        //        t1.PanCard = pancard.Trim();
        //    }
        //    if (joining != "")
        //    {
        //        t1.JoiningLetter = joining.Trim();
        //    }
        //    if (offer != "")
        //    {
        //        t1.OfferLetter = offer.Trim();
        //    }
        //    if (resume != "")
        //    {
        //        t1.Resume = resume.Trim();
        //        t1.resumetype = resext.Trim();
        //    }
        //    if (passport != "")
        //    {
        //        t1.passport = passport.Trim();
        //    }
        //                db.SubmitChanges();

        //   return Json(response, JsonRequestBehavior.AllowGet);

        //}
       
       
        public JsonResult updateemploeedetails(EmployeeRegistration employee)
        {
            try
            {
             
                Tbl_EmployeeRegistration t = db.Tbl_EmployeeRegistrations.Single(s => s.Emp_id == Convert.ToInt32(employee.EmpId));
                t.Emp_id = Convert.ToInt32(employee.EmpId);
                t.Emp_Name = employee.Empname.Trim();
                t.Emp_Mobile = employee.mobile.Trim();
                t.Emp_Homeno = employee.homeno.Trim();
                t.Emp_FatherName = employee.Fname.Trim();
                t.Emp_DOB = Convert.ToDateTime(employee.DOB); 
                t.Emp_DOJ = Convert.ToDateTime(employee.DOJ);
                t.Department = employee.Department.Trim();
                t.Emp_Passport_No = employee.passportno.Trim();
                t.Designation = employee.Designation.Trim();
                t.Gender = employee.gender.Trim();
                t.MeritialStetus = employee.meritialstetus.Trim();
                t.PanNo = employee.panno.Trim();
                t.UserName = employee.username.Trim();
                t.Password = employee.password.Trim();
                t.AccountNo = employee.acountno.Trim();
                t.BankAccountName = employee.nameinbank.Trim();
                t.BankName = employee.bankname.Trim();
                t.BasicSalary = Convert.ToDouble(employee.basicsalary);
                t.IFSCcode = employee.ifsc.Trim();
                t.BranchName = employee.brancename.Trim();
                t.EmailId = employee.emailid.Trim();



                Tbl_EmpEduDetail t1 = db.Tbl_EmpEduDetails.Single(x => x.Emp_id == Convert.ToInt32(employee.EmpId));

                t1.Emp_id = Convert.ToInt32(employee.EmpId);
                t1.PG_Colgname = employee.PG_Colgname.Trim();
                t1.PG_State = employee.PG_State.Trim();
                t1.PG_Universityname = employee.PG_Universityname.Trim();
                t1.PG_yearofPassing = employee.PG_yearofPassing.Trim();
                t1.PG_City = employee.PG_City.Trim();
                t1.PGMarks = employee.PGMarks.Trim();
                t1.G_colgname = employee.G_colgname.Trim();
                t1.G_University = employee.G_University.Trim();
                t1.G_State = employee.G_State.Trim();
                t1.G_City = employee.G_City.Trim();
                t1.G_marks = employee.G_marks.Trim();
                t1.G_yearOfPassing = employee.G_yearOfPassing.Trim();
                t1._12_ColgName = employee._12_ColgName.Trim();
                t1._12_Board = employee._12_Board.Trim();
                t1._12_State = employee._12_State.Trim();
                t1._12_City = employee._12_City.Trim();
                t1._12_marks = employee._12_marks.Trim();
                t1._12_yearofpassing = employee._12_yearofpassing.Trim();
                t1.school_name = employee.school_name.Trim();
                t1.S_City = employee.S_City.Trim();
                t1.S_state = employee.S_state.Trim();
                t1.S_yaerofpassing = employee.S_yaerofpassing.Trim();
                t1.Board_name = employee.Board_name.Trim();
                t1.S_Marks = employee.S_Marks.Trim();

                db.SubmitChanges();
                var message = "Record Updated Successfully";
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                var message = "No Recods Found!!!";
                return Json(message, JsonRequestBehavior.AllowGet);
            }
        }
            public JsonResult saveemploeedetails(EmployeeRegistration employee )
        {
            try
            {
                Tbl_EmployeeRegistration t = new Tbl_EmployeeRegistration();
                t.Emp_id = Convert.ToInt32(employee.EmpId);
                t.Emp_Name = employee.Empname.Trim();
                t.Emp_Mobile = employee.mobile.Trim();
                t.Emp_Homeno = employee.homeno.Trim();
                t.Emp_FatherName = employee.Fname.Trim();
                t.Emp_DOB = Convert.ToDateTime(employee.DOB);
                t.Emp_DOJ = Convert.ToDateTime(employee.DOJ);
                t.Department = employee.Department.Trim();
                t.Emp_Passport_No = employee.passportno.Trim();
                t.Designation = employee.Designation.Trim();
                t.Gender = employee.gender.Trim();
                t.MeritialStetus = employee.meritialstetus.Trim();
                t.PanNo = employee.panno.Trim();
                t.UserName = employee.username.Trim();
                t.Password = employee.password.Trim();
                t.AccountNo = employee.acountno.Trim();
                t.BankAccountName = employee.nameinbank.Trim();
                t.BankName = employee.bankname.Trim();
                t.BasicSalary = Convert.ToDouble(employee.basicsalary);
                t.IFSCcode = employee.ifsc.Trim();
                t.BranchName = employee.brancename.Trim();
                t.EmailId = employee.emailid.Trim();
                t.Date = DateTime.Now;
              
              

                Tbl_EmpEduDetail t1 = new Tbl_EmpEduDetail();
                t1.Detail_Id = maxedudetailsId();
                t1.Emp_id= Convert.ToInt32(employee.EmpId);
                t1.PG_Colgname = employee.PG_Colgname.Trim();
                t1.PG_State = employee.PG_State.Trim();
                t1.PG_Universityname = employee.PG_Universityname.Trim();
                t1.PG_yearofPassing = employee.PG_yearofPassing.Trim();
                t1.PG_City = employee.PG_City.Trim();
                t1.PGMarks = employee.PGMarks.Trim();
                t1.G_colgname = employee.G_colgname.Trim();
                t1.G_University = employee.G_University.Trim();
                t1.G_State = employee.G_State.Trim();
                t1.G_City = employee.G_City.Trim();
                t1.G_marks = employee.G_marks.Trim();
                t1.G_yearOfPassing = employee.G_yearOfPassing.Trim();
                t1._12_ColgName = employee._12_ColgName.Trim();
                t1._12_Board = employee._12_Board.Trim();
                t1._12_State = employee._12_State.Trim();
                t1._12_City = employee._12_City.Trim();
                t1._12_marks = employee._12_marks.Trim();
                t1._12_yearofpassing = employee._12_yearofpassing.Trim();
                t1.school_name = employee.school_name.Trim();
                t1.S_City = employee.S_City.Trim();
                t1.S_state = employee.S_state.Trim();
                t1.S_yaerofpassing = employee.S_yaerofpassing.Trim();
                t1.Board_name = employee.Board_name.Trim();
                t1.S_Marks = employee.S_Marks.Trim();
                db.Tbl_EmployeeRegistrations.InsertOnSubmit(t);
                db.Tbl_EmpEduDetails.InsertOnSubmit(t1);
                db.SubmitChanges();
                var message = "Record Submited Successfully";
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                var message = "Failed";
                return Json(message, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
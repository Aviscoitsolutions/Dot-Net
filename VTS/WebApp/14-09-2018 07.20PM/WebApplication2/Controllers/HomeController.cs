
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI;

namespace WebApplication2.Controllers
{

    public class HomeController : Controller
    {

   
        // GET: Home

        DataClasses1DataContext db = new DataClasses1DataContext();
        int UserId;
        DateTime from1, to;
        int deviceid;

        public ActionResult Index()
        {

            return Redirect("~/Login.aspx");
        }

        public ActionResult Demo()
        {

            return View();
        }
        public ActionResult MainPage()
        {

            var a = (string)Session["username"];

           
            if (a == ""||a==null)
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
        //for change user password
        public JsonResult changepassworddata(product product)
        {
            var b = (string)Session["username"];
            string data1;

            if (b== "" || b == null)
            {
                Response.Write("Your session has een expired please login again");
                Response.Redirect("/Login.aspx");

            }
            else
            {
          
                var data = (from a in db.Tbl_Users where a.username == (string)Session["username"] select a).SingleOrDefault();
                if (data.Password.Trim() == product.oldpassword.Trim())
                {
                    Tbl_User t = db.Tbl_Users.Single(u => u.username == (string)Session["username"]);
                    t.Password = Convert.ToString(product.newpassword).Trim();
                    db.SubmitChanges();
                    data1 = "Changed";
                    return Json(data1, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    data1 = "Not";
                    return Json(data1, JsonRequestBehavior.AllowGet);
                }
              
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Log()
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
        public ActionResult Visit()
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
        public ActionResult FuelMonitoring()
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
        public JsonResult expire()
        { 
            Session["username"] = "";
           Response.Redirect("/Login.aspx");
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DashBoard()
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
        public ActionResult Reports()
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
        public ActionResult PlayMap(string Vehicle_No)
        {
            var b = (string)Session["username"];

            
            if (b == "" || b == null)
            {
                Response.Write("Your session has een expired please login again");
                Response.Redirect("/Login.aspx");

            }
            else
            {
                var data = (from a in db.Tbl_DeviceRegistrations where a.DeviceIMEI == Convert.ToInt64(Vehicle_No) select a).SingleOrDefault();
                ViewBag.type = data.Vehicle_Type.ToString();
                ViewBag.Vehicle_No = data.DeviceName.ToString();
                return View();
            }
            return View();
        }

        //Retriving data for paly map page
        public JsonResult playdata( product product)
        {

            var s = (string)Session["username"];


            if (s == "" || s == null)
            {
                Response.Write("Your session has een expired please login again");
                Response.Redirect("/Login.aspx");

            }
            var imei = (from a in db.Tbl_DeviceRegistrations where a.DeviceName == product.IMEI select a.DeviceIMEI).SingleOrDefault();
            try
            {
                var data = (from a in db.telemetries
                            where a.imei == imei && a.time > DateTime.Now.Date &&a.digital_inputs>=35936&&a.latitude!=null
                            orderby a.id ascending
                            //group a by a.id into g
                         
                            select new
                            {
                            //    Lat = g.First().latitude,
                            //    Long = g.First().longitude,
                                Lat = a.latitude,
                                Long = a.longitude,
                                stete=a.digital_inputs,
                                speed=a.speed,
                            }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }

        }
        //begin end day report page table log
        public ActionResult BeginEndDayReport(string Vehicle_No)
        {
            var b = (string)Session["username"];

          
            if (b == "" || b == null)
            {
                Response.Write("Your session has een expired please login again");
                Response.Redirect("/Login.aspx");

            }
            else
            {


              

                ViewBag.To = Convert.ToDateTime(DateTime.Now.Date).AddSeconds(1);
                ViewBag.From = Convert.ToDateTime(DateTime.Now.Date.AddDays(1).AddSeconds(-1));
                //ViewBag.type = type.ToString();
                var data1 = (from a in db.Tbl_DeviceRegistrations where a.DeviceIMEI == Convert.ToInt64(Vehicle_No) select a.DeviceName).FirstOrDefault();
                ViewBag.Vehicle_No = data1.ToString();
                var data = (from a in db.Tbl_DeviceRegistrations where a.DeviceIMEI == Convert.ToInt64(Vehicle_No) select a.Vehicle_Type).FirstOrDefault();
                ViewBag.type = data.ToString();
                return View();
            }
            return View();
        }
        //bind start stop report page table log
        public ActionResult StartStopReport(string Vehicle_No)
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
        public ActionResult ReportsSpeedVoilation( string To,string From,string type,string Vehicle_No)
        {
            var a = (string)Session["username"];

          
            if (a == "" || a == null)
            {
                Response.Write("Your session has een expired please login again");
                Response.Redirect("/Login.aspx");

            }
            else
            {



                ViewBag.To = Convert.ToDateTime(From);
                ViewBag.From = Convert.ToDateTime(To);
                ViewBag.type = type.ToString();
                ViewBag.Vehicle_No = Vehicle_No.ToString();
                return View();
            }
            return View();
        }
        public ActionResult UploadLogo()
        {
            return View();
        }

        public ActionResult ReportsAlert(string To, string From, string type, string Vehicle_No)
        {
            var a = (string)Session["username"];

            
            if (a == "" || a == null)
            {
                Response.Write("Your session has een expired please login again");
                Response.Redirect("/Login.aspx");

            }
            else
            {
                ViewBag.To = Convert.ToDateTime(From);
                ViewBag.From = Convert.ToDateTime(To);
                ViewBag.type = type.ToString();
                ViewBag.Vehicle_No = Vehicle_No.ToString();
                return View();
            }
            return View();
        }
        //bind fleet anyalisis page table log
        public ActionResult ReportsFleetAnyalysis(string To, string From, string type, string Vehicle_No)
        {
            var b = (string)Session["username"];

            
            if (b == "" || b == null)
            {
                Response.Write("Your session has een expired please login again");
                Response.Redirect("/Login.aspx");

            }
            else
            {
                try
                {
                    if (From == null && To == null)
                    {
                        ViewBag.To = Convert.ToDateTime(DateTime.Now.Date);
                        ViewBag.From = Convert.ToDateTime(DateTime.Now.Date.AddDays(1));
                        var vtype = (from a in db.Tbl_DeviceRegistrations where a.DeviceIMEI == Convert.ToInt64(Vehicle_No) select a.Vehicle_Type).SingleOrDefault();
                        ViewBag.type = vtype.ToString();
                        ViewBag.Vehicle_No = Vehicle_No.ToString();
                        return View();
                    }
                    else
                    {
                        ViewBag.To = Convert.ToDateTime(From);
                        ViewBag.From = Convert.ToDateTime(To);
                        ViewBag.type = type.ToString();
                        ViewBag.Vehicle_No = Vehicle_No.ToString();
                        return View();
                    }
                }
                catch (Exception)
                {

                    ViewBag.Vehicle_No = Vehicle_No.ToString();
                    return View();
                }
            }
            return View();
        }
        public ActionResult ReportsDistanceTravelled(string To, string From, string type, string Vehicle_No)
        {
            var a = (string)Session["username"];

           
            if (a == "" || a == null)
            {
                Response.Write("Your session has een expired please login again");
                Response.Redirect("/Login.aspx");

            }
            else
            {
                ViewBag.To = Convert.ToDateTime(From);
                ViewBag.From = Convert.ToDateTime(To);
                ViewBag.type = type.ToString();
                ViewBag.Vehicle_No = Vehicle_No.ToString();
                return View();
            }
            return View();
        }
        public ActionResult ShowLogLocation(string Lat,string Long)
        {
            var a = (string)Session["username"];

           
            if (a == "" || a == null)
            {
                Response.Write("Your session has een expired please login again");
                Response.Redirect("/Login.aspx");

            }
            else
            {
                ViewBag.Lat = Lat.ToString();
                ViewBag.Long = Long.ToString();
                return View();
            }
            return View();
        }
        [HttpPost]
        public ActionResult gettype(string Lat)
        {
            var s = (string)Session["username"];


            if (s == "" || s == null)
            {
                Response.Write("Your session has een expired please login again");
                Response.Redirect("/Login.aspx");

            }
            string value = Lat;
            return RedirectToAction("ShowLogLocation", "Home", new { Lat1 = Lat });
           
        }
        //Change device stetus i.e it is shown on site or not
        public JsonResult enabledisebleDevice(product product)
        {
            var s = (string)Session["username"];


            if (s == "" || s == null)
            {
                Response.Write("Your session has een expired please login again");
                Response.Redirect("/Login.aspx");

            }
            Tbl_DeviceRegistration t = db.Tbl_DeviceRegistrations.Single(u => u.DeviceIMEI ==Convert.ToInt64( product.IMEI));
            t.stetus ="0";
            db.SubmitChanges();
            return Json(null, JsonRequestBehavior.AllowGet);

        }

        //Retrive device information and marker value to main page
        public JsonResult GetAllLocation1(product product)
        {
            var s = (string)Session["username"];


            if (s == "" || s == null)
            {
                Response.Write("Your session has een expired please login again");
                Response.Redirect("/Login.aspx");

            }
            var uId = (from a in db.Tbl_Users where a.username == Request.RequestContext.HttpContext.Session["username"].ToString() select a.UserID).SingleOrDefault();
            UserId = Convert.ToInt32(uId);
            if (product.Dropdown != null)
            {

                var data = (from a in db.Tbl_DeviceRegistrations
                            where a.UserId == UserId && a.DeviceIMEI != Convert.ToInt64(product.Dropdown)&&a.stetus!="0"
                            select new
                            {
                                DeviceName = a.DeviceName,
                                imei = a.DeviceIMEI,
                                Lat = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == a.DeviceIMEI).Select(t => t.latitude).First().ToString(),
                                Long = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == a.DeviceIMEI).Select(t => t.longitude).First().ToString(),
                                LocalCode = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == a.DeviceIMEI).Select(t => t.local_area_code).First().ToString(),
                                Speed = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == a.DeviceIMEI).Select(t => t.speed).First().ToString(),
                                State = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == a.DeviceIMEI).Select(t => t.digital_inputs).First().ToString(),
                                Speed1 = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == a.DeviceIMEI).Select(t => t.speed).First().ToString(),
                            }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = (from a in db.Tbl_DeviceRegistrations where a.UserId == UserId&&a.stetus!="0"
                            select new
                            {
                                DeviceName = a.DeviceName,
                                imei = a.DeviceIMEI,
                                Lat = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == a.DeviceIMEI).Select(t => t.latitude).First().ToString(),
                                Long = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == a.DeviceIMEI).Select(t => t.longitude).First().ToString(),
                                LocalCode = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == a.DeviceIMEI).Select(t => t.local_area_code).First().ToString(),
                                Speed = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == a.DeviceIMEI).Select(t => t.speed).First().ToString(),
                                State = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == a.DeviceIMEI).Select(t => t.digital_inputs).First().ToString(),
                                Speed1 = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == a.DeviceIMEI).Select(t => t.speed).First().ToString(),
                            }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }

        }
        //Bin dealers Name
        public JsonResult binddealers()
        {
            var data = (from a in db.Tbl_DealerRegistrations
                        select new
                        {
                            Dealer_Id= a.Dealer_Id,
                            DealerName=  a.DealerName
                        }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

            //retrive data for dashboard page table
            public JsonResult DashBoardLive()
        {
            var s = (string)Session["username"];


            if (s == "" || s == null)
            {
                Response.Write("Your session has een expired please login again");
                Response.Redirect("/Login.aspx");

            }
            var username =(string)Session["username"];
            var uId = (from a in db.Tbl_Users where a.username == username.ToString() select a.UserID).SingleOrDefault();
            UserId = Convert.ToInt32(uId);
            try
               {
                var data = (from a in db.Tbl_DeviceRegistrations
                            where a.UserId == /*1*/ UserId && a.stetus!="0"
                            select new
                            {
                                DeviceName = a.DeviceName,
                                imei = a.DeviceIMEI,
                                type = a.Vehicle_Type,
                                Lat = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == a.DeviceIMEI).Select(t => t.latitude).First().ToString(),
                                Long = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == a.DeviceIMEI).Select(t => t.longitude).First().ToString(),
                               // LocalCode = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == a.DeviceIMEI).Select(t => t.local_area_code).First().ToString(),
                                MaxSpeed = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == a.DeviceIMEI && t.time > DateTime.Now.Date).Select(t => t.speed).Max().ToString(),
                                State = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == a.DeviceIMEI).Select(t => t.digital_inputs).First().ToString(),
                                Speed = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == a.DeviceIMEI && t.time > DateTime.Now.Date).Select(t => t.speed).First().ToString(),
                                //totoalDistance = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == a.DeviceIMEI && t.digital_inputs > 32950 && t.time >DateTime.Now.Date).Select(t => Convert.ToInt64(t.odometer)).Sum(),


                            }).ToList();
                     return Json(data, JsonRequestBehavior.AllowGet);
                }

            catch (Exception ex)
                {

                throw;
                }
            }

        
        public JsonResult DashBord(product product)
        {
            var s = (string)Session["username"];


            if (s == "" || s == null)
            {
                Response.Write("Your session has een expired please login again");
                Response.Redirect("/Login.aspx");

            }
            var uId = (from a in db.Tbl_Users where a.username == Request.RequestContext.HttpContext.Session["username"].ToString() select a.UserID).SingleOrDefault();
            UserId = Convert.ToInt32(uId);
            if (product.Dropdown != null)
            {

                var data = (from a in db.Tbl_DeviceRegistrations
                            where a.UserId ==/*1*/ UserId && a.DeviceIMEI != Convert.ToInt64(product.Dropdown)&&a.stetus!="0"
                            select new
                            {
                                DeviceName = a.DeviceName,
                                imei = a.DeviceIMEI,
                               Lat = db.telemetries.Where(t=>t.time>product.Date1).OrderByDescending(t => t.id).Where(t => t.time > Convert.ToDateTime(product.Date1)&& t.imei == a.DeviceIMEI).Select(t => t.latitude).First().ToString(),
                               
                                //Long = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == a.DeviceIMEI && t.time > Convert.ToDateTime(product.Date1)).Select(t => t.longitude).First().ToString(),
                                //LocalCode = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == a.DeviceIMEI && t.time > Convert.ToDateTime(product.Date1)).Select(t => t.local_area_code).First().ToString(),
                                //Speed = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == a.DeviceIMEI && t.time > Convert.ToDateTime(product.Date1)).Select(t => t.speed ).First().ToString(),
                                //State = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == a.DeviceIMEI && t.time > Convert.ToDateTime(product.Date1)).Select(t => t.digital_inputs).First().ToString(),
                                //Speed1 = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == a.DeviceIMEI && t.time > Convert.ToDateTime(product.Date1)).Select(t => t.speed).First().ToString(),
                            }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = (from a in db.Tbl_DeviceRegistrations
                            where a.UserId ==/*1 */UserId && a.stetus != "0"
                            select new
                            {
                                DeviceName = a.DeviceName,
                                imei = a.DeviceIMEI,
                                Lat = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == a.DeviceIMEI && t.time == Convert.ToDateTime(product.Date1).Date).Select(t => t.latitude).First().ToString(),
                                Long = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == a.DeviceIMEI && t.time == Convert.ToDateTime(product.Date1).Date).Select(t => t.longitude).First().ToString(),
                                LocalCode = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == a.DeviceIMEI && t.time == Convert.ToDateTime(product.Date1).Date).Select(t => t.local_area_code).First().ToString(),
                                Speed = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == a.DeviceIMEI && t.time == Convert.ToDateTime(product.Date1).Date).Select(t => t.speed).First().ToString(),
                                State = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == a.DeviceIMEI && t.time == Convert.ToDateTime(product.Date1).Date).Select(t => t.digital_inputs).First().ToString(),
                                Speed1 = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == a.DeviceIMEI && t.time == Convert.ToDateTime(product.Date1).Date).Select(t => t.speed).First().ToString(),
                            }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult DrawLine(product product)
        {
            var s = (string)Session["username"];


            if (s == "" || s == null)
            {
                Response.Write("Your session has een expired please login again");
                Response.Redirect("/Login.aspx");

            }
            var uId = (from a in db.Tbl_Users where a.username == Request.RequestContext.HttpContext.Session["username"].ToString() select a.UserID).SingleOrDefault();
            var count = (from b in db.Tbl_DeviceRegistrations where b.UserId == Convert.ToInt32(uId) select b).ToList();
            UserId = Convert.ToInt32(uId);


            //var data = (from c in db.telemetries
            //             where c.imei==count[j].DeviceIMEI
            //             select new
            //             {
            //                 Lat = c.latitude.ToString(),
            //                 Long = c.longitude.ToString(),
            //                 //LocalCode = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == a.DeviceIMEI).Select(t => t.local_area_code).First().ToString(),
            //                 //Speed = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == a.DeviceIMEI).Select(t => t.speed).First().ToString(),
            //                 //State = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == a.DeviceIMEI).Select(t => t.digital_inputs).First().ToString(),
            //                 //Speed1 = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == a.DeviceIMEI).Select(t => t.speed).First().ToString(),
            //             }).ToList();


            return Json(null, JsonRequestBehavior.AllowGet);
        }

        //retrive data for object block table on main page
        public JsonResult BindDorpDown()

        {
            var s = (string)Session["username"];


            if (s == "" || s == null)
            {
                Response.Write("Your session has een expired please login again");
                Response.Redirect("/Login.aspx");

            }
            var uId = (from a in db.Tbl_Users where a.username == Request.RequestContext.HttpContext.Session["username"].ToString() select a.UserID).SingleOrDefault();
            UserId = Convert.ToInt32(uId);
            // EmployeeModel em = new EmployeeModel();
            var data = (from a in db.Tbl_DeviceRegistrations
                        where a.UserId == UserId&&a.stetus!="0"
                        select new
                        {
                            a.DeviceIMEI,
                            a.DeviceName,
                            current = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == a.DeviceIMEI).Select(t => t.time).First(),
                            Speed = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == a.DeviceIMEI && t.speed > 45).Select(t => t.speed).First(),
                            time = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == a.DeviceIMEI && t.speed > 45).Select(t => t.time).First().ToString(),
                            State = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == a.DeviceIMEI).Select(t => t.digital_inputs).First().ToString(),
                            Speed1 = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == a.DeviceIMEI).Select(t => t.speed).First().ToString(),
                            currenttime = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == a.DeviceIMEI).Select(t => t.time).First().ToString(),

                        }).ToList();
            //  var data11 = (from a in db.telemetries select a).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetVehicleNo(product product)
        {
            var s = (string)Session["username"];


            if (s == "" || s == null)
            {
                Response.Write("Your session has een expired please login again");
                Response.Redirect("/Login.aspx");

            }
            var username = (string)Session["username"];
            var uId = (from a in db.Tbl_Users where a.username == username.ToString() select a.UserID).SingleOrDefault();
            UserId = Convert.ToInt32(uId);
            var data = (from a in db.Tbl_DeviceRegistrations
                        where a.Vehicle_Type == product.Dropdown&&a.UserId== UserId
                        select new
                        {
                            a.DeviceIMEI,
                            a.DeviceName,
                        }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //Bind vehicle number dropdown 
        public JsonResult BindDorpDown1()
        {
            var s = (string)Session["username"];


            if (s == "" || s == null)
            {
                Response.Write("Your session has een expired please login again");
                Response.Redirect("/Login.aspx");

            }
            var username = (string)Session["username"];
            var uId = (from a in db.Tbl_Users where a.username == username.ToString() select a.UserID).SingleOrDefault();
            UserId = Convert.ToInt32(uId);
            // EmployeeModel em = new EmployeeModel();
            var data = (from a in db.Tbl_DeviceRegistrations
                        where a.UserId == UserId && a.stetus != "0"
                        select new
                        {
                            a.DeviceIMEI,
                            a.DeviceName,


                        }).ToList();
            //  var data11 = (from a in db.telemetries select a).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);

        }
        //visit page vehicle no dropdown
        public JsonResult BindDorpDownVisit()
        {
            var s = (string)Session["username"];


            if (s == "" || s == null)
            {
                Response.Write("Your session has een expired please login again");
                Response.Redirect("/Login.aspx");

            }
            var username = (string)Session["username"];
            var uId = (from a in db.Tbl_Users where a.username == username.ToString() select a.UserID).SingleOrDefault();
            UserId = Convert.ToInt32(uId);
            // EmployeeModel em = new EmployeeModel();
            var data = (from a in db.Tbl_DeviceRegistrations
                        where a.UserId == UserId && a.stetus != "0"
                        select new
                        {
                            a.DeviceIMEI,
                            a.DeviceName,


                        }).ToList();
            //  var data11 = (from a in db.telemetries select a).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);

        }

        //Retrive vehicle types from database
        public JsonResult GetVehicleType()
        {
            var s = (string)Session["username"];


            if (s == "" || s == null)
            {
                Response.Write("Your session has een expired please login again");
                Response.Redirect("/Login.aspx");

            }
            var username = (string)Session["username"];
            var uId = (from a in db.Tbl_Users where a.username == username.ToString() select a.UserID).SingleOrDefault();
            UserId = Convert.ToInt32(uId);
           
            var data = (from a in db.Tbl_DeviceRegistrations
                        where a.UserId ==/*1*/UserId && a.stetus != "0"
                        select new
                        {
                          a.Vehicle_Type


                        }).Distinct().ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
          

        }
        public class product
        {
            public string Dropdown { get; set; }
            public DateTime From { get; set; }
            public DateTime To { get; set; }
            public DateTime Date1 { get; set; }
            public string DeviceName { get; set; }
            public string IMEI { get; set; }
            public string oldpassword { get; set; }
            public string newpassword { get; set; }
            public string filter { get; set; }
            public string Vehicle_Type { get; set; }

        }
        //visit page map direction data
        public JsonResult Direction(product product)
        {
           
            try
                {




                    var data = (from a in db.telemetries
                                orderby a.id descending
                                where a.imei == Convert.ToInt64(product.Dropdown) && a.time > Convert.ToDateTime(product.From) && a.time < Convert.ToDateTime(product.To.AddDays(+1)) && a.digital_inputs >= 35936 && a.latitude != null
                                select new
                                {

                                    Date = a.time,
                                    Id = a.id,
                                    Lat = a.latitude,
                                    Long = a.longitude,
                                    DeviceName = db.Tbl_DeviceRegistrations.Where(t => t.DeviceIMEI == a.imei).Select(t => t.DeviceName).SingleOrDefault(),
                                    imei = a.imei,
                                    State = a.digital_inputs
                                }).ToList();
                    return Json(data, JsonRequestBehavior.AllowGet);
                }


                catch (Exception)
                {

                    var data = (from a in db.telemetries
                                where a.imei == Convert.ToInt64(product.Dropdown) && a.time >= Convert.ToDateTime(product.From) && a.time <= Convert.ToDateTime(product.To)
                                select new
                                {

                                    Date = a.time,
                                    Id = a.id,
                                    Lat = a.latitude,
                                    Long = a.longitude,
                                    DeviceName = db.Tbl_DeviceRegistrations.Where(t => t.DeviceIMEI == a.imei).Select(t => t.DeviceName),
                                    imei = a.imei,
                                }).ToList();
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            
        }

        //retrive device log
        public JsonResult GetLog(product product)
        {
            //var uId = (from a in db.Tbl_Users where a.username == Request.RequestContext.HttpContext.Session["username"].ToString() select a.UserID).SingleOrDefault();

            var s = (string)Session["username"];


            if (s == "" || s == null)
            {
                Response.Write("Your session has een expired please login again");
                Response.Redirect("/Login.aspx");

            }
            var uId = (from a in db.Tbl_Users where a.username == Request.RequestContext.HttpContext.Session["username"].ToString() select a.UserID).SingleOrDefault();

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
            try
            {
                if (product.filter.ToString() != "-1" && product.filter != null&&product.filter!="cc")
                {
                    if (product.filter == "1")
                    {
                        try
                        {
                            DateTime fil = DateTime.Now.Date;


                            from1 = Convert.ToDateTime(product.From);
                            to = Convert.ToDateTime(product.To);
                            deviceid = Convert.ToInt32(product.IMEI);
                          
                            var data1 = (from a in db.telemetries
                                         orderby a.id descending
                                         where a.imei == Convert.ToInt64(product.Dropdown) && a.time > Convert.ToDateTime(DateTime.Now.Date)
                                         select new
                                         {

                                             Date = a.time,
                                             Id = a.id,
                                             Lat = a.latitude,
                                             Long = a.longitude,
                                             DeviceName = db.Tbl_DeviceRegistrations.Where(t => t.DeviceIMEI == a.imei).Select(t => t.DeviceName).SingleOrDefault(),
                                             imei = a.imei,
                                             speed = a.speed,
                                             Odeometer = a.odometer,
                                             hdop = a.hdop,
                                             scount = a.satellites,
                                             gsmStength = a.recieved_signal_strength
                                         }).ToList();
                            return Json(data1, JsonRequestBehavior.AllowGet);
                        }
                        catch (Exception)
                        {
                            var data1 = (from b in db.telemetries
                                         orderby b.id descending
                                         where b.time > Convert.ToDateTime(DateTime.Now.Date)

                                         select new
                                         {
                                             Date = b.time,
                                             Id = b.id,
                                             Lat = b.latitude,
                                             Long = b.longitude,
                                             DeviceName = db.Tbl_DeviceRegistrations.Where(t => t.DeviceIMEI == b.imei).Select(t => t.DeviceName).SingleOrDefault(),
                                             imei = b.imei,
                                             speed = b.speed,
                                             Odeometer = b.odometer,
                                             hdop = b.hdop,
                                             scount = b.satellites,
                                             gsmStength = b.recieved_signal_strength
                                         }).ToList();
                            return Json(data1, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else if (product.filter.ToString() == "2")
                    {


                        try
                        {
                            //uId = (from a in db.Tbl_Users where a.username == Request.RequestContext.HttpContext.Session["username"].ToString() select a.UserID).SingleOrDefault();
                            //UserId = Convert.ToInt32(uId);
                            var data1 = (from a in db.telemetries
                                         orderby a.id descending
                                         where a.imei == Convert.ToInt64(product.Dropdown) && a.time > Convert.ToDateTime(DateTime.Now.AddDays(-1)) && a.time < DateTime.Now.Date
                                         select new
                                         {

                                             Date = a.time,
                                             Id = a.id,
                                             Lat = a.latitude,
                                             Long = a.longitude,
                                             DeviceName = db.Tbl_DeviceRegistrations.Where(t => t.DeviceIMEI == a.imei).Select(t => t.DeviceName).SingleOrDefault(),
                                             imei = a.imei,
                                             speed = a.speed,
                                             Odeometer = a.odometer,
                                             hdop = a.hdop,
                                             scount = a.satellites,
                                             gsmStength = a.recieved_signal_strength
                                         }).ToList();
                            return Json(data1, JsonRequestBehavior.AllowGet);
                        }
                        catch (Exception)
                        {

                            var data1 = (from b in db.telemetries
                                         orderby b.id descending
                                         where b.time > Convert.ToDateTime(DateTime.Now.AddDays(-1)) && b.time < DateTime.Now.Date

                                         select new
                                         {
                                             Date = b.time,
                                             Id = b.id,
                                             Lat = b.latitude,
                                             Long = b.longitude,
                                             DeviceName = db.Tbl_DeviceRegistrations.Where(t => t.DeviceIMEI == b.imei).Select(t => t.DeviceName).SingleOrDefault(),
                                             imei = b.imei,
                                             speed = b.speed,
                                             Odeometer = b.odometer,
                                             hdop = b.hdop,
                                             scount = b.satellites,
                                             gsmStength = b.recieved_signal_strength
                                         }).ToList();
                            return Json(data1, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else if (product.filter == "8")
                    {

                        try
                        {
                          
                           
                            //uId = (from a in db.Tbl_Users where a.username == Request.RequestContext.HttpContext.Session["username"].ToString() select a.UserID).SingleOrDefault();
                            //UserId = Convert.ToInt32(uId);
                            var data1 = (from a in db.telemetries
                                         orderby a.id descending
                                         where a.imei == Convert.ToInt64(product.Dropdown) && a.time > Convert.ToDateTime(previousWeekStart) && a.time < previousWeekEnd
                            select new
                                         {

                                             Date = a.time,
                                             Id = a.id,
                                             Lat = a.latitude,
                                             Long = a.longitude,
                                             DeviceName = db.Tbl_DeviceRegistrations.Where(t => t.DeviceIMEI == a.imei).Select(t => t.DeviceName).SingleOrDefault(),
                                             imei = a.imei,
                                             speed = a.speed,
                                             Odeometer = a.odometer,
                                             hdop = a.hdop,
                                             scount = a.satellites,
                                             gsmStength = a.recieved_signal_strength
                                         }).ToList();
                            return Json(data1, JsonRequestBehavior.AllowGet);
                        }
                        catch (Exception ex)
                        {
                          
                            var data1 = (from b in db.telemetries
                                         orderby b.id descending
                                         where b.time > Convert.ToDateTime(previousWeekStart) && b.time < previousWeekEnd

                                         select new
                                         {
                                             Date = b.time,
                                             Id = b.id,
                                             Lat = b.latitude,
                                             Long = b.longitude,
                                             DeviceName = db.Tbl_DeviceRegistrations.Where(t => t.DeviceIMEI == b.imei).Select(t => t.DeviceName).SingleOrDefault(),
                                             imei = b.imei,
                                             speed = b.speed,
                                             Odeometer = b.odometer,
                                             hdop = b.hdop,
                                             scount = b.satellites,
                                             gsmStength = b.recieved_signal_strength
                                         }).ToList();
                            return Json(data1, JsonRequestBehavior.AllowGet);
                        }

                    }
                    else if (product.filter == "30")
                    {
                      
                        //uId = (from a in db.Tbl_Users where a.username == Request.RequestContext.HttpContext.Session["username"].ToString() select a.UserID).SingleOrDefault();
                        //UserId = Convert.ToInt32(uId);
                        var data1 = (from a in db.telemetries
                                     orderby a.id descending
                                     where a.imei == Convert.ToInt64(product.Dropdown) && a.time >=Convert.ToDateTime(firstDayLastMonth) && a.time <= lastDayLastMonth
                                     select new
                                     {

                                         Date = a.time,
                                         Id = a.id,
                                         Lat = a.latitude,
                                         Long = a.longitude,
                                         DeviceName = db.Tbl_DeviceRegistrations.Where(t => t.DeviceIMEI == a.imei).Select(t => t.DeviceName).SingleOrDefault(),
                                         imei = a.imei,
                                         speed = a.speed,
                                         Odeometer = a.odometer,
                                         hdop = a.hdop,
                                         scount = a.satellites,
                                         gsmStength = a.recieved_signal_strength
                                     }).ToList();
                        return Json(data1, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception)
            {
                if (product.filter != "All")
                {
                    from1 = Convert.ToDateTime(product.From);
                    to = Convert.ToDateTime(product.To);
                    deviceid = Convert.ToInt32(product.IMEI);
                    //uId = (from a in db.Tbl_Users where a.username == Request.RequestContext.HttpContext.Session["username"].ToString() select a.UserID).SingleOrDefault();
                    //UserId = Convert.ToInt32(uId);
                    var data = (from a in db.telemetries
                                where a.imei == Convert.ToInt64(product.Dropdown) && a.time > Convert.ToDateTime(product.To) && a.time <= Convert.ToDateTime(product.From.AddDays(1))
                                orderby a.id descending
                                select new
                                {

                                    Date = a.time,
                                    Id = a.id,
                                    Lat = a.latitude,
                                    Long = a.longitude,
                                    DeviceName = db.Tbl_DeviceRegistrations.Where(t => t.DeviceIMEI == a.imei).Select(t => t.DeviceName).SingleOrDefault(),
                                    imei = a.imei,
                                    speed = a.speed,
                                    Odeometer = a.odometer,
                                    hdop = a.hdop,
                                    scount = a.satellites,
                                    gsmStength = a.recieved_signal_strength
                                }).ToList();
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
         

            }

            try
            {
                from1 = Convert.ToDateTime(product.From);
                to = Convert.ToDateTime(product.To);
                deviceid = Convert.ToInt32(product.IMEI);
                uId = (from a in db.Tbl_Users where a.username == Request.RequestContext.HttpContext.Session["username"].ToString() select a.UserID).SingleOrDefault();
                UserId = Convert.ToInt32(uId);
                var data = (from a in db.telemetries
                            where a.imei == Convert.ToInt64(product.Dropdown) && a.time > Convert.ToDateTime(product.From) && a.time <= Convert.ToDateTime(product.To.AddDays(1))
                            orderby a.id descending
                            select new
                            {

                                Date = a.time,
                                Id = a.id,
                                Lat = a.latitude,
                                Long = a.longitude,
                                DeviceName = db.Tbl_DeviceRegistrations.Where(t => t.DeviceIMEI == a.imei).Select(t => t.DeviceName).SingleOrDefault(),
                                imei = a.imei,
                                speed = a.speed,
                                Odeometer = a.odometer,
                                hdop = a.hdop,
                                scount = a.satellites,
                                gsmStength = a.recieved_signal_strength
                            }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                var data1 = (from b in db.telemetries
                             orderby b.id descending
                             where b.time > Convert.ToDateTime(product.To) && b.time <= Convert.ToDateTime(product.From.AddDays(1))
                             select new
                             {
                                 Date = b.time,
                                 Id = b.id,
                                 Lat = b.latitude,
                                 Long = b.longitude,
                                 DeviceName = db.Tbl_DeviceRegistrations.Where(t => t.DeviceIMEI == b.imei).Select(t => t.DeviceName).SingleOrDefault(),
                                 imei = b.imei,
                                 speed = b.speed,
                                 Odeometer = b.odometer,
                                 hdop = b.hdop,
                                 scount = b.satellites,
                                 gsmStength = b.recieved_signal_strength
                             }).ToList();
                return Json(data1, JsonRequestBehavior.AllowGet);

            }
        }


        
        //public JsonResult GetOverSpeedLog(product product)
        //{
        //    var uId = (from a in db.Tbl_Users where a.username == Request.RequestContext.HttpContext.Session["username"].ToString() select a.UserID).SingleOrDefault();

        //    try
        //    {
        //        if (product.filter.ToString() != "-1" && product.filter != null && product.filter != "cc")
        //        {
        //            if (product.filter == "1")
        //            {
        //                try
        //                {
        //                    DateTime fil = DateTime.Now.Date;


        //                    from1 = Convert.ToDateTime(product.From);
        //                    to = Convert.ToDateTime(product.To);
        //                    deviceid = Convert.ToInt32(product.IMEI);
        //                    uId = (from a in db.Tbl_Users where a.username == Request.RequestContext.HttpContext.Session["username"].ToString() select a.UserID).SingleOrDefault();
        //                    UserId = Convert.ToInt32(uId);
        //                    var data1 = (from a in db.telemetries
        //                                 orderby a.id descending
        //                                 where a.speed > 45 && a.imei == Convert.ToInt64(product.Dropdown) && a.time > Convert.ToDateTime(DateTime.Now.Date)
        //                                 select new
        //                                 {
        //                                     Date = a.time,
        //                                     Id = a.id,
        //                                     Lat = a.latitude,
        //                                     Long = a.longitude,
        //                                     DeviceName = db.Tbl_DeviceRegistrations.Where(t => t.DeviceIMEI == a.imei).Select(t => t.DeviceName).SingleOrDefault(),
        //                                     imei = a.imei,
        //                                     speed = a.speed,
        //                                     Odeometer = a.odometer,
        //                                     hdop = a.hdop,
        //                                     scount = a.satellites,
        //                                     gsmStength = a.recieved_signal_strength
        //                                 }).ToList();
        //                    return Json(data1, JsonRequestBehavior.AllowGet);
        //                }
        //                catch (Exception)
        //                {

        //                    DateTime fil = DateTime.Now.Date;
        //                    from1 = Convert.ToDateTime(product.From);
        //                    to = Convert.ToDateTime(product.To);
        //                    deviceid = Convert.ToInt32(product.IMEI);
        //                    uId = (from a in db.Tbl_Users where a.username == Request.RequestContext.HttpContext.Session["username"].ToString() select a.UserID).SingleOrDefault();
        //                    UserId = Convert.ToInt32(uId);
        //                    var data1 = (from a in db.telemetries
        //                                 orderby a.id descending
        //                                 where a.speed > 45&& a.time > Convert.ToDateTime(DateTime.Now.Date)
        //                                 select new
        //                                 {

        //                                     Date = a.time,
        //                                     Id = a.id,
        //                                     Lat = a.latitude,
        //                                     Long = a.longitude,
        //                                     DeviceName = db.Tbl_DeviceRegistrations.Where(t => t.DeviceIMEI == a.imei).Select(t => t.DeviceName).SingleOrDefault(),
        //                                     imei = a.imei,
        //                                     speed = a.speed,
        //                                     Odeometer = a.odometer,
        //                                     hdop = a.hdop,
        //                                     scount = a.satellites,
        //                                     gsmStength = a.recieved_signal_strength
        //                                 }).ToList();
        //                    return Json(data1, JsonRequestBehavior.AllowGet);

        //                }
        //            }
        //            else if (product.filter.ToString() == "2")
        //            {

        //                try
        //                {
        //                    uId = (from a in db.Tbl_Users where a.username == Request.RequestContext.HttpContext.Session["username"].ToString() select a.UserID).SingleOrDefault();
        //                    UserId = Convert.ToInt32(uId);
        //                    var data1 = (from a in db.telemetries
        //                                 orderby a.id descending
        //                                 where a.speed > 45 && a.imei == Convert.ToInt64(product.Dropdown) && a.time > Convert.ToDateTime(DateTime.Now.AddDays(-1)) && a.time < DateTime.Now.Date
        //                                 select new
        //                                 {
        //                                     Date = a.time,
        //                                     Id = a.id,
        //                                     Lat = a.latitude,
        //                                     Long = a.longitude,
        //                                     DeviceName = db.Tbl_DeviceRegistrations.Where(t => t.DeviceIMEI == a.imei).Select(t => t.DeviceName).SingleOrDefault(),
        //                                     imei = a.imei,
        //                                     speed = a.speed,
        //                                     Odeometer = a.odometer,
        //                                     hdop = a.hdop,
        //                                     scount = a.satellites,
        //                                     gsmStength = a.recieved_signal_strength
        //                                 }).ToList();
        //                    return Json(data1, JsonRequestBehavior.AllowGet);
        //                }
        //                catch (Exception)
        //                {
        //                    uId = (from a in db.Tbl_Users where a.username == Request.RequestContext.HttpContext.Session["username"].ToString() select a.UserID).SingleOrDefault();
        //                    UserId = Convert.ToInt32(uId);
        //                    var data1 = (from a in db.telemetries
        //                                 orderby a.id descending
        //                                 where a.speed > 45  && a.time > Convert.ToDateTime(DateTime.Now.AddDays(-1)) && a.time < DateTime.Now.Date
        //                                 select new
        //                                 {

        //                                     Date = a.time,
        //                                     Id = a.id,
        //                                     Lat = a.latitude,
        //                                     Long = a.longitude,
        //                                     DeviceName = db.Tbl_DeviceRegistrations.Where(t => t.DeviceIMEI == a.imei).Select(t => t.DeviceName).SingleOrDefault(),
        //                                     imei = a.imei,
        //                                     speed = a.speed,
        //                                     Odeometer = a.odometer,
        //                                     hdop = a.hdop,
        //                                     scount = a.satellites,
        //                                     gsmStength = a.recieved_signal_strength
        //                                 }).ToList();
        //                    return Json(data1, JsonRequestBehavior.AllowGet);

        //                }
        //            }
        //            else if (product.filter == "8")
        //            {

        //                try
        //                {
        //                    DayOfWeek weekStart = DayOfWeek.Monday; // or Sunday, or whenever
        //                    DateTime startingDate = DateTime.Today;

        //                    while (startingDate.DayOfWeek != weekStart)
        //                        startingDate = startingDate.AddDays(-1);

        //                    DateTime previousWeekStart = startingDate.AddDays(-7);
        //                    DateTime previousWeekEnd = startingDate.AddDays(-1);

        //                    uId = (from a in db.Tbl_Users where a.username == Request.RequestContext.HttpContext.Session["username"].ToString() select a.UserID).SingleOrDefault();
        //                    UserId = Convert.ToInt32(uId);
        //                    var data1 = (from a in db.telemetries
        //                                 orderby a.id descending
        //                                 where a.speed > 45 && a.imei == Convert.ToInt64(product.Dropdown) && a.time > Convert.ToDateTime(previousWeekStart) && a.time <= previousWeekEnd.AddDays(1)
        //                                 select new
        //                                 {

        //                                     Date = a.time,
        //                                     Id = a.id,
        //                                     Lat = a.latitude,
        //                                     Long = a.longitude,
        //                                     DeviceName = db.Tbl_DeviceRegistrations.Where(t => t.DeviceIMEI == a.imei).Select(t => t.DeviceName).SingleOrDefault(),
        //                                     imei = a.imei,
        //                                     speed = a.speed,
        //                                     Odeometer = a.odometer,
        //                                     hdop = a.hdop,
        //                                     scount = a.satellites,
        //                                     gsmStength = a.recieved_signal_strength
        //                                 }).ToList();
        //                    return Json(data1, JsonRequestBehavior.AllowGet);
        //                }
        //                catch (Exception)
        //                {

        //                    DayOfWeek weekStart = DayOfWeek.Monday; // or Sunday, or whenever
        //                    DateTime startingDate = DateTime.Today;

        //                    while (startingDate.DayOfWeek != weekStart)
        //                        startingDate = startingDate.AddDays(-1);

        //                    DateTime previousWeekStart = startingDate.AddDays(-7);
        //                    DateTime previousWeekEnd = startingDate.AddDays(-1);

        //                    uId = (from a in db.Tbl_Users where a.username == Request.RequestContext.HttpContext.Session["username"].ToString() select a.UserID).SingleOrDefault();
        //                    UserId = Convert.ToInt32(uId);
        //                    var data1 = (from a in db.telemetries
        //                                 orderby a.id descending
        //                                 where a.speed > 45  && a.time > Convert.ToDateTime(previousWeekStart) && a.time <= previousWeekEnd.AddDays(1)
        //                                 select new
        //                                 {

        //                                     Date = a.time,
        //                                     Id = a.id,
        //                                     Lat = a.latitude,
        //                                     Long = a.longitude,
        //                                     DeviceName = db.Tbl_DeviceRegistrations.Where(t => t.DeviceIMEI == a.imei).Select(t => t.DeviceName).SingleOrDefault(),
        //                                     imei = a.imei,
        //                                     speed = a.speed,
        //                                     Odeometer = a.odometer,
        //                                     hdop = a.hdop,
        //                                     scount = a.satellites,
        //                                     gsmStength = a.recieved_signal_strength
        //                                 }).ToList();
        //                    return Json(data1, JsonRequestBehavior.AllowGet);
        //                }

        //            }
        //            else if (product.filter == "30")
        //            {

        //                try
        //                {
        //                    DateTime firstDayLastMonth = DateTime.Now.AddDays(1 - DateTime.Now.Day).AddMonths(-1);
        //                    DateTime lastDayLastMonth = new DateTime(firstDayLastMonth.Year, firstDayLastMonth.Month, DateTime.DaysInMonth(firstDayLastMonth.Year, firstDayLastMonth.Month));

        //                    uId = (from a in db.Tbl_Users where a.username == Request.RequestContext.HttpContext.Session["username"].ToString() select a.UserID).SingleOrDefault();
        //                    UserId = Convert.ToInt32(uId);
        //                    var data1 = (from a in db.telemetries
        //                                 orderby a.id descending
        //                                 where a.speed > 45 && a.imei == Convert.ToInt64(product.Dropdown) &&a.time > Convert.ToDateTime(firstDayLastMonth) && a.time < lastDayLastMonth
        //                                 select new
        //                                 {

        //                                     Date = a.time,
        //                                     Id = a.id,
        //                                     Lat = a.latitude,
        //                                     Long = a.longitude,
        //                                     DeviceName = db.Tbl_DeviceRegistrations.Where(t => t.DeviceIMEI == a.imei).Select(t => t.DeviceName).SingleOrDefault(),
        //                                     imei = a.imei,
        //                                     speed = a.speed,
        //                                     Odeometer = a.odometer,
        //                                     hdop = a.hdop,
        //                                     scount = a.satellites,
        //                                     gsmStength = a.recieved_signal_strength
        //                                 }).ToList();
        //                    return Json(data1, JsonRequestBehavior.AllowGet);
        //                }
        //                catch (Exception)
        //                {


        //                    DateTime firstDayLastMonth = DateTime.Now.AddDays(1 - DateTime.Now.Day).AddMonths(-1);
        //                    DateTime lastDayLastMonth = new DateTime(firstDayLastMonth.Year, firstDayLastMonth.Month, DateTime.DaysInMonth(firstDayLastMonth.Year, firstDayLastMonth.Month));

        //                    uId = (from a in db.Tbl_Users where a.username == Request.RequestContext.HttpContext.Session["username"].ToString() select a.UserID).SingleOrDefault();
        //                    UserId = Convert.ToInt32(uId);
        //                    var data1 = (from a in db.telemetries
        //                                 orderby a.id descending
        //                                 where a.speed > 45 && a.time > Convert.ToDateTime(firstDayLastMonth) && a.time < lastDayLastMonth
        //                                 select new
        //                                 {

        //                                     Date = a.time,
        //                                     Id = a.id,
        //                                     Lat = a.latitude,
        //                                     Long = a.longitude,
        //                                     DeviceName = db.Tbl_DeviceRegistrations.Where(t => t.DeviceIMEI == a.imei).Select(t => t.DeviceName).SingleOrDefault(),
        //                                     imei = a.imei,
        //                                     speed = a.speed,
        //                                     Odeometer = a.odometer,
        //                                     hdop = a.hdop,
        //                                     scount = a.satellites,
        //                                     gsmStength = a.recieved_signal_strength
        //                                 }).ToList();
        //                    return Json(data1, JsonRequestBehavior.AllowGet);
        //                }

        //            }

        //        }


        //        try
        //        {
        //            from1 = Convert.ToDateTime(product.From);
        //            to = Convert.ToDateTime(product.To);
        //            deviceid = Convert.ToInt32(product.IMEI);
        //            uId = (from a in db.Tbl_Users where a.username == Request.RequestContext.HttpContext.Session["username"].ToString() select a.UserID).SingleOrDefault();
        //            UserId = Convert.ToInt32(uId);
        //            var data = (from a in db.telemetries

        //                        where a.speed > 45 && a.imei == Convert.ToInt64(product.Dropdown) && a.time > Convert.ToDateTime(product.To) && a.time <= Convert.ToDateTime(product.From.AddDays(1))
        //                        orderby a.id descending
        //                        select new
        //                        {

        //                            Date = a.time,
        //                            Id = a.id,
        //                            Lat = a.latitude,
        //                            Long = a.longitude,
        //                            DeviceName = db.Tbl_DeviceRegistrations.Where(t => t.DeviceIMEI == a.imei).Select(t => t.DeviceName).SingleOrDefault(),
        //                            imei = a.imei,
        //                            speed = a.speed,
        //                            Odeometer = a.odometer,
        //                            hdop = a.hdop,
        //                            scount = a.satellites,
        //                            gsmStength = a.recieved_signal_strength
        //                        }).ToList();
        //            return Json(data, JsonRequestBehavior.AllowGet);
        //        }
        //        catch (Exception)
        //        {
        //            from1 = Convert.ToDateTime(product.From);
        //            to = Convert.ToDateTime(product.To);
        //            deviceid = Convert.ToInt32(product.IMEI);
        //            uId = (from a in db.Tbl_Users where a.username == Request.RequestContext.HttpContext.Session["username"].ToString() select a.UserID).SingleOrDefault();
        //            UserId = Convert.ToInt32(uId);
        //            var data = (from a in db.telemetries

        //                        where a.speed > 45 &&  a.time > Convert.ToDateTime(product.To) && a.time <= Convert.ToDateTime(product.From.AddDays(1))
        //                        orderby a.id descending
        //                        select new
        //                        {

        //                            Date = a.time,
        //                            Id = a.id,
        //                            Lat = a.latitude,
        //                            Long = a.longitude,
        //                            DeviceName = db.Tbl_DeviceRegistrations.Where(t => t.DeviceIMEI == a.imei).Select(t => t.DeviceName).SingleOrDefault(),
        //                            imei = a.imei,
        //                            speed = a.speed,
        //                            Odeometer = a.odometer,
        //                            hdop = a.hdop,
        //                            scount = a.satellites,
        //                            gsmStength = a.recieved_signal_strength
        //                        }).ToList();
        //            return Json(data, JsonRequestBehavior.AllowGet);

        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        from1 = Convert.ToDateTime(product.From);
        //        to = Convert.ToDateTime(product.To);
        //        deviceid = Convert.ToInt32(product.IMEI);
        //        uId = (from a in db.Tbl_Users where a.username == Request.RequestContext.HttpContext.Session["username"].ToString() select a.UserID).SingleOrDefault();
        //        UserId = Convert.ToInt32(uId);
        //        var data = (from a in db.telemetries

        //                    where a.speed > 45 && a.imei == Convert.ToInt64(product.Dropdown) && a.time > Convert.ToDateTime(product.To) && a.time <= Convert.ToDateTime(product.From.AddDays(1))
        //                    orderby a.id descending
        //                    select new
        //                    {

        //                        Date = a.time,
        //                        Id = a.id,
        //                        Lat = a.latitude,
        //                        Long = a.longitude,
        //                        DeviceName = db.Tbl_DeviceRegistrations.Where(t => t.DeviceIMEI == a.imei).Select(t => t.DeviceName).SingleOrDefault(),
        //                        imei = a.imei,
        //                        speed = a.speed,
        //                        Odeometer = a.odometer,
        //                        hdop = a.hdop,
        //                        scount = a.satellites,
        //                        gsmStength = a.recieved_signal_strength
        //                    }).ToList();
        //        return Json(data, JsonRequestBehavior.AllowGet);

        //    }


        //}

            //for beginend day report page table
        public JsonResult GetLog1(product product)
        {
            try
            {
                var s = (string)Session["username"];


                if (s == "" || s == null)
                {
                    Response.Write("Your session has een expired please login again");
                    Response.Redirect("/Login.aspx");

                }
                var username = (string)Session["username"];
            var uId = (from a in db.Tbl_Users where a.username == username.ToString() select a.UserID).SingleOrDefault();
            UserId = Convert.ToInt32(uId);

              
                    DateTime fil = DateTime.Now.Date;
                    from1 = Convert.ToDateTime(DateTime.Now.Date.AddDays(-1));
                    to = Convert.ToDateTime(DateTime.Now.Date);
                    var imei = (from b in db.Tbl_DeviceRegistrations where b.DeviceName == product.Dropdown.Trim() select b.DeviceIMEI).SingleOrDefault();
                    product.IMEI = imei.ToString();
                    //uId = (from a in db.Tbl_Users where a.username == Request.RequestContext.HttpContext.Session["username"].ToString() select a.UserID).SingleOrDefault();



                    var data = (from a in db.telemetries
                                //orderby a.id descending
                            where a.imei == Convert.ToInt64(imei) &&a.latitude!=null&&a.longitude!=null && a.time > DateTime.Now.Date
                            group a by a.imei into g
                            select new
                            {
                                CurrentDate = DateTime.Now,
                                Date = g.First().time,
                                Id = g.First().id,
                                DeviceName = db.Tbl_DeviceRegistrations.Where(t => t.DeviceIMEI == g.First().imei).Select(t => t.DeviceName).SingleOrDefault(),
                                imei = g.First().imei,
                                workingStartLat = db.telemetries.Where(t => t.imei == Convert.ToInt64(product.IMEI) && t.digital_inputs > 32950 && t.time > Convert.ToDateTime(DateTime.Now.Date)).Select(t => t.latitude).FirstOrDefault(),
                                workingStartLong = db.telemetries.Where(t => t.imei == Convert.ToInt64(product.IMEI) && t.digital_inputs > 32950 && t.time > Convert.ToDateTime(DateTime.Now.Date)).Select(t => t.longitude).FirstOrDefault(),
                                workingEndLat = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == Convert.ToInt64(product.IMEI) && t.digital_inputs < 32950 && t.time > Convert.ToDateTime(DateTime.Now.Date)).Select(t => t.latitude).FirstOrDefault(),
                                workingEndLong = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == Convert.ToInt64(product.IMEI) && t.digital_inputs < 32950 && t.time > Convert.ToDateTime(DateTime.Now.Date)).Select(t => t.longitude).FirstOrDefault(),
                                Maxspeed = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == Convert.ToInt64(product.IMEI) && t.time > DateTime.Now.Date).Select(t => t.speed).Max(),
                                AvrageSpeed = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == Convert.ToInt64(product.IMEI) && t.speed != 0 && t.time > DateTime.Now.Date).Average(t => t.speed),
                                totoalDistance = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == Convert.ToInt64(product.IMEI) && t.digital_inputs != null && t.digital_inputs > 32950 && t.time > Convert.ToDateTime(DateTime.Now.Date)).Select(t => Convert.ToInt64(t.odometer)).Sum(),
                                Motion = db.telemetries.Where(t => t.imei == g.First().imei && t.digital_inputs > 32950 && t.time > DateTime.Now.Date).Select(t => Convert.ToDateTime(t.time).Second).Sum(),
                                Stop = db.telemetries.Where(t => t.imei == g.First().imei && t.digital_inputs < 32950 && t.time > DateTime.Now.Date).Select(t => Convert.ToDateTime(t.time).Second).Sum(),
                                Idel = db.telemetries.Where(t => t.imei == g.First().imei && t.digital_inputs == 32953 && t.time > DateTime.Now.Date).Select(t => Convert.ToDateTime(t.time).Second).Sum(),

                            }).ToList();

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var data = ex.Message.ToString();
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
                     
                       
        }
        //start stop log table data
        public JsonResult StartStopLog(product product)
        {
            var s = (string)Session["username"];


            if (s == "" || s == null)
            {
                Response.Write("Your session has een expired please login again");
                Response.Redirect("/Login.aspx");

            }
            var uId = 1;
          
                DateTime fil = DateTime.Now.Date;
                from1 = Convert.ToDateTime(DateTime.Now.Date.AddDays(-1));
                to = Convert.ToDateTime(DateTime.Now.Date);
                var imei = (from b in db.Tbl_DeviceRegistrations where b.DeviceName == product.IMEI.Trim() select b.DeviceIMEI).SingleOrDefault();
            //product.IMEI = imei.ToString();
            product.IMEI = "866710039995144";
                //uId = (from a in db.Tbl_Users where a.username == Request.RequestContext.HttpContext.Session["username"].ToString() select a.UserID).SingleOrDefault();
                uId = 1;
                UserId = Convert.ToInt32(uId);
            try
            {
                var data1 = (from a in db.telemetries
                                 orderby a.id ascending
                             where a.imei == Convert.ToInt64(product.IMEI) && a.time > Convert.ToDateTime(DateTime.Now.Date).AddDays(-1)
                             group a by a.id into g
                          
                             select new
                             {
                                 CurrentDate = DateTime.Now,
                                 Date = g.First().time,
                                 // stoptime = db.telemetries.Where(t => t.imei == a.imei).Select(t => t.time).SingleOrDefault(),
                                 Id = g.First().id,
                                 DeviceName = db.Tbl_DeviceRegistrations.Where(t => t.DeviceIMEI == g.First().imei).Select(t => t.DeviceName).SingleOrDefault(),
                                 imei = g.First().imei,
                                 Distance = g.First().odometer,
                                 state = g.First().digital_inputs,
                                 odeometer = g.First().odometer,
                                 Lat = g.First().latitude,
                                 Long = g.First().longitude,
                               
                                 //motion1=(double?)(from v in db.telemetries
                                 //              group v by v.time into f
                                 //                where f.First().imei == g.First().imei && f.First().digital_inputs > 32950
                                 //                && f.First().time > Convert.ToDateTime(DateTime.Now.Date)
                                 //                select Convert.ToDateTime(f.First().time).Hour).Sum()
                                 //Motion= db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == Convert.ToInt64(product.IMEI) && t.time != null && t.digital_inputs > 32950 && t.time > Convert.ToDateTime(DateTime.Now.Date)).Select(t => Convert.ToDateTime(t.time).Minute).Sum(),
                                 //Idel = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == Convert.ToInt64(product.IMEI) && t.time != null && t.digital_inputs == 32952 && t.time > Convert.ToDateTime(DateTime.Now.Date)).Select(t => Convert.ToDateTime(t.time).Minute).Sum(),
                                 //Stop = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == Convert.ToInt64(product.IMEI) && t.time != null && t.digital_inputs < 32952 && t.time > Convert.ToDateTime(DateTime.Now.Date)).Select(t => Convert.ToDateTime(t.time).Minute).Sum(),
                                 //data1=(db.telemetries.Where(t => t.imei == Convert.ToInt64(g.First().imei) && t.digital_inputs > 32950 && t.time > Convert.ToDateTime(DateTime.Now.Date)).Select(t =>Convert.ToDateTime( t.time)).SingleOrDefault().Subtract(db.telemetries.Where(t => t.imei == Convert.ToInt64(g.First().imei) && t.digital_inputs < 32950 && t.time > Convert.ToDateTime(DateTime.Now.Date)).Select(t => Convert.ToDateTime(t.time)).FirstOrDefault())),

                             }).Distinct().ToList();
              
              
             
                return Json(data1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw;
            }
               
              
            }
        public JsonResult getDistance(product product)
        {
            var s = (string)Session["username"];


            if (s == "" || s == null)
            {
                Response.Write("Your session has een expired please login again");
                Response.Redirect("/Login.aspx");

            }
            var uId = 1;

            DateTime fil = DateTime.Now.Date;
            from1 = Convert.ToDateTime(DateTime.Now.Date.AddDays(-1));
            to = Convert.ToDateTime(DateTime.Now.Date);
            var imei = (from b in db.Tbl_DeviceRegistrations where b.DeviceName == product.IMEI.Trim() select b.DeviceIMEI).SingleOrDefault();
            //product.IMEI = imei.ToString();
            product.IMEI = product.IMEI.Trim();
            uId = (from a in db.Tbl_Users where a.username == Request.RequestContext.HttpContext.Session["username"].ToString() select a.UserID).SingleOrDefault();
            /*uId = 1*/;
            UserId = Convert.ToInt32(uId);
            try
            {
                var data1 = (from a in db.telemetries
                            // orderby a.id descending
                             where a.imei == Convert.ToInt64(product.IMEI) && a.time > Convert.ToDateTime(DateTime.Now.Date)

                             select new
                             {
                                 CurrentDate = DateTime.Now,
                                 Date = a.time,
                                 // stoptime = db.telemetries.Where(t => t.imei == a.imei).Select(t => t.time).SingleOrDefault(),
                                 Id = a.id,
                                 DeviceName = db.Tbl_DeviceRegistrations.Where(t => t.DeviceIMEI == a.imei).Select(t => t.DeviceName).SingleOrDefault(),
                                 imei = a.imei,
                                 Distance = a.odometer,
                                 state = a.digital_inputs,
                                 odeometer = a.odometer,
                                 Lat = a.latitude,
                                 Long = a.longitude
                                 //motion1 = (double?)(from v in db.telemetries
                                 //                    group v by v.time into f
                                 //                    where f.First().imei == a.imei && f.First().digital_inputs > 32950
                                 //                    && f.First().time > Convert.ToDateTime(DateTime.Now.Date)
                                 //                    select f.First().odometer).SingleOrDefault()
                                 //motion1=(double?)(from v in db.telemetries
                                 //              group v by v.time into f
                                 //                where f.First().imei == g.First().imei && f.First().digital_inputs > 32950
                                 //                && f.First().time > Convert.ToDateTime(DateTime.Now.Date)
                                 //                select Convert.ToDateTime(f.First().time).Hour).Sum()
                                 //Motion= db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == Convert.ToInt64(product.IMEI) && t.time != null && t.digital_inputs > 32950 && t.time > Convert.ToDateTime(DateTime.Now.Date)).Select(t => Convert.ToDateTime(t.time).Minute).Sum(),
                                 //Idel = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == Convert.ToInt64(product.IMEI) && t.time != null && t.digital_inputs == 32952 && t.time > Convert.ToDateTime(DateTime.Now.Date)).Select(t => Convert.ToDateTime(t.time).Minute).Sum(),
                                 //Stop = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == Convert.ToInt64(product.IMEI) && t.time != null && t.digital_inputs < 32952 && t.time > Convert.ToDateTime(DateTime.Now.Date)).Select(t => Convert.ToDateTime(t.time).Minute).Sum(),
                                 //data1=(db.telemetries.Where(t => t.imei == Convert.ToInt64(g.First().imei) && t.digital_inputs > 32950 && t.time > Convert.ToDateTime(DateTime.Now.Date)).Select(t =>Convert.ToDateTime( t.time)).SingleOrDefault().Subtract(db.telemetries.Where(t => t.imei == Convert.ToInt64(g.First().imei) && t.digital_inputs < 32950 && t.time > Convert.ToDateTime(DateTime.Now.Date)).Select(t => Convert.ToDateTime(t.time)).FirstOrDefault())),

                             }).ToList();



                return Json(data1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        //for begin end day report pdf
        public ActionResult ReportBeginEndDayPDF(String vno, String from1, String to)
        {
            var s = (string)Session["username"];


            if (s == "" || s == null)
            {
                Response.Write("Your session has een expired please login again");
                Response.Redirect("/Login.aspx");

            }
           
          
           
                DateTime fil = DateTime.Now.Date;
                
                var imei = (from b in db.Tbl_DeviceRegistrations where b.DeviceName == vno.Trim() select b.DeviceIMEI).SingleOrDefault();

            var uId = (from a in db.Tbl_Users where a.username == Request.RequestContext.HttpContext.Session["username"].ToString() select a.UserID).SingleOrDefault();
            //uId = 1;
                UserId = Convert.ToInt32(uId);
                var data1 = (from a in db.telemetries
                                 //orderby a.id descending
                             where a.imei == Convert.ToInt64(imei) && a.time > Convert.ToDateTime(DateTime.Now.Date)
                             group a by a.imei into g
                             select new
                             {
                                 CurrentDate = DateTime.Now,
                                 Date = g.First().time,
                                 Id = g.First().id,
                                 DeviceName = db.Tbl_DeviceRegistrations.Where(t => t.DeviceIMEI == g.First().imei).Select(t => t.DeviceName).SingleOrDefault(),
                                 imei = g.First().imei,
                                 workingStartLat = db.telemetries.Where(t => t.imei == Convert.ToInt64(imei) && t.digital_inputs > 32950 && t.time > Convert.ToDateTime(DateTime.Now.Date)).Select(t => t.latitude).FirstOrDefault(),
                                 workingStartLong = db.telemetries.Where(t => t.imei == Convert.ToInt64(imei) && t.digital_inputs > 32950 && t.time > Convert.ToDateTime(DateTime.Now.Date)).Select(t => t.longitude).FirstOrDefault(),
                                 workingEndLat = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == Convert.ToInt64(imei) && t.digital_inputs < 32950 && t.time > Convert.ToDateTime(DateTime.Now.Date)).Select(t => t.latitude).FirstOrDefault(),
                                 workingEndLong = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == Convert.ToInt64(imei) && t.digital_inputs < 32950 && t.time > Convert.ToDateTime(DateTime.Now.Date)).Select(t => t.longitude).FirstOrDefault(),
                                 Maxspeed = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == Convert.ToInt64(imei) && t.time > DateTime.Now.Date).Select(t => t.speed).Max(),
                                 AvrageSpeed = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == Convert.ToInt64(imei) && t.speed != 0 && t.time > DateTime.Now.Date).Average(t => t.speed),
                                 totoalDistance = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == Convert.ToInt64(imei) && t.digital_inputs != null && t.digital_inputs > 32950 && t.time > Convert.ToDateTime(DateTime.Now.Date)).Select(t => Convert.ToInt64(t.odometer)).Sum(),
                                 Motion = db.telemetries.Where(t => t.imei == g.First().imei && t.digital_inputs > 32950 && t.time > DateTime.Now.Date).Select(t => Convert.ToDateTime(t.time).Second).Sum(),
                                 Stop = db.telemetries.Where(t => t.imei == g.First().imei && t.digital_inputs < 32950 && t.time > DateTime.Now.Date).Select(t => Convert.ToDateTime(t.time).Second).Sum(),
                                 Idel = db.telemetries.Where(t => t.imei == g.First().imei && t.digital_inputs == 32953 && t.time > DateTime.Now.Date).Select(t => Convert.ToDateTime(t.time).Second).Sum(),
                              

                             }).ToList();
                  
            LocalReport lc1 = new LocalReport();
            lc1.ReportPath = Server.MapPath("~/Reports/RptBeginEndDay.rdlc");
            ReportDataSource rd1 = new ReportDataSource("ReportBeginEndDay", data1.ToList());
            rd1.Value = data1;
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
            Response.AddHeader("content-disposition", "attachment;filename=BeginEndDay_Report." + filenameextention1);
            return File(renderdbyte1, "." + filenameextention1);
        }
        //for begin end day report excel
        public ActionResult ReportBeginEndDayExcel(String vno, String from1, String to)
        {
            var s = (string)Session["username"];


            if (s == "" || s == null)
            {
                Response.Write("Your session has een expired please login again");
                Response.Redirect("/Login.aspx");

            }
            
          

            DateTime fil = DateTime.Now.Date;

            var imei = (from b in db.Tbl_DeviceRegistrations where b.DeviceName == vno.Trim() select b.DeviceIMEI).SingleOrDefault();

           var uId = (from a in db.Tbl_Users where a.username == Request.RequestContext.HttpContext.Session["username"].ToString() select a.UserID).SingleOrDefault();
            //uId = 1;
            UserId = Convert.ToInt32(uId);
            var data1 = (from a in db.telemetries
                             //orderby a.id descending
                         where a.imei == Convert.ToInt64(imei) && a.time > Convert.ToDateTime(DateTime.Now.Date)
                         group a by a.imei into g
                         select new
                         {
                             CurrentDate = DateTime.Now,
                             Date = g.First().time,
                             Id = g.First().id,
                             DeviceName = db.Tbl_DeviceRegistrations.Where(t => t.DeviceIMEI == g.First().imei).Select(t => t.DeviceName).SingleOrDefault(),
                             imei = g.First().imei,
                             workingStartLat = db.telemetries.Where(t => t.imei == Convert.ToInt64(imei) && t.digital_inputs > 32950 && t.time > Convert.ToDateTime(DateTime.Now.Date)).Select(t => t.latitude).FirstOrDefault(),
                             workingStartLong = db.telemetries.Where(t => t.imei == Convert.ToInt64(imei) && t.digital_inputs > 32950 && t.time > Convert.ToDateTime(DateTime.Now.Date)).Select(t => t.longitude).FirstOrDefault(),
                             workingEndLat = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == Convert.ToInt64(imei) && t.digital_inputs < 32950 && t.time > Convert.ToDateTime(DateTime.Now.Date)).Select(t => t.latitude).FirstOrDefault(),
                             workingEndLong = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == Convert.ToInt64(imei) && t.digital_inputs < 32950 && t.time > Convert.ToDateTime(DateTime.Now.Date)).Select(t => t.longitude).FirstOrDefault(),
                             Maxspeed = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == Convert.ToInt64(imei) && t.time > DateTime.Now.Date).Select(t => t.speed).Max(),
                             AvrageSpeed = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == Convert.ToInt64(imei) && t.speed != 0 && t.time > DateTime.Now.Date).Average(t => t.speed),
                             totoalDistance = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == Convert.ToInt64(imei) && t.digital_inputs != null && t.digital_inputs > 32950 && t.time > Convert.ToDateTime(DateTime.Now.Date)).Select(t => Convert.ToInt64(t.odometer)).Sum(),
                             Motion = db.telemetries.Where(t => t.imei == g.First().imei && t.digital_inputs > 32950 && t.time > DateTime.Now.Date).Select(t => Convert.ToDateTime(t.time).Second).Sum(),
                             Stop = db.telemetries.Where(t => t.imei == g.First().imei && t.digital_inputs < 32950 && t.time > DateTime.Now.Date).Select(t => Convert.ToDateTime(t.time).Second).Sum(),
                             Idel = db.telemetries.Where(t => t.imei == g.First().imei && t.digital_inputs == 32953 && t.time > DateTime.Now.Date).Select(t => Convert.ToDateTime(t.time).Second).Sum(),
                  

                         }).ToList();
            
            LocalReport lc1 = new LocalReport();
            lc1.ReportPath = Server.MapPath("~/Reports/RptBeginEndDay.rdlc");
            ReportDataSource rd1 = new ReportDataSource("ReportBeginEndDay", data1.ToList());
            rd1.Value = data1;
            lc1.DataSources.Add(rd1);
            string reporttype = "Excel";
            string mimetype1;
            string encoding1;
            string filenameextention1;


            filenameextention1 = "xls";
            string[] strems1;
            Warning[] warnings1;
            byte[] renderdbyte1;
            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                , out strems1, out warnings1);
            Response.AddHeader("content-disposition", "attachment;filename=BeginEndDay_Report." + filenameextention1);
            return File(renderdbyte1, "." + filenameextention1);
        }

        //Show log report
        public ActionResult LogReport(String reporttype, String id, String from1, String to, string filter)
        {
            var s = (string)Session["username"];


            if (s == "" || s == null)
            {
                Response.Write("Your session has een expired please login again");
                Response.Redirect("/Login.aspx");

            }
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

            var uId = (from a in db.Tbl_Users where a.username == Request.RequestContext.HttpContext.Session["username"].ToString() select a.UserID).SingleOrDefault();
            if (filter != "-1"&&filter!="cc")
            {
                //this block is execute if filter is != select and custom

                if (filter == "1")
                {
                    //this block execute to retrive todays data

                    try
                    {

                        UserId = Convert.ToInt32(uId);
                        var data1 = (from a in db.telemetries
                                     orderby a.id descending
                                     where a.imei == Convert.ToInt64(id) && a.time > Convert.ToDateTime(DateTime.Now.Date)
                                     select new
                                     {

                                         Date = a.time,
                                         Id = a.id,
                                         Lat = a.latitude,
                                         Long = a.longitude,
                                         DeviceName = db.Tbl_DeviceRegistrations.Where(t => t.DeviceIMEI == a.imei).Select(t => t.DeviceName).SingleOrDefault(),
                                         imei = a.imei,
                                         speed = a.speed,
                                         Odeometer = a.odometer,
                                         hdop = a.hdop,
                                         scount = a.satellites,
                                         gsmStength = a.recieved_signal_strength,
                                         CurrentDate = DateTime.Now.ToShortDateString()
                                     }).ToList();
                        LocalReport lc1 = new LocalReport();
                        lc1.ReportPath = Server.MapPath("~/Reports/RptLogs.rdlc");
                        ReportDataSource rd1 = new ReportDataSource("LogReport", data1.ToList());
                        rd1.Value = data1;
                        lc1.DataSources.Add(rd1);
                        string Reporttype1 = reporttype;
                        string mimetype1;
                        string encoding1;
                        string filenameextention1;
                        //check report type
                        if (reporttype == "Excel")
                        {
                            filenameextention1 = ".xls";

                        }
                        else if (reporttype == "PDF")
                        {
                            filenameextention1 = "pdf";

                        }
                        else if (reporttype == "Word")
                        {
                            filenameextention1 = "docx";

                        }
                        //pass report
                        if (reporttype == "PDF")
                        {
                            
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline:filename=LogReport." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }
                        else
                        {
                            Response.ContentType = "application/ms-excel";

                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "attachment;filename=LogReport." + filenameextention1);
                            return File(renderdbyte1.ToArray(), filenameextention1);

                        }
                    }
                    catch (Exception)
                    {
                        UserId = Convert.ToInt32(uId);
                        var data1 = (from a in db.telemetries
                                     orderby a.id descending
                                     where a.time > DateTime.Now.Date
                                     select new
                                     {

                                         Date = a.time,
                                         Id = a.id,
                                         Lat = a.latitude,
                                         Long = a.longitude,
                                         DeviceName = db.Tbl_DeviceRegistrations.Where(t => t.DeviceIMEI == a.imei).Select(t => t.DeviceName).SingleOrDefault(),
                                         imei = a.imei,
                                         speed = a.speed,
                                         Odeometer = a.odometer,
                                         hdop = a.hdop,
                                         scount = a.satellites,
                                         gsmStength = a.recieved_signal_strength,
                                         CurrentDate = DateTime.Now.ToShortDateString()
                                     }).ToList();
                        LocalReport lc1 = new LocalReport();
                        lc1.ReportPath = Server.MapPath("~/Reports/RptLogs.rdlc");
                        ReportDataSource rd1 = new ReportDataSource("LogReport", data1.ToList());
                        rd1.Value = data1;
                        lc1.DataSources.Add(rd1);
                        string Reporttype1 = reporttype;
                        string mimetype1;
                        string encoding1;
                        string filenameextention1;
                        if (reporttype == "Excel")
                        {
                            filenameextention1 = ".xls";

                        }
                        else if (reporttype == "PDF")
                        {
                            filenameextention1 = "pdf";

                        }
                        else if (reporttype == "Word")
                        {
                            filenameextention1 = "docx";

                        }
                        else if (reporttype == "Image")
                        {
                            filenameextention1 = "jpg";
                        }

                        if (reporttype == "PDF")
                        {
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline:filename=LogReport." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }
                        else
                        {
                            Response.ContentType = "application/ms-excel";

                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "attachment;filename=LogReport." + filenameextention1);
                            return File(renderdbyte1.ToArray(), filenameextention1);

                        }

                    }
                  
                }

                else if (filter == "2")
                {
                    //this block is Execute if filter is yesterday


                    try
                    {

                        UserId = Convert.ToInt32(uId);
                        var data1 = (from a in db.telemetries
                                     orderby a.id descending
                                     where a.imei == Convert.ToInt64(id) && a.time > Convert.ToDateTime(DateTime.Now.AddDays(-1)) && a.time < DateTime.Now.Date
                                     select new
                                     {

                                         Date = a.time,
                                         Id = a.id,
                                         Lat = a.latitude,
                                         Long = a.longitude,
                                         DeviceName = db.Tbl_DeviceRegistrations.Where(t => t.DeviceIMEI == a.imei).Select(t => t.DeviceName).SingleOrDefault(),
                                         imei = a.imei,
                                         speed = a.speed,
                                         Odeometer = a.odometer,
                                         hdop = a.hdop,
                                         scount = a.satellites,
                                         gsmStength = a.recieved_signal_strength,
                                         CurrentDate = DateTime.Now.ToShortDateString()
                                     }).ToList();
                        LocalReport lc1 = new LocalReport();
                        lc1.ReportPath = Server.MapPath("~/Reports/RptLogs.rdlc");
                        ReportDataSource rd1 = new ReportDataSource("LogReport", data1.ToList());
                        rd1.Value = data1;
                        lc1.DataSources.Add(rd1);
                        string Reporttype1 = reporttype;
                        string mimetype1;
                        string encoding1;
                        string filenameextention1;
                        if (reporttype == "Excel")
                        {
                            filenameextention1 = ".xls";

                        }
                        else if (reporttype == "PDF")
                        {
                            filenameextention1 = "pdf";

                        }
                        else if (reporttype == "Word")
                        {
                            filenameextention1 = "docx";

                        }
                        if (reporttype == "PDF")
                        {
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline:filename=LogReport." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }
                        else
                        {
                            Response.ContentType = "application/ms-excel";

                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "attachment;filename=LogReport." + filenameextention1);
                            return File(renderdbyte1.ToArray(), filenameextention1);

                        }
                    }
                    catch (Exception)
                    {


                        UserId = Convert.ToInt32(uId);
                        var data1 = (from a in db.telemetries
                                     orderby a.id descending
                                     where  a.time > Convert.ToDateTime(DateTime.Now.AddDays(-1)) && a.time < DateTime.Now.Date
                                     select new
                                     {

                                         Date = a.time,
                                         Id = a.id,
                                         Lat = a.latitude,
                                         Long = a.longitude,
                                         DeviceName = db.Tbl_DeviceRegistrations.Where(t => t.DeviceIMEI == a.imei).Select(t => t.DeviceName).SingleOrDefault(),
                                         imei = a.imei,
                                         speed = a.speed,
                                         Odeometer = a.odometer,
                                         hdop = a.hdop,
                                         scount = a.satellites,
                                         gsmStength = a.recieved_signal_strength,
                                         CurrentDate = DateTime.Now.ToShortDateString()
                                     }).ToList();
                        LocalReport lc1 = new LocalReport();
                        lc1.ReportPath = Server.MapPath("~/Reports/RptLogs.rdlc");
                        ReportDataSource rd1 = new ReportDataSource("LogReport", data1.ToList());
                        rd1.Value = data1;
                        lc1.DataSources.Add(rd1);
                        string Reporttype1 = reporttype;
                        string mimetype1;
                        string encoding1;
                        string filenameextention1;
                        if (reporttype == "Excel")
                        {
                            filenameextention1 = ".xls";

                        }
                        else if (reporttype == "PDF")
                        {
                            filenameextention1 = "pdf";

                        }
                        else if (reporttype == "Word")
                        {
                            filenameextention1 = "docx";

                        }
                        if (reporttype == "PDF")
                        {
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline:filename=LogReport." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }
                        else
                        {
                            Response.ContentType = "application/ms-excel";

                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "attachment;filename=LogReport." + filenameextention1);
                            return File(renderdbyte1.ToArray(), filenameextention1);

                        }
                    }


                }
                else if (filter == "30")
                {
                    //this block is Execute if filter is last month
                    try
                    {
                       

                        UserId = Convert.ToInt32(uId);
                        var data1 = (from a in db.telemetries
                                     orderby a.id descending
                                     where a.imei == Convert.ToInt64(id) && a.time >= Convert.ToDateTime(firstDayLastMonth) && a.time <= lastDayLastMonth
                                     select new
                                     {

                                         Date = a.time,
                                         Id = a.id,
                                         Lat = a.latitude,
                                         Long = a.longitude,
                                         DeviceName = db.Tbl_DeviceRegistrations.Where(t => t.DeviceIMEI == a.imei).Select(t => t.DeviceName).SingleOrDefault(),
                                         imei = a.imei,
                                         speed = a.speed,
                                         Odeometer = a.odometer,
                                         hdop = a.hdop,
                                         scount = a.satellites,
                                         gsmStength = a.recieved_signal_strength,
                                         CurrentDate = DateTime.Now.ToShortDateString()
                                     }).ToList();
                        LocalReport lc1 = new LocalReport();
                        lc1.ReportPath = Server.MapPath("~/Reports/RptLogs.rdlc");
                        ReportDataSource rd1 = new ReportDataSource("LogReport", data1.ToList());
                        rd1.Value = data1;
                        lc1.DataSources.Add(rd1);
                        string Reporttype1 = reporttype;
                        string mimetype1;
                        string encoding1;
                        string filenameextention1;
                        if (reporttype == "Excel")
                        {
                            filenameextention1 = ".xls";

                        }
                        else if (reporttype == "PDF")
                        {
                            filenameextention1 = "pdf";

                        }
                        else if (reporttype == "Word")
                        {
                            filenameextention1 = "docx";

                        }

                        if (reporttype == "PDF")
                        {
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline:filename=LogReport." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }
                        else
                        {
                            Response.ContentType = "application/ms-excel";

                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "attachment;filename=LogReport." + filenameextention1);
                            return File(renderdbyte1.ToArray(), filenameextention1);

                        }

                    }
                    catch (Exception)
                    {
                      

                        UserId = Convert.ToInt32(uId);
                        var data1 = (from a in db.telemetries
                                     orderby a.id descending
                                     where a.time >= Convert.ToDateTime(firstDayLastMonth) && a.time <=lastDayLastMonth.AddDays(1)
                                     select new
                                     {

                                         Date = a.time,
                                         Id = a.id,
                                         Lat = a.latitude,
                                         Long = a.longitude,
                                         DeviceName = db.Tbl_DeviceRegistrations.Where(t => t.DeviceIMEI == a.imei).Select(t => t.DeviceName).SingleOrDefault(),
                                         imei = a.imei,
                                         speed = a.speed,
                                         Odeometer = a.odometer,
                                         hdop = a.hdop,
                                         scount = a.satellites,
                                         gsmStength = a.recieved_signal_strength,
                                         CurrentDate = DateTime.Now.ToShortDateString()
                                     }).ToList();
                        LocalReport lc1 = new LocalReport();
                        lc1.ReportPath = Server.MapPath("~/Reports/RptLogs.rdlc");
                        ReportDataSource rd1 = new ReportDataSource("LogReport", data1.ToList());
                        rd1.Value = data1;
                        lc1.DataSources.Add(rd1);
                        string Reporttype1 = reporttype;
                        string mimetype1;
                        string encoding1;
                        string filenameextention1;
                        if (reporttype == "Excel")
                        {
                            filenameextention1 = ".xls";

                        }
                        else if (reporttype == "PDF")
                        {
                            filenameextention1 = "pdf";

                        }
                        else if (reporttype == "Word")
                        {
                            filenameextention1 = "docx";

                        }

                        if (reporttype == "PDF")
                        {
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline:filename=LogReport." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }
                        else
                        {
                            Response.ContentType = "application/ms-excel";

                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "attachment;filename=LogReport." + filenameextention1);
                            return File(renderdbyte1.ToArray(), filenameextention1);

                        }

                    }

                }

                else if (filter == "8")
                {
                    //this block is Execute if filter is last week
                    try
                    {
                        

                        UserId = Convert.ToInt32(uId);
                        var data1 = (from a in db.telemetries
                                     orderby a.id descending
                                     where a.imei == Convert.ToInt64(id) && a.time > Convert.ToDateTime(previousWeekStart) && a.time < previousWeekEnd
                                     select new
                                     {

                                         Date = a.time,
                                         Id = a.id,
                                         Lat = a.latitude,
                                         Long = a.longitude,
                                         DeviceName = db.Tbl_DeviceRegistrations.Where(t => t.DeviceIMEI == a.imei).Select(t => t.DeviceName).SingleOrDefault(),
                                         imei = a.imei,
                                         speed = a.speed,
                                         Odeometer = a.odometer,
                                         hdop = a.hdop,
                                         scount = a.satellites,
                                         gsmStength = a.recieved_signal_strength,
                                         CurrentDate = DateTime.Now.ToShortDateString()
                                     }).ToList();
                        LocalReport lc1 = new LocalReport();
                        lc1.ReportPath = Server.MapPath("~/Reports/RptLogs.rdlc");
                        ReportDataSource rd1 = new ReportDataSource("LogReport", data1.ToList());
                        rd1.Value = data1;
                        lc1.DataSources.Add(rd1);
                        string Reporttype1 = reporttype;
                        string mimetype1;
                        string encoding1;
                        string filenameextention1;
                        if (reporttype == "Excel")
                        {
                            filenameextention1 = ".xls";

                        }
                        else if (reporttype == "PDF")
                        {
                            filenameextention1 = "pdf";

                        }
                        else if (reporttype == "Word")
                        {
                            filenameextention1 = "docx";

                        }
                        if (reporttype == "PDF")
                        {
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline:filename=LogReport." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }
                        else
                        {
                            Response.ContentType = "application/ms-excel";

                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "attachment;filename=LogReport." + filenameextention1);
                            return File(renderdbyte1.ToArray(), filenameextention1);

                        }

                    }
                    catch (Exception)
                    {
                       

                        UserId = Convert.ToInt32(uId);
                        var data1 = (from a in db.telemetries
                                     orderby a.id descending
                                     where a.time > Convert.ToDateTime(previousWeekStart) && a.time <previousWeekEnd
                                     select new
                                     {

                                         Date = a.time,
                                         Id = a.id,
                                         Lat = a.latitude,
                                         Long = a.longitude,
                                         DeviceName = db.Tbl_DeviceRegistrations.Where(t => t.DeviceIMEI == a.imei).Select(t => t.DeviceName).SingleOrDefault(),
                                         imei = a.imei,
                                         speed = a.speed,
                                         Odeometer = a.odometer,
                                         hdop = a.hdop,
                                         scount = a.satellites,
                                         gsmStength = a.recieved_signal_strength,
                                         CurrentDate = DateTime.Now.ToShortDateString()
                                     }).ToList();
                        LocalReport lc1 = new LocalReport();
                        lc1.ReportPath = Server.MapPath("~/Reports/RptLogs.rdlc");
                        ReportDataSource rd1 = new ReportDataSource("LogReport", data1.ToList());
                        rd1.Value = data1;
                        lc1.DataSources.Add(rd1);
                        string Reporttype1 = reporttype;
                        string mimetype1;
                        string encoding1;
                        string filenameextention1;
                        if (reporttype == "Excel")
                        {
                            filenameextention1 = ".xls";

                        }
                        else if (reporttype == "PDF")
                        {
                            filenameextention1 = "pdf";

                        }
                        else if (reporttype == "Word")
                        {
                            filenameextention1 = "docx";

                        }
                        if (reporttype == "PDF")
                        {
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline:filename=LogReport." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }
                        else
                        {
                            Response.ContentType = "application/ms-excel";

                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "attachment;filename=LogReport." + filenameextention1);
                            return File(renderdbyte1.ToArray(), filenameextention1);

                        }

                    }
                }
            }



            try
            {

                uId = (from a in db.Tbl_Users where a.username == Request.RequestContext.HttpContext.Session["username"].ToString() select a.UserID).SingleOrDefault();
                UserId = Convert.ToInt32(uId);
                var data = (from a in db.telemetries
                            orderby a.id descending
                            where a.imei == Convert.ToInt64(id) && a.time >= Convert.ToDateTime(from1) && a.time <= Convert.ToDateTime(to).AddDays(1)
                            select new
                            {

                                Date = a.time,
                                Id = a.id,
                                Lat = a.latitude,
                                Long = a.longitude,
                                DeviceName = db.Tbl_DeviceRegistrations.Where(t => t.DeviceIMEI == a.imei).Select(t => t.DeviceName).SingleOrDefault(),
                                imei = a.imei,
                                speed = a.speed,
                                Odeometer = a.odometer,
                                hdop = a.hdop,
                                scount = a.satellites,
                                gsmStength = a.recieved_signal_strength,
                                CurrentDate = DateTime.Now.ToShortDateString()
                            }).ToList();
               LocalReport lc1 = new LocalReport();
                        lc1.ReportPath = Server.MapPath("~/Reports/RptLogs.rdlc");
                        ReportDataSource rd1 = new ReportDataSource("LogReport", data.ToList());
                        rd1.Value = data;
                        lc1.DataSources.Add(rd1);
                        string Reporttype1 = reporttype;
                        string mimetype1;
                        string encoding1;
                        string filenameextention1;
                        if (reporttype == "Excel")
                        {
                            filenameextention1 = ".xls";

                        }
                        else if (reporttype == "PDF")
                        {
                            filenameextention1 = "pdf";

                        }
                        else if (reporttype == "Word")
                        {
                            filenameextention1 = "docx";

                        }
                        else if (reporttype == "Image")
                        {
                            filenameextention1 = "jpg";
                        }
                        if (reporttype == "PDF")
                        {
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "inline:filename=LogReport." + filenameextention1);
                            return File(renderdbyte1, "." + filenameextention1);
                        }
                        else
                        {
                            Response.ContentType = "application/ms-excel";
                            string[] strems1;
                            Warning[] warnings1;
                            byte[] renderdbyte1;
                            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                                , out strems1, out warnings1);
                            Response.AddHeader("content-disposition", "attachment;filename=LogReport." + filenameextention1);
                            return File(renderdbyte1.ToArray(), filenameextention1);

                        }
                

            }
            catch (Exception)
            {


                uId = (from a in db.Tbl_Users where a.username == Request.RequestContext.HttpContext.Session["username"].ToString() select a.UserID).SingleOrDefault();
                UserId = Convert.ToInt32(uId);
                var data = (from a in db.telemetries
                            orderby a.id descending
                            where a.time >= Convert.ToDateTime(from1) && a.time < Convert.ToDateTime(to).AddDays(1)
                            select new
                            {

                                Date = a.time,
                                Id = a.id,
                                Lat = a.latitude,
                                Long = a.longitude,
                                DeviceName = db.Tbl_DeviceRegistrations.Where(t => t.DeviceIMEI == a.imei).Select(t => t.DeviceName).SingleOrDefault(),
                                imei = a.imei,
                                speed = a.speed,
                                Odeometer = a.odometer,
                                hdop = a.hdop,
                                scount = a.satellites,
                                gsmStength = a.recieved_signal_strength,
                                CurrentDate = DateTime.Now.ToShortDateString()
                            }).ToList();
                LocalReport lc1 = new LocalReport();
                lc1.ReportPath = Server.MapPath("~/Reports/RptLogs.rdlc");
                ReportDataSource rd1 = new ReportDataSource("LogReport", data.ToList());
                rd1.Value = data;
                lc1.DataSources.Add(rd1);
                string Reporttype1 = reporttype;
                string mimetype1;
                string encoding1;
                string filenameextention1;
                if (reporttype == "Excel")
                {
                    filenameextention1 = ".xls";

                }
                else if (reporttype == "PDF")
                {
                    filenameextention1 = "pdf";

                }
                else if (reporttype == "Word")
                {
                    filenameextention1 = "docx";

                }
                else if (reporttype == "Image")
                {
                    filenameextention1 = "jpg";
                }
                if (reporttype == "PDF")
                {
                    string[] strems1;
                    Warning[] warnings1;
                    byte[] renderdbyte1;
                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                        , out strems1, out warnings1);
                    Response.AddHeader("content-disposition", "inline:filename=LogReport." + filenameextention1);
                    return File(renderdbyte1, "." + filenameextention1);
                }
                else
                {
                    Response.ContentType = "application/ms-excel";
                    string[] strems1;
                    Warning[] warnings1;
                    byte[] renderdbyte1;
                    renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                        , out strems1, out warnings1);
                    Response.AddHeader("content-disposition", "attachment;filename=LogReport." + filenameextention1);
                    return File(renderdbyte1.ToArray(), filenameextention1);

                }
            }

        }

        //for over speed report pdf
        public ActionResult OverSpeedReportPdf(String vno, String from1, String to)
        {
            //this method is execute for overspeed report if report typemis pdf
            var s = (string)Session["username"];


            if (s == "" || s == null)
            {
                Response.Write("Your session has een expired please login again");
                Response.Redirect("/Login.aspx");

            }
            var imei1 = (from a in db.Tbl_DeviceRegistrations
                         where a.DeviceName ==vno.Trim()
                         select a.DeviceIMEI).SingleOrDefault();
            //UserId = Convert.ToInt32(uId);
            var data1 = (from a in db.telemetries
                                     orderby a.id descending
                                     where a.speed > 45 && a.imei == Convert.ToInt64(imei1) && a.time > Convert.ToDateTime(to) &&a.time<=Convert.ToDateTime(from1)
                                     select new
                                     {

                                         Date = a.time,
                                         Id = a.id,
                                         Lat = a.latitude,
                                         Long = a.longitude,
                                         DeviceName = db.Tbl_DeviceRegistrations.Where(t => t.DeviceIMEI == a.imei).Select(t => t.DeviceName).SingleOrDefault(),
                                         //imei = a.imei,
                                         speed = a.speed,
                                         //Odeometer = a.odometer,
                                         //hdop = a.hdop,
                                         //scount = a.satellites,
                                         //gsmStength = a.recieved_signal_strength,
                                         CurrentDate = DateTime.Now.ToShortDateString()
                                     }).ToList();
                        LocalReport lc1 = new LocalReport();
                        lc1.ReportPath = Server.MapPath("~/Reports/RptOverSpeedLog.rdlc");
                        ReportDataSource rd1 = new ReportDataSource("OverSpeed", data1.ToList());
                        rd1.Value = data1;
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
            Response.AddHeader("content-disposition", "attachment;filename=SpeedViolation_Report." + filenameextention1);
            return File(renderdbyte1, "." + filenameextention1);
                       
        }
        //for over speed report excel
        public ActionResult OverSpeedReportExcel(String vno, String from1, String to)
        {
            //this method is execute for overspeed report if report typemis execl
            var s = (string)Session["username"];


            if (s == "" || s == null)
            {
                Response.Write("Your session has een expired please login again");
                Response.Redirect("/Login.aspx");

            }
            var imei1 = (from a in db.Tbl_DeviceRegistrations
                         where a.DeviceName == vno.Trim()
                         select a.DeviceIMEI).SingleOrDefault();
            //UserId = Convert.ToInt32(uId);
            var data1 = (from a in db.telemetries
                         orderby a.id descending
                         where a.speed > 45 && a.imei == Convert.ToInt64(imei1) && a.time > Convert.ToDateTime(to) && a.time <= Convert.ToDateTime(from1)
                         select new
                         {

                             Date = a.time,
                             Id = a.id,
                             Lat = a.latitude,
                             Long = a.longitude,
                             DeviceName = db.Tbl_DeviceRegistrations.Where(t => t.DeviceIMEI == a.imei).Select(t => t.DeviceName).SingleOrDefault(),
                             //imei = a.imei,
                             speed = a.speed,
                             //Odeometer = a.odometer,
                             //hdop = a.hdop,
                             //scount = a.satellites,
                             //gsmStength = a.recieved_signal_strength,
                             CurrentDate = DateTime.Now.ToShortDateString()
                         }).ToList();
            LocalReport lc1 = new LocalReport();
            lc1.ReportPath = Server.MapPath("~/Reports/RptOverSpeedLog.rdlc");
            ReportDataSource rd1 = new ReportDataSource("OverSpeed", data1.ToList());
            rd1.Value = data1;
            lc1.DataSources.Add(rd1);
            string reporttype = "Excel";
            string mimetype1;
            string encoding1;
            string filenameextention1;


            filenameextention1 = "xls";


            Response.ContentType = "application/ms-excel";

            string[] strems1;
            Warning[] warnings1;
            byte[] renderdbyte1;
            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                , out strems1, out warnings1);
            Response.AddHeader("content-disposition", "attachment;filename=SpeedViolation_Report." + filenameextention1);
            return File(renderdbyte1.ToArray(), filenameextention1);

        }

        public JsonResult GetFuel()
        {

            var data = (from a in db.Tbl_Logs select a).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);

        }
        //Device Registration
        public JsonResult DeviceRegistration(product product)
        {
            var uId1 = (from a in db.Tbl_Users where a.username == Request.RequestContext.HttpContext.Session["username"].ToString() select a.UserID).SingleOrDefault();
            if (uId1.ToString() == "")
            {
                Response.Redirect("/Login.aspx");
            }
            else
            {
                UserId = Convert.ToInt32(uId1);
                var Imei = (from a in db.Tbl_DeviceRegistrations where a.DeviceIMEI == Convert.ToInt64(product.IMEI) select a).SingleOrDefault();
                var DeviceName = (from a in db.Tbl_DeviceRegistrations where a.DeviceName == product.DeviceName.ToString() && a.UserId == UserId select a).SingleOrDefault();
                if (Imei != null)
                {
                    string result = "Device_Alreday_Registerd";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else if (DeviceName != null)
                {
                    string result = "Device_Name_Alreday_Exist";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var uId = (from a in db.Tbl_Users where a.username == Request.RequestContext.HttpContext.Session["username"].ToString() select a.UserID).SingleOrDefault();
                    UserId = Convert.ToInt32(uId);
                    Tbl_DeviceRegistration t = new Tbl_DeviceRegistration();
                    t.UserId = UserId;
                    t.DeviceIMEI = Convert.ToInt64(product.IMEI);
                    t.DeviceName = product.DeviceName.ToString();
                    t.Vehicle_Type= product.Vehicle_Type.ToString().Trim().ToUpper();
                    db.Tbl_DeviceRegistrations.InsertOnSubmit(t);
                    db.SubmitChanges();
                    return Json(product, JsonRequestBehavior.AllowGet);

                }
              
            }
            return Json(product, JsonRequestBehavior.AllowGet);
        }



        //}

            //for speed voilation report table log
        public JsonResult SpeedLog(product product)
        {


            var s = (string)Session["username"];


            if (s == "" || s == null)
            {
                Response.Write("Your session has een expired please login again");
                Response.Redirect("/Login.aspx");

            }
            var imei1 = (from a in db.Tbl_DeviceRegistrations
                        where a.DeviceName==product.Dropdown.Trim()
                        select a.DeviceIMEI).SingleOrDefault();
            var data = (from b in db.telemetries
                         orderby b.id descending
                         where b.imei == Convert.ToInt64(imei1) && b.time > Convert.ToDateTime(product.To).AddDays(-1) && b.time <= Convert.ToDateTime(product.From) && b.speed > 45
                         //var data = (from b in db.telemetries
                         //            where b.imei ==Convert.ToInt64(imei1) && b.time >Convert.ToDateTime( product.To) && b.time < Convert.ToDateTime(product.From)&&b.speed>45
                         select new
                        {
                            b.id,
                           Lat= b.latitude,
                           Long=  b.longitude,
                            Date = b.time,
                           speed = b.speed,
                            DeviceName=db.Tbl_DeviceRegistrations.Where(t => t.DeviceIMEI == b.imei).Select(t => t.DeviceName).SingleOrDefault()

                        }).ToList();
           
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AlertLog(product product)
        {
            var s = (string)Session["username"];


            if (s == "" || s == null)
            {
                Response.Write("Your session has een expired please login again");
                Response.Redirect("/Login.aspx");

            }
            var imei1 = (from a in db.Tbl_DeviceRegistrations
                         where a.DeviceName == product.Dropdown.Trim()
                         select a.DeviceIMEI).SingleOrDefault();
            var data = (from b in db.telemetries
                        orderby b.id descending
                        where b.imei == Convert.ToInt64(imei1) && b.time > Convert.ToDateTime(product.To).AddDays(-1) && b.time <= Convert.ToDateTime(product.From) && b.speed > 45
                        group b by b.imei into g
                        //var data = (from b in db.telemetries
                        //            where b.imei ==Convert.ToInt64(imei1) && b.time >Convert.ToDateTime( product.To) && b.time < Convert.ToDateTime(product.From)&&b.speed>45
                        select new
                        {
                            g.First().id,
                            //Lat = b.latitude,
                            //Long = b.longitude,
                            //Date = b.time,
                            speedCount = db.telemetries.Where(t => t.imei == g.First().imei && t.speed > 45 && t.time > Convert.ToDateTime(product.To) && t.time <= Convert.ToDateTime(product.From)).Select(t => Convert.ToInt64(t.speed)).Count(),
                            //speedCount = db.telemetries.Where(t=>t.imei== g.First().imei&& g.First().time > Convert.ToDateTime(product.To).AddDays(-1) && g.First().time <= Convert.ToDateTime(product.From) && g.First().speed > 45).Select(t=>t.id).Count(),
                            Stoppage = db.telemetries.Where(t => t.imei == g.First().imei && t.digital_inputs > 32936&& t.digital_inputs < 32950 && t.time > Convert.ToDateTime(product.To) && t.time <= Convert.ToDateTime(product.From)).Select(t => Convert.ToInt64(t.speed)).Count(),
                            DeviceName = db.Tbl_DeviceRegistrations.Where(t => t.DeviceIMEI == g.First().imei).Select(t => t.DeviceName).SingleOrDefault()

                        }).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FleetLog(product product)
        {
            var s = (string)Session["username"];


            if (s == "" || s == null)
            {
                Response.Write("Your session has een expired please login again");
                Response.Redirect("/Login.aspx");

            }
            //db.CommandTimeout = 240;
            try
            {
                var imei1 = (from a in db.Tbl_DeviceRegistrations
                             where a.DeviceName == product.Dropdown.Trim()
                             select a.DeviceIMEI).SingleOrDefault();
              
                    if (imei1!=0)
                    {
                        var data = (from b in db.telemetries
                                    where b.imei == Convert.ToInt64(imei1) && b.time > Convert.ToDateTime(product.To) && b.time <= Convert.ToDateTime(product.From)
                                
                                    group b by b.imei into g
                                    select new
                                    {
                                        id = g.First().id,
                                        DeviceNo =db.Tbl_DeviceRegistrations.Where(t=>t.DeviceIMEI==g.First().imei).Select(t=>t.DeviceName).SingleOrDefault(),
                                        //Long = g.First().longitude,
                                        //Date = b.time,
                                        //speed = b.speed,
                                        workingHrStart = db.telemetries.Where(t => t.imei == g.First().imei && t.digital_inputs > 32950 && t.time > Convert.ToDateTime(product.To) && t.time <= Convert.ToDateTime(product.From)).Select(t => t.time).FirstOrDefault(),
                                        workingHrEnd = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == g.First().imei && t.digital_inputs > 32950 && t.time > Convert.ToDateTime(product.To) && t.time <= Convert.ToDateTime(product.From)).Select(t => t.time).FirstOrDefault(),
                                        TotalWorkingHr = db.telemetries.Where(t => t.imei == g.First().imei && t.time > Convert.ToDateTime(product.To) && t.time <= Convert.ToDateTime(product.From)).Select(t => Convert.ToDateTime(t.time).Second).Sum(),
                                        totoalDistance = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == g.First().imei && t.digital_inputs > 32950 && t.time > Convert.ToDateTime(product.To) && t.time <= Convert.ToDateTime(product.From)).Select(t => Convert.ToInt64(t.odometer)).Sum(),
                                        EngineRunningTime = db.telemetries.Where(t => t.imei == g.First().imei && t.digital_inputs > 32950 && t.time > Convert.ToDateTime(product.To) && t.time <= Convert.ToDateTime(product.From)).Select(t => Convert.ToDateTime(t.time).Second).Sum(),
                                        SpeedVoilation = db.telemetries.Where(t => t.imei == g.First().imei && t.speed > 45 && t.time > Convert.ToDateTime(product.To) && t.time <= Convert.ToDateTime(product.From)).Select(t => Convert.ToInt64(t.speed)).Count(),
                                        stetus = db.telemetries.Where(t => t.imei == g.First().imei && t.time == DateTime.Now).Select(t => t.latitude).SingleOrDefault(),
                            

                                    }).ToList();
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {

                    var data = (from b in db.telemetries
                                where b.imei == Convert.ToInt64(product.Dropdown) && b.time > Convert.ToDateTime(product.To) && b.time <= Convert.ToDateTime(product.From)
                           
                                group b by b.imei into g
                                select new
                                {
                                    id = g.First().id,
                                    DeviceNo = db.Tbl_DeviceRegistrations.Where(t => t.DeviceIMEI == g.First().imei).Select(t => t.DeviceName).SingleOrDefault(),
                                   
                                    workingHrStart = db.telemetries.Where(t => t.imei == g.First().imei && t.digital_inputs > 32950 && t.time > Convert.ToDateTime(product.To) && t.time <= Convert.ToDateTime(product.From)).Select(t => t.time).FirstOrDefault(),
                                    workingHrEnd = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == g.First().imei && t.digital_inputs > 32950 && t.time > Convert.ToDateTime(product.To) && t.time <= Convert.ToDateTime(product.From)).Select(t => t.time).FirstOrDefault(),
                                    TotalWorkingHr = db.telemetries.Where(t => t.imei == g.First().imei && t.time > Convert.ToDateTime(product.To) && t.time <= Convert.ToDateTime(product.From)).Select(t => Convert.ToDateTime(t.time).Second).Sum(),
                                    totoalDistance = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == g.First().imei &&t.odometer!=null&& t.digital_inputs > 32950 && t.time > Convert.ToDateTime(product.To) && t.time <= Convert.ToDateTime(product.From)).Select(t => Convert.ToInt64(t.odometer)).Sum(),
                                    EngineRunningTime = db.telemetries.Where(t => t.imei == g.First().imei && t.digital_inputs > 32950 && t.time > Convert.ToDateTime(product.To) && t.time <= Convert.ToDateTime(product.From)).Select(t => Convert.ToDateTime(t.time).Second).Sum(),
                                    SpeedVoilation = db.telemetries.Where(t => t.imei == g.First().imei&&t.speed!=null && t.speed > 45 && t.time > Convert.ToDateTime(product.To) && t.time <= Convert.ToDateTime(product.From)).Select(t => Convert.ToInt64(t.speed)).Count(),
                                    stetus = db.telemetries.Where(t => t.imei == g.First().imei && t.time == DateTime.Now).Select(t => t.latitude).SingleOrDefault(),
                                  

                                }).ToList();
                    return Json(data, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ex)
            {
                return Json(null, JsonRequestBehavior.AllowGet);

            }
           
        }
        public JsonResult DistanceLog(product product)
        {
            var s = (string)Session["username"];


            if (s == "" || s == null)
            {
                Response.Write("Your session has een expired please login again");
                Response.Redirect("/Login.aspx");

            }
            db.CommandTimeout = 180;
            try
            {
                var imei1 = (from a in db.Tbl_DeviceRegistrations
                             where a.DeviceName == product.Dropdown.Trim()
                             select a.DeviceIMEI).SingleOrDefault();

                var data = (from b in db.telemetries
                            where b.imei == Convert.ToInt64(imei1)&&b.odometer!=null&&b.digital_inputs >32950 && b.time > Convert.ToDateTime(product.To) && b.time <= Convert.ToDateTime(product.From)
                            group b by Convert.ToDateTime(b.time).Date into g
                            select new
                            {
                                id = g.First().id,
                              
                                totoalDistance =db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == g.First().imei && t.digital_inputs > 32950 &&Convert.ToDateTime(t.time).Date ==Convert.ToDateTime(g.First().time).Date).Select(t => Convert.ToInt64(t.odometer)).Sum(),
                                DeviceName = db.Tbl_DeviceRegistrations.Where(t => t.DeviceIMEI == g.First().imei).Select(t => t.DeviceName).SingleOrDefault(),
                                date =Convert.ToDateTime(g.First().time).Date,
                                CurrentDate = DateTime.Now

                            }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            }
        public ActionResult RportDistanceTravelledPDF(String vno, String from1, String to)
        {

            var s = (string)Session["username"];


            if (s == "" || s == null)
            {
                Response.Write("Your session has een expired please login again");
                Response.Redirect("/Login.aspx");

            }
            db.CommandTimeout = 180;
           
                var imei1 = (from a in db.Tbl_DeviceRegistrations
                             where a.DeviceName == vno.Trim()
                             select a.DeviceIMEI).SingleOrDefault();

                var data = (from b in db.telemetries
                            where b.imei == Convert.ToInt64(imei1) && b.odometer != null && b.digital_inputs > 32950 && b.time > Convert.ToDateTime(to) && b.time <= Convert.ToDateTime(from1)
                            group b by Convert.ToDateTime(b.time).Date into g
                            select new
                            {
                                id = g.First().id,
                                totoalDistance = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == g.First().imei && t.digital_inputs > 32950 && Convert.ToDateTime(t.time).Date == Convert.ToDateTime(g.First().time).Date).Select(t => Convert.ToInt64(t.odometer)).Sum()/1000,
                                DeviceName = db.Tbl_DeviceRegistrations.Where(t => t.DeviceIMEI == g.First().imei).Select(t => t.DeviceName).SingleOrDefault(),
                                date = Convert.ToDateTime(g.First().time).Date,
                                CurrentDate = DateTime.Now

                            }).ToList();
               
             LocalReport lc1 = new LocalReport();
            lc1.ReportPath = Server.MapPath("~/Reports/RptDistanceTravelled.rdlc");
            ReportDataSource rd1 = new ReportDataSource("ReportDistanceTravelled", data.ToList());
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
            Response.AddHeader("content-disposition", "attachment;filename=DistanceTravelled_Report." + filenameextention1);
            return File(renderdbyte1, "." + filenameextention1);
        }
        public ActionResult RportDistanceTravelledExcel(String vno, String from1, String to)
        {

            var s = (string)Session["username"];


            if (s == "" || s == null)
            {
                Response.Write("Your session has een expired please login again");
                Response.Redirect("/Login.aspx");

            }
            db.CommandTimeout = 180;

            var imei1 = (from a in db.Tbl_DeviceRegistrations
                         where a.DeviceName == vno.Trim()
                         select a.DeviceIMEI).SingleOrDefault();

            var data = (from b in db.telemetries
                        where b.imei == Convert.ToInt64(imei1) && b.odometer != null && b.digital_inputs > 32950 && b.time > Convert.ToDateTime(to) && b.time <= Convert.ToDateTime(from1)
                        group b by Convert.ToDateTime(b.time).Date into g
                        select new
                        {
                            id = g.First().id,
                            totoalDistance = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == g.First().imei && t.digital_inputs > 32950 && Convert.ToDateTime(t.time).Date == Convert.ToDateTime(g.First().time).Date).Select(t => Convert.ToInt64(t.odometer)).Sum() / 1000,
                            DeviceName = db.Tbl_DeviceRegistrations.Where(t => t.DeviceIMEI == g.First().imei).Select(t => t.DeviceName).SingleOrDefault(),
                            date = Convert.ToDateTime(g.First().time).Date,
                            CurrentDate = DateTime.Now

                        }).ToList();

            LocalReport lc1 = new LocalReport();
            lc1.ReportPath = Server.MapPath("~/Reports/RptDistanceTravelled.rdlc");
            ReportDataSource rd1 = new ReportDataSource("ReportDistanceTravelled", data.ToList());
            rd1.Value = data;
            lc1.DataSources.Add(rd1);
            string reporttype = "Excel";
            string mimetype1;
            string encoding1;
            string filenameextention1;
            filenameextention1 = "xls";
            Response.ContentType = "application/ms-excel";
            string[] strems1;
            Warning[] warnings1;
            byte[] renderdbyte1;
            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                , out strems1, out warnings1);
            Response.AddHeader("content-disposition", "attachment;filename=DistanceTravelled_Report." + filenameextention1);
            return File(renderdbyte1.ToArray(), filenameextention1);
        }
        public ActionResult RportFleetLogPDF(String vno, String from1, String to)
        {

            var s = (string)Session["username"];


            if (s == "" || s == null)
            {
                Response.Write("Your session has een expired please login again");
                Response.Redirect("/Login.aspx");

            }
            //db.CommandTimeout = 240;
            var imei1 = (from a in db.Tbl_DeviceRegistrations
                         where a.DeviceName == vno.Trim()
                         select a.DeviceIMEI).SingleOrDefault();

            try
            {
                if (imei1 != 0)
                {
                    var data = (from b in db.telemetries
                                where b.imei == Convert.ToInt64(imei1) && b.time > Convert.ToDateTime(to) && b.time <= Convert.ToDateTime(from1)
                           
                                group b by b.imei into g
                                select new
                                {
                                    Vehicle_no =db.Tbl_DeviceRegistrations.Where(t => t.DeviceIMEI == g.First().imei).Select(t => t.DeviceName).SingleOrDefault(),
                                    id = g.First().id,
                                    imei = g.First().imei,
                                  
                                    workingHrStart = db.telemetries.Where(t => t.imei == g.First().imei && t.digital_inputs > 32950 && t.time > Convert.ToDateTime(to) && t.time <= Convert.ToDateTime(from1)).Select(t => t.time).FirstOrDefault(),
                                    workingHrEnd = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == g.First().imei && t.digital_inputs > 32950 && t.time > Convert.ToDateTime(to) && t.time <= Convert.ToDateTime(from1)).Select(t => t.time).FirstOrDefault(),
                                    TotalWorkingHr = db.telemetries.Where(t => t.imei == g.First().imei && t.time > Convert.ToDateTime(to) && t.time <= Convert.ToDateTime(from1)).Select(t => Convert.ToDateTime(t.time).Second).Sum(),
                                    totoalDistance = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == g.First().imei && t.digital_inputs > 32950 && t.time > Convert.ToDateTime(to) && t.time <= Convert.ToDateTime(from1)).Select(t => Convert.ToInt64(t.odometer)).Sum(),
                                    EngineRunningTime = db.telemetries.Where(t => t.imei == g.First().imei && t.digital_inputs > 32950 && t.time > Convert.ToDateTime(to) && t.time <= Convert.ToDateTime(from1)).Select(t => Convert.ToDateTime(t.time).Second).Sum(),
                                    SpeedVoilation = db.telemetries.Where(t => t.imei == g.First().imei && t.speed > 45 && t.time > Convert.ToDateTime(to) && t.time <= Convert.ToDateTime(from1)).Select(t => Convert.ToInt64(t.speed)).Count(),
                            
                                    CurrentDate = DateTime.Now
                                  



                                }).ToList();
                    LocalReport lc1 = new LocalReport();
                    lc1.ReportPath = Server.MapPath("/Reports/RptFleetAnyalisis.rdlc");
                    ReportDataSource rd1 = new ReportDataSource("FleetAnyalisisReport", data.ToList());
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
                    Response.AddHeader("content-disposition", "attachment;filename=FleetAnalysis_Report." + filenameextention1);
                    return File(renderdbyte1, "." + filenameextention1);
                }
                else
                {
                    var data = (from b in db.telemetries
                                where b.imei == Convert.ToInt64(vno) && b.time > Convert.ToDateTime(to) && b.time <= Convert.ToDateTime(from1)
                              
                                group b by b.imei into g
                                select new
                                {
                                    id = g.First().id,
                                    imei = g.First().imei,
                                    Vehicle_no = db.Tbl_DeviceRegistrations.Where(t => t.DeviceIMEI == g.First().imei).Select(t => t.DeviceName).SingleOrDefault(),
                                    
                                    workingHrStart = db.telemetries.Where(t => t.imei == g.First().imei && t.digital_inputs > 32950 && t.time > Convert.ToDateTime(to) && t.time <= Convert.ToDateTime(from1)).Select(t => t.time).FirstOrDefault(),
                                    workingHrEnd = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == g.First().imei && t.digital_inputs > 32950 && t.time > Convert.ToDateTime(to) && t.time <= Convert.ToDateTime(from1)).Select(t => t.time).FirstOrDefault(),
                                    TotalWorkingHr =(Convert.ToInt32( db.telemetries.Where(t => t.imei == g.First().imei && t.time > Convert.ToDateTime(to) && t.time <= Convert.ToDateTime(from1)).Select(t => Convert.ToDateTime(t.time).Second).Sum())/3600),
                                    totoalDistance = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == g.First().imei && t.digital_inputs > 32950 && t.time > Convert.ToDateTime(to) && t.time <= Convert.ToDateTime(from1)).Select(t => Convert.ToInt64(t.odometer)).Sum(),
                                    EngineRunningTime = db.telemetries.Where(t => t.imei == g.First().imei && t.digital_inputs > 32950 && t.time > Convert.ToDateTime(to) && t.time <= Convert.ToDateTime(from1)).Select(t => Convert.ToDateTime(t.time).Second).Sum(),
                                    SpeedVoilation = db.telemetries.Where(t => t.imei == g.First().imei && t.speed > 45 && t.time > Convert.ToDateTime(to) && t.time <= Convert.ToDateTime(from1)).Select(t => Convert.ToInt64(t.speed)).Count(),
                                
                                }).ToList();
                    LocalReport lc1 = new LocalReport();
                    lc1.ReportPath = Server.MapPath("/Reports/RptFleetAnyalisis.rdlc");
                    ReportDataSource rd1 = new ReportDataSource("FleetAnyalisisReport", data.ToList());
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
                    Response.AddHeader("content-disposition", "attachment;filename=FleetAnalysis_Report." + filenameextention1);
                    return File(renderdbyte1, "." + filenameextention1);
                }
            }
            catch (Exception ex)
            {
              
                throw(ex);
            }
        }
        public ActionResult RportFleetLogExcel(String vno, String from1, String to)
        {

            var s = (string)Session["username"];


            if (s == "" || s == null)
            {
                Response.Write("Your session has een expired please login again");
                Response.Redirect("/Login.aspx");

            }
            //db.CommandTimeout = 240;
            var imei1 = (from a in db.Tbl_DeviceRegistrations
                         where a.DeviceName == vno.Trim()
                         select a.DeviceIMEI).SingleOrDefault();

            if (imei1 != 0)
            {
                var data = (from b in db.telemetries
                            where b.imei == Convert.ToInt64(imei1) && b.time > Convert.ToDateTime(to) && b.time <= Convert.ToDateTime(from1)
                          
                            group b by b.imei into g
                            select new
                            {
                                id = g.First().id,
                                imei = g.First().imei,
                                Vehicle_no = db.Tbl_DeviceRegistrations.Where(t => t.DeviceIMEI == g.First().imei).Select(t => t.DeviceName).SingleOrDefault(),
                               
                                workingHrStart = db.telemetries.Where(t => t.imei == g.First().imei && t.digital_inputs > 32950 && t.time > Convert.ToDateTime(to) && t.time <= Convert.ToDateTime(from1)).Select(t => t.time).FirstOrDefault(),
                                workingHrEnd = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == g.First().imei && t.digital_inputs > 32950 && t.time > Convert.ToDateTime(to) && t.time <= Convert.ToDateTime(from1)).Select(t => t.time).FirstOrDefault(),
                                TotalWorkingHr = db.telemetries.Where(t => t.imei == g.First().imei && t.time > Convert.ToDateTime(to) && t.time <= Convert.ToDateTime(from1)).Select(t => Convert.ToDateTime(t.time).Second).Sum(),
                                totoalDistance = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == g.First().imei && t.digital_inputs > 32950 && t.time > Convert.ToDateTime(to) && t.time <= Convert.ToDateTime(from1)).Select(t => Convert.ToInt64(t.odometer)).Sum(),
                                EngineRunningTime = db.telemetries.Where(t => t.imei == g.First().imei && t.digital_inputs > 32950 && t.time > Convert.ToDateTime(to) && t.time <= Convert.ToDateTime(from1)).Select(t => Convert.ToDateTime(t.time).Second).Sum(),
                                SpeedVoilation = db.telemetries.Where(t => t.imei == g.First().imei && t.speed > 45 && t.time > Convert.ToDateTime(to) && t.time <= Convert.ToDateTime(from1)).Select(t => Convert.ToInt64(t.speed)).Count(),
                              



                            }).ToList();
                LocalReport lc1 = new LocalReport();
                lc1.ReportPath = Server.MapPath("/Reports/RptFleetAnyalisis.rdlc");
                ReportDataSource rd1 = new ReportDataSource("FleetAnyalisisReport", data.ToList());
                rd1.Value = data;
                lc1.DataSources.Add(rd1);
                string reporttype = "Excel";
                string mimetype1;
                string encoding1;
                string filenameextention1;


                filenameextention1 = "xls";


                Response.ContentType = "application/ms-excel";

                string[] strems1;
                Warning[] warnings1;
                byte[] renderdbyte1;
                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                    , out strems1, out warnings1);
                Response.AddHeader("content-disposition", "attachment;filename=FleetAnalysis_Report." + filenameextention1);
                return File(renderdbyte1.ToArray(), filenameextention1);
            }
            else
            {
                var data = (from b in db.telemetries
                            where b.imei == Convert.ToInt64(vno) && b.time > Convert.ToDateTime(to) && b.time <= Convert.ToDateTime(from1)
                           
                            group b by b.imei into g
                            select new
                            {
                                id = g.First().id,
                                imei = g.First().imei,
                                Vehicle_no = db.Tbl_DeviceRegistrations.Where(t => t.DeviceIMEI == g.First().imei).Select(t => t.DeviceName).SingleOrDefault(),
                                
                                workingHrStart = db.telemetries.Where(t => t.imei == g.First().imei && t.digital_inputs > 32950 && t.time > Convert.ToDateTime(to) && t.time <= Convert.ToDateTime(from1)).Select(t => t.time).FirstOrDefault(),
                                workingHrEnd = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == g.First().imei && t.digital_inputs > 32950 && t.time > Convert.ToDateTime(to) && t.time <= Convert.ToDateTime(from1)).Select(t => t.time).FirstOrDefault(),
                                TotalWorkingHr = db.telemetries.Where(t => t.imei == g.First().imei && t.time > Convert.ToDateTime(to) && t.time <= Convert.ToDateTime(from1)).Select(t => Convert.ToDateTime(t.time).Second).Sum(),
                                totoalDistance = db.telemetries.OrderByDescending(t => t.id).Where(t => t.imei == g.First().imei && t.digital_inputs > 32950 && t.time > Convert.ToDateTime(to) && t.time <= Convert.ToDateTime(from1)).Select(t => Convert.ToInt64(t.odometer)).Sum(),
                                EngineRunningTime = db.telemetries.Where(t => t.imei == g.First().imei && t.digital_inputs > 32950 && t.time > Convert.ToDateTime(to) && t.time <= Convert.ToDateTime(from1)).Select(t => Convert.ToDateTime(t.time).Second).Sum(),
                                SpeedVoilation = db.telemetries.Where(t => t.imei == g.First().imei && t.speed > 45 && t.time > Convert.ToDateTime(to) && t.time <= Convert.ToDateTime(from1)).Select(t => Convert.ToInt64(t.speed)).Count(),
                          
                            }).ToList();
                LocalReport lc1 = new LocalReport();
                lc1.ReportPath = Server.MapPath("/Reports/RptFleetAnyalisis.rdlc");
                ReportDataSource rd1 = new ReportDataSource("FleetAnyalisisReport", data.ToList());
                rd1.Value = data;
                lc1.DataSources.Add(rd1);
                string reporttype = "Excel";
                string mimetype1;
                string encoding1;
                string filenameextention1;


                filenameextention1 = "xls";


                Response.ContentType = "application/ms-excel";

                string[] strems1;
                Warning[] warnings1;
                byte[] renderdbyte1;
                renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                    , out strems1, out warnings1);
                Response.AddHeader("content-disposition", "attachment;filename=FleetAnalysis_Report." + filenameextention1);
                return File(renderdbyte1.ToArray(), filenameextention1);
            }
            }
      
        public ActionResult ReportAlertLogPDF(String vno, String from1, String to)
        {

            var s = (string)Session["username"];


            if (s == "" || s == null)
            {
                Response.Write("Your session has een expired please login again");
                Response.Redirect("/Login.aspx");

            }
            var imei1 = (from a in db.Tbl_DeviceRegistrations
                         where a.DeviceName == vno.Trim()
                         select a.DeviceIMEI).SingleOrDefault();
            var data = (from b in db.telemetries
                        orderby b.id descending
                        where b.imei == Convert.ToInt64(imei1) && b.time > Convert.ToDateTime(to).AddDays(-1) && b.time <= Convert.ToDateTime(from1) && b.speed > 45
                        group b by b.imei into g
                      
                        select new
                        {
                            g.First().id,
                           
                            speedCount = db.telemetries.Where(t => t.imei == g.First().imei && t.speed > 45 && t.time > Convert.ToDateTime(to) && t.time <= Convert.ToDateTime(from1)).Select(t => Convert.ToInt64(t.speed)).Count(),
                          
                            Stoppage = db.telemetries.Where(t => t.imei == g.First().imei && t.digital_inputs > 32936 && t.digital_inputs < 32950 && t.time > Convert.ToDateTime(to) && t.time <= Convert.ToDateTime(from1)).Select(t => Convert.ToInt64(t.speed)).Count(),
                            DeviceName = db.Tbl_DeviceRegistrations.Where(t => t.DeviceIMEI == g.First().imei).Select(t => t.DeviceName).SingleOrDefault(),
                            CurrentDate=DateTime.Now
                        }).ToList();

            LocalReport lc1 = new LocalReport();
            lc1.ReportPath = Server.MapPath("/Reports/RptAlertLog.rdlc");
            ReportDataSource rd1 = new ReportDataSource("AlertReport", data.ToList());
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
            Response.AddHeader("content-disposition", "attachment;filename=AlertReport." + filenameextention1);
            return File(renderdbyte1, "." + filenameextention1);

        }
        public ActionResult ReportAlertLogExcel(String vno, String from1, String to)
        {

            var s = (string)Session["username"];


            if (s == "" || s == null)
            {
                Response.Write("Your session has een expired please login again");
                Response.Redirect("/Login.aspx");

            }
            var imei1 = (from a in db.Tbl_DeviceRegistrations
                         where a.DeviceName == vno.Trim()
                         select a.DeviceIMEI).SingleOrDefault();
            var data = (from b in db.telemetries
                        orderby b.id descending
                        where b.imei == Convert.ToInt64(imei1) && b.time > Convert.ToDateTime(to).AddDays(-1) && b.time <= Convert.ToDateTime(from1) && b.speed > 45
                        group b by b.imei into g
                       
                        select new
                        {
                            g.First().id,
                            speedCount = db.telemetries.Where(t => t.imei == g.First().imei && t.speed > 45 && t.time > Convert.ToDateTime(to) && t.time <= Convert.ToDateTime(from1)).Select(t => Convert.ToInt64(t.speed)).Count(),
                            Stoppage = db.telemetries.Where(t => t.imei == g.First().imei && t.digital_inputs > 32936 && t.digital_inputs < 32950 && t.time > Convert.ToDateTime(to) && t.time <= Convert.ToDateTime(from1)).Select(t => Convert.ToInt64(t.speed)).Count(),
                            DeviceName = db.Tbl_DeviceRegistrations.Where(t => t.DeviceIMEI == g.First().imei).Select(t => t.DeviceName).SingleOrDefault(),
                            CurrentDate = DateTime.Now
                        }).ToList();

            LocalReport lc1 = new LocalReport();
            lc1.ReportPath = Server.MapPath("/Reports/RptAlertLog.rdlc");
            ReportDataSource rd1 = new ReportDataSource("AlertReport", data.ToList());
            rd1.Value = data;
            lc1.DataSources.Add(rd1);
            string reporttype = "Excel";
            string mimetype1;
            string encoding1;
            string filenameextention1;

            filenameextention1 = "xls";
            Response.ContentType = "application/ms-excel";
            string[] strems1;
            Warning[] warnings1;
            byte[] renderdbyte1;
            renderdbyte1 = lc1.Render(reporttype, "", out mimetype1, out encoding1, out filenameextention1
                , out strems1, out warnings1);
            Response.AddHeader("content-disposition", "attachment;filename=Alert_Report." + filenameextention1);
            return File(renderdbyte1.ToArray(), filenameextention1);

        }
        //public JsonResult StartDevices()
        //{
        //    try
        //    {


        //        var s = (string)Session["username"];

              
        //        if (s == "" || s == null)
        //        {
        //            Response.Write("Your session has een expired please login again");
        //            Response.Redirect("/Login.aspx");

        //        }
        //        var uId = (from a in db.Tbl_Users where a.username == Request.RequestContext.HttpContext.Session["username"].ToString() select a.UserID).SingleOrDefault();
        //        UserId = Convert.ToInt32(uId);
        //        var Start = (from a in db.Tbl_DeviceRegistrations
                               
        //                     where a.UserId == uId
                         
        //                     select new
        //                     {
                              
        //                         count = db.Tbl_DeviceRegistrations.GroupBy(b => b.UserId).Where(t => t.First().UserId == uId).Select(t => t.First().UserId).Count(),
        //                         running = (double?)(from v in db.telemetries
        //                                             orderby v.id descending
        //                                             where v.imei == a.DeviceIMEI 
        //                                             select v.digital_inputs).FirstOrDefault()
        //                     }).ToList();
                
               
        //        return Json(Start, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(null, JsonRequestBehavior.AllowGet);

        //    }
        //}
        //Dashboard page all chart data
        public JsonResult StopDevices()
        {
            try
            {
                var s = (string)Session["username"];


                if (s == "" || s == null)
                {
                    Response.Write("Your session has een expired please login again");
                    Response.Redirect("/Login.aspx");

                }
                var uId = (from a in db.Tbl_Users where a.username == Request.RequestContext.HttpContext.Session["username"].ToString() select a.UserID).SingleOrDefault();
                UserId = Convert.ToInt32(uId);
                var Stop = (from a in db.Tbl_DeviceRegistrations
                            where a.UserId == UserId
                            select new
                            {
                                
                              
                                //count = db.Tbl_DeviceRegistrations.GroupBy(b => b.UserId).Where(t => t.First().UserId == UserId ).Select(t => t.First().UserId).Count(),
                                running = (double?)(from v in db.telemetries
                                                    orderby v.id descending
                                                    where v.imei == a.DeviceIMEI
                                                    //&&v.digital_inputs<32950
                                                    select v.digital_inputs).FirstOrDefault(),
                                speed = (from v in db.telemetries
                                                    orderby v.id descending
                                                    where v.imei == a.DeviceIMEI
                                                    //&&v.digital_inputs<32950
                                                    select v.speed).FirstOrDefault(),
                            }).ToList();
                
             
                return Json(Stop, JsonRequestBehavior.AllowGet);
                
            }
            catch (Exception ex)
            {
                return Json(null, JsonRequestBehavior.AllowGet);

            }
        }
        //public JsonResult InactiveDevices()
        //{
        //    try
        //    {
        //        var s = (string)Session["username"];


        //        if (s == "" || s == null)
        //        {
        //            Response.Write("Your session has een expired please login again");
        //            Response.Redirect("/Login.aspx");

        //        }
        //        var Inactive = (from a in db.Tbl_DeviceRegistrations
        //                    where a.UserId == 1
        //                    select new
        //                    {
        //                        running = db.telemetries.GroupBy(b => b.imei).Where(t => t.First().imei == a.DeviceIMEI).Select(t => t.First().latitude).First(),
        //                        count = db.Tbl_DeviceRegistrations.GroupBy(b => b.UserId).Where(t => t.First().UserId == 1).Select(t => t.First().UserId).Count(),
        //                    }).ToList();

        //        // var total1 = total * 100;
        //        return Json(Inactive, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(null, JsonRequestBehavior.AllowGet);

        //    }
        //}


    }
     }
   


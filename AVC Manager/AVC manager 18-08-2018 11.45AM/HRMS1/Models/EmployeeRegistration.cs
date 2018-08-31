using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMS1.Models
{
    public class EmployeeRegistration
    {
        public string reporttype { get; set; }
        public string dropdown { get; set; }
        public DateTime datefrom { get; set; }
        public DateTime todate { get; set; }
        public string newpassword { get; set; }
        public string oldpassword { get; set; }
        public string redate { get; set; }
        public string hours { get; set; }
        public string min { get; set; }
        public string button { get; set; }
        public string EmpId { get; set; }
        public string Empname { get; set; }
        public string Fname { get; set; }
        public string DOJ { get; set; }
        public string passportno { get; set; }
        public string mobile { get; set; }
        public string homeno { get; set; }
        public string panno { get; set; }
        public string emailid { get; set; }
        public string DOB { get; set; }
        public string Designation { get; set; }
        public string gender { get; set; }
        public string meritialstetus { get; set; }
        public string Department { get; set; }
        public string basicsalary { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string acountno { get; set; }
        public string bankname { get; set; }
        public string brancename { get; set; }
        public string ifsc { get; set; }
        public string nameinbank { get; set; }
      

        public string PG_Colgname { get; set; }
        public string PG_Universityname { get; set; }
        public string PG_State { get; set; }
        public string PG_City { get; set; }
        public string PG_yearofPassing { get; set; }
        public string PGMarks { get; set; }
        public string G_colgname { get; set; }
        public string G_University { get; set; }
        public string G_State { get; set; }
        public string G_City { get; set; }
        public string G_yearOfPassing { get; set; }
        public string G_marks { get; set; }
        public string _12_ColgName { get; set; }
        public string _12_Board { get; set; }
        public string _12_State { get; set; }
        public string _12_City { get; set; }
        public string _12_yearofpassing { get; set; }
        public string _12_marks { get; set; }
        public string school_name { get; set; }
        public string Board_name { get; set; }
        public string S_state { get; set; }
        public string S_City { get; set; }
        public string S_yaerofpassing { get; set; }
        public string S_Marks { get; set; }
    }
    public class empdoc
    {
       public string profile { get; set; }
        public string id { get; set; }
        public string resume { get; set; }
        public string offerletter { get; set; }
        public string joiningletter { get; set; }
        public string pancard { get; set; }
        public string aadharcard { get; set; }
        public string addressproof { get; set; }

    }
    public class Sendleaves
    {
        public int EmpId { get; set; }
        public string leavetype { get; set; }
        public DateTime DateTo { get; set; }
        public DateTime DateFrom { get; set; }
        public string comment { get; set; }

    }
    public class salary
    {
        public int EmpId { get; set; }
        public DateTime DateTo { get; set; }
        public DateTime DateFrom { get; set; }
        public float basicsalary { get; set; }
        public int totaldays { get; set; }

        public int presentdays { get; set; }
        public int leavedays { get; set; }
        public float salaryperday { get; set; }
        public float otherpay { get; set; }
        public float totalpay { get; set; }
        public float leavededuct { get; set; }
        public float TDS { get; set; }
        public float profTax { get; set; }
        public float otherdeduction { get; set; }
        public float totalearning { get; set; }


    }

    }
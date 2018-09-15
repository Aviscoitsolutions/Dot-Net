<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebApplication2.Login.Login" %>

<!--
Author: W3layouts
Author URL:http://www.aviscoitsolutions.com
    License: Creative Commons Attribution 3.0 Unported
License URL: http://creativecommons.org/licenses/by/3.0/
-->
<!DOCTYPE html>
<html>
<head>
      <title>GPS TRACKING SYSTEM |User Login</title>
	<%--<meta name="viewport" content="width=device-width, initial-scale=1" />
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<meta name="keywords" content="package signup Form Responsive Widget,Login form widgets, Sign up Web forms , Login signup Responsive web form,Flat Pricing table,Flat Drop downs,Registration Forms,News letter Forms,Elements" />--%>
<script type="application/x-javascript"> addEventListener("load", function() { setTimeout(hideURLbar, 0); }, false);
function hideURLbar(){ window.scrollTo(0,1); } </script>
<!-- Meta tag Keywords -->
   
<link href="Login/css/style.css"  rel="stylesheet" type="text/css" media="all">
    <%--<link href="../MainPage/css/bootstrap.css" rel="stylesheet" />--%>
<link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.1.1/css/bootstrap.min.css" integrity="sha384-WskhaSGFgHYWDcbwN70/dfYBj47jz9qbsMId/iRN3ewGhXQFZCSftd1LZCfmhktB" crossorigin="anonymous">
    <link href="../MainPage/css/font-awesome.min.css" rel="stylesheet" />
<link href="http://fonts.googleapis.com/css?family=Montserrat:100,200,300,400,500,600,700,800,900&amp;subset=cyrillic,cyrillic-ext,latin-ext,vietnamese" rel="stylesheet">
</head>
<body>
    <div class="container-fluid nopdg">
        <div class="row nomlr">
            <div class="col-md-4 vh100">
              <%--  <div class="logo pt-10">
                   <img src="Login/images/AVISCO%2012.png" class="mt-5"/>
                </div>--%>
                <div class="form1 pl-3">
	                <div class="form-content1 pt-10"> <!--Remove-->
		                <form id="form11"  class="login"  runat="server">
			                <div class="form-info1" style="text-align: center;">
				                <h2 style="color:#fff">LOGIN</h2>
                                <asp:Label  ID="Label1"  runat="server" Text="Label" CssClass="loginlabel" Visible="False"  ></asp:Label>   
			                </div>
                           
			                <div class="name1 w100" >
				                <%--<label class="w100">User-Name</label>--%>
                                <div class="row">
                                    <div class="col-md-1">
                                     <i class="fa fa-user float-left" aria-hidden="true" ></i></div>
                                    <div class="col-md-10">
                                        <input class="input1 w100 float-right"  type="text" name="name" placeholder="Username" required="" tabindex="1">
                                    </div>
			                    </div>
                                </div>

			                <div class="mt-3">
				                <div class="row">
                                    <div class="col-md-1">
                                        <i class="fa fa-unlock" aria-hidden="true"></i>
                                        </div>
                                    <div class="col-md-10">
				                        <input class="input1 w100" type="password" name="pasdsword" placeholder="Password" required="">
                                    </div>
			                    </div>
			                </div>
			                <div class="signup text-center">
				                &nbsp;<asp:Button ID="Button1" runat="server" Text="LOGIN" OnClick="Button1_Click"/>
			                </div>            		   
		                </form>                         
	                </div>
<a href="Home/UploadLogo" >Upload Image</a>
                </div>

                 <div class="ptf">
                     <%--opyright &copy; AVISCO.<br /> 
                     All rights reserved |--%> Design by <a href="http://www.avtrack.net/">AVTRACK.NET</a>
                 </div>
                  
                </div>

            <div class="col-md-8">                
                <img src="Login/images/bri_1.jpg" class="vh100 w100"/>
            </div>

            </div>
        </div>
   <%-- <%--<div class="hyperlink">
     <a href="../Registration/Registration.aspx">Create Account</a>
        </div>--%>
<%--<footer>&copy; AVISCO. All rights reserved | Design by <a href="http://<%--www.aviscoitsolutions.com/">AVISCO IT SOLUTIONS</a></footer>-
    <footer>&copy; AVTRACK. All rights reserved | Design by <a href="http://www.aviscoitsolutions.com/">AVTRACK.NET</a></footer>--%>
     
   

    
</html>
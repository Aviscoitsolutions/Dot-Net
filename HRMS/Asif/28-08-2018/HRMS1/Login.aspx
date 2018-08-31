<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="HRMS1.Login" %>

<!DOCTYPE html>

<html>
<head>
    <script src="//code.jquery.com/jquery-1.11.1.min.js"></script>
    <meta name="viewport" content="width=device-width" />
    <title>login</title>
    <style>
        body {
            background-color: white;
            font-family: Arial, Helvetica, sans-serif;
        }

        form {
            border: 3px solid #898787;
            width: 40%;
            margin: 0 auto;
            margin-top: 100px;
            box-shadow: 0 0 20px #808080;
        }

        input[type=text], input[type=password] {
            width: 100%;
            padding: 12px 20px;
            margin: 8px 0;
            display: inline-block;
            border: 1px solid #ccc;
            box-sizing: border-box;
        }

        button {
            background-color: #4CAF50;
            color: white;
            padding: 14px 20px;
            margin: 8px 0;
            border: none;
            cursor: pointer;
            width: 100%;
        }

            button:hover {
                opacity: 0.8;
            }

        .cancelbtn {
            width: auto;
            padding: 10px 18px;
            background-color: #f44336;
        }

        .imgcontainer {
            text-align: center;
            /*// margin: 24px 0 12px 0;*/
            background-color: #f1f1f1;
            font-size: 27px;
            padding-top: 20px;
            padding-bottom: 10px;
            height: 50px;
            text-align: center;
        }

        img.avatar {
            width: 40%;
            border-radius: 50%;
        }

        .container {
            padding: 16px;
        }

        span.psw {
            float: right;
            padding-top: 16px;
        }
    </style>

</head>
<body>
    <h1></h1><h1></h1><br />
     <form id="form"  class="login"  runat="server">

        <div class="imgcontainer">
          <label >Login</label>
        </div>

        <div class="container">
            <label for="uname" ><b>User ID</b></label>
            <input type="text" id="username" placeholder="Enter User id" name="name"  required >

            <label for="psw"><b>Password</b></label>
            <input type="password" placeholder="Enter Password"  required name="pasdsword" id="password" >
       
          <%--  <button type="submit" name="login" id="Button1" OnClick="Button1_Click" >Login</button>--%>
            <asp:Button ID="Button2" runat="server" Text="LOGIN" Class="submit" OnClick="Button1_Click" />
            <label>
                <input type="checkbox" checked="checked" name="remember"> Remember me
            </label>
        </div>

        <div class="container" style="background-color:#f1f1f1">
            <button type="button" class="cancelbtn" id="clear">Cancel</button>
            <span class="psw">Forgot <a href="#">password?</a></span>
        </div>
    </form>


</body>

</html>
<script>
    $('#clear').click(function () {
        document.getElementById('username').value = "";
        document.getElementById('password').value = "";
        document.getElementById('username').focus();
    });
</script>

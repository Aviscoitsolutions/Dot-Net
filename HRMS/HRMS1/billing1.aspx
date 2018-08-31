<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="billing1.aspx.cs" Inherits="HRMS1.billing1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <link href="~/css/style.css" rel="stylesheet" />
    <script src="~/bootstrap/jquery.min.js"></script>
    <script src="~/Scripts/modernizr-2.8.3.js"></script>
    <link href="~/Content/bootstrap.min.css" rel="stylesheet" />
    <link href="~/Content/bootstrap.css" rel="stylesheet" />
    <link href="~/Content/bootstrap-theme.css" rel="stylesheet" />
</head>
<body>
     <div class="container">
           <div class="row">
            <div class="col-sm-9 local-setting" id="bill-pay">
    <form id="form1" runat="server">
        <div>
          
                    
                       <h1> Bill Amount<asp:Label ID="Label1" runat="server" Text="Label"></asp:Label></h1>
                  
                 
                    <p class="heading1"> Bill Pay</p>
                    <hr class="hr" />
                    <label class="lable-font ">Full Name:</label>
                <%--    <input type="text" id="fullname" onKeyPress="return ValidateAlpha(event);" style=" width:500px; margin-left:60px;" />--%><asp:TextBox ID="txtname" runat="server" onKeyPress="return ValidateAlpha(event);" style=" width:500px; margin-left:60px;"></asp:TextBox><br /><p></p>
                    <label class="lable-font">Address:</label>
                    <%--<input type="text" id="address" style=" width:500px; margin-left:72px;" />--%><asp:TextBox ID="txtaddress" runat="server" style=" width:500px; margin-left:72px;"></asp:TextBox><br />
                    <label style="margin-left:150px;">Street Address</label><br />
                   <%-- <input type="text" id="city" onKeyPress="return ValidateAlpha(event);" style="margin-left:150px;" />--%>
            <asp:TextBox ID="txtcity" runat="server" onKeyPress="return ValidateAlpha(event);" style="margin-left:150px;"></asp:TextBox>
                 <%--   <input type="text" id="state" onKeyPress="return ValidateAlpha(event);" style="margin-left:20px" />--%>  <asp:TextBox ID="txtstate" runat="server" onKeyPress="return ValidateAlpha(event);" style="margin-left:20px"></asp:TextBox><br />
                    <label style="margin-left:150px">City</label>
                    <label style="margin-left:158px;">State</label><br />
                   <%-- <input type="text" id="pin-cone" onKeyPress="return isNumber(event);" style="margin-left:150px" />--%>  <asp:TextBox ID="txtpincode" runat="server" onKeyPress="return isNumber(event);" style="margin-left:150px"></asp:TextBox><br />
                    <label style="margin-left:150px">Postal code</label>
                    <br />
                    <label class="lable-font">Email:</label>
                   <%-- <input type="email" id="email" style="margin-left:90px; width:500px;" />--%>
            <asp:TextBox ID="txtemail" runat="server" style="margin-left:90px; width:500px;"></asp:TextBox>
              <asp:TextBox ID="txnid1" runat="server"></asp:TextBox>
                    <br />
                    <br />
                <%--    <input class="btn-success p-3 mt-2" style="width:30%;padding: 10px;margin-left: 30%;" id="proceed" value="Proced To Pay" />--%>
              <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" />
             <input type="hidden" runat="server" id="key" name="key" value="gtKFFx" />
        <input type="hidden" runat="server" id="salt" name="salt" value="eCwWELxi" />
        <input type="hidden" runat="server" id="hash" name="hash" value=""  />
              
        </div>
      
                
      
                </form>
         </div>
               </div>
         </div>
        
   
</body>
</html>
<script>
   
        paypal.Button.render({
            env: 'sandbox',
            client: {
            sandbox: 'AR1nMutE7cxQo8pogQITV_7VilBXxQ1fN8EKrb9zruZZHuClJuX1bVznNVPUcnh8I5sA2CE07hAoGUgf'
            },
            payment: function (data, actions) {
                return actions.payment.create({
            transactions: [{
            amount: {
            total: '0.01',
                            currency: 'USD'
                        }
                    }]
                });
            },
            onAuthorize: function (data, actions) {
                return actions.payment.execute()
                    .then(function () {
            window.alert('Thank you for your purchase!');
                    });
            }
        }, '#paypal-button');
    
    function isNumber(evt) {
        var iKeyCode = (evt.which) ? evt.which : evt.keyCode
        if (iKeyCode != 46 && iKeyCode > 31 && (iKeyCode < 48 || iKeyCode > 57))
            return false;

        return true;
    }
    function ValidateAlpha(evt) {
        var keyCode = (evt.which) ? evt.which : evt.keyCode
        if ((keyCode < 65 || keyCode > 90) && (keyCode < 97 || keyCode > 123) && keyCode != 32)

            return false;
        return true;
    }
</script>

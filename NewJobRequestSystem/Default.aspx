<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="NewJobRequestSystem.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />

    <meta charset="utf-8" />

    <meta http-equiv="X-UA-Compatible" content="IE=edge" />

    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <title>Login</title>

    <link rel="shortcut icon" href="images/sohbiicon.ico" />

    <link href="../Bootstrap/Login/vendors/bootstrap/dist/css/bootstrap.min.css" rel="stylesheet" />

    <link href="../Bootstrap/Login/vendors/font-awesome/css/font-awesome.min.css" rel="stylesheet" />

    <link href="../Bootstrap/Login/vendors/nprogress/nprogress.css" rel="stylesheet" />

    <link href="../Bootstrap/Login/vendors/animate.css/animate.min.css" rel="stylesheet" />

    <link href="../Bootstrap/Login/build/css/custom.min.css" rel="stylesheet" />
</head>

<body class="login">

    <form id="form1" runat="server">
        
        <asp:ScriptManager runat="server" ID="ScriptManager1"></asp:ScriptManager>
        
        <div class="login_wrapper">

            <div class="animate form login_form">
                <br />
                <br />
                <br />
                <section class="login_content">

                    <div class="login-logo aligncenter">

                        <img src="images/skpilogopic.png" style="height: 100px; width: 120px;" />

                        <br />

                        <b>Sohbi Kohgei (Phils.), Inc.</b>

                        <h4 style="color: #337ab7;">Online Job Order System</h4>

                    </div>
                    <div class="separator"></div>

                    <br />
                    <div>
                        <asp:TextBox ID="tbEmpID" class="form-control" MaxLength="12" runat="server" placeholder="Employee ID" OnTextChanged="Login" />
                    </div>

                    <br />
                    <div>
                        <asp:Button class="btn btn-primary submit" ID="btnsignin" Width="100%" runat="server" Text="Sign In" OnClick="Login"></asp:Button>
                        <br />
                    </div>

                    <div class="clearfix">
                        <br />
                    </div>

                </section>
            </div>

        </div>

    </form>

</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotPass.aspx.cs" Inherits="peerreviewproject.ForgotPass" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Outfit" />
    <style>
          html, body {
            margin: 0;
            height: 100%;
            font-family: "Outfit", sans-serif;
            overflow: hidden;
            background-image: linear-gradient(lightsteelblue, white);
            
            background-attachment: fixed;
        }

        .bg-container {
            width: 100%;
            height: 100%;
            
            box-sizing: border-box;
            background-image: url("backgroundUSI.png");
            
            background-repeat: no-repeat;
            background-position: top;
        }

        .centered {
            position: fixed;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -100%);
        }

    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="centered">
            Enter Username<br />
            <asp:TextBox ID="UsernameTB" runat="server" MaxLength="49"></asp:TextBox>
&nbsp;&nbsp;&nbsp;
            <asp:Button ID="UsersubmitBttn" runat="server" OnClick="UsersubmitBttn_Click" Text="Submit" />
            <br />
            <asp:Label ID="Label1" runat="server" Text="Email sent to user with password instructions" Visible="False"></asp:Label>
            <br />
            <asp:Label ID="tokenLabel" runat="server" ForeColor="Red" Text="Token has expired! Please create a new one by entering Email again." Visible="False"></asp:Label>
            <br />
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Return to Login Page" />
        </div>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePass.aspx.cs" Inherits="peerreviewproject.ChangePass" %>

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
            <h3>Enter a new password</h3><br />
            
            <asp:TextBox ID="passTB1" runat="server" TextMode="Password" MaxLength="49"></asp:TextBox>
            <br />
            <p>Must be at least 8 characters</p><br />
            <br />
            <h3>Confirm Password</h3><br />
            <asp:TextBox ID="PassTB2" runat="server" TextMode="Password" MaxLength="49"></asp:TextBox>
&nbsp;&nbsp;&nbsp;
            <asp:Button ID="Button1" runat="server" OnClick="SubmitBttn_click" Text="Submit" />
            <br />
            <br />
            <br />
            <asp:Label ID="Label1" runat="server" Visible="False"></asp:Label>
        </div>
        <asp:Button ID="HomeBttn" runat="server" OnClick="HomeBttn_Click" Text="Home" Visible="False" />
    </form>
</body>
</html>

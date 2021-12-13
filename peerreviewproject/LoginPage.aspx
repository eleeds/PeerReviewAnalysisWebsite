<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginPage.aspx.cs" Inherits="peerreviewproject.WebForm1" %>

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
    <form id="form1" runat="server" class="bg-container">
        <div class="centered">
            <table style="margin: auto"><tr><td><h1>PEER REVIEW APPLICATION</h1></td></tr></table>
            <table style="margin: auto">
                
                <tr>
                    <td>
                        <asp:Label ID="emailLbl" runat="server" Text="Email"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="emailBox" runat="server" MaxLength="49">idk300@eagles.usi.edu</asp:TextBox></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="passwordLbl" runat="server" Text="Password"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="passwordBox" runat="server" TextMode="Password" MaxLength="49"></asp:TextBox></td>
                </tr>
             
                
            </table>
            <table style="margin: auto"><tr><td><asp:Button ID="loginButton" runat="server" Text="Login"
                OnClick="LoginButton_Click" />&nbsp;&nbsp;
                <asp:Button ID="Button1" runat="server" OnClick="ForgotBttn_click" Text="Forgot Password" />
                </td></tr></table>
            <table style="margin: auto"><tr><td><asp:Label ID="lblError" runat="server" Text="The email or password entered is incorrect" 
                ForeColor="Red"></asp:Label></td></tr></table>
        </div>
    </form>
</body>
</html>

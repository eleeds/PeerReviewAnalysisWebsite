<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginPage.aspx.cs" Inherits="peerreviewproject.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        body{
            background-color: white;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table style="margin: auto"><tr><td><b>USI PEER REVIEW APPLICATION</b></td></tr></table>
            <table style="margin: auto; border: 5px solid white">
                
                <tr>
                    <td>
                        <asp:Label ID="emailLbl" runat="server" Text="email"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="emailBox" runat="server">idk300@eagles.usi.edu</asp:TextBox></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="passwordLbl" runat="server" Text="password"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="passwordBox" runat="server" TextMode="Password"></asp:TextBox></td>
                </tr>
             
                
            </table>
            <table style="margin: auto"><tr><td><asp:Button ID="loginButton" runat="server" Text="Login"
                OnClick="loginButton_Click" />&nbsp;&nbsp;
                <asp:Button ID="Button1" runat="server" OnClick="ForgotBttn_click" Text="Forgot Password" />
                </td></tr></table>
            <table style="margin: auto"><tr><td><asp:Label ID="lblError" runat="server" Text="the email or password entered is incorrect" 
                ForeColor="Red"></asp:Label></td></tr></table>
        </div>
    </form>
</body>
</html>

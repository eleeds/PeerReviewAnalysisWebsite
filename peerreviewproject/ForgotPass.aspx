<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotPass.aspx.cs" Inherits="peerreviewproject.ForgotPass" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            Enter Username<br />
            <asp:TextBox ID="UsernameTB" runat="server"></asp:TextBox>
&nbsp;&nbsp;&nbsp;
            <asp:Button ID="UsersubmitBttn" runat="server" OnClick="UsersubmitBttn_Click" Text="Submit" />
            <br />
            <asp:Label ID="Label1" runat="server" Text="Email sent to user with password instructions" Visible="False"></asp:Label>
        </div>
    </form>
</body>
</html>

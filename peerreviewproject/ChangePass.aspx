<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePass.aspx.cs" Inherits="peerreviewproject.ChangePass" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            Enter New Password<br />
            <br />
            <asp:TextBox ID="passTB1" runat="server" TextMode="Password"></asp:TextBox>
            <br />
            Must be at least 8 characters<br />
            <br />
            Confirm Password<br />
            <asp:TextBox ID="PassTB2" runat="server" TextMode="Password"></asp:TextBox>
&nbsp;&nbsp;&nbsp;
            <asp:Button ID="Button1" runat="server" OnClick="SubmitBttn_click" Text="Submit" />
            <br />
            <br />
            <br />
            <asp:Label ID="Label1" runat="server" Visible="False"></asp:Label>
        </div>
    </form>
</body>
</html>

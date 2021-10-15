<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StudentMain.aspx.cs" Inherits="peerreviewproject.StudentMain" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
   <form id="form1" runat="server">
       <div>
        <table>
        <tr><td>USI PEER REVIEW APPLICATION STUDENT DASHBOARD</td></tr>
            <tr>
                <td>
                <asp:Label ID="lblStudentDetails" runat="server" Text=""></asp:Label>

                </td>

            </tr>
            <tr></tr>
    
        </table>
           <asp:Button ID="logoutButton" runat="server" Text="Logout" OnClick="logoutButton_Click" />
       
   </div>
   </form>
    </body>
</html>

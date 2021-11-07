<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateClassPage.aspx.cs" Inherits="peerreviewproject.CreateClassPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="font-size: 42px">
            <asp:Label ID="Label4" runat="server" BackColor="#CCCCCC" Text="Create new course"></asp:Label>
            <br />
            <br />
            <asp:Table ID="Table1" runat="server" GridLines="Both" Width="696px" Height="385px" BorderStyle="Double">
                <asp:TableRow runat="server">
                    <asp:TableCell runat="server">
                        <asp:Label ID="Label1" runat="server" Text="Course Department"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell runat="server">
                        <asp:TextBox ID="CourseDepartTB" runat="server"></asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server">
                    <asp:TableCell runat="server">
                        <asp:Label ID="Label2" runat="server" Text="Course Number"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell runat="server">
                        <asp:TextBox ID="CourseNumberTB" runat="server"></asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server">
                    <asp:TableCell runat="server">
                        <asp:Label ID="Label3" runat="server" Text="Course Name"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell runat="server">
                        <asp:TextBox ID="CourseNameTB" runat="server"></asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            
            
            
            <br />
            <asp:Button ID="CreateCourse_btn" runat="server" Text="Create Course" BorderStyle="Inset" OnClick="CreateCourse_btnclick" />
            
            
            
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Back" />
            
            
            
        </div>
    </form>
</body>
</html>

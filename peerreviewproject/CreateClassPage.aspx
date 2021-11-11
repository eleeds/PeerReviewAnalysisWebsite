<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateClassPage.aspx.cs" Inherits="peerreviewproject.CreateClassPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            height: 966px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div style="font-size: 42px" class="auto-style1">
            <asp:Label ID="Label4" runat="server" BackColor="#CCCCCC" Text="Create new course"></asp:Label>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Caption="Current Classes" CellPadding="4" DataKeyNames="courseID" DataSourceID="SqlDataSource1" EmptyDataText="No classes" ForeColor="#333333" GridLines="None" HorizontalAlign="Right" ShowHeaderWhenEmpty="True">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField DataField="courseName" HeaderText="Course Name" SortExpression="courseName" />
                    <asp:BoundField DataField="courseID" HeaderText="courseID" InsertVisible="False" ReadOnly="True" SortExpression="courseID" Visible="False" />
                    <asp:CommandField DeleteText="Delete Class" ShowDeleteButton="True" />
                </Columns>
                <EditRowStyle BackColor="#7C6F57" />
                <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#E3EAEB" />
                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#F8FAFA" />
                <SortedAscendingHeaderStyle BackColor="#246B61" />
                <SortedDescendingCellStyle BackColor="#D4DFE1" />
                <SortedDescendingHeaderStyle BackColor="#15524A" />
            </asp:GridView>
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
                        <asp:TextBox ID="CourseNumberTB"  runat="server"></asp:TextBox>
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
            
            
            
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Peer_ReviewConnectionString2 %>" DeleteCommand="DELETE FROM Course_table WHERE (courseID = @courseID)" SelectCommand="SELECT a.courseName, a.courseID FROM Course_table AS a INNER JOIN Course_access_table AS b ON a.courseID = b.courseID WHERE (b.userID = @user )">
                <DeleteParameters>
                    <asp:Parameter Name="courseID" />
                </DeleteParameters>
                <SelectParameters>
                    <asp:SessionParameter Name="user" SessionField="userID" />
                </SelectParameters>
            </asp:SqlDataSource>
            
            
            
            <br />
            <asp:Button ID="CreateCourse_btn" runat="server" Text="Create Course" BorderStyle="Inset" OnClick="CreateCourse_btnclick" />
            
            
            
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Back" />
            
            
            
        </div>
    </form>
</body>
</html>

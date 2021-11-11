<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StudentMain.aspx.cs" Inherits="peerreviewproject.StudentMain" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>STUDENT DASHBOARD</title>
</head>
<body>
   <form id="form1" runat="server">
       <div>
        <table style="margin: auto">
        <tr><td>USI PEER REVIEW APPLICATION STUDENT DASHBOARD</td></tr>
            <tr>
                <td>
                <asp:Label ID="lblStudentDetails" runat="server" Text=""></asp:Label>

                </td>

            </tr>
            <tr>
                <td>
                    
                </td>
            </tr>
                        
        </table>
           <table style="margin: auto" >
               <tr><td><asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Caption="Classes" CellPadding="4" DataSourceID="SqlDataSource1" EmptyDataText="No Classes" ForeColor="#333333" GridLines="None" ShowFooter="True" ShowHeaderWhenEmpty="True">
                   <AlternatingRowStyle BackColor="White" />
               <Columns>
                   <asp:BoundField DataField="courseName" HeaderText="courseName" SortExpression="courseName" />
                   <asp:BoundField DataField="name" HeaderText="name" SortExpression="name" />
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
                   <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Peer_ReviewConnectionString2 %>" SelectCommand="SELECT Course_table.courseName, teams_table.name FROM Course_access_table INNER JOIN Course_table ON Course_access_table.courseID = Course_table.courseID INNER JOIN User_table ON Course_access_table.userID = User_table.ID INNER JOIN UserTeam_table ON Course_table.courseID = UserTeam_table.courseID AND User_table.ID = UserTeam_table.userID INNER JOIN teams_table ON Course_table.courseID = teams_table.courseID AND UserTeam_table.teamID = teams_table.teamID WHERE (User_table.email = @email)">
                       <SelectParameters>
                           <asp:SessionParameter Name="email" SessionField="email" />
                       </SelectParameters>
                   </asp:SqlDataSource>
                   </td></tr>
               <tr><td><asp:Button ID="logoutButton" runat="server" Text="Logout" OnClick="logoutButton_Click" /></td></tr>
           </table>
           
           
       
   </div>
   </form>
    </body>
</html>

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
                    
                    <asp:Button ID="ChangePassButton" runat="server" OnClick="ChangePass_Click" Text="Change Password" />
                    
                </td>
            </tr>
                        
        </table>
           <table style="margin: auto" >
               <tr><td>
                   </td></tr>
               <tr><td><asp:Button ID="logoutButton" runat="server" Text="Logout" OnClick="logoutButton_Click" />&nbsp;&nbsp; </td></tr>
               <tr><td>
                   <asp:GridView ID="StudentReviewsGridview" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource2" DataKeyNames="courseID,teamID" OnSelectedIndexChanged="StudentReviewsGridView_SelectedIndexChanged" Width="699px" AllowSorting="True" OnDataBound="StudentReviewsGridView_DataBound" Caption="Classes" EmptyDataText="No current Classes" ShowHeaderWhenEmpty="True">
                       <Columns>
                           <asp:CommandField ShowSelectButton="True" />
                           <asp:BoundField DataField="courseName" HeaderText="Course" SortExpression="courseName" />
                           <asp:BoundField DataField="teamID" HeaderText="teamID" SortExpression="teamID" Visible="False" />
                           <asp:BoundField DataField="questionSet" HeaderText="Set" SortExpression="questionSet" >
                           <ItemStyle HorizontalAlign="Center" />
                           </asp:BoundField>
                           <asp:BoundField DataField="dueDate" HeaderText="Due Date" SortExpression="dueDate" >
                           <ItemStyle ForeColor="Red" HorizontalAlign="Center" />
                           </asp:BoundField>
                           <asp:BoundField DataField="courseID" HeaderText="courseID" InsertVisible="False" ReadOnly="True" SortExpression="courseID" Visible="False" />
                           <asp:BoundField DataField="name" HeaderText="Team" SortExpression="name" >
                           <ItemStyle HorizontalAlign="Center" />
                           </asp:BoundField>
                           <asp:TemplateField HeaderText="Complete">
                               <ItemStyle ForeColor="Green" HorizontalAlign="Center" />
                           </asp:TemplateField>
                       </Columns>
                   </asp:GridView>
                   <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Peer_ReviewConnectionString2 %>" SelectCommand="SELECT Course_table.courseName, UserTeam_table.teamID, SetDueDates_table.questionSet, SetDueDates_table.dueDate, Course_table.courseID, teams_table.name FROM Course_table INNER JOIN UserTeam_table ON Course_table.courseID = UserTeam_table.courseID INNER JOIN SetDueDates_table ON Course_table.courseID = SetDueDates_table.courseID INNER JOIN teams_table ON Course_table.courseID = teams_table.courseID AND UserTeam_table.teamID = teams_table.teamID WHERE (UserTeam_table.userID = @userID) AND (SetDueDates_table.showStudents = 1) ORDER BY Course_table.courseName, SetDueDates_table.dueDate">
                       <SelectParameters>
                           <asp:SessionParameter Name="userID" SessionField="userID" />
                       </SelectParameters>
                   </asp:SqlDataSource>
                   </td></tr>
           </table>
           
           
       
   </div>
   </form>
    </body>
</html>

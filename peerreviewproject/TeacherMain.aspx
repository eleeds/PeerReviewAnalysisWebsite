<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TeacherMain.aspx.cs" Inherits="peerreviewproject.TeacherMain" EnableSessionState="True" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Professor DASHBOARD</title>
</head>
<body>
   <form id="form1" runat="server">
       <div>
        <table style="margin: auto">
        <tr><td>USI PEER REVIEW APPLICATION PROFESSOR DASHBOARD</td></tr>
            <tr>
                <td>
                <asp:Label ID="lblprofessor" runat="server"></asp:Label>

                </td>

            </tr>
            <tr>
                <td>
                    
                    <asp:Button ID="logoutButton" runat="server" Text="Logout" OnClick="logoutButton_Click" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    
                    <asp:Button ID="ChangePassButton" runat="server" OnClick="ChangePassButton_Click" Text="Change Password" />
                    
                    <asp:Button ID="AdminButton" runat="server" Enabled="False" Text="Admin" Visible="False" OnClick="AdminButton_Click" />
                    
                </td>
            </tr>
                        
        </table>
           <table style="margin: auto" >
               <tr><td><asp:GridView ID="TeacherCourseGridview" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" OnSelectedIndexChanged="TeacherCourseGridview_SelectedIndexChanged" AllowPaging="True" AllowSorting="True" CellPadding="4" ForeColor="#333333">
                   <AlternatingRowStyle BackColor="White" />
               <Columns>
                   <asp:CommandField ShowSelectButton="True" SelectText="View Reviews" />
                   <asp:BoundField DataField="Course" HeaderText="Course" SortExpression="Course" />
                   <asp:BoundField DataField="courseNumber" HeaderText="Course Number" SortExpression="courseNumber" />
                   <asp:BoundField DataField="Semester" HeaderText="Semester" SortExpression="Semester" />
                   <asp:BoundField DataField="Year" HeaderText="Year" SortExpression="Year" />
                   <asp:BoundField DataField="College" HeaderText="College" SortExpression="College" />
               </Columns>
                   <EditRowStyle BackColor="#2461BF" />
                   <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                   <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                   <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                   <RowStyle BackColor="#EFF3FB" />
                   <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                   <SortedAscendingCellStyle BackColor="#F5F7FB" />
                   <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                   <SortedDescendingCellStyle BackColor="#E9EBEF" />
                   <SortedDescendingHeaderStyle BackColor="#4870BE" />
           </asp:GridView>
                   <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Peer_ReviewConnectionString2 %>" SelectCommand="SELECT b.courseName AS Course, b.courseSemester AS Semester, b.year AS Year, b.courseDepartment AS College, b.courseNumber FROM Course_access_table AS a INNER JOIN Course_table AS b ON a.courseID = b.courseID WHERE (a.userID = @user )">
                       <SelectParameters>
                           <asp:SessionParameter Name="user" SessionField="userID" />
                       </SelectParameters>
                   </asp:SqlDataSource>
                   </td></tr>
               <tr><td>&nbsp;</td></tr>
               <tr><td>
                   <asp:Button ID="EditCoursesbttn" runat="server" OnClick="EditClassesbttn_Click" Text="Edit Courses" />
                   <asp:Button ID="CourseStudentsbttn" runat="server" OnClick="EditStudentsbttn_Click" Text="Edit Course Students" />
                   </td></tr>
               <tr><td>
                   <br />
                   <asp:Button ID="Groupsbttn" runat="server" OnClick="EditGroupsbttn_Click" Text="Edit Groups" />
                   <asp:Button ID="CourseReviewsbttn" runat="server" OnClick="EditReviewsbttn_Click" Text="Edit Course Reviews" />
                   <br />
                   &nbsp;
                   </td></tr>
               <tr><td>
                   &nbsp;</td></tr>
           </table>
           
           
       
   </div>
   </form>
    </body>
</html>


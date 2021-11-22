<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TeacherMain.aspx.cs" Inherits="peerreviewproject.TeacherMain" %>

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
                    
                </td>
            </tr>
                        
        </table>
           <table style="margin: auto" >
               <tr><td><asp:GridView ID="TeacherCourseGridview" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" DataKeyNames="courseName" OnSelectedIndexChanged="TeacherCourseGridview_SelectedIndexChanged">
               <Columns>
                   <asp:CommandField SelectText="View" ShowSelectButton="True" />
                   <asp:BoundField DataField="courseName" HeaderText="Course" SortExpression="courseName" />
               </Columns>
           </asp:GridView>
                   <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Peer_ReviewConnectionString2 %>" SelectCommand="SELECT b.courseName FROM Course_access_table AS a INNER JOIN Course_table AS b ON a.courseID = b.courseID WHERE (a.userID = @user )">
                       <SelectParameters>
                           <asp:SessionParameter Name="user" SessionField="userID" />
                       </SelectParameters>
                   </asp:SqlDataSource>
                   </td></tr>
               <tr><td><asp:Button ID="logoutButton" runat="server" Text="Logout" OnClick="logoutButton_Click" /></td></tr>
               <tr><td>
                   <asp:Button ID="EditCoursesbttn" runat="server" OnClick="EditClassesbttn_Click" Text="Edit Courses" />
                   </td></tr>
               <tr><td>
                   <asp:Button ID="CourseStudentsbttn" runat="server" OnClick="EditStudentsbttn_Click" Text="Edit Course Students" />
                   <br />
                   <br />
                   <asp:Button ID="CourseReviewsbttn" runat="server" OnClick="EditReviewsbttn_Click" Text="Edit Course Reviews" />
                   &nbsp;
                   <asp:Button ID="Groupsbttn" runat="server" OnClick="EditGroupsbttn_Click" Text="Edit Groups" />
                   </td></tr>
               <tr><td>
                   &nbsp;</td></tr>
           </table>
           
           
       
   </div>
   </form>
    </body>
</html>


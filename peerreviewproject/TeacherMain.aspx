<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TeacherMain.aspx.cs" Inherits="peerreviewproject.TeacherMain" EnableSessionState="True" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Professor DASHBOARD</title>
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Outfit" />
    <style>
        html,body{
            margin: 0;
            height: 100%;
            font-family: "Outfit", sans-serif;
            
            background-image: linear-gradient(lightsteelblue, white);
            
            
        }

        #test1,#test2,#test3,#test4,#test5 {
            display: inline-block;
            vertical-align: top;
            
        }

        .child {
            display: flex;
            justify-content: center;
        }

        .centered {
            position: fixed;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -75%);
        }
    </style>
</head>
<body>
   <form id="form1" runat="server">
       <div class="centered">
        <table style="margin: auto">
        <tr><td><h1>USI PEER REVIEW APPLICATION</h1></td></tr>
            <tr><td><h3 style="margin: auto">TEACHER DASHBOARD</h3></td></tr>
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
               
           </table>
           <div class="row child">
                <div class="col-sm-12">
                    
                    <asp:Button ID="EditCoursesbttn" runat="server" OnClick="EditClassesbttn_Click" Text="Edit Courses" />
                    <asp:Button ID="CourseStudentsbttn" runat="server" OnClick="EditStudentsbttn_Click" Text="Edit Course Students" />
                    <asp:Button ID="CourseReviewsbttn" runat="server" OnClick="EditReviewsbttn_Click" Text="Edit Course Reviews" />
                    <asp:Button ID="Groupsbttn" runat="server" OnClick="EditGroupsbttn_Click" Text="Edit Groups" />
                    
                    
                </div>
                
               </div>
           
       
   </div>
   </form>
    </body>
</html>


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
               <tr><td><asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False">
               <Columns>
                   <asp:BoundField DataField="CourseDepartment" HeaderText="Course Dept" />
                   <asp:BoundField DataField="CourseNumber" HeaderText="Course Num" />
                   <asp:BoundField DataField="CourseName" HeaderText="Course Name" />
                   <asp:HyperLinkField text="Open" navigateurl="~\DefaultNavPage.aspx" HeaderText="Test Field"/>
               </Columns>
           </asp:GridView></td></tr>
               <tr><td><asp:Button ID="logoutButton" runat="server" Text="Logout" OnClick="logoutButton_Click" /></td></tr>
           </table>
           
           
       
   </div>
   </form>
    </body>
</html>

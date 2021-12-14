<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateClassPage.aspx.cs" Inherits="peerreviewproject.CreateClassPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Outfit" />
    <style>
          html, body {
            margin: 0;
            height: 100%;
            font-family: "Outfit", sans-serif;
           
            background-image: linear-gradient(lightsteelblue, white);
            
            background-attachment: fixed;
        }

        .bg-container {
            width: 100%;
            height: 100%;
            
            box-sizing: border-box;
            background-image: url("backgroundUSI.png");
            
            background-repeat: no-repeat;
            background-position: top;
        }

        .centered {
            position: fixed;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -100%);
        }

        .center-text{
            display: flex;

            justify-content: center;

            align-items: center;
        }

        .child {
            display: flex;
            justify-content: center;
        }

    </style>
    </head>
<body>
    <form id="form1" runat="server">
        <div>
            <div class="center-text">
                <h1>CREATE A NEW COURSE</h1>
            </div>
            
            <div class="row center-text">
                <table><tr><td><h3>Current Courses</h3></td></tr></table>
            </div>
            
                <asp:GridView ID="CurrentCourseGridView" runat="server" AutoGenerateColumns="False"  CellPadding="4" DataKeyNames="courseID" DataSourceID="SqlDataSource1" EmptyDataText="No classes" ForeColor="#333333" HorizontalAlign="Center" ShowHeaderWhenEmpty="True" AllowSorting="True" OnDataBound="CurrentCourseGridView_DataBound"  OnRowDeleted="CurrentCourseGridView_RowDeleted" ToolTip="Class contains multiple Professors if underlined" GridLines="None" OnRowDeleting="CurrentCourseGridView_RowDeleting">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField DataField="courseName" HeaderText="Course" SortExpression="courseName" />
                    <asp:BoundField DataField="courseID" HeaderText="courseID" InsertVisible="False" ReadOnly="True" SortExpression="courseID" Visible="False" />
                    <asp:BoundField DataField="courseNumber" HeaderText="Course Number" SortExpression="courseNumber" >
                    <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="courseSemester" HeaderText="Semester" SortExpression="courseSemester" >
                    <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="year" HeaderText="Year" SortExpression="year">
                    <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:CommandField ShowDeleteButton="True" />
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
            
            
            <br />
            <br />
            <asp:Table ID="Table1" runat="server" GridLines="Both" Width="696px" Height="385px" BorderStyle="Double" HorizontalAlign="Center">
                <asp:TableRow runat="server">
                    <asp:TableCell runat="server">
                        <asp:Label ID="CollegeLabel" runat="server" Text="College"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell runat="server">
                        <asp:DropDownList ID="CollegeDroplist" runat="server">
                            <asp:ListItem>College of Liberal Arts</asp:ListItem>
                            <asp:ListItem>College of Nursing & Health Professions</asp:ListItem>
                            <asp:ListItem>Pott College of Science/Engineering/Education</asp:ListItem>
                            <asp:ListItem>Romain College of Business</asp:ListItem>
                            <asp:ListItem>University Division</asp:ListItem>
                        </asp:DropDownList>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server">
                    <asp:TableCell runat="server">
                        <asp:Label ID="CourseNumLabel" runat="server" Text="Course Number"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell runat="server">
                        <asp:TextBox ID="CourseNumberTB"  runat="server" MaxLength="49"></asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server">
                    <asp:TableCell runat="server">
                        <asp:Label ID="NameLabel" runat="server" Text="Course Name"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell runat="server">
                        <asp:TextBox ID="CourseNameTB" runat="server"></asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
                                <asp:TableRow runat="server">
                    <asp:TableCell runat="server">
                        <asp:Label ID="SemesterLabel" runat="server" Text="Semester"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell runat="server">
                        <asp:DropDownList ID="SemesterDropDown" runat="server">
                            <asp:ListItem>Spring</asp:ListItem>
                            <asp:ListItem>First Summer</asp:ListItem>
                            <asp:ListItem>Second Summer</asp:ListItem>
                            <asp:ListItem>Fall</asp:ListItem>
                        </asp:DropDownList>
                    </asp:TableCell>
                </asp:TableRow>
                                                <asp:TableRow runat="server">
                    <asp:TableCell runat="server">
                        <asp:Label ID="YearLabel" runat="server" Text="Year"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell runat="server">
                        <asp:DropDownList ID="YearDropList" runat="server"></asp:DropDownList>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server">
                    <asp:TableCell runat="server">
                        <asp:Label ID="AnotherProfessorLabel" runat="server" Text="If Co-Teaching enter other Professor's email"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell runat="server">
                        <asp:TextBox ID="AnotherProfessorTextBox" runat="server" TextMode="Email" MaxLength="49"></asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            
            
            
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Peer_ReviewConnectionString2 %>" DeleteCommand="DELETE FROM Course_access_table WHERE(courseID = @courseID) AND(userID = @userID)" SelectCommand="SELECT a.courseName, a.courseID, a.courseNumber, a.courseSemester, a.year FROM Course_table AS a INNER JOIN Course_access_table AS b ON a.courseID = b.courseID WHERE (b.userID = @user )">
                <DeleteParameters>
                    <asp:Parameter Name="courseID" />
                    <asp:Parameter Name="userID" />
                </DeleteParameters>
                <SelectParameters>
                    <asp:SessionParameter Name="user" SessionField="userID" />
                </SelectParameters>
            </asp:SqlDataSource>
            
            
           <div class="row child">
               <div class="col-sm-12">
                   <asp:Label ID="WarningLabel" runat="server" BackColor="White" Visible="False"></asp:Label>
            
            
            <asp:Button ID="HomeBttn" runat="server" OnClick="HomeBttn_Click" Text="Home" />
            
            &nbsp;&nbsp;&nbsp;
            
            <asp:Button ID="CreateCourse_btn" runat="server" Text="Create Course" OnClick="CreateCourse_btnclick"/>
               </div>

           </div> 
            
            
            
            
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            
            
            
            
        </div>
    </form>
</body>
</html>

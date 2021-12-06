<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddUserstoClass.aspx.cs" Inherits="peerreviewproject.AddUserstoClass" %>

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
             <h1>ADD STUDENTS TO CLASS</h1>
            </div>
            <div class="center-text">
                <h3>Select the students you would like to add below</h3>
            </div>
        
        </div>
       
        <div>
            
            <br />
           
            
            <br />
            <div class="center-text">
                <asp:DropDownList ID="CourseAvailable_dropdownlist" runat="server" DataSourceID="Peer_review_datasource" DataTextField="courseName" DataValueField="courseID" AutoPostBack="True" OnSelectedIndexChanged="Dropdownchange" ToolTip="View Current Classes">
            </asp:DropDownList>
                
                
            <br />
            <br />
             

            </div>
            
                
            <br />
             
            
            </div>
            <div class="row center-text">

                 <asp:Label ID="CurrentRoster_lbl" runat="server" Font-Bold="True"></asp:Label>

                </div>
        <br />
        <br />
            
            
            
            <asp:Table ID="Table3" runat="server" HorizontalAlign="Center">
                <asp:TableRow runat="server">
                    <asp:TableCell runat="server"><asp:GridView ID="StudentsToAddGridview" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" Height="283px" Width="676px" AutoGenerateEditButton="True" OnRowEditing="StudentsToAddGridview_RowEditing" OnRowCancelingEdit="StudentToAddGridView_RowCancelingEdit" OnRowDataBound="StudentsToAddGridview_RowDataBound" OnRowDeleting="StudentsToAddGridview_RowDeleting" OnRowUpdating="StudentsToAddGridview_RowUpdating" Caption="Students to be added" CaptionAlign="Top" Font-Bold="True" HorizontalAlign="Right" EmptyDataText=" " CssClass="auto-style3" BorderStyle="Groove">
<AlternatingRowStyle BackColor="White" />
<Columns>
<asp:BoundField DataField="First Name" HeaderText="First Name" NullDisplayText="insert" />
<asp:BoundField DataField="Last Name" HeaderText="Last Name" />
<asp:BoundField DataField="Email" HeaderText="Email" />
    <asp:BoundField DataField="Team"></asp:BoundField>
    <asp:TemplateField ShowHeader="False"><ItemTemplate>
                                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete" Text="Remove"></asp:LinkButton>
                                    
</ItemTemplate>
</asp:TemplateField>
</Columns>

<EditRowStyle BackColor="#7C6F57" HorizontalAlign="Right" />

<EmptyDataRowStyle BackColor="#E3EAEB" BorderStyle="Double" HorizontalAlign="Right" />

<FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />

<HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />

<PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />

<RowStyle BackColor="#E3EAEB" />

<SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" HorizontalAlign="Right" />

<SortedAscendingCellStyle BackColor="#F8FAFA" />

<SortedAscendingHeaderStyle BackColor="#246B61" />

<SortedDescendingCellStyle BackColor="#D4DFE1" />

<SortedDescendingHeaderStyle BackColor="#15524A" />
</asp:GridView>
</asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server" HorizontalAlign="Center">
                    <asp:TableCell runat="server"><asp:Button ID="StudentToClass_bttn" runat="server" Onclick="StudentToClass_bttnClick" Text="Place students in class" BorderStyle="NotSet" />
</asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            
            
            
            <br />
            
            <asp:GridView ID="CurrentRosterGridView" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" HorizontalAlign="Center" EmptyDataText="No current students" OnRowDeleting="CurrentRosterGridView_RowDeleting" CellPadding="4" DataKeyNames="userID" ForeColor="#333333" GridLines="None" ShowHeaderWhenEmpty="True" OnRowDataBound="CurrentRosterGridview_RowDataBound">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField DataField="Student" HeaderText="Student" ReadOnly="True" SortExpression="Student" />
                    <asp:BoundField DataField="userID" HeaderText="userID" SortExpression="userID" Visible="False" />
                    
                    <asp:TemplateField ShowHeader="False"><ItemTemplate>
                                        <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Delete" Text="Remove from class"></asp:LinkButton>
                                    
</ItemTemplate>
</asp:TemplateField>
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
            
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            
            <br />
            <asp:SqlDataSource ID="Peer_review_datasource" runat="server" ConnectionString="<%$ ConnectionStrings:Peer_ReviewConnectionString2 %>" SelectCommand="SELECT DISTINCT a.courseName, a.courseID FROM Course_table AS a INNER JOIN Course_access_table AS b ON a.courseID = b.courseID WHERE (b.userID = @user )">
                <SelectParameters>
                    <asp:SessionParameter Name="user" SessionField="userID" />
                </SelectParameters>
            </asp:SqlDataSource>
            
            
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            
            
            
            
            
            
            
            <br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

            &nbsp;
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="SuccessLabel" runat="server" ForeColor="#009933"></asp:Label>
            <br />
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Peer_ReviewConnectionString2 %>" SelectCommand="SELECT User_table.firstName + ' ' + User_table.lastName AS Student, Course_access_table.userID FROM User_table INNER JOIN Course_access_table ON User_table.ID = Course_access_table.userID INNER JOIN Course_table ON Course_table.courseID = Course_access_table.courseID WHERE (User_table.type = 'Student') AND (Course_table.courseID = @courseID)" DeleteCommand="_removeStudentFromClass" DeleteCommandType="StoredProcedure">
                <DeleteParameters>
                    <asp:Parameter Name="userID" />
                    <asp:Parameter Name="courseID" Type="Int32" />
                </DeleteParameters>
                <SelectParameters>
                    <asp:ControlParameter ControlID="CourseAvailable_dropdownlist" Name="courseID" PropertyName="SelectedValue" />
                </SelectParameters>
            </asp:SqlDataSource>
        
        <div class="row center-text">
            <asp:Label ID="Label11" runat="server" Font-Bold="True" Text="Enter student info to add to list"></asp:Label>
        </div>
            
            
            
            <br />
            <asp:Table ID="Table1" runat="server" Height="67px" HorizontalAlign="Center" Width="600px" CssClass="auto-style2" Font-Bold="True" Font-Size="Large" BackColor="#CCCCCC" BorderStyle="Groove">
                <asp:TableRow runat="server">
                    <asp:TableCell runat="server">
                        <asp:Label ID="Label5" runat="server" Text="First Name"></asp:Label>
                    

</asp:TableCell>
                    <asp:TableCell runat="server">
                        
                    <asp:Label ID="Last" runat="server" Text="Last Name"></asp:Label>
</asp:TableCell>
                    <asp:TableCell runat="server">
                        
                    <asp:Label ID="EmailLabel" runat="server" Text="Email"></asp:Label>
</asp:TableCell>
                    <asp:TableCell runat="server">
                        <asp:Label ID="teamlbl" runat="server" Text="Team#"></asp:Label>
                    
</asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server">
                    <asp:TableCell runat="server">
                        <asp:TextBox ID="FirstnameTB" runat="server"></asp:TextBox>
                    

</asp:TableCell>
                    <asp:TableCell runat="server">
                        <asp:TextBox ID="LastnameTB" runat="server"> </asp:TextBox>
                    

</asp:TableCell>
                    <asp:TableCell runat="server">
                        <asp:TextBox ID="EmailTB" runat="server">   </asp:TextBox>
                    

</asp:TableCell>
                    <asp:TableCell runat="server">
                        <asp:TextBox ID="TeamTB" runat="server" TextMode="Number"></asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server">
                    <asp:TableCell runat="server"></asp:TableCell>
                    <asp:TableCell runat="server">
                    </asp:TableCell>
                    <asp:TableCell runat="server"></asp:TableCell>
                    </asp:TableRow>
                <asp:TableRow runat="server">
                    
                    <asp:TableCell runat="server">
                        <asp:Button ID="AddStudent_bttn" runat="server" Text="Place student on list" OnClick="AddStudent_bttnclick" Font-Bold="False" BorderStyle="NotSet" ToolTip="Add Students Button" />
                    

</asp:TableCell>
                    <asp:TableCell runat="server"></asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            
            
            <br />
            <br />
            
            
            <div class="row center-text">
            <asp:Label ID="Label12" runat="server" Font-Bold="True" Text="Upload CSV file to insert student infomation"></asp:Label>
            </div>
            
            
            <asp:Table ID="Table2" runat="server" BackColor="#CCCCCC" Font-Bold="True" HorizontalAlign="Center" BorderStyle="Groove">
                <asp:TableRow runat="server">
                    <asp:TableCell runat="server">
                        <asp:Label ID="UploadfileLabel" runat="server" Font-Bold="True">Upload a CSV file: </asp:Label></asp:TableCell>
                    <asp:TableCell runat="server">
                        <asp:FileUpload ID="FileUpload1" runat="server" accept=".csv" /></asp:TableCell>
                    <asp:TableCell>
                        <asp:Button ID="ImportBttn" runat="server" OnClick="Import_bttn_click" Text="Import file" /></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server">
                    <asp:TableCell runat="server"></asp:TableCell>
                    <asp:TableCell runat="server">
                        <asp:Label ID="WarningLabel" runat="server" ForeColor="Red"></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            
            
        </div>
    </form>
</body>
</html>

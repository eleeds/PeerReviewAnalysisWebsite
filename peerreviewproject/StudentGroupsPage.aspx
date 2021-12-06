<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StudentGroupsPage.aspx.cs" Inherits="peerreviewproject.StudentGroupsPage" %>

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
        <div class="center-text">
            <h1>STUDENT GROUP MANAGEMENT</h1>
        </div>
        <div class="center-text">
            <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource1" DataTextField="courseName" DataValueField="courseID">
            </asp:DropDownList>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Peer_ReviewConnectionString2 %>" SelectCommand="SELECT b.courseName, b.courseID FROM Course_access_table AS a INNER JOIN Course_table AS b ON a.courseID = b.courseID WHERE (a.userID = @user )">
                <SelectParameters>
                    <asp:SessionParameter Name="user" SessionField="userID" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
        <div>
            
            
                
                <br />
                <asp:GridView ID="RosterGridView" runat="server" AutoGenerateColumns="False" CellPadding="4" DataSourceID="SqlDataSource3" HorizontalAlign="Center" EmptyDataText="No Students" ShowHeaderWhenEmpty="True" AllowSorting="True" Caption="Course Roster" DataKeyNames="ID" OnRowUpdating="RosterGridView_RowUpdating1" OnRowDataBound="RosterGridView_RowDataBound" ForeColor="#333333" Width="545px" GridLines="None">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:BoundField DataField="Student" HeaderText="Student" ReadOnly="True" SortExpression="Student" />
                        <asp:BoundField DataField="Team#" HeaderText="Team#" SortExpression="Team#" >
                        <FooterStyle HorizontalAlign="Center" />
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="ID" Visible="False" />
                        <asp:BoundField DataField="teamID" HeaderText="teamID" SortExpression="teamID" Visible="False" />
                        <asp:CommandField ShowEditButton="True" EditText="Change Team" >
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:CommandField>
                    </Columns>
                    <EditRowStyle BackColor="#2461BF" />
                    <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
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
            <asp:Table ID="Table1" runat="server" CssClass="center-text">
                <asp:TableRow runat="server">
                    <asp:TableCell>
<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" Caption="Teams" CellPadding="4" DataKeyNames="teamID" DataSourceID="SqlDataSource2" EmptyDataText="No teams for current class" GridLines="Horizontal" HorizontalAlign="Center" ShowHeaderWhenEmpty="True" Width="539px" AutoGenerateEditButton="True" OnRowUpdating="GridView1_RowUpdating" OnRowUpdated="GridView1_RowUpdated" OnRowDeleting="GridView1_RowDeleting" AllowSorting="True">
                    <Columns>
                        
                        
                        
<asp:BoundField DataField="name" HeaderText="Team Name" SortExpression="name" />
                        
<asp:CommandField ShowDeleteButton="True" />
                    
</Columns>
                    

<FooterStyle BackColor="White" ForeColor="#333333" />
                    

<HeaderStyle BackColor="#336666" Font-Bold="True" ForeColor="White" />
                    

<PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
                    

<RowStyle BackColor="White" ForeColor="#333333" />
                    

<SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
                    

<SortedAscendingCellStyle BackColor="#F7F7F7" />
                    

<SortedAscendingHeaderStyle BackColor="#487575" />
                    

<SortedDescendingCellStyle BackColor="#E5E5E5" />

<SortedDescendingHeaderStyle BackColor="#275353" />
                
</asp:GridView>
                    </asp:TableCell>
                    <asp:TableCell runat="server">
                    </asp:TableCell>
                    <asp:TableCell runat="server"></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server">
                    <asp:TableCell>
                        <asp:TextBox ID="TextBox1" runat="server"  TextMode="Number"></asp:TextBox>
                
<asp:Button ID="AddTeamBttn" runat="server" OnClick="AddTeamBttn_click" Text="Add team" />
                        <div class="row">
                            <asp:Label ID="Label1" runat="server" Text="Label" ForeColor="MediumSeaGreen" Visible="False"></asp:Label>
                        </div>
                    
</asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <br />
            
            <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Peer_ReviewConnectionString2 %>" SelectCommand="SELECT { fn CONCAT(User_table.firstName, { fn CONCAT(' ', User_table.lastName) }) } AS Student, teams_table.name AS Team#, User_table.ID, UserTeam_table.teamID FROM User_table INNER JOIN UserTeam_table ON User_table.ID = UserTeam_table.userID LEFT OUTER JOIN teams_table ON UserTeam_table.teamID = teams_table.teamID WHERE (UserTeam_table.courseID = @courseID) ORDER BY 'Team#'" UpdateCommand="UPDATE UserTeam_table SET teamID = a.teamID FROM UserTeam_table CROSS JOIN (SELECT teamID FROM teams_table WHERE (name = @name)) AS a WHERE (UserTeam_table.userID = @userID)">
                <SelectParameters>
                    <asp:ControlParameter ControlID="DropDownList1" Name="courseID" PropertyName="SelectedValue" Type="Int32" />
                </SelectParameters>
                <UpdateParameters>
                    <asp:Parameter Name="name" Type="Int32" />
                    <asp:Parameter Name="userID" Type="Int32" />
                </UpdateParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Peer_ReviewConnectionString2 %>" SelectCommand="SELECT teamID, name, courseID FROM teams_table WHERE (courseID = @courseID) ORDER BY name" DeleteCommand="_deleteTeam" DeleteCommandType="StoredProcedure" InsertCommand="_insertTeam" InsertCommandType="StoredProcedure" UpdateCommand="UPDATE teams_table SET name = @name WHERE (courseID = @courseID) AND (name = @old)">
                <DeleteParameters>
                    <asp:Parameter Name="teamID" Type="Int32" />
                </DeleteParameters>
                <InsertParameters>
                    <asp:Parameter Name="courseID" Type="Int32" />
                    <asp:Parameter Name="name" Type="Int32" />
                </InsertParameters>
                <SelectParameters>
                    <asp:ControlParameter ControlID="DropDownList1" Name="courseID" PropertyName="SelectedValue" Type="Int32" />
                </SelectParameters>
                <UpdateParameters>
                    <asp:Parameter Name="name" Type="Int32" />
                    <asp:Parameter Name="courseID" />
                    <asp:Parameter Name="old"></asp:Parameter>
                </UpdateParameters>
            </asp:SqlDataSource>
        </div>
    </form>
</body>
</html>

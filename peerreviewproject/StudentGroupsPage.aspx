<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StudentGroupsPage.aspx.cs" Inherits="peerreviewproject.StudentGroupsPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    </head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource1" DataTextField="courseName" DataValueField="courseID">
            </asp:DropDownList>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Peer_ReviewConnectionString2 %>" SelectCommand="SELECT b.courseName, b.courseID FROM Course_access_table AS a INNER JOIN Course_table AS b ON a.courseID = b.courseID WHERE (a.userID = @user )">
                <SelectParameters>
                    <asp:SessionParameter Name="user" SessionField="userID" />
                </SelectParameters>
            </asp:SqlDataSource>
            
                
                <br />
                <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False" CellPadding="4" DataSourceID="SqlDataSource3" HorizontalAlign="Center" EmptyDataText="No Students" ShowHeaderWhenEmpty="True" AllowSorting="True" Caption="Class Roster" DataKeyNames="ID" OnRowUpdating="GridView3_RowUpdating1" OnRowDataBound="GridView3_RowDataBound" ForeColor="#333333" Width="545px">
                    <Columns>
                        <asp:BoundField DataField="Student" HeaderText="Student" ReadOnly="True" SortExpression="Student" />
                        <asp:BoundField DataField="Team#" HeaderText="Team#" SortExpression="Team#" >
                        <FooterStyle HorizontalAlign="Center" />
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="ID" Visible="False" />
                        <asp:BoundField DataField="teamID" HeaderText="teamID" SortExpression="teamID" Visible="False" />
                        <asp:CommandField ShowEditButton="True" EditText="Change Team" />
                    </Columns>
                    <EditRowStyle BackColor="#7C6F57" />
                    <FooterStyle BackColor="#1C5E55" ForeColor="White" Font-Bold="True" />
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
            <asp:Table ID="Table1" runat="server">
                <asp:TableRow runat="server">
                    <asp:TableCell>
<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" Caption="Teams" CellPadding="4" DataKeyNames="teamID" DataSourceID="SqlDataSource2" EmptyDataText="No teams for current class" GridLines="Horizontal" HorizontalAlign="Left" ShowHeaderWhenEmpty="True" Width="539px" AutoGenerateEditButton="True" OnRowUpdating="GridView1_RowUpdating" OnRowUpdated="GridView1_RowUpdated" OnRowDeleting="GridView1_RowDeleting" AllowSorting="True">
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
                        <asp:TextBox ID="TextBox1" runat="server"  ></asp:TextBox>
                
<asp:Button ID="AddTeamBttn" runat="server" OnClick="Button1_Click" Text="Add team" />
                    
</asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <br />
            <asp:Label ID="Label1" runat="server" Text="Label" Visible="False"></asp:Label>
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

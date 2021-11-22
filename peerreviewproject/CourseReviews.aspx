<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CourseReviews.aspx.cs" Inherits="peerreviewproject.CourseReviews" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:DropDownList ID="CourseDropDown" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource1" DataTextField="courseName" DataValueField="courseID" OnDataBound="DropDownList1_DataBound" OnSelectedIndexChanged="CourseDropDown_SelectedIndexChanged">
            </asp:DropDownList>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Peer_ReviewConnectionString2 %>" SelectCommand="SELECT Course_table.courseName, Course_table.courseID FROM Course_access_table INNER JOIN Course_table ON Course_access_table.courseID = Course_table.courseID WHERE (Course_access_table.userID = @userID) AND (Course_access_table.permissionType = @permissionType)">
                <SelectParameters>
                    <asp:SessionParameter Name="userID" SessionField="userID" Type="Int32" />
                    <asp:Parameter DefaultValue="Professor" Name="permissionType" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>
            <br />
            <asp:ListBox ID="GroupListbox" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource2" DataTextField="name" DataValueField="teamID" Width="295px" OnSelectedIndexChanged="GroupListbox_SelectedIndexChanged" OnDataBound="GroupListbox_DataBound"></asp:ListBox>
            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Peer_ReviewConnectionString2 %>" SelectCommand="SELECT [name], [teamID] FROM [teams_table] WHERE ([courseID] = @courseID2) ORDER BY [name]">
                <SelectParameters>
                    <asp:ControlParameter ControlID="CourseDropDown" Name="courseID2" PropertyName="SelectedValue" Type="Int32" />
                </SelectParameters>
            </asp:SqlDataSource>
            <br />
            <br />
        </div>
        <asp:GridView ID="GroupMembersGridview" runat="server" AutoGenerateColumns="False" DataKeyNames="ID" DataSourceID="SqlDataSource3" Caption="Reviews for selected student">
            <Columns>
                <asp:CommandField ShowSelectButton="True" />
                <asp:BoundField DataField="Student" HeaderText="Student" SortExpression="Student" ReadOnly="True" />
                <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" InsertVisible="False" ReadOnly="True" Visible="False" />
            </Columns>
            <SelectedRowStyle BackColor="#CCCCCC" />
        </asp:GridView>
        <asp:Panel ID="Panel2" runat="server" Width="1009px">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:Peer_ReviewConnectionString2 %>" SelectCommand="SELECT DISTINCT [questionSet] FROM [questions_table] WHERE ([courseID] = @courseID)">
                <SelectParameters>
                    <asp:ControlParameter ControlID="CourseDropDown" Name="courseID" PropertyName="SelectedValue" Type="Int32" />
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:GridView ID="ResultsGridview" runat="server" AutoGenerateColumns="False" DataKeyNames="responseID,type" DataSourceID="SqlDataSource4" EmptyDataText="Select student to see reviews" OnDataBound="ResultsGridview_DataBound" ShowHeaderWhenEmpty="True" Font-Bold="False">
                <Columns>
                    <asp:BoundField DataField="Student" HeaderText="Student" ReadOnly="True" SortExpression="Student" />
                    <asp:BoundField DataField="responseID" HeaderText="responseID" InsertVisible="False" ReadOnly="True" SortExpression="responseID" Visible="False" />
                    <asp:BoundField DataField="userResponse" HeaderText="Response" SortExpression="userResponse">
                    </asp:BoundField>
                    <asp:BoundField DataField="reviewQuestionID" HeaderText="reviewQuestionID" SortExpression="reviewQuestionID" Visible="False" />
                    <asp:BoundField DataField="questionName" HeaderText="Question" SortExpression="questionName">
                    </asp:BoundField>
                    <asp:BoundField DataField="questionSet" HeaderText="Set" SortExpression="questionSet">
                    <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="type" HeaderText="type" SortExpression="type" Visible="False" />
                    <asp:BoundField DataField="dateComplete" HeaderText="Date Completed" SortExpression="dateComplete">
                    <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
            &nbsp;
        </asp:Panel>
        <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:Peer_ReviewConnectionString2 %>" SelectCommand="SELECT { fn CONCAT(User_table.firstName, { fn CONCAT(' ', User_table.lastName) }) } AS Student, Response_table.responseID, Response_table.userResponse, Response_table.reviewQuestionID, questions_table.questionName, Response_table.questionSet, questions_table.type, Response_table.dateComplete FROM Response_table INNER JOIN questions_table ON Response_table.reviewQuestionID = questions_table.reviewQuestionID INNER JOIN User_table ON Response_table.userID = User_table.ID WHERE (Response_table.studentReviewed = @studentReviewed)">
            <SelectParameters>
                <asp:ControlParameter ControlID="GroupMembersGridview" Name="studentReviewed" PropertyName="SelectedValue" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Peer_ReviewConnectionString2 %>" SelectCommand="SELECT { fn CONCAT(User_table.firstName, { fn CONCAT(' ', User_table.lastName) }) } AS Student, User_table.ID FROM User_table INNER JOIN UserTeam_table ON User_table.ID = UserTeam_table.userID WHERE (UserTeam_table.teamID = @teamID)">
            <SelectParameters>
                <asp:ControlParameter ControlID="GroupListbox" Name="teamID" PropertyName="SelectedValue" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:Panel ID="Panel1" runat="server" HorizontalAlign="Center" Width="1264px">
            <br />
            <asp:GridView ID="RatingGridview" runat="server" Caption="Scores" EmptyDataText="Choose Student For Results" ShowHeaderWhenEmpty="True">
            </asp:GridView>
            <asp:GridView ID="CommentsGridview" runat="server" Caption="Comments" EmptyDataText="Choose Student For Results" ShowHeaderWhenEmpty="True">
            </asp:GridView>
            <br />
        </asp:Panel>
    </form>
</body>
</html>

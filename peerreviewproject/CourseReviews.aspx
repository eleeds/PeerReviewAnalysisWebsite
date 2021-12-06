<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CourseReviews.aspx.cs" Inherits="peerreviewproject.CourseReviews" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Outfit" />
    <style>
        html, body{
            margin: 0;
            height: 100%;
            font-family: "Outfit", sans-serif;
            
            background-image: linear-gradient(lightsteelblue, white);
            
            
           
            
        }

        .centered {
            position: fixed;
            
            left: 30%;
            padding-top: 150px;
            
        }

        .aside{
            position: fixed;
            
            left: 46%;
            padding-top: 150px;
        }

        .centered-text {
            position: fixed;
            align-content: center;
            left: 40%;
            padding-top: 50px;
            
        }

        
    </style>
    
</head>
<body>
    <form id="form1" runat="server">
        <table><tr><td><h1 class="centered-text">COURSE REVIEWS</h1></td></tr></table>
        <div class="centered">
            <table><tr><td><h3>Course Selection</h3></td></tr></table>
            <asp:DropDownList ID="CourseDropDown" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource1" DataTextField="courseName" DataValueField="courseID" OnDataBound="DropDownList1_DataBound" OnSelectedIndexChanged="CourseDropDown_SelectedIndexChanged">
            </asp:DropDownList>
            <div class="row"></div>
            <table><tr><td><h3>Team Selection</h3></td></tr></table>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4" DataKeyNames="teamID" DataSourceID="SqlDataSource2" ForeColor="#333333" GridLines="None" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:CommandField SelectText="View" ShowSelectButton="True" />
                    <asp:BoundField DataField="name" HeaderText="Team Name" SortExpression="name">
                    <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="teamID" HeaderText="teamID" InsertVisible="False" ReadOnly="True" SortExpression="teamID" Visible="False" />
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
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Peer_ReviewConnectionString2 %>" SelectCommand="SELECT Course_table.courseName, Course_table.courseID FROM Course_access_table INNER JOIN Course_table ON Course_access_table.courseID = Course_table.courseID WHERE (Course_access_table.userID = @userID) AND (Course_access_table.permissionType = @permissionType)">
                <SelectParameters>
                    <asp:SessionParameter Name="userID" SessionField="userID" Type="Int32" />
                    <asp:Parameter DefaultValue="Professor" Name="permissionType" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>
            <br />
            <asp:ListBox ID="GroupListbox" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource2" DataTextField="name" DataValueField="teamID" Width="295px" OnSelectedIndexChanged="GroupListbox_SelectedIndexChanged" Visible="False"></asp:ListBox>
            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Peer_ReviewConnectionString2 %>" SelectCommand="SELECT [name], [teamID] FROM [teams_table] WHERE ([courseID] = @courseID2) ORDER BY [name]">
                <SelectParameters>
                    <asp:ControlParameter ControlID="CourseDropDown" Name="courseID2" PropertyName="SelectedValue" Type="Int32" />
                </SelectParameters>
            </asp:SqlDataSource>
            <br />
            <div class="row">

            </div>
            <table><tr><td><h3>Survey Selection</h3></td></tr></table>
            <asp:Panel ID="Panel3" runat="server" HorizontalAlign="Right" Width="225px">
                <asp:ListBox ID="CourseSurveyListBox" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource6" DataTextField="questionSet" DataValueField="questionSet" OnSelectedIndexChanged="CourseSurveyListBox_SelectedIndexChanged" Width="237px"></asp:ListBox>
            </asp:Panel>
        </div>
        <div class="aside">
            <asp:GridView ID="GroupMembersGridview" runat="server" AutoGenerateColumns="False" DataKeyNames="ID" DataSourceID="SqlDataSource3" CellPadding="4" ForeColor="#333333" GridLines="None">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:CommandField SelectText="View" ShowSelectButton="True" />
                <asp:BoundField DataField="Student" HeaderText="Student" SortExpression="Student" ReadOnly="True" />
                <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" InsertVisible="False" ReadOnly="True" Visible="False" />
                <asp:BoundField DataField="courseID" HeaderText="courseID" SortExpression="courseID" Visible="False" />
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
        <asp:SqlDataSource ID="SqlDataSource7" runat="server" ConnectionString="<%$ ConnectionStrings:Peer_ReviewConnectionString2 %>" SelectCommand="SELECT questions_table.questionName, FORMAT(CONVERT (float, COUNT(Response_table.userResponse) / (numberOfSubmits.number * .1 * 10)), 'P') AS Votes, questions_table.question, Response_table.userResponse FROM questions_table INNER JOIN Response_table ON questions_table.reviewQuestionID = Response_table.reviewQuestionID CROSS JOIN (SELECT COUNT(userID) AS number FROM Response_table AS Response_table_1 WHERE (questionSet = @questionSet)) AS numberOfSubmits WHERE (questions_table.questionSet = @questionSet) GROUP BY questions_table.questionName, numberOfSubmits.number, questions_table.question, Response_table.userResponse">
            <SelectParameters>
                <asp:ControlParameter ControlID="CourseSurveyListBox" Name="questionSet" PropertyName="SelectedValue" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSource6" runat="server" ConnectionString="<%$ ConnectionStrings:Peer_ReviewConnectionString2 %>" SelectCommand="SELECT DISTINCT questionSet FROM questions_table WHERE (classSurvey = 'True') AND (courseID = @courseID)">
            <SelectParameters>
                <asp:ControlParameter ControlID="CourseDropDown" Name="courseID" PropertyName="SelectedValue" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:GridView ID="ClassSurveyGridView" runat="server" Caption="Class Survey" CellPadding="4" EmptyDataText="No results yet" ForeColor="#333333" OnRowDataBound="ClassSurveyGridView_RowDataBound">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
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
        <asp:Panel ID="Panel2" runat="server" Width="1009px">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:Peer_ReviewConnectionString2 %>" SelectCommand="SELECT DISTINCT [questionSet] FROM [questions_table] WHERE ([courseID] = @courseID)">
                <SelectParameters>
                    <asp:ControlParameter ControlID="CourseDropDown" Name="courseID" PropertyName="SelectedValue" Type="Int32" />
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:GridView ID="ResultsGridview" runat="server" AutoGenerateColumns="False" DataKeyNames="responseID,type" DataSourceID="SqlDataSource4" EmptyDataText="Select student to see reviews" OnDataBound="ResultsGridview_DataBound" ShowHeaderWhenEmpty="True" Font-Bold="False" CellPadding="4" ForeColor="#333333" Visible="False" AllowPaging="True">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField DataField="Student" HeaderText="Reviewed By" ReadOnly="True" SortExpression="Student" />
                    <asp:BoundField DataField="responseID" HeaderText="responseID" InsertVisible="False" ReadOnly="True" SortExpression="responseID" Visible="False" />
                    <asp:BoundField DataField="userResponse" HeaderText="Response" SortExpression="userResponse">
                    <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="reviewQuestionID" HeaderText="reviewQuestionID" SortExpression="reviewQuestionID" Visible="False" />
                    <asp:BoundField DataField="questionName" HeaderText="Question" SortExpression="questionName">
                    </asp:BoundField>
                    <asp:BoundField DataField="questionSet" HeaderText="Set" SortExpression="questionSet">
                    <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="type" HeaderText="Type" SortExpression="type" />
                    <asp:BoundField DataField="dateComplete" HeaderText="Date Complete" SortExpression="dateComplete">
                    <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
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
            &nbsp;
        </asp:Panel>
        <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:Peer_ReviewConnectionString2 %>" SelectCommand="SELECT { fn CONCAT(User_table.firstName, { fn CONCAT(' ', User_table.lastName) }) } AS Student, Response_table.responseID, Response_table.userResponse, Response_table.reviewQuestionID, questions_table.questionName, Response_table.questionSet, questions_table.type, Response_table.dateComplete FROM Response_table INNER JOIN questions_table ON Response_table.reviewQuestionID = questions_table.reviewQuestionID INNER JOIN User_table ON Response_table.userID = User_table.ID WHERE (Response_table.studentReviewed = @studentReviewed) AND (questions_table.courseID = @courseID)">
            <SelectParameters>
                <asp:ControlParameter ControlID="GroupMembersGridview" Name="studentReviewed" PropertyName="SelectedValue" />
                <asp:ControlParameter ControlID="CourseDropDown" Name="courseID" PropertyName="SelectedValue" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Peer_ReviewConnectionString2 %>" SelectCommand="SELECT { fn CONCAT(User_table.firstName, { fn CONCAT(' ', User_table.lastName) }) } AS Student, User_table.ID, UserTeam_table.courseID FROM User_table INNER JOIN UserTeam_table ON User_table.ID = UserTeam_table.userID WHERE (UserTeam_table.teamID = @teamID) AND (UserTeam_table.courseID = @courseID)">
            <SelectParameters>
                <asp:ControlParameter ControlID="GridView1" Name="teamID" PropertyName="SelectedValue" />
                <asp:ControlParameter ControlID="CourseDropDown" Name="courseID" PropertyName="SelectedValue" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:Panel ID="Panel1" runat="server" HorizontalAlign="Center" Width="694px">
            <br />
            <div class="row">
                <asp:GridView ID="RatingGridview" runat="server" Caption="Scores"  EmptyDataText="Choose Student For Results" ShowHeaderWhenEmpty="True" CellPadding="4" ForeColor="#333333" HorizontalAlign="Left">
                <AlternatingRowStyle BackColor="White" />
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
            
            <asp:GridView ID="CommentsGridview" runat="server" Caption="Comments" EmptyDataText="Choose Student For Results" ShowHeaderWhenEmpty="True" CellPadding="4" ForeColor="#333333" HorizontalAlign="Right">
                <AlternatingRowStyle BackColor="White" />
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
            </div>
            
            <br />
        </asp:Panel>
        </div>
        
    </form>
</body>
</html>

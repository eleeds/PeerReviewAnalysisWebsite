<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreatePeerReviewPage.aspx.cs" Inherits="peerreviewproject.CreatePeerReviewPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title></title>  
    <style type="text/css">
        .auto-style1 {
            width: 922px;
        }
        .auto-style2 {
            font-weight: bold;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <p style="font-weight: bold">
            Select Class</p>
        <p>
            <asp:ListBox ID="Course_listbox" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource1" DataTextField="courseName" DataValueField="courseID" Width="399px" OnSelectedIndexChanged="Course_listbox_SelectedIndexChanged"></asp:ListBox>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Peer_ReviewConnectionString2 %>" SelectCommand="SELECT DISTINCT a.courseName, a.courseID FROM Course_table AS a INNER JOIN Course_access_table AS b ON a.courseID = b.courseID WHERE (b.userID = @user )">
                <SelectParameters>
                    <asp:SessionParameter Name="user" SessionField="userID" />
                </SelectParameters>
            </asp:SqlDataSource>
        </p>
        <p style="font-weight: bold">
            Select existing review set or create new set</p>
        <p class="auto-style1">
            <asp:ListBox ID="CurrentQuestionSet_listbox" runat="server" Width="405px" AutoPostBack="True" OnSelectedIndexChanged="CurrentQuestionSet_listbox_SelectedIndexChanged"></asp:ListBox>
            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Peer_ReviewConnectionString2 %>" SelectCommand="SELECT DISTINCT questionSet FROM questions_table WHERE (courseID = @courseID)">
                <SelectParameters>
                    <asp:ControlParameter ControlID="Course_listbox" Name="courseID" PropertyName="SelectedValue" Type="Int32" />
                </SelectParameters>
            </asp:SqlDataSource>
        </p>
        <asp:Panel ID="Panel3" runat="server" HorizontalAlign="Center">
            <br />
        </asp:Panel>
        <asp:Panel ID="Panel2" runat="server" Enabled="False" HorizontalAlign="Right" Width="1166px">
            <asp:Label ID="dueDatelbl" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
            &nbsp;
            <asp:TextBox ID="DueDateTB" runat="server" TextMode="Date" AutoPostBack="True" OnTextChanged="DueDateTB_TextChanged" ></asp:TextBox>
            <br />
            <br />
            <br />
            <asp:Label ID="CheckBoxLabel" runat="server" Font-Bold="True" Text="Make set a Class Survey?"></asp:Label>
            <br />
            <asp:CheckBox ID="NoCheckBox" runat="server" AutoPostBack="True" Font-Bold="False" OnCheckedChanged="NoCheckBox_CheckedChanged" Text="No" />
            &nbsp;
            <asp:CheckBox ID="YesCheckBox" runat="server" AutoPostBack="True" Font-Bold="False" OnCheckedChanged="YesCheckBox_CheckedChanged" Text="Yes" />
            <br />
            <br />
            <span class="auto-style2">Show set to students?
            <br />
            </span>
            <asp:CheckBox ID="NoCheckBoxStudents" runat="server" AutoPostBack="True" Font-Bold="False" OnCheckedChanged="NoCheckBoxStudents_CheckedChanged" Text="No" />
            <asp:CheckBox ID="YesCheckBoxStudents" runat="server" AutoPostBack="True" Font-Bold="False" OnCheckedChanged="YesCheckBoxStudents_CheckedChanged" Text="Yes" />
        </asp:Panel>
        <p>
            <asp:Button ID="NewSetButton" runat="server" OnClick="NewSetButton_click" Text="Add new set" />
            <asp:Label ID="Label1" runat="server"></asp:Label>
            <asp:GridView ID="QuestionsInSetGridview" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource4" Width="1109px" AutoGenerateEditButton="True" DataKeyNames="reviewQuestionID,classSurvey" OnRowEditing="QuestionsInSetGridview_RowEditing" OnRowUpdated="QuestionsInSetGridview_RowUpdated" EmptyDataText="No questions added yet" OnRowCancelingEdit="QuestionsInSetGridview_RowCancelingEdit" OnRowDataBound="QuestionsInSetGridview_RowDataBound" OnRowUpdating="QuestionsInSetGridview_RowUpdating" OnRowDeleted="QuestionsInSetGridview_RowDeleted" OnRowDeleting="QuestionsInSetGridview_RowDeleting" CellPadding="4" ForeColor="#333333" GridLines="None">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField DataField="reviewQuestionID" HeaderText="reviewQuestionID" SortExpression="reviewQuestionID" InsertVisible="False" ReadOnly="True" Visible="False" />
                    <asp:BoundField DataField="question" HeaderText="Question" SortExpression="question" />
                    <asp:BoundField DataField="type" HeaderText="Type" SortExpression="type" />
                    <asp:BoundField DataField="correctResponses" HeaderText="Accepted Responses" SortExpression="correctResponses" /> 
                    <asp:BoundField DataField="questionName" HeaderText="Name" SortExpression="questionName" />
                    <asp:BoundField DataField="questionSet" HeaderText="Set" SortExpression="questionSet" >
                    <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:CheckBoxField DataField="classSurvey" HeaderText="classSurvey" SortExpression="classSurvey" Visible="False" >
                    </asp:CheckBoxField>
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
            <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:Peer_ReviewConnectionString2 %>" SelectCommand="SELECT questions_table.reviewQuestionID, questions_table.question, questions_table.type, questions_table.correctResponses, questions_table.questionName, questions_table.questionSet, questions_table.classSurvey FROM Course_table INNER JOIN questions_table ON Course_table.courseID = questions_table.courseID AND Course_table.courseID = questions_table.courseID WHERE (questions_table.questionSet = @questionSet) AND (questions_table.courseID = @course)" DeleteCommand="DELETE FROM [questions_table] WHERE [reviewQuestionID] = @reviewQuestionID" InsertCommand="INSERT INTO [questions_table] ([courseID], [question], [type], [correctResponses], [questionName], [questionSet]) VALUES (@courseID, @question, @type, @correctResponses, @questionName, @questionSet)" UpdateCommand="UPDATE questions_table SET question = @question, type = @type, correctResponses = @correctResponses, questionName = @questionName, questionSet = @questionSet WHERE (reviewQuestionID = @reviewQuestionID)">
                <DeleteParameters>
                    <asp:Parameter Name="reviewQuestionID" Type="Int32" />
                </DeleteParameters>
                <InsertParameters>
                    <asp:Parameter Name="courseID" Type="Int32" />
                    <asp:Parameter Name="question" Type="String" />
                    <asp:Parameter Name="type" Type="String" />
                    <asp:Parameter Name="correctResponses" Type="String" />
                    <asp:Parameter Name="questionName" Type="String" />
                    <asp:Parameter Name="questionSet" Type="String" />
                </InsertParameters>
                <SelectParameters>
                    <asp:ControlParameter ControlID="CurrentQuestionSet_listbox" Name="questionSet" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="Course_listbox" Name="course" PropertyName="SelectedValue" />
                </SelectParameters>
                <UpdateParameters>
                    <asp:Parameter Name="question" />
                    <asp:Parameter Name="type" />
                    <asp:Parameter Name="correctResponses" />
                    <asp:Parameter Name="questionName" />
                    <asp:Parameter Name="questionSet" />
                    <asp:Parameter Name="reviewQuestionID" Type="Int32"/>
                </UpdateParameters>
            </asp:SqlDataSource>
        </p>
        <p draggable="true">
            &nbsp;</p>
        <p style="font-weight: bold">
            <asp:TextBox ID="questDescriptionTB" runat="server" Height="100px" Width="500px" EnableTheming="True" TextMode="MultiLine"></asp:TextBox>
            Question Description</p>
        <p>
            <asp:RadioButtonList ID="type_radiobttn" runat="server" OnSelectedIndexChanged="type_radiobttn_SelectedIndexChanged" AutoPostBack="True" CausesValidation="True">
                <asp:ListItem Selected="True">Comment Response</asp:ListItem>
                <asp:ListItem Value="1-4 Score Rating">1-4 Score Rating/Multiple Choice</asp:ListItem>
                <asp:ListItem Value="1-5 Score Rating">1-5 Score Rating/Multiple Choice</asp:ListItem>
            </asp:RadioButtonList>        

        <asp:Panel ID="Panel1" runat="server" Height="150px" HorizontalAlign="Left" Visible="False" Width="400px" Font-Bold="True">
            1 <asp:TextBox ID="tb1" runat="server"></asp:TextBox>
            <br />
            2 <asp:TextBox ID="tb2" runat="server"></asp:TextBox>
            <br /> 3 <asp:TextBox ID="tb3" runat="server"></asp:TextBox>
            <br />
            4 <asp:TextBox ID="tb4" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="score5lbl" runat="server" Font-Bold="True" Text="5"></asp:Label>
            <asp:TextBox ID="tb5" runat="server"></asp:TextBox>
        </asp:Panel>

        <p>
            <asp:TextBox ID="name_tb" runat="server"></asp:TextBox>
        &nbsp;&nbsp;&nbsp; Name of Question&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="SubmitBttn" runat="server" OnClick="SubmitBttn_Click" Text="Submit" />
        </p>
        <p>
            &nbsp;</p>
        <p>
            &nbsp;</p>
        <p>
            &nbsp;</p>
        <p>
            &nbsp;</p>
        <p>
            &nbsp;
        </p>
        <p>
            &nbsp;</p>
    </form>
</body>
</html>

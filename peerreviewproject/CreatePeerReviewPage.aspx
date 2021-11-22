<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreatePeerReviewPage.aspx.cs" Inherits="peerreviewproject.CreatePeerReviewPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title></title>  
    <style type="text/css">
        .auto-style1 {
            width: 922px;
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
        <asp:Panel ID="Panel2" runat="server" Enabled="False" HorizontalAlign="Right" Width="1166px">
            <asp:Label ID="dueDatelbl" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
            &nbsp;
            <asp:TextBox ID="DueDateTB" runat="server" TextMode="Date" AutoPostBack="True" OnTextChanged="DueDateTB_TextChanged" ></asp:TextBox>
        </asp:Panel>
        <p>
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click1" Text="Add new set" />
            <asp:Label ID="Label1" runat="server"></asp:Label>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource4" Width="1109px" AutoGenerateEditButton="True" DataKeyNames="reviewQuestionID" OnRowEditing="GridView1_RowEditing" OnRowUpdated="GridView1_RowUpdated" EmptyDataText="No questions added yet" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowDataBound="GridView1_RowDataBound" OnRowUpdating="GridView1_RowUpdating" OnRowDeleted="GridView1_RowDeleted" OnRowDeleting="GridView1_RowDeleting">
                <Columns>
                    <asp:BoundField DataField="reviewQuestionID" HeaderText="reviewQuestionID" SortExpression="reviewQuestionID" InsertVisible="False" ReadOnly="True" Visible="False" />
                    <asp:BoundField DataField="question" HeaderText="Description" SortExpression="question" />
                    <asp:BoundField DataField="type" HeaderText="Type" SortExpression="type" />
                    <asp:BoundField DataField="correctResponses" HeaderText="Accepted Responses" SortExpression="correctResponses" /> 
                    <asp:BoundField DataField="questionName" HeaderText="Name" SortExpression="questionName" />
                    <asp:BoundField DataField="questionSet" HeaderText="Set" SortExpression="questionSet" />
                    <asp:CommandField ShowDeleteButton="True" />
                </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:Peer_ReviewConnectionString2 %>" SelectCommand="SELECT questions_table.reviewQuestionID, questions_table.question, questions_table.type, questions_table.correctResponses, questions_table.questionName, questions_table.questionSet FROM questions_table INNER JOIN Course_table ON questions_table.courseID = Course_table.courseID AND questions_table.courseID = Course_table.courseID WHERE (questions_table.questionSet = @questionSet) AND (questions_table.courseID = @course)" DeleteCommand="DELETE FROM [questions_table] WHERE [reviewQuestionID] = @reviewQuestionID" InsertCommand="INSERT INTO [questions_table] ([courseID], [question], [type], [correctResponses], [questionName], [questionSet]) VALUES (@courseID, @question, @type, @correctResponses, @questionName, @questionSet)" UpdateCommand="UPDATE questions_table SET question = @question, type = @type, correctResponses = @correctResponses, questionName = @questionName, questionSet = @questionSet WHERE (reviewQuestionID = @reviewQuestionID)">
                <DeleteParameters>
                    <asp:Parameter Name="reviewQuestionID" Type="Int32" />
                </DeleteParameters>
                <InsertParameters>
                    <asp:Parameter Name="courseID" Type="Int32" />
                    <asp:Parameter Name="question" Type="String" />
                    <asp:Parameter Name="type" Type="String" />
                    <asp:Parameter Name="correctResponses" Type="String" />
                    <asp:Parameter Name="questionName" Type="String" />
                    <asp:Parameter Name="questionSet" Type="Int32" />
                </InsertParameters>
                <SelectParameters>
                    <asp:ControlParameter ControlID="CurrentQuestionSet_listbox" Name="questionSet" PropertyName="SelectedValue" Type="Int32" />
                    <asp:ControlParameter ControlID="Course_listbox" Name="course" PropertyName="SelectedValue" />
                </SelectParameters>
                <UpdateParameters>
                    <asp:Parameter Name="question" />
                    <asp:Parameter Name="type" />
                    <asp:Parameter Name="correctResponses" />
                    <asp:Parameter Name="questionName" />
                    <asp:Parameter Name="questionSet" />
                    <asp:Parameter Name="reviewQuestionID" Type="Int32" />
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
                <asp:ListItem>1-4 Score Rating</asp:ListItem>
                <asp:ListItem>1-5 Score Rating</asp:ListItem>
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

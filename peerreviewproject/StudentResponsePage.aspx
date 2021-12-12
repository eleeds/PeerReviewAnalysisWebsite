<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StudentResponsePage.aspx.cs" Inherits="peerreviewproject.StudentResponsePage"  EnableSessionState="True"%>

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
            transform: translate(-50%, -75%);
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
        <div class="centered">
            <asp:Label ID="MemberNamelbl" runat="server" Font-Bold="True"></asp:Label>
            <asp:GridView ID="StudentGridview" runat="server" DataSourceID="StudentGridDataSource" AutoGenerateColumns="False" Caption="Team Members" Visible="False" DataKeyNames="userID,teamID">
                <Columns>
                    <asp:BoundField DataField="Student" HeaderText="Student" ReadOnly="True" SortExpression="Student" />
                    <asp:BoundField DataField="userID" HeaderText="userID" SortExpression="userID" Visible="False" />
                    <asp:BoundField DataField="teamID" HeaderText="teamID" SortExpression="teamID" Visible="False" />
                </Columns>
            </asp:GridView>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Questionlbl" runat="server" Font-Bold="True"></asp:Label>
            <asp:SqlDataSource ID="StudentGridDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:Peer_ReviewConnectionString2 %>" SelectCommand="SELECT { fn CONCAT(User_table.firstName, { fn CONCAT(' ', User_table.lastName) }) } AS Student, UserTeam_table.userID, UserTeam_table.teamID FROM teams_table INNER JOIN UserTeam_table ON teams_table.teamID = UserTeam_table.teamID INNER JOIN User_table ON UserTeam_table.userID = User_table.ID WHERE (teams_table.teamID = @teamID) AND (@userID NOT IN (UserTeam_table.userID))">
                <SelectParameters>
                    <asp:SessionParameter Name="teamID" SessionField="teamID" />
                    <asp:SessionParameter Name="userID" SessionField="userID" />
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:Panel ID="Panel1" runat="server" CssClass="auto-style1" HorizontalAlign="Left">
            </asp:Panel>
            <asp:TextBox ID="TextBox1" runat="server" BorderStyle="None" Font-Bold="False" Height="187px" ReadOnly="True" TextMode="MultiLine" Width="740px" EnableTheming="True" Font-Size="Large"></asp:TextBox>
            <br />
            <asp:TextBox ID="feedbackTxtbox" runat="server" Visible="False"></asp:TextBox>
            <asp:RadioButtonList ID="RadioBttns1to4" runat="server" Visible="False">
                <asp:ListItem Value="1"></asp:ListItem>
                <asp:ListItem Value="2"></asp:ListItem>
                <asp:ListItem Value="3"></asp:ListItem>
                <asp:ListItem Value="4"></asp:ListItem>
            </asp:RadioButtonList>
            <asp:RadioButtonList ID="Radiobttns1to5" runat="server" Visible="False">
                <asp:ListItem Value="1"></asp:ListItem>
                <asp:ListItem Value="2"></asp:ListItem>
                <asp:ListItem Value="3"></asp:ListItem>
                <asp:ListItem Value="4"></asp:ListItem>
                <asp:ListItem Value="5"></asp:ListItem>
            </asp:RadioButtonList>
            <asp:RadioButtonList ID="RadiobttnsYesorNo" runat="server" Visible="False">
                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                <asp:ListItem Value="No">No</asp:ListItem>
            </asp:RadioButtonList>
            <br />
            <asp:Button ID="Button1" runat="server" Text="Submit" OnClick="SubmitBttnClick" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="Button2" runat="server" Text="Home" OnClick="HomeBttnClick" />
            <br />
            <br />
&nbsp;<asp:Label ID="Errorlbl" runat="server" Visible="False" ForeColor="Red"></asp:Label>
            <br />
        </div>
    </form>
</body>
</html>

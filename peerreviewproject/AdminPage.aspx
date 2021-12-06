<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminPage.aspx.cs" Inherits="peerreviewproject.AdminPage" %>

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
        <div class="row" style="padding-top: 50px">
            <div class="center-text">
                <h1>ADMINISTRATION MANAGEMENT</h1>
            </div>
        </div>
        <div class="row" style="padding-top: 125px">

        </div>
        <div>
            <asp:Table ID="Table1" runat="server" GridLines="Both" Width="400px" Height="150px" BorderStyle="Double" HorizontalAlign="Center">

                <asp:TableRow runat="server">
                    <asp:TableCell runat="server">
                        <asp:Label ID="ProfessorFirstNameLabel" runat="server" Text="Professor First Name"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell runat="server">
                        <asp:TextBox ID="FirstNameTB"  runat="server"></asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server">
                    <asp:TableCell runat="server">
                        <asp:Label ID="ProfessorLastNameLabel" runat="server" Text="Professor Last Name"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell runat="server">
                        <asp:TextBox ID="LastNameTB" runat="server"></asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
                                <asp:TableRow runat="server">
                    <asp:TableCell runat="server">
                        <asp:Label ID="EmailLabel" runat="server" Text="Professor Email"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell runat="server">
                        <asp:TextBox ID="EmailTB" runat="server" TextMode="Email"></asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            
            
            <br />
            <div class="row">
                <div class="center-text">
                <asp:Button ID="CreateUserButton" runat="server" OnClick="CreateUserButton_Click" Text="Create User" />
                    </div>
            </div>
            
            
            
            
            <br />
            <asp:GridView ID="UserGridview" runat="server" AllowPaging="True" AllowSorting="True" Caption="Users" CellPadding="4" ForeColor="#333333" GridLines="None" HorizontalAlign="Center" OnPageIndexChanging="OnPaging" OnRowDeleting="UserGridView_RowDeleting">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
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
            
            <div class="row">
                <div class="center-text">
                    <asp:TextBox ID="txtSearch" runat="server" OnTextChanged="Search" AutoPostBack="true">Search box</asp:TextBox>

                </div>
            </div>
            


            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Caption="Ignore for now" CellPadding="4" DataKeyNames="ID" DataSourceID="SqlDataSource1" EmptyDataText="Nothing" ForeColor="#333333" HorizontalAlign="Right" OnRowDeleted="GridView1_RowDeleted" ShowHeaderWhenEmpty="True" OnRowDeleting="GridView1_RowDeleting" AllowPaging="True" AllowSorting="True" Visible="False">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField DataField="Name" HeaderText="Name" ReadOnly="True" SortExpression="Name" />
                    <asp:BoundField DataField="email" HeaderText="email" SortExpression="email" />
                    <asp:BoundField DataField="type" HeaderText="type" SortExpression="type" />
                    <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="ID" />
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
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Peer_ReviewConnectionString2 %>" DeleteCommand="DELETE FROM User_table WHERE (ID = @ID)" SelectCommand="SELECT { fn CONCAT(firstName, { fn CONCAT(' ', lastName) }) } AS Name, email, type, ID FROM User_table WHERE (type NOT IN ('Admin')) ORDER BY type">
                <DeleteParameters>
                    <asp:Parameter Name="ID" />
                </DeleteParameters>
            </asp:SqlDataSource>
            <br />
            
            
            
        </div>
    </form>
</body>
</html>

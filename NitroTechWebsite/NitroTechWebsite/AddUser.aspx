<%@ Page Title="Account" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddUser.aspx.cs" Inherits="NitroTechWebsite.AddUser" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <style>
        .form-container {
            max-width: 800px;
            margin: 50px auto;
            padding: 20px;
            background: #191919;
            border-radius: 10px;
            box-shadow: 0px 4px 8px rgba(0,0,0,0.1);
        }

        .form-container h2,
        .form-container h3 {
            text-align: center;
            margin-bottom: 20px;
            color: white;
        }

        .form-group {
            display: flex;
            align-items: center;
            margin-bottom: 15px;
            max-width: 600px;
            margin-left: auto;
            margin-right: auto;
        }

        .form-group label {
            width: 250px;
            font-weight: bold;
            text-align: right;
            margin-right: 75px;
            color: white;
        }

        .form-group input {
            flex: 1;
            padding: 8px;
            border: 1px solid #ccc;
            border-radius: 5px;
            color: black;
        }

        .form-group select {
            flex: 1;
            padding: 8px;
            border: 1px solid #ccc;
            border-radius: 6px;
            font-size: 14px;
            width: 100%;
            color: black;
            background-color: white;
        }

        .form-group select option[value=""] {
            color: gray;
        }

        .btn-purple {
            display: block;
            padding: 12px 30px;
            margin: 30px auto 10px auto;
            background: transparent;
            color: white;
            border: 2px solid #3c00a0;
            border-radius: 5px;
            font-size: 16px;
            letter-spacing: 2px;
            cursor: pointer;
            transition: 0.5s;
            text-transform: uppercase;
        }

        .btn-purple:hover {
            background-color: #3c00a0;
            color: #fff;
            box-shadow: 0 0 5px #3c00a0,
                        0 0 25px #3c00a0,
                        0 0 50px #3c00a0,
                        0 0 100px #3c00a0;
        }

        .text-align {
            font-weight: bold;
            text-align: center;
            color: grey;
        }

        .text-align:hover {
            color: purple;
        }
    </style>

    <div class="form-container">
        <h2><%: Title %></h2>
        <h3>Add a User</h3>

        <div class="form-group">
            <label for="txtUsername">Username:</label>
            <asp:TextBox ID="txtUsername" runat="server" />
        </div>

        <div class="form-group">
            <label for="ddlRoles">Role:</label>
            <asp:DropDownList ID="ddlRoles" runat="server">
                <asp:ListItem Value="">-- Select a Role --</asp:ListItem>
                <asp:ListItem Value="ROLE001">Administrator</asp:ListItem>
                <asp:ListItem Value="ROLE002">Clerk</asp:ListItem>
                <asp:ListItem Value="ROLE003">Manager</asp:ListItem>
                <asp:ListItem Value="ROLE004">Director</asp:ListItem>
            </asp:DropDownList>
        </div>

        <div class="form-group">
            <label for="txtPassword">Password:</label>
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" />
        </div>

        <div class="form-group">
            <label for="txtConfirmPassword">Confirm Password:</label>
            <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" />
        </div>

        <div class="form-group">
            <label for="txtSecurityQuestion">Security Question:</label>
            <asp:TextBox ID="txtSecurityQuestion" runat="server" />
        </div>

        <div class="form-group">
            <label for="txtSecurityAnswer">Security Answer:</label>
            <asp:TextBox ID="txtSecurityAnswer" runat="server" />
        </div>

        <asp:Button ID="btnAddUser" runat="server" Text="Add User" CssClass="btn-purple" OnClick="btnAddUser_Click" />

        <div style="text-align:center; margin-top:20px;">
            <asp:Label ID="lblMessage" runat="server" ForeColor="White" />
        </div>
    </div>

    <script>
        function togglePassword() {
            var pwd = document.getElementById('<%= txtPassword.ClientID %>');
            var confirmPwd = document.getElementById('<%= txtConfirmPassword.ClientID %>');
            if (pwd.type === "password") {
                pwd.type = "text";
                confirmPwd.type = "text";
            } else {
                pwd.type = "password";
                confirmPwd.type = "password";
            }
        }
    </script>
</asp:Content>

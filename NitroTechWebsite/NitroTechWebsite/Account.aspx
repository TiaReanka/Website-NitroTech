<%@ Page Title="Account" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Account.aspx.cs" Inherits="NitroTechWebsite.Account" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .form-container {
            max-width: 800px;       /* limit width so it doesn't stretch */
            margin: 50px auto;      /* center horizontally and add space on top */
            padding: 20px;
            background: #191919;    /* light background */
            border-radius: 10px;
            box-shadow: 0px 4px 8px rgba(0,0,0,0.1);
        }

        .form-container h2,
        .form-container h3 {
            text-align: center;
            margin-bottom: 20px;
        }

        .form-group {
            display: flex;
            align-items: center;
            margin-bottom: 15px;
            max-width: 600px;       /* shrink form fields area */
            margin-left: auto;      /* center horizontally */
            margin-right: auto;
        }

        .form-group label {
            width: 250px;          /* same width for all labels */
            font-weight: bold;
            text-align: right;      /* align text close to input */
            margin-right: 75px;
        }

        .form-group input {
            flex: 1;               /* take remaining space */
            padding: 8px;
            border: 1px solid #ccc;
            border-radius: 5px;
        }

        .form-container button {
            display: block;
            max-width: 600px;
            margin: 20px auto;      /* center under form */
            padding: 12px;
            background-color: #1a2db9;
            color: white;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            font-size: 16px;
        }

        .form-container button:hover {
            background-color: purple;
        }

        .text-align { /* same width for all labels */
            font-weight: bold;
            text-align: center;      /* align text close to input */
            color: grey;
        }

        .text-align:hover {
            color: purple;
        }

    </style>

    <div class="form-container">
        <h2><%: Title %></h2>
        <h3>Login</h3>

        <div class="form-group">
            <label for="Username">Username:</label>
            <input type="text" id="username" name="username" />
        </div>

        <div class="form-group">
            <label for="password">Password:</label>
            <input type="text" id="password" name="password" />
        </div>

        <button type="submit">Login</button>

        <div class="text-align">
            <asp:Label ID="newAccount" runat="server" Text="Create an Account" Font-Underline="true"></asp:Label>
        </div>

        <div class="text-align">
            <asp:Label ID="newPassword" runat="server" Text="Forgot Password?" Font-Underline="true"></asp:Label>
        </div>

    </div>
</asp:Content>
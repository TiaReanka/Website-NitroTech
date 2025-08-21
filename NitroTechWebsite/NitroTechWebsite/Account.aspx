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

        .form-group input {
            flex: 1;
            padding: 8px;
            border: 1px solid #ccc;
            border-radius: 5px;
            background: #fff;     /* white background */
            color: #000;          /* black text */
}

        .btn1 a {
            text-align: center;   /* center the anchor inside */
            margin: 20px auto;
            padding: 10px 20px;
            color: #3c00a0;
            font-size: 16px;
            text-decoration: none;
            text-transform: uppercase;
            overflow: hidden;
            transition: 0.5s;
            margin-top: 20px;
            letter-spacing: 4px;
            line-height: 20px;
            width: 15%;             /* stretch across the form */
            display: flex;           /* use flexbox */
            justify-content: center; /* center child horizontally */
            margin-top: 20px;  
        }

        .btn1 a:hover {
            background-color: #3c00a0;
            color: #fff;
            border-radius: 5px;
            box-shadow: 0 0 5px #3c00a0, 0 0 25px #3c00a0, 0 0 50px #3c00a0, 0 0 100px #3c00a0;
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

        <div class="btn1">
            <a href="#" onclick="document.forms[0].submit(); return false;">Login</a>
        </div>

        <div class="text-align">
            <asp:Label ID="newAccount" runat="server" Text="Create an Account" Font-Underline="true"></asp:Label>
        </div>

        <div class="text-align">
            <asp:Label ID="newPassword" runat="server" Text="Forgot Password?" Font-Underline="true"></asp:Label>
        </div>

    </div>
</asp:Content>
<%@ Page Title="Account" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddUser.aspx.cs" Inherits="NitroTechWebsite.AddUser" %>

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

        .form-group select {
            flex: 1;
            padding: 8px;
            border: 1px solid #ccc;
            border-radius: 6px;
            font-size: 14px;
            width: 100%;           /* same size as textbox */
        color: black;          /* default text color */
        background-color: white;
        }

        .form-group select option[value=""] {
            color: gray;
        }

        .show-password {
            margin-left: 45px; /* align with inputs */
            color: white;
        }

    </style>

    <div class="form-container">
        <h2><%: Title %></h2>
        <h3>Add a User</h3>

        <div class="form-group">
            <label for="Username">Username:</label>
            <input type="text" id="username" name="username" />
        </div>

        <div class="form-group">
            <label for="role">Role:</label>
            <select id="role" name="role">
                <option value="">-- Select a Role --</option>
                <option value="ROLE001">Administrator</option>
                <option value="ROLE002">Clerk</option>
                <option value="ROLE003">Manager</option>
                <option value="ROLE004">Director</option>
            </select>
        </div>

        <div class="form-group">
            <label for="password">Password:</label>
            <input type="text" id="password" name="password" />
        </div>

        <div class="form-group">
            <label for="confirmmPassword">Confirm Password:</label>
            <input type="text" id="confirmmPassword" name="confirmmPassword" />
        </div>

        <div class="form-group">
            <label style="width: auto; margin-left: 325px;">
                <input type="checkbox" id="showPassword" onclick="togglePassword()" />
                Show Password
            </label>
        </div>

        <div class="form-group">
            <label for="securityQ">Security Question:</label>
            <input type="text" id="securityQ" name="securityQ" />
        </div>

        <div class="form-group">
            <label for="securityA">Security Question Answer:</label>
            <input type="text" id="securityA" name="securityA" />
        </div>

        <button type="submit">Add User</button>

    </div>
</asp:Content>

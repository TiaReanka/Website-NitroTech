<%@ Page Title="Account" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Account.aspx.cs" Inherits="NitroTechWebsite.Account" %>

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
        }

            .form-group input {
                flex: 1;
                padding: 8px;
                border: 1px solid #ccc;
                border-radius: 5px;
                color: black; 
                background-color: #fff; 
            }

        .btn1 a {
            text-align: center;
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
            width: 15%; 
            display: flex; 
            justify-content: center; 
            margin-top: 20px;
            border: 2px solid #3c00a0;
            border-radius: 5px;
        }
            

        .btn1 a:hover {
            background-color: #3c00a0;
            color: #fff;
            border-radius: 5px;
            box-shadow: 0 0 5px #3c00a0, 0 0 25px #3c00a0, 0 0 50px #3c00a0, 0 0 100px #3c00a0;
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
        <h3>Login</h3>

        <div class="form-group">
            <label for="Username">Username:</label>
            <asp:TextBox ID="txtUsername" runat="server" />
        </div>

        <div class="form-group">
            <label for="password">Password:</label>
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" />
        </div>

        <div class="btn1">
            <a href="#" style="color:white" onclick="document.forms[0].submit(); return false;">Login</a>
        </div>

        <div class="text-align">
            <asp:HyperLink ID="lnkForgotPassword"
                runat="server"
                NavigateUrl="~/NewPassword.aspx"
                Text="Change Password?" />
        </div>

    </div>
</asp:Content>
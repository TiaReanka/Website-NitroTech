<%@ Page Title="Account" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NewPassword.aspx.cs" Inherits="NitroTechWebsite.NewPassword" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .form-container {
            max-width: 900px;
            margin: 50px auto;
            padding: 30px;
            background: #191919;
            border-radius: 10px;
            box-shadow: 0px 4px 8px rgba(0,0,0,0.2);
        }

            .form-container h2,
            .form-container h3 {
                text-align: center;
                font-size: 28px;
                margin-bottom: 10px;
                color: white;
            }

        .form-group {
            display: flex;
            align-items: center;
            margin-bottom: 20px;
            max-width: 700px;
            margin-left: auto;
            margin-right: auto;
        }

            .form-group label {
                width: 220px;
                font-weight: bold;
                text-align: right;
                margin-right: 50px;
                color: white;
            }

            .form-group input {
                flex: 1;
                padding: 8px;
                border: 1px solid #ccc;
                border-radius: 5px;
            }

            .form-group img {
                position: absolute;
                right: 10px;
                top: 50%;
                transform: translateY(-50%);
                cursor: pointer;
                width: 22px;
                opacity: 0.6;
                transition: opacity 0.2s ease;
                display: block; 
            }

        input[type="password"]::-ms-reveal,
        input[type="password"]::-ms-clear {
            display: block; /* don't hide them */
        }

        
        .radio-options {
            display: flex;
            flex-direction: column; /* stack vertically */
            gap: 10px;
        }

        .radio-line {
            display: flex;
            align-items: center;
        }

            .radio-line input[type="radio"] {
                margin: 0 6px 0 0; /* small gap between circle and text */
            }

        
        input[type="text"],
        input[type="password"],
        textarea {
            color: #000 !important;
            background-color: #fff; 
        }

        .form-container button {
            display: block;
            margin-left: 15px;
            padding: 12px 20px;
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

        .button-row {
            display: flex;
            justify-content: space-between;
            max-width: 450px;
            margin: 20px auto 0 auto;
        }

        .button {
            display: block;
            margin-left: 15px;
            padding: 12px 20px;
            background-color: #1a2db9;
            color: white;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            font-size: 16px;
        }

            .button:hover {
                background-color: purple;
            }


    </style>

    <div class="form-container">
        <h2><%: Title %></h2>
        <h3>Change Password</h3>

        <div class="form-group">
            <label for="username">Username:</label>
            <input type="text" id="username" runat="server" />
            <button id="loadSQButton" runat="server" onserverclick="LoadSecurityQuestion_Click">Load Security Question</button>

        </div>

        <div class="form-group">
            <asp:Label ID="oldPasswordLabel" runat="server" AssociatedControlID="oldPassword" Text="Old Password:" ForeColor="White"></asp:Label>
            <input type="password" id="oldPassword" runat="server" />
            <img src="eye-icon.png" alt="" />
        </div>

        <div class="form-group">
            <label for="newPassword">New Password:</label>
            <input type="password" id="newPassword" runat="server" />
            <img src="eye-icon.png" alt="" />
        </div>

        <div class="form-group">
            <label for="confirmPassword">Confirm Password:</label>
            <input type="password" id="confirmPassword" runat="server" />
            <img src="eye-icon.png" alt="" />
        </div>

        <div class="form-group">
            <label>Password Reset Options:</label>
            <div class="radio-options">
                <div class="radio-line">
                    <asp:RadioButton ID="oldPasswordRadio1"
                        runat="server"
                        GroupName="resetOption"
                        AutoPostBack="true"
                        OnCheckedChanged="OptionChanged" />
                    <span>Use Old Password</span>
                </div>

                <div class="radio-line">
                    <asp:RadioButton ID="securityQuestionRadio1"
                        runat="server"
                        GroupName="resetOption"
                        AutoPostBack="true"
                        OnCheckedChanged="OptionChanged" />
                    <span>Use Security Question</span>
                </div>
            </div>
        </div>

        <div class="form-group">
            <asp:Label ID="securityQuestionLabel" runat="server" AssociatedControlID="securityAnswer" Text="SecurityQuestion:" ForeColor="White"></asp:Label>
            <input type="text" id="securityAnswer" runat="server" />
        </div>

        <div class="button-row">
            <asp:Button ID="btnChangePassword" runat="server" Text="Change Password" CssClass="button" OnClick="ChangePassword_Click" />
            <asp:Button ID="CloseButton" runat="server" Text="Close" CssClass="button" PostBackUrl="~/Default.aspx" />
        </div>
    </div>
</asp:Content>

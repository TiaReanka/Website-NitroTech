<%@ Page Title="Customer Management" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CustomerManage.aspx.cs" Inherits="NitroTechWebsite.CustomerManage" %>

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

            .form-container h2, .form-container h3 {
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
            }

        .form-container button {
            display: block;
            max-width: 600px;
            margin: 20px auto;
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
    </style>

    <div class="form-container">
        <h2><%: Title %></h2>
        <h3>Remove Customer</h3>

        <div class="form-group">
            <label for="customerName">Customer Name:</label>
            <input type="text" id="customerName" name="customerName" />
        </div>

        <button type="submit">Remove Customer From System</button>
    </div>


</asp:Content>

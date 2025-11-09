<%@ Page Title="Vehicle Transfer" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CustomerManage.aspx.cs" Inherits="NitroTechWebsite.CustomerManage" %>

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

            .form-group input[type="text"], .form-group textarea, .form-group select {
                flex: 1;
                padding: 8px;
                border: 1px solid #ccc;
                border-radius: 5px;
                color: black;
                background-color: white;
            }

        .form-container button, .form-container input[type=submit] {
            display: block;
            max-width: 600px;
            margin: 20px auto;
            padding: 12px;
            color: white;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            font-size: 16px;
            background: transparent;
            border: 2px solid #3c00a0;
            border-radius: 5px;
            cursor: pointer;
        }

            .form-container button:hover, .form-container input[type=submit]:hover {
                background-color: #3c00a0;
                color: #fff;
                border-radius: 5px;
                box-shadow: 0 0 5px #3c00a0, 0 0 25px #3c00a0, 0 0 50px #3c00a0, 0 0 100px #3c00a0;
            }
    </style>

    <div class="form-container">
        <h2><%: Title %></h2>
        <h3>Transfer Vehicle</h3>

        <div class="form-group">
            <label for="ddlVIN">Select Vehicle VIN:</label>
            <asp:DropDownList ID="ddlVIN" runat="server" AutoPostBack="true" 
                OnSelectedIndexChanged="ddlVIN_SelectedIndexChanged" />
        </div>

        <div class="form-group">
            <label for="ddlCustomer">Select New Customer:</label>
            <asp:DropDownList ID="ddlCustomer" runat="server" AutoPostBack="true" 
                OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged" Enabled="true" />
        </div>

        <h3>Old Customer Info</h3>
        <div class="form-group"><label>Customer Name:</label><asp:TextBox ID="txtOldName" runat="server" ReadOnly="true" /></div>
        <div class="form-group"><label>Contact Number:</label><asp:TextBox ID="txtOldContact" runat="server" ReadOnly="true" /></div>
        <div class="form-group"><label>Email Address:</label><asp:TextBox ID="txtOldEmail" runat="server" ReadOnly="true" /></div>
        <div class="form-group"><label>Address:</label><asp:TextBox ID="txtOldAddress" runat="server" ReadOnly="true" /></div>

        <h3>New Customer Info</h3>
        <div class="form-group"><label>Customer Name:</label><asp:TextBox ID="txtNewName" runat="server" ReadOnly="true" /></div>
        <div class="form-group"><label>Contact Number:</label><asp:TextBox ID="txtNewContact" runat="server" ReadOnly="true" /></div>
        <div class="form-group"><label>Email Address:</label><asp:TextBox ID="txtNewEmail" runat="server" ReadOnly="true" /></div>
        <div class="form-group"><label>Address:</label><asp:TextBox ID="txtNewAddress" runat="server" ReadOnly="true" /></div>

        <asp:Button ID="btnTransfer" runat="server" Text="Transfer Vehicle" 
            OnClick="btnTransfer_Click" Enabled="true" />

        <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Text="" />

        


    </div>
</asp:Content>

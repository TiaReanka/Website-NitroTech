<%@ Page Title="Update Customers" Language="C#" MasterPageFile="~/Site.Master" 
    AutoEventWireup="true" CodeBehind="CustomerUpdate.aspx.cs" Inherits="NitroTechWebsite.Customers" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .form-container {
            max-width: 900px;
            margin: 50px auto;
            padding: 20px;
            background: #191919;
            border-radius: 10px;
            box-shadow: 0px 4px 8px rgba(0,0,0,0.2);
            color: white;
        }

        .form-container h2, .form-container h3 {
            text-align: center;
            margin-bottom: 20px;
            color: white;
        }

        .form-row {
            display: flex;
            gap: 20px;
            justify-content: space-between;
            margin-bottom: 15px;
        }

        .form-group {
            display: flex;
            align-items: center;
            flex: 1;
        }

        .form-group label {
            width: 120px;
            font-weight: bold;
            text-align: right;
            margin-right: 10px;
        }

        .form-group input, .form-group select {
            flex: 1;
            padding: 8px;
            border: 1px solid #ccc;
            border-radius: 5px;
            font-size: 14px;
            color: black;
            background-color: white;
        }

        .form-container button, .form-container input[type="submit"], .form-container input[type="button"], .form-container .aspNetButton {
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

            .form-container button:hover, .form-container input[type="submit"]:hover,
            .form-container input[type="button"]:hover, .form-container .aspNetButton:hover {
                background-color: #3c00a0;
                color: #fff;
                border-radius: 5px;
                box-shadow: 0 0 5px #3c00a0, 0 0 25px #3c00a0, 0 0 50px #3c00a0, 0 0 100px #3c00a0;
            }

        .section-title {
            margin-top: 30px;
            margin-bottom: 15px;
            font-size: 18px;
            font-weight: bold;
            text-align: center;
        }

        .search-container {
            display: flex;
            flex-direction: column;
            align-items: center;
            margin-bottom: 20px;
        }

        .search-group {
            display: flex;
            align-items: center;
            margin: 10px 0;
            width: 400px;
        }

        .search-group label {
            width: 180px;
            font-weight: bold;
            color: white;
            text-align: right;
            margin-right: 20px;
        }

        .search-group input {
            flex: 1;
            padding: 8px;
            border: 1px solid #ccc;
            border-radius: 5px;
            font-size: 14px;
            color: black;
            background-color: white;
        }
    </style>

    <div class="form-container">
        <h2><%: Title %></h2>
        <h3>Customer</h3>

        <div class="search-container">
            <div class="search-group">
                <label for="txtSearchCustomer">Search by Customer ID:</label>
                <asp:TextBox ID="txtSearchCustomer" runat="server" CssClass="textBox" />
            </div>
            <div class="search-group">
                <label for="txtSearchVIN">Search by VIN:</label>
                <asp:TextBox ID="txtSearchVIN" runat="server" CssClass="textBox" />
            </div>
            <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" CssClass="aspNetButton"/>
        </div>

        <div class="section-title">Customer Information</div>
        <div class="form-row">
            <div class="form-group">
                <label for="custID">Customer ID:</label>
                <asp:TextBox ID="custID" runat="server" ReadOnly="true" CssClass="textBox"/>
            </div>
            <div class="form-group">
                <label for="custName">Customer Name:</label>
                <asp:TextBox ID="custName" runat="server" CssClass="textBox"/>
            </div>
        </div>
        <div class="form-row">
            <div class="form-group">
                <label for="custPhone">Phone Number:</label>
                <asp:TextBox ID="custPhone" runat="server" CssClass="textBox"/>
            </div>
            <div class="form-group">
                <label for="custEmail">Email:</label>
                <asp:TextBox ID="custEmail" runat="server" CssClass="textBox"/>
            </div>
        </div>
        <div class="form-row">
            <div class="form-group">
                <label for="custAddress">Address:</label>
                <asp:TextBox ID="custAddress" runat="server" CssClass="textBox"/>
            </div>
        </div>

        <div class="section-title">Vehicle Information</div>
        <div class="form-row">
            <div class="form-group">
                <label for="vin">VIN:</label>
                <asp:TextBox ID="vin" runat="server" ReadOnly="true" CssClass="textBox"/>
            </div>
            <div class="form-group">
                <label for="make">Make:</label>
                <asp:TextBox ID="make" runat="server" CssClass="textBox"/>
            </div>
        </div>
        <div class="form-row">
            <div class="form-group">
                <label for="model">Model:</label>
                <asp:TextBox ID="model" runat="server" CssClass="textBox"/>
            </div>
            <div class="form-group">
                <label for="year">Year:</label>
                <asp:TextBox ID="year" runat="server" CssClass="textBox"/>
            </div>
        </div>
        <div class="form-row">
            <div class="form-group">
                <label for="trim">Trim:</label>
                <asp:TextBox ID="trim" runat="server" CssClass="textBox"/>
            </div>
            <div class="form-group">
                <label for="engine">Engine:</label>
                <asp:TextBox ID="engine" runat="server" CssClass="textBox"/>
            </div>
        </div>
        <div class="form-row">
            <div class="form-group">
                <label for="transmission">Transmission:</label>
                <asp:TextBox ID="transmission" runat="server" CssClass="textBox"/>
            </div>
            <div class="form-group">
                <label for="drivetrain">Drivetrain:</label>
                <asp:TextBox ID="drivetrain" runat="server" CssClass="textBox"/>
            </div>
        </div>
        <div class="form-row">
            <div class="form-group">
                <label for="fuelType">Fuel Type:</label>
                <asp:DropDownList ID="fuelType" runat="server" CssClass="textBox">
                    <asp:ListItem Text="Select a Fuel Type" Value="" />
                    <asp:ListItem Text="Gasoline" Value="Gasoline" />
                    <asp:ListItem Text="Petrol" Value="Petrol" />
                    <asp:ListItem Text="Diesel" Value="Diesel" />
                    <asp:ListItem Text="Hybrid" Value="Hybrid" />
                </asp:DropDownList>
            </div>
        </div>

        <asp:Button ID="btnSave" runat="server" Text="Save Customer & Vehicle" OnClick="btnSave_Click" CssClass="aspNetButton"/>
    </div>
</asp:Content>

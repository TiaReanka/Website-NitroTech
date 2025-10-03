<%@ Page Title="Update Customers" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CustomerUpdate.aspx.cs" Inherits="NitroTechWebsite.Customers" %>

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

        .form-container h2,
        .form-container h3 {
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

        .form-group input,
        .form-group select {
            flex: 1;
            padding: 8px;
            border: 1px solid #ccc;
            border-radius: 5px;
            font-size: 14px;
            color: black;
            background-color: white;
        }

        .form-container button, .form-container .asp-btn {
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

        .form-container button:hover, .form-container .asp-btn:hover {
            background-color: purple;
        }

        .grid-container {
            margin-top: 20px;
            overflow-x: auto;
        }

        .table {
            width: 100%;
            border-collapse: collapse;
            background-color: white;
            color: black;
        }

        .table th, .table td {
            border: 1px solid #ccc;
            padding: 8px;
            text-align: left;
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

        <!-- 🔎 Search Section -->
        <div class="search-container">
            <div class="search-group">
                <label for="txtSearchCustomer">Search by Customer ID:</label>
                <asp:TextBox ID="txtSearchCustomer" runat="server" />
            </div>
            <div class="search-group">
                <label for="txtSearchVIN">Search by VIN:</label>
                <asp:TextBox ID="txtSearchVIN" runat="server" />
            </div>
            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="asp-btn" OnClick="btnSearch_Click" />
        </div>

        <!-- 📊 Results Grid -->
        <div class="grid-container">
            <asp:GridView ID="gvResults" runat="server" AutoGenerateColumns="False" CssClass="table">
                <Columns>
                    <asp:BoundField DataField="customerID" HeaderText="Customer ID" />
                    <asp:BoundField DataField="customerName" HeaderText="Customer Name" />
                    <asp:BoundField DataField="customerAddress" HeaderText="Address" />
                    <asp:BoundField DataField="customerContactNumber" HeaderText="Phone" />
                    <asp:BoundField DataField="customerEmailAddress" HeaderText="Email" />
                    <asp:BoundField DataField="VIN" HeaderText="VIN" />
                    <asp:BoundField DataField="vehicleMake" HeaderText="Make" />
                    <asp:BoundField DataField="vehicleModel" HeaderText="Model" />
                    <asp:BoundField DataField="vehicleTrim" HeaderText="Trim" />
                    <asp:BoundField DataField="vehicleYear" HeaderText="Year" />
                </Columns>
            </asp:GridView>
        </div>

        <!-- 👤 Customer Information -->
        <div class="section-title">Customer Information</div>
        <div class="form-row">
            <div class="form-group">
                <label for="custID">Customer ID:</label>
                <asp:TextBox ID="custID" runat="server" />
            </div>
            <div class="form-group">
                <label for="custName">Customer Name:</label>
                <asp:TextBox ID="custName" runat="server" />
            </div>
        </div>
        <div class="form-row">
            <div class="form-group">
                <label for="custPhone">Phone Number:</label>
                <asp:TextBox ID="custPhone" runat="server" />
            </div>
            <div class="form-group">
                <label for="custEmail">Email:</label>
                <asp:TextBox ID="custEmail" runat="server" TextMode="Email" />
            </div>
        </div>
        <div class="form-row">
            <div class="form-group">
                <label for="custAddress">Address:</label>
                <asp:TextBox ID="custAddress" runat="server" />
            </div>
        </div>

        <!-- 🚗 Vehicle Information -->
        <div class="section-title">Vehicle Information</div>
        <div class="form-row">
            <div class="form-group">
                <label for="vin">VIN:</label>
                <asp:TextBox ID="vin" runat="server" />
            </div>
            <div class="form-group">
                <label for="make">Make:</label>
                <asp:TextBox ID="make" runat="server" />
            </div>
        </div>
        <div class="form-row">
            <div class="form-group">
                <label for="model">Model:</label>
                <asp:TextBox ID="model" runat="server" />
            </div>
            <div class="form-group">
                <label for="year">Year:</label>
                <asp:TextBox ID="year" runat="server" />
            </div>
        </div>
        <div class="form-row">
            <div class="form-group">
                <label for="trim">Trim:</label>
                <asp:TextBox ID="trim" runat="server" />
            </div>
            <div class="form-group">
                <label for="engine">Engine:</label>
                <asp:TextBox ID="engine" runat="server" />
            </div>
        </div>
        <div class="form-row">
            <div class="form-group">
                <label for="transmission">Transmission:</label>
                <asp:TextBox ID="transmission" runat="server" />
            </div>
            <div class="form-group">
                <label for="drivetrain">Drivetrain:</label>
                <asp:TextBox ID="drivetrain" runat="server" />
            </div>
        </div>
        <div class="form-row">
            <div class="form-group">
                <label for="fuelType">Fuel Type:</label>
                <asp:DropDownList ID="fuelType" runat="server">
                    <asp:ListItem Text="Select a Fuel Type" Value="" />
                    <asp:ListItem Text="Petrol" Value="Petrol" />
                    <asp:ListItem Text="Diesel" Value="Diesel" />
                    <asp:ListItem Text="Hybrid" Value="Hybrid" />
                    <asp:ListItem Text="Electric" Value="Electric" />
                </asp:DropDownList>
            </div>
        </div>

        <asp:Button ID="btnSave" runat="server" Text="Save Customer & Vehicle" CssClass="asp-btn" OnClick="btnSave_Click" />
    </div>
</asp:Content>

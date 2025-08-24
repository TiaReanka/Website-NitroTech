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
            align-items: center; /* center whole block in page */
            margin-bottom: 20px;
        }

        .search-group {
            display: flex;
            align-items: center;
            margin: 10px 0;
            width: 400px; /* fixed width so both align */
        }

            .search-group label {
                width: 180px; /* same width for labels */
                font-weight: bold;
                color: white;
                text-align: right; /* push label text right */
                margin-right: 20px; /* space between label & textbox */
            }

            .search-group input {
                flex: 1; /* take remaining space */
                padding: 8px;
                border: 1px solid #ccc;
                border-radius: 5px;
                font-size: 14px;
                color: black;
                background-color: white;
            }

        .search-container button {
            margin-top: 15px;
            padding: 10px 20px;
            border: none;
            border-radius: 5px;
            background-color: #1a2db9;
            color: white;
            cursor: pointer;
        }

            .search-container button:hover {
                background-color: purple;
            }
    </style>

    <div class="form-container">
        <h2><%: Title %></h2>
        <h3>Customer</h3>

        <!-- Search Section -->
        <div class="search-container">
            <div class="search-group">
                <label for="txtSearchCustomer">Search by Customer ID:</label>
                <input type="text" id="txtSearchCustomer" />
            </div>
            <div class="search-group">
                <label for="txtSearchVIN">Search by VIN:</label>
                <input type="text" id="txtSearchVIN" />
            </div>
            <button type="submit">Search</button>
        </div>

        <!-- Results Grid -->
        <div class="grid-container">
            <asp:GridView ID="gvResults" runat="server" AutoGenerateColumns="False" CssClass="table">
                <Columns>
                    <asp:BoundField DataField="customerID" HeaderText="Customer ID" />
                    <asp:BoundField DataField="customerName" HeaderText="Customer Name" />
                    <asp:BoundField DataField="customerAddr" HeaderText="Address" />
                    <asp:BoundField DataField="customerContact" HeaderText="Phone" />
                    <asp:BoundField DataField="customerEmail" HeaderText="Email" />
                    <asp:BoundField DataField="VIN" HeaderText="VIN" />
                    <asp:BoundField DataField="vehicleMake" HeaderText="Make" />
                    <asp:BoundField DataField="vehicleModel" HeaderText="Model" />
                    <asp:BoundField DataField="vehicleTrim" HeaderText="Trim" />
                    <asp:BoundField DataField="vehicleYear" HeaderText="Year" />
                </Columns>
            </asp:GridView>
        </div>

        <!-- Customer Info Section -->
        <div class="section-title">Customer Information</div>
        <div class="form-row">
            <div class="form-group">
                <label for="custID">Customer ID:</label>
                <input type="text" id="custID" />
            </div>
            <div class="form-group">
                <label for="custName">Customer Name:</label>
                <input type="text" id="custName" />
            </div>
        </div>
        <div class="form-row">
            <div class="form-group">
                <label for="custPhone">Phone Number:</label>
                <input type="text" id="custPhone" />
            </div>
            <div class="form-group">
                <label for="custEmail">Email:</label>
                <input type="email" id="custEmail" />
            </div>
        </div>
        <div class="form-row">
            <div class="form-group">
                <label for="custAddress">Address:</label>
                <input type="text" id="custAddress" />
            </div>
        </div>

        <!-- Vehicle Info Section -->
        <div class="section-title">Vehicle Information</div>
        <div class="form-row">
            <div class="form-group">
                <label for="vin">VIN:</label>
                <input type="text" id="vin" />
            </div>
            <div class="form-group">
                <label for="make">Make:</label>
                <input type="text" id="make" />
            </div>
        </div>
        <div class="form-row">
            <div class="form-group">
                <label for="model">Model:</label>
                <input type="text" id="model" />
            </div>
            <div class="form-group">
                <label for="year">Year:</label>
                <input type="text" id="year" />
            </div>
        </div>
        <div class="form-row">
            <div class="form-group">
                <label for="trim">Trim:</label>
                <input type="text" id="trim" />
            </div>
            <div class="form-group">
                <label for="engine">Engine:</label>
                <input type="text" id="engine" />
            </div>
        </div>
        <div class="form-row">
            <div class="form-group">
                <label for="transmission">Transmission:</label>
                <input type="text" id="transmission" />
            </div>
            <div class="form-group">
                <label for="drivetrain">Drivetrain:</label>
                <input type="text" id="drivetrain" />
            </div>
        </div>
        <div class="form-row">
            <div class="form-group">
                <label for="fuelType">Fuel Type:</label>
                <select id="fuelType">
                    <option value="">Select a Fuel Type</option>
                    <option value="Petrol">Petrol</option>
                    <option value="Diesel">Diesel</option>
                    <option value="Hybrid">Hybrid</option>
                    <option value="Electric">Electric</option>
                </select>
            </div>
        </div>

        <button type="submit">Save Customer & Vehicle</button>
    </div>
</asp:Content>

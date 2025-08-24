<%@ Page Title="Payments" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Payments.aspx.cs" Inherits="NitroTechWebsite.Payments" %>

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

        .form-group select {
            flex: 1;
            padding: 8px;
            border: 1px solid #ccc;
            border-radius: 6px;
            font-size: 14px;
            width: 100%;        
            color: black;       
            background-color: white;
        }

        .form-group select option[value=""] {
            color: gray;
        }
    </style>

    <div class="form-container">
        <h2><%: Title %></h2>
        <h3>Add Payment Details</h3>

        <div class="form-group">
            <label for="customerID">ID of Customer:</label>
            <select id="customerID" name="customerID">
                <option value="">-- Select Customer ID --</option>
                <option value="CUST001">CUST001</option>
                <option value="CUST002">CUST002</option>
                <option value="CUST003">CUST003</option>
            </select>
        </div>

        <div class="form-group">
            <label for="amount">Amount in Rands (R):</label>
            <input type="text" id="amount" name="amount" />
        </div>

        <div class="form-group">
            <label for="paymentDate">Date of Payment:</label>
            <input type="date" id="paymentDate" name="paymentDate" />
        </div>

        <button type="submit">Add Payment</button>
    </div>
</asp:Content>

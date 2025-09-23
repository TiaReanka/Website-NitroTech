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

        .btn1 {
            display: flex;
            justify-content: center; /* center horizontally */
            margin-top: 20px;
        }

            .btn1 input[type=submit] {
                text-align: center;
                margin: 20px auto;
                padding: 10px 30px;
                min-width: 50px;
                color: #fff;
                font-size: 15px;
                text-decoration: none;
                text-transform: uppercase;
                overflow: hidden;
                transition: 0.5s;
                letter-spacing: 2px;
                line-height: 20px;
                width: auto;
                display: flex;
                justify-content: center;
                background: transparent;
                border: 2px solid #3c00a0;
                border-radius: 5px;
                cursor: pointer;
            }

                .btn1 input[type=submit]:hover {
                    background-color: #3c00a0;
                    color: #fff;
                    border-radius: 5px;
                    box-shadow: 0 0 5px #3c00a0, 0 0 25px #3c00a0, 0 0 50px #3c00a0, 0 0 100px #3c00a0;
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
            <asp:DropDownList ID="ddlCustomerID" runat="server"></asp:DropDownList>
        </div>

        <div class="form-group">
            <label for="amount">Amount in Rands (R):</label>
            <asp:TextBox ID="txtAmount" runat="server" ForeColor="Black"></asp:TextBox>
        </div>

        <div class="form-group">
            <label for="paymentDate">Date of Payment:</label>
            <asp:TextBox ID="txtPaymentDate" runat="server" TextMode="Date" ForeColor="Black"></asp:TextBox>
        </div>

        <div class="btn1">
            <asp:Button ID="btnAddPayment" runat="server" Text="Add Payment" OnClick="btnAddPayment_Click" />
        </div>
        
    </div>
</asp:Content>

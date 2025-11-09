<%@ Page Title="Generate Invoices" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Invoices.aspx.cs"
    Inherits="NitroTechWebsite.Invoices" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server"> 
    <style> 
        .form-container { 
            max-width: 1000px;          
            margin: 60px auto;          
            padding: 40px;            
            background: #191919; 
            border-radius: 12px; 
            box-shadow: 0px 6px 15px rgba(0,0,0,0.25); 
        } 

        .form-container h2, 
        .form-container h3 { 
            text-align: center; 
            margin-bottom: 30px;       
            color: white; 
        } 

        .form-group { 
            display: flex; 
            align-items: center; 
            justify-content: space-between; 
            margin-bottom: 25px;       
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

        .form-group select, 
        .form-group .aspNetDropDown { 
            flex: 1; 
            padding: 12px; 
            border: 1px solid #ccc; 
            border-radius: 8px; 
            font-size: 15px; 
            width: 100%; 
            color: black; 
            background-color: white; 
        }

        .form-container button,
        .btn {
            display: block;
            max-width: 300px;
            margin: 30px auto;
            padding: 14px;
            color: white;
            border: none;
            border-radius: 8px;
            cursor: pointer;
            font-size: 17px;
            background: transparent;
            border: 2px solid #3c00a0;
            border-radius: 5px;
            cursor: pointer;
        }

            .form-container button:hover,
            .btn:hover {
                background-color: #3c00a0;
                color: #fff;
                border-radius: 5px;
                box-shadow: 0 0 5px #3c00a0, 0 0 25px #3c00a0, 0 0 50px #3c00a0, 0 0 100px #3c00a0;
            } 
    </style> 

    <div class="form-container"> 
        <h2><%: Title %></h2> 

        <div class="form-group"> 
            <label for="quotationNumber">Select Quotation Number:</label> 
            <asp:DropDownList ID="quotationNumber" runat="server" CssClass="aspNetDropDown"> 
            </asp:DropDownList> 
        </div> 

        <asp:Button ID="btnConfirmQuotation" runat="server" Text="Confirm Quotation" 
            CssClass="btn" OnClick="btnConfirmQuotation_Click" />

        <asp:Button ID="btnGenerateInvoice" runat="server" Text="Generate Invoice" 
            CssClass="btn" OnClick="btnGenerateInvoice_Click" Enabled="false" />
    </div> 
</asp:Content>

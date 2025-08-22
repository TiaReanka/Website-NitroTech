<%@ Page Title="Generate Invoices" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Invoices.aspx.cs" Inherits="NitroTechWebsite.Invoices" %> 
 
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server"> 
    <style> 
        .form-container { 
            max-width: 1000px;          /* wider panel */ 
            margin: 60px auto;          /* more vertical centering */ 
            padding: 40px;              /* more inner padding */ 
            background: #191919; 
            border-radius: 12px; 
            box-shadow: 0px 6px 15px rgba(0,0,0,0.25); 
        } 
 
        .form-container h2, 
        .form-container h3 { 
            text-align: center; 
            margin-bottom: 30px;        /* more space under headings */ 
            color: white; 
        } 
 
        .form-group { 
            display: flex; 
            align-items: center; 
            justify-content: space-between; 
            margin-bottom: 25px;        /* more spacing between groups */ 
            max-width: 700px; 
            margin-left: auto; 
            margin-right: auto; 
        } 
 
        .form-group label { 
            width: 220px;               /* slightly smaller so input has more space */ 
            font-weight: bold; 
            text-align: right;          /* fixed typo (was centre) */ 
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
            max-width: 300px;           /* consistent narrower button */ 
            margin: 30px auto;          /* more spacing between buttons */ 
            padding: 14px; 
            background-color: #1a2db9; 
            color: white; 
            border: none; 
            border-radius: 8px; 
            cursor: pointer; 
            font-size: 17px; 
        } 
 
        .form-container button:hover, 
        .btn:hover { 
            background-color: purple; 
        } 
    </style> 
 
    <div class="form-container"> 
        <h2><%: Title %></h2> 
         
 
        <div class="form-group"> 
            <label for="quotationNumber">Select Quotation Number:</label> 
            <asp:DropDownList ID="quotationNumber" runat="server" 
CssClass="aspNetDropDown"> 
                <asp:ListItem Value="">-- Select Quotation Number --</asp:ListItem> 
                <asp:ListItem Value="QUOTE001">QUOTE001</asp:ListItem> 
                <asp:ListItem Value="QUOTE002">QUOTE002</asp:ListItem> 
                <asp:ListItem Value="QUOTE003">QUOTE003</asp:ListItem> 
            </asp:DropDownList> 
        </div> 
 
        <asp:Button ID="btnConfirmQuotation" runat="server" Text="Confirm Quotation" 
CssClass="btn" OnClick="btnConfirmQuotation_Click" /> 
 
        <asp:Button ID="btnGenerateInvoice" runat="server" Text="Generate Invoice" 
CssClass="btn" OnClick="btnGenerateInvoice_Click" /> 
    </div> 
</asp:Content>
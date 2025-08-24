<%@ Page Title="Review Active Quotations" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ActiveQuotation.aspx.cs" Inherits="NitroTechWebsite.ReviewQuotations" %> 
 
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server"> 
    <style> 
        .form-container { 
            max-width: 1000px; 
            margin: 60px auto; 
            padding: 40px; 
            background: #191919; 
            border-radius: 12px; 
            box-shadow: 0px 6px 15px rgba(0,0,0,0.25); 
            text-align: center; 
        } 
 
        .form-container h2 { 
            text-align: center; 
            margin-bottom: 30px; 
            color: white; 
        } 
 
        .form-group { 
            display: flex; 
            justify-content: space-between; 
            align-items: center; 
            margin: 15px auto; 
            max-width: 500px; 
        } 
 
        .form-group label { 
            font-weight: bold; 
            color: white; 
            flex: 1; 
            text-align: left; 
            margin-right: 10px; 
        } 
 
        .form-group input[type="text"] { 
            flex: 2; 
            padding: 10px; 
            border: 1px solid #ccc; 
            border-radius: 8px; 
            font-size: 15px; 
            color: black; 
            background-color: white; 
        } 
 
        .form-container button, 
        .btn { 
            display: block; 
            max-width: 200px; 
            margin: 20px auto; 
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

        .gridview { 
            width: 90%; 
            margin: 30px auto; 
            border-collapse: collapse; 
            background: white; 
            border-radius: 8px; 
            overflow: hidden; 
        } 
 
        .gridview th { 
            background: #1a2db9; 
            color: white; 
            padding: 12px; 
            text-align: left; 
        } 
 
        .gridview td { 
            padding: 10px; 
            border-bottom: 1px solid #ddd; 
        } 
 
        .gridview tr:nth-child(even) { 
            background-color: #f9f9f9; 
        } 
    </style> 
 
    <div class="form-container"> 
        <h2><%: Title %></h2> 
 
        <div class="form-group"> 
            <label for="txtCustomerName">Customer Name:</label> 
            <asp:TextBox ID="txtCustomerName" runat="server" placeholder="Enter customer name..." /> 
        </div> 
 
        <div class="form-group"> 
            <label for="txtQuotationNumber">Quotation Number:</label> 
            <asp:TextBox ID="txtQuotationNumber" runat="server" placeholder="Enter quotation number..." /> 
        </div> 
 
        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn" OnClick="btnSearch_Click" /> 
 
        <asp:GridView ID="gvQuotations" runat="server" AutoGenerateColumns="true" CssClass="gridview"> 
        </asp:GridView> 
    </div> 
</asp:Content> 
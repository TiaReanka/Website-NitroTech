<%@ Page Title="View Active Quotations" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ActiveQuotation.aspx.cs" Inherits="NitroTechWebsite.ReviewQuotations" %> 
 
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
            background: transparent; 
            color: white; 
            border: none; 
            border-radius: 8px; 
            cursor: pointer; 
            font-size: 17px;
            border: 2px solid #3c00a0;
            border-radius: 5px;
        }

            .form-container button:hover,
            .btn:hover {
                background-color: #3c00a0;
                color: #fff;
                border-radius: 5px;
                box-shadow: 0 0 5px #3c00a0, 0 0 25px #3c00a0, 0 0 50px #3c00a0, 0 0 100px #3c00a0;
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
            color: black;
        } 


 
        .gridview tr:nth-child(even) { 
            background-color: #f9f9f9; 
        } 

        .radio-group {
            display: flex;
            justify-content: center;
            gap: 20px;
            margin: 15px 0;
        }
        .radio-group label {
            color: white;
            font-weight: normal;
        }

        .button-group {
            display: flex;
            justify-content: center;
            gap: 20px; 
            margin-top: 20px;
        }

    </style> 
 
    <div class="form-container"> 
        <h2><%: Title %></h2> 
 
       <div class="radio-group">
            <label><asp:RadioButton ID="rbCustomerID" runat="server" GroupName="SearchBy" Text="Customer ID" Checked="true"/></label>
            <label><asp:RadioButton ID="rbCustomerName" runat="server" GroupName="SearchBy" Text="Customer Name" /></label>
            <label><asp:RadioButton ID="rbQuotationNumber" runat="server" GroupName="SearchBy" Text="Quotation Number" /></label>
            <label><asp:RadioButton ID="rbVIN" runat="server" GroupName="SearchBy" Text="VIN" /></label>
        </div>

        <div class="form-group">
            <label for="txtSearch">Search:</label>
            <asp:TextBox ID="txtSearch" runat="server" placeholder="Enter search value..." OnTextChanged="txtSearch_TextChanged"/>
        </div>
 
        <div class="button-group">
            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn" OnClick="btnSearch_Click" />
            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn" OnClick="btnClear_Click" />
        </div>

 
        <asp:GridView ID="gvQuotations" runat="server" AutoGenerateColumns="true" CssClass="gridview"  OnRowDataBound="gvQuotations_RowDataBound"> 
        </asp:GridView> 
    </div> 
</asp:Content> 
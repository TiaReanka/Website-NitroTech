<%@ Page Title="Generate Statements" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GenerateStatement.aspx.cs" Inherits="NitroTechWebsite.Statements" %> 
 
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
            <label for="customerID">Select Customer ID:</label> 
            <asp:DropDownList ID="customerID" runat="server" CssClass="aspNetDropDown"> 
                <asp:ListItem Value="">-- Select Customer ID --</asp:ListItem> 
                <asp:ListItem Value="CUST001">CUST001</asp:ListItem> 
                <asp:ListItem Value="CUST002">CUST002</asp:ListItem> 
                <asp:ListItem Value="8601260526053">8601260526053</asp:ListItem> 
            </asp:DropDownList> 
        </div> 
 
        <asp:Button ID="btnGenerateStatement" runat="server" Text="Generate Statement" 
CssClass="btn" OnClick="btnGenerateStatement_Click" /> 
    </div> 
</asp:Content> 
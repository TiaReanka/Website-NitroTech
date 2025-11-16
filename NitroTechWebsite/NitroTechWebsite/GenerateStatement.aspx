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
    align-items: center;       /* vertically center label & dropdown */
    justify-content: center;   /* center the whole group horizontally */
    margin-bottom: 25px;
    gap: 10px;                 /* space between label & dropdown */
}

.form-group label {
    font-weight: bold;
    color: white;
    text-align: right;
}

.form-group select,
.form-group .aspNetDropDown {
    min-width: 200px;          /* ensures dropdown isn’t too small */
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
        

        <div class="btn1">
            <asp:Button ID="Button1" runat="server" Text="Generate Statement" CssClass="btn" OnClick="btnGenerateStatement_Click" /> 
        </div>

    </div> 
</asp:Content> 
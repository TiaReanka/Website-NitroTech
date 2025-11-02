<%@ Page Title="Parts" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReviewPart.aspx.cs" Inherits="NitroTechWebsite.ReviewPart" %>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

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
            color: white;
        }

        .form-group {
            display: flex;
            align-items: center;
            margin-bottom: 15px;
            max-width: 600px;
            margin-left: auto;
            margin-right: auto;
        }

        .form-group input,
        .form-group select {
            flex: 1;
            padding: 8px;
            border: 1px solid #ccc;
            border-radius: 5px;
            color: black;
        }

        .form-container button,
        .form-container asp:Button {
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

        .form-container button:hover,
        .form-container asp:Button:hover {
            background-color: purple;
        }

        .form-group select option[value=""] {
            color: gray;
        }

        .grid-container {
            margin-top: 30px;
        }

        .grid-container table {
            width: 100%;
            border-collapse: collapse;
            background: white;
            color: black;
            text-align: center;
            font-size: 14px;
            min-height: 300px;
        }

        .grid-container th, .grid-container td {
            border: 1px solid #ccc;
            padding: 8px;
        }

        .grid-container th {
            background: #eee;
        }
    </style>

    <div class="form-container">
        <h2><%: Title %></h2>
        <h3>Review Parts Report</h3>

        <div class="form-group">
            <asp:DropDownList ID="cmbSearch" runat="server" AppendDataBoundItems="true" CssClass="form-control">
                
            </asp:DropDownList>
            <asp:Button ID="btnFind" runat="server" Text="Search" OnClick="btnFind_Click" Style="margin-left:15px;" />

           
        <cr:CrystalReportViewer 
    ID="CrystalReportViewer1" 
    runat="server" 
    AutoDataBind="true"
    Width="100%" 
    Height="900px" 
    ToolPanelView="None" />

       

        </div>

        <div class="grid-container">
            <asp:GridView ID="PartsGrid" runat="server" AutoGenerateColumns="true" />
        </div>

           

    </div>




</asp:Content>

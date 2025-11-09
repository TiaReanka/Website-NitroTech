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
            flex-direction: column;
            align-items: center;
            justify-content: center;
            margin-bottom: 15px;
            gap: 15px;
        }

        .form-group select {
            flex: 1;
            padding: 8px;
            border: 1px solid #ccc;
            border-radius: 5px;
            color: black;
            width: 100%;
        }

        .btn-search {
            padding: 10px 30px;
            color: #fff;
            font-size: 15px;
            text-transform: uppercase;
            background: transparent;
            border: 2px solid #3c00a0;
            border-radius: 5px;
            cursor: pointer;
            letter-spacing: 2px;
            transition: 0.5s;
        }

        .btn-search:hover {
            background-color: #3c00a0;
            color: #fff;
            border-radius: 5px;
            box-shadow: 0 0 5px #3c00a0,
                        0 0 25px #3c00a0,
                        0 0 50px #3c00a0,
                        0 0 100px #3c00a0;
        }

        .button-row {
            display: flex;
            justify-content: center;
            gap: 20px;
            margin-top: 15px;
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
            <asp:DropDownList ID="cmbSearch" runat="server" AppendDataBoundItems="true" CssClass="form-control"></asp:DropDownList>

            <div class="button-row">
                <asp:Button ID="btnFind" runat="server" Text="Search" ForeColor="White" CssClass="btn-search" OnClick="btnFind_Click" />
                <asp:Button ID="btnReport" runat="server" Text="Generate Report" ForeColor="White" CssClass="btn-search" OnClick="btnReport_Click" />
            </div>
        </div>

        <cr:CrystalReportViewer 
            ID="CrystalReportViewer1" 
            runat="server" 
            AutoDataBind="true"
            Width="100%" 
            Height="900px" 
            ToolPanelView="None" />

        <div class="grid-container">
            <asp:GridView ID="PartsGrid" runat="server" AutoGenerateColumns="true" />
        </div>
    </div>
</asp:Content>
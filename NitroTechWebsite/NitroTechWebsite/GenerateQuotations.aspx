<%@ Page Title="Generate Quotations" Language="C#" MasterPageFile="~/Site.Master" 
AutoEventWireup="true" CodeBehind="GenerateQuotations.aspx.cs" 
Inherits="NitroTechWebsite.Quotations" %> 
 
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server"> 
    <style> 
        .form-container { 
            max-width: 900px; margin:50px auto; padding:30px; 
            background:#191919; border-radius:12px; color:white; 
        } 
        .tabs { display:flex; margin-bottom:20px; } 
        .tab-header { 
            flex:1; text-align:center; padding:10px; cursor:pointer; 
            background:#1a2db9; border-radius:6px 6px 0 0; margin-right:2px; 
        } 
        .tab-header.active { background:purple; } 
        .tab-panel { display:none; padding:15px; border:1px solid #ccc; border-radius:0 6px 6px 
6px; background:white; color:black; } 
        .tab-panel.active { display:block; } 
        .form-group { margin:15px 0; } 
        .form-group label { display:block; font-weight:bold; margin-bottom:5px; } 
        .form-group input, .form-group select, .form-group textarea { width:100%; padding:8px; 
border-radius:6px; border:1px solid #ccc; font-size:14px; } 
        .btn-generate { display:block; margin:15px 0; padding:12px 25px; font-size:16px; 
border-radius:6px; background:#1a2db9; color:white; border:none; cursor:pointer; } 
        .btn-generate:hover { background:purple; } 
        .gridview { width:100%; border-collapse:collapse; margin-top:20px; } 
        .gridview th, .gridview td { padding:8px; border:1px solid #ccc; text-align:left; } 
        .gridview th { background:#1a2db9; color:white; } 
        .gridview tr:nth-child(even) { background:#f2f2f2; } 
    </style> 
 
    <div class="form-container"> 
        <h2>Generate Quotation</h2> 
 
        <!-- Tab Headers --> 
        <div class="tabs"> 
            <div class="tab-header active" onclick="showTab('pnlCustomer', this)">Customer 
Info</div> 
            <div class="tab-header" onclick="showTab('pnlVehicle', this)">Vehicle Info</div> 
            <div class="tab-header" onclick="showTab('pnlFaults', this)">Faults</div> 
        </div> 
 
        <!-- Customer Info --> 
        <asp:Panel ID="pnlCustomer" runat="server" CssClass="tab-panel active" 
ClientIDMode="Static"> 
            <div class="form-group"><label>Customer ID:</label><asp:TextBox ID="txtCustID" 
runat="server" ClientIDMode="Static" /></div> 
            <div class="form-group"><label>Name:</label><asp:TextBox ID="txtCustName" 
runat="server" ClientIDMode="Static" /></div> 
            <div class="form-group"><label>Email:</label><asp:TextBox ID="txtCustEmail" 
runat="server" ClientIDMode="Static" /></div> 
            <div class="form-group"><label>Phone:</label><asp:TextBox ID="txtCustPhone" 
runat="server" ClientIDMode="Static" /></div> 
            <div class="form-group"><label>Address:</label><asp:TextBox ID="txtCustAddress" 
runat="server" TextMode="MultiLine" Rows="3" ClientIDMode="Static" /></div> 
            <div class="form-group"><label>Quotation Date:</label><asp:TextBox 
ID="txtQuotationDate" runat="server" ReadOnly="true" ClientIDMode="Static" /></div> 
            <asp:Button ID="btnNextToVehicle" runat="server" Text="Next" CssClass="btn-generate" 
OnClientClick="showTab('pnlVehicle', document.querySelectorAll('.tab-header')[1]); return false;" 
/> 
        </asp:Panel> 
 
        <!-- Vehicle Info --> 
        <asp:Panel ID="pnlVehicle" runat="server" CssClass="tab-panel" ClientIDMode="Static"> 
            <div class="form-group"><label>VIN:</label><asp:TextBox ID="txtVIN" runat="server" 
ClientIDMode="Static" /></div> 
            <div class="form-group"><label>Make:</label><asp:TextBox ID="txtMake" 
runat="server" ClientIDMode="Static" /></div> 
            <div class="form-group"><label>Model:</label><asp:TextBox ID="txtModel" 
runat="server" ClientIDMode="Static" /></div> 
            <div class="form-group"><label>Year:</label><asp:TextBox ID="txtYear" runat="server" 
ClientIDMode="Static" /></div> 
            <div class="form-group"><label>Engine:</label><asp:TextBox ID="txtEngine" 
runat="server" ClientIDMode="Static" /></div> 
            <div class="form-group"><label>Transmission:</label><asp:TextBox 
ID="txtTransmission" runat="server" ClientIDMode="Static" /></div> 
            <div class="form-group"><label>Drivetrain:</label><asp:TextBox ID="txtDrivetrain" 
runat="server" ClientIDMode="Static" /></div> 
            <div class="form-group"><label>Fuel:</label> 
                <asp:DropDownList ID="ddlFuel" runat="server" ClientIDMode="Static"> 
                    <asp:ListItem Value="">-- Select Fuel --</asp:ListItem> 
                    <asp:ListItem Value="Petrol">Petrol</asp:ListItem> 
                    <asp:ListItem Value="Diesel">Diesel</asp:ListItem> 
                    <asp:ListItem Value="Hybrid">Hybrid</asp:ListItem> 
                </asp:DropDownList> 
            </div> 
            <asp:Button ID="btnNextToFaults" runat="server" Text="Next" CssClass="btn-generate" 
OnClientClick="showTab('pnlFaults', document.querySelectorAll('.tab-header')[2]); return false;" 
/> 
        </asp:Panel> 
 
    <!-- Faults Info --> 
    <asp:Panel ID="pnlFaults" runat="server" CssClass="tab-panel" ClientIDMode="Static"> 
        <div class="form-group"><label>Fault Description:</label><asp:TextBox ID="txtFault" 
runat="server" ClientIDMode="Static" /></div> 
    
        <div class="form-group"> 
            <label>Select Part:</label> 
            <asp:DropDownList ID="cmbPart" runat="server" ClientIDMode="Static"> 
                <asp:ListItem>-- Select Part --</asp:ListItem> 
            </asp:DropDownList> 
        </div> 
    
        <div class="form-group"><label>Quantity:</label><asp:TextBox ID="nudQuantity" 
runat="server" Text="1" ClientIDMode="Static" /></div> 
    
        <asp:Button ID="btnAddFault" runat="server" Text="Add Fault" CssClass="btn-generate" 
OnClick="btnAddFault_Click" /> 
        <asp:GridView ID="gvFaults" runat="server" CssClass="gridview" 
AutoGenerateColumns="true" /> 
        <asp:Button ID="btnGenerateQuotation" runat="server" Text="Generate Quotation" 
CssClass="btn-generate" OnClick="btnGenerateQuotation_Click" /> 
    </asp:Panel> 
    </div> 
 
    <script> 
        function showTab(panelId, header) { 
            document.querySelectorAll('.tab-panel').forEach(p => p.classList.remove('active')); 
            document.querySelectorAll('.tab-header').forEach(h => h.classList.remove('active')); 
            document.getElementById(panelId).classList.add('active'); 
            header.classList.add('active'); 
        } 
    </script> 
</asp:Content>
<%@ Page Title="Generate Quotations" Language="C#" MasterPageFile="~/Site.Master"  
AutoEventWireup="true" CodeBehind="GenerateQuotations.aspx.cs"  
Inherits="NitroTechWebsite.Quotations" %>  
  
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">  
    <style>
        .form-container {
            max-width: 900px;
            margin: 50px auto;
            padding: 30px;
            background: #191919;
            border-radius: 12px;
            color: white;
        }

        .tabs {
            display: flex;
            margin-bottom: 20px;
        }

        .tab-header {
            flex: 1;
            text-align: center;
            padding: 10px;
            cursor: pointer;
            background: #1a2db9;
            border-radius: 6px 6px 0 0;
            margin-right: 2px;
        }

            .tab-header.active {
                background: purple;
            }

        .tab-panel {
            display: none;
            padding: 15px;
            border: 1px solid #ccc;
            border-radius: 0 6px 6px 6px;
            background: white;
            color: black;
        }

            .tab-panel.active {
                display: block;
            }

        .form-group {
            margin: 15px 0;
        }

            .form-group label {
                display: block;
                font-weight: bold;
                margin-bottom: 5px;
            }

            .form-group input, .form-group select, .form-group textarea {
                width: 100%;
                padding: 8px;
                border-radius: 6px;
                border: 1px solid #ccc;
                font-size: 14px;
            }

        .faults-container {
            display: flex;
            gap: 20px;
            align-items: flex-start; 
        }

        .faults-left {
            flex: 2; 
        }

        .faults-summary {
            flex: 1;
        }

            .faults-summary label {
                display: block;
                margin-bottom: 5px;
                font-weight: bold;
            }

            .faults-summary textarea,
            .faults-summary .gridview {
                width: 100%;
                box-sizing: border-box;
            }


        .button-row {
            display: flex;
            justify-content: space-between; 
            gap: 10px; 
            margin-top: 15px;
        }

        .btn-generate {
            display: block;
            margin: 15px 0;
            padding: 12px 25px;
            font-size: 16px;
            border-radius: 6px;
            background: #1a2db9;
            color: white;
            border: none;
            cursor: pointer;
        }

            .btn-generate:hover {
                background: purple;
            }

        .gridview {
            width: 100%;
            border-collapse: collapse;
            margin-top: 20px;
        }

            .gridview th, .gridview td {
                padding: 8px;
                border: 1px solid #ccc;
                text-align: left;
            }

            .gridview th {
                background: #1a2db9;
                color: white;
            }

            .gridview tr:nth-child(even) {
                background: #f2f2f2;
            }

        .faults-summary textarea {
            resize: none; 
        }
    </style>  
  
    <div class="form-container">
        <h2>Generate Quotation</h2>

        <div class="tabs">
            <div class="tab-header active" onclick="showTab('pnlCustomer', this)">
                Customer Info
            </div>
            <div class="tab-header" onclick="showTab('pnlVehicle', this)">Vehicle Info</div>
            <div class="tab-header" onclick="showTab('pnlFaults', this)">Faults</div>
        </div>

        <!-- Customer Tab -->
        <asp:Panel ID="pnlCustomer" runat="server" CssClass="tab-panel active"
            ClientIDMode="Static">
            <div class="form-group">
                <label>Customer ID:</label><asp:TextBox ID="txtCustID"
                    runat="server" ClientIDMode="Static" />
            </div>
            <div class="form-group">
                <label>Name:</label><asp:TextBox ID="txtCustName"
                    runat="server" ClientIDMode="Static" />
            </div>
            <div class="form-group">
                <label>Email:</label><asp:TextBox ID="txtCustEmail"
                    runat="server" ClientIDMode="Static" />
            </div>
            <div class="form-group">
                <label>Phone:</label><asp:TextBox ID="txtCustPhone"
                    runat="server" ClientIDMode="Static" />
            </div>
            <div class="form-group">
                <label>Address:</label><asp:TextBox ID="txtCustAddress"
                    runat="server" TextMode="MultiLine" Rows="3" ClientIDMode="Static" />
            </div>
            <div class="form-group">
                <label>Quotation Date:</label><asp:TextBox
                    ID="txtQuotationDate" runat="server" ReadOnly="true" ClientIDMode="Static" />
            </div>

            <div class="form-group">
                <label>Search by Name:</label>
                <asp:TextBox ID="txtSearchCustomer" runat="server" AutoPostBack="true"
                    OnTextChanged="txtSearchCustomer_TextChanged" CssClass="form-control" />
            </div>

            <div style="max-height: 300px; overflow-y: auto; overflow-x: auto;">
                <asp:GridView ID="gvCustomerVehicles" runat="server"
                    AutoGenerateColumns="False"
                    CssClass="table table-striped table-bordered"
                    DataKeyNames="customerID,customerName,customerEmailAddress,customerContactNumber,customerAddress,
                  VIN,vehicleMake,vehicleModel,vehicleYear,vehicleEngine,
                  vehicleTransmission,vehicleDriveTrain,vehicleFuelType"
                    OnSelectedIndexChanged="gvCustomerVehicles_SelectedIndexChanged">

                    <Columns>
                        <asp:CommandField ShowSelectButton="True" SelectText="Select" />
                        <asp:BoundField DataField="customerID" HeaderText="Customer ID" />
                        <asp:BoundField DataField="customerName" HeaderText="Customer Name" />
                        <asp:BoundField DataField="customerEmailAddress" HeaderText="Email" />
                        <asp:BoundField DataField="customerContactNumber" HeaderText="Phone" />
                        <asp:BoundField DataField="customerAddress" HeaderText="Address" />

                        <asp:BoundField DataField="VIN" HeaderText="VIN" />
                        <asp:BoundField DataField="vehicleMake" HeaderText="Make" />
                        <asp:BoundField DataField="vehicleModel" HeaderText="Model" />
                        <asp:BoundField DataField="vehicleYear" HeaderText="Year" />
                        <asp:BoundField DataField="vehicleEngine" HeaderText="Engine" />
                        <asp:BoundField DataField="vehicleTransmission" HeaderText="Transmission" />
                        <asp:BoundField DataField="vehicleDriveTrain" HeaderText="Drivetrain" />
                        <asp:BoundField DataField="vehicleFuelType" HeaderText="Fuel Type" />

                       
                        
                    </Columns>
                </asp:GridView>

</div>

            <asp:Button ID="btnResetAll" runat="server" Text="Reset All"
                CssClass="btn-generate" OnClick="btnResetAll_Click" />


            <asp:Button ID="btnNextToVehicle" runat="server" Text="Next" CssClass="btn-generate"
                OnClientClick="showTab('pnlVehicle', document.querySelectorAll('.tab-header')[1]); return false;" />
        </asp:Panel>

        <!-- Vehicle Tab -->
        <asp:Panel ID="pnlVehicle" runat="server" CssClass="tab-panel" ClientIDMode="Static">
            <div class="form-group">
                <label>VIN:</label><asp:TextBox ID="txtVIN" runat="server"
                    ClientIDMode="Static" />
            </div>
            <div class="form-group">
                <label>Make:</label><asp:TextBox ID="txtMake"
                    runat="server" ClientIDMode="Static" />
            </div>
            <div class="form-group">
                <label>Model:</label><asp:TextBox ID="txtModel"
                    runat="server" ClientIDMode="Static" />
            </div>
            <div class="form-group">
                <label>Year:</label><asp:TextBox ID="txtYear" runat="server"
                    ClientIDMode="Static" />
            </div>
            <div class="form-group">
                <label>Engine:</label><asp:TextBox ID="txtEngine"
                    runat="server" ClientIDMode="Static" />
            </div>
            <div class="form-group">
                <label>Transmission:</label><asp:TextBox
                    ID="txtTransmission" runat="server" ClientIDMode="Static" />
            </div>
            <div class="form-group">
                <label>Drivetrain:</label><asp:TextBox ID="txtDrivetrain"
                    runat="server" ClientIDMode="Static" />
            </div>
            <div class="form-group">
                <label>Fuel:</label>
                <asp:DropDownList ID="ddlFuel" runat="server" ClientIDMode="Static">
                    <asp:ListItem Value="">-- Select Fuel --</asp:ListItem>
                    <asp:ListItem Value="Gasoline">Gasoline</asp:ListItem>
                    <asp:ListItem Value="Diesel">Diesel</asp:ListItem>
                    <asp:ListItem Value="Hybrid">Hybrid</asp:ListItem>
                    <asp:ListItem Value="Electric">Electric</asp:ListItem>
                    <asp:ListItem Value="Ethanol">Ethanol</asp:ListItem>
                    <asp:ListItem Value="Natural Gas">Natural Gas</asp:ListItem>

                </asp:DropDownList>
            </div>

            <asp:Button ID="btnResetVehicle" runat="server" Text="Reset Vehicle"
                CssClass="btn-generate" OnClick="btnResetVehicle_Click" />

            <div class="button-row">
                <asp:Button ID="btnBackToCustomer" runat="server" Text="Back" CssClass="btn-generate"
                    OnClientClick="showTab('pnlCustomer', document.querySelectorAll('.tab-header')[0]); return false;" />
                <asp:Button ID="btnNextToFaults" runat="server" Text="Next" CssClass="btn-generate"
                    OnClientClick="showTab('pnlFaults', document.querySelectorAll('.tab-header')[2]); return false;" />
            </div>
        </asp:Panel>

        <!-- Faults Tab -->
        <asp:Panel ID="pnlFaults" runat="server" CssClass="tab-panel" ClientIDMode="Static">
            <div class="faults-container">

                <!-- LEFT SIDE -->
                <div class="faults-left">
                    <div class="form-group">
                        <label>Fault Description:</label>
                        <asp:TextBox ID="txtFault" runat="server" ClientIDMode="Static" />
                    </div>
                    <div class="form-group">
                        <label>Select Part:</label>
                        <asp:DropDownList ID="cmbPart" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="cmbPart_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label>Quantity:</label>
                        <asp:TextBox ID="nudQuantity" runat="server" Text="1" ClientIDMode="Static" />
                    </div>

                    <asp:Button ID="btnAddFault" runat="server" Text="Add Fault" CssClass="btn-generate"
                        OnClick="btnAddFault_Click" />

                    <asp:GridView ID="gvFaults" runat="server" CssClass="gridview" AutoGenerateColumns="true" />
                </div>

                <!-- RIGHT SIDE -->
                <div class="faults-summary">
                    <label>Fault Summary:</label>
                    <asp:TextBox ID="txtFaultSummary" runat="server" TextMode="MultiLine" Rows="20"
                        ReadOnly="true" CssClass="gridview" />
                </div>

            </div>

            <div class="button-row">
                <asp:Button ID="btnBackToVehicle" runat="server" Text="Back" CssClass="btn-generate"
                    OnClientClick="showTab('pnlVehicle', document.querySelectorAll('.tab-header')[1]); return false;" />
                <asp:Button ID="btnGenerateQuotation" runat="server" Text="Generate Quotation"
                    CssClass="btn-generate" OnClick="btnGenerateQuotation_Click" />
            </div>
        </asp:Panel>

        <script>
            document.addEventListener("DOMContentLoaded", function () {
                var searchBox = document.getElementById("<%= txtSearchCustomer.ClientID %>");
                var grid = document.getElementById("<%= gvCustomerVehicles.ClientID %>");

                searchBox.addEventListener("keyup", function () {
                    var filter = searchBox.value.toLowerCase();
                    var rows = grid.getElementsByTagName("tr");

                    for (var i = 1; i < rows.length; i++) {
                        var nameCell = rows[i].cells[2]; // customerName column
                        if (nameCell) {
                            var txtValue = nameCell.textContent || nameCell.innerText;
                            rows[i].style.display = txtValue.toLowerCase().indexOf(filter) > -1 ? "" : "none";
                        }
                    }
                });
            });
        </script>
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




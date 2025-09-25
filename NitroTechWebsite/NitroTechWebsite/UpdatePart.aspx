<%@ Page Title="Parts" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UpdatePart.aspx.cs" Inherits="NitroTechWebsite.UpdatePart" %>

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

        .form-group {
            display: flex;
            align-items: center;
            margin-bottom: 15px;
            max-width: 600px;
            margin-left: auto;
            margin-right: auto;
        }

        .form-group label {
            width: 250px;
            font-weight: bold;
            text-align: right;
            margin-right: 75px;
        }

        .form-group select,
        .form-group input {
            flex: 1;
            padding: 8px;
            border: 1px solid #ccc;
            border-radius: 5px;
            color: black; 
        }

        .btn1 {
            display: flex;
            justify-content: center;
            margin-top: 20px;
        }

        .btn1 input[type=submit],
        .btn1 button {
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

        .btn1 input[type=submit]:hover,
        .btn1 button:hover {
            background-color: #3c00a0;
            color: #fff;
            border-radius: 5px;
            box-shadow: 0 0 5px #3c00a0, 0 0 25px #3c00a0, 0 0 50px #3c00a0, 0 0 100px #3c00a0;
        }

        .message {
            text-align: center;
            margin-top: 15px;
            font-weight: bold;
        }

        .success {
            color: #28a745;
        }

        .error {
            color: #dc3545;
        }
    </style>

    <div class="form-container">
        <h2 style="text-align:center;"><%: Title %></h2>
        <h3 style="text-align:center;">Update Part</h3>

        <!-- hidden field for active tab -->
        <asp:HiddenField ID="ActiveTab" runat="server" />

        <ul class="nav nav-tabs" id="partTabs" role="tablist">
            <li class="active">
                <a href="#tab1" role="tab" data-toggle="tab">Increase Levels</a>
            </li>
            <li>
                <a href="#tab2" role="tab" data-toggle="tab">Decrease Levels</a>
            </li>
        </ul>

        <div class="tab-content" style="margin-top:20px;">
            <!-- Tab 1: Increase -->
            <div class="tab-pane fade in active" id="tab1">
                <div class="form-group">
                    <label for="partNameInc">Name of Part:</label>
                    <asp:DropDownList ID="partNameInc" runat="server"></asp:DropDownList>
                </div>

                <div class="form-group">
                    <label for="quantityInc">Quantity to Add:</label>
                    <asp:TextBox ID="quantityInc" runat="server" TextMode="Number"></asp:TextBox>
                </div>

                <div class="btn1">
                    <asp:Button ID="btnIncrease" runat="server" CssClass="btn" Text="Update Part Levels" OnClick="btnIncrease_Click" />
                </div>

                <asp:Label ID="lblMessageInc" runat="server" CssClass="message"></asp:Label>
            </div>

            <!-- Tab 2: Decrease -->
            <div class="tab-pane fade" id="tab2">
                <div class="form-group">
                    <label for="partNameDec">Name of Part:</label>
                    <asp:DropDownList ID="partNameDec" runat="server"></asp:DropDownList>
                </div>

                <div class="form-group">
                    <label for="quantityDec">Quantity Used:</label>
                    <asp:TextBox ID="quantityDec" runat="server" TextMode="Number"></asp:TextBox>
                </div>

                <div class="btn1">
                    <asp:Button ID="btnDecrease" runat="server" CssClass="btn" Text="Update Part Levels" OnClick="btnDecrease_Click" />
                </div>

                <asp:Label ID="lblMessageDec" runat="server" CssClass="message"></asp:Label>
            </div>
        </div>
    </div>

    <script>
        // Save the selected tab before postback
        $(function () {
            $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
                var activeTab = $(e.target).attr("href");
                $('#<%= ActiveTab.ClientID %>').val(activeTab);
            });

            // Restore the selected tab after postback
            var selectedTab = $('#<%= ActiveTab.ClientID %>').val();
            if (selectedTab) {
                $('#partTabs a[href="' + selectedTab + '"]').tab('show');
            }
        });
    </script>
</asp:Content>

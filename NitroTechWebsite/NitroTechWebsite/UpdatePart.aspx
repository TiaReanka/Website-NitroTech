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

        .form-group input,
        .form-group select {
            flex: 1;
            padding: 8px;
            border: 1px solid #ccc;
            border-radius: 5px;
            color: black; 
        }

        .form-container button {
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

        .form-container button:hover {
            background-color: purple;
        }
    </style>

    <div class="form-container">
        <h2 style="text-align:center;"><%: Title %></h2>
        <h3 style="text-align:center;">Update Part</h3>
        <ul class="nav nav-tabs" role="tablist">
            <li class="active">
                <a href="#tab1" role="tab" data-toggle="tab">Increase Levels</a>
            </li>
            <li>
                <a href="#tab2" role="tab" data-toggle="tab">Decrease Levels</a>
            </li>
        </ul>

        <div class="tab-content" style="margin-top:20px;">
            <div class="tab-pane fade in active" id="tab1">
                <div class="form-group">
                    <label for="partName">Name of Part:</label>
                    <select id="partNameInc" name="partName">
                        <option value="">-- Select Name of Part --</option>
                        <option value="PART1">Tires</option>
                        <option value="PART2">Screws</option>
                        <option value="PART3">Brake pads</option>
                    </select>
                </div>

                <div class="form-group">
                    <label for="quantity">Quantity to Add:</label>
                    <input type="number" id="quantityInc" name="quantity" />
                </div>

                <button type="submit">Update Part Levels</button>
            </div>

            <div class="tab-pane fade" id="tab2">
                <div class="form-group">
                    <label for="partName">Name of Part:</label>
                    <select id="partNameDec" name="partName">
                        <option value="">-- Select Name of Part --</option>
                        <option value="PART1">Tires</option>
                        <option value="PART2">Screws</option>
                        <option value="PART3">Brake pads</option>
                    </select>
                </div>

                <div class="form-group">
                    <label for="quantity">Quantity Used:</label>
                    <input type="number" id="quantityDec" name="quantity" />
                </div>

                <button type="submit">Update Part Levels</button>
            </div>
        </div>
    </div>
</asp:Content>

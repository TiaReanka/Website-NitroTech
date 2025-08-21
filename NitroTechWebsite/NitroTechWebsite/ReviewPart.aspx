<%@ Page Title="Parts" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReviewPart.aspx.cs" Inherits="NitroTechWebsite.ReviewPart" %>

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
            border-collapse: collapse;
            text-align: left;
            font-size: 14px;
            min-height: 300px; 
        }

        .grid-container th, .grid-container td {
            border: 1px solid #ccc;
            padding: 8px;
            text-align: center;
        }

        .grid-container th {
            background: #eee;
        }
    </style>

    <div class="form-container">
        <h2><%: Title %></h2>
        <h3>Review Parts Report</h3>

        <div class="form-group">
            <select id="partNameInc" name="partName" runat="server">
                <option value="">-- Select Name of Part --</option>
                <option value="PART1">Tires</option>
                <option value="PART2">Screws</option>
                <option value="PART3">Brake pads</option>
            </select>
            <button type="submit">Search</button>
        </div>

        <!-- GridView to show results -->
        <div class="grid-container">
            <asp:GridView ID="PartsGrid" runat="server" AutoGenerateColumns="true" />
        </div>
    </div>
</asp:Content>

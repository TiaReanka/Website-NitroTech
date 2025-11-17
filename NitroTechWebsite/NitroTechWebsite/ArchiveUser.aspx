<%@ Page Title="Account" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ArchiveUser.aspx.cs" Inherits="NitroTechWebsite.ArchiveUser" %>

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
            flex-direction: column; /* so textbox + button stack nicely */
            align-items: center;
            margin-bottom: 15px;
            max-width: 600px;
            margin-left: auto;
            margin-right: auto;
        }

            .form-group label {
                width: 100%;
                font-weight: bold;
                text-align: center;
                margin-bottom: 10px;
                color: white;
            }

        /* Style for ASP.NET TextBox (renders as input[type=text]) */
        .form-control {
            width: 100%;
            max-width: 400px;
            padding: 8px;
            border: 1px solid #ccc;
            border-radius: 5px;
            font-size: 14px;
        }

        /* ASP.NET Button (renders as input[type=submit]) */
        .btn {
            display: block;
            max-width: 200px;
            margin: 20px auto;
            padding: 12px;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            font-size: 16px;
            background: transparent;
            border: 2px solid #3c00a0;
            border-radius: 5px;
            color: white;
        }

        .btn1:hover {
            background-color: #3c00a0;
            color: #fff;
            border-radius: 5px;
            box-shadow: 0 0 5px #3c00a0, 0 0 25px #3c00a0, 0 0 50px #3c00a0, 0 0 100px #3c00a0;
        }

        /* For any dropdowns later */
        .form-group select {
            width: 100%;
            max-width: 400px;
            padding: 8px;
            border: 1px solid #ccc;
            border-radius: 6px;
            font-size: 14px;
            color: black;
            background-color: white;
        }

            .form-group select option[value=""] {
                color: gray;
            }
    </style>

    <div class="form-container">
    <h2><%: Title %></h2>
    <h3>Archive User</h3>

    <asp:Panel ID="pnlArchiveUser" runat="server" CssClass="form-group">
        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label><br />

        <asp:TextBox 
            ID="txtUsername" 
            runat="server" 
            CssClass="form-control" 
            placeholder="Enter username">
        </asp:TextBox>
        <br />

        <asp:Button 
            ID="btnArchive" 
            runat="server" 
            Text="Archive User" 
            CssClass="btn btn1"
            OnClick="btnArchive_Click" />
    </asp:Panel>
</div>

</asp:Content>

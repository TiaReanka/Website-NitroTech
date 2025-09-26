<%@ Page Title="Contact Us" Language="C#" MasterPageFile="~/Site.Master" 
    AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="NitroTechWebsite.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .form-container {
            max-width: 800px;
            margin: 60px auto;
            padding: 40px;
            background: #191919;
            border-radius: 12px;
            box-shadow: 0px 6px 15px rgba(0,0,0,0.25);
            text-align: center;
        }

        .form-container h2 {
            text-align: center;
            margin-bottom: 30px;
            color: white;
        }

        .form-group {
            margin: 20px auto;
            max-width: 700px;
            text-align: center;
        }

        .form-group label {
            display: block;
            margin-bottom: 10px;
            font-weight: bold;
            color: white;
        }

        .form-group textarea {
            width: 100%;
            height: 150px;
            padding: 12px;
            border: 1px solid #ccc;
            border-radius: 8px;
            font-size: 15px;
            resize: vertical;
        }

        .btn {
            display: block;
            max-width: 200px;
            margin: 20px auto;
            padding: 14px;
            background-color: #1a2db9;
            color: white;
            border: none;
            border-radius: 8px;
            cursor: pointer;
            font-size: 17px;
        }

        .btn:hover {
            background-color: purple;
        }
    </style>

    <div class="form-container">
        <h2>Give AMG Petronas feedback</h2>

    <div class="form-group">
      <label for="txtFeedback">Your Feedback:</label>
      <asp:TextBox ID="txtFeedback" runat="server" TextMode="MultiLine" 
        placeholder="Type your feedback here..." 
        style="color:black; background-color:white; width:100%; height:150px; padding:12px; border-radius:8px; border:1px solid #ccc;" />
    </div>


        <asp:Button ID="btnSubmitFeedback" runat="server" Text="Submit Feedback" CssClass="btn" OnClick="btnSubmitFeedback_Click" />
    </div>
</asp:Content>

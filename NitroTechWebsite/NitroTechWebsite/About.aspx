<%@ Page Title="About NitroTech" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="NitroTechWebsite.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        body{
            margin: 0;
            background: linear-gradient(to right, purple, #1a2db9);
            color: white;
        }
        .about-section {
            background: linear-gradient(to right, purple, #1a2db9);
            padding: 20px;
            border-radius: 10px;
            color: white; /* for better text visibility on dark background */
        }

        h2 {
            color: #f0f0f0;
            border-bottom: 2px solid rgba(255, 255, 255, 0.5);
            padding-bottom: 10px;
        }

        h3 {
            color: #e0e0e0;
        }
    </style>

    <div class="about-section">
        <h2><%: Title %></h2>
        <h3>NitroTech is made for the automotive industry.</h3>
        <p>
           In 2025, NitroTech was established as a software service for the automobile sector.  It offers a complete solution for overseeing important dealership functions.  The platform is intended to assist automakers in increasing overall productivity and streamlining their operations.
        </p>
        <p>
 Important attributes and offerings:<br />
            <ul>
                <li>NitroTech's software gives dealerships a unified platform to manage their business by combining several crucial operations into a single system.  Among its primary services are:</li><br />

                <li>Vehicle Tracking: Automakers can use the program to keep tabs on vehicles as they arrive at their showroom.  They can constantly keep an eye on stock levels thanks to this real-time visibility into the car inventory.</li><br />

                <li>Payment Monitoring: NitroTech has tools for monitoring payments.  This aids dealerships in handling billing, invoices, and financial transactions—all of which are essential for preserving a positive cash flow.</li><br />

                <li>Management of Inventory: A major feature is its ability to manage inventory. This goes beyond just tracking vehicles; it also helps dealerships control and monitor their stock of parts and other products. This functionality helps minimize waste and ensures that businesses have the right items available when needed.</li>
        
            </ul>
 </p>
    </div>
</asp:Content>


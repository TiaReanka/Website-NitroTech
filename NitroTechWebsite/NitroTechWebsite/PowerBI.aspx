<%@ Page Title="Analysis Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PowerBI.aspx.cs" Inherits="NitroTechWebsite.PowerBI" %>
<%@ Register Assembly="System.Web.DataVisualization"
    Namespace="System.Web.UI.DataVisualization.Charting"
    TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Heading + Month Selector -->
    <div style="display: flex; align-items: center; justify-content: space-between; margin-bottom: 20px; margin-top: 20px;">
        <!-- Page Heading with Right-Fading Underline -->
        <h2 class="dashboard-heading" style="margin:0; padding-bottom:5px; position:relative;">
            Analysis Dashboard
            <span style="position:absolute; left:0; bottom:0; width:100%; height:2px; 
                         background: linear-gradient(to right, white, rgba(255,255,255,0));"></span>
        </h2>

        <!-- Month Selector with Caption -->
        <div style="display: flex; align-items: center; gap: 10px;">
            <span style="color:white; font-size:14px;">According to month</span>
            <asp:DropDownList ID="ddlMonth" runat="server" CssClass="month-dropdown" style="padding:5px 10px; font-size:14px; border-radius:5px;">
                <asp:ListItem Text="January" Value="1"></asp:ListItem>
                <asp:ListItem Text="February" Value="2"></asp:ListItem>
                <asp:ListItem Text="March" Value="3"></asp:ListItem>
                <asp:ListItem Text="April" Value="4"></asp:ListItem>
                <asp:ListItem Text="May" Value="5"></asp:ListItem>
                <asp:ListItem Text="June" Value="6"></asp:ListItem>
                <asp:ListItem Text="July" Value="7"></asp:ListItem>
                <asp:ListItem Text="August" Value="8"></asp:ListItem>
                <asp:ListItem Text="September" Value="9"></asp:ListItem>
                <asp:ListItem Text="October" Value="10"></asp:ListItem>
                <asp:ListItem Text="November" Value="11"></asp:ListItem>
                <asp:ListItem Text="December" Value="12"></asp:ListItem>
            </asp:DropDownList>
        </div>
    </div>

    <style>
        /* Heading underline */
        .dashboard-heading {
            display: inline-block;
            position: relative;
            padding-bottom: 5px;
        }

        .dashboard-heading::after {
            content: "";
            position: absolute;
            left: 0;
            bottom: 0;
            width: 100%;
            height: 2px;
            background: linear-gradient(to right, white, rgba(255,255,255,0));
        }

        /* Container for cards */
        .cards-container {
            display: flex;
            justify-content: space-between;
            margin-top: 20px;
            margin-bottom: 40px;
        }

        /* Individual cards with horizontal layout for icon + content */
        .dashboard-card {
            background-color: rgba(255, 255, 255, 0.1);
            color: white;
            padding: 20px;
            flex: 1;
            margin: 0 10px;
            border-radius: 8px;
            display: flex;
            align-items: center;
            transition: transform 0.2s ease, background-color 0.2s ease;
        }

        .dashboard-card:hover {
            transform: translateY(-5px);
            background-color: rgba(255, 255, 255, 0.2);
        }

        /* Icon on left */
        .dashboard-card img.icon {
            width: 40px;
            height: 40px;
            margin-right: 15px;
        }

        /* Text content inside card */
        .card-content {
            display: flex;
            flex-direction: column;
            justify-content: center;
        }

        .card-content .title {
            font-weight: bold;
            font-size: 16px;
        }

        .card-content .number {
            font-size: 24px;
            margin-top: 5px;
        }

        .jobs-values {
            display: flex;
            align-items: center;
            gap: 5px;
            font-size: 24px;
            font-weight: bold;
        }

        .vs-text {
            color: #fff;
            font-weight: normal;
        }

        .pending {
            color: #FFD700;
        }

        .completed {
            color: lawngreen;
        }

        .pending-text {
            color: #FFD700;
        }

        .completed-text {
            color: lawngreen;
        }

        .month-dropdown {
            padding: 5px 10px;
            border-radius: 5px;
            font-size: 14px;
            margin-right: 10px;
            color: black;
        }

        /* Large graph containers */
        .graph-container {
            display: flex;
            justify-content: space-between;
            margin-top: 30px;
            gap: 20px;
        }

        .graph-box {
            background-color: rgba(255,255,255,0.1);
            flex: 1;
            height: 300px; /* large enough for graphs */
            border-radius: 10px;
            display: flex;
            justify-content: center;
            align-items: center;
            color: white;
            font-weight: bold;
            font-size: 16px;
        }
    </style>

    <!-- Cards Section -->
    <div class="cards-container">

        <!-- Total Customers -->
        <div class="dashboard-card">
            <img class="icon" runat="server" src="~/Images/customer.png" alt="Total Customers" />
            <div class="card-content">
                <span class="title">Total Customers</span>
                <asp:Label ID="lblTotalCustomers" runat="server" Text="0" CssClass="number"></asp:Label>
            </div>
        </div>

        <!-- Total Payments -->
        <div class="dashboard-card">
            <img class="icon" runat="server" src="~/Images/payment-method.png" alt="Total Payments" />
            <div class="card-content">
                <span class="title total-payments-title">Total Payments</span>
                <asp:Label ID="lblTotalPayments" runat="server" Text="R0" CssClass="number"></asp:Label>
            </div>
        </div>

        <!-- Pending vs Completed Jobs -->
        <div class="dashboard-card">
            <img class="icon" runat="server" src="~/Images/job-offer.png" alt="Jobs" />
            <div class="card-content">
                <span class="title">
                    <span class="completed-text">Completed Jobs</span> vs <span class="pending-text">Pending Jobs</span>
                </span>
                <div class="jobs-values">
                    <asp:Label ID="lblCompletedJobs" runat="server" Text="0" CssClass="number completed"></asp:Label>
                    <span class="vs-text">vs</span>
                    <asp:Label ID="lblPendingJobs" runat="server" Text="0" CssClass="number pending"></asp:Label>
                </div>
            </div>
        </div>

        <!-- Popular Manufacturer -->
        <div class="dashboard-card">
            <img class="icon" runat="server" src="~/Images/car-repair.png" alt="Popular Manufacturer" />
            <div class="card-content">
                <span class="title">Popular Manufacturer</span>
                <asp:Label ID="lblPopularManufacturer" runat="server" Text="-" CssClass="number"></asp:Label>
            </div>
        </div>

    </div>


<!-- Graph Section -->
    <iframe title="NitroTechV3" width="600" height="373.5"
src="https://app.powerbi.com/view?r=eyJrIjoiYjRjYjU2NjYtMDUwZS00ZmQ3LWIxMzQtMzUxYzExMDQ1ZDU1IiwidCI6IjIyNjgyN2Q2LWE5ZDAtNDcwZC04YzE1LWIxNDZiMDE5MmQ1MSIsImMiOjh9&pageName=e973eb489fdf8cf01fc3"
frameborder="0" allowFullScreen="true"></iframe>

</asp:Content>

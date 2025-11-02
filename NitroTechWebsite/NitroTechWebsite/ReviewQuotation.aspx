<%@ Page Title="Add Job" Language="C#" MasterPageFile="~/Site.Master" 
    AutoEventWireup="true" CodeBehind="ReviewQuotation.aspx.cs" Inherits="NitroTechWebsite.AddJob" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server"> 
    <style> 
        .form-container { 
            max-width: 1000px; 
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
            max-width: 900px; 
            text-align: center; 
        } 
 
        .form-group label { 
            display: block; 
            margin-bottom: 10px; 
            font-weight: bold; 
            color: white; 
        } 
 
        .form-group select { 
            display: block; 
            margin: 0 auto; 
            width: 400px;               
            padding: 12px; 
            border: 1px solid #ccc; 
            border-radius: 8px; 
            font-size: 15px; 
            min-height: 44px; 
            line-height: 1.2; 
            color: black; 
            background-color: white; 
        }

        .form-container button,
        .btn {
            display: block;
            max-width: 200px;
            margin: 20px auto;
            padding: 14px;
            color: white;
            border: none;
            border-radius: 8px;
            cursor: pointer;
            font-size: 17px;
            background: transparent;
            border: 2px solid #3c00a0;
            border-radius: 5px;
        }

            .form-container button:hover,
            .btn:hover {
                background-color: #3c00a0;
                color: #fff;
                border-radius: 5px;
                box-shadow: 0 0 5px #3c00a0, 0 0 25px #3c00a0, 0 0 50px #3c00a0, 0 0 100px #3c00a0;
            } 

        .grid-label { 
            display: block; 
            width: 90%; 
            margin: 20px auto 10px auto; /* aligns with grid width */
            text-align: left; 
            font-weight: bold; 
            font-size: 16px; 
            color: white; 
        } 
 
        .gridview { 
            width: 90%; 
            margin: 10px auto 30px auto; 
            border-collapse: collapse; 
            background: white; 
            border-radius: 8px; 
            overflow: hidden; 
        } 
 
        .gridview th { 
            background: #1a2db9; 
            color: white; 
            padding: 12px; 
            text-align: left; 
        } 
 
        .gridview td { 
            padding: 10px; 
            border-bottom: 1px solid #ddd; 
            color: black;
        } 
 
        .gridview tr:nth-child(even) { 
            background-color: #f9f9f9; 
        } 
    </style> 
 
    <div class="form-container"> 
        <h2><%: Title %></h2> 
 
        <div class="form-group"> 
            <label for="ddlQuotations">Select Quotation:</label> 
            <asp:DropDownList ID="ddlQuotations" runat="server" CssClass="quotation-dropdown"> 
            </asp:DropDownList> 
        </div> 
 
        <asp:Button ID="btnAddJob" runat="server" Text="Add Job" CssClass="btn" 
            OnClick="btnAddJob_Click" /> 

        <!-- Label for jobs added -->
        <asp:Label ID="lblJobsAdded" runat="server" Text="Jobs added:" CssClass="grid-label"></asp:Label>
 
        <asp:GridView ID="gvJobs" runat="server" AutoGenerateColumns="true" 
            CssClass="gridview" OnRowDataBound="gvJobs_RowDataBound"> 
        </asp:GridView> 
    </div> 
</asp:Content>

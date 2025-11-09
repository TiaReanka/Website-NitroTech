<%@ Page Title="Analysis Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PowerBI.aspx.cs" Inherits="NitroTechWebsite.PowerBI" %>
<%@ Register Assembly="System.Web.DataVisualization"
    Namespace="System.Web.UI.DataVisualization.Charting"
    TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

   <style>
    .powerbi-container {
      width: 100%;
      height: 95vh; /* full viewport height */
      padding: 0;
      margin: 0;
    }

    .powerbi-container iframe {
      width: 100%;
      height: 100%;
      border: none;
      display: block;
    }
  </style>

  <div class="powerbi-container">
    <iframe title="NitroTechV3"
      src="https://app.powerbi.com/view?r=eyJrIjoiNmQ3M2JmYTAtYmZkYS00MWRjLThmMGEtOGFlNzE3N2Y5MzllIiwidCI6IjIyNjgyN2Q2LWE5ZDAtNDcwZC04YzE1LWIxNDZiMDE5MmQ1MSIsImMiOjh9"
      allowFullScreen>
    </iframe>
  </div>

</asp:Content>

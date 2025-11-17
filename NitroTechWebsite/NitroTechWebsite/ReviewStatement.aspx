<%@ Page Title="Review Statements" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReviewStatement.aspx.cs" Inherits="NitroTechWebsite.ReviewStatement" %> 
 
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
    max-width: 500px;
    display: flex;             /* make it flex container */
    flex-direction: column;    /* stack label and dropdown vertically */
    align-items: center;       /* center horizontally */
    text-align: center;
}

 
        .form-group label { 
            display: block; 
            margin-bottom: 10px; 
            font-weight: bold; 
            color: white; 
        } 
 
        .form-group input[type="text"] { 
            padding: 12px; 
            border: 1px solid #ccc; 
            border-radius: 8px; 
            font-size: 15px; 
            width: 100%; 
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
 
        .gridview { 
            width: 90%; 
            margin: 30px auto; 
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

         .btn1 {
     display: flex;
     justify-content: center; /* center horizontally */
     margin-top: 20px;
         }

     .btn1 input[type=submit] {
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

         .btn1 input[type=submit]:hover {
             background-color: #3c00a0;
             color: #fff;
             border-radius: 5px;
             box-shadow: 0 0 5px #3c00a0, 0 0 25px #3c00a0, 0 0 50px #3c00a0, 0 0 100px #3c00a0;
         }
    </style> 
 
    <div class="form-container"> 
        <h2><%: Title %></h2> 
 
        <div class="form-group">
    <label for="ddlCustomer">Search customer:</label>
    <asp:DropDownList 
        ID="ddlCustomer" 
        runat="server"
        CssClass="form-control">
    </asp:DropDownList>
</div>



        
    
        <div class="btn1">
            <asp:Button ID="Button1" runat="server" Text="Search" CssClass="btn" OnClick="btnSearch_Click" /> 
        </div>

        

 
        <asp:GridView ID="gvStatements" runat="server" AutoGenerateColumns="true" 
CssClass="gridview"> 
        </asp:GridView> 
    </div> 
</asp:Content> 
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NitroTechWebsite
{
    public partial class ReviewQuotations : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDummyGrid();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // Currently just rebind dummy data; you can later filter by customer name & quotation number
            BindDummyGrid();
        }

        private void BindDummyGrid()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("QuotationID");
            dt.Columns.Add("CustomerName");
            dt.Columns.Add("Date");
            dt.Columns.Add("TotalAmount");

            // Sample data 
            dt.Rows.Add("QUO001", "Alice", "2025-08-01", "R 1500.00");
            dt.Rows.Add("QUO002", "Bob", "2025-08-05", "R 2200.00");
            dt.Rows.Add("QUO003", "Charlie", "2025-08-10", "R 1800.00");

            gvQuotations.DataSource = dt;
            gvQuotations.DataBind();
        }
    }
}
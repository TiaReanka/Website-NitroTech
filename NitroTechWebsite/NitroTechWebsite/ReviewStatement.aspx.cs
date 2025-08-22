using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NitroTechWebsite
{
    public partial class ReviewStatement : System.Web.UI.Page
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
            // For now, ignore the search box and just rebind dummy data 
            BindDummyGrid();
        }

        private void BindDummyGrid()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("StatementID");
            dt.Columns.Add("CustomerName");
            dt.Columns.Add("Date");
            dt.Columns.Add("Amount");

            // Add some sample rows 
            dt.Rows.Add("STMT001", "Alice", "2025-08-01", "R 1500.00");
            dt.Rows.Add("STMT002", "Bob", "2025-08-10", "R 2500.00");
            dt.Rows.Add("STMT003", "Charlie", "2025-08-15", "R 1800.00");

            gvStatements.DataSource = dt;
            gvStatements.DataBind();
        }
    }
}
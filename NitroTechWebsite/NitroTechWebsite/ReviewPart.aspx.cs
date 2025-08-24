using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace NitroTechWebsite
{
    public partial class ReviewPart : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDummyData();
            }
        }

        private void LoadDummyData()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PartID");
            dt.Columns.Add("PartName");
            dt.Columns.Add("Quantity");
            dt.Columns.Add("Price");

            dt.Rows.Add("P001", "Tires", "50", "R100");
            dt.Rows.Add("P002", "Screws", "200", "R5");
            dt.Rows.Add("P003", "Brake Pads", "80", "R60");

            PartsGrid.DataSource = dt;
            PartsGrid.DataBind();
        }
    }
}
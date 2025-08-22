using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace NitroTechWebsite
{
    public partial class Quotations : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtQuotationDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

                // Populate sample parts for fault tab 
                cmbPart.Items.Add(new ListItem("Tires"));
                cmbPart.Items.Add(new ListItem("Brake Pads"));
                cmbPart.Items.Add(new ListItem("Screws"));
            }
        }

        protected void btnAddFault_Click(object sender, EventArgs e)
        {
            var dt = ViewState["Faults"] as DataTable;
            if (dt == null)
            {
                dt = new DataTable();
                dt.Columns.Add("Part");
                dt.Columns.Add("FaultDescription");
                dt.Columns.Add("Quantity");
            }

            DataRow row = dt.NewRow();
            row["Part"] = cmbPart.SelectedValue;
            row["FaultDescription"] = txtFault.Text;
            row["Quantity"] = nudQuantity.Text;
            dt.Rows.Add(row);

            ViewState["Faults"] = dt;
            gvFaults.DataSource = dt;
            gvFaults.DataBind();

            cmbPart.SelectedIndex = 0;
            txtFault.Text = "";
            nudQuantity.Text = "1";
        }

        protected void btnGenerateQuotation_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Quotation generated successfully!');", true); 
        }
    }
}
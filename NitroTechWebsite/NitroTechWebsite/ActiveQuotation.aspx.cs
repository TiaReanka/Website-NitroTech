using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
                LoadQuotations();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadQuotations();
        }

        private string MapStatus(int status)
        {
            switch (status)
            {
                case 0: return "Created";
                case 1: return "Added as Job";
                case 2: return "Closed Job";
                case 3: return "Expired";
                case 4: return "Invoice already Generated";
                default: return "Unknown";
            }
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadQuotations();
        }

        private void LoadQuotations()
        {
            string baseQuery = @"SELECT q.quotationNumber, 
                                q.customerID, 
                                q.quotationDate, 
                                q.quotationTotal, 
                                q.quotationStatus, 
                                q.vehicleVIN
                         FROM tblQuotation q";
            string whereClause = "";
            string filterValue = txtSearch.Text.Trim();

            if (!string.IsNullOrEmpty(filterValue))
            {
                if (rbCustomerID.Checked)
                    whereClause = " WHERE q.customerID LIKE @value";
                else if (rbCustomerName.Checked)
                {
                    baseQuery = @"SELECT q.quotationNumber, 
                                 c.customerID, 
                                 q.quotationDate, 
                                 q.quotationTotal, 
                                 q.quotationStatus, 
                                 q.vehicleVIN
                          FROM tblQuotation q
                          INNER JOIN tblCustomer c ON q.customerID = c.customerID";
                    whereClause = " WHERE c.customerName LIKE @value";
                }
                else if (rbQuotationNumber.Checked)
                    whereClause = " WHERE q.quotationNumber LIKE @value";
                else if (rbVIN.Checked)
                    whereClause = " WHERE q.vehicleVIN LIKE @value";
            }

            string finalQuery = baseQuery + whereClause;

            using (var conn = DatabaseHelper.OpenConnection())
            using (var cmd = new SqlCommand(finalQuery, conn))
            {
                if (!string.IsNullOrEmpty(filterValue))
                {
                    cmd.Parameters.AddWithValue("@value", "%" + filterValue + "%");
                }

                using (var da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Add human-readable status
                    if (!dt.Columns.Contains("StatusText"))
                        dt.Columns.Add("StatusText", typeof(string));

                    foreach (DataRow row in dt.Rows)
                    {
                        int status = Convert.ToInt32(row["quotationStatus"]);
                        row["StatusText"] = MapStatus(status);
                    }

                    gvQuotations.DataSource = dt;
                    gvQuotations.DataBind();
                }
            }
        }

        protected void gvQuotations_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    switch (i)
                    {
                        case 0: e.Row.Cells[i].Text = "Quotation Number"; 
                            break;
                        case 1: e.Row.Cells[i].Text = "Customer ID"; 
                            break;
                        case 2: e.Row.Cells[i].Text = "Quotation Date"; 
                            break;
                        case 3: e.Row.Cells[i].Text = "Total Amount"; 
                            break;
                        case 4: e.Row.Cells[i].Text = "Status"; 
                            break;
                        case 5: e.Row.Cells[i].Text = "Vehicle VIN"; 
                            break;
                    }
                }
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";      
            rbCustomerID.Checked = true;  
            LoadQuotations();             
        }
    }

    //TODO: Limit search results to n number of records - Calvin
    //TODO: Add paging to gridview - Calvin
    //TODO: Add sorting to gridview - Calvin

    //TODO: CrystalReports - TIa
}
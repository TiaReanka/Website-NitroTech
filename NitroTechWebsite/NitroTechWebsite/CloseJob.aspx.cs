using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NitroTechWebsite
{
    public partial class CloseJob : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadQuotations();    // show dropdown with active jobs
                LoadClosedJobsGrid(); // grid shows active jobs
            }
        }

        // Populate dropdown with quotations that are active (status = 1)
        private void LoadQuotations()
        {
            ddlQuotations.Items.Clear();
            ddlQuotations.Items.Add(new ListItem("Select a quotation...", ""));

            string query = @"SELECT quotationNumber 
                             FROM tblQuotation 
                             WHERE quotationStatus = 1";  // active jobs

            using (var conn = DatabaseHelper.OpenConnection())
            using (var cmd = new SqlCommand(query, conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string quotationNumber = reader["quotationNumber"].ToString();
                    ddlQuotations.Items.Add(new ListItem("Quotation #" + quotationNumber, quotationNumber));
                }
            }
        }

        // Grid shows all active jobs (status = 1) on page load
        private void LoadClosedJobsGrid()
        {
            string query = @"SELECT 
                        q.quotationNumber AS [QuotationNumber], 
                        q.customerID AS [CustomerID], 
                        q.quotationDate AS [QuotationDate], 
                        q.quotationTotal AS [QuotationTotal], 
                        q.vehicleVIN AS [VehicleVIN]
                     FROM tblQuotation q
                     WHERE q.quotationStatus = 2"; // closed jobs only

            DataTable dt = new DataTable();
            using (var conn = DatabaseHelper.OpenConnection())
            using (var cmd = new SqlCommand(query, conn))
            using (var da = new SqlDataAdapter(cmd))
            {
                da.Fill(dt);
            }

            gvJobs.DataSource = dt;
            gvJobs.DataBind();
        }


        // Close job button click
        protected void btnCloseJob_Click(object sender, EventArgs e)
        {
            string selectedQuotation = ddlQuotations.SelectedValue;

            if (!string.IsNullOrEmpty(selectedQuotation))
            {
                string updateQuery = @"UPDATE tblQuotation 
                               SET quotationStatus = 2 
                               WHERE quotationNumber = @quotationNumber";

                using (var conn = DatabaseHelper.OpenConnection())
                using (var cmd = new SqlCommand(updateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@quotationNumber", selectedQuotation);
                    cmd.ExecuteNonQuery();
                }

                // Refresh dropdown (still active jobs) 
                LoadQuotations();
                // Refresh grid to show closed jobs
                LoadClosedJobsGrid();

                ScriptManager.RegisterStartupScript(this, GetType(),
                    "alertMessage",
                    $"alert('Quotation {selectedQuotation} successfully closed!');",
                    true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(),
                    "alertMessage",
                    "alert('⚠ Please select a quotation first.');",
                    true);
            }
        }


        // Optional: nice headers
        protected void gvJobs_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Text = "Quotation Number";
                e.Row.Cells[1].Text = "Customer ID";
                e.Row.Cells[2].Text = "Quotation Date";
                e.Row.Cells[3].Text = "Total Amount";
                e.Row.Cells[4].Text = "Vehicle VIN";
            }
        }


    }
}

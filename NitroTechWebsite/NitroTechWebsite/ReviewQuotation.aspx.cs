using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NitroTechWebsite
{
    public partial class AddJob : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadQuotations();   // only pending ones (status=0)
                LoadJobs();         // always show jobs with status=1
            }
        }

        private void LoadQuotations()
        {
            ddlQuotations.Items.Clear();
            ddlQuotations.Items.Add(new ListItem("Select a quotation...", ""));

            string query = @"SELECT quotationNumber 
                             FROM tblQuotation 
                             WHERE quotationStatus = 0";

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

        private void LoadJobs()
        {
            string query = @"SELECT 
                                q.quotationNumber AS [QuotationNumber], 
                                q.customerID AS [CustomerID], 
                                q.quotationDate AS [QuotationDate], 
                                q.quotationTotal AS [QuotationTotal], 
                                q.vehicleVIN AS [VehicleVIN]
                             FROM tblQuotation q
                             WHERE q.quotationStatus = 1";

            using (var conn = DatabaseHelper.OpenConnection())
            using (var cmd = new SqlCommand(query, conn))
            using (var da = new SqlDataAdapter(cmd))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvJobs.DataSource = dt;
                gvJobs.DataBind();

                
            }
        }

        protected void btnAddJob_Click(object sender, EventArgs e)
        {
            string selectedQuotation = ddlQuotations.SelectedValue;

            if (!string.IsNullOrEmpty(selectedQuotation))
            {
                string updateQuery = @"UPDATE tblQuotation 
                                       SET quotationStatus = 1 
                                       WHERE quotationNumber = @quotationNumber";

                using (var conn = DatabaseHelper.OpenConnection())
                using (var cmd = new SqlCommand(updateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@quotationNumber", selectedQuotation);
                    cmd.ExecuteNonQuery();
                }

                // Refresh both lists
                LoadQuotations();   // refresh pending list
                LoadJobs();         // refresh jobs list

                ScriptManager.RegisterStartupScript(this, GetType(),
                    "alertMessage",
                    $"alert('Quotation {selectedQuotation} successfully added as a job!');",
                    true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(),
                    "alertMessage",
                    "alert('Please select a quotation first.');",
                    true);
            }
        }

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

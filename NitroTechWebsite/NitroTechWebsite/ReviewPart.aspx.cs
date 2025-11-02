using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Web;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;

namespace NitroTechWebsite
{
    public partial class ReviewPart : System.Web.UI.Page
    {
        private DataSet ds;
        private string GetConnectionString()
        {
            var cs = ConfigurationManager.ConnectionStrings["WstGrp4"]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(cs))
                throw new InvalidOperationException("❌ Missing connection string in Web.config.");
            return cs;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadPartsDropdown();
                LoadAllParts();
                LoadDataset();
                DisplayReport();
            }
        }

        private void LoadPartsDropdown()
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            using (SqlCommand cmd = new SqlCommand("SELECT DISTINCT partName FROM tblParts ORDER BY partName", conn))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);

                cmbSearch.DataSource = dt;
                cmbSearch.DataTextField = "partName";
                cmbSearch.DataValueField = "partName";
                cmbSearch.DataBind();

                // Add default placeholder again
                cmbSearch.Items.Insert(0, new ListItem("-- All Parts --", ""));
            }
        }

        private void LoadAllParts()
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM tblParts", conn))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                PartsGrid.DataSource = dt;
                PartsGrid.DataBind();
            }
        }

        private void LoadPartsByName(string partName)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM tblParts WHERE partName = @partName", conn))
            {
                cmd.Parameters.AddWithValue("@partName", partName);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    PartsGrid.DataSource = dt;
                    PartsGrid.DataBind();
                }
            }
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cmbSearch.SelectedValue))
            {
                LoadPartsByName(cmbSearch.SelectedValue);
            }
            else
            {
                LoadAllParts(); // If no part selected, show all
            }
        }

        private void LoadDataset()
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                conn.Open();

                // Create SQL commands for your tables
                SqlCommand cmdYourTable = new SqlCommand("SELECT * FROM tblParts", conn);

                // Add more tables as needed
                // SqlCommand cmdRelatedTable = new SqlCommand("SELECT * FROM [RelatedTable]", conn);

                // Create data adapters
                SqlDataAdapter adapter = new SqlDataAdapter(cmdYourTable);
                // SqlDataAdapter relatedAdapter = new SqlDataAdapter(cmdRelatedTable);

                // Initialize DataSet
                ds = new DataSet();
                ds.EnforceConstraints = false; // Prevents constraint conflicts

                // Fill DataSet with table data
                adapter.Fill(ds, "tblParts");
                // relatedAdapter.Fill(ds, "RelatedTable");
            }
        }

        //private DataTable FilterData()
        //{
        //    // Clone the table structure
        //    DataTable filteredData = ds.Tables["tblParts"].Clone();

        //    // Parse filter criteria
        //    DateTime startDate = DateTime.Parse(txtStartDate.Text);
        //    DateTime endDate = DateTime.Parse(txtEndDate.Text);
        //    string filterText = txtFilter.Text?.Trim();

        //    // Apply filters
        //    foreach (DataRow row in ds.Tables["tblParts"].Rows)
        //    {
        //        // Example: Filter by date
        //        DateTime recordDate = Convert.ToDateTime(row["DateColumn"]);

        //        // Example: Filter by text
        //        string recordName = row["NameColumn"]?.ToString();

        //        // Apply your filter conditions
        //        bool matchesDate = recordDate >= startDate && recordDate <= endDate;
        //        bool matchesFilter = string.IsNullOrEmpty(filterText) ||
        //                            recordName.Contains(filterText);

        //        if (matchesDate && matchesFilter)
        //        {
        //            filteredData.ImportRow(row);
        //        }
        //    }

        //    return filteredData;
        //}

        private void DisplayReport()
        {
            // Create report document
            ReportDocument rptDoc = new ReportDocument();

            try
            {
                // Load the Crystal Report file
                rptDoc.Load(Server.MapPath("~/PartsReport.rpt"));

                // Set the data source
                rptDoc.SetDataSource(ds);

                // Clear database login info (IMPORTANT!)
                foreach (CrystalDecisions.CrystalReports.Engine.Table table in rptDoc.Database.Tables)
                {
                    table.LogOnInfo.ConnectionInfo = new CrystalDecisions.Shared.ConnectionInfo();
                    table.ApplyLogOnInfo(table.LogOnInfo);
                }

                // Optional: Set report parameters
                // rptDoc.SetParameterValue("ParameterName", parameterValue);

                // Export to PDF and send to browser
                

                // Define temp folder and file path
                string tempFolder = Server.MapPath("~/TempReports/");
                if (!System.IO.Directory.Exists(tempFolder))
                    System.IO.Directory.CreateDirectory(tempFolder);

                string fileName = $"PartsReport_{Guid.NewGuid()}.pdf";
                string fullPath = System.IO.Path.Combine(tempFolder, fileName);

                // Export to PDF file
                rptDoc.ExportToDisk(ExportFormatType.PortableDocFormat, fullPath);

                // Open in new tab
                string fileUrl = ResolveUrl($"~/TempReports/{fileName}");
                string script = $"window.open('{fileUrl}', '_blank');";
                ClientScript.RegisterStartupScript(this.GetType(), "OpenReport", script, true);
            }
            catch (Exception ex)
            {
                ShowAlert($"Error generating report: {ex.Message}");
            }
            finally
            {
                rptDoc.Close();
                rptDoc.Dispose();
            }
        }

        private void ShowAlert(string message)
        {
            ClientScript.RegisterStartupScript(
                this.GetType(),
                "alert",
                $"alert('{message}');",
                true
            );
        }
    }
}

using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Web;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace NitroTechWebsite
{
    public partial class ReviewPart : System.Web.UI.Page
    {
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
                LoadReport();
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
                cmbSearch.Items.Insert(0, new ListItem("-- Select Name of Part --", ""));
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

        private void LoadReport()
        {
            string reportPath = Server.MapPath("~/PartsReport.rpt");

            if (!System.IO.File.Exists(reportPath))
                throw new Exception("Report not found at: " + reportPath);

            ReportDocument rpt = new ReportDocument();
            rpt.Load(reportPath);

            Crystal ds = GetReportData();

            if (ds.tblParts.Rows.Count == 0)
            {
                Response.Write("No parts found in the database.");
                return;
            }

            rpt.SetDataSource(ds);

            CrystalReportViewer1.ReportSource = rpt;
            CrystalReportViewer1.DataBind();
            CrystalReportViewer1.RefreshReport();
        }

        private Crystal GetReportData()
        {
            Crystal ds = new Crystal();

            using (SqlConnection con = new SqlConnection("Data Source=146.230.177.46;Initial Catalog=WstGrp4;Persist Security Info=True;User ID=WstGrp4;Password=3d55d;Encrypt=True;TrustServerCertificate=True"))
            {
                con.Open();

                SqlDataAdapter daParts = new SqlDataAdapter("SELECT * FROM tblParts", con);
                daParts.Fill(ds, "tblParts"); // Must match DataTable name in Crystal.xsd
            }

            return ds;
        }

    }
}

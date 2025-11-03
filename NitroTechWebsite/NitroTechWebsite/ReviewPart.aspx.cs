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
                LoadDataset();
                
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

        protected void btnReport_Click(object sender, EventArgs e)
        {
            DisplayReport();
        }

        protected void btnReport_Click(object sender, EventArgs e)
        {
            DisplayReport();
        }
    }
}

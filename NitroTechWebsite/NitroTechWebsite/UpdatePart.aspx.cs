using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace NitroTechWebsite
{
    public partial class UpdatePart : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindPartDropdowns();
                ActiveTab.Value = "#tab1"; // default tab
            }
        }

        private string GetConnectionString()
        {
            var cs = ConfigurationManager.ConnectionStrings["WstGrp4"]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(cs))
                throw new InvalidOperationException("❌ Missing connection string in Web.config.");
            return cs;
        }

        private void BindPartDropdowns()
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                string sql = "SELECT partID, partName FROM tblParts";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    partNameInc.DataSource = dr;
                    partNameInc.DataTextField = "partName";   // what user sees
                    partNameInc.DataValueField = "partID";    // actual value
                    partNameInc.DataBind();

                    dr.Close();

                    // re-run query for second dropdown
                    cmd.CommandText = "SELECT partID, partName FROM tblParts";
                    dr = cmd.ExecuteReader();

                    partNameDec.DataSource = dr;
                    partNameDec.DataTextField = "partName";
                    partNameDec.DataValueField = "partID";
                    partNameDec.DataBind();

                    dr.Close();
                }
            }

            // default option
            partNameInc.Items.Insert(0, new ListItem("-- Select Part --", ""));
            partNameDec.Items.Insert(0, new ListItem("-- Select Part --", ""));
        }

        protected void btnIncrease_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(partNameInc.SelectedValue))
            {
                lblMessageInc.Text = "⚠ Please select a part.";
                lblMessageInc.CssClass = "message error";
                return;
            }

            if (!int.TryParse(quantityInc.Text, out int qty) || qty <= 0)
            {
                lblMessageInc.Text = "⚠ Please enter a valid amount.";
                lblMessageInc.CssClass = "message error";
                return;
            }

            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                string sql = "UPDATE tblParts SET partQuantity = partQuantity + @qty WHERE partID = @id";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@qty", qty);
                    cmd.Parameters.AddWithValue("@id", partNameInc.SelectedValue);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            string partName = partNameInc.SelectedItem.Text;
            Alert($"{partName} has been increased successfully.");

            // reset fields
            quantityInc.Text = "";
            partNameInc.SelectedIndex = 0;

            // stay on Increase tab
            ActiveTab.Value = "#tab1";
        }

        protected void btnDecrease_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(partNameDec.SelectedValue))
            {
                lblMessageDec.Text = "⚠ Please select a part.";
                lblMessageDec.CssClass = "message error";
                return;
            }

            if (!int.TryParse(quantityDec.Text, out int qty) || qty <= 0)
            {
                lblMessageDec.Text = "⚠ Please enter a valid amount.";
                lblMessageDec.CssClass = "message error";
                return;
            }

            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                string sql = "UPDATE tblParts SET partQuantity = partQuantity - @qty WHERE partID = @id";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@qty", qty);
                    cmd.Parameters.AddWithValue("@id", partNameDec.SelectedValue);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            string partName = partNameDec.SelectedItem.Text;
            Alert($"{partName} has been decreased successfully.");

            // reset fields
            quantityDec.Text = "";
            partNameDec.SelectedIndex = 0;

            // stay on Decrease tab
            ActiveTab.Value = "#tab2";
        }

        private void Alert(string message)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "alert",
                $"alert('{message.Replace("'", "\\'")}');", true);
        }
    }
}

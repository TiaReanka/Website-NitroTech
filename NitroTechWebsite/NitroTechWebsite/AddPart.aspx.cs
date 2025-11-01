using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace NitroTechWebsite
{
    public partial class Parts : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnAddPart_Click(object sender, EventArgs e)
        {
            try
            {
                string partName = txtPartName.Text.Trim();

                if (string.IsNullOrWhiteSpace(partName))
                {
                    Alert("⚠ Part name cannot be empty");
                    return;
                }

                if (!decimal.TryParse(txtPrice.Text, out decimal price))
                {
                    Alert("⚠ Price must be a valid number (no letters or special characters).");
                    return;
      
                }

                if (!int.TryParse(txtQuantity.Text, out int qty) || qty <= 0)
                {
                    Alert("⚠ Quantity must be greater than 0.");
                    return;
                }

                if (!int.TryParse(txtReOrderAmt.Text, out int reorderLevel))
                {
                    Alert("⚠ Reorder level must be a valid number.");
                    return;
                }

                using (var conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();

                    // 1. Check if part already exists
                    using (var checkCmd = new SqlCommand(
                        "SELECT COUNT(*) FROM tblParts WHERE LOWER(partName) = LOWER(@name)", conn))
                    {
                        checkCmd.Parameters.AddWithValue("@name", partName);
                        int exists = (int)checkCmd.ExecuteScalar();

                        if (exists > 0)
                        {
                            Alert("❌ Part already exists in the database.");
                            return;
                        }
                    }

                    // 2. Insert new part
                    using (var insertCmd = new SqlCommand(
                        @"INSERT INTO tblParts (partName, partQuantity, partUnitPrice, partReOrderLevel) 
                          VALUES (@name, @qty, @price, @reorder)", conn))
                    {
                        insertCmd.Parameters.AddWithValue("@name", partName);
                        insertCmd.Parameters.AddWithValue("@qty", qty);
                        insertCmd.Parameters.AddWithValue("@price", price);
                        insertCmd.Parameters.AddWithValue("@reorder", reorderLevel);

                        insertCmd.ExecuteNonQuery();
                    }
                }

                Alert($"{partName} has been added successfully.");
                ClearForm();
            }
            catch (Exception ex)
            {
                Alert("Error: " + ex.Message);
            }
        }

        private string GetConnectionString()
        {
            var cs = ConfigurationManager.ConnectionStrings["WstGrp4"]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(cs))
                throw new InvalidOperationException("❌ Missing connection string in Web.config.");
            return cs;
        }

        private void ClearForm()
        {
            txtPartName.Text = "";
            txtPrice.Text = "";
            txtQuantity.Text = "";
            txtReOrderAmt.Text = "";
        }

        private void Alert(string message)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "alert",
                $"alert('{message.Replace("'", "\\'")}');", true);
        }
    }
}

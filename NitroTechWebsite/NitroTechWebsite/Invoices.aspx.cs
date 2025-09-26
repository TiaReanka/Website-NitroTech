using System;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NitroTechWebsite
{
    public partial class Invoices : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadQuotations();
                btnGenerateInvoice.Enabled = false; // initially disabled
            }
        }

        // Populate dropdown with quotations ready for invoicing (status = 2)
        private void LoadQuotations()
        {
            quotationNumber.Items.Clear();
            quotationNumber.Items.Add(new ListItem("-- Select Quotation --", ""));

            string query = @"SELECT quotationNumber 
                             FROM tblQuotation 
                             WHERE quotationStatus = 2";

            using (var conn = DatabaseHelper.OpenConnection())
            using (var cmd = new SqlCommand(query, conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string qNum = reader["quotationNumber"].ToString();
                    quotationNumber.Items.Add(new ListItem(qNum, qNum));
                }
            }
        }

        // Confirm button
        protected void btnConfirmQuotation_Click(object sender, EventArgs e)
        {
            string qNum = quotationNumber.SelectedValue;
            if (string.IsNullOrEmpty(qNum))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                    "alert('Please select a quotation.');", true);
                btnGenerateInvoice.Enabled = false;
                return;
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                $"alert('Quotation {qNum} confirmed.');", true);
            btnGenerateInvoice.Enabled = true;
        }

        // Generate Invoice button
        protected void btnGenerateInvoice_Click(object sender, EventArgs e)
        {
            string qNum = quotationNumber.SelectedValue;
            if (string.IsNullOrEmpty(qNum))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(),
                    "noQuotation",
                    "alert('⚠ Please select a quotation before generating an invoice.');",
                    true);
                return;
            }

            using (var conn = DatabaseHelper.OpenConnection())
            {
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // 1. Get quotation details
                    string selectQuery = @"SELECT quotationTotal, customerID, vehicleVIN 
                                   FROM tblQuotation 
                                   WHERE quotationNumber = @quotationNumber";

                    string customerID = "";
                    string vehicleVIN = "";
                    decimal quotationTotal = 0;

                    using (var cmd = new SqlCommand(selectQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@quotationNumber", qNum);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                customerID = reader["customerID"].ToString();
                                vehicleVIN = reader["vehicleVIN"].ToString();
                                quotationTotal = Convert.ToDecimal(reader["quotationTotal"]);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(),
                                    "noDetails",
                                    "alert('⚠ Quotation details not found.');",
                                    true);
                                return;
                            }
                        }
                    }

                    // 2. Update quotationStatus to 4 (invoice generated)
                    string updateQuery = @"UPDATE tblQuotation 
                                   SET quotationStatus = 4 
                                   WHERE quotationNumber = @quotationNumber";

                    using (var cmd = new SqlCommand(updateQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@quotationNumber", qNum);
                        cmd.ExecuteNonQuery();
                    }

                    // 3. Insert into tblInvoice
                    string invoiceNumber = "I" + qNum + vehicleVIN;

                    string insertQuery = @"INSERT INTO tblInvoice 
                                    (invoiceNumber, invoiceDate, customerID, quotationNumber, invoiceAmountDue, vehicleVIN)
                                   VALUES 
                                    (@invoiceNumber, @invoiceDate, @customerID, @quotationNumber, @invoiceAmountDue, @vehicleVIN)";

                    using (var cmd = new SqlCommand(insertQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@invoiceNumber", invoiceNumber);
                        cmd.Parameters.AddWithValue("@invoiceDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@customerID", customerID);
                        cmd.Parameters.AddWithValue("@quotationNumber", qNum);
                        cmd.Parameters.AddWithValue("@invoiceAmountDue", quotationTotal);
                        cmd.Parameters.AddWithValue("@vehicleVIN", vehicleVIN);

                        cmd.ExecuteNonQuery();
                    }

                    // 4. Update customerOwe in tblCustomer
                    string updateCustomerQuery = @"UPDATE tblCustomer 
                                           SET customerOwe = customerOwe + @amount 
                                           WHERE customerID = @customerID";

                    using (var cmd = new SqlCommand(updateCustomerQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@amount", quotationTotal);
                        cmd.Parameters.AddWithValue("@customerID", customerID);
                        cmd.ExecuteNonQuery();
                    }

                    // ✅ Commit transaction
                    transaction.Commit();

                    // ✅ Show popup
                    ScriptManager.RegisterStartupScript(this, this.GetType(),
                        "success",
                        $"alert('✔ Invoice {invoiceNumber} successfully generated for quotation {qNum}. Customer balance updated.');",
                        true);

                    // Refresh dropdown and disable Generate button until new confirm
                    LoadQuotations();
                    btnGenerateInvoice.Enabled = false;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    ScriptManager.RegisterStartupScript(this, this.GetType(),
                        "error",
                        $"alert('❌ Error generating invoice: {ex.Message}');",
                        true);
                }
            }
        }


    }
}

using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace NitroTechWebsite
{
    public partial class Payments : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCustomers();
            }
        }

        private void BindCustomers()
        {
            try
            {
                DataTable dt = GetCustomers();
                ddlCustomerID.Items.Clear();

                foreach (DataRow row in dt.Rows)
                {
                    string name = row["customerName"].ToString();
                    string id = row["customerID"].ToString();
                    ddlCustomerID.Items.Add(new System.Web.UI.WebControls.ListItem($"{name} - {id}", id));
                }

                ddlCustomerID.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Customer --", ""));
            }
            catch (Exception ex)
            {
                Alert("⚠ Failed to load customers: " + ex.Message);
            }
        }

        protected void btnAddPayment_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ddlCustomerID.SelectedValue))
            {
                Alert("⚠ Please select a customer.");
                return;
            }

            if (!decimal.TryParse(txtAmount.Text, out decimal amount))
            {
                Alert("⚠ Please enter a valid amount.");
                return;
            }

            if (!DateTime.TryParse(txtPaymentDate.Text, out DateTime paymentDate))
            {
                Alert("⚠ Please enter a valid date.");
                return;
            }

            try
            {
                AddPaymentAndUpdateCustomer(amount, paymentDate, ddlCustomerID.SelectedValue);
                Alert("The amount has been updated.");
                ddlCustomerID.SelectedIndex = 0;
                txtAmount.Text = "";
                txtPaymentDate.Text = "";
            }
            catch (Exception ex)
            {
                Alert("Error: " + ex.Message);
            }
        }

        private void Alert(string message)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('{message.Replace("'", "\\'")}');", true);
        }

        // -------------------
        // Payments-specific SQL queries
        // -------------------

        private string GetConnectionString()
        {
            var cs = ConfigurationManager.ConnectionStrings["WstGrp4"]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(cs))
                throw new InvalidOperationException("❌ Missing connection string in Web.config.");
            return cs;
        }

        private DataTable GetCustomers()
        {
            using (var conn = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand("SELECT customerID, customerName FROM tblCustomer WHERE customerOwe > 0 ORDER BY customerName", conn))
            using (var da = new SqlDataAdapter(cmd))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        private void AddPaymentAndUpdateCustomer(decimal amount, DateTime paymentDate, string customerID)
        {
            using (var conn = new SqlConnection(GetConnectionString()))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        // 1. Get current owed (make sure we actually get the amount owed, not the ID)
                        decimal currentOwe;
                        using (var cmd = new SqlCommand(
                            "SELECT customerOwe FROM tblCustomer WITH (UPDLOCK, ROWLOCK) WHERE customerID = @custId AND customerOwe > 0", conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@custId", customerID);
                            object scalar = cmd.ExecuteScalar();
                            if (scalar == null || scalar == DBNull.Value)
                                throw new InvalidOperationException("⚠ Customer not found or does not owe any money.");
                            currentOwe = Convert.ToDecimal(scalar);
                        }

                        if (amount > currentOwe)
                            throw new InvalidOperationException($"⚠ Amount exceeds the owed amount of: R{currentOwe}");

                        // calculate remaining owe AFTER the payment
                        decimal remaining = currentOwe - amount;

                        // 2. Insert payment: store paidAmount and the amount remaining (not the pre-payment owe)
                        using (var cmd = new SqlCommand(
                            "INSERT INTO tblPayment (paidAmount, dateOfPayment, customerID, amountDue) " +
                            "VALUES (@amount, @date, @custId, @remaining)", conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@amount", amount);
                            cmd.Parameters.AddWithValue("@date", paymentDate);
                            cmd.Parameters.AddWithValue("@custId", customerID);
                            cmd.Parameters.AddWithValue("@remaining", remaining);   // <-- remaining after payment
                            cmd.ExecuteNonQuery();
                        }

                        // 3. Update owed: set to the remaining amount (safer than subtracting again)
                        using (var cmd = new SqlCommand(
                            "UPDATE tblCustomer " +
                            "SET customerOwe = @remaining " +
                            "WHERE customerID = @custId", conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@remaining", remaining);
                            cmd.Parameters.AddWithValue("@custId", customerID);
                            cmd.ExecuteNonQuery();
                        }

                        tran.Commit();
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}

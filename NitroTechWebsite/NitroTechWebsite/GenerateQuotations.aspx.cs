using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace NitroTechWebsite
{
    public partial class Quotations : Page
    {

        // Cache faults in ViewState during the session
        private DataTable FaultsTable
        {
            get
            {
                if (ViewState["Faults"] == null)
                {
                    var dt = new DataTable();
                    dt.Columns.Add("ProductID", typeof(int));
                    dt.Columns.Add("PartName", typeof(string));
                    dt.Columns.Add("FaultDescription", typeof(string));
                    dt.Columns.Add("Quantity", typeof(int));
                    ViewState["Faults"] = dt;
                }
                return (DataTable)ViewState["Faults"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtQuotationDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                LoadParts();
            }
        }

        private void LoadParts()
        {
            cmbPart.Items.Clear();
            cmbPart.Items.Add(new ListItem("-- Select Part --", ""));

            using (var conn = DatabaseHelper.OpenConnection())
            using (var cmd = new SqlCommand(
                "SELECT productID, productName FROM tblProduct WHERE productActiveStatus=1", conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    cmbPart.Items.Add(new ListItem(
                        reader["productName"].ToString(),
                        reader["productID"].ToString()
                    ));
                }
            }
        }

        protected void btnAddFault_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cmbPart.SelectedValue) ||
                string.IsNullOrWhiteSpace(txtFault.Text)) return;

            DataRow row = FaultsTable.NewRow();
            row["ProductID"] = int.Parse(cmbPart.SelectedValue);
            row["PartName"] = cmbPart.SelectedItem.Text;
            row["FaultDescription"] = txtFault.Text.Trim();
            row["Quantity"] = int.TryParse(nudQuantity.Text, out int qty) ? qty : 1;
            FaultsTable.Rows.Add(row);

            gvFaults.DataSource = FaultsTable;
            gvFaults.DataBind();

            txtFaultSummary.Text = string.Join(Environment.NewLine,
                FaultsTable.AsEnumerable()
                    .Select(r => $"{r["PartName"]} - {r["FaultDescription"]} (Qty: {r["Quantity"]})"));

            cmbPart.SelectedIndex = 0;
            txtFault.Text = "";
            nudQuantity.Text = "1";
        }

        protected void btnGenerateQuotation_Click(object sender, EventArgs e)
        {
            if (FaultsTable.Rows.Count == 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert",
                    "alert('Cannot generate a quotation with no faults.');", true);
                return;
            }

            int quotationId;
            using (var conn = DatabaseHelper.OpenConnection())
            using (var tran = conn.BeginTransaction())
            {
                try
                {
                    // 1️ Insert Customer if not exists
                    using (var checkCust = new SqlCommand(
                        "SELECT COUNT(*) FROM tblCustomer WHERE customerID=@CustID", conn, tran))
                    {
                        checkCust.Parameters.AddWithValue("@CustID", txtCustID.Text.Trim());
                        int custExists = (int)checkCust.ExecuteScalar();
                        if (custExists == 0)
                        {
                            using (var insertCust = new SqlCommand(@"
                        INSERT INTO tblCustomer (customerID, customerName)
                        VALUES (@CustID, @CustName)", conn, tran))
                            {
                                insertCust.Parameters.AddWithValue("@CustID", txtCustID.Text.Trim());
                                insertCust.Parameters.AddWithValue("@CustName", txtCustName.Text.Trim());
                                insertCust.ExecuteNonQuery();
                            }
                        }
                    }

                    // 2️ Insert Vehicle if not exists
                    using (var checkVehicle = new SqlCommand(
                        "SELECT COUNT(*) FROM tblCustomerVehicle WHERE VIN=@VIN", conn, tran))
                    {
                        checkVehicle.Parameters.AddWithValue("@VIN", txtVIN.Text.Trim());
                        int vehicleExists = (int)checkVehicle.ExecuteScalar();
                        if (vehicleExists == 0)
                        {
                            using (var insertVehicle = new SqlCommand(@"
                        INSERT INTO tblCustomerVehicle
                        (customerID, VIN, Make, Model, Year, Engine, Transmission, Drivetrain, FuelType)
                        VALUES (@CustID,@VIN,@Make,@Model,@Year,@Engine,@Trans,@Drive,@Fuel)", conn, tran))
                            {
                                insertVehicle.Parameters.AddWithValue("@CustID", txtCustID.Text.Trim());
                                insertVehicle.Parameters.AddWithValue("@VIN", txtVIN.Text.Trim());
                                insertVehicle.Parameters.AddWithValue("@Make", txtMake.Text.Trim());
                                insertVehicle.Parameters.AddWithValue("@Model", txtModel.Text.Trim());
                                insertVehicle.Parameters.AddWithValue("@Year", txtYear.Text.Trim());
                                insertVehicle.Parameters.AddWithValue("@Engine", txtEngine.Text.Trim());
                                insertVehicle.Parameters.AddWithValue("@Trans", txtTransmission.Text.Trim());
                                insertVehicle.Parameters.AddWithValue("@Drive", txtDrivetrain.Text.Trim());
                                insertVehicle.Parameters.AddWithValue("@Fuel", ddlFuel.SelectedValue);
                                insertVehicle.ExecuteNonQuery();
                            }
                        }
                    }

                    // 3️ Insert Quotation
                    string quotationNumber = GenerateQuotationNumber();

                    var insertQuotation = new SqlCommand(@"
        INSERT INTO tblQuotation
        (customerID, quotationDate, quotationStatus, quotationTotal, quotationNumber)
        OUTPUT INSERTED.quotationID
        VALUES (@CustID, @Date, @Status, 0, @QNum)", conn, tran);

                    insertQuotation.Parameters.AddWithValue("@CustID", txtCustID.Text.Trim());
                    insertQuotation.Parameters.AddWithValue("@Date", DateTime.Now);
                    insertQuotation.Parameters.AddWithValue("@Status", 0); // 0 = Created
                    insertQuotation.Parameters.AddWithValue("@QNum", quotationNumber);

                    quotationId = (int)insertQuotation.ExecuteScalar();

                    // 4️ Insert Faults and compute total
                    decimal totalAmount = 0;
                    foreach (DataRow r in FaultsTable.Rows)
                    {
                        using (var insertFault = new SqlCommand(@"
                    INSERT INTO tblFaults
                    (quotationID, productID, faultDescription, quantity)
                    VALUES (@QID,@Prod,@Desc,@Qty)", conn, tran))
                        {
                            insertFault.Parameters.AddWithValue("@QID", quotationId);
                            insertFault.Parameters.AddWithValue("@Prod", r["ProductID"]);
                            insertFault.Parameters.AddWithValue("@Desc", r["FaultDescription"]);
                            insertFault.Parameters.AddWithValue("@Qty", r["Quantity"]);
                            insertFault.ExecuteNonQuery();
                        }

                        // Fetch product price
                        using (var priceCmd = new SqlCommand(
                            "SELECT productPrice FROM tblProduct WHERE productID=@Prod", conn, tran))
                        {
                            priceCmd.Parameters.AddWithValue("@Prod", r["ProductID"]);
                            decimal price = (decimal)priceCmd.ExecuteScalar();
                            totalAmount += price * Convert.ToInt32(r["Quantity"]);
                        }
                    }

                    // 5️⃣ Update Quotation Total
                    using (var updateTotal = new SqlCommand(
                        "UPDATE tblQuotation SET quotationTotal=@Total WHERE quotationID=@QID", conn, tran))
                    {
                        updateTotal.Parameters.AddWithValue("@Total", totalAmount);
                        updateTotal.Parameters.AddWithValue("@QID", quotationId);
                        updateTotal.ExecuteNonQuery();
                    }

                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    ClientScript.RegisterStartupScript(this.GetType(), "alert",
                        "alert('Error generating quotation.');", true);
                    return;
                }
            }

            // Clear UI
            FaultsTable.Clear();
            gvFaults.DataSource = null;
            gvFaults.DataBind();
            txtFaultSummary.Text = "";

            ClientScript.RegisterStartupScript(this.GetType(), "alert",
                "alert('Quotation generated successfully!');", true);
        }

        public static bool IsValidVin(string vin)
        {
            if (string.IsNullOrWhiteSpace(vin))
                return false;

            vin = vin.ToUpperInvariant();

            // Must be exactly 17 characters
            if (vin.Length != 17)
                return false;

            // Must not contain I, O, Q
            if (vin.IndexOfAny(new char[] { 'I', 'O', 'Q' }) >= 0)
                return false;

            // Must only contain A-Z and 0-9
            if (!Regex.IsMatch(vin, "^[A-Z0-9]{17}$"))
                return false;

            return true;
        }

        private string GenerateQuotationNumber()
        {
            // Query to count all existing quotations
            string query = "SELECT COUNT(*) FROM tblQuotation";

            // Use DatabaseHelper to execute the scalar query
            object result = DatabaseHelper.ExecuteScalar(query);
            int count;

            if (result != null && int.TryParse(result.ToString(), out count))
            {
                // Increment count and append VIN
                return "Q" + (count + 1) + "-" + txtVIN.Text.Trim();
            }
            else
            {
                // If no records yet, start at Q1
                return "Q1-" + txtVIN.Text.Trim();
            }
        }
    }
}
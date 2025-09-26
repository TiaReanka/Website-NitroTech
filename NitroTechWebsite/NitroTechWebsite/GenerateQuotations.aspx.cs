using NitroTechWebsite.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
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

        private void LoadCustomerVehicles(string searchTerm = "")
        {
            using (var conn = DatabaseHelper.OpenConnection())
            using (var cmd = new SqlCommand(@"
        SELECT c.customerID, c.customerName, c.customerEmailAddress, c.customerContactNumber, c.customerAddress,
               v.VIN, v.vehicleMake, v.vehicleModel, v.vehicleYear, v.vehicleEngine, v.vehicleTransmission, v.vehicleDriveTrain, v.vehicleFuelType
        FROM tblCustomer c
        INNER JOIN tblVehicle v ON c.customerID = v.customerID
        WHERE (@Search = '' OR c.customerName LIKE '%' + @Search + '%')", conn))
            {
                cmd.Parameters.AddWithValue("@Search", searchTerm);
                using (var adapter = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    gvCustomerVehicles.DataSource = dt;
                    gvCustomerVehicles.DataBind();
                }
            }
        }

        protected void txtSearchCustomer_TextChanged(object sender, EventArgs e)
        {
            LoadCustomerVehicles(txtSearchCustomer.Text.Trim());
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtQuotationDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                LoadParts();
                LoadCustomerVehicles();
            }
        }

        protected void gvCustomerVehicles_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (gvCustomerVehicles.SelectedRow == null) return;

            var keys = gvCustomerVehicles.DataKeys[gvCustomerVehicles.SelectedIndex];

            txtCustID.Text = keys["customerID"].ToString();
            txtCustName.Text = keys["customerName"].ToString();
            txtCustEmail.Text = keys["customerEmailAddress"].ToString();
            txtCustPhone.Text = keys["customerContactNumber"].ToString();
            txtCustAddress.Text = keys["customerAddress"].ToString();

            txtVIN.Text = keys["VIN"].ToString();
            txtMake.Text = keys["vehicleMake"].ToString();
            txtModel.Text = keys["vehicleModel"].ToString();
            txtYear.Text = keys["vehicleYear"].ToString();
            txtEngine.Text = keys["vehicleEngine"].ToString();
            txtTransmission.Text = keys["vehicleTransmission"].ToString();
            txtDrivetrain.Text = keys["vehicleDriveTrain"].ToString();
            string fuel = keys["vehicleFuelType"]?.ToString().Trim();
            if (!string.IsNullOrEmpty(fuel) && ddlFuel.Items.FindByValue(fuel) != null)
            {
                ddlFuel.SelectedValue = fuel;
            }
            else
            {
                ddlFuel.SelectedIndex = 0; // default
            }

            ToggleInputs(false);

        }

        protected void btnResetAll_Click(object sender, EventArgs e)
        {
            // Clear customer
            txtCustID.Text = "";
            txtCustName.Text = "";
            txtCustEmail.Text = "";
            txtCustPhone.Text = "";
            txtCustAddress.Text = "";

            // Clear vehicle
            txtVIN.Text = "";
            txtMake.Text = "";
            txtModel.Text = "";
            txtYear.Text = "";
            txtEngine.Text = "";
            txtTransmission.Text = "";
            txtDrivetrain.Text = "";
            ddlFuel.SelectedIndex = 0;

            // Re-enable inputs
            ToggleInputs(true);
        }

        protected void btnResetVehicle_Click(object sender, EventArgs e)
        {
            txtVIN.Text = "";
            txtMake.Text = "";
            txtModel.Text = "";
            txtYear.Text = "";
            txtEngine.Text = "";
            txtTransmission.Text = "";
            txtDrivetrain.Text = "";
            ddlFuel.SelectedIndex = 0;

            ToggleVehicleInputs(true);

            pnlCustomer.Visible = false;
            pnlVehicle.Visible = true;
        }

        private void ToggleInputs(bool enabled)
        {
            // Customer
            txtCustID.Enabled = enabled;
            txtCustName.Enabled = enabled;
            txtCustEmail.Enabled = enabled;
            txtCustPhone.Enabled = enabled;
            txtCustAddress.Enabled = enabled;

            // Vehicle
            ToggleVehicleInputs(enabled);
        }

        private void ToggleVehicleInputs(bool enabled)
        {
            txtVIN.Enabled = enabled;
            txtMake.Enabled = enabled;
            txtModel.Enabled = enabled;
            txtYear.Enabled = enabled;
            txtEngine.Enabled = enabled;
            txtTransmission.Enabled = enabled;
            txtDrivetrain.Enabled = enabled;
            ddlFuel.Enabled = enabled;
        }

        private void LoadParts()
        {
            cmbPart.Items.Clear();
            cmbPart.Items.Add(new ListItem("-- Select Part --", ""));

            using (var conn = DatabaseHelper.OpenConnection())
            using (var cmd = new SqlCommand(
                "SELECT partID, partName FROM tblParts", conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    cmbPart.Items.Add(new ListItem(
                        reader["partName"].ToString()
                    ));
                }
            }
        }

        protected void cmbPart_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedPart = cmbPart.Text;

            if (selectedPart == "Strip and Clean" ||
                selectedPart == "Booster" ||
                selectedPart == "Tighten" ||
                selectedPart == "Clutch" ||
                selectedPart == "Wiper Fluid" ||
                selectedPart == "Re-Alignment")
            {
                nudQuantity.Text = "1";
                nudQuantity.Enabled = false;
            }
            else
            {
                nudQuantity.Enabled = true;
                nudQuantity.Text = "0"; // reset quantity
            }
        }

        protected void btnAddFault_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cmbPart.SelectedValue) ||
                string.IsNullOrWhiteSpace(txtFault.Text)) return;

            if (cmbPart.Text == "Clutch" && txtTransmission.Text == "Automatic")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert",
                    "alert('Clutches are not used in automatic cars!');", true);
                return;
            }

            if (cmbPart.SelectedIndex <= 0 || string.IsNullOrEmpty(txtFault.Text) || !int.TryParse(nudQuantity.Text, out int qty) || qty <= 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert",
                    "alert('Please select a part, enter a fault description, and specify a valid quantity.');", true);
                return;
            }

            DataRow row = FaultsTable.NewRow();
            row["PartName"] = cmbPart.SelectedItem.Text;
            row["FaultDescription"] = txtFault.Text.Trim();
            row["Quantity"] = int.TryParse(nudQuantity.Text, out int qty1) ? qty1 : 1;
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
                    using (var insertVehicle = new SqlCommand(@"
    INSERT INTO tblVehicle
    (customerID, VIN, vehicleMake, vehicleModel, vehicleYear, vehicleEngine, vehicleTransmission, vehicleDriveTrain, vehicleFuelType)
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

        public static bool CheckValidID(string idNumber)
        {
            // Ensure input is not null and has at least 6 characters
            if (string.IsNullOrWhiteSpace(idNumber) || idNumber.Length < 6)
                return false;

            string yy = idNumber.Substring(0, 2);
            string mm = idNumber.Substring(2, 2);
            string dd = idNumber.Substring(4, 2);

            int year, month, day;

            // Try parsing parts to integers
            if (!int.TryParse(yy, out year) || !int.TryParse(mm, out month) || !int.TryParse(dd, out day))
                return false;

            // Adjust year to full year (2000s or 1900s)
            int currentYear = DateTime.Now.Year % 100;
            int fullYear = (year <= currentYear) ? (2000 + year) : (1900 + year);

            // Try creating a valid date
            try
            {
                DateTime date = new DateTime(fullYear, month, day);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void makeQuotation(string[,] products, string clientName, string clientAddress, string clientEmail, string clientPhone, string vehicleVIN, string vehicleName, string quotationNumber, string labourFee, string total)
        {
            var quotationService = new QuotationService();
            var document = quotationService.GetQuotation(products, clientName, clientAddress, clientEmail, clientPhone, vehicleVIN, vehicleName, quotationNumber, labourFee, total);

            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition",
                "attachment;filename=" + quotationNumber + ".pdf");

            using (var ms = new MemoryStream())
            {
                document.Save(ms);
                ms.WriteTo(Response.OutputStream);
            }

            Response.End();
        }
    }
}
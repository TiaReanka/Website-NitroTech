using NitroTechWebsite.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NitroTechWebsite
{
    public partial class Quotations : Page
    {
        
        private List<Fault> Faults
        {
            get
            {
                if (ViewState["Faults"] == null)
                    ViewState["Faults"] = new List<Fault>();
                return (List<Fault>)ViewState["Faults"];
            }
            set => ViewState["Faults"] = value;
        }
        // Cache faults in ViewState during the session



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
            cmbPart.Items.Add(new ListItem("-- Select Part --", "")); // placeholder

            using (var conn = DatabaseHelper.OpenConnection())
            using (var cmd = new SqlCommand("SELECT partID, partName FROM tblParts", conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    ListItem item = new ListItem(reader["partName"].ToString(), reader["partID"].ToString());
                    cmbPart.Items.Add(item);
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
                nudQuantity.Text = "1"; // reset quantity
            }
        }

        protected void btnAddFault_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFault.Text))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('⚠ Please enter a fault description.');", true);
                return;
            }

            if (cmbPart.SelectedIndex == 0 || string.IsNullOrEmpty(cmbPart.SelectedValue))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('⚠ Please select a valid part.');", true);
                return;
            }

            int productId = int.Parse(cmbPart.SelectedValue);  // <- use SelectedValue
            int qty = int.TryParse(nudQuantity.Text, out int parsedQty) ? parsedQty : 1;
            if (qty < 1) qty = 1;

            decimal unitPrice = 0;
            using (var conn = DatabaseHelper.OpenConnection())
            using (var cmd = new SqlCommand("SELECT partUnitPrice FROM tblParts WHERE partID=@PartID", conn))
            {
                cmd.Parameters.AddWithValue("@PartID", productId);
                object result = cmd.ExecuteScalar();
                if (result != null)
                    unitPrice = Convert.ToDecimal(result);
            }

            var fault = new Fault
            {
                FaultDescription = txtFault.Text.Trim(),
                ProductID = productId,
                AmountUsed = qty,
                TotalAmount = unitPrice * qty
            };

            Faults.Add(fault);

            UpdateFaultSummary();

            txtFault.Text = "";
            cmbPart.SelectedIndex = 0;
            nudQuantity.Text = "1";
            updParts.Update();
        }





        private void UpdateFaultSummary()
        {
            var sb = new StringBuilder();
            int count = 1;

            foreach (var fault in Faults)
            {
                // Map ProductID to the part name
                string partName = cmbPart.Items.Cast<ListItem>()
                                     .FirstOrDefault(i => i.Value == fault.ProductID.ToString())?.Text ?? "Unknown";

                sb.AppendLine($"{count}. Fault: {fault.FaultDescription}");
                sb.AppendLine($"   Part: {partName}, Quantity: {fault.AmountUsed}");
                sb.AppendLine();

                count++;
            }

            // Set the textbox value (this replaces old content with all faults)
            txtFaultSummary.Text = sb.ToString();

            // Force UpdatePanel to refresh
            updParts.Update();
        }



        protected void btnGenerateQuotation_Click(object sender, EventArgs e)
        {
            if (Faults.Count == 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert",
                    "alert('⚠ Cannot generate a quotation with no faults.');", true);
                return;
            }

            string quotationNumber = GenerateQuotationNumber();

            using (var conn = DatabaseHelper.OpenConnection())
            using (var tran = conn.BeginTransaction())
            {
                try
                {
                    // --- Insert Customer only if txtCustID is empty or enabled ---
                    if (txtCustID.Enabled)
                    {
                        var customerCmd = new SqlCommand(@"
                    INSERT INTO tblCustomer (customerID, customerName, customerAddress, customerContactNumber, customerEmailAddress, customerOwe)
                    VALUES (@CustID, @Name, @Address, @Phone, @Email, 0)", conn, tran);

                        customerCmd.Parameters.AddWithValue("@CustID", txtCustID.Text);
                        customerCmd.Parameters.AddWithValue("@Name", txtCustName.Text);
                        customerCmd.Parameters.AddWithValue("@Address", txtCustAddress.Text);
                        customerCmd.Parameters.AddWithValue("@Phone", txtCustPhone.Text);
                        customerCmd.Parameters.AddWithValue("@Email", txtCustEmail.Text);
                        customerCmd.ExecuteNonQuery();
                    }

                    // --- Insert Vehicle only if txtVIN is empty or enabled ---
                    if (txtVIN.Enabled)
                    {
                        var vehicleCmd = new SqlCommand(@"
                    INSERT INTO tblVehicle (VIN, vehicleMake, vehicleModel, vehicleTrim, vehicleYear, vehicleEngine, vehicleTransmission, vehicleDriveTrain, vehicleFuelType, customerID)
                    VALUES (@VIN, @Make, @Model, @Trim, @Year, @Engine, @Transmission, @DriveTrain, @Fuel, @CustID)", conn, tran);

                        vehicleCmd.Parameters.AddWithValue("@VIN", txtVIN.Text);
                        vehicleCmd.Parameters.AddWithValue("@Make", txtMake.Text);
                        vehicleCmd.Parameters.AddWithValue("@Model", txtModel.Text);
                        vehicleCmd.Parameters.AddWithValue("@Trim", "Premium");
                        vehicleCmd.Parameters.AddWithValue("@Year", txtYear.Text);
                        vehicleCmd.Parameters.AddWithValue("@Engine", txtEngine.Text);
                        vehicleCmd.Parameters.AddWithValue("@Transmission", txtTransmission.Text);
                        vehicleCmd.Parameters.AddWithValue("@DriveTrain", txtDrivetrain.Text);
                        vehicleCmd.Parameters.AddWithValue("@Fuel", ddlFuel.SelectedValue);
                        vehicleCmd.Parameters.AddWithValue("@CustID", txtCustID.Text);
                        vehicleCmd.ExecuteNonQuery();
                    }
                    decimal totalFaults = 0;
                    foreach (var f in Faults)
                    {
                        // Suppose f.TotalAmount is already set to product price * quantity
                        totalFaults += f.TotalAmount;
                    }

                    // Add 20% labour fee
                    decimal totalWithLabour = totalFaults * 1.2m;

                    // Add 15% tax
                    decimal finalTotal = totalWithLabour * 1.15m;

                    // --- Insert quotation ---
                    var qCmd = new SqlCommand(@"
                INSERT INTO tblQuotation (quotationNumber, quotationStatus, customerID,  quotationTotal, quotationDate, vehicleVIN)
                VALUES (@QNum, 0, @CustID, @FT, @Date, @VIN)", conn, tran);

                    qCmd.Parameters.AddWithValue("@QNum", quotationNumber);
                    qCmd.Parameters.AddWithValue("@CustID", txtCustID.Text);
                    qCmd.Parameters.AddWithValue("@FT", finalTotal);
                    qCmd.Parameters.AddWithValue("@Date", DateTime.Now);
                    qCmd.Parameters.AddWithValue("@VIN", txtVIN.Text);
                    qCmd.ExecuteNonQuery();

                    // --- Insert faults ---
                    foreach (var f in Faults)
                    {
                        var fCmd = new SqlCommand(@"
                    INSERT INTO tblFaults (fault, productID, amountProductUsed, quotationNumber, faultTotal)
                    VALUES (@Fault, @Prod, @Qty, @QNum, @Total)", conn, tran);//perhaps

                        fCmd.Parameters.AddWithValue("@Fault", f.FaultDescription);
                        fCmd.Parameters.AddWithValue("@Prod", f.ProductID);
                        fCmd.Parameters.AddWithValue("@Qty", f.AmountUsed);
                        fCmd.Parameters.AddWithValue("@QNum", quotationNumber);
                        fCmd.Parameters.AddWithValue("@Total", f.TotalAmount);
                        fCmd.ExecuteNonQuery();
                    }

                    tran.Commit();

                    // Open PDF generator in new tab
                    string script = $"window.open('DownloadQuotation.aspx?qnum={quotationNumber}', '_blank');";
                    ClientScript.RegisterStartupScript(this.GetType(), "openPDF", script, true);

                    // Reset the form immediately
                    ResetForm();

                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    ClientScript.RegisterStartupScript(this.GetType(), "alert",
                        $"alert('❌ Error generating quotation: {ex.Message}');", true);
                }
            }
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

        private void ResetForm()
        {
            // Clear faults
            Faults.Clear();
            txtFaultSummary.Text = "";

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

            // Reset inputs to editable
            ToggleInputs(true);

            // Clear parts selection
            cmbPart.SelectedIndex = 0;
            nudQuantity.Text = "1";

            // Force update panel refresh
            updParts.Update();
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

            Response.Flush();                    
            Response.SuppressContent = true;        
            HttpContext.Current.ApplicationInstance.CompleteRequest(); 


        }

        private void GenerateQuotation(string quotationNumber)
        {
            try
            {
                // Fetch faults for the quotation
                var faults = new DataTable();
                using (var conn = DatabaseHelper.OpenConnection())
                using (var cmd = new SqlCommand(
                    "SELECT f.fault, f.amountProductUsed, p.partID, p.partName, p.partUnitPrice, f.faultTotal " +
                    "FROM tblFaults f " +
                    "INNER JOIN tblParts p ON f.productID = p.partID " +
                    "INNER JOIN tblQuotation q ON f.quotationNumber = q.quotationNumber " +
                    "WHERE q.quotationNumber = @QNum", conn))
                {
                    cmd.Parameters.AddWithValue("@QNum", quotationNumber);
                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(faults);
                    }
                }

                int rowCount = faults.Rows.Count;
                string[,] products = new string[rowCount, 6];
                decimal totalProductCost = 0;

                for (int i = 0; i < rowCount; i++)
                {
                    var row = faults.Rows[i];
                    int itemNo = i + 1;
                    string faultDesc = row["fault"].ToString();
                    string partName = row["partName"].ToString();
                    string amountUsed = row["amountProductUsed"].ToString();
                    string productBaseCost = "R" + Convert.ToDecimal(row["partUnitPrice"]).ToString("0.00");
                    string totalAmount = "R" + Convert.ToDecimal(row["faultTotal"]).ToString("0.00");

                    totalProductCost += Convert.ToDecimal(row["faultTotal"]);

                    products[i, 0] = itemNo.ToString();
                    products[i, 1] = faultDesc;
                    products[i, 2] = partName;
                    products[i, 3] = amountUsed;
                    products[i, 4] = productBaseCost;
                    products[i, 5] = totalAmount;
                }

                decimal labourFee = totalProductCost * 0.2m; // 20% labour

                string clientName = "";
                string clientAddress = "";
                string clientEmail = "";
                string clientPhone = "";
                string vehicleVIN = "";
                string vehicleName = "";

                using (var conn = DatabaseHelper.OpenConnection())
                using (var cmd = new SqlCommand(@"
    SELECT 
        c.customerName,
        c.customerAddress,
        c.customerEmailAddress,
        c.customerContactNumber,
        v.VIN,
        v.vehicleMake,
        v.vehicleModel,
        v.vehicleYear
    FROM tblQuotation q
    INNER JOIN tblCustomer c ON q.customerID = c.customerID
    INNER JOIN tblVehicle v ON q.vehicleVIN = v.VIN
    WHERE q.quotationNumber = @QNum", conn))
                {
                    cmd.Parameters.AddWithValue("@QNum", quotationNumber);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            clientName = reader["customerName"].ToString();
                            clientAddress = reader["customerAddress"].ToString();
                            clientEmail = reader["customerEmailAddress"].ToString();
                            clientPhone = reader["customerContactNumber"].ToString();

                            vehicleVIN = reader["VIN"].ToString();
                            vehicleName = $"{reader["vehicleMake"]} {reader["vehicleModel"]} ({reader["vehicleYear"]})";
                        }
                    }
                }


                string labourFeeStr = labourFee.ToString("0.00");
                string totalStr = totalProductCost.ToString("0.00");

                // Generate PDF and send to browser
                makeQuotation(products, clientName, clientAddress, clientEmail, clientPhone, vehicleVIN, vehicleName, quotationNumber, labourFeeStr, totalStr);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert",
                    $"alert('❌ Error generating quotation PDF: {ex.Message}');", true);
            }
        }

    }
}
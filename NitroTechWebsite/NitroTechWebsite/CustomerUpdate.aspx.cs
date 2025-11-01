using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace NitroTechWebsite
{
    public partial class Customers : Page
    {
        // Use the correct connection string name from your Web.config
        private readonly string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["WstGrp4"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Initial setup if needed
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string searchCustomerID = txtSearchCustomer.Text.Trim();
            string searchVIN = txtSearchVIN.Text.Trim();

            if (string.IsNullOrEmpty(searchCustomerID) && string.IsNullOrEmpty(searchVIN))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('⚠ Please enter Customer ID or VIN to search.');", true);

                // Clear all textboxes and reset dropdown
                custID.Text = custName.Text = custPhone.Text = custEmail.Text = custAddress.Text = "";
                vin.Text = make.Text = model.Text = year.Text = trim.Text = engine.Text = transmission.Text = drivetrain.Text = "";
                fuelType.SelectedIndex = 0;

                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = @"
            SELECT c.customerID, c.customerName, c.customerAddress, c.customerContactNumber, c.customerEmailAddress,
                   v.VIN, v.vehicleMake, v.vehicleModel, v.vehicleTrim, v.vehicleYear, v.vehicleEngine,
                   v.vehicleTransmission, v.vehicleDriveTrain, v.vehicleFuelType
            FROM tblCustomer c
            INNER JOIN tblVehicle v ON c.customerID = v.customerID
            WHERE (@customerID = '' OR c.customerID = @customerID)
              AND (@vin = '' OR v.VIN = @vin);";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@customerID", searchCustomerID);
                    cmd.Parameters.AddWithValue("@vin", searchVIN);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            custID.Text = reader["customerID"].ToString();
                            custName.Text = reader["customerName"].ToString();
                            custPhone.Text = reader["customerContactNumber"].ToString();
                            custEmail.Text = reader["customerEmailAddress"].ToString();
                            custAddress.Text = reader["customerAddress"].ToString();

                            vin.Text = reader["VIN"].ToString();
                            make.Text = reader["vehicleMake"].ToString();
                            model.Text = reader["vehicleModel"].ToString();
                            year.Text = reader["vehicleYear"].ToString();
                            trim.Text = reader["vehicleTrim"].ToString();
                            engine.Text = reader["vehicleEngine"].ToString();
                            transmission.Text = reader["vehicleTransmission"].ToString();
                            drivetrain.Text = reader["vehicleDriveTrain"].ToString();

                            string fuel = reader["vehicleFuelType"].ToString();
                            if (fuelType.Items.FindByValue(fuel) != null)
                                fuelType.SelectedValue = fuel;
                            else
                                fuelType.SelectedIndex = 0;
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('⚠ Customer not found.');", true);

                            // Clear fields
                            custID.Text = custName.Text = custPhone.Text = custEmail.Text = custAddress.Text = "";
                            vin.Text = make.Text = model.Text = year.Text = trim.Text = engine.Text = transmission.Text = drivetrain.Text = "";
                            fuelType.SelectedIndex = 0;
                        }
                    }
                }
            }
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(custID.Text) || string.IsNullOrEmpty(vin.Text))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('⚠ Please search and select a customer before saving.');", true);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // Update Customer
                    string updateCustomerQuery = @"
                UPDATE tblCustomer
                SET customerName = @customerName,
                    customerContactNumber = @customerContact,
                    customerEmailAddress = @customerEmail,
                    customerAddress = @customerAddress
                WHERE customerID = @customerID";

                    using (SqlCommand cmd = new SqlCommand(updateCustomerQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@customerID", custID.Text);
                        cmd.Parameters.AddWithValue("@customerName", custName.Text);
                        cmd.Parameters.AddWithValue("@customerContact", custPhone.Text);
                        cmd.Parameters.AddWithValue("@customerEmail", custEmail.Text);
                        cmd.Parameters.AddWithValue("@customerAddress", custAddress.Text);
                        cmd.ExecuteNonQuery();
                    }

                    // Update Vehicle
                    string updateVehicleQuery = @"
                UPDATE tblVehicle
                SET vehicleMake = @make,
                    vehicleModel = @model,
                    vehicleTrim = @trim,
                    vehicleYear = @year,
                    vehicleEngine = @engine,
                    vehicleTransmission = @transmission,
                    vehicleDriveTrain = @drivetrain,
                    vehicleFuelType = @fuelType
                WHERE VIN = @vin";

                    using (SqlCommand cmd = new SqlCommand(updateVehicleQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@vin", vin.Text);
                        cmd.Parameters.AddWithValue("@make", make.Text);
                        cmd.Parameters.AddWithValue("@model", model.Text);
                        cmd.Parameters.AddWithValue("@trim", trim.Text);
                        cmd.Parameters.AddWithValue("@year", year.Text);
                        cmd.Parameters.AddWithValue("@engine", engine.Text);
                        cmd.Parameters.AddWithValue("@transmission", transmission.Text);
                        cmd.Parameters.AddWithValue("@drivetrain", drivetrain.Text);
                        cmd.Parameters.AddWithValue("@fuelType", fuelType.SelectedValue);
                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();

                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('⚠ Customer and vehicle updated successfully.');", true);

                    // Clear all textboxes and reset dropdown
                    ClearFields();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"alert('❌ Error: {ex.Message}');", true);
                }
            }
        }

        // Utility function to clear all textboxes and reset dropdown
        private void ClearFields()
        {
            custID.Text = custName.Text = custPhone.Text = custEmail.Text = custAddress.Text = "";
            vin.Text = make.Text = model.Text = year.Text = trim.Text = engine.Text = transmission.Text = drivetrain.Text = "";
            fuelType.SelectedIndex = 0;
            txtSearchCustomer.Text = txtSearchVIN.Text = "";
        }

    }
}

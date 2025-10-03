using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NitroTechWebsite
{
    public partial class Customers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCustomerData();
            }
        }

        private void BindCustomerData()
        {
            using (var conn = DatabaseHelper.OpenConnection())
            using (var cmd = new SqlCommand(@"SELECT c.customerID, c.customerName, c.customerAddress, 
                                                     c.customerContactNumber, c.customerEmailAddress,
                                                     v.VIN, v.vehicleMake, v.vehicleModel, v.vehicleTrim, v.vehicleYear
                                              FROM tblCustomer c
                                              LEFT JOIN tblVehicle v ON c.customerID = v.customerID", conn))
            {
                var dt = new DataTable();
                using (var da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                    gvResults.DataSource = dt;
                    gvResults.DataBind();
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string customerId = txtSearchCustomer.Text.Trim();
            string vin = txtSearchVIN.Text.Trim();

            using (var conn = DatabaseHelper.OpenConnection())
            using (var cmd = new SqlCommand(@"SELECT c.customerID, c.customerName, c.customerAddress, 
                                                     c.customerContactNumber, c.customerEmailAddress,
                                                     v.VIN, v.vehicleMake, v.vehicleModel, v.vehicleTrim, v.vehicleYear
                                              FROM tblCustomer c
                                              LEFT JOIN tblVehicle v ON c.customerID = v.customerID
                                              WHERE (@customerID = '' OR c.customerID = @customerID)
                                                AND (@VIN = '' OR v.VIN = @VIN)", conn))
            {
                cmd.Parameters.AddWithValue("@customerID", string.IsNullOrEmpty(customerId) ? "" : customerId);
                cmd.Parameters.AddWithValue("@VIN", string.IsNullOrEmpty(vin) ? "" : vin);

                var dt = new DataTable();
                using (var da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                    gvResults.DataSource = dt;
                    gvResults.DataBind();
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string custId = custID.Text.Trim();
            string newName = custName.Text.Trim();
            string newPhone = custPhone.Text.Trim();
            string newEmail = custEmail.Text.Trim();
            string newAddress = custAddress.Text.Trim();

            string vinVal = vin.Text.Trim();
            string makeVal = make.Text.Trim();
            string modelVal = model.Text.Trim();
            string yearVal = year.Text.Trim();
            string trimVal = trim.Text.Trim();
            string engineVal = engine.Text.Trim();
            string transmissionVal = transmission.Text.Trim();
            string drivetrainVal = drivetrain.Text.Trim();
            string fuelVal = fuelType.SelectedValue;

            if (string.IsNullOrWhiteSpace(custId))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Please enter a valid Customer ID.');", true);
                return;
            }

            if (!string.IsNullOrWhiteSpace(newPhone) && newPhone.Length != 10)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Please enter a valid 10-digit phone number.');", true);
                return;
            }

            using (var conn = DatabaseHelper.OpenConnection())
            {
                // First check if customer exists
                bool customerExists;
                using (var cmd = new SqlCommand("SELECT COUNT(1) FROM tblCustomer WHERE customerID=@customerID", conn))
                {
                    cmd.Parameters.AddWithValue("@customerID", custId);
                    customerExists = (int)cmd.ExecuteScalar() > 0;
                }

                if (customerExists)
                {
                    // Update existing customer
                    using (var cmd = new SqlCommand(@"UPDATE tblCustomer 
                                              SET customerName=@customerName, 
                                                  customerContactNumber=@customerContactNumber,
                                                  customerEmailAddress=@customerEmailAddress,
                                                  customerAddress=@customerAddress
                                              WHERE customerID=@customerID", conn))
                    {
                        cmd.Parameters.AddWithValue("@customerName", string.IsNullOrEmpty(newName) ? (object)DBNull.Value : newName);
                        cmd.Parameters.AddWithValue("@customerContactNumber", string.IsNullOrEmpty(newPhone) ? (object)DBNull.Value : newPhone);
                        cmd.Parameters.AddWithValue("@customerEmailAddress", string.IsNullOrEmpty(newEmail) ? (object)DBNull.Value : newEmail);
                        cmd.Parameters.AddWithValue("@customerAddress", string.IsNullOrEmpty(newAddress) ? (object)DBNull.Value : newAddress);
                        cmd.Parameters.AddWithValue("@customerID", custId);
                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    // Insert new customer
                    using (var cmd = new SqlCommand(@"INSERT INTO tblCustomer 
                              (customerID, customerName, customerContactNumber, customerEmailAddress, customerAddress, customerOwe) 
                              VALUES (@customerID, @customerName, @customerContactNumber, @customerEmailAddress, @customerAddress, @customerOwe)", conn))
                    {
                        cmd.Parameters.AddWithValue("@customerID", custId);
                        cmd.Parameters.AddWithValue("@customerName", string.IsNullOrEmpty(newName) ? (object)DBNull.Value : newName);
                        cmd.Parameters.AddWithValue("@customerContactNumber", string.IsNullOrEmpty(newPhone) ? (object)DBNull.Value : newPhone);
                        cmd.Parameters.AddWithValue("@customerEmailAddress", string.IsNullOrEmpty(newEmail) ? (object)DBNull.Value : newEmail);
                        cmd.Parameters.AddWithValue("@customerAddress", string.IsNullOrEmpty(newAddress) ? (object)DBNull.Value : newAddress);
                        cmd.Parameters.AddWithValue("@customerOwe", 0);
                        cmd.ExecuteNonQuery();
                    }
                }

                // Insert or Update Vehicle if VIN is entered
                if (!string.IsNullOrEmpty(vinVal))
                {
                    using (var cmd = new SqlCommand(@"
                IF EXISTS (SELECT 1 FROM tblVehicle WHERE VIN=@VIN)
                    UPDATE tblVehicle
                    SET vehicleMake=@vehicleMake, vehicleModel=@vehicleModel, vehicleYear=@vehicleYear,
                        vehicleTrim=@vehicleTrim, vehicleEngine=@vehicleEngine, vehicleTransmission=@vehicleTransmission,
                        vehicleDriveTrain=@vehicleDriveTrain, vehicleFuelType=@vehicleFuelType
                    WHERE VIN=@VIN
                ELSE
                    INSERT INTO tblVehicle (VIN, vehicleMake, vehicleModel, vehicleYear, vehicleTrim, vehicleEngine, vehicleTransmission, vehicleDriveTrain, vehicleFuelType, customerID)
                    VALUES (@VIN, @vehicleMake, @vehicleModel, @vehicleYear, @vehicleTrim, @vehicleEngine, @vehicleTransmission, @vehicleDriveTrain, @vehicleFuelType, @customerID)", conn))
                    {
                        cmd.Parameters.AddWithValue("@VIN", vinVal);
                        cmd.Parameters.AddWithValue("@vehicleMake", string.IsNullOrEmpty(makeVal) ? (object)DBNull.Value : makeVal);
                        cmd.Parameters.AddWithValue("@vehicleModel", string.IsNullOrEmpty(modelVal) ? (object)DBNull.Value : modelVal);
                        cmd.Parameters.AddWithValue("@vehicleYear", string.IsNullOrEmpty(yearVal) ? (object)DBNull.Value : yearVal);
                        cmd.Parameters.AddWithValue("@vehicleTrim", string.IsNullOrEmpty(trimVal) ? (object)DBNull.Value : trimVal);
                        cmd.Parameters.AddWithValue("@vehicleEngine", string.IsNullOrEmpty(engineVal) ? (object)DBNull.Value : engineVal);
                        cmd.Parameters.AddWithValue("@vehicleTransmission", string.IsNullOrEmpty(transmissionVal) ? (object)DBNull.Value : transmissionVal);
                        cmd.Parameters.AddWithValue("@vehicleDriveTrain", string.IsNullOrEmpty(drivetrainVal) ? (object)DBNull.Value : drivetrainVal);
                        cmd.Parameters.AddWithValue("@vehicleFuelType", string.IsNullOrEmpty(fuelVal) ? (object)DBNull.Value : fuelVal);
                        cmd.Parameters.AddWithValue("@CustomerID", custId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Customer and Vehicle details saved successfully.');", true);
            BindCustomerData();
        }
    }
}


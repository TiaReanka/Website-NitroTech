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
                BindVehicleData();
            }
        }

        private void BindCustomerData()
        {
            using (var conn = DatabaseHelper.OpenConnection())
            using (var cmd = new SqlCommand("SELECT customerID, customerName, customerAddress, customerContactNumber, customerEmailAddress FROM tblCustomer", conn))
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

        private void BindVehicleData()
        {
            using (var conn = DatabaseHelper.OpenConnection())
            using (var cmd = new SqlCommand(@"SELECT v.VIN, v.vehicleMake, v.vehicleModel, v.vehicleTrim, v.vehicleYear, 
                                                     c.customerID, c.customerName, c.customerAddress, c.customerContactNumber, c.customerEmailAddress
                                              FROM tblVehicle v
                                              INNER JOIN tblCustomer c ON v.customerID = c.customerID", conn))
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
            string customerId = Request.Form["txtSearchCustomer"];
            string vin = Request.Form["txtSearchVIN"];

            using (var conn = DatabaseHelper.OpenConnection())
            using (var cmd = new SqlCommand(@"SELECT v.VIN, v.vehicleMake, v.vehicleModel, v.vehicleTrim, v.vehicleYear, 
                                                     c.customerID, c.customerName, c.customerAddress, c.customerContactNumber, c.customerEmailAddress
                                              FROM tblVehicle v
                                              INNER JOIN tblCustomer c ON v.customerID = c.customerID
                                              WHERE (@custID = '' OR c.customerID = @custID)
                                                AND (@vin = '' OR v.VIN = @vin)", conn))
            {
                cmd.Parameters.AddWithValue("@custID", customerId ?? "");
                cmd.Parameters.AddWithValue("@vin", vin ?? "");

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
            string custId = Request.Form["custID"];
            string newName = Request.Form["custName"];
            string newPhone = Request.Form["custPhone"];
            string newEmail = Request.Form["custEmail"];
            string newAddress = Request.Form["custAddress"];

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
            using (var cmd = new SqlCommand(@"UPDATE tblCustomer 
                                              SET customerName=@Name, 
                                                  customerContactNumber=@Phone,
                                                  customerEmailAddress=@Email,
                                                  customerAddress=@Address
                                              WHERE customerID=@ID", conn))
            {
                cmd.Parameters.AddWithValue("@Name", newName ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Phone", newPhone ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Email", newEmail ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Address", newAddress ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ID", custId);

                int rows = cmd.ExecuteNonQuery();

                if (rows > 0)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Customer details updated successfully.');", true);
                    BindCustomerData();
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Update failed. No customer affected.');", true);
                }
            }
        }
    }
}


using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NitroTechWebsite
{
    public partial class Customers : Page
    {
        private readonly string connectionString =
System.Configuration.ConfigurationManager.ConnectionStrings["WstGrp4"].ConnectionString; 
 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCustomers();

                ddlVIN.Items.Clear();
                ddlVIN.Items.Add(new ListItem("-- Select VIN --", ""));
                ddlVIN.Enabled = false; // VIN disabled initially  
            }
        }

        private void LoadCustomers()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT customerID, customerName FROM tblCustomer ORDER BY customerName"; 
                SqlCommand cmd = new SqlCommand(query, conn);
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());

                ddlCustomer.Items.Clear();
                ddlCustomer.Items.Add(new ListItem("-- Select Customer --", ""));

                foreach (DataRow row in dt.Rows)
                {
                    string text = $"{row["customerName"]} - {row["customerID"]}";
                    string value = row["customerID"].ToString();
                    ListItem item = new ListItem(text, value);
                    item.Attributes.CssStyle.Add("color", "black");
                    ddlCustomer.Items.Add(item);
                }
            }
        }

        protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlVIN.Items.Clear();
            ddlVIN.Items.Add(new ListItem("-- Select VIN --", ""));
            ClearFields();

            if (string.IsNullOrEmpty(ddlCustomer.SelectedValue))
            {
                ddlVIN.Enabled = false;
                return;
            }

            ddlVIN.Enabled = true;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT VIN FROM tblVehicle WHERE customerID=@cid";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@cid", ddlCustomer.SelectedValue);

                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());

                ddlVIN.DataSource = dt;
                ddlVIN.DataTextField = "VIN";
                ddlVIN.DataValueField = "VIN";
                ddlVIN.DataBind();

                foreach (ListItem item in ddlVIN.Items)
                    item.Attributes.CssStyle.Add("color", "black");

                ddlVIN.Items.Insert(0, new ListItem("-- Select VIN --", ""));
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string customerID = ddlCustomer.SelectedValue;
            string vinNumber = ddlVIN.SelectedValue;

            // Existing validation: Check if both are empty 
            if (string.IsNullOrEmpty(customerID) && string.IsNullOrEmpty(vinNumber))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('⚠ Please select Customer and / or VIN to search.');", true); 
                ClearFields();
                ddlCustomer.SelectedIndex = 0;
                ddlVIN.SelectedIndex = 0;
                return;
            }

            //     NEW VALIDATION: If a Customer is selected, a VIN must also be selected. 
            if (!string.IsNullOrEmpty(customerID) && string.IsNullOrEmpty(vinNumber))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('⚠ Please select a VIN number to search for the customer and vehicle information.');", true); 
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"  
                    SELECT c.customerID, c.customerName, c.customerAddress,  
customerContactNumber, c.customerEmailAddress,  
                        v.VIN, v.vehicleMake, v.vehicleModel, v.vehicleTrim, v.vehicleYear,  
v.vehicleEngine,  
                        v.vehicleTransmission, v.vehicleDriveTrain, v.vehicleFuelType  
                    FROM tblCustomer c  
                    INNER JOIN tblVehicle v ON c.customerID = v.customerID  
                    WHERE (@customerID = '' OR c.customerID = @customerID)  
                     AND (@vin = '' OR v.VIN = @vin);";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@customerID", customerID ?? "");
                cmd.Parameters.AddWithValue("@vin", vinNumber ?? "");

                SqlDataReader reader = cmd.ExecuteReader();
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
                    fuelType.SelectedValue = fuelType.Items.FindByValue(fuel) != null ? fuel : "";

                    ddlCustomer.SelectedValue = custID.Text;
                    ddlVIN.SelectedValue = vin.Text;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('⚠ Customer not found.');", true); 
                    ClearFields();
                    ddlCustomer.SelectedIndex = 0;
                    ddlVIN.SelectedIndex = 0;
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string customerID = ddlCustomer.SelectedValue;
            string vinNumber = ddlVIN.SelectedValue;

            if (string.IsNullOrEmpty(customerID) || string.IsNullOrEmpty(vinNumber))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('⚠ Please select a customer and a VIN before saving.');", true); 
                return;
            }

            //INSERT VALIDATIONS HERE// 
            if (custPhone.Text.Length != 10 || !custPhone.Text.All(char.IsDigit))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert",
                    "alert('⚠ Invalid Customer Phone Number format.');", true);
                return;
            }

            if (custEmail.Text.IndexOf('@') == -1 || custEmail.Text.IndexOf('.') == -1)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert",
                    "alert('⚠ Invalid Customer Email Address format.');", true);
                return;
            }

            //cpush test 


            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    string updateCustomer = @"  
                        UPDATE tblCustomer  
                        SET customerName=@name, customerContactNumber=@contact,  
customerEmailAddress=@email, customerAddress=@address  
                        WHERE customerID=@cid";
                    SqlCommand cmdCust = new SqlCommand(updateCustomer, conn,
transaction);
                    cmdCust.Parameters.AddWithValue("@cid", customerID);
                    cmdCust.Parameters.AddWithValue("@name", custName.Text);
                    cmdCust.Parameters.AddWithValue("@contact", custPhone.Text);
                    cmdCust.Parameters.AddWithValue("@email", custEmail.Text);
                    cmdCust.Parameters.AddWithValue("@address", custAddress.Text);
                    cmdCust.ExecuteNonQuery();

                    string updateVehicle = @"  
                        UPDATE tblVehicle  
                        SET vehicleMake=@make, vehicleModel=@model, vehicleTrim=@trim,  
vehicleYear=@year,  
                            vehicleEngine=@engine, vehicleTransmission=@trans,  
vehicleDriveTrain=@drive, vehicleFuelType=@fuel  
                        WHERE VIN=@vin";
                    SqlCommand cmdVeh = new SqlCommand(updateVehicle, conn,
transaction);
                    cmdVeh.Parameters.AddWithValue("@vin", vinNumber);
                    cmdVeh.Parameters.AddWithValue("@make", make.Text);
                    cmdVeh.Parameters.AddWithValue("@model", model.Text);
                    cmdVeh.Parameters.AddWithValue("@trim", trim.Text);
                    cmdVeh.Parameters.AddWithValue("@year", year.Text);
                    cmdVeh.Parameters.AddWithValue("@engine", engine.Text);
                    cmdVeh.Parameters.AddWithValue("@trans", transmission.Text);
                    cmdVeh.Parameters.AddWithValue("@drive", drivetrain.Text);
                    cmdVeh.Parameters.AddWithValue("@fuel", fuelType.SelectedValue);
                    cmdVeh.ExecuteNonQuery();

                    transaction.Commit();
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('✔ Customer and vehicle updated successfully.');", true); 



                    ClearFields();
                    ddlCustomer.SelectedIndex = 0;
                    ddlVIN.Items.Clear();
                    ddlVIN.Items.Add(new ListItem("-- Select VIN --", ""));
                    ddlVIN.Enabled = false;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"alert('  Error:{ ex.Message}');", true); 
                }
            }
        }

        private void ClearFields()
        {
            custID.Text = custName.Text = custPhone.Text = custEmail.Text = custAddress.Text= "";
            vin.Text = make.Text = model.Text = year.Text = trim.Text = engine.Text =transmission.Text = drivetrain.Text = "";
            fuelType.SelectedIndex = 0;
        }
    }
}
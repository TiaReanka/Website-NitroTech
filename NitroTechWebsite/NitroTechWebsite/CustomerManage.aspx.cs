using System;
using System.Data;
using System.Web.UI;
using NitroTechWebsite;

namespace NitroTechWebsite
{
    public partial class CustomerManage : System.Web.UI.Page
    {
        public VehicleTransfer vehicleService = new VehicleTransfer();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadVehicles(); // Load all vehicles into ddlVIN
                LoadNewCustomers(); // Load all customers into ddlCustomer
            }
        }

        private void LoadVehicles()
        {
            
            DataTable dtVehicles = vehicleService.GetDataTable("SELECT VIN, vehicleMake, vehicleModel, vehicleYear FROM tblVehicle");
            ddlVIN.DataSource = dtVehicles;
            ddlVIN.DataTextField = "VIN"; 
            ddlVIN.DataValueField = "VIN";
            ddlVIN.DataBind();
            ddlVIN.Items.Insert(0, "--Select Vehicle--");
        }

        private void LoadNewCustomers()
        {
            DataTable dtCustomers = vehicleService.GetAllCustomers();

            // Create a combined display column
            dtCustomers.Columns.Add("DisplayText", typeof(string));

            foreach (DataRow row in dtCustomers.Rows)
            {
                row["DisplayText"] = row["customerName"] + " (" + row["customerID"] + ")";
            }

            ddlCustomer.DataSource = dtCustomers;
            ddlCustomer.DataTextField = "DisplayText";
            ddlCustomer.DataValueField = "customerID";
            ddlCustomer.DataBind();

            ddlCustomer.Items.Insert(0, "--Select Customer--");
        }

        protected void ddlVIN_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlVIN.SelectedIndex > 0)
            {
                // Load old customer info for selected vehicle
                string vin = ddlVIN.SelectedValue;
                DataTable dt = vehicleService.GetDataTable(
                    "SELECT c.customerName, c.customerContactNumber, c.customerEmailAddress, c.customerAddress FROM tblVehicle v INNER JOIN tblCustomer c ON v.customerID = c.customerID WHERE v.VIN = @VIN",
                    new System.Data.SqlClient.SqlParameter("@VIN", vin)
                );

                if (dt.Rows.Count > 0)
                {
                    var row = dt.Rows[0];
                    txtOldName.Text = row["customerName"].ToString();
                    txtOldContact.Text = row["customerContactNumber"].ToString();
                    txtOldEmail.Text = row["customerEmailAddress"].ToString();
                    txtOldAddress.Text = row["customerAddress"].ToString();
                    btnTransfer.Enabled = true;
                }
            }
        }

        protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCustomer.SelectedIndex > 0)
            {
                string newCustomerId = ddlCustomer.SelectedValue;
                DataTable dt = vehicleService.GetDataTable(
                    "SELECT customerName, customerContactNumber, customerEmailAddress, customerAddress FROM tblCustomer WHERE customerID = @CustomerID",
                    new System.Data.SqlClient.SqlParameter("@CustomerID", newCustomerId)
                );

                if (dt.Rows.Count > 0)
                {
                    var row = dt.Rows[0];
                    txtNewName.Text = row["customerName"].ToString();
                    txtNewContact.Text = row["customerContactNumber"].ToString();
                    txtNewEmail.Text = row["customerEmailAddress"].ToString();
                    txtNewAddress.Text = row["customerAddress"].ToString();
                }
            }
        }

        protected void btnTransfer_Click(object sender, EventArgs e)
        {
            if (ddlVIN.SelectedIndex <= 0 || ddlCustomer.SelectedIndex <= 0)
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(),
                        "error",
                        $"alert('❌ Please select a vehicle and a new customer');",
                        true);
                return;
            }

            string vin = ddlVIN.SelectedValue;
            string oldCustomerId = ""; // Get from database
            DataTable dtOldCustomer = vehicleService.GetDataTable(
                "SELECT customerID FROM tblVehicle WHERE VIN = @VIN",
                new System.Data.SqlClient.SqlParameter("@VIN", vin)
            );

            if (dtOldCustomer.Rows.Count > 0)
                oldCustomerId = dtOldCustomer.Rows[0]["customerID"].ToString();

            string newCustomerId = ddlCustomer.SelectedValue;

            var result = vehicleService.TransferVehicle(vin, oldCustomerId, newCustomerId);

            ScriptManager.RegisterStartupScript(this, this.GetType(),
                        "error",
                        $"alert('{result.Message}');",
                        true);

            if (result.Success)
            {
                // Reset form
                ddlVIN.SelectedIndex = 0;
                ddlCustomer.SelectedIndex = 0;
                txtOldName.Text = txtOldContact.Text = txtOldEmail.Text = txtOldAddress.Text = "";
                txtNewName.Text = txtNewContact.Text = txtNewEmail.Text = txtNewAddress.Text = "";
                btnTransfer.Enabled = false;
            }
        }
    }
}
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
    public partial class CustomerManage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadVINs();
                LoadCustomers();
            }
        }

        private void LoadVINs()
        {
            using (var conn = DatabaseHelper.OpenConnection())
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT VIN FROM tblVehicle", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlVIN.DataSource = dt;
                ddlVIN.DataTextField = "VIN";
                ddlVIN.DataBind();
                ddlVIN.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select VIN--", ""));
            }
        }

        private void LoadCustomers()
        {
            using (var conn = DatabaseHelper.OpenConnection())
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT customerID, customerName FROM tblCustomer", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlCustomer.DataSource = dt;
                ddlCustomer.DataTextField = "customerName";
                ddlCustomer.DataValueField = "customerID";
                ddlCustomer.DataBind();
                ddlCustomer.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Customer--", ""));
            }
        }

        protected void ddlVIN_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlVIN.SelectedValue))
                return;

            using (var conn = DatabaseHelper.OpenConnection())
            {
                SqlCommand cmd = new SqlCommand(@"
                    SELECT c.customerName, c.customerContactNumber, c.customerEmailAddress, c.customerAddress
                    FROM tblVehicle v
                    INNER JOIN tblCustomer c ON v.customerID = c.customerID
                    WHERE v.VIN = @VIN", conn);

                cmd.Parameters.AddWithValue("@VIN", ddlVIN.SelectedValue);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        txtOldName.Text = reader["customerName"].ToString();
                        txtOldContact.Text = reader["customerContactNumber"].ToString();
                        txtOldEmail.Text = reader["customerEmailAddress"].ToString();
                        txtOldAddress.Text = reader["customerAddress"].ToString();

                        ddlCustomer.Enabled = true;
                    }
                }
            }
        }

        protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlCustomer.SelectedValue))
                return;

            using (var conn = DatabaseHelper.OpenConnection())
            {
                SqlCommand cmd = new SqlCommand(@"
                    SELECT customerName, customerContactNumber, customerEmailAddress, customerAddress 
                    FROM tblCustomer
                    WHERE customerID = @customerID", conn);

                cmd.Parameters.AddWithValue("@customerID", ddlCustomer.SelectedValue);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        txtNewName.Text = reader["customerName"].ToString();
                        txtNewContact.Text = reader["customerContactNumber"].ToString();
                        txtNewEmail.Text = reader["customerEmailAddress"].ToString();
                        txtNewAddress.Text = reader["customerAddress"].ToString();

                        btnTransfer.Enabled = true;
                    }
                }
            }
        }

        protected void btnTransfer_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlVIN.SelectedValue) || string.IsNullOrEmpty(ddlCustomer.SelectedValue))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Please select both VIN and Customer.');", true);
                return;
            }

            using (var conn = DatabaseHelper.OpenConnection())
            {
                SqlCommand cmd = new SqlCommand(@"
                    UPDATE tblVehicle 
                    SET customerID = @newCustomerID 
                    WHERE VIN = @VIN", conn);

                cmd.Parameters.AddWithValue("@newCustomerID", ddlCustomer.SelectedValue);
                cmd.Parameters.AddWithValue("@VIN", ddlVIN.SelectedValue);
                cmd.ExecuteNonQuery();
            }

            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Vehicle transferred successfully.');", true);

            // Reset form
            ddlVIN.SelectedIndex = 0;
            ddlCustomer.SelectedIndex = 0;
            ddlCustomer.Enabled = false;
            btnTransfer.Enabled = false;

            txtOldName.Text = txtOldContact.Text = txtOldEmail.Text = txtOldAddress.Text = "";
            txtNewName.Text = txtNewContact.Text = txtNewEmail.Text = txtNewAddress.Text = "";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace NitroTechWebsite
{
    public partial class ReviewStatement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCustomers(); // NEW
                gvStatements.DataSource = null;
                gvStatements.DataBind();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {

            BindGrid();
        }

        private void BindGrid()
        {
            if (string.IsNullOrEmpty(ddlCustomer.SelectedValue))
            {
                gvStatements.DataSource = null;
                gvStatements.DataBind();
                return;
            }

            // Extract ONLY the customer name from "Name - ID"
            string selectedText = ddlCustomer.SelectedItem.Text;
            string customerName = selectedText.Split('-')[0].Trim();

            using (SqlConnection sqlConnection = DatabaseHelper.OpenConnection())
            {
                string sql = @"
            SELECT s.statementNumber, c.customerName, s.statementDate, s.statementAmountDue
            FROM tblStatement s
            INNER JOIN tblCustomer c ON s.customerID = c.customerID
            WHERE c.customerName = @CustomerName
            ORDER BY s.statementDate DESC";

                using (SqlCommand cmd = new SqlCommand(sql, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@CustomerName", customerName);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);

                        // Convert statementDate to string to remove time
                        if (dt.Columns.Contains("statementDate"))
                        {
                            dt.Columns.Add("statementDateFormatted", typeof(string));

                            foreach (DataRow row in dt.Rows)
                            {
                                if (row["statementDate"] != DBNull.Value)
                                {
                                    DateTime dtValue = Convert.ToDateTime(row["statementDate"]);
                                    row["statementDateFormatted"] = dtValue.ToString("yyyy-MM-dd"); // or "dd/MM/yyyy"
                                }
                            }

                            // Remove the original column
                            dt.Columns.Remove("statementDate");

                            // Rename the formatted column to original name
                            dt.Columns["statementDateFormatted"].ColumnName = "statementDate";
                        }

                        if (dt.Rows.Count == 0)
                        {
                            // Show alert if no statements exist
                            ClientScript.RegisterStartupScript(this.GetType(), "noStatements",
                                "alert('ℹ This customer has no statements yet.');", true);

                            gvStatements.DataSource = null;
                            gvStatements.DataBind();
                        }
                        else
                        {
                            gvStatements.DataSource = dt;
                            gvStatements.DataBind();
                        }
                    }
                }
            }
        }

        private void LoadCustomers()
        {
            DataTable dt = ExecuteDataTable(
                "SELECT customerID, customerName FROM tblCustomer ORDER BY customerName"
            );

            dt.Columns.Add("DisplayText", typeof(string));

            foreach (DataRow row in dt.Rows)
                row["DisplayText"] = row["customerName"] + " - " + row["customerID"];

            ddlCustomer.DataSource = dt;
            ddlCustomer.DataTextField = "DisplayText";
            ddlCustomer.DataValueField = "customerID";
            ddlCustomer.DataBind();

            ddlCustomer.Items.Insert(0, new ListItem("-- Select Customer --", ""));
        }

        private DataTable ExecuteDataTable(string query, params SqlParameter[] parameters)
        {
            using (var conn = DatabaseHelper.OpenConnection())
            using (var cmd = new SqlCommand(query, conn))
            {
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                using (var reader = cmd.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return dt;
                }
            }
        }

    }
}
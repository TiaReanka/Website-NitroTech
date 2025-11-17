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
            string searchName = txtCustomerName.Text.Trim();

            if (string.IsNullOrWhiteSpace(searchName))
            {
                gvStatements.DataSource = null;
                gvStatements.DataBind();
                return;
            }

            using (SqlConnection sqlConnection = DatabaseHelper.OpenConnection())
            {
                string sql = @"
            SELECT s.statementNumber, c.customerName, s.statementDate, s.statementAmountDue
            FROM tblStatement s
            INNER JOIN tblCustomer c ON s.customerID = c.customerID
            WHERE c.customerName LIKE @SearchName
            ORDER BY c.customerName";

                using (SqlCommand cmd = new SqlCommand(sql, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@SearchName", "%" + searchName + "%");

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        gvStatements.DataSource = dt;
                        gvStatements.DataBind();
                    }
                }
            }
        }
    }
}